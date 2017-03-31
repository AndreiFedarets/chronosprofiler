using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChronosBuildTool.Model
{
    public class BuildResult
    {
        private readonly ObservableCollection<string> _errors;
        private readonly ObservableCollection<string> _warnings;

        public BuildResult()
        {
            _errors = new ObservableCollection<string>();
            _warnings = new ObservableCollection<string>();
        }

        public IEnumerable<string> Errors
        {
            get { return _errors; }
        }

        public IEnumerable<string> Warnings
        {
            get { return _warnings; }
        }

        public TimeSpan Elapsed { get; private set; }

        public void AddError(string message)
        {
            _errors.Add(message);
        }

        public void AddWarning(string message)
        {
            _warnings.Add(message);
        }

        internal void ProcessMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            if (IsWarning(message))
            {
                AddWarning(message);
            }
            else if (IsError(message))
            {
                AddError(message);
            }
        }

        private bool IsError(string message)
        {
            return message.Contains("error ");
        }

        private bool IsWarning(string message)
        {
            return message.Contains("warning ");
        }

        internal void SetTime(TimeSpan timeSpan)
        {
            Elapsed = timeSpan;
        }
    }
}
