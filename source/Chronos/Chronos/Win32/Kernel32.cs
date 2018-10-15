using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Chronos.Win32
{
    public static class Kernel32
    {
        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary", CharSet = CharSet.Auto, BestFitMapping = false, SetLastError = true)]
        private static extern IntPtr _LoadLibrary(string libraryPath);

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr _GetProcAddress(IntPtr library, string procedureName);

        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary", SetLastError = true)]
        private static extern bool _FreeLibrary(IntPtr library);

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = true)]
        private static extern void _CopyMemory(IntPtr dest, IntPtr src, int count);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", EntryPoint = "WaitNamedPipe", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool _WaitNamedPipe(string name, int timeout);

        [DllImport("kernel32.dll", EntryPoint = "QueryPerformanceFrequency", SetLastError = true)]
        private static extern bool _QueryPerformanceFrequency(out long frequency);

        [DllImport("kernel32.dll", EntryPoint = "QueryPerformanceCounter", SetLastError = true)]
        private static extern bool _QueryPerformanceCounter(out long counter);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Int32 WTSGetActiveConsoleSessionId();

        public static IntPtr LoadLibrary(string libraryPath)
        {
            IntPtr library = _LoadLibrary(libraryPath);
            if (IsZeroOrMinusOne(library))
            {
                VerifyLastError();   
            }
            return library;
        }

        public static IntPtr GetProcAddress(IntPtr library, string procedureName)
        {
            IntPtr procedure = _GetProcAddress(library, procedureName);
            if (IsZero(procedure))
            {
                VerifyLastError();   
            }
            return procedure;
        }

        public static bool FreeLibrary(IntPtr library)
        {
            bool result = _FreeLibrary(library);
            if (!result)
            {
                VerifyLastError();
            }
            return result;
        }

        public static bool WaitNamedPipe(string name, int timeout)
        {
            bool result = _WaitNamedPipe(name, timeout);
            return result;
        }

        public static void CopyMemory(IntPtr dest, IntPtr src, int count)
        {
            _CopyMemory(dest, src, count);
            VerifyLastError();
        }

        public static long QueryPerformanceFrequency()
        {
            long frequency;
            if (!_QueryPerformanceFrequency(out frequency))
            {
                VerifyLastError();
            }
            return frequency;
        }

        public static long QueryPerformanceCounter()
        {
            long counter;
            if (!_QueryPerformanceCounter(out counter))
            {
                VerifyLastError();
            }
            return counter;
        }

        public static bool IsZeroOrMinusOne(IntPtr handle)
        {
            return handle == IntPtr.Zero || handle == (IntPtr)(-1);
        }

        public static bool IsZero(IntPtr handle)
        {
            return handle == IntPtr.Zero;
        }

        private static void VerifyLastError()
        {
            int lastError = Marshal.GetLastWin32Error();
            if (lastError != 0)
            {
                throw new Win32Exception(lastError);
            }
        }
    }
}
