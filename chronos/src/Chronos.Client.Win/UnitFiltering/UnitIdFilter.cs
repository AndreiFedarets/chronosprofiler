using System;
using System.ComponentModel;
using System.Globalization;
using Chronos.Core;

namespace Chronos.Client.Win.UnitFiltering
{
	internal class UnitIdFilter : IUnitFilter
	{
		private string _value;
		private readonly ICollectionView _view;

		public UnitIdFilter(ICollectionView view)
		{
			_view = view;
		}

		public string DisplayName
		{
			get { return "Id"; }
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
			UnitBase unit = (UnitBase)item;
			if (unit == default(UnitBase) || string.IsNullOrEmpty(_value))
			{
				return true;
			}
			return unit.Id.ToString(CultureInfo.InvariantCulture).IndexOf(_value, StringComparison.InvariantCultureIgnoreCase) >= 0;
		}

	}
}
