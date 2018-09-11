using System;
using System.ComponentModel;

namespace Chronos.DotNet.BasicProfiler.Client.Win.UnitFiltering
{
    internal class UnitNameFilter : IUnitFilter
    {
        private string _value;
        private readonly ICollectionView _view;

        public UnitNameFilter(ICollectionView view)
        {
            _view = view;
        }

        public string DisplayName
        {
            get { return "Name"; }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                _view.Filter = FilterInternal;
            }
        }

        private bool FilterInternal(object item)
        {
            UnitBase unit = (UnitBase) item;
            if (unit == default(UnitBase) || string.IsNullOrEmpty(_value))
            {
                return true;
            }
            return unit.Name.IndexOf(_value, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}
