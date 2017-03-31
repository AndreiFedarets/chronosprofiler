namespace Chronos.Daemon.Internal
{
    internal interface IWorker
    {
        void Enqueue(Task task);
    }
}
