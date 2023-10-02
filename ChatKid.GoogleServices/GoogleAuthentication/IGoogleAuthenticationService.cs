using ChatKid.Common.CommandResult;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2.Data;

namespace ChatKid.GoogleServices.GoogleAuthentication
{
    public interface IGoogleAuthenticationService
    {
        Task<CommandResult> GoogleLogin(string token);
    }
}
