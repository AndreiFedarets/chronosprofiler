using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Text;

namespace Chronos.Win32
{
    public class NativeMethods
    {

        internal static class DataHandlerRouter
        {
            internal delegate bool HandlePackage(IntPtr data, int dataSize);

            [DllImport("Chronos.Agent.dll", EntryPoint = "DataHandlerRouter_Create", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Create([MarshalAs(UnmanagedType.FunctionPtr)]HandlePackage callback);

            [DllImport("Chronos.Agent.dll", EntryPoint = "DataHandlerRouter_Destroy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Destroy(IntPtr token);
        }

        public const UInt32 MaximumAllowed = 0x2000000;

        [DllImport("Chronos.Agent.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Alloc(int size);

        [DllImport("Chronos.Agent.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Free(IntPtr pointer);

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr handle);

        [DllImport("Wtsapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool WTSQueryUserToken(int sessionId, ref IntPtr token);

        [DllImport("userenv.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CreateEnvironmentBlock(ref IntPtr environment, IntPtr token, bool inherit);

        [DllImport("userenv.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DestroyEnvironmentBlock(IntPtr environment);

        public static StringDictionary ReadEnvironmentVariables(IntPtr environment)
        {
            StringDictionary environmentVariables = new StringDictionary();
            StringBuilder testData = new StringBuilder(string.Empty);
            unsafe
            {
                short* start = (short*)environment.ToPointer();
                bool done = false;
                short* current = start;
                while (!done)
                {
                    if ((testData.Length > 0) && (*current == 0) && (current != start))
                    {
                        String data = testData.ToString();
                        int index = data.IndexOf('=');
                        if (index == -1)
                        {
                            environmentVariables.Add(data, string.Empty);
                        }
                        else if (index == (data.Length - 1))
                        {
                            environmentVariables.Add(data.Substring(0, index), string.Empty);
                        }
                        else
                        {
                            environmentVariables.Add(data.Substring(0, index), data.Substring(index + 1));
                        }
                        testData.Length = 0;
                    }
                    if ((*current == 0) && (current != start) && (*(current - 1) == 0))
                    {
                        done = true;
                    }
                    if (*current != 0)
                    {
                        testData.Append((char)*current);
                    }
                    current++;
                }
            }
            return environmentVariables;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct StartupInfo
        {
            public Int32 Size;
            public String Reserved;
            public String Desktop;
            public String Title;
            public UInt32 X;
            public UInt32 Y;
            public UInt32 XSize;
            public UInt32 YSize;
            public UInt32 XCountChars;
            public UInt32 YCountChars;
            public UInt32 FillAttribute;
            public UInt32 Flags;
            public short ShowWindow;
            public short Reserved2;
            public IntPtr Reserved3;
            public IntPtr StdInput;
            public IntPtr StdOutput;
            public IntPtr StdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessInformation
        {
            public IntPtr Process;
            public IntPtr Thread;
            public UInt32 ProcessId;
            public UInt32 ThreadId;
        }
    }
}
