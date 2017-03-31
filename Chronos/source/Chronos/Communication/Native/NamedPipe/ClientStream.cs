using Chronos.Win32;
using System.IO;
using System.IO.Pipes;

namespace Chronos.Communication.Native.NamedPipe
{
    public class ClientStream : IClientStream
    {
        private readonly string _pipeName;

        public ClientStream(string pipeName)
        {
            _pipeName = pipeName;
        }

        public Stream Connect()
        {
            VerifyServer();
            NamedPipeClientStream stream = new NamedPipeClientStream(".", _pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            stream.Connect();
            return stream;
        }

        private void VerifyServer()
        {
            string pipeNativeName = string.Format("\\\\.\\pipe\\{0}", _pipeName);
            bool result = Kernel32.WaitNamedPipe(pipeNativeName, 0);
            if (!result)
            {
                throw new TempException("Server pipe does not exist");
            }
        }
    }
}
