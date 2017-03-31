using System;
using System.Runtime.InteropServices;

namespace Chronos.Win32
{
    public class UnmangedLibrary : IDisposable
    {
        private readonly string _libraryFullName;
        private IntPtr _library;

        public UnmangedLibrary(string libraryFullName)
        {
            _libraryFullName = libraryFullName;
        }

        protected UnmangedLibrary()
        {
            _libraryFullName = string.Empty;
        }

        ~UnmangedLibrary()
        {
            Dispose();
        }

        private IntPtr Library
        {
            get
            {
                if (_library == IntPtr.Zero)
                {
                    string libraryFullName = GetLibraryFullName();
                    _library = Kernel32.LoadLibrary(libraryFullName);
                    if (_library == IntPtr.Zero || _library == (IntPtr.Zero)-1)
                    {
                        throw new TempException();
                    }
                }
                return _library;
            }
        }

        public T GetFunction<T>(string functionName) where T : class
        {
            IntPtr procedurePointer = Kernel32.GetProcAddress(Library, functionName);
            if (procedurePointer == IntPtr.Zero)
            {
                return null;
            }
            Delegate procedure = Marshal.GetDelegateForFunctionPointer(procedurePointer, typeof(T));
            object procedureObject = procedure;
            return (T)procedureObject;
        }

        public void Dispose()
        {
            if (_library != IntPtr.Zero)
            {
                Kernel32.FreeLibrary(_library);
                _library = IntPtr.Zero;
            }
        }

        protected virtual string GetLibraryFullName()
        {
            return _libraryFullName;
        }
    }
}
