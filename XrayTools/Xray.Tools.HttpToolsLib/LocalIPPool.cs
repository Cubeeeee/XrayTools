using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xray.Tools.HttpToolsLib
{
    /// <summary>
    /// 本地缓存代理IP池
    /// </summary>
    public class LocalIPPool
    {
        static bool Runing = false;
        static String api = "";
        static ConcurrentQueue<string> _Ipqueue { get; set; } = new ConcurrentQueue<string>();
        static ConcurrentQueue<string> _checkqueue { get; set; } = new ConcurrentQueue<string>();

        public static void Start(String apiurl)
        {
            Console.WriteLine("IP池初始化成功");
            if (Runing)
            {
                return;
            }
            Runing = true;
            api = apiurl;
            Task.Factory.StartNew(IPFunc);
        }
        private static void IPFunc()
        {
            while (true)
            {
                if (_Ipqueue.IsEmpty)
                {
                    _Ipqueue = Xray.Tools.HttpToolsLib.HttpMethod.InputApi(api);
                }
                Thread.Sleep(100);
            }
        }

        public static String PopIP()
        {
            if (!Runing)
            {
                return String.Empty;
            }

            String ip = String.Empty;
            if (_checkqueue.TryDequeue(out ip))
            {
                return ip;
            }
            else
            {
                while (!_Ipqueue.TryDequeue(out ip))
                {
                    Thread.Sleep(100);
                }
            }
            return ip;
        }

        public static void PushIP(String ip)
        {
            if (!Runing)
            {
                return;
            }
            if (!String.IsNullOrEmpty(ip))
                _checkqueue.Enqueue(ip);
        }
    }
}
