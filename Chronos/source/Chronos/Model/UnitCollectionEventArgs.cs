using System;

namespace Chronos.Model
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
