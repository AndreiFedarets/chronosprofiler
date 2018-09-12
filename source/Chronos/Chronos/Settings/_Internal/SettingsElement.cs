using System.Xml.Linq;

namespace Chronos.Settings
{
    internal abstract class SettingsElement
    {
        protected readonly XElement Element;

        protected SettingsElement(XElement element)
        {
            if (element == null)
            {
                throw new TempException();
            }
            Element = element;
        }
    }
}
