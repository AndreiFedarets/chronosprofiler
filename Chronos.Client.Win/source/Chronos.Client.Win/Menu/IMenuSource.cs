using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.Menu
{
    public interface IMenuSource
    {
        IMenu GetMenu(PageViewModel pageViewModel);
    }
}
