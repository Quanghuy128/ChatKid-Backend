using ChatKid.ApiFramework;
using ChatKid.DataLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using ChatKid.Application;
using ChatKid.OpenAI;
using ChatKid.GoogleServices;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using ChatKid.Common.Validation;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;
using ChatKid.Notify;
using ChatKid.Notification.Notification;
using CorePush.Apple;
using CorePush.Google;
using ChatKid.Notification.FcmNotificationConfig;
using ChatKid.Common.CommandResult;

namespace ChatKid.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            Environment = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets(typeof(Startup).Assembly, true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor()
                    .AddOptions()
                    .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                    .RegisterAuthentication(Configuration)
                    .AddResponseCaching()
                    .AddAuthorization()
                    .AddHealthChecks();

            //register application services
            services.RegisterApiService()
                .RegisterDataLayer()
                .RegisterServices()
                .RegisterOpenAI(Configuration)
                .RegisterGoogleService(Configuration);
                //.RegisterFcmService(Configuration);

            //*** Add by hand
            services.AddTransient<INotificationService, NotificationService>();
            services.AddHttpClient<FcmSender>();
            services.AddHttpClient<ApnSender>();
            // Configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("FcmNotification");
            services.Configure<FcmNotificationSetting>(appSettingsSection);
            //*****



            var dbConnectionStrings = Configuration.GetSection("ConnectionStrings");
            services.AddDbContext<ApplicationDBContext>(opts => opts.UseNpgsql(dbConnectionStrings["ChatKidDB"], dbo => dbo.EnableRetryOnFailure()));

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddControllers().AddJsonOptions(x =>
            {
                // serialize enums as strings in api responses (e.g. Role)
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                // ignore omitted parameters on models to enable optional params (e.g. User update)
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            }).AddControllersAsServices();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = $"Please follow format {JwtBearerDefaults.AuthenticationScheme} + JWT Token to access unauthorized apis",
                    Name = HeaderNames.Authorization,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "oauth2",
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseExceptionHandler(appError =>
            {
                appError.Run(HandleException);
            });


            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private async Task HandleException(HttpContext context)
        {
            var feature = context.Features.Get<IExceptionHandlerPathFeature>();
            var json = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>();

            if (feature?.Error == null)
            {
                return;
            }

            switch (feature.Error)
            {
                case FluentValidation.ValidationException validateException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.Headers.ContentType = "application/json";
                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(validateException.Format(), json?.Value.SerializerOptions));
                    break;
                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync(string.Empty);
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsync(new CommandResultError()
                    {
                        Code = context.Response.StatusCode,
                        Description = feature.Error.Message
                    }.ToString());
                    break;
            }
        }
    }
}
