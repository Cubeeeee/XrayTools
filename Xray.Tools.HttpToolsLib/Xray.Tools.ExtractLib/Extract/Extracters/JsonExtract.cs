using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xray.Tools.ExtractLib.Extract.ExtractParms;
using Xray.Tools.ExtractLib.Interfaces;

namespace Xray.Tools.ExtractLib.Extract.Extracters
{
    [ExtractType(ExtractType.Json)]
    public class JsonExtract<T> : IExtract<T> where T : BaseParm
    {
        public bool Check(string Txt, string Jpath, T parm = null)
        {
            Txt = Txt?.Trim();
            if(Txt.StartsWith("{")&&Txt.EndsWith("}")&&JsonConvert.DeserializeObject(Txt) is JObject jobj)
            {
                JsonParm jsonam = BaseParm.ConvertParm<JsonParm>(parm);
                String str = Convert.ToString(jobj.SelectToken(Jpath));
                return str.Equals(jsonam.CheckStr, jsonam.caseoption);
            }
            return false;
        }

        public string GetResult(string Txt, string Jpath, T parm)
        {
            Txt = Txt?.Trim();
            if (Txt.StartsWith("{") && Txt.EndsWith("}") && JsonConvert.DeserializeObject(Txt) is JObject jobj)
            {
                JsonParm jsonam = BaseParm.ConvertParm<JsonParm>(parm);
                String str = Convert.ToString(jobj.SelectToken(Jpath));
                return str;
            }
            return String.Empty;
        }

        public IEnumerable<string> GetResults(string Txt, string Jpath, T parm)
        {
            Txt = Txt?.Trim();
            List<String> list = new List<string>();
            if (Txt.StartsWith("{") && Txt.EndsWith("}") && JsonConvert.DeserializeObject(Txt) is JObject jobj)
            {
                JsonParm jsonam = BaseParm.ConvertParm<JsonParm>(parm);
                var tokens = jobj.SelectTokens(Jpath);
                if(tokens?.Count()>0)
                {
                    list = (from a in tokens select Convert.ToString(a))?.ToList();
                }
            }
            return list;
        }

        public string Replace(string Txt, string Jpath, string RepStr)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> Split(string Txt, string Reg)
        {
            throw new System.NotImplementedException();
        }
    }
}
