using System;
using System.Web;
using Xray.Tools.ExtractLib.Interfaces;

namespace Xray.Tools.ExtractLib.Encode.Encoders
{
    [EncodeType(EncodeType.HtmlEncode)]
    class HtmlEncode<T> : IEncoder<T>
    {
        public T Encode(T str, object parm)
        {
            if (parm is int time)
            {
                if (time == 0)
                {
                    time = 1;
                }
                for (int i = 0; i < time; i++)
                {
                    str = (T)(object)HttpUtility.HtmlEncode(str);
                }
            }
            return str;
        }

    }

    [EncodeType(EncodeType.HtmlDecode)]
    class HtmlDecode<T> : IEncoder<T>
    {
        public T Encode(T str, object parm)
        {
            if (parm is int time)
            {
                if (time == 0)
                {
                    time = 1;
                }
                for (int i = 0; i < time; i++)
                {
                    str = (T)(object)HttpUtility.HtmlDecode(Convert.ToString(str));
                }
            }
            return str;
        }
    }
}
