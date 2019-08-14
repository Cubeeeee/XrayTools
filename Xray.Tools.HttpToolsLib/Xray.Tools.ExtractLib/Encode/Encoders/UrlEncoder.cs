using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Xray.Tools.ExtractLib.Encode.Encoders
{
    [EncodeType(EncodeType.UrlEncode)]
    class UrlEncode<T> : IEncoder<T>
    {
        public T Encode(T str, object parm)
        {
            if(parm is UrlEncodeParm urlparm)
            {
                return URLEncode(str,urlparm.upper,urlparm.time,urlparm.encodestr);
            }
            return str;
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="Str">待编码字符串</param>
        /// <param name="time">编码次数</param>
        /// <returns></returns>
        public static T URLEncode(T str, bool upper, int time = 1, string encodestr = "utf-8")
        {
            String Str = Convert.ToString(str);
            if (String.IsNullOrEmpty(Str))
            {
                return str;
            }
            else
            {
                Str = Str.Trim();
            }
            for (int i = 0; i < time; i++)
            {
                Str = HttpUtility.UrlEncode(Str, System.Text.Encoding.GetEncoding(encodestr));
            }
            return (T)(object)(upper ? Str.ToUpper() : Str.ToLower());
        }
    }

    public class UrlEncodeParm {
        public bool upper { get; set; } = false;
        public int time { get; set; } = 1;
        public String encodestr { get; set; } = "utf-8";
    };
    [EncodeType(EncodeType.UrlDecode)]
    class UrlDecode<T> : IEncoder<T>
    {
        public T Encode(T str, object parm)
        {
            if (parm is UrlEncodeParm urlparm)
            {
                return URLDecode(str, urlparm.time, urlparm.encodestr);
            }
            return str;
        }

        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="Str">待编码字符串</param>
        /// <param name="time">解码次数</param>
        /// <returns></returns>
        public static T URLDecode(T str, int time = 1, String encodestr = "utf-8")
        {
            String Str = Convert.ToString(str);
            if (String.IsNullOrEmpty(Str))
            {
                return str;
            }
            for (int i = 0; i < time; i++)
            {
                Str = HttpUtility.UrlDecode(Str, System.Text.Encoding.GetEncoding(encodestr));
            }
            return (T)(object)Str;

        }
    }
}
