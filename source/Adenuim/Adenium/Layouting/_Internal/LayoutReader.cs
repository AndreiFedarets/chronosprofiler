using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Adenium.Layouting
{
    internal sealed class LayoutReader
    {
        private const string LayoutElementName = "Layout";
        private const string ViewModelElementName = "ViewModel";
        private const string MenuElementName = "Menu";
        private const string MenuItemElementName = "MenuItem";
        private const string IdAttributeName = "Id";
        private const string TypeAttributeName = "Type";
        private const string ModeAttributeName = "Mode";
        private const string ActivationAttributeName = "Activation";

        public ViewModelLayout Read(string content, IActivator activator)
        {
            using (StringReader stringReader = new StringReader(content))
            {
                return Read(stringReader, activator);
            }
        }

        public ViewModelLayout Read(TextReader reader, IActivator activator)
        {
            using (XmlReader xmlReader = new XmlTextReader(reader))
            {
                return Read(xmlReader, activator);
            }
        }

        public ViewModelLayout Read(XmlReader reader, IActivator activator)
        {
            //Move to <Layout> element
            MoveToElement(reader, LayoutElementName);

            List<ViewModelReference> viewModels = new List<ViewModelReference>();
            MenuCollection menus = new MenuCollection();

            //Read <Layout> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && string.Equals(LayoutElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case ViewModelElementName:
                        ViewModelReference viewModel = ReadViewModelReference(reader);
                        viewModels.Add(viewModel);
                        break;
                    case MenuElementName:
                        Menu menu = ReadMenu(reader, activator);
                        menus.Add(menu);
                        break;
                }
            }

            ViewModelLayout layout = new ViewModelLayout(viewModels, menus, activator);
            return layout;
        }

        private Menu ReadMenu(XmlReader reader, IActivator activator)
        {
            //Move to <Menu> element
            MoveToElement(reader, MenuElementName);

            string id = string.Empty;
            Type controlHandlerType = null;

            //Read <Menu> attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case IdAttributeName:
                        id = reader.ReadContentAsString();
                        break;
                    case TypeAttributeName:
                        string type = reader.ReadContentAsString();
                        controlHandlerType = Type.GetType(type);
                        break;
                }
            }

            Menu control = new Menu(id);

            IMenuControlHandler controlHandler = new MenuControlHandlerLazy(controlHandlerType, activator);
            control.AttachHandler(controlHandler);

            //Read <Menu> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && string.Equals(LayoutElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case MenuItemElementName:
                        MenuItem menuItem = ReadMenuItem(reader, activator);
                        control.Add(menuItem);
                        break;
                }
            }

            return control;
        }

        private MenuItem ReadMenuItem(XmlReader reader, IActivator activator)
        {
            //Move to <MenuItem> element
            MoveToElement(reader, MenuElementName);

            string id = string.Empty;
            Type controlHandlerType = null;

            //Read <MenuItem> attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case IdAttributeName:
                        id = reader.ReadContentAsString();
                        break;
                    case TypeAttributeName:
                        string type = reader.ReadContentAsString();
                        controlHandlerType = Type.GetType(type);
                        break;
                }
            }

            MenuItem control = new MenuItem(id);
            IMenuControlHandler controlHandler = new MenuControlHandlerLazy(controlHandlerType, activator);
            control.AttachHandler(controlHandler);

            //Read <MenuItem> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && string.Equals(LayoutElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case MenuItemElementName:
                        MenuItem menuItem = ReadMenuItem(reader, activator);
                        control.Add(menuItem);
                        break;
                }
            }

            return control;
        }

        private ViewModelReference ReadViewModelReference(XmlReader reader)
        {
            //Move to <ViewModel> element
            MoveToElement(reader, ViewModelElementName);

            string typeName = string.Empty;
            ViewModelMode mode = ViewModelMode.Multiple;
            ViewModelActivation activation = ViewModelActivation.OnStartup;

            //Read <ViewModel> attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case TypeAttributeName:
                        typeName = reader.ReadContentAsString();
                        break;
                    case ModeAttributeName:
                        mode = reader.ReadContentAsEnum<ViewModelMode>();
                        break;
                    case ActivationAttributeName:
                        activation = reader.ReadContentAsEnum<ViewModelActivation>();
                        break;
                }
            }

            return new ViewModelReference(typeName, mode, activation);
        }

        private void MoveToElement(XmlReader reader, string elementName)
        {
            if (string.Equals(reader.Name, elementName, StringComparison.InvariantCulture))
            {
                return;
            }
            if (reader.ReadToFollowing(elementName))
            {
                return;
            }
            throw new Exception();
        }

    }
}
