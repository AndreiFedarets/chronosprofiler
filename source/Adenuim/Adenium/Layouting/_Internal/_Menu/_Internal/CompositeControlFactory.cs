using System;

namespace Adenium.Menu
{
    internal static class CompositeControlFactory
    {
        public static ICompositeControl Create(IMenuControl control)
        {
            ICompositeControl compositeControl;
            if (control is IMenuItem)
            {
                compositeControl = new CompositeMenuItem((IMenuItem)control);
            }
            else
            {
                throw new Exception("Unknown control type");
            }
            return compositeControl;
        }
    }
}
