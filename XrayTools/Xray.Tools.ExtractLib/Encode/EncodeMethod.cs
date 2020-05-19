#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Xray.Tools.ExtractLib.Encode
* 项目描述 ：
* 类 名 称 ：EncodeMethod
* 类 描 述 ：
* 命名空间 ：Xray.Tools.ExtractLib.Encode
* 机器名称 ：XXY-PC 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：XXY
* 创建时间 ：2019/4/10 16:31:18
* 更新时间 ：2019/4/10 16:31:18
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ XXY 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xray.Tools.ExtractLib.Encode.Encoders;
using Xray.Tools.ExtractLib.Interfaces;

namespace Xray.Tools.ExtractLib.Encode
{
    public class EncodeMethod
    {
        static Dictionary<EncodeType, Type> typesdic = new Dictionary<EncodeType, Type>();
        static EncodeMethod()
        {
            var types = Assembly.GetExecutingAssembly().DefinedTypes.Where(dt=> dt.IsClass&&dt.IsGenericTypeDefinition && Array.Exists(dt.GetInterfaces(), t => t.GetGenericTypeDefinition() == typeof(IEncoder<>)));
            
            types?.ToList().ForEach(item=> {
                EncodeType et = (item.GetCustomAttribute(typeof(EncodeTypeAttribute)) as EncodeTypeAttribute).encodetype;
                if(et != EncodeType.None&& !typesdic.ContainsKey(et))
                {
                    typesdic.Add(et,item);
                }
            });
        }
        /// <summary>
        /// 编码操作
        /// </summary>
        /// <typeparam name="T">输入数据类型</typeparam>
        /// <param name="encodeType">指定的编码方式</param>
        /// <param name="str">输入的待编码变量 可以是object int String等 最终都会通过Convert.ToString转化为字符串</param>
        /// <param name="parm">非通用字段 例如编码结果大小写 连续编码次数等</param>
        /// <returns>返回编码后的结果</returns>
        public static T Encode<T>(EncodeType encodeType, T str, object parm = null)
        {
            var encodertype = typesdic[encodeType];
            if(encodertype == null)
            {
                return str;
            }
            if(parm == null)
            {
                parm = CreateNewParm(encodeType);
            }
            return ((IEncoder<T>)Activator.CreateInstance(encodertype.MakeGenericType(typeof(T)))).Encode(str,parm);
        }

        private static object CreateNewParm(EncodeType encodeType)
        {
            object parm = new object();
            switch (encodeType)
            {
                case EncodeType.RsaEncode:
                case EncodeType.RsaDecode:
                    break;
                case EncodeType.HtmlEncode:
                case EncodeType.HtmlDecode:
                    parm = 0;
                    break;
                case EncodeType.UrlEncode:
                case EncodeType.UrlDecode:
                    parm = new UrlEncodeParm();
                    break;
                case EncodeType.UnicodeEncode:
                case EncodeType.UnicodeDecode:
                    break;
                case EncodeType.Base64Encode:
                case EncodeType.Base64Decode:
                    parm = "utf-8";
                    break;
                case EncodeType.MD5Encode:
                    break;
            }
            return parm;
        }


        #region TimeSamp
        /// <summary>
        /// 时间戳方法 DateTime转long型时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetTimeSamp(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }
        /// <summary>
        /// 时间戳方法 String类型时间转long型时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(String date)
        {
            long jsTimeStamp = Convert.ToInt64(date);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddMilliseconds(jsTimeStamp);
            return dt;
        }
        /// <summary>
        /// 时间戳方法 long型时间戳转DateTime时间
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(long date)
        {
            long jsTimeStamp = date;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddMilliseconds(jsTimeStamp);
            return dt;
        }
        /// <summary>
        /// 时间戳方法 long型时间戳转String时间
        /// </summary>
        /// <returns></returns>
        public static String GetTimeSamp()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数
            return Convert.ToString(timeStamp);
        }
        #endregion


    }
}