#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Xray.Tools.HttpToolsLib
* 项目描述 ：
* 类 名 称 ：HttpInfo
* 类 描 述 ：
* 命名空间 ：Xray.Tools.HttpToolsLib
* 机器名称 ：XXY-PC 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：XXY
* 创建时间 ：2019/4/9 16:37:14
* 更新时间 ：2019/4/9 16:37:14
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ XXY 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

namespace Xray.Tools.HttpToolsLib
{
    public class HttpInfo:HttpItem
    {
        /// <summary>
        /// Http版本类型
        /// </summary>
        public enum ProtocolVersionEnum { V10 = 10, V11 };
        public bool Expect100Continue { get; set; } = false;
        public bool AllowWriteStreamBuffering { get; set; } = false;
        public int MaximumAutomaticRedirections { get; set; } = 50;
        public bool IgnoreWebException { get; set; } = false;
        public bool KeepAlive { get; set; } = false;

    }
}