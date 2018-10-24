namespace Chronos.Client.Win.DotNet.FindReference
{
    internal interface IEventSearch
    {
        bool CanFindPrevious { get; }

        bool CanFindNext { get; }

        void FindNext();

        void FindPrevious();
    }
}
