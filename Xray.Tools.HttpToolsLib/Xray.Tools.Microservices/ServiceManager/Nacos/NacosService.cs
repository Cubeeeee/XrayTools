using System;
using System.Collections.Generic;
using System.Text;
using Xray.Tools.Microservices.Interface;

namespace Xray.Tools.Microservices.ServiceManager.Nacos
{
    public class NacosService : IServiceInfo
    {
        public string Name { get; set; }
        public string NameSpaceId { get ; set ; }
        public string ip { get ; set ; }
        public int port { get ; set ; }
        public string clustername { get ; set ; }
        public int weight { get; set; } = 1;
    }
}
