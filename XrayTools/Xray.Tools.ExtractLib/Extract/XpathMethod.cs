#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Xray.Tools.ExtractLib.Extract
* 项目描述 ：
* 类 名 称 ：XpathMethod
* 类 描 述 ：
* 命名空间 ：Xray.Tools.ExtractLib.Extract
* 机器名称 ：XXY-PC 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：XXY
* 创建时间 ：2019/4/10 16:44:58
* 更新时间 ：2019/4/10 16:44:58
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ XXY 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Web;

namespace Xray.Tools.ExtractLib.Extract
{
    /// <summary>
    /// 使用xpath抽取文本
    /// </summary>
    public class XpathMethod
    {
        #region 构造函数
        /// <summary>
        /// 构造函数重载 传入html源代码 生成HtmlDocument对象
        /// </summary>
        /// <param name="html"></param>
        public XpathMethod(String html)
        {
            Document.LoadHtml(html);
        }
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public XpathMethod()
        {
            // TODO: Complete member initialization
        }
        #endregion

        #region 属性
        private HtmlAgilityPack.HtmlDocument _Document = new HtmlDocument();

        public HtmlAgilityPack.HtmlDocument Document
        {
            get { return _Document; }
            set { _Document = value; }
        }
        #endregion

        #region 静态方法 
        /// <summary>
        /// 抽取单个node的内容
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <returns></returns>
        public static String GetSingleResult(String Xpath, String Html)
        {
            String retStr = String.Empty;
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNode node = doc.DocumentNode.SelectSingleNode(Xpath);
            if (node != null)
            {
                retStr = node.InnerText.Trim(); ;
            }
            return retStr;
        }
        /// <summary>
        /// 抽取单个node的内容  Flag标识抽取或匹配
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <param name="Flag">0为匹配 1为抽取</param>
        /// <returns></returns>
        public static String GetSingleResult(String Xpath, String Html, int Flag)
        {
            String retStr = String.Empty;
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNode node = doc.DocumentNode.SelectSingleNode(Xpath);
            if (node != null)
            {
                if (Flag == 1)
                {
                    retStr = node.InnerText;
                }
                else if (Flag == 0)
                {
                    retStr = node.OuterHtml;
                }
            }
            return retStr;
        }
        /// <summary>
        /// 抽取单个node指定属性
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <param name="Attributes">待抽取的节点属性</param>
        /// <returns></returns>
        public static String GetSingleResult(String Xpath, String Html, String Attributes)
        {
            String retStr = String.Empty;
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNode node = doc.DocumentNode.SelectSingleNode(Xpath);
            if (node != null)
            {
                try
                {
                    if (node.Attributes.Contains(Attributes))
                    {
                        retStr = node.Attributes[Attributes].Value;
                    }
                }
                catch
                {

                }
            }
            return retStr;
        }
        /// <summary>
        /// 抽取多个节点内容
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <returns></returns>
        public static List<String> GetMutResult(String Xpath, String Html)
        {
            List<String> retlist = new List<string>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(Xpath);
            if (nodes != null && nodes.Count > 0)
            {
                foreach (var node in nodes)
                {
                    retlist.Add(node.InnerText.Trim());
                }
            }
            return retlist;
        }
        /// <summary>
        /// 抽取多个节点内容
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <param name="Flag">0为匹配 1为抽取</param>
        /// <returns></returns>
        public static List<String> GetMutResult(String Xpath, String Html, int Flag)
        {
            List<String> retlist = new List<string>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(Xpath);
            if (nodes != null && nodes.Count > 0)
            {
                foreach (var node in nodes)
                {
                    if (Flag == 1)
                    {
                        retlist.Add(node.InnerText);
                    }
                    else if (Flag == 0)
                    {
                        retlist.Add(node.OuterHtml);
                    }
                }
            }
            return retlist;
        }
        /// <summary>
        /// 抽取多个node 指定属性
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <param name="Attributes">待抽取的节点属性</param>
        /// <returns></returns>
        public static List<String> GetMutResult(String Xpath, String Html, String Attributes)
        {
            List<String> retlist = new List<string>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(Xpath);
            if (nodes != null && nodes.Count > 0)
            {
                foreach (var node in nodes)
                {
                    try
                    {
                        if (node.Attributes.Contains(Attributes))
                        {
                            retlist.Add(node.Attributes[Attributes].Value);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            return retlist;
        }
        /// <summary>
        /// 抽取多个node
        /// </summary>
        /// <param name="Xpath">Xpath字符串</param>
        /// <param name="Html">待抽取的标准Html源代码</param>
        /// <returns></returns>
        public static HtmlNodeCollection GetMutNodes(String Xpath, String Html)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(Xpath);

            return nodes;
        }
        #endregion
    }
}