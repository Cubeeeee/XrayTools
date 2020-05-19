# XrayTools
Xray工具类库汇总

# Xray.Demo.XXX
 测试Demo程序，包含.Net和.Net Core两个Demo,类库大多数使用.Net Standard开发，两者都支持


# [Xray.Tools.ExtractLib](http://gitlab.zcznb.top/XXY/xraytool/-/blob/master/XrayTools/Xray.Tools.ExtractLib/Xray.Tools.ExtractLib.md)
编码转换，字符抽取，加解密类库，支持正则表达式、Xpath、JPath几种方式进行匹配和抽取。支持时间戳、MD5、Unicode、Url等加解密或编码转换
使用示例:  
`ExtractMethod.GetResult( ExtractType.Regex, "正则/XPATH/JPATH", "inputstr");`  
`EncodeMethod.Encode( EncodeType.UrlEncode,"inputstr");`


# Xray.Tools.File
封装了Zip文件压缩方法


# Xray.Tools.HttpToolsLib
射线网络Http请求相关工具库，集成HttpHelper，并支持HttpClient使用  
封装了Http、SMTP请求，其中HTTP请求基于HttpWebRequest和HttpClient、并添加了如切换代理重试、重试次数控制等请求方法。封装了API代理IP池实现  
2019/8/19 支持SMTP协议发送邮件  
2019/12/27 支持快速手动重定向，主要为了解决.net core下的AllowAutoRedirect无效的问题  
2020/05/08 添加默认的json格式校验器  
2020/05/18 添加查询字符串和请求体参数快速拆分方法  

# Xray.Tools.Microservices
微服务注册帮助类，目前实现了API方式注册Nacos


# Xray.Tools.Office
封装了Office办公套件操作，基于Aspose.dll,目前实现了对于Excel的标准化输出，支持Excel样式，只能用于.Net平台，使用Demo见
<a href="https://blog.csdn.net/qq_26712977/article/details/78529077" target="_blank"><span class="article-type type-1 float-none">Excel操作帮助类 (基于Aspose.Cells.dll)</span></a>

# Xray.Tools.ScriptEngine
封装了JS脚本执行引擎


# Xray.Tools.SimpleServer
简易的Http服务器


# Xray.Tools.SpiderHelper
任务队列式爬虫模板
