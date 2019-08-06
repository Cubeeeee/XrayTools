#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Xray.Tools.ExtractLib
* 项目描述 ：
* 类 名 称 ：OtherEncode
* 类 描 述 ：
* 命名空间 ：Xray.Tools.ExtractLib
* 机器名称 ：XXY-PC 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：XXY
* 创建时间 ：2019/6/11 10:15:23
* 更新时间 ：2019/6/11 10:15:23
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ XXY 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion
using System;
using System.Collections.Generic;
using System.Web;

namespace Xray.Tools.ExtractLib
{
    public class OtherEncode
    {
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetTimeSamp(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }
        public static String GetTimeSamp()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数
            return Convert.ToString(timeStamp);
        }
    }
}