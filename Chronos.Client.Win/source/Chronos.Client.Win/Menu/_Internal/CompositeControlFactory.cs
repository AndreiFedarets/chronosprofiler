namespace Chronos.Client.Win.Menu
{
    internal static class CompositeControlFactory
    {
        public static ICompositeControl Create(IControl control)
        {
            ICompositeControl compositeControl;
            if (control is IMenuItem)
            {
                compositeControl = new CompositeMenuItem((IMenuItem)control);
            }
            else
            {
                throw new TempException("Unknown control type");
            }
            return compositeControl;
        }
    }
}
