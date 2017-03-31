using System;
using System.Collections.Generic;

namespace Chronos.Client.Win
{
    public interface IHostApplicationSelector : IEnumerable<Host.IApplication>
    {
        Host.IApplication SelectedApplication { get; set; }

        event EventHandler SelectionChanged;
    }
}
