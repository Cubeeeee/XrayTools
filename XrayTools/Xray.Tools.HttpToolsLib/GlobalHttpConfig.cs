#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Xray.Tools.HttpToolsLib
* 项目描述 ：
* 类 名 称 ：GlobalHttpConfig
* 类 描 述 ：
* 命名空间 ：Xray.Tools.HttpToolsLib
* 机器名称 ：XXY-PC 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：XXY
* 创建时间 ：2019/4/9 16:41:32
* 更新时间 ：2019/4/9 16:41:32
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ XXY 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using System.Net;

namespace Xray.Tools.HttpToolsLib
{
    /// <summary>
    /// 全局Http请求配置
    /// </summary>
    public class GlobalHttpConfig
    {
        public static void UpdateConfig()
        {
            ServicePointManager.DefaultConnectionLimit = DefaultConnectionLimit;
        }
        public static int DefaultConnectionLimit { get; set; } = short.MaxValue;
    }
}