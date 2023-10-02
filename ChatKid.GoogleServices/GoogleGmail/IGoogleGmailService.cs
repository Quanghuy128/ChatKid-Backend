
using Google.Apis.Gmail.v1.Data;

namespace ChatKid.GoogleServices.GoogleGmail
{
    public interface IGoogleGmailService
    {
        Task<Message> VerifyEmail(string email);
    }
}
