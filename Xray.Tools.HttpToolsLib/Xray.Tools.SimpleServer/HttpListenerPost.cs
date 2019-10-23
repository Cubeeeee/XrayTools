using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xray.Tools.SimpleServer
{
    public class SimpleServer
    {
        static bool Runing = false;
        static HttpListener httpPostRequest { get; set; } = new HttpListener();

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="responsefunc">响应处理方法</param>
        public static void StartServer(String url, Func<String, String, String, String> responsefunc)
        {
            if(!Runing)
            {
                Runing = true;
                httpPostRequest.Prefixes.Add(url);
                httpPostRequest.Start();
                Console.WriteLine($"Http监听服务启动 地址:{url}");
                Task.Run(()=> httpPostRequestHandle(responsefunc));
            }
        }

        public static void StopServer()
        {
            try
            {
                httpPostRequest.Stop();
                Runing = false;
            }
            catch (Exception)
            {

            }
        }

        private static void httpPostRequestHandle(Func<String,String, String,String> responsefunc)
        {
            while (true)
            {
                HttpListenerContext requestContext = httpPostRequest.GetContext();
                Thread threadsub = new Thread(new ParameterizedThreadStart((requestcontext) =>
                {
                    HttpListenerContext request = (HttpListenerContext)requestcontext;
                    String body = String.Empty;
                    using (StreamReader sr = new StreamReader(request.Request.InputStream))
                    {
                        body = sr.ReadToEnd();
                    }

                    request.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    request.Response.StatusCode = 200;
                    request.Response.ContentType = "application/json";
                    requestContext.Response.ContentEncoding = Encoding.UTF8;

                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responsefunc.Invoke(request.Request.HttpMethod,request.Request.Url.ToString(), body));
                    //Response
                    request.Response.ContentLength64 = buffer.Length;
                    var output = request.Response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }));
                threadsub.Start(requestContext);
            }
        }
    }


    /// <summary>
    /// HttpListenner监听Post请求参数值实体
    /// </summary>
    public class HttpListenerPostValue
    {
        /// <summary>
        /// 0=> 参数
        /// 1=> 文件
        /// </summary>
        public int type = 0;
        public string name;
        public byte[] datas;
    }

    /// <summary>
    /// 获取Post请求中的参数和值帮助类
    /// </summary>
    public class HttpListenerPostParaHelper
    {
        private HttpListenerContext request;

        public HttpListenerPostParaHelper(HttpListenerContext request)
        {
            this.request = request;
        }

        private bool CompareBytes(byte[] source, byte[] comparison)
        {
            try
            {
                int count = source.Length;
                if (source.Length != comparison.Length)
                    return false;
                for (int i = 0; i < count; i++)
                    if (source[i] != comparison[i])
                        return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private byte[] ReadLineAsBytes(Stream SourceStream)
        {
            var resultStream = new MemoryStream();
            while (true)
            {
                int data = SourceStream.ReadByte();
                resultStream.WriteByte((byte)data);
                if (data == 10)
                    break;
            }
            resultStream.Position = 0;
            byte[] dataBytes = new byte[resultStream.Length];
            resultStream.Read(dataBytes, 0, dataBytes.Length);
            return dataBytes;
        }

        /// <summary>
        /// 获取Post过来的参数和数据
        /// </summary>
        /// <returns></returns>
        public List<HttpListenerPostValue> GetHttpListenerPostValue()
        {
            try
            {
                List<HttpListenerPostValue> HttpListenerPostValueList = new List<HttpListenerPostValue>();
                if (request.Request.ContentType.Length > 20 && string.Compare(request.Request.ContentType.Substring(0, 20), "multipart/form-data;", true) == 0)
                {
                    string[] HttpListenerPostValue = request.Request.ContentType.Split(';').Skip(1).ToArray();
                    string boundary = string.Join(";", HttpListenerPostValue).Replace("boundary=", "").Trim();
                    byte[] ChunkBoundary = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");
                    byte[] EndBoundary = Encoding.UTF8.GetBytes("--" + boundary + "--\r\n");
                    Stream SourceStream = request.Request.InputStream;
                    var resultStream = new MemoryStream();
                    bool CanMoveNext = true;
                    HttpListenerPostValue data = null;
                    while (CanMoveNext)
                    {
                        byte[] currentChunk = ReadLineAsBytes(SourceStream);
                        if (!Encoding.UTF8.GetString(currentChunk).Equals("\r\n"))
                            resultStream.Write(currentChunk, 0, currentChunk.Length);
                        if (CompareBytes(ChunkBoundary, currentChunk))
                        {
                            byte[] result = new byte[resultStream.Length - ChunkBoundary.Length];
                            resultStream.Position = 0;
                            resultStream.Read(result, 0, result.Length);
                            CanMoveNext = true;
                            if (result.Length > 0)
                                data.datas = result;
                            data = new HttpListenerPostValue();
                            HttpListenerPostValueList.Add(data);
                            resultStream.Dispose();
                            resultStream = new MemoryStream();

                        }
                        else if (Encoding.UTF8.GetString(currentChunk).Contains("Content-Disposition"))
                        {
                            byte[] result = new byte[resultStream.Length - 2];
                            resultStream.Position = 0;
                            resultStream.Read(result, 0, result.Length);
                            CanMoveNext = true;
                            data.name = Encoding.UTF8.GetString(result).Replace("Content-Disposition: form-data; name=\"", "").Replace("\"", "").Split(';')[0];
                            resultStream.Dispose();
                            resultStream = new MemoryStream();
                        }
                        else if (Encoding.UTF8.GetString(currentChunk).Contains("Content-Type"))
                        {
                            CanMoveNext = true;
                            data.type = 1;
                            resultStream.Dispose();
                            resultStream = new MemoryStream();
                        }
                        else if (CompareBytes(EndBoundary, currentChunk))
                        {
                            byte[] result = new byte[resultStream.Length - EndBoundary.Length - 2];
                            resultStream.Position = 0;
                            resultStream.Read(result, 0, result.Length);
                            data.datas = result;
                            resultStream.Dispose();
                            CanMoveNext = false;
                        }
                    }
                }
                return HttpListenerPostValueList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
