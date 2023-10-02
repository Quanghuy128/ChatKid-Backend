namespace ChatKid.GoogleServices.GoogleSettings
{
    public class GoogleConfigSettings
    {
        public const string GoogleConfigSection = "Authentication:Google";
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string ClientSecretFile {  get; set; } = string.Empty;
    }
}
