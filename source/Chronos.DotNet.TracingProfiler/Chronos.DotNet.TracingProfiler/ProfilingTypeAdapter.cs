using Chronos.Common.EventsTree;
using Chronos.Communication.Native;
using Chronos.DotNet.BasicProfiler;
using Chronos.Storage;

namespace Chronos.DotNet.TracingProfiler
{
    public class ProfilingTypeAdapter : IProfilingTypeAdapter
    {
        public void LoadData()
        {
        }

        public void ReloadData()
        {
        }

        public void SaveData()
        {
        }

        public void ConfigureForProfiling(Chronos.ProfilingTypeSettings settings)
        {
            ProfilingTypeSettings profilingTypeSettings = new ProfilingTypeSettings(settings);
            if (profilingTypeSettings.EnableExclusions)
            {
                profilingTypeSettings.Exclusions = ExclusionsProvider.GetDefaultExclusions();
            }
        }

        public void StartProfiling(Chronos.ProfilingTypeSettings settings)
        {
        }

        public void StopProfiling()
        {
        }

        public void AttachStorage(IDataStorage storage)
        {
            
        }

        public void ExportServices(IServiceContainer container)
        {
        }

        public void ImportServices(IServiceContainer container)
        {
            IFunctionCollection functions = container.Resolve<IFunctionCollection>();
            IThreadCollection threads = container.Resolve<IThreadCollection>();
            IEventMessageBuilder messageBuilder = container.Resolve<IEventMessageBuilder>();

            IManagedFunctionCallEventMessage functionCallEventMessage = new ManagedFunctionCallEventMessage(functions);
            messageBuilder.RegisterMessage(Constants.EventType.ManagedFunctionCall, functionCallEventMessage);

            IManagedToUnmanagedTransactionEventMessage managedToUnmanagedTransactionEventMessage = new ManagedToUnmanagedTransactionEventMessage();
            messageBuilder.RegisterMessage(Constants.EventType.ManagedToUnmanagedTransaction, managedToUnmanagedTransactionEventMessage);

            IUnmanagedToManagedTransactionEventMessage unmanagedToManagedTransactionEventMessage = new UnmanagedToManagedTransactionEventMessage();
            messageBuilder.RegisterMessage(Constants.EventType.UnmanagedToManagedTransaction, unmanagedToManagedTransactionEventMessage);

            IThreadCreateEventMessage threadCreateEventMessage = new ThreadCreateEventMessage(threads);
            messageBuilder.RegisterMessage(Constants.EventType.ThreadCreate, threadCreateEventMessage);

            IThreadDestroyEventMessage threadDestroyEventMessage = new ThreadDestroyEventMessage(threads);
            messageBuilder.RegisterMessage(Constants.EventType.ThreadDestroy, threadDestroyEventMessage);
        }

        public IDataHandler CreateDataHandler()
        {
            return null;
        }
    }
}
