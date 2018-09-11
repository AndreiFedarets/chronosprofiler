using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Chronos.Win32
{
    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    public sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeLibraryHandle() : base(true) { }

        protected override bool ReleaseHandle()
        {
            return Kernel32.FreeLibrary(handle);
        }
    }
}
