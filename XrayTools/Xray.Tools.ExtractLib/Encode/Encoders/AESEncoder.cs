using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Xray.Tools.ExtractLib.Interfaces;

namespace Xray.Tools.ExtractLib.Encode.Encoders
{
    /// <summary>
    /// AES加解密 和java结果一致
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EncodeType(EncodeType.AESEncode)]
    public class AESEncoder<T> : IEncoder<T>
    {
        public T Encode(T str, object parm)
        {
            String result = String.Empty;
            String toEncrypt = Convert.ToString(str);
            var key = Convert.ToString(parm);
            if (key == null || key.Length != 16)
            {
                result = "key不能为空并且需要16位长度";
            }
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //将+替换为%2B
            string resultArrayAfter = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            resultArrayAfter = resultArrayAfter.Replace("+", "%2B");

            result = resultArrayAfter;
            return (T)(object)result;
        }
    }

    [EncodeType(EncodeType.AESDecode)]

    public class AESDecode<T> : IEncoder<T>
    {
        public T Encode(T str, object parm)
        {
            String result = String.Empty;
            String toDecrypt = Convert.ToString(str);
            var key = Convert.ToString(parm);

            toDecrypt = toDecrypt.Replace("%2B", "+");
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            result = UTF8Encoding.UTF8.GetString(resultArray);
            return (T)(object)result;
        }
    }
}
