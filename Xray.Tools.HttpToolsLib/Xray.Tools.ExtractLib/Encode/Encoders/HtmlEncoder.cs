using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Xray.Tools.ExtractLib.Encode.Encoders
{
    [EncodeType(EncodeType.HtmlEncode)]
    class HtmlEncode<T> : IEncoder<T>
    {
        public T Encode(T str, object parm)
        {
            if (parm is int time)
            {
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
                for (int i = 0; i < time; i++)
                {
                    str = (T)(object)HttpUtility.HtmlDecode(Convert.ToString(str));
                }
            }
            return str;
        }
    }
}
