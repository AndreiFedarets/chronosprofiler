using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using ChronosBuildTool.Model;

namespace ChronosBuildTool
{
    public class BuildResultToColorConverter : IValueConverter
    {
        private static readonly SolidColorBrush WarningColor;
        private static readonly SolidColorBrush ErrorColor;
        private static readonly SolidColorBrush SuccessColor;
        private static readonly SolidColorBrush UndefinedColor;

        static BuildResultToColorConverter()
        {
            WarningColor = new SolidColorBrush(Colors.Yellow);
            ErrorColor = new SolidColorBrush(Colors.Red);
            SuccessColor = new SolidColorBrush(Colors.Green);
            UndefinedColor = new SolidColorBrush(Colors.Gray);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BuildResult buildResult = value as BuildResult;
            if (buildResult == null)
            {
                return UndefinedColor;
            }
            if (buildResult.Errors.Count() > 0)
            {
                return ErrorColor;
            }
            if (buildResult.Warnings.Count() > 0)
            {
                return WarningColor;
            }
            return SuccessColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
