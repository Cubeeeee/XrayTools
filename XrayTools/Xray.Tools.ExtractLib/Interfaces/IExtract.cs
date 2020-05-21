using System;
using System.Collections.Generic;
using System.Text;
using Xray.Tools.ExtractLib.Extract.ExtractParms;

namespace Xray.Tools.ExtractLib.Interfaces
{
    public interface IExtract<T> where T:BaseParm
    {
        bool Check(String Txt,String Reg,T parm);
        String GetResult(String Txt,String Reg,T parm);
        List<String> GetResults(String Txt, String Reg, T parm);
        List<String> Split(String Txt, String Reg);
        String Replace(String Txt,String Reg,String RepStr);
    }
}
