using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Chronos
{
    public class DisposableTracker : IDisposable
    {
        private static readonly List<DisposableTracker> Collection;
        private readonly IDisposable _disposable;
        private volatile bool _disposed;
        private StackTrace _createdStackTrace;
        private StackTrace _disposedStackTrace;

        static DisposableTracker()
        {
            Collection = new List<DisposableTracker>();
        }

        public DisposableTracker(IDisposable disposable)
        {
            _disposable = disposable;
            _disposed = false;
            _createdStackTrace = new StackTrace();
            lock (Collection)
            {
                Collection.Add(this);
            }
        }

        public void Dispose()
        {
            _disposed = true;
            _disposedStackTrace = new StackTrace();
            lock (Collection)
            {
                Collection.Remove(this);
            }
        }

        public void VerifyDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(_disposedStackTrace);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Resource Type: {0}", _disposable.GetType().ToString());
            builder.AppendLine();
            builder.AppendFormat("Created StackTrace: {0}{1}", Environment.NewLine, _createdStackTrace);
            builder.AppendLine();
            builder.AppendFormat("Disposed StackTrace: {0}{1}", Environment.NewLine, _disposedStackTrace);
            builder.AppendLine();
            return builder.ToString();
        }

        private static List<DisposableTracker> GetAliveTrackers()
        {
            List<DisposableTracker> collection;
            lock (Collection)
            {
                collection = new List<DisposableTracker>(Collection);
            }
            return collection;
        }

        internal static void TraceUndisposedResources()
        {
            if (LoggingProvider.Current.ShouldLog(TraceEventType.Warning))
            {
                List<DisposableTracker> collection = GetAliveTrackers();
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("Undisposed resources:");
                foreach (DisposableTracker tracker in collection)
                {
                    builder.AppendLine(tracker.ToString());
                }
                LoggingProvider.Current.Log(TraceEventType.Warning, builder.ToString());
            }
        }
    }
}
