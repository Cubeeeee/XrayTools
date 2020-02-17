using System;
using System.Collections.Generic;
using System.Text;

namespace Xray.Tools.ExtractLib.Extract.ExtractParms
{
    public class XpathPam:BaseParm
    {
        public bool InnerHtml { get; set; } = false;
        public String Attr { get; set; } = String.Empty;
    }
}
