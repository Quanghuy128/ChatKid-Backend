using ChatKid.Common.Constants;
using ChatKid.Common.Enum;
using Microsoft.VisualBasic;

namespace ChatKid.Common.Logger
{
    public class Logger<T> where T : class
    {
        private const string INFO = "INFO";
        private const string ERROR = "ERROR";
        private const string WARNING = "WARNING";
        private const string path = "./Logger/";
        private const string fileName = "Log-{0}.txt";

        public static async Task InformationAsync(params string[] message)
        {
            await WritelLogAsync(LogTypeEnum.INFO, message);
        }

        public static async Task ErrorAsync(params string[] message)
        {
            await WritelLogAsync(LogTypeEnum.ERROR, message);
        }

        public static async Task ErrorAsync(Exception ex, params string[] message)
        {
            await WritelLogAsync(LogTypeEnum.ERROR, string.Join("||", message),
                "Message: " + ex.Message,
                "Inner: " + (ex.InnerException != null ? ex.InnerException.Message : "No inner"),
                "StackTrace: " + (ex.StackTrace != null ? ex.StackTrace : "No StackTrace"),
                "Source: " + (ex.Source != null ? ex.Source : "No Source"));
        }

        public static async Task WarningAsync(params string[] message)
        {
            await WritelLogAsync(LogTypeEnum.WARNING, message);
        }

        private static async Task WritelLogAsync(LogTypeEnum logType, params string[] message)
        {
            try
            {
                DateTime datetime = DateTime.Now;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (StreamWriter fs = File.AppendText(string.Format(path + fileName, datetime.ToString(DateFormatConstants.DateFormatStandart))))
                {
                    var logContent = logType + "[" + datetime.ToString(DateFormatConstants.DateTimeFormat) + "]:: " + typeof(T).Name + ":: " + string.Join("||", message);
                    await fs.WriteLineAsync(logContent);
                }
            }
            catch (Exception ex)
            {
                await ErrorAsync(ex, message);
            }
        }

        public static void Information(params string[] message)
        {
            WritelLog(INFO, message);
        }

        public static void Error(params string[] message)
        {
            WritelLog(ERROR, message);
        }

        public static void Error(Exception ex, params string[] message)
        {
            WritelLog(ERROR, string.Join("||", message),
                "Message: " + ex.Message,
                "Inner: " + (ex.InnerException != null ? ex.InnerException.Message : "No inner"),
                "StackTrace: " + (ex.StackTrace != null ? ex.StackTrace : "No StackTrace"),
                "Source: " + (ex.Source != null ? ex.Source : "No Source"));
        }

        public static void Warning(params string[] message)
        {
            WritelLog(WARNING, message);
        }

        private static void WritelLog(string logType, params string[] message)
        {
            try
            {
                DateTime datetime = DateTime.Now;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (StreamWriter fs = File.AppendText(string.Format(path + fileName, datetime.ToString(DateFormatConstants.DateFormatStandart))))
                {
                    var logContent = logType + "[" + datetime.ToString(DateFormatConstants.DateTimeFormat) + "]:: " + typeof(T).Name + ":: " + string.Join("||", message) + "[ENDLOG]";
                    fs.WriteLine(logContent);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Thread.Sleep(5000);
                    WritelLog(logType, message);
                }
                catch
                {
                    Error(ex, message);
                }
            }
        }
    }
}
