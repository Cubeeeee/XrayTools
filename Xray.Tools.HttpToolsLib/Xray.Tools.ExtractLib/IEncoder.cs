using System;
using System.Collections.Generic;
using System.Text;

namespace Xray.Tools.ExtractLib
{
    public interface IEncoder<T>
    {
        T Encode(T str,object parm);
    }
}
