namespace Adenium.Layouting
{
    internal sealed class MenuControlHandlerDefault : MenuControlHandlerBase
    {
        public override bool GetEnabled()
        {
            return false;
        }

        public override bool GetVisible()
        {
            return false;
        }

        public override string GetText()
        {
            return string.Empty;
        }
    }
}
