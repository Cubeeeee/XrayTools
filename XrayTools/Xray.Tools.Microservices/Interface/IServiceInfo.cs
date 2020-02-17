using System;
using System.Collections.Generic;
using System.Text;

namespace Xray.Tools.Microservices.Interface
{
    /// <summary>
    /// 返回用
    /// </summary>
    public interface IServiceInfo
    {
        string Name { get; set; }
        string NameSpaceId { get; set; }
        string ip { get; set; }
        int port { get; set; }
        string clustername { get; set; }
        int weight { get; set; }
    }
}
