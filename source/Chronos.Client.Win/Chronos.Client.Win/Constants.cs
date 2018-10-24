namespace Chronos.Client.Win
{
    public class Constants
    {
        public static class ApplicationCodeName
        {
            public const string WinClient = "client.win";
        }

        public static class ViewModelId
        {
            public const string Home = "D8AA923A-285B-4BD0-B375-0C3351D49E20";
            public const string Start = "C05E0E01-4ACB-42AB-919E-C298BA99E76E";
            public const string Profiling = "3CCC4391-3A85-40A6-936E-868A055B7662";
            public const string OpenFile = "80FCFEC6-51DA-4B99-99F0-825ED90545EF";
            public const string Units = "A0090269-7848-4CFD-B6C4-940B9EA2FFAB";
        }

        public static class CoreProcessName
        {
            public const string Client = "Chronos.Client.Win.Application";
        }

        public static class CoreExecutableName
        {
            public const string Client = "Chronos.Client.Win.Application.exe";
        }

        public static class Message
        {
            public const uint BuildProfilingViewMenu = 0x00000003;
            public const uint ViewModelActivated = 0x00000001;
            public const uint ViewModelDeactivated = 0x00000002;
        }

        public static class Menus
        {
            public const string MainMenu = "Menu.Main";
            public const string ContextMenu = "Menu.Context";
            public const string ItemContextMenu = "Menu.Context.Item";
        }
    }
}
