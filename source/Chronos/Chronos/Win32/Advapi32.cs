using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
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

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int LogonUser(string userName, string domain, string password, int logonType, int logonProvider, ref IntPtr token);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr token, int impersonationLevel, ref IntPtr newToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("advapi32", CharSet = CharSet.Auto, SetLastError = true), SuppressUnmanagedCodeSecurity]
        public static extern Boolean OpenProcessToken(IntPtr processHandle, // handle to process
            Int32 desiredAccess, // desired access to process
            ref IntPtr tokenHandle); // handle to open access token

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CreateProcessAsUser(
            IntPtr token, String applicationName, String commandLine,
            IntPtr processAttributes, IntPtr threadAttributes,
            bool inheritHandle, UInt32 creationFlags, IntPtr environment,
            String currentDirectory, ref NativeMethods.StartupInfo startupInfo,
            out NativeMethods.ProcessInformation processInformation);


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
