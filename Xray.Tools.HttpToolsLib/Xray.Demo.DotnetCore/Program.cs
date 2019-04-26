using System;
using System.Text;
using Xray.Tools.HttpToolsLib;

namespace Xray.Demo.DotnetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine(HttpMethod.HttpWork(new HttpInfo { URL = "http://2019.ip138.com/ic.asp",Encoding = Encoding.GetEncoding("gb2312") }).Html);
            Console.ReadLine();
        }
    }
}
