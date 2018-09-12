using System.IO;

namespace Chronos.Communication.Native
{
    public interface IClientStream
    {
        Stream Connect();
    }
}
