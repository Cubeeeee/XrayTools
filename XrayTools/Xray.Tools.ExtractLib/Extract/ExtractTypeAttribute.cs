using System;

namespace Xray.Tools.ExtractLib.Extract
{
    internal class ExtractTypeAttribute : Attribute
    {
        public ExtractType extracttype { get; set; }

        public ExtractTypeAttribute(ExtractType et)
        {
            this.extracttype = et;
        }
    }
}