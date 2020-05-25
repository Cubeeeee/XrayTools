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
        /// <summary>
        /// 枚举->类型字典
        /// </summary>
        static Dictionary<ExtractType, Type> typesdic { get; set; } = new Dictionary<ExtractType, Type>();
        /// <summary>
        /// 静态类进行反射装载
        /// </summary>
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
        /// <summary>
        /// 校验
        /// </summary>
        /// <typeparam name="T">BaseParm的继承类 用于配置非通用化的参数</typeparam>
        /// <param name="extractType">匹配方式 枚举值</param>
        /// <param name="Txt">待匹配的文本</param>
        /// <param name="path">正则、Xpath、Jpath表达式</param>
        /// <param name="parm">非通用化参数  例如正则的分组、Xpath的属性、需要校验的值等</param>
        /// <returns>校验结果</returns>
        public static bool Check<T>(ExtractType extractType, string Txt, string path, T parm ) where T : BaseParm
        {
            var et = typesdic[extractType];
            if (et != null)
            {
                return ((IExtract<T>)Activator.CreateInstance(et.MakeGenericType(typeof(T)))).Check(Txt, path,parm);
            }
            return false; ;
        }
        /// <summary>
        /// 匹配返回第一条数据
        /// </summary>
        /// <typeparam name="T">BaseParm的继承类 用于配置非通用化的参数</typeparam>
        /// <param name="extractType">匹配方式 枚举值</param>
        /// <param name="Txt">待匹配的文本</param>
        /// <param name="path">正则、Xpath、Jpath表达式</param>
        /// <param name="parm">非通用化参数  例如正则的分组 Xpath的属性等</param>
        /// <returns>第一条匹配数据</returns>
        public static string GetResult<T>(ExtractType extractType,string Txt, string path, T parm) where T : BaseParm
        {
            var et = typesdic[extractType];
            if (et != null)
            {
                return ((IExtract<T>)Activator.CreateInstance(et.MakeGenericType(typeof(T)))).GetResult(Txt, path, parm);
            }
            return String.Empty; ;
        }
        /// <summary>
        /// 返回所有匹配的数据
        /// </summary>
        /// <typeparam name="T">BaseParm的继承类 用于配置非通用化的参数</typeparam>
        /// <param name="extractType">匹配方式 枚举值</param>
        /// <param name="Txt">待匹配的文本</param>
        /// <param name="path">正则、Xpath、Jpath表达式</param>
        /// <param name="parm">非通用化参数  例如正则的分组 Xpath的属性等</param>
        /// <returns>所有匹配的数据</returns>
        public static List<string> GetResults<T>(ExtractType extractType, string Txt, string path, T parm) where T : BaseParm
        {
            var et = typesdic[extractType];
            if (et != null)
            {
                return ((IExtract<T>)Activator.CreateInstance(et.MakeGenericType(typeof(T)))).GetResults(Txt, path, parm);
            }
            return new List<String>();
        }
        /// <summary>
        /// 替换符合匹配条件的数据 目前只支持正则
        /// </summary>
        /// <param name="extractType">匹配方式 枚举值</param>
        /// <param name="Txt">待匹配的文本</param>
        /// <param name="path">正则表达式</param>
        /// <param name="RepStr">替换的文本</param>
        /// <returns>替换后的字符串</returns>
        public static string Replace(string Txt, string path, string RepStr, ExtractType extractType = ExtractType.Regex)
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
        /// <summary>
        /// 使用正则表达式进行分割
        /// </summary>
        /// <param name="extractType">匹配方式 枚举值</param>
        /// <param name="Txt">待匹配的文本</param>
        /// <param name="path">正则表达式</param>
        /// <returns>分割后的字符串集合</returns>
        public static List<string> Split(string Txt, string path, ExtractType extractType = ExtractType.Regex)
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

        /// <summary>
        /// 校验 不带扩展参数
        /// </summary>
        /// <param name="extractType">匹配方式 枚举值</param>
        /// <param name="Txt">待匹配的文本</param>
        /// <param name="path">正则、Xpath、Jpath表达式</param>
        /// <returns>校验结果</returns>
        public static bool Check(ExtractType extractType, string Txt, string path)
        {
            var et = typesdic[extractType];
            if (et != null)
            {
                return ((IExtract<BaseParm>)Activator.CreateInstance(et.MakeGenericType(typeof(BaseParm)))).Check(Txt, path, null);
            }
            return false; ;
        }
        /// <summary>
        /// 匹配第一条 不带扩展参数
        /// </summary>
        /// <param name="extractType">匹配方式 枚举值</param>
        /// <param name="Txt">待匹配的文本</param>
        /// <param name="path">正则、Xpath、Jpath表达式</param>
        /// <returns匹配到的第一条数据></returns>
        public static string GetResult(ExtractType extractType, string Txt, string path)
        {
            var et = typesdic[extractType];
            if (et != null)
            {
                return ((IExtract<BaseParm>)Activator.CreateInstance(et.MakeGenericType(typeof(BaseParm)))).GetResult(Txt, path, null);
            }
            return String.Empty; ;
        }
        /// <summary>
        /// 匹配多条 不带扩展参数
        /// </summary>
        /// <param name="extractType">匹配方式 枚举值</param>
        /// <param name="Txt">待匹配的文本</param>
        /// <param name="path">正则、Xpath、Jpath表达式</param>
        /// <returns>匹配到的数据集合</returns>
        public static List<string> GetResults(ExtractType extractType, string Txt, string path) 
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
