using System;

namespace Chronos.Common
{
    [Serializable]
    public sealed class UnitCollectionEventArgs<T> : EventArgs where T : UnitBase
    {
        public UnitCollectionEventArgs(T[] units)
        {
            Units = units;
        }

        public T[] Units { get; private set; }
    }
}
