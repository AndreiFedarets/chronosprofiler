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
        private const string ViewTypeAttributeName = "ViewType";
        private const string ModeAttributeName = "Mode";
        private const string ActivationAttributeName = "Activation";
        private const string OrderAttributeName = "Order";

        public ViewModelLayout Read(string content, IViewModel viewModel, IContainer container)
        {
            using (StringReader stringReader = new StringReader(content))
            {
                return Read(stringReader, viewModel, container);
            }
        }

        public ViewModelLayout Read(TextReader reader, IViewModel viewModel, IContainer container)
        {
            using (XmlReader xmlReader = new XmlTextReader(reader))
            {
                return Read(xmlReader, viewModel, container);
            }
        }

        public ViewModelLayout Read(XmlReader reader, IViewModel viewModel, IContainer container)
        {
            //Move to <Layout> element
            MoveToElement(reader, LayoutElementName);

            List<ViewModelAttachment> attachments = new List<ViewModelAttachment>();
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
                        ViewModelAttachment attachment = ReadViewModelAttachment(reader, viewModel);
                        attachments.Add(attachment);
                        break;
                    case MenuElementName:
                        Menu menu = ReadMenu(reader, container);
                        menus.Add(menu);
                        break;
                }
            }

            ViewModelLayout layout = new ViewModelLayout(attachments, menus);
            return layout;
        }

        private Menu ReadMenu(XmlReader reader, IContainer container)
        {
            //Move to <Menu> element
            MoveToElement(reader, MenuElementName);

            string id = string.Empty;
            Type controlHandlerType = typeof(MenuControlHandlerDefault);

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

            IMenuControlHandler controlHandler = new MenuControlHandlerLazy(controlHandlerType, container);
            control.AttachHandler(controlHandler);

            //Read <Menu> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && string.Equals(MenuElementName, reader.Name))
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
                        MenuItem menuItem = ReadMenuItem(reader, container);
                        control.Add(menuItem);
                        break;
                }
            }

            return control;
        }

        private MenuItem ReadMenuItem(XmlReader reader, IContainer container)
        {
            //Move to <MenuItem> element
            MoveToElement(reader, MenuItemElementName);

            string id = string.Empty;
            Type controlHandlerType = typeof(MenuControlHandlerDefault);

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
            IMenuControlHandler controlHandler = new MenuControlHandlerLazy(controlHandlerType, container);
            control.AttachHandler(controlHandler);

            //Read <MenuItem> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && string.Equals(MenuItemElementName, reader.Name))
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
                        MenuItem menuItem = ReadMenuItem(reader, container);
                        control.Add(menuItem);
                        break;
                }
            }   

            return control;
        }

        private ViewModelAttachment ReadViewModelAttachment(XmlReader reader, IViewModel viewModel)
        {
            //Move to <ViewModel> element
            MoveToElement(reader, ViewModelElementName);

            string id = string.Empty;
            string typeName = string.Empty;
            ViewModelMode mode = ViewModelMode.Multiple;
            ViewModelActivation activation = ViewModelActivation.OnStartup;
            int order = 0;
            string viewType = string.Empty;

            //Read <ViewModel> attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case IdAttributeName:
                        id = reader.ReadContentAsString();
                        break;
                    case TypeAttributeName:
                        typeName = reader.ReadContentAsString();
                        break;
                    case ViewTypeAttributeName:
                        viewType = reader.ReadContentAsString();
                        break;
                    case ModeAttributeName:
                        mode = reader.ReadContentAsEnum<ViewModelMode>();
                        break;
                    case ActivationAttributeName:
                        activation = reader.ReadContentAsEnum<ViewModelActivation>();
                        break;
                    case OrderAttributeName:
                        order = reader.ReadContentAsInt();
                        break;
                }
            }

            IViewModelFactory viewModelFactory = CreateViewModelFactory(viewModel, typeName, mode);

            return new ViewModelAttachment(id, activation, order, viewType, viewModelFactory);
        }

        private IViewModelFactory CreateViewModelFactory(IViewModel viewModel, string typeName, ViewModelMode mode)
        {
            IViewModelFactory factory;
            if (mode == ViewModelMode.Single)
            {
                factory = new SingleViewModelFactory(viewModel, typeName);
            }
            else
            {
                factory = new MultiViewModelFactory(viewModel, typeName);
            }
            return factory;
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
