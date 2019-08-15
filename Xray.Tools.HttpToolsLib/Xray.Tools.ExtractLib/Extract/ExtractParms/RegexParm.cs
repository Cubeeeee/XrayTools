using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Xray.Tools.ExtractLib.Extract.ExtractParms
{
    public class RegexPam:BaseParm
    {
        public RegexOptions Options { get; set; } = RegexOptions.None;
        public int Gropu { get; set; } = 0;
    }
}
