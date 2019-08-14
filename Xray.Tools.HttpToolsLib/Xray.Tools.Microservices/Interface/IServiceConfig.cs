using System;
using System.Collections.Generic;
using System.Text;

namespace Xray.Tools.Microservices.Interface
{
    public interface IServiceConfig
    {
        string dataId { get; set; }
        string group { get; set; }
        string configstr { get; set; }
    }
}
