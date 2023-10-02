namespace ChatKid.OpenAI.Settings
{
    public class BotTrainingConfig
    {
        public const string BotTrainingConfigSection = "OpenAISettings:TrainingMessage";
        public string Default { get; set; } = string.Empty;
    }
}
