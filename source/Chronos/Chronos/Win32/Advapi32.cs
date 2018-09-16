using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace Chronos.Win32
{
    public static class Advapi32
    {
        [StructLayout(LayoutKind.Sequential)]
        private sealed class SERVICE_STATUS_PROCESS
        {
            [MarshalAs(UnmanagedType.U4)] public uint dwServiceType;
            [MarshalAs(UnmanagedType.U4)] public uint dwCurrentState;
            [MarshalAs(UnmanagedType.U4)] public uint dwControlsAccepted;
            [MarshalAs(UnmanagedType.U4)] public uint dwWin32ExitCode;
            [MarshalAs(UnmanagedType.U4)] public uint dwServiceSpecificExitCode;
            [MarshalAs(UnmanagedType.U4)] public uint dwCheckPoint;
            [MarshalAs(UnmanagedType.U4)] public uint dwWaitHint;
            [MarshalAs(UnmanagedType.U4)] public uint dwProcessId;
            [MarshalAs(UnmanagedType.U4)] public uint dwServiceFlags;
        }

        private const int ERROR_INSUFFICIENT_BUFFER = 0x7a;
        private const int SC_STATUS_PROCESS_INFO = 0;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool QueryServiceStatusEx(SafeHandle hService, int infoLevel, IntPtr lpBuffer, uint cbBufSize, out uint pcbBytesNeeded);

        public static Process GetServiceProcessId(ServiceController sc)
        {
            IntPtr statusProcessPtr = IntPtr.Zero;
            try
            {
                UInt32 dwBytesNeeded;
                // Call once to figure the size of the output buffer.
                QueryServiceStatusEx(sc.ServiceHandle, SC_STATUS_PROCESS_INFO, statusProcessPtr, 0, out dwBytesNeeded);
                if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
                {
                    // Allocate required buffer and call again.
                    statusProcessPtr = Marshal.AllocHGlobal((int) dwBytesNeeded);

                    if (QueryServiceStatusEx(sc.ServiceHandle, SC_STATUS_PROCESS_INFO, statusProcessPtr, dwBytesNeeded, out dwBytesNeeded))
                    {
                        SERVICE_STATUS_PROCESS serviceStatus = new SERVICE_STATUS_PROCESS();
                        Marshal.PtrToStructure(statusProcessPtr, serviceStatus);
                        int processId = (int)serviceStatus.dwProcessId;
                        return Process.GetProcessById(processId);
                    }
                }
            }
            finally
            {
                if (statusProcessPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(statusProcessPtr);
                }
            }
            return null;
        }
    }
}
