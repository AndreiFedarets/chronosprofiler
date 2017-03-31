using System.IO;
using System.Xml;

namespace Chronos.Client.Win.Menu
{
    public static class MenuReader
    {
        private static readonly MenuReaderInternal Reader;

        static MenuReader()
        {
            Reader = new MenuReaderInternal();
        }

        public static IMenu ReadMenu(string menuXml, ResolutionDependencies dependencies = null)
        {
            IContainer container = CreateContainer(dependencies);
            return Reader.ReadMenu(menuXml, container);
        }

        public static IMenu ReadMenu(TextReader reader, ResolutionDependencies dependencies = null)
        {
            IContainer container = CreateContainer(dependencies);
            return Reader.ReadMenu(reader, container);
        }

        public static IMenu ReadMenu(XmlReader reader, ResolutionDependencies dependencies = null)
        {
            IContainer container = CreateContainer(dependencies);
            return Reader.ReadMenu(reader, container);
        }

        private static IContainer CreateContainer(ResolutionDependencies dependencies)
        {
            IContainer container = new Container();
            if (dependencies != null)
            {
                dependencies.RegisterInContainer(container);
            }
            return container;
        }
    }
}
