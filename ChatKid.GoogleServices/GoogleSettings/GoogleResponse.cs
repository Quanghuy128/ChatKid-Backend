using Google.Apis.Auth.OAuth2;

namespace ChatKid.GoogleServices.GoogleSettings
{
    public class GoogleResponse
    {
        public UserCredential Credential { get; set; }
        public string AccessToken { get; set; } = string.Empty;
    }
}
