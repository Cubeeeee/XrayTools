using System;
using System.Collections.Generic;
using System.Text;

namespace Xray.Tools.Microservices.Interface
{
    /// <summary>
    /// 服务发现接口
    /// </summary>
    public interface IServiceDescovery
    {
        IServiceInfo query { get; set; }
        void DescoveryInit(IServiceInfo query);
        IServiceConfig GetConfig(IServiceConfig  config);
        void ListenConfig();
        bool RegInstance();
        bool Beat();
        bool CreateService();
        IServiceInfo QueryService();
        IEnumerable<IServiceInfo> QueryServiceList();
    }
}
