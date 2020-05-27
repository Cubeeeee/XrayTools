using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Xray.Tools.ExtractLib.Interfaces;

namespace Xray.Tools.ExtractLib.Encode.Encoders
{
    /// <summary>
    /// AES加解密 ECB模式 和java结果一致
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EncodeType(EncodeType.AES_ECBEncode)]
    public class AES_ECBEncoder<T> : IEncoder<T>
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

    public class AES_ECBDecode<T> : IEncoder<T>
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


    /// <summary>
    /// AES CBC 加解密
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EncodeType(EncodeType.AES_CCBEncode)]
    public class AES_CCBEncoder<T> : IEncoder<T>
    {
        public T Encode(T str, object parm)
        {
            String text = Convert.ToString(str);

            var cbcparm = (AES_CBCParm)parm;
            String result = String.Empty;
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(cbcparm.key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length) len = keyBytes.Length;
            System.Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(cbcparm.vector);
            if(ivBytes.Length == 0)
            {
                rijndaelCipher.IV = new byte[16];
            }
            else
            {
                rijndaelCipher.IV = ivBytes;
            }
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(text);
            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);
            return (T)(object)Convert.ToBase64String(cipherBytes);
        }
    }

    /// <summary>
    /// AES加解密 和java结果一致
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EncodeType(EncodeType.AES_CCBDEcode)]
    public class AES_CCBDEcoder<T> : IEncoder<T>
    {
        public T Encode(T str, object parm)
        {
            String text = Convert.ToString(str);

            var cbcparm = (AES_CBCParm)parm;
            String result = String.Empty;
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            byte[] encryptedData = Convert.FromBase64String(text);
            byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(cbcparm.key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length) len = keyBytes.Length;
            System.Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(cbcparm.vector);
            if (ivBytes.Length == 0)
            {
                rijndaelCipher.IV = new byte[16];
            }
            else
            {
                rijndaelCipher.IV = ivBytes;
            }
            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            result =  Encoding.UTF8.GetString(plainText);
            return (T)(object)result;
        }
    }
    public class AES_CBCParm
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public String key { get; set; }
        /// <summary>
        /// 向量
        /// </summary>
        public String vector { get; set; }
    }
}
