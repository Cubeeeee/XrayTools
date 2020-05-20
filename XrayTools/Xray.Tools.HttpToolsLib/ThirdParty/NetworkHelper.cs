using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Xray.Tools.HttpToolsLib.ThirdParty
{
    public class NetworkHelper
    {
        /// <summary>
        /// 获得本地ip地址
        /// </summary>
        public static string LocalIPAddress
        {
            get
            {
                UnicastIPAddressInformation mostSuitableIp = null;
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (var network in networkInterfaces)
                {
                    if (network.OperationalStatus != OperationalStatus.Up)
                        continue;
                    var properties = network.GetIPProperties();
                    if (properties.GatewayAddresses.Count == 0)
                        continue;

                    foreach (var address in properties.UnicastAddresses)
                    {
                        if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                            continue;
                        if (IPAddress.IsLoopback(address.Address))
                            continue;
                        return address.Address.ToString();
                    }
                }

                return mostSuitableIp != null
                    ? mostSuitableIp.Address.ToString()
                    : "";
            }
        }
        /// <summary>
        /// 获得随机未被占用端口
        /// </summary>
        /// <param name="minPort"></param>
        /// <param name="maxPort"></param>
        /// <returns></returns>
        public static int GetRandomAvaliablePort(int minPort = 1024, int maxPort = 65535)
        {
            Random rand = new Random();
            while (true)
            {
                int port = rand.Next(minPort, maxPort);
                if (!IsPortInUsed(port))
                {
                    return port;
                }
            }
        }
        /// <summary>
        /// 判断端口是否被占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private static bool IsPortInUsed(int port)
        {
            IPGlobalProperties ipGlobalProps = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipsTCP = ipGlobalProps.GetActiveTcpListeners();

            if (ipsTCP.Any(p => p.Port == port))
            {
                return true;
            }

            IPEndPoint[] ipsUDP = ipGlobalProps.GetActiveUdpListeners();
            if (ipsUDP.Any(p => p.Port == port))
            {
                return true;
            }

            TcpConnectionInformation[] tcpConnInfos = ipGlobalProps.GetActiveTcpConnections();
            if (tcpConnInfos.Any(conn => conn.LocalEndPoint.Port == port))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 获得出口ip
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static String GetNetworkIP(HttpItem item)
        {
           return  Regex.Match( HttpMethod.HttpWork_Html(item), @"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}(:\d{1,5})?").Value;
        }
        /// <summary>
        /// 获得出口 指定校验网站
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static String GetNetworkIP(String url)
        {
            return Regex.Match(HttpMethod.FastMethod_HttpHelper(url), @"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}(:\d{1,5})?").Value;
        }
    }
}
