using ChatKid.Common.CommandResult;
using OpenAI_API.Chat;

namespace ChatKid.OpenAI.ChatCompletions
{
    public interface IChatCompletionService
    {
        Task<CommandResult> GetChatCompletion(string message);
        Task<CommandResult> GetChatStreamCompletion(string message);
    }
}
