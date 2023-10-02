using ChatKid.OpenAI.Settings;
using OpenAI_API.Chat;
using System;

namespace ChatKid.OpenAI.ChatCompletions.BotDefaultTraining
{
    public static class BotSystemTraining
    {
        public static void DefaultTrain(this Conversation conversation, BotTrainingConfig config)
        {
            conversation.AppendSystemMessage(config.Default);
        }
    }
}
