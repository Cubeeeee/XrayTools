#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Xray.Tools.HttpToolsLib
* 项目描述 ：
* 类 名 称 ：HttpMethod
* 类 描 述 ：
* 命名空间 ：Xray.Tools.HttpToolsLib
* 机器名称 ：XXY-PC 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：XXY
* 创建时间 ：2019/4/9 16:51:53
* 更新时间 ：2019/4/9 16:51:53
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ XXY 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion
using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Xray.Tools.HttpToolsLib
{
    public class HttpMethod
    {
        static String CheckUrl = $"http://{DateTime.Now.Year}.ip138.com/ic.asp";
        static WebClient wclient = new WebClient();
        static HttpMethod()
        {
            GlobalHttpConfig.UpdateConfig();
        }


        /// <summary>
        /// 封装的Http参数化请求
        /// </summary>
        /// <param name="Httpinfo"></param>
        /// <returns>返回的源代码</returns>

        public static String HttpWork_Html(HttpItem httpItem)
        {
            return new HttpHelper().GetHtml(httpItem).Html;
        }
        public static HttpResult HttpWork(HttpInfo Httpinfo, HttpClient client)
        {
            MemoryStream _stream = null;
            HttpResponseMessage res = null;
            IWebProxy webProxy = null;
            HttpRequestMessage myRequest = new HttpRequestMessage();
            HttpResult result = new HttpResult();
            try
            {
                if (!String.IsNullOrEmpty(Httpinfo.ProxyIp))
                {
                    String ip = Httpinfo.ProxyIp;
                    try
                    {
                        var arr = ip.Split(':');
                        var port = Convert.ToInt32(arr[arr.Length - 1]);
                        var address = ip.Replace(":" + port, String.Empty);
                        webProxy = new WebProxy(address, port)
                        {
                            Credentials = new NetworkCredential(Httpinfo.ProxyUserName, Httpinfo.ProxyPwd)
                        };
                        arr = null;
                        address = null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("设置代理ip失败:{0}", ex.Message);
                    }
                }
                //填充请求头
                myRequest.RequestUri = new Uri(Httpinfo.URL);
                myRequest.Headers.Add("Accept", Httpinfo.Accept);
                myRequest.Headers.Add("Cookie", Httpinfo.Cookie);
                myRequest.Headers.Add("User-Agent", Httpinfo.UserAgent);
                myRequest.Headers.Add("Referer", String.IsNullOrEmpty(Httpinfo.Referer) ? Httpinfo.URL : Httpinfo.Referer);
                myRequest.Headers.Add("Connection", Httpinfo.KeepAlive ? "keep-alive" : "close");
                //自定义头
                foreach (var k in Httpinfo.Header.AllKeys)
                {
                    myRequest.Headers.Add(k, Httpinfo.Header[k]);
                }
                //Get
                if (String.IsNullOrEmpty(Httpinfo.Postdata) && Httpinfo.Method.ToUpper().Equals("GET"))
                {
                    myRequest.Method = System.Net.Http.HttpMethod.Get;
                }
                //Post
                else
                {
                    myRequest.Content = new StringContent(Httpinfo.Postdata);
                    myRequest.Method = System.Net.Http.HttpMethod.Post;
                    myRequest.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Httpinfo.ContentType);
                }
                res = client.SendAsync(myRequest).Result;


                result.StatusCode = res.StatusCode;
                res.Headers?.ToList().ForEach(head =>
                {
                    if (Enum.IsDefined(typeof(HttpRequestHeader), head.Key))
                    {
                        result.Header.Add((HttpRequestHeader)Enum.Parse(typeof(HttpRequestHeader), head.Key), String.Join(";", head.Value));
                    }
                });
                if (res.Headers.Contains("Set-Cookie"))
                    result.Cookie = String.Join(";", res.Headers.GetValues("Set-Cookie"));
                using (Stream result_stream = res.Content.ReadAsStreamAsync().Result)
                {
                    if (res.Content.Headers.ContentEncoding.ToList().Exists(en => en.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        _stream = HttpHelper.GetMemoryStream(new GZipStream(result_stream, CompressionMode.Decompress));
                    }
                    else
                    {
                        _stream = HttpHelper.GetMemoryStream(result_stream);
                    }
                }
                //获取Byte
                byte[] RawResponse = _stream.ToArray();
                _stream.Close();
                //是否返回Byte类型数据
                if (Httpinfo.ResultType == ResultType.Byte)
                    result.ResultByte = RawResponse;
                //从这里开始我们要无视编码了
                if (Httpinfo.Encoding == null)
                {
                    Match meta = Regex.Match(Encoding.Default.GetString(RawResponse), "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                    string charter = (meta.Groups.Count > 1) ? meta.Groups[2].Value.ToLower() : string.Empty;
                    charter = charter.Replace("\"", "").Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk");
                    if (charter.Length > 2)
                        Httpinfo.Encoding = Encoding.GetEncoding(charter.Trim());
                    else
                    {
                        Httpinfo.Encoding = Encoding.UTF8;
                    }
                }
                //得到返回的HTML
                result.Html = Httpinfo.Encoding.GetString(RawResponse);

            }
            catch (Exception ex)
            {
                result.Html = ex.Message;
            }
            finally
            {
                if (_stream != null)
                {
                    _stream.Close();
                    _stream.Dispose();
                    _stream = null;
                }
                if (res != null)
                {
                    res.Dispose();
                    res = null;
                }
            }

            return result;
        }
        /// <summary>
        /// 使用HttpClient请求
        /// </summary>
        /// <param name="Httpinfo"></param>
        /// <returns></returns>
        public static HttpResult HttpWork(HttpInfo Httpinfo)
        {
            MemoryStream _stream = null;
            HttpResponseMessage res = null;
            IWebProxy webProxy = null;
            HttpRequestMessage myRequest = new HttpRequestMessage();
            HttpResult result = new HttpResult();
            try
            {
                if (!String.IsNullOrEmpty(Httpinfo.ProxyIp))
                {
                    String ip = Httpinfo.ProxyIp;
                    try
                    {
                        var arr = ip.Split(':');
                        var port = Convert.ToInt32(arr[arr.Length - 1]);
                        var address = ip.Replace(":" + port, String.Empty);
                        webProxy = new WebProxy(address, port)
                        {
                            Credentials = new NetworkCredential(Httpinfo.ProxyUserName, Httpinfo.ProxyPwd)
                        };
                        arr = null;
                        address = null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("设置代理ip失败:{0}", ex.Message);
                    }
                }
                using (HttpClient _httpClient = new HttpClient(new HttpClientHandler
                {
                    UseCookies = false,
                    AllowAutoRedirect = Httpinfo.Allowautoredirect,
                    AutomaticDecompression = DecompressionMethods.GZip,
                    UseProxy = !String.IsNullOrEmpty(Httpinfo.ProxyIp),
                    Proxy = webProxy
                })
                {
                    Timeout = new TimeSpan(0, 0, 60)
                })
                {
                    //填充请求头
                    myRequest.RequestUri = new Uri(Httpinfo.URL);
                    myRequest.Headers.Add("Accept", Httpinfo.Accept);
                    myRequest.Headers.Add("Cookie", Httpinfo.Cookie);
                    myRequest.Headers.Add("User-Agent", Httpinfo.UserAgent);
                    myRequest.Headers.Add("Referer", String.IsNullOrEmpty(Httpinfo.Referer) ? Httpinfo.URL : Httpinfo.Referer);
                    myRequest.Headers.Add("Connection", Httpinfo.KeepAlive ? "keep-alive" : "close");
                    //自定义头
                    foreach (var k in Httpinfo.Header.AllKeys)
                    {
                        myRequest.Headers.Add(k, Httpinfo.Header[k]);
                    }
                    //Get
                    if (String.IsNullOrEmpty(Httpinfo.Postdata) && Httpinfo.Method.ToUpper().Equals("GET"))
                    {
                        myRequest.Method = System.Net.Http.HttpMethod.Get;
                    }
                    //Post
                    else
                    {
                        myRequest.Content = new StringContent(Httpinfo.Postdata);
                        myRequest.Method = System.Net.Http.HttpMethod.Post;
                        myRequest.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Httpinfo.ContentType);
                    }
                    res = _httpClient.SendAsync(myRequest).Result;
                }


                result.StatusCode = res.StatusCode;
                res.Headers?.ToList().ForEach(head =>
                {
                    if (Enum.IsDefined(typeof(HttpRequestHeader), head.Key))
                    {
                        result.Header.Add((HttpRequestHeader)Enum.Parse(typeof(HttpRequestHeader), head.Key), String.Join(";", head.Value));
                    }
                });
                if (res.Headers.Contains("Set-Cookie"))
                    result.Cookie = String.Join(";", res.Headers.GetValues("Set-Cookie"));
                using (Stream result_stream = res.Content.ReadAsStreamAsync().Result)
                {
                    if (res.Content.Headers.ContentEncoding.ToList().Exists(en => en.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        _stream = HttpHelper.GetMemoryStream(new GZipStream(result_stream, CompressionMode.Decompress));
                    }
                    else
                    {
                        _stream = HttpHelper.GetMemoryStream(result_stream);
                    }
                }
                //获取Byte
                byte[] RawResponse = _stream.ToArray();
                _stream.Close();
                //是否返回Byte类型数据
                if (Httpinfo.ResultType == ResultType.Byte)
                    result.ResultByte = RawResponse;
                //从这里开始我们要无视编码了
                if (Httpinfo.Encoding == null)
                {
                    Match meta = Regex.Match(Encoding.Default.GetString(RawResponse), "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                    string charter = (meta.Groups.Count > 1) ? meta.Groups[2].Value.ToLower() : string.Empty;
                    charter = charter.Replace("\"", "").Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk");
                    if (charter.Length > 2)
                        Httpinfo.Encoding = Encoding.GetEncoding(charter.Trim());
                    else
                    {
                        Httpinfo.Encoding = Encoding.UTF8;
                    }
                }
                //得到返回的HTML
                result.Html = Httpinfo.Encoding.GetString(RawResponse);
            }
            catch (Exception ex)
            {
                result.Html = ex.Message;
            }
            finally
            {
                if (_stream != null)
                {
                    _stream.Close();
                    _stream.Dispose();
                    _stream = null;
                }
                if (res != null)
                {
                    res.Dispose();
                    res = null;
                }
            }

            return result;
        }
        /// <summary>
        /// 使用HttpHelper请求
        /// </summary>
        /// <param name="httpItem"></param>
        /// <returns></returns>
        public static HttpResult HttpWork(HttpItem httpItem)
        {
            return new HttpHelper().GetHtml(httpItem);
        }
        /// <summary>
        /// 尝试指定次数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="htmlcheckfunc"></param>
        /// <param name="httpItem"></param>
        /// <param name="trytime"></param>
        /// <returns></returns>
        public static HttpResult HttpWork<T>(Func<String, bool> htmlcheckfunc, T httpItem, int trytime = 10, HttpItem item = null) where T : HttpItem
        {
            HttpResult result = HttpWork(httpItem);
            try
            {
                while (!htmlcheckfunc.Invoke(result.Html) && --trytime > 0)
                {
                    result = HttpWork(httpItem);
                    Thread.Sleep(1);
                }
            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// 尝试指定次数 每次都修改请求  多用于代理IP重试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="htmlcheckfunc"></param>
        /// <param name="httpItem"></param>
        /// <param name="trytime"></param>
        /// <returns></returns>
        public static HttpResult HttpWork<T>(Func<HttpResult, bool> resultcheckfunc, Func<T, T> requestchangefunc, T httpItem, int trytime = 20) where T : HttpItem
        {
            HttpResult result = null;
            try
            {
                if (trytime == -1)
                {
                    trytime = int.MaxValue;
                }
                else
                {
                    trytime++;
                }
                while (result == null || !resultcheckfunc.Invoke(result) && --trytime > 0)
                {
                    result = HttpWork(requestchangefunc.Invoke(httpItem));
                    Thread.Sleep(1);
                }
            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// 尝试指定次数 每次都修改请求   多用于代理IP重试  增加结束委托 可用于代理IP回收利用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="htmlcheckfunc"></param>
        /// <param name="httpItem"></param>
        /// <param name="trytime"></param>
        /// <returns></returns>
        public static HttpResult HttpWork<T>(Func<HttpResult, bool> resultcheckfunc, Func<T, T> requestchangefunc, Func<T, bool> endfunc, T httpItem, int trytime = 20) where T : HttpItem
        {
            HttpResult result = null;
            try
            {
                T item = default(T);
                if (trytime == -1)
                {
                    trytime = int.MaxValue;
                }
                else
                {
                    trytime++;
                }
                while (!resultcheckfunc.Invoke(result) && --trytime > 0)
                {
                    item = requestchangefunc.Invoke(httpItem);
                    result = HttpWork(item);
                    //if(trytime == -1)
                    //{
                    //    Console.WriteLine($"剩余尝试第{ int.MaxValue - trytime}次");
                    //}
                    //else
                    //{
                    //    Console.WriteLine($"剩余尝试第{ trytime}次");
                    //}
                    Thread.Sleep(1);
                }
                if (resultcheckfunc.Invoke(result))
                {
                    endfunc.Invoke(item);
                }
            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// 异步
        /// </summary>
        /// <param name="Httpinfo"></param>
        /// <returns></returns>
        public async static Task<HttpResult> HttpWorkAsync(HttpInfo Httpinfo)
        {
            MemoryStream _stream = null;
            HttpResponseMessage res = null;
            IWebProxy webProxy = null;
            HttpRequestMessage myRequest = new HttpRequestMessage();
            HttpResult result = new HttpResult();
            try
            {
                if (!String.IsNullOrEmpty(Httpinfo.ProxyIp))
                {
                    String ip = Httpinfo.ProxyIp;
                    try
                    {
                        var arr = ip.Split(':');
                        var port = Convert.ToInt32(arr[arr.Length - 1]);
                        var address = ip.Replace(":" + port, String.Empty);
                        webProxy = new WebProxy(address, port)
                        {
                            Credentials = new NetworkCredential(Httpinfo.ProxyUserName, Httpinfo.ProxyPwd)
                        };
                        arr = null;
                        address = null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("设置代理ip失败:{0}", ex.Message);
                    }
                }
                using (HttpClient _httpClient = new HttpClient(new HttpClientHandler
                {
                    UseCookies = true,
                    AllowAutoRedirect = Httpinfo.Allowautoredirect,
                    AutomaticDecompression = DecompressionMethods.GZip,
                    UseProxy = !String.IsNullOrEmpty(Httpinfo.ProxyIp),
                    Proxy = webProxy
                })
                {
                    Timeout = new TimeSpan(0, 0, 60)
                })
                {
                    //填充请求头
                    myRequest.RequestUri = new Uri(Httpinfo.URL);
                    myRequest.Headers.Add("Accept", Httpinfo.Accept);
                    myRequest.Headers.Add("Cookie", Httpinfo.Cookie);
                    myRequest.Headers.Add("User-Agent", Httpinfo.UserAgent);
                    myRequest.Headers.Add("Referer", String.IsNullOrEmpty(Httpinfo.Referer) ? Httpinfo.URL : Httpinfo.Referer);
                    myRequest.Headers.Add("Connection", Httpinfo.KeepAlive ? "keep-alive" : "close");
                    //自定义头
                    foreach (var k in Httpinfo.Header.AllKeys)
                    {
                        myRequest.Headers.Add(k, Httpinfo.Header[k]);
                    }
                    //Get
                    if (String.IsNullOrEmpty(Httpinfo.Postdata) && Httpinfo.Method.ToUpper().Equals("GET"))
                    {
                        myRequest.Method = System.Net.Http.HttpMethod.Get;
                    }
                    //Post
                    else
                    {
                        myRequest.Content = new StringContent(Httpinfo.Postdata);
                        myRequest.Method = System.Net.Http.HttpMethod.Post;
                        myRequest.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Httpinfo.ContentType);
                    }
                    res = await _httpClient.SendAsync(myRequest);
                }


                result.StatusCode = res.StatusCode;
                res.Headers?.ToList().ForEach(head =>
                {
                    if (Enum.IsDefined(typeof(HttpRequestHeader), head.Key))
                    {
                        result.Header.Add((HttpRequestHeader)Enum.Parse(typeof(HttpRequestHeader), head.Key), String.Join(";", head.Value));
                    }
                });
                if (res.Headers.Contains("Set-Cookie"))
                    result.Cookie = String.Join(";", res.Headers.GetValues("Set-Cookie"));
                using (Stream result_stream = res.Content.ReadAsStreamAsync().Result)
                {
                    if (res.Content.Headers.ContentEncoding.ToList().Exists(en => en.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        _stream = HttpHelper.GetMemoryStream(new GZipStream(result_stream, CompressionMode.Decompress));
                    }
                    else
                    {
                        _stream = HttpHelper.GetMemoryStream(result_stream);
                    }
                }
                //获取Byte
                byte[] RawResponse = _stream.ToArray();
                _stream.Close();
                //是否返回Byte类型数据
                if (Httpinfo.ResultType == ResultType.Byte)
                    result.ResultByte = RawResponse;
                //从这里开始我们要无视编码了
                if (Httpinfo.Encoding == null)
                {
                    Match meta = Regex.Match(Encoding.Default.GetString(RawResponse), "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                    string charter = (meta.Groups.Count > 1) ? meta.Groups[2].Value.ToLower() : string.Empty;
                    charter = charter.Replace("\"", "").Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk");
                    if (charter.Length > 2)
                        Httpinfo.Encoding = Encoding.GetEncoding(charter.Trim());
                    else
                    {
                        Httpinfo.Encoding = Encoding.UTF8;
                    }
                }
                //得到返回的HTML
                result.Html = Httpinfo.Encoding.GetString(RawResponse);

            }
            catch (Exception ex)
            {
                result.Html = ex.Message;
            }
            finally
            {
                if (_stream != null)
                {
                    _stream.Close();
                    _stream.Dispose();
                    _stream = null;
                }
                if (res != null)
                {
                    res.Dispose();
                    res = null;
                }
            }

            return result;
        }

        public static String GetRow(HttpItem item)
        {
            StringBuilder sb = new StringBuilder();
            if (String.IsNullOrEmpty(item.Method))
            {
                item.Method = "GET";
            }
            else
            {
                sb.AppendLine($"{item.Method} {item.URL}  HTTP/1.1");
                sb.AppendLine($"Content-Type:{item.ContentType}");
                sb.AppendLine($"User-Agent:{item.UserAgent}");
                sb.AppendLine($"Accept:{item.Accept}");
                sb.AppendLine($"Referer:{item.Referer}");
                sb.AppendLine($"Cookie:{item.Cookie}");

                foreach (String key in item.Header)
                {
                    sb.AppendLine($"{key}:{item.Header[key]}");
                }

                sb.AppendLine();
                sb.Append(item.Postdata);
            }
            return sb.ToString();
        }

        #region 快速方法
        /// <summary>
        /// 获得代理IP
        /// </summary>
        /// <param name="textbox">textbox控件</param>
        /// <param name="sender">响应事件</param>
        /// <returns>分割后的IP栈堆</returns>
        public static ConcurrentQueue<String> InputApi(String ApiUrl)
        {
            //局部变量 分别表示IP总字符串和分割后的IP栈堆
            String IP = null;
            ConcurrentQueue<String> API = new ConcurrentQueue<string>();
            do
            {
                //获取IP
                IP = FastMethod_HttpHelper(ApiUrl);
            }
            while (IP == null);
            //用正则表达式分割
            MatchCollection m = Regex.Matches(IP, @"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}:\d{1,5}");
            foreach (Match c in m)
            {
                //进队
                API.Enqueue(c.Value);
            }

            IP = null;
            m = null;

            return API;
        }

        public static String FastMethod_HttpHelper(String Url, String Method = "Get", String PostData = "")
        {
            return HttpWork(new HttpItem
            {
                Method = Method,
                URL = Url,
                Postdata = PostData
            })?.Html;
        }

        public static String FastMethod_HttpClient(String Url, String Method = "Get", String PostData = "")
        {
            return HttpWork(new HttpInfo
            {
                Method = Method,
                URL = Url,
                Postdata = PostData
            })?.Html;
        }
        /// <summary>
        /// 校验代理IP
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static bool CheckProxy(String proxy, String username = "", String password = "")
        {
            var ip = proxy?.Split(':')?[0];
            return Convert.ToBoolean(HttpWork(new HttpItem
            {
                URL = CheckUrl,
                ProxyIp = proxy,
                ProxyUserName = username,
                ProxyPwd = password
            })?.Html.Contains(ip));
        }
        /// <summary>
        /// 手动重定向
        /// </summary>
        /// <param name="item">HttpItem</param>
        /// <param name="Limit">最大重定向次数 默认20次</param>
        /// <returns></returns>
        public static HttpResult HttpWork_Redirect(HttpItem item, int Limit = 20)
        {
            HttpResult result = new HttpResult();
            bool finish = false;
            item.Allowautoredirect = false;
            while (!finish && --Limit > 0)
            {
                result = HttpMethod.HttpWork(item);
                item.Method = "GET";
                item.Postdata = String.Empty;
                String location = result.Header["Location"];
                finish = String.IsNullOrEmpty(location);
                item.URL = location;
                Thread.Sleep(1);
            }
            return result;
        }
        #endregion

        #region 文件上传下载
        public static HttpResult UpLoadFile(String url, String path, String encode = "utf-8")
        {
            wclient = new WebClient();
            return new HttpResult
            {
                Html = Encoding.GetEncoding(encode).GetString(wclient.UploadFile(url, path)),
                Cookie = wclient.ResponseHeaders["Set-Cookie"]
            };
        }
        public static HttpResult DownLoadFile(String url, String encode = "utf-8")
        {
            wclient = new WebClient();
            return new HttpResult
            {
                Html = Encoding.GetEncoding(encode).GetString(wclient.DownloadData(url)),
                Cookie = wclient.ResponseHeaders["Set-Cookie"],
            };
        }
        public static byte[] DownLoadFile(String url)
        {
            return wclient.DownloadData(url);
        }
        public static bool DownLoadFileNoCookie(String url, String filename)
        {
            wclient.DownloadFile(url, filename);
            return File.Exists(filename);
        }

        #endregion

        #region 返回校验方法
        /// <summary>
        /// 简易的json校验器
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static bool JsonCheckSimple(String html)
        {
            return Convert.ToBoolean(html?.Trim().StartsWith("{")) && Convert.ToBoolean(html?.Trim().EndsWith("}"));
        }
        #endregion
    }
}