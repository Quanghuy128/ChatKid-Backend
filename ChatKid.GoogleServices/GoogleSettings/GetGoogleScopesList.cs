using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;

namespace ChatKid.GoogleServices.GoogleSettings
{
    public static class GetGoogleScopesList
    {
        public static List<string> GetScopes()
        {
            return new List<string>
            {
                GmailService.Scope.GmailSend,
                Oauth2Service.Scope.UserinfoProfile,
                Oauth2Service.Scope.UserinfoEmail,
                "https://www.googleapis.com/auth/devstorage.full_control"
            };
        }
    }
}
