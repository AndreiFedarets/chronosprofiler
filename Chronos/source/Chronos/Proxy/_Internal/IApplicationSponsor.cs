using System.Runtime.InteropServices;

namespace Chronos.Proxy
{
    [ComVisible(false)]
    public interface IApplicationSponsor
    {
        void Register(object obj);

        void Unregister(object obj);
    }
}
