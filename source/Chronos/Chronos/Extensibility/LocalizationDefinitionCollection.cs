using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace Chronos.Extensibility
{
    public sealed class LocalizationDefinitionCollection : ReadOnlyCollection<LocalizationDefinition>
    {
        private CultureInfo _currentCulture;
        private LocalizationDefinition _currentDefinition;

        internal LocalizationDefinitionCollection(IList<LocalizationDefinition> list)
            : base(list)
        {
            _currentCulture = null;
            _currentDefinition = null;
        }

        public string CurrentName
        {
            get
            {
                InitializeCurrentDefinition();
                return _currentDefinition.Name;
            }
        }

        public string CurrentDescription
        {
            get
            {
                InitializeCurrentDefinition();
                return _currentDefinition.Description;
            }
        }

        public Bitmap CurrentIcon
        {
            get
            {
                InitializeCurrentDefinition();
                return _currentDefinition.Icon;
            }
        }

        public string CurrentIconUri
        {
            get
            {
                InitializeCurrentDefinition();
                return _currentDefinition.IconUri;
            }
        }

        public IEnumerable<CultureInfo> SupportedCultures
        {
            get { return Items.Select(x => x.Culture); }
        }

        private void InitializeCurrentDefinition()
        {
            CultureInfo currentCulture = CultureInfo.CurrentUICulture;
            if (_currentCulture == null || !currentCulture.Equals(_currentCulture) || _currentDefinition == null)
            {
                //Find Name value for current culture
                LocalizationDefinition definition = Items.FirstOrDefault(x => currentCulture.Equals(x.Culture));
                if (definition == null)
                {
                    definition = Items.FirstOrDefault(x => Constants.DefaultCulture.Equals(x.Culture));
                }
                //Find Name value for default culture
                if (definition == null && Items.Count > 0)
                {
                    definition = Items[0];
                }
                //There is no Name value
                if (definition == null)
                {
                    throw new TempException();
                }
                _currentCulture = currentCulture;
                _currentDefinition = definition;
            }
        }
    }
}
