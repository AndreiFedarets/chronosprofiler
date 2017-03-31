using System;
using System.Drawing;
using System.Globalization;

namespace Chronos.Extensibility
{
    public sealed class LocalizationDefinition
    {
        internal LocalizationDefinition(CultureInfo culture, string name, string description, string iconUri)
        {
            Culture = culture;
            Name = name;
            Description = description;
            IconUri = iconUri;
        }

        public CultureInfo Culture { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string IconUri { get; private set; }

        public Bitmap Icon
        {
            get
            {
                if (string.IsNullOrWhiteSpace(IconUri))
                {
                    return null;
                }
                throw new NotImplementedException();
            }
        }
    }
}
