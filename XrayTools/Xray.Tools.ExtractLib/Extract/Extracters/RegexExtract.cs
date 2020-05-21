using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xray.Tools.ExtractLib.Extract.ExtractParms;
using Xray.Tools.ExtractLib.Interfaces;

namespace Xray.Tools.ExtractLib.Extract.Extracters
{
    [ExtractType(ExtractType.Regex)]

    public class RegexExtract<T> : IExtract<T> where T:BaseParm
    {
        public bool Check(string Txt, string Reg, T parm)
        {
            if (!String.IsNullOrEmpty(Txt))
            {
                RegexPam regexPam = BaseParm.ConvertParm<RegexPam>(parm);
                Regex reg = new Regex(Reg, regexPam.Options);
                return reg.IsMatch(Txt);
            }
            return false;
        }

        public String GetResult(string Txt, string Regstr, T parm)
        {
            String retStr = String.Empty;
            if (!String.IsNullOrEmpty(Txt))
            {
                RegexPam regexPam = BaseParm.ConvertParm<RegexPam>(parm);
                Regex reg = new Regex(Regstr, regexPam.Options);
                var match = reg.Match(Txt);
                if (match?.Groups?.Count >= regexPam.Group)
                {
                    retStr = reg.Match(Txt).Groups[regexPam.Group].Value;
                }
            }
            return retStr;
        }

        public List<String> GetResults(string Txt, string Regstr, T parm)
        {
            List<String> list = new List<String>();
            if (!String.IsNullOrEmpty(Txt))
            {
                RegexPam regexPam = BaseParm.ConvertParm<RegexPam>(parm);
                if (!String.IsNullOrEmpty(Regstr))
                {
                    Regex reg = new Regex(Regstr, regexPam.Options);
                    var matches = reg.Matches(Txt);
                    foreach (Match match in matches)
                    {
                        if (match?.Groups?.Count >= regexPam.Group)
                        {
                            list.Add(reg.Match(Txt).Groups[regexPam.Group].Value);
                        }
                    }
                }
            }

            return list;
        }

        public string Replace(string Txt, string Reg, string RepStr)
        {
            if (!String.IsNullOrEmpty(Txt))
            {
                Regex reg = new Regex(Reg);
                return reg.Replace(Txt, RepStr);
            }
            return String.Empty;
        }

        public List<String> Split(string Txt, string Reg)
        {
            Regex reg = new Regex(Reg);
            return reg.Split(Txt)?.ToList();
        }
    }


}
