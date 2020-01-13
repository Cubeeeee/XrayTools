using System;
using System.Collections.Generic;
using System.Text;

namespace Xray.Tools.ExtractLib.Extract.ExtractParms
{
    public abstract class BaseParm
    {
        public String CheckStr { get; set; } = String.Empty;
        public StringComparison caseoption { get; set; } = StringComparison.CurrentCultureIgnoreCase;

        public static T ConvertParm<T>(object parm) where T:BaseParm,new ()
        {
            if (parm == null)
            {
                return new T();
            }
            return parm as T;
        }
    }
}
