using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Xray.Tools.ExtractLib.Encode.Encoders
{
    [EncodeType(EncodeType.RsaEncode)]
    public class RsaEncode<T>: IEncoder<T>
    {
        public T Encode(T encryptString, object parm)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] CypherTextBArray;
                string Result;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(Convert.ToString(parm));
                PlainTextBArray = (new UnicodeEncoding()).GetBytes(Convert.ToString(encryptString));
                CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return (T)(object)Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    [EncodeType(EncodeType.RsaDecode)]
    public class RsaDecode<T> : IEncoder<T>
    {
        public T Encode(T decryptString, object parm)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] DypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(Convert.ToString(parm));
                PlainTextBArray = Convert.FromBase64String(Convert.ToString(decryptString));
                DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
                Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
                return (T)(object)Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
