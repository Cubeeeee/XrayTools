using System;
using System.Collections.Generic;
using System.Text;

namespace Xray.Tools.Microservices.ServiceManager.Nacos
{
    public class NacosAPIEntity
    {
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public string Method { get; internal set; }
        public string Url { get; internal set; }
        public string Parms { get; internal set; }
        public string Body { get; internal set; }
    }
}
