using ChatKid.Common.Constants;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatKid.ApiFramework.Authentication
{
    public class AuthenticationSettings
    {
        public const string AppSettingsSection = "AuthenticationSettings:Server";
        public string Key { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; } = AuthenticationConstants.EXPIRY_MINUTES;
    }
}
