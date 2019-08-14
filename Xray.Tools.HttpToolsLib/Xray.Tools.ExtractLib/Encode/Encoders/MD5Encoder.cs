using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Xray.Tools.ExtractLib.Encode.Encoders
{
    [EncodeType(EncodeType.MD5Encode)]
    public class MD5Encoder<T> : IEncoder<T>
    {
        public T Encode(T str, object parm)
        {
            try
            {
                return (T)(object)(MD5(Convert.ToString(str)));
            }
            catch (Exception)
            {

            }
            return str;
        }

        #region MD5
        public static string MD5(string inputString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }
        #endregion
    }
}
