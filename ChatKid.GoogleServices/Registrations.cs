using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.Extensions.Configuration;
using ChatKid.GoogleServices.GoogleSettings;
using ChatKid.GoogleServices.GoogleAuthentication;
using ChatKid.GoogleServices.GoogleGmail;
using ChatKid.GoogleServices.GoogleCloudStorage;

namespace ChatKid.GoogleServices
{
    public static class Registrations
    {
        public static IServiceCollection RegisterGoogleService(this IServiceCollection services, IConfiguration configuration)
        {
            var _googleSettings = configuration.GetSection(GoogleConfigSettings.GoogleConfigSection).Get<GoogleConfigSettings>();
            services.AddSingleton(_ => _googleSettings);

            services
             .AddAuthentication(o =>
             {
                 // This forces challenge results to be handled by Google OpenID Handler, so there's no
                 // need to add an AccountController that emits challenges for Login.
                 o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                 // This forces forbid results to be handled by Google OpenID Handler, which checks if
                 // extra scopes are required and does automatic incremental auth.
                 o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                 // Default scheme that will handle everything else.
                 // Once a user is authenticated, the OAuth2 token info is stored in cookies.
                 o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
             })
             .AddCookie()
             .AddGoogleOpenIdConnect(options =>
             {
                 options.ClientId = _googleSettings.ClientId;
                 options.ClientSecret = _googleSettings.ClientSecret;
             });

            services.AddSingleton<GoogleResponse>();
            services.AddScoped<IGoogleAuthenticationService, GoogleAuthenticationService>();
            services.AddScoped<IGoogleGmailService, GoogleGmailService>();
            services.AddSingleton<ICloudStorageService, CloudStorageService>();
            return services;
        }
    }
}
