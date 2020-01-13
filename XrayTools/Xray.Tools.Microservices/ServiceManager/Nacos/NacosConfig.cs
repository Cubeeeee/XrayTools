using System;
using System.Collections.Generic;
using System.Text;
using Xray.Tools.Microservices.Interface;

namespace Xray.Tools.Microservices.ServiceManager.Nacos
{
    public class NacosConfig : IServiceConfig
    {
        public string dataId { get ; set ; }
        public string group { get ; set ; }
        public string configstr { get ; set ; }
    }
}
