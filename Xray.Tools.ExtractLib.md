# Xray.Tools.ExtractLib工具库介绍

## 工具库功能概述
**本工具库基于.Net Standard 2.0平台，分为两部分，其中ExtractMethod类封装了数据抽取解析的一系列相关方法。  
包括正则表达式、Xpath、Json等。**
  
用法示例:  
//使用正则表达式匹配多条结果 返回123,42  
`var regex_result =  ExtractMethod.GetResults(ExtractType.Regex,"xray123cube42","\\d+");`  
//使用jsonpath匹配单条结果 返回 1  
`var json_result = ExtractMethod.GetResults(ExtractType.Json,"{\"code\":1,\"message\":\"ok\"}","code");`  

**EncodeMethod类封装了数据编码机密的一系列相关方法。目前支持Url编码、MD5、Base64、Rsa等**。  
用法示例:  
//url编码 连续两次编码 输出大写  返回%25E5%25B7%25A5%25E5%2585%25B7%25E5%25BA%2593  
`var result = EncodeMethod.Encode( EncodeType.UrlEncode,"工具库",new UrlEncodeParm {  time = 2, upper = true});`
