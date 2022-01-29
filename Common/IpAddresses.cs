using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Common
{
    public class IpAddresses
    {

        public static string GetHostName()
        {
            string ip = null;

            var localAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var item in localAddresses)
            {
                if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ip = item.ToString();
                }
            }

            return ip;
        }
    }
}
