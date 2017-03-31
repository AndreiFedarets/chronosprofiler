using System;
using System.IO;
using Chronos.Marshaling;

namespace Chronos.Communication.Native
{
    internal class Invoker
    {
        private readonly Stream _sourceStream;
        private readonly MemoryStream _cacheStream;

        public Invoker(Stream sourceStream)
            : this(sourceStream, false)
        {
        }

        public Invoker(Stream sourceStream, bool useCache)
        {
            _sourceStream = sourceStream;
            if (useCache)
            {
                _cacheStream = new MemoryStream();
            }
        }

        private Stream OutputStream
        {
            get { return _cacheStream ?? _sourceStream; }
        }

        private Stream InputStream
        {
            get { return _sourceStream; }
        }

        public void Flush()
        {
            if (_cacheStream != null)
            {
                byte[] data = _cacheStream.ToArray();
                _cacheStream.SetLength(0);
                _sourceStream.Write(data, 0, data.Length);
            }
        }

        public void WriteResultCode(int result)
        {
            Int32Marshaler.Marshal(result, OutputStream);
        }

        public int ReadResultCode()
        {
            return Int32Marshaler.Demarshal(InputStream);
        }

        public void WriteOperationId(Guid operationId)
        {
            GuidMarshaler.Marshal(operationId, OutputStream);
        }

        public Guid ReadOperationId()
        {
            return GuidMarshaler.Demarshal(InputStream);
        }

        public void WriteArguments(object[] arguments)
        {
            byte[] data;
            using (MemoryStream stream = new MemoryStream())
            {
                foreach (object argument in arguments)
                {
                    MarshalingManager.Marshal(argument, stream);
                }
                data = stream.ToArray();
            }
            ByteArrayMarshaler.Marshal(data, OutputStream);
        }

        public object[] ReadArguments(Type[] signature)
        {
            object[] arguments = new object[signature.Length];
            byte[] data = ByteArrayMarshaler.Demarshal(InputStream);
            using (MemoryStream stream = new MemoryStream(data))
            {
                for (int i = 0; i < arguments.Length; i++)
                {
                    Type type = signature[i];
                    ITypeMarshaler marshaler = MarshalingManager.GetMarshaler(type);
                    arguments[i] = marshaler.DemarshalObject(stream);
                }
            }
            return arguments;
        }

        public void WriteResult(object result)
        {
            if (result == null)
            {
                return;
            }
            byte[] data;
            using (MemoryStream stream = new MemoryStream())
            {
                MarshalingManager.Marshal(result, stream);
                data = stream.ToArray();
            }
            ByteArrayMarshaler.Marshal(data, OutputStream);
        }

        public object ReadResult(Type returnType)
        {
            object result;
            byte[] data = ByteArrayMarshaler.Demarshal(InputStream);
            using (MemoryStream stream = new MemoryStream(data))
            {
                ITypeMarshaler marshaler = MarshalingManager.GetMarshaler(returnType);
                result = marshaler.DemarshalObject(stream);
            }
            return result;
        }
    }
}
