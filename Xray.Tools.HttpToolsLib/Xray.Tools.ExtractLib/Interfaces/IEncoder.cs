namespace Xray.Tools.ExtractLib.Interfaces
{
    public interface IEncoder<T>
    {
        T Encode(T str,object parm);
    }
}
