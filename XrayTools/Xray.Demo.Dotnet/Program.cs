using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xray.Tools.ExtractLib.Encode;
using Xray.Tools.ExtractLib.Encode.Encoders;
using Xray.Tools.ExtractLib.Extract;
using Xray.Tools.ExtractLib.Extract.ExtractParms;
using Xray.Tools.HttpToolsLib;

namespace Xray.Demo.Dotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 1;
            //标准头
            HttpItem item = new HttpItem();
            item.URL = $"https://hz.meituan.com/meishi/pn{i}/";
            item.Postdata = "";
            item.UserAgent = "Mozilla/5.0 (Windows NT 6.3; rv:36.0) Gecko/20100101 Firefox/36.04";
            item.Referer = "";
            item.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            item.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            item.Cookie = "";
            item.ProxyIp = "";
            item.ProxyUserName = "";
            item.ProxyPwd = "";
            item.Method = "GET";
            //自定义头
            item.Header.Add("Accept-Encoding", "gzip, deflate");
            String html = HttpMethod.HttpWork(item).Html;
            //Console.WriteLine(html);
            var poiId1 = ExtractMethod.GetResults(ExtractType.Regex, html, "\"poiId\":(\\d+),", new RegexPam { Group = 1 });
            var poiId = RegexMethod.GetMutResult("\"poiId\":(\\d+),", html, 1);
            //Console.WriteLine(poiId1);

            Console.ReadLine();

        }
    }
}
