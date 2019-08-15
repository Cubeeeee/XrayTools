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
using System.Security.Cryptography;
using System.Text;
using System.Web;

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

        public static T Encode<T>(EncodeType encodeType, T str, object parm)
        {
            var encodertype = typesdic[encodeType];
            if(encodertype == null)
            {
                return str;
            }
            return ((IEncoder<T>)Activator.CreateInstance(encodertype.MakeGenericType(typeof(T)))).Encode(str,parm);
        }

        #region UrlEncode

        #endregion

    

        #region TimeSamp
        public static long GetTimeSamp(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }

        public static DateTime GetDateTime(String date)
        {
            long jsTimeStamp = Convert.ToInt64(date);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddMilliseconds(jsTimeStamp);
            return dt;
        }
        public static DateTime GetDateTime(long date)
        {
            long jsTimeStamp = date;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddMilliseconds(jsTimeStamp);
            return dt;
        }
        #endregion

     
    }
}