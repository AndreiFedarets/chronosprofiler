using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Chronos
{
    /// <summary>
    /// Represents process platform
    /// </summary>
    public static class ProcessPlatformDetector
    {
        //private const int PePointerOffset = 60;
        //private const int MachineOffset = 4;

        //[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //private static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

        ///// <summary>
        ///// Detects platform of specified process.
        ///// </summary>
        ///// <param name="fileFullName">Path and name of process to detect</param>
        ///// <returns>Process platform</returns>
        //public static ProcessPlatform DetectProcessPlatform(string fileFullName)
        //{
        //    byte[] data = new byte[4096];
        //    using (Stream stream = new FileStream(fileFullName, FileMode.Open, FileAccess.Read))
        //    {
        //        int read = stream.Read(data, 0, data.Length);
        //        if (read < PePointerOffset + MachineOffset)
        //        {
        //            return ProcessPlatform.Native;
        //        }
        //    }
        //    // dos header is 64 bytes, last element, long (4 bytes) is the address of the PE header
        //    int peHeaderAddress = BitConverter.ToInt32(data, PePointerOffset);
        //    int machineUint = BitConverter.ToUInt16(data, peHeaderAddress + MachineOffset);
        //    return (ProcessPlatform) machineUint;
        //}

        ///// <summary>
        ///// Detects platform on Windows
        ///// </summary>
        ///// <returns>Windows platform</returns>
        //public static ProcessPlatform DetectSystemPlatform()
        //{
        //    try
        //    {
        //        using (Process process = Process.GetCurrentProcess())
        //        {
        //            bool isWow64;
        //            IsWow64Process(process.Handle, out isWow64);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return ProcessPlatform.I386;
        //    }
        //    return ProcessPlatform.X64;
        //}

        //public static ProcessPlatform DetectProcessPlatform(int processId)
        //{
        //    try
        //    {
        //        Process process = Process.GetProcessById(processId);
        //        return DetectProcessPlatform(process.MainModule.FileName);
        //    }
        //    catch (Exception)
        //    {
        //        return ProcessPlatform.I386;
        //    }
        //}
    }
}