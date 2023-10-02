using ChatKid.OpenAI.ChatCompletions;
using ChatKid.OpenAI.ImageGeneration;
using ChatKid.OpenAI.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatKid.OpenAI
{
    public static class Registrations
    {
        public static IServiceCollection RegisterOpenAI(this IServiceCollection services, IConfiguration configuration)
        {
            var openAISettings = configuration.GetSection(OpenAISettings.OpenAISection).Get<OpenAISettings>();
            services.AddSingleton(_ => openAISettings);

            var trainingConfig = configuration.GetSection(BotTrainingConfig.BotTrainingConfigSection).Get<BotTrainingConfig>();
            services.AddSingleton(_ => trainingConfig);

            services.AddScoped<IChatCompletionService, ChatCompletionService>();
            services.AddScoped<IImageGenerateService, ImageGenerateService>();
            return services;
        }
    }
}
