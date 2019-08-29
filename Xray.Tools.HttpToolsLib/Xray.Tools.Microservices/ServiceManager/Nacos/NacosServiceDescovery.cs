using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xray.Tools.HttpToolsLib;
using Xray.Tools.Microservices.Interface;

namespace Xray.Tools.Microservices.ServiceManager.Nacos
{
    public class NacosServiceDescovery : IServiceDescovery
    {
        static HttpItem item { get; set; } = new HttpItem();
        IEnumerable<NacosAPIEntity> nacosAPIs { get; set; }
        static String configstr { get; set; } = String.Empty;
        static String nacoshost { get; set; } = String.Empty;
        static String routeaddress { get; set; } = String.Empty;
        static String routeinfo { get; set; } = String.Empty;
        public IServiceInfo query { get; set; }

        static NacosServiceDescovery()
        {
            configstr = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "nacosapi.json"), Encoding.UTF8);
        }
        public bool Beat()
        {
            NacosAPIEntity entity = GetNacosAPI("Beat");
            item = new HttpItem
            {
                URL = $"{nacoshost}{entity.Url}?{String.Format(entity.Parms, query.Name, JsonConvert.SerializeObject(new { cluster = query.clustername, ip = query.ip, port = query.port, scheduled = true, serviceName = query.Name, query.weight }),query.NameSpaceId)}",
                Method = entity.Method
            };
            String html = HttpMethod.HttpWork(item).Html;
            //Console.WriteLine($"Beat:{html}");
            return html.Contains("ok");
        }

        public bool CreateService()
        {
            NacosAPIEntity entity = GetNacosAPI("CreateService");
            item = new HttpItem
            {
                URL = $"{nacoshost}{entity.Url}?{String.Format(entity.Parms, query.Name, query.NameSpaceId)}",
                Method = entity.Method
            };
            return HttpMethod.HttpWork(item).Html.Contains("ok");
        }

        private NacosAPIEntity GetNacosAPI(string name)
        {
            return nacosAPIs.FirstOrDefault(api => api.Name.Equals(name));
        }

        public void DescoveryInit(IServiceInfo service)
        {
            query = service;
            nacosAPIs = CreateNacosApis();
        }

        private IEnumerable<NacosAPIEntity> CreateNacosApis()
        {
            if (JsonConvert.DeserializeObject(configstr) is JObject jobj)
            {
                nacoshost = Convert.ToString(jobj.SelectToken("NacosHost"));
                routeaddress = Convert.ToString(jobj.SelectToken("Route.address"));
                routeinfo = Convert.ToString(jobj.SelectToken("Route.routeinfo"));
                var tokens = jobj.SelectTokens("NacosApis[*]");
                return from a in tokens
                       select new NacosAPIEntity
                       {
                           Name = Convert.ToString(a["Name"]),
                           Description = Convert.ToString(a["Description"]),
                           Method = Convert.ToString(a["Method"]),
                           Url = Convert.ToString(a["Url"]),
                           Parms = Convert.ToString(a["Parms"]),
                           Body = Convert.ToString(a["Body"]),
                       };
            }
            return null;
        }

        public IServiceConfig GetConfig(IServiceConfig config)
        {
            NacosAPIEntity entity = GetNacosAPI("GetConfig");
            item = new HttpItem
            {
                URL = $"{nacoshost}{entity.Url}?{String.Format(entity.Parms,query.NameSpaceId, config.dataId, config.group)}",
                Method = entity.Method
            };
            config.configstr = HttpMethod.HttpWork(item).Html;
            return config;
        }

        public void ListenConfig()
        {
            NacosAPIEntity entity = GetNacosAPI("ListenConfig");
        }

        public IEnumerable<IServiceInfo> QueryServiceList()
        {
            NacosAPIEntity entity = GetNacosAPI("QueryServiceList");
            item = new HttpItem
            {
                URL = $"{nacoshost}{entity.Url}?{String.Format(entity.Parms, query.NameSpaceId)}",
                Method = entity.Method
            };
            String html = HttpMethod.HttpWork(item).Html;
            if (JsonConvert.DeserializeObject(html) is JObject jobj)
            {
                var doms = jobj.SelectTokens("doms[*]");
                return from a in doms
                       select new NacosService
                       {
                           Name = Convert.ToString(a),
                           NameSpaceId = query.NameSpaceId
                       };
            }
            return null;
        }

        public bool RegInstance()
        {
            NacosAPIEntity entity = GetNacosAPI("RegInstance");
            item = new HttpItem
            {
                URL = $"{nacoshost}{entity.Url}?{String.Format(entity.Parms, query.ip, query.port, query.NameSpaceId, query.Name,query.clustername,query.weight)}",
                Method = entity.Method
            };
            //映射路由
            String html = HttpMethod.HttpWork(item).Html;
            item = new HttpItem
            {
                URL = routeaddress,
                Method = "POST",
                Postdata = routeinfo.Replace("{ServiceName}", query.Name),
                ContentType = "application/json; charset=UTF-8"
            };
            var result = HttpMethod.HttpWork(item);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("注册路由出错");
            }
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    Beat();
                }
            });
            return html.Contains("ok");
        }

        public IServiceInfo QueryService()
        {
            NacosAPIEntity entity = GetNacosAPI("QueryService");
            item = new HttpItem
            {
                URL = $"{nacoshost}{entity.Url}?{String.Format(entity.Parms, query.NameSpaceId)}",
                Method = entity.Method
            };
            String html = HttpMethod.HttpWork(item).Html;
            if (html.Contains("ok"))
            {
                return new NacosService
                {
                    Name = Convert.ToString(query.Name),
                    NameSpaceId = query.NameSpaceId,
                    ip = query.ip,
                    port = query.port,
                };
            }
            return null;
        }
    }
}
