# Xray.Tools.HttpToolsLib
﻿
﻿封装了一系列网络请求操作，其中HttpWebRequest的使用基于苏飞的HttpHelper请求框架  
﻿
## HttpMethod类  
封装的静态方法及功能见下图，这边重点介绍一下HttpWork方法和其重载  
1.  public static HttpResult HttpWork(HttpItem item)  最基础的调用方式 直接使用HttpHelper发起网络请求
2.  public static HttpResult HttpWork(HttpInfo Httpinfo) 基于.Net 的HttpClient发起网络请求
3.  public static HttpResult HttpWork<T>(Func<String, bool> htmlcheckfunc, T httpItem, int trytime = 10, HttpItem item = null) where T : HttpItem 带有结果校验的请求方法 调用方法会一直尝试请求直到超过次数限制或者满足校验条件
4.  public static HttpResult HttpWork<T>(Func<HttpResult, bool> htmlcheckfunc, T httpItem, int trytime = 10, HttpItem item = null) where T : HttpItem 功能与3相同，不同的是校验方法的入参是HttpRequest
5.  public static HttpResult HttpWork<T>(Func<HttpResult, bool> resultcheckfunc, Func<T, T> requestchangefunc, T httpItem, int trytime = 20) where T : HttpItem 带有校验器和请求修改器的请求方法 除了有校验器的功能外，还可以在每次请求时对请求参数进行修改，例如每次请求换用不同的代理ip
6.  public static HttpResult HttpWork<T>(Func<HttpResult, bool> resultcheckfunc, Func<T, T> requestchangefunc, Func<T, bool> endfunc, T httpItem, int trytime = 20) where T : HttpItem 功能同5 但是添加了请求结束方法 即在请求完毕时会调用传入的方法 例如对有效的代理ip进行缓存重复利用

![](http://gitlab.zcznb.top/XXY/xraytool/-/raw/master/XrayTools/Images/Xray.Tools.HttpToolsLib.png?inline=false)