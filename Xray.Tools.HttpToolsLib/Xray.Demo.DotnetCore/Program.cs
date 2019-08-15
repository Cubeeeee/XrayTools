using System;
using Xray.Tools.HttpToolsLib;

namespace Xray.Demo.DotnetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(HttpMethod.FastMethod_HttpHelper("https://blog.csdn.net/qq_26712977"));
            Console.ReadKey();
        }
    }
}
