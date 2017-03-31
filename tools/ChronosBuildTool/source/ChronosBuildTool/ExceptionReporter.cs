using System;
using System.Windows;

namespace ChronosBuildTool
{
    internal static class ExceptionReporter
    {
        public static void ShowError(Exception exception)
        {
            MessageBox.Show(exception.ToString());
        }
    }
}
