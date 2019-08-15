using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xray.Tools.ExtractLib.Extract.ExtractParms;
using Xray.Tools.ExtractLib.Interfaces;

namespace Xray.Tools.ExtractLib.Extract
{
    public class ExtractMethod
    {
        static Dictionary<ExtractType, Type> typesdic = new Dictionary<ExtractType, Type>();
        static ExtractMethod()
        {
            var types = Assembly.GetExecutingAssembly().DefinedTypes.Where(dt => dt.IsClass && dt.IsGenericTypeDefinition && Array.Exists(dt.GetInterfaces(), t => t.GetGenericTypeDefinition() == typeof(IExtract<>)));

            types?.ToList().ForEach(item => {
                ExtractType et = (item.GetCustomAttribute(typeof(ExtractTypeAttribute)) as ExtractTypeAttribute).extracttype;
                if (et != ExtractType.None && !typesdic.ContainsKey(et))
                {
                    typesdic.Add(et, item);
                }
            });
        }

        public static bool Check<T>(ExtractType extractType, string Txt, string path, T parm ) where T : BaseParm
        {
            var et = typesdic[extractType];
            if (et != null)
            {
                return ((IExtract<T>)Activator.CreateInstance(et.MakeGenericType(typeof(T)))).Check(Txt, path,parm);
            }
            return false; ;
        }

        public static string GetResult<T>(ExtractType extractType,string Txt, string path, T parm) where T : BaseParm
        {
            var et = typesdic[extractType];
            if (et != null)
            {
                return ((IExtract<T>)Activator.CreateInstance(et.MakeGenericType(typeof(T)))).GetResult(Txt, path, parm);
            }
            return String.Empty; ;
        }

        public static IEnumerable<string> GetResults<T>(ExtractType extractType, string Txt, string path, T parm) where T : BaseParm
        {
            var et = typesdic[extractType];
            if (et != null)
            {
                return ((IExtract<T>)Activator.CreateInstance(et.MakeGenericType(typeof(T)))).GetResults(Txt, path, parm);
            }
            return new List<String>();
        }

        public static string Replace(ExtractType extractType,string Txt, string path, string RepStr)
        {
            if(extractType == ExtractType.Regex)
            {
                var et = typesdic[extractType];
                if (et != null)
                {
                    return ((IExtract<BaseParm>)Activator.CreateInstance(et.MakeGenericType(typeof(BaseParm)))).Replace(Txt, path, RepStr);
                }
            }
            return Txt;
        }

        public static IEnumerable<string> Split(ExtractType extractType,string Txt, string path)
        {
            List<String> list = new List<string>();
            if (extractType == ExtractType.Regex)
            {
                var et = typesdic[extractType];
                if (et != null)
                {
                    return ((IExtract<BaseParm>)Activator.CreateInstance(et.MakeGenericType(typeof(BaseParm)))).Split(Txt, path);
                }
            }
            return list;
        }


        public static bool Check(ExtractType extractType, string Txt, string path)
        {
            var et = typesdic[extractType];
            if (et != null)
            {
                return ((IExtract<BaseParm>)Activator.CreateInstance(et.MakeGenericType(typeof(BaseParm)))).Check(Txt, path, null);
            }
            return false; ;
        }

        public static string GetResult(ExtractType extractType, string Txt, string path)
        {
            var et = typesdic[extractType];
            if (et != null)
            {
                return ((IExtract<BaseParm>)Activator.CreateInstance(et.MakeGenericType(typeof(BaseParm)))).GetResult(Txt, path, null);
            }
            return String.Empty; ;
        }

        public static IEnumerable<string> GetResults(ExtractType extractType, string Txt, string path) 
        {
            var et = typesdic[extractType];
            if (et != null)
            {
                return ((IExtract<BaseParm>)Activator.CreateInstance(et.MakeGenericType(typeof(BaseParm)))).GetResults(Txt, path, null);
            }
            return new List<String>();
        }
    }
}
