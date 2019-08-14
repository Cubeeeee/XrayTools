using System;

namespace Xray.Tools.ExtractLib.Encode
{
    internal class EncodeTypeAttribute : Attribute
    {
        public EncodeType encodetype { get; set; }

        public EncodeTypeAttribute(EncodeType et)
        {
            this.encodetype = et;
        }
    }
}