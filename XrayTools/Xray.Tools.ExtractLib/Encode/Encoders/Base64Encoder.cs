using System;
using System.Text;
using Xray.Tools.ExtractLib.Interfaces;

namespace Xray.Tools.ExtractLib.Encode.Encoders
{
    [EncodeType(EncodeType.Base64Encode)]
    class Base64Encode<T>:IEncoder<T>
    {
        #region Base64
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static T Base64_Encode(T Str, String encode = "utf-8")
        {
            String source = Convert.ToString(Str);
            String base64str = String.Empty;
            byte[] bytes = Encoding.GetEncoding(encode).GetBytes(source);
            try
            {
                base64str = Convert.ToBase64String(bytes);
            }
            catch
            {
                base64str = source;
            }
            return (T)(object)base64str;
        }

        public T Encode(T str, object parm)
        {
            if(parm is String encodertype)
            {
                return Base64_Encode(str,encodertype);
            }
            return str;
        }
        #endregion
    }
    [EncodeType(EncodeType.Base64Decode)]
    class Base64Decode<T> : IEncoder<T>
    {
        #region Base64

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static T Decode_Base64(T Str, String encode = "utf-8")
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
            return (T)(object)decode;
        }

        public T Encode(T str, object parm)
        {
            if (parm is String encodertype)
            {
                return Decode_Base64(str, encodertype);
            }
            return str;
        }
        #endregion
    }
}
