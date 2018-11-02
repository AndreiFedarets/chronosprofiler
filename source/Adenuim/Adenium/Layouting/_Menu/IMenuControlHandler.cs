using System;

namespace Adenium.Layouting
{
    public interface IMenuControlHandler : IDisposable
    {
        void OnControlAttached(IMenuControl control);

        void OnViewModelAttached(IViewModel ownerViewModel);

        void OnAction();

        bool GetEnabled();

        bool GetVisible();

        string GetText();
    }
}
