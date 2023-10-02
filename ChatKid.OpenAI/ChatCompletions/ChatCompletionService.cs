using ChatKid.Common.CommandResult;
using ChatKid.Common.Logger;
using ChatKid.OpenAI.Settings;
using Newtonsoft.Json.Linq;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.Net;

namespace ChatKid.OpenAI.ChatCompletions
{
    public class ChatCompletionService : IChatCompletionService
    {
        private readonly OpenAISettings _settings;
        private readonly BotTrainingConfig _config;
        private IOpenAIAPI api;
        public ChatCompletionService(OpenAISettings settings, BotTrainingConfig config) {  
            _settings = settings; 
            _config = config;

            api = new OpenAI_API.OpenAIAPI(_settings.Key);
        }
        public async Task<CommandResult> GetChatCompletion(string message)
        {
            string result = string.Empty;
            try
            {
                var chatRequest = new ChatRequest()
                {
                    Model = Model.ChatGPTTurbo,
                    MaxTokens = 200,
                    Temperature = 0.1,
                    Messages = new[] {
                        new ChatMessage(ChatMessageRole.System, _config.Default),
                        new ChatMessage(ChatMessageRole.User, message)
                    }
                };

                ChatResult chatResult = await api.Chat.CreateChatCompletionAsync(chatRequest);
                result = chatResult.Choices[0].Message.Content;
            }
            catch(Exception ex)
            {
                Logger<ChatCompletionService>.Error(ex, "Data: " + message);
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = "GetChatCompletion - " + ex,
                });
            }
                
            return CommandResult.SuccessWithData(result);
        }

        public async Task<CommandResult> GetChatStreamCompletion(string message)
        {
            IAsyncEnumerable<ChatResult> result = null;
            try
            {
                var chatRequest = new ChatRequest()
                {
                    Model = Model.ChatGPTTurbo,
                    MaxTokens = 200,
                    Temperature = 0.1,
                    Messages = new[] {
                        new ChatMessage(ChatMessageRole.System, _config.Default),
                        new ChatMessage(ChatMessageRole.User, message)
                    }
                };

                result = api.Chat.StreamChatEnumerableAsync(chatRequest);
            }
            catch (Exception ex)
            {
                Logger<ChatCompletionService>.Error(ex, "Data: " + message);
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = "GetChatStreamCompletion - " + ex,
                });
            }

            return CommandResult.SuccessWithData(result);
        }
    }
}
