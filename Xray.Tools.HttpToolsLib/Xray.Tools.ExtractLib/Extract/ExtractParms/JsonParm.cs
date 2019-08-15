using System;
using System.Collections.Generic;
using System.Text;

namespace Xray.Tools.ExtractLib.Extract.ExtractParms
{
    public class JsonParm : BaseParm
    {
        public StringComparison caseoption { get; set; } = StringComparison.CurrentCultureIgnoreCase;
    }
}
