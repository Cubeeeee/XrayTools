#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Xray.Tools.ExtractLib.Encode
* 项目描述 ：
* 类 名 称 ：EncodeMethod
* 类 描 述 ：
* 命名空间 ：Xray.Tools.ExtractLib.Encode
* 机器名称 ：XXY-PC 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：XXY
* 创建时间 ：2019/4/10 16:31:18
* 更新时间 ：2019/4/10 16:31:18
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ XXY 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Xray.Tools.ExtractLib.Encode
{
    public class EncodeMethod
    {
        #region HtmlEncoder
        /// <summary>
        /// html解码 泛型
        /// </summary>
        /// <typeparam name="T">可以转换为String类型的对象</typeparam>
        /// <param name="Str">待编码字符串</param>
        /// <param name="time">编码次数</param>
        /// <returns></returns>
        public static String HtmlDecode<T>(T Str, int time = 1)
        {
            String str = Convert.ToString(Str);
            for (int i = 0; i < time; i++)
            {
                str = HttpUtility.HtmlDecode(str);
            }
            return str;
        }
        /// <summary>
        /// html解码 泛型
        /// </summary>
        /// <typeparam name="T">可以转换为String类型的对象</typeparam>
        /// <param name="Str">待解码字符串</param>
        /// <param name="time">解码次数</param>
        /// <returns></returns>
        public static String HtmlEncode<T>(T Str, int time = 1)
        {
            String str = Convert.ToString(Str);
            for (int i = 0; i < time; i++)
            {
                str = HttpUtility.HtmlEncode(str);
            }
            return str;
        }
        #endregion

        #region UrlEncode
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="Str">待编码字符串</param>
        /// <param name="time">编码次数</param>
        /// <returns></returns>
        public static String URLEncode<T>(T str, int time = 1, String encodestr = "utf-8")
        {
            String Str = Convert.ToString(str);
            if (String.IsNullOrEmpty(Str))
            {
                return String.Empty;
            }
            else
            {
                Str = Str.Trim();
            }
            for (int i = 0; i < time; i++)
            {
                Str = HttpUtility.UrlEncode(Str, System.Text.Encoding.GetEncoding(encodestr));
            }
            return Str;
        }
        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="Str">待编码字符串</param>
        /// <param name="time">解码次数</param>
        /// <returns></returns>
        public static String URLDecode<T>(T str, int time = 1, String encodestr = "utf-8")
        {
            String Str = Convert.ToString(str);
            if (String.IsNullOrEmpty(Str))
            {
                return String.Empty;
            }
            for (int i = 0; i < time; i++)
            {
                Str = HttpUtility.UrlDecode(Str, System.Text.Encoding.GetEncoding(encodestr));
            }
            return Str;

        }
        #endregion

        #region Unicode
        /// <summary>
        /// 标准unicode编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UnicodeEncode<T>(T Str)
        {
            String text = Convert.ToString(Str);
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            int len = text.Length;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                builder.Append("\\u");
                builder.Append(UShortToHex((ushort)text[i]));
            }
            return builder.ToString();
        }

        public static string UnicodeDecode<T>(T Str)
        {
            String unicode = Convert.ToString(Str);
            if (string.IsNullOrEmpty(unicode))
            {
                return string.Empty;
            }
            string[] ls = unicode.Split(new string[] { "\\u" }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder builder = new StringBuilder();
            int len = ls.Length;
            for (int i = 0; i < len; i++)
            {
                builder.Append(Convert.ToChar(ushort.Parse(ls[i], System.Globalization.NumberStyles.HexNumber)));
            }
            return builder.ToString();
        }

        private static char[] UShortToHex(ushort n)
        {
            int num;
            char[] hex = new char[4];
            for (int i = 0; i < 4; i++)
            {
                num = n % 16;

                if (num < 10)
                    hex[3 - i] = (char)('0' + num);
                else
                    hex[3 - i] = (char)('A' + (num - 10));

                n >>= 4;
            }
            return hex;
        }
        #endregion

        #region Base64
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode<T>( T Str, String encode = "utf-8")
        {
            String source = Convert.ToString(Str);
            String base64str = String.Empty;
            byte[] bytes =Encoding.GetEncoding(encode).GetBytes(source);
            try
            {
                base64str = Convert.ToBase64String(bytes);
            }
            catch
            {
                base64str = source;
            }
            return base64str;
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64<T>(T Str, String encode = "utf-8")
        {
            String result = Convert.ToString(Str);
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = Encoding.GetEncoding(encode).GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
        #endregion
    }
}