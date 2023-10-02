using System.Linq;
using System.Net;

namespace ChatKid.Common.CommandResult
{
    public static class CommandResultExtensions
    {

        public static int GetStatusCode(this CommandResult commandResult)
        {
            if (commandResult.StatusCode.HasValue)
            {
                return (int)commandResult.StatusCode;
            }
            return commandResult.Succeeded ? (int)HttpStatusCode.OK : commandResult.Errors.FirstOrDefault().Code;
        }

        public static CommandResult WithCustomStatusCode(this CommandResult commandResult, HttpStatusCode statusCode)
        {
            commandResult.StatusCode = statusCode;
            return commandResult;
        }

        public static object GetData(this CommandResult commandResult)
        {
            return commandResult.Succeeded ? commandResult.Data : commandResult.Errors.FirstOrDefault()?.Description;
        }

        public static int? GetFirstErrorCode(this CommandResult commandResult)
        {
            return commandResult.Errors.FirstOrDefault()?.Code;
        }

        public static string GetFirstErrorMessage(this CommandResult commandResult)
        {
            return commandResult.Errors.FirstOrDefault()?.Description;
        }
    }
}
