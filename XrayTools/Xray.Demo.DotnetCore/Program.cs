using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
            ////使用正则表达式匹配多条结果 返回123,42
            //var regex_result =  ExtractMethod.GetResults(ExtractType.Regex,"xray123cube42","\\d+");
            ////使用jsonpath匹配单条结果 返回 1
            //var json_result = ExtractMethod.GetResults(ExtractType.Json,"{\"code\":1,\"message\":\"ok\"}","code");

            ////url编码 连续两次编码 输出大写  返回%25E5%25B7%25A5%25E5%2585%25B7%25E5%25BA%2593
            //var result = EncodeMethod.Encode( EncodeType.UrlEncode,"工具库",new UrlEncodeParm {  time = 2, upper = true});
            //Console.WriteLine(result);

            //String filepath = "tyc-num.woff";
            //WebClient client = new WebClient();
            //String html = Encoding.UTF8.GetString(client.UploadFile("https://convertio.co/process/upload_metadata", filepath));
            //Console.WriteLine(html);
            //Console.WriteLine(client.ResponseHeaders["Set-Cookie"]);
            //ExtractMethod.GetResult( ExtractType.Regex, "正则/XPATH/JPATH", "inputstr");
            //EncodeMethod.Encode( EncodeType.UrlEncode,"inputstr");
            Console.ReadKey();
        }


        public static byte[] Hex2Byte(string byteStr)
        {
            try
            {
                byteStr = byteStr.ToUpper().Replace(" ", "");
                int len = byteStr.Length / 2;
                byte[] data = new byte[len];
                for (int i = 0; i < len; i++)
                {
                    data[i] = Convert.ToByte(byteStr.Substring(i * 2, 2), 16);

                }
                return data;
            }
            catch (Exception ex)
            {
                return new byte[] { };
            }
        }
        private static String GetRandom()
        {
            Random r = new Random();
            String result = String.Empty;
            const String text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            for(int i = 0;i<16;i++)
            {
                result += text[r.Next(0, text.Length)];
            }
            return result;
        }
    }
}
