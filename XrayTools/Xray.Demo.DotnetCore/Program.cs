using System;
using System.Collections.Generic;
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
    /// demo
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            String filepath = "tyc-num.woff";
            WebClient client = new WebClient();
            String html = Encoding.UTF8.GetString(client.UploadFile("https://convertio.co/process/upload_metadata", filepath));
            Console.WriteLine(html);
            Console.WriteLine(client.ResponseHeaders["Set-Cookie"]);
            
            Console.ReadKey();
        }
    }
}
