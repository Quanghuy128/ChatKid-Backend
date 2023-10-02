using ChatKid.Common.CommandResult;
using ChatKid.Common.Logger;
using ChatKid.OpenAI.ChatCompletions;
using ChatKid.OpenAI.Settings;
using OpenAI_API;
using OpenAI_API.Images;
using System.Net;
using System.Runtime;

namespace ChatKid.OpenAI.ImageGeneration
{
    public class ImageGenerateService : IImageGenerateService
    {
        private OpenAISettings _settings;
        private IOpenAIAPI api;
        public ImageGenerateService(OpenAISettings settings)
        {
            _settings = settings;
            api = new OpenAI_API.OpenAIAPI(_settings.Key);
        }
        public async Task<CommandResult> GenerateImage(string prompt, int n, int size)
        {
            ImageResult result = null;
            ImageSize imageSize = GetImageSize(size);
            try
            {
                var imageGenerationRequest = new ImageGenerationRequest(prompt, n, imageSize);
                result = await api.ImageGenerations.CreateImageAsync(imageGenerationRequest);

            }catch (Exception ex)
            {
                Logger<ImageGenerateService>.Error(ex, "Data:" + prompt + " - " + n + " - " + size);
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = "GenerateImage - " + ex,
                });
            }

            return CommandResult.SuccessWithData(result);
        }
        private ImageSize GetImageSize(int size)
        {
            if (size == 256) return ImageSize._256;
            else if(size == 512) return ImageSize._512;
            else return ImageSize._1024;
        }
    }
}
