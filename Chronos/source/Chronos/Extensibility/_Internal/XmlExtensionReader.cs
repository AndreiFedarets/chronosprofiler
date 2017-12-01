using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Chronos.Extensibility
{
    internal sealed class XmlExtensionReader : IExtensionReader
    {
        private const string UidAttributeName = "Uid";
        private const string FrameworkUidAttributeName = "FrameworkUid";
        private const string ProfilingTypeUidAttributeName = "ProfilingTypeUid";
        private const string EntryPointAttributeName = "EntryPoint";
        private const string EntryPoint32AttributeName = "EntryPoint32";
        private const string EntryPoint64AttributeName = "EntryPoint64";
        private const string ApplicationAttributeName = "Application";
        private const string CultureAttributeName = "Culture";
        private const string IconUriAttributeName = "IconUri";
        private const string DependencyTypeAttributeName = "Type";
        private const string LoadBehaviorAttributeName = "LoadBehavior";
        private const string AttachmentTargetUidAttributeName = "TargetUid";
        private const string AttributeNameAttributeName = "Name";
        private const string AttributeValueAttributeName = "Value";
        private const string AttributeTypeAttributeName = "Type";

        private const string AttributeElementName = "Attribute";
        private const string ExtensionElementName = "Extension";
        private const string AttachmentElementName = "Attachment";
        private const string ProfilingTypeElementName = "ProfilingType";
        private const string ProfilingTargetElementName = "ProfilingTarget";
        private const string FrameworkElementName = "Framework";
        private const string ExportElementName = "Export";
        private const string DependencyElementName = "Dependency";
        private const string LocalizationElementName = "Localization";
        private const string NameElementName = "Name";
        private const string DescriptionElementName = "Description";
        private const string ProductivityElementName = "Productivity";
        private const string ApplicationExtensionElementName = "ApplicationExtension";

        public ExtensionDefinition ReadExtension(string extensionPath)
        {
            using (FileStream stream = new FileStream(extensionPath, FileMode.Open, FileAccess.Read))
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    string baseDirectory = Path.GetDirectoryName(extensionPath);
                    ExtensionDefinition definition = ReadExtension(reader, baseDirectory);
                    return definition;
                }
            }
        }

        private ExtensionDefinition ReadExtension(XmlReader reader, string baseDirectory)
        {
            //Move to <Extension> element
            MoveToElement(reader, ExtensionElementName);

            //Prepare Extension properties
            Guid uid = Guid.Empty;
            List<ProfilingTypeDefinition> profilingTypes = new List<ProfilingTypeDefinition>();
            List<ProfilingTargetDefinition> profilingTargets = new List<ProfilingTargetDefinition>();
            List<FrameworkDefinition> frameworks = new List<FrameworkDefinition>();
            List<ProductivityDefinition> productivities = new List<ProductivityDefinition>();
            List<ApplicationExtensionDefinition> applicationExtensions = new List<ApplicationExtensionDefinition>();
            List<LocalizationDefinition> localizations = new List<LocalizationDefinition>();
            List<AttachmentDefinition> attachments = new List<AttachmentDefinition>();

            //Read Extension attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case UidAttributeName:
                        uid = reader.ReadContentAsGuid();
                        break;
                }
            }

            //Move back to <Extension> element
            reader.MoveToElement();

            //Read <Extension> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(ExtensionElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case ProfilingTypeElementName:
                        ProfilingTypeDefinition profilingType = ReadProfilingType(reader, baseDirectory);
                        profilingTypes.Add(profilingType);
                        break;
                    case ProfilingTargetElementName:
                        ProfilingTargetDefinition profilingTarget = ReadProfilingTarget(reader, baseDirectory);
                        profilingTargets.Add(profilingTarget);
                        break;
                    case FrameworkElementName:
                        FrameworkDefinition framework = ReadFramework(reader, baseDirectory);
                        frameworks.Add(framework);
                        break;
                    case ProductivityElementName:
                        ProductivityDefinition productivity = ReadProductivity(reader, baseDirectory);
                        productivities.Add(productivity);
                        break;
                    case ApplicationExtensionElementName:
                        ApplicationExtensionDefinition applicationExtension = ReadApplicationExtension(reader, baseDirectory);
                        applicationExtensions.Add(applicationExtension);
                        break;
                    case LocalizationElementName:
                        LocalizationDefinition localization = ReadLocalization(reader);
                        localizations.Add(localization);
                        break;
                    case AttachmentElementName:
                        AttachmentDefinition attachment = ReadAttachment(reader, baseDirectory);
                        attachments.Add(attachment);
                        break;
                }
            }
            ExtensionDefinition extensionDefinition = new ExtensionDefinition(uid, baseDirectory, profilingTypes, profilingTargets, frameworks, productivities, applicationExtensions, attachments, localizations);
            return extensionDefinition;
        }

        private AttributeDefinition ReadAttribute(XmlReader reader)
        {
            //Move to <Attribute> element
            MoveToElement(reader, AttributeElementName);
            string name = string.Empty;
            string value = string.Empty;
            string type = string.Empty;

            //Read Attachment attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case AttributeNameAttributeName:
                        name = reader.ReadContentAsString();
                        break;
                    case AttributeValueAttributeName:
                        value = reader.ReadContentAsString();
                        break;
                    case AttributeTypeAttributeName:
                        type = reader.ReadContentAsString();
                        break;
                }
            }

            AttributeDefinition definition = new AttributeDefinition(name, value, type);
            return definition;
        }

        private AttachmentDefinition ReadAttachment(XmlReader reader, string baseDirectory)
        {
            //Move to <ProfilingType> element
            MoveToElement(reader, AttachmentElementName);
            Guid targetUid = Guid.Empty;
            List<ExportDefinition> exports = new List<ExportDefinition>();
            List<DependencyDefinition> dependencies = new List<DependencyDefinition>();
            List<AttributeDefinition> attributes = new List<AttributeDefinition>();

            //Read Attachment attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case AttachmentTargetUidAttributeName:
                        targetUid = reader.ReadContentAsGuid();
                        break;
                }
            }

            //Move back to <Attachment> element
            reader.MoveToElement();

            //Read <Attachment> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(AttachmentElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case ExportElementName:
                        ExportDefinition export = ReadExport(reader, baseDirectory);
                        exports.Add(export);
                        break;
                    case DependencyElementName:
                        DependencyDefinition dependency = ReadDependency(reader);
                        dependencies.Add(dependency);
                        break;
                    case AttributeElementName:
                        AttributeDefinition attribute = ReadAttribute(reader);
                        attributes.Add(attribute);
                        break;
                }
            }
            AttachmentDefinition definition = new AttachmentDefinition(targetUid, exports, dependencies, attributes);
            return definition;
        }

        private ProfilingTypeDefinition ReadProfilingType(XmlReader reader, string baseDirectory)
        {
            //Move to <ProfilingType> element
            MoveToElement(reader, ProfilingTypeElementName);

            //Prepare ProfilingType properties
            Guid uid = Guid.Empty;
            Guid frameworkUid = Guid.Empty;
            List<ExportDefinition> exports = new List<ExportDefinition>();
            List<DependencyDefinition> dependencies = new List<DependencyDefinition>();
            List<LocalizationDefinition> localizations = new List<LocalizationDefinition>();
            List<AttributeDefinition> attributes = new List<AttributeDefinition>();

            //Read ProfilingType attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case UidAttributeName:
                        uid = reader.ReadContentAsGuid();
                        break;
                    case FrameworkUidAttributeName:
                        frameworkUid = reader.ReadContentAsGuid();
                        break;
                }
            }

            //Move back to <ProfilingType> element
            reader.MoveToElement();

            //Read <ProfilingType> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(ProfilingTypeElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case ExportElementName:
                        ExportDefinition export = ReadExport(reader, baseDirectory);
                        exports.Add(export);
                        break;
                    case DependencyElementName:
                        DependencyDefinition dependency = ReadDependency(reader);
                        dependencies.Add(dependency);
                        break;
                    case LocalizationElementName:
                        LocalizationDefinition localization = ReadLocalization(reader);
                        localizations.Add(localization);
                        break;
                    case AttributeElementName:
                        AttributeDefinition attribute = ReadAttribute(reader);
                        attributes.Add(attribute);
                        break;
                }
            }
            ProfilingTypeDefinition definition = new ProfilingTypeDefinition(uid, frameworkUid, exports, dependencies, localizations, attributes);
            return definition;
        }

        private ProfilingTargetDefinition ReadProfilingTarget(XmlReader reader, string baseDirectory)
        {
            //Move to <ProfilingTarget> element
            MoveToElement(reader, ProfilingTargetElementName);

            //Prepare ProfilingTarget properties
            Guid uid = Guid.Empty;
            List<ExportDefinition> exports = new List<ExportDefinition>();
            List<LocalizationDefinition> localizations = new List<LocalizationDefinition>();
            List<AttributeDefinition> attributes = new List<AttributeDefinition>();

            //Read ProfilingTarget attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case UidAttributeName:
                        uid = reader.ReadContentAsGuid();
                        break;
                }
            }

            //Move back to <ProfilingTarget> element
            reader.MoveToElement();

            //Read <ProfilingTarget> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(ProfilingTargetElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case ExportElementName:
                        ExportDefinition export = ReadExport(reader, baseDirectory);
                        exports.Add(export);
                        break;
                    case LocalizationElementName:
                        LocalizationDefinition localization = ReadLocalization(reader);
                        localizations.Add(localization);
                        break;
                    case AttributeElementName:
                        AttributeDefinition attribute = ReadAttribute(reader);
                        attributes.Add(attribute);
                        break;
                }
            }

            ProfilingTargetDefinition definition = new ProfilingTargetDefinition(uid, exports, localizations, attributes);
            return definition;
        }

        private FrameworkDefinition ReadFramework(XmlReader reader, string baseDirectory)
        {
            //Move to <Framework> element
            MoveToElement(reader, FrameworkElementName);

            //Prepare Framework properties
            Guid uid = Guid.Empty;
            List<ExportDefinition> exports = new List<ExportDefinition>();
            List<LocalizationDefinition> localizations = new List<LocalizationDefinition>();
            List<AttributeDefinition> attributes = new List<AttributeDefinition>();

            //Read Framework attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case UidAttributeName:
                        uid = reader.ReadContentAsGuid();
                        break;
                }
            }

            //Move back to <Framework> element
            reader.MoveToElement();

            //Read <Framework> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(FrameworkElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case ExportElementName:
                        ExportDefinition export = ReadExport(reader, baseDirectory);
                        exports.Add(export);
                        break;
                    case LocalizationElementName:
                        LocalizationDefinition localization = ReadLocalization(reader);
                        localizations.Add(localization);
                        break;
                    case AttributeElementName:
                        AttributeDefinition attribute = ReadAttribute(reader);
                        attributes.Add(attribute);
                        break;
                }
            }

            FrameworkDefinition definition = new FrameworkDefinition(uid, exports, localizations, attributes);
            return definition;
        }

        private ProductivityDefinition ReadProductivity(XmlReader reader, string baseDirectory)
        {
            //Move to <Productivity> element
            MoveToElement(reader, ProductivityElementName);

            //Prepare Productivity properties
            Guid uid = Guid.Empty;
            Guid profilingTypeUid = Guid.Empty;
            List<ExportDefinition> exports = new List<ExportDefinition>();
            List<LocalizationDefinition> localizations = new List<LocalizationDefinition>();
            List<AttributeDefinition> attributes = new List<AttributeDefinition>();
            List<DependencyDefinition> dependencies = new List<DependencyDefinition>();

            //Read Productivity attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case UidAttributeName:
                        uid = reader.ReadContentAsGuid();
                        break;
                    case ProfilingTypeUidAttributeName:
                        profilingTypeUid = reader.ReadContentAsGuid();
                        break;
                }
            }

            //Move back to <Productivity> element
            reader.MoveToElement();

            //Read <Productivity> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(ProductivityElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case ExportElementName:
                        ExportDefinition export = ReadExport(reader, baseDirectory);
                        exports.Add(export);
                        break;
                    case LocalizationElementName:
                        LocalizationDefinition localization = ReadLocalization(reader);
                        localizations.Add(localization);
                        break;
                    case AttributeElementName:
                        AttributeDefinition attribute = ReadAttribute(reader);
                        attributes.Add(attribute);
                        break;
                    case DependencyElementName:
                        DependencyDefinition dependency = ReadDependency(reader);
                        dependencies.Add(dependency);
                        break;
                }
            }

            ProductivityDefinition definition = new ProductivityDefinition(uid, exports, dependencies, localizations, attributes);
            return definition;
        }

        private ApplicationExtensionDefinition ReadApplicationExtension(XmlReader reader, string baseDirectory)
        {
            //Move to <ApplicationExtension> element
            MoveToElement(reader, ApplicationExtensionElementName);

            //Prepare ApplicationExtension properties
            Guid uid = Guid.Empty;
            List<ExportDefinition> exports = new List<ExportDefinition>();
            List<LocalizationDefinition> localizations = new List<LocalizationDefinition>();
            List<AttributeDefinition> attributes = new List<AttributeDefinition>();

            //Read ApplicationExtension attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case UidAttributeName:
                        uid = reader.ReadContentAsGuid();
                        break;
                }
            }

            //Move back to <ApplicationExtension> element
            reader.MoveToElement();

            //Read <ApplicationExtension> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(ApplicationExtensionElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case ExportElementName:
                        ExportDefinition export = ReadExport(reader, baseDirectory);
                        exports.Add(export);
                        break;
                    case LocalizationElementName:
                        LocalizationDefinition localization = ReadLocalization(reader);
                        localizations.Add(localization);
                        break;
                    case AttributeElementName:
                        AttributeDefinition attribute = ReadAttribute(reader);
                        attributes.Add(attribute);
                        break;
                }
            }

            ApplicationExtensionDefinition definition = new ApplicationExtensionDefinition(uid, exports, localizations, attributes);
            return definition;
        }

        private ExportDefinition ReadExport(XmlReader reader, string baseDirectory)
        {
            //Move to <Export> element
            MoveToElement(reader, ExportElementName);

            //Prepare Export properties
            string application = string.Empty;
            string entryPoint = string.Empty;
            string entryPoint32 = string.Empty;
            string entryPoint64 = string.Empty;
            LoadBehavior loadBehavior = LoadBehavior.OnDemand;

            //Read Export attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case ApplicationAttributeName:
                        application = reader.ReadContentAsString();
                        break;
                    case EntryPointAttributeName:
                        entryPoint = reader.ReadContentAsString();
                        break;
                    case EntryPoint32AttributeName:
                        entryPoint32 = reader.ReadContentAsString();
                        break;
                    case EntryPoint64AttributeName:
                        entryPoint64 = reader.ReadContentAsString();
                        break;
                    case LoadBehaviorAttributeName:
                        loadBehavior = reader.ReadContentAsEnum<LoadBehavior>();
                        break;
                }
            }

            //Move back to <Export> element
            reader.MoveToElement();

            ExportDefinition definition = new ExportDefinition(application, baseDirectory, entryPoint, entryPoint32, entryPoint64, loadBehavior);
            return definition;
        }

        private DependencyDefinition ReadDependency(XmlReader reader)
        {
            //Move to <Dependency> element
            MoveToElement(reader, DependencyElementName);

            //Prepare Dependency properties
            Guid uid = Guid.Empty;
            DependencyType dependencyType = DependencyType.Required;

            //Read Dependency attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case UidAttributeName:
                        uid = reader.ReadContentAsGuid();
                        break;
                    case DependencyTypeAttributeName:
                        dependencyType = reader.ReadContentAsEnum<DependencyType>();
                        break;
                }
            }

            //Move back to <Export> element
            reader.MoveToElement();

            DependencyDefinition definition = new DependencyDefinition(uid, dependencyType);
            return definition;
        }

        private LocalizationDefinition ReadLocalization(XmlReader reader)
        {
            //Move to <Localization> element
            MoveToElement(reader, LocalizationElementName);

            //Prepare Localization properties
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            string name = string.Empty;
            string description = string.Empty;
            string iconUri = null;

            //Read Localization attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case CultureAttributeName:
                        cultureInfo = reader.ReadContentAsCultureInfo();
                        break;
                    case IconUriAttributeName:
                        iconUri = reader.ReadContentAsString();
                        break;
                }
            }

            //Move back to <Localization> element
            reader.MoveToElement();

            //Read <Localization> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(LocalizationElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case NameElementName:
                        name = reader.ReadElementContentAsString();
                        break;
                    case DescriptionElementName:
                        description = reader.ReadElementContentAsString();
                        break;
                }
            }

            LocalizationDefinition definition = new LocalizationDefinition(cultureInfo, name, description, iconUri);
            return definition;
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
