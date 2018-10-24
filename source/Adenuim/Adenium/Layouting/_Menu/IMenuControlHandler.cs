using System;

namespace Adenium.Layouting
{
    public interface IMenuControlHandler : IDisposable
    {
        void OnControlAttached(IMenuControl control);

        void OnAction();

        bool GetEnabled();

        bool GetVisible();

        string GetText();
    }
}
