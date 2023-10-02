using ChatKid.ApiFramework.AuthTokenIssuer;

namespace ChatKid.Api
{
    public static class Registrations
    {
        public static IServiceCollection RegisterApiService(this IServiceCollection services)
        {
            services.AddSingleton<ITokenIssuer, TokenIssuer>();

            return services;
        }
    }
}
