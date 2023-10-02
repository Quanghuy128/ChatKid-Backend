namespace ChatKid.ApiFramework.Authentication
{
    public class TrustedIssuerSettings
    {
        public const string AppSettingsSection = "AuthenticationSettings:TrustedIssuers";
        public string Issuer { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
