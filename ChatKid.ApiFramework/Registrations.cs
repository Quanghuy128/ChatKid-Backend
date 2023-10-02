using ChatKid.ApiFramework.Authentication;
using ChatKid.ApiFramework.AuthJwtIssuer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatKid.ApiFramework
{
    public static class Registrations
    {
        public static IServiceCollection RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationSettings = configuration.GetSection(AuthenticationSettings.AppSettingsSection).Get<AuthenticationSettings>();
            services.AddSingleton(_ => authenticationSettings);

            var trustedIssuer = configuration.GetSection(TrustedIssuerSettings.AppSettingsSection).Get<TrustedIssuerSettings>();
            services.AddSingleton(_ => trustedIssuer);

            services.AddSingleton<IJwtTokenIssuer, JwtTokenIssuer>();

            _ = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = trustedIssuer.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authenticationSettings.Key))
                };
            });
            return services;
        }

    }
}
