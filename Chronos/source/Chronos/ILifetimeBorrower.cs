using System;

namespace Chronos
{
    public interface ILifetimeBorrower
    {
        Guid RegisterSponsor(ILifetimeSponsor sponsor);

        void UnregisterSponsor(Guid cookie);
    }
}
