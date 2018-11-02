using System;

namespace Chronos.Common
{
    [Serializable]
    public abstract class UnitBase
    {
        private NativeUnitBase _nativeUnit;

        protected UnitBase(NativeUnitBase nativeUnit)
        {
            _nativeUnit = nativeUnit;
        }

        protected internal NativeUnitBase NativeUnit
        {
            get { return _nativeUnit; }
        }

        public ulong Uid
        {
            get { return NativeUnit.Uid; }
        }

        public ulong Id
        {
            get { return NativeUnit.Id; }
        }

        public ulong BeginLifetime
        {
            get { return NativeUnit.BeginLifetime; }
        }

        public ulong EndLifetime
        {
            get { return NativeUnit.EndLifetime; }
        }

        public string Name
        {
            get { return NativeUnit.Name; }
        }

        protected internal void Update(NativeUnitBase unitBase)
        {
            _nativeUnit = unitBase;
        }
    }
}
