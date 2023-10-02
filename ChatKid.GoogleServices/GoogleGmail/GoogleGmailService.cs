using ChatKid.Common.Extensions;
using ChatKid.GoogleServices.GoogleSettings;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Net;

namespace ChatKid.GoogleServices.GoogleGmail
{
    public class GoogleGmailService : IGoogleGmailService
    {
        private readonly GoogleConfigSettings _settings;
        public GoogleGmailService(GoogleConfigSettings settings)
        {
            _settings = settings;
        }
        public async Task<Message> VerifyEmail(string email)
        {
            UserCredential credentials;
            using (var stream = new FileStream(_settings.ClientSecretFile, FileMode.Open, FileAccess.Read))
            {
                credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] {
                        GmailService.Scope.GmailSend
                    },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("KidTalkie.Gmail")).Result;
            }

            var gmailService = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = "Kidtalkie.Gmail"
            });

            var message = new Message()
            {
                Raw = EncodingExtensions.Base64UrlEncode($"<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" bgcolor=\"#f7f7f7\"> <tr> <td align=\"center\"> <table width=\"600\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"border-collapse: collapse;\"> <tr> <td align=\"center\" bgcolor=\"#ffffff\" style=\"padding: 20px;\"> <h1>Verification Code</h1> <p>Your verification code is:</p> <div style=\"background-color: #f2f2f2; padding: 10px; font-size: 24px; font-weight: bold; text-align: center;\"> <!-- Replace this with the actual verification code --> 1234 </div> <p>Please use the above code to verify your account.</p> </td> </tr> </table> </td> </tr> </table>")
            };

            Message response = gmailService.Users.Messages.Send(message, email).Execute();

            return response;
        }

        
    }
}
