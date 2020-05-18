using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Xray.Tools.ExtractLib.Encode;
using Xray.Tools.ExtractLib.Encode.Encoders;
using Xray.Tools.ExtractLib.Extract;
using Xray.Tools.ExtractLib.Extract.ExtractParms;
using Xray.Tools.HttpToolsLib;

namespace Xray.Demo.DotnetCore
{
    /// <summary>
    /// 测试Dem
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            GetParmTest();
            //String filepath = "tyc-num.woff";
            //WebClient client = new WebClient();
            //String html = Encoding.UTF8.GetString(client.UploadFile("https://convertio.co/process/upload_metadata", filepath));
            //Console.WriteLine(html);
            //Console.WriteLine(client.ResponseHeaders["Set-Cookie"]);
            
            Console.ReadKey();
        }

        private static void GetParmTest()
        {
            var dic = HttpMethod.GetParms("https://cn.bing.com/search?q=CSDN%E5%9B%BE%E6%A0%87&qs=n&form=QBRE&sp=-1&pq=csdntu%27b&sc=0-8&sk=&cvid=4D2266ACBA1548FF989E909540F7FB20", "pageId=38e0eb52e3eca460cc31b98a77fcce2e&s21=泰隆");
            Console.WriteLine(String.Join("\n", from a in dic.Keys  select $"{a}={dic[a]}"));
        }
    }
}
