using System;
using System.Text;
using Xray.Tools.ExtractLib.Interfaces;

namespace Xray.Tools.ExtractLib.Encode.Encoders
{
    [EncodeType(EncodeType.UnicodeEncode)]
    class UnicodeEncode<T>:IEncoder<T>
    {
        public T Encode(T str, object parm)
        {
            return Unicode_Encode(str);
        }
        #region Unicode
        /// <summary>
        /// 标准unicode编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static T Unicode_Encode(T Str)
        {
            String text = Convert.ToString(Str);
            if (string.IsNullOrEmpty(text))
            {
                return Str;
            }
            int len = text.Length;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                builder.Append("\\u");
                builder.Append(UShortToHex((ushort)text[i]));
            }
            return (T)(object)builder.ToString();
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
    }

    [EncodeType(EncodeType.UnicodeDecode)]
    class UnicodeDecode<T> : IEncoder<T>
    {
        public T Encode(T str, object parm)
        {
            return Unicode_Decode(str);
        }
        #region Unicode

        public static T Unicode_Decode(T Str)
        {
            String unicode = Convert.ToString(Str);
            if (string.IsNullOrEmpty(unicode))
            {
                return Str;
            }
            string[] ls = unicode.Split(new string[] { "\\u" }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder builder = new StringBuilder();
            int len = ls.Length;
            for (int i = 0; i < len; i++)
            {
                builder.Append(Convert.ToChar(ushort.Parse(ls[i], System.Globalization.NumberStyles.HexNumber)));
            }
            return (T)(object)builder.ToString();
        }

        #endregion
    }
}
