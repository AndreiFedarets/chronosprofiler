using System.Windows;
using System.Windows.Media;

namespace Adenium
{
    public static class FontsAndColors
    {
        public static Brush LightMainBrush { get; private set; }
        public static Brush NormalMainBrush { get; private set; }
        public static Brush DarkMainBrush { get; private set; }
        public static Brush NeutralBrush { get; private set; }
        public static Brush DarkNeutralBrush { get; private set; }
        public static FontFamily FontFamily { get; private set; }
        //Content
        public static double ContentFontSize { get; private set; }
        public static FontWeight ContentFontWeight { get; private set; }
        //Title
        public static double TitleFontSize { get; private set; }
        public static FontWeight TitleFontWeight { get; private set; }
        //Colors
        public static Color LightMainColor { get; private set; }
        public static Color NormalMainColor { get; private set; }
        public static Color DarkMainColor { get; private set; }
        public static Color NeutralColor { get; private set; }
        public static Color DarkNeutralColor { get; private set; }

        static FontsAndColors()
        {
            LightMainColor = (Color)ColorConverter.ConvertFromString("#52c3ff");
            NormalMainColor = (Color)ColorConverter.ConvertFromString("#31b6fd"); //#31b6fd //#007DD1
            DarkMainColor = (Color)ColorConverter.ConvertFromString("#444444");
            NeutralColor = (Color)ColorConverter.ConvertFromString("#ffffff");
            DarkNeutralColor = (Color)ColorConverter.ConvertFromString("#e3e3e3");

            //LightMainColor = (Color) ColorConverter.ConvertFromString("#52c3ff");
            //NormalMainColor = (Color)ColorConverter.ConvertFromString("#f3f3f3");
            //DarkMainColor = (Color)ColorConverter.ConvertFromString("#999999");
            //NeutralColor = (Color)ColorConverter.ConvertFromString("#ffffff");

            LightMainBrush = new SolidColorBrush(LightMainColor);
            NormalMainBrush = new SolidColorBrush(NormalMainColor);
            DarkMainBrush = new SolidColorBrush(DarkMainColor);
            NeutralBrush = new SolidColorBrush(NeutralColor);
            DarkNeutralBrush = new SolidColorBrush(DarkNeutralColor);

            FontFamily = new FontFamily("Segoe UI"); //Calibri, Corbel, Verdana, Segoe UI

            ContentFontSize = 14.0;
            ContentFontWeight = FontWeights.Normal;

            TitleFontSize = 14.0;
            TitleFontWeight = FontWeights.Normal;
        }

    }
}
