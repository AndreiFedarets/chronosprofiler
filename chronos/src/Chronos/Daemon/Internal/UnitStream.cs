using System;
using System.IO.Pipes;
using Chronos.Communication.NamedPipe;
using Chronos.Core;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.Daemon.Internal
{
    internal class UnitStream<T> : IDisposable where T : UnitBase
    {
        private readonly IUnitCollection<T> _collection;
        private readonly NamedPipeServerStream _stream;

        public UnitStream(Guid daemonToken, IUnitCollection<T> collection)
        {
            _collection = collection;
            PipeSecurity security = NamedPipeServerStreamController.GetLowLevelPipeSecurity();
            _stream = new NamedPipeServerStream(PipeNameFormatter.GetDaemonServerUnitPipeName(daemonToken, collection.UnitType),
                            PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous | PipeOptions.WriteThrough, 0, 0, security);
            _stream.BeginWaitForConnection(BeginRead, null);
        }

        public bool IsConnected
        {
            get { return _stream.IsConnected; }
        }

        private void BeginRead(IAsyncResult asyncResult)
        {
            try
            {
                _stream.EndWaitForConnection(asyncResult);
            }
            catch
            {
                return;
            }
            while (IsConnected)
            {
                try
                {
                    T[] array = MarshalingManager.Demarshal<T[]>(_stream);
                    _collection.Update(array);
                }
                catch (Exception)
                {
                    if (!IsConnected)
                    {
                        //Pipe is closed - client disconected
                        return;
                    }
                    System.Diagnostics.Debugger.Break();
                }
            }
        }

        public void Dispose()
        {
            if (_stream.IsConnected)
            {
                _stream.Disconnect();
            }
            _stream.Close();
            _stream.Dispose();
        }
    }
}
