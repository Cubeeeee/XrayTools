using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Xray.Tools.ExtractLib.Encode;
using Xray.Tools.File;
using Xray.Tools.HttpToolsLib;

namespace Xray.Demo.DotnetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(EncodeMethod.Encode(Tools.ExtractLib.EncodeType.MD5Encode, "123", null));
            Console.ReadKey();
        }
    }
}
