using System;
using System.IO;
using Chronos.Common.EventsTree;
using Chronos.Communication.Native;
using Chronos.Marshaling;
using Chronos.Storage;

namespace Chronos.DotNet.SqlProfiler
{
    public sealed class ProfilingTypeAdapter : IProfilingTypeAdapter, IManagedDataHandler
    {
        private readonly SqlQueryCollection _sqlQueries;
        private IDataStorage _storage;

        static ProfilingTypeAdapter()
        {
            MarshalingManager.RegisterMarshaler(new Marshaling.SqlQueryInfoMarshaler());
        }

        public ProfilingTypeAdapter()
        {
            _sqlQueries = new SqlQueryCollection();
        }

        public void ConfigureForProfiling(ProfilingTypeSettings settings)
        {
        }

        public void AttachStorage(IDataStorage storage)
        {
            _storage = storage;
        }

        public void LoadData()
        {
            _sqlQueries.Load(_storage);
        }

        public void ReloadData()
        {
        }

        public void SaveData()
        {
            _sqlQueries.Save(_storage);
        }

        public void StartProfiling(ProfilingTypeSettings settings)
        {
        }

        public void StopProfiling()
        {
        }

        public void ExportServices(IServiceContainer container)
        {
            container.Register(_sqlQueries);
        }

        public void ImportServices(IServiceContainer container)
        {
            IEventMessageBuilder messageBuilder = container.Resolve<IEventMessageBuilder>();
            ISqlQueryEventMessage sqlQueryEventMessage = new SqlQueryEventMessage(_sqlQueries);
            messageBuilder.RegisterMessage(Constants.EventType.SqlQuery, sqlQueryEventMessage);
        }

        public bool HandlePackage(NativeArray array)
        {
            using (Stream stream = array.OpenRead())
            {
                while (stream.Position < stream.Length)
                {
                    UnitType unitType = (UnitType) Int32Marshaler.Demarshal(stream);
                    switch (unitType)
                    {
                        case UnitType.SqlQuery:
                            SqlQueryNativeInfo[] sqlQueries = MarshalingManager.Demarshal<SqlQueryNativeInfo[]>(stream);
                            _sqlQueries.Update(sqlQueries);
                            break;
                        default:
                            throw new Exception("Unknown UnitType");
                    }
                }
            }
            return true;
        }
    }
}
