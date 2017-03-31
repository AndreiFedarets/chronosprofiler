using System.Text;

namespace ChronosBuildTool
{
    internal class Output : NotifyPropertyChanged, IOutput
    {
        private readonly StringBuilder _output;

        public Output()
        {
            _output = new StringBuilder();
        }

        public string Text
        {
            get
            {
                lock (_output)
                {
                    return _output.ToString();
                }
            }
        }

        public void Clear()
        {
            lock (_output)
            {
                _output.Clear();
                OnPropertyChanged(() => Text);
            }
        }

        public void WriteLine(string line)
        {
            lock (_output)
            {
                _output.AppendLine(line);
                OnPropertyChanged(() => Text);
            }
        }
    }
}
