using System;
using System.IO;
using System.Text;
using Xray.Tools.File;
using Xray.Tools.HttpToolsLib;

namespace Xray.Demo.DotnetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            FileMethod.CopyDIr("d0", "dd");
            //    var files = Directory.GetFiles("d0", "*.dll", SearchOption.AllDirectories);
            //Console.WriteLine(String.Join("\n", FileMethod.GetFiles("d0")));
            //Console.WriteLine("------------------------------------------------------");
            //Console.WriteLine(String.Join("\n", files)); 
            Console.WriteLine(  123);
            Console.ReadLine();
        }
    }
}
