using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using Xray.Tools.ExtractLib.Extract.ExtractParms;
using Xray.Tools.ExtractLib.Interfaces;

namespace Xray.Tools.ExtractLib.Extract.Extracters
{
    [ExtractType(ExtractType.Xpath)]
    public class XpathExtract<T> : IExtract<T> where T :BaseParm
    {
        public bool Check(string Txt, string Xpath, T parm)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Txt);
            var node = doc.DocumentNode.SelectSingleNode(Xpath);
            XpathPam xpathparm = BaseParm.ConvertParm<XpathPam>(parm);
            if(node == null)
            {
                return false;
            }
            //null 校验是否存在
            if (String.IsNullOrEmpty(xpathparm.CheckStr))
            {
                return true;
            }
            //非null校验值
            else
            {
                String value = String.Empty;
                //没有属性值直接取innertext
                if(!String.IsNullOrEmpty(xpathparm.Attr))
                {
                    value = node.InnerText;
                }
                else
                {
                    value = node.Attributes[xpathparm.Attr]?.Value;
                }
                return value.Equals(xpathparm.CheckStr, xpathparm.caseoption);
            }
        }

        public string GetResult(string Txt, string Xpath, T parm)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Txt);
            var node = doc.DocumentNode.SelectSingleNode(Xpath);
            XpathPam xpathparm = BaseParm.ConvertParm<XpathPam>(parm);
            if (node == null)
            {
                return String.Empty;
            }
            //null 校验是否存在
            if (xpathparm.InnerHtml)
            {
                return node.InnerHtml;
            }
            //非null校验值
            else if(!String.IsNullOrEmpty(xpathparm.Attr))
            {
                return node.Attributes[xpathparm.Attr]?.Value;
            }
            else
            {
                return node.InnerText;
            }
        }

        public IEnumerable<string> GetResults(string Txt, string Xpath, T parm)
        {
            List<String> list = new List<string>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Txt);
            var nodes = doc.DocumentNode.SelectNodes(Xpath);
            XpathPam xpathparm = BaseParm.ConvertParm<XpathPam>(parm);
            if (nodes?.Count > 0)
            {
                foreach(var node in nodes)
                {
                    //null 校验是否存在
                    if (xpathparm.InnerHtml)
                    {
                        list.Add(node.InnerHtml);
                    }
                    //非null校验值
                    else if (!String.IsNullOrEmpty(xpathparm.Attr))
                    {
                        list.Add(node.Attributes[xpathparm.Attr]?.Value);
                    }
                    else
                    {
                        list.Add(node.InnerText);
                    }
                }
            }
            return list;
        }

        public string Replace(string Txt, string Reg, string RepStr)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Split(string Txt, string Reg)
        {
            throw new NotImplementedException();
        }

    }


}
