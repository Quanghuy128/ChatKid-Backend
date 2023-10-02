using ChatKid.GoogleServices.GoogleSettings;
using Google.Apis.Oauth2.v2.Data;
using Newtonsoft.Json.Linq;
using ChatKid.Common.CommandResult;
using System.Net;
using ChatKid.Common.Logger;
using Google.Apis.Auth.OAuth2;

namespace ChatKid.GoogleServices.GoogleAuthentication
{
    public class GoogleAuthenticationService : IGoogleAuthenticationService
    {
        private readonly GoogleConfigSettings _settings;
        private GoogleResponse _googleResponse;
        public GoogleAuthenticationService(GoogleConfigSettings settings, GoogleResponse googleResponse)
        {
            _settings = settings;
            _googleResponse = googleResponse;
        }
        public async Task<CommandResult> GoogleLogin(string token)
        {
            Userinfo userinfo = null;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync("https://www.googleapis.com/oauth2/v1/userinfo?access_token="+token);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        JObject userInfoJSON = JObject.Parse(json);
                        userinfo = new Userinfo()
                        {
                            Id = userInfoJSON["id"]!.ToString(),
                            Email = userInfoJSON["email"]!.ToString(),
                            Name = userInfoJSON["name"]!.ToString(),
                            GivenName = userInfoJSON["given_name"]!.ToString(),
                            FamilyName = userInfoJSON["family_name"]!.ToString(),
                            Picture = userInfoJSON["picture"]!.ToString(),
                            VerifiedEmail = Boolean.Parse(userInfoJSON["verified_email"]!.ToString()),
                        };
                    }
                    else
                    {
                        return CommandResult.Failed(new CommandResultError()
                        {
                            Code = (int)HttpStatusCode.NotFound,
                            Description = "User Not Found!!!"
                        });
                    }
                }
                catch (Exception ex)
                {
                    Logger<GoogleAuthenticationService>.Error(ex, "Data: " + token);
                    return CommandResult.Failed(new CommandResultError()
                    {
                        Code = (int)HttpStatusCode.InternalServerError,
                        Description = "GoogleLogin" + ex
                    });
                }
            }
            return CommandResult.SuccessWithData(userinfo);
        }
    }

}
// <summary>https://accounts.google.com/o/oauth2/revoke?token=ACCESS_TOKEN</summary> //revoke access token
// <summary>https://www.googleapis.com/oauth2/v1/userinfo?access_token=ACCESS_TOKEN</summary> //get user info from accesstoken
