using System.Net.Sockets;
using System.Net;
using ChatKid.Common.Extensions;

namespace ChatKid.Common.Providers
{
    public static class LocalIPAddressProvider
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public static string EncryptIPAddress()
        {
            var encryptObject = new
            {
                CreatedTime = DateTimeProvider.UtcNow(),
                IpAddress = GetLocalIPAddress().ToBase62(),
            };
            return encryptObject.ToString().ToBase62();
        }
    }
}
