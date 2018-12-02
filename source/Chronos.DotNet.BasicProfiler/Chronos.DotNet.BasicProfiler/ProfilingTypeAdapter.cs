using System.IO;
using Chronos.Communication.Native;
using Chronos.Marshaling;
using Chronos.Storage;

namespace Chronos.DotNet.BasicProfiler
{
    public class ProfilingTypeAdapter : IProfilingTypeAdapter, IManagedDataHandler
    {
        private readonly AppDomainCollection _appDomains;
        private readonly AssemblyCollection _assemblies;
        private readonly ModuleCollection _modules;
        private readonly ClassCollection _classes;
        private readonly FunctionCollection _functions;
        private readonly ThreadCollection _threads;
        private IDataStorage _storage;

        static ProfilingTypeAdapter()
        {
            MarshalingManager.RegisterMarshaler(new Marshaling.AppDomainInfoMarshaler());
            MarshalingManager.RegisterMarshaler(new Marshaling.AssemblyInfoMarshaler());
            MarshalingManager.RegisterMarshaler(new Marshaling.ModuleInfoMarshaler());
            MarshalingManager.RegisterMarshaler(new Marshaling.ClassInfoMarshaler());
            MarshalingManager.RegisterMarshaler(new Marshaling.FunctionInfoMarshaler());
            MarshalingManager.RegisterMarshaler(new Marshaling.ThreadInfoMarshaler());
        }

        public ProfilingTypeAdapter()
        {
            _appDomains = new AppDomainCollection();
            _assemblies = new AssemblyCollection();
            _modules = new ModuleCollection();
            _classes = new ClassCollection();
            _functions = new FunctionCollection();
            _threads = new ThreadCollection();
        }

        public void ConfigureForProfiling(ProfilingTypeSettings settings)
        {
        }

        public void StartProfiling(ProfilingTypeSettings settings)
        {
        }

        public void StopProfiling()
        {
        }

        public void LoadData()
        {
            _appDomains.Load(_storage);
            _assemblies.Load(_storage);
            _modules.Load(_storage);
            _classes.Load(_storage);
            _functions.Load(_storage);
            _threads.Load(_storage);
        }

        public void ReloadData()
        {
        }

        public void SaveData()
        {
            _appDomains.Save(_storage);
            _assemblies.Save(_storage);
            _modules.Save(_storage);
            _classes.Save(_storage);
            _functions.Save(_storage);
            _threads.Save(_storage);
        }

        public void AttachStorage(IDataStorage storage)
        {
            _storage = storage;
        }

        public void ExportServices(IServiceContainer container)
        {
            container.Register(_appDomains);
            container.Register(_assemblies);
            container.Register(_modules);
            container.Register(_classes);
            container.Register(_functions);
            container.Register(_threads);
        }

        public void ImportServices(IServiceContainer container)
        {
            
        }

        public bool HandlePackage(NativeArray array)
        {
            using (Stream stream = array.OpenRead())
            {
                while (stream.Position < stream.Length)
                {
                    UnitType unitType = (UnitType)Int32Marshaler.Demarshal(stream);
                    switch (unitType)
                    {
                        case UnitType.AppDomain:
                            AppDomainNativeInfo[] appDomains = MarshalingManager.Demarshal<AppDomainNativeInfo[]>(stream);
                            _appDomains.Update(appDomains);
                            break;
                        case UnitType.Assembly:
                            AssemblyNativeInfo[] assemblies = MarshalingManager.Demarshal<AssemblyNativeInfo[]>(stream);
                            _assemblies.Update(assemblies);
                            break;
                        case UnitType.Module:
                            ModuleNativeInfo[] modules = MarshalingManager.Demarshal<ModuleNativeInfo[]>(stream);
                            _modules.Update(modules);
                            break;
                        case UnitType.Class:
                            ClassNativeInfo[] classes = MarshalingManager.Demarshal<ClassNativeInfo[]>(stream);
                            _classes.Update(classes);
                            break;
                        case UnitType.Function:
                            FunctionNativeInfo[] functions = MarshalingManager.Demarshal<FunctionNativeInfo[]>(stream);
                            _functions.Update(functions);
                            break;
                        case UnitType.Thread:
                            ThreadNativeInfo[] threads = MarshalingManager.Demarshal<ThreadNativeInfo[]>(stream);
                            _threads.Update(threads);
                            break;
                        default:
                            throw new TempException("Unknown UnitType");
                    }
                }
            }
            return true;
        }

        public void Dispose()
        {
            
        }
    }
}
