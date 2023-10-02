using ChatKid.Common.CommandResult;
using OpenAI_API.Images;

namespace ChatKid.OpenAI.ImageGeneration
{
    public interface IImageGenerateService
    {
        Task<CommandResult> GenerateImage(string prompt, int n, int size);
    }
}
