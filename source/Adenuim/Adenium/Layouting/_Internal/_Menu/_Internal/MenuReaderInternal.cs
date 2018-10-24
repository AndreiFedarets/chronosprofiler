using System;
using System.IO;
using System.Xml;

namespace Adenium
{
    internal sealed class MenuReaderInternal
    {
        private const string MenuElementName = "Menu";
        private const string ControlElementName = "Control";

        private const string TypeAttributeName = "Type";
        private const string IdAttributeName = "Id";

        public IMenu ReadMenu(string menuXml, IContainer container)
        {
            using (StringReader stringReader = new StringReader(menuXml))
            {
                return ReadMenu(stringReader, container);
            }
        }

        public IMenu ReadMenu(TextReader reader, IContainer container)
        {
            using (XmlReader xmlReader = XmlReader.Create(reader))
            {
                return ReadMenu(xmlReader, container);
            }
        }

        public IMenu ReadMenu(XmlReader reader, IContainer container)
        {
            IMenu menu = ReadMenuElement(reader, container);

            //Read <Menu> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(MenuElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                IControl control = ReadControl(reader, container);
                if (control != null)
                {
                    menu.Add(control);
                }
            }

            return menu;
        }

        private IControl ReadControl(XmlReader reader, IContainer container)
        {
            //Move to <Control> element
            MoveToElement(reader, ControlElementName);

            string typeName = string.Empty;
            string id = string.Empty;

            //Read Control attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case TypeAttributeName:
                        typeName = reader.ReadContentAsString();
                        break;
                    case IdAttributeName:
                        id = reader.ReadContentAsString();
                        break;
                }
            }

            //Move back to <Control> element
            reader.MoveToElement();
            
            if (string.IsNullOrWhiteSpace(typeName) && string.IsNullOrWhiteSpace(id))
            {
                throw new TempException("Invalid menu control");
            }

            IControl control;

            if (!string.IsNullOrWhiteSpace(typeName))
            {
                Type type = Type.GetType(typeName, true);
                if (string.IsNullOrWhiteSpace(id))
                {
                    control = (IControl) container.Resolve(type);
                }
                else
                {
                    control = (IControl) Activator.CreateInstance(type, id);
                }
            } 
            else
            {
                control = new ControlStub(id);
            }

            IControlCollection controlCollection = control as IControlCollection;
            if (controlCollection != null)
            {
                ReadControlChildren(reader, controlCollection, container);
            }

            return control;
        }

        private void ReadControlChildren(XmlReader reader, IControlCollection parent, IContainer container)
        {
            if (reader.IsEmptyElement)
            {
                return;
            }
            //Read <Control> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(MenuElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                IControl control = ReadControl(reader, container);
                if (control != null)
                {
                    parent.Add(control);
                }
            }
        }

        private IMenu ReadMenuElement(XmlReader reader, IContainer container)
        {
            //Move to <Menu> element
            MoveToElement(reader, MenuElementName);

            string typeName = string.Empty;

            //Read Menu attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case TypeAttributeName:
                        typeName = reader.ReadContentAsString();
                        break;
                }
            }

            //Move back to <Menu> element
            reader.MoveToElement();

            if (string.IsNullOrEmpty(typeName))
            {
                typeName = typeof (Menu).GetFullyQualifiedName();
            }
            Type type = Type.GetType(typeName, true);
            IMenu menu = (IMenu) container.Resolve(type);
            return menu;
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
