namespace ChatKid.OpenAI.Settings
{
    public class OpenAISettings
    {
        public const string OpenAISection = "OpenAISettings:Common";
        public string Key { get; set; } = string.Empty;
        public string Model {  get; set; } = string.Empty;
        public string FileModel {  get; set; } = string.Empty;
    }
}
