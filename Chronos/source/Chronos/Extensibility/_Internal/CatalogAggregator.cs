using Chronos.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chronos.Extensibility
{
    internal sealed class CatalogAggregator
    {
        private readonly List<ExtensionDefinition> _extensions;
        private readonly List<FrameworkDefinition> _frameworks;
        private readonly List<ProfilingTypeDefinition> _profilingTypes;
        private readonly List<ProfilingTargetDefinition> _profilingTargets;
        private readonly List<ProductivityDefinition> _productivities;
        private readonly List<ApplicationExtensionDefinition> _applicationExtensions;
        private readonly List<AttachmentDefinition> _attachments;
        private readonly IExtensionReader _reader;
        private readonly IExtensionAssemblyResolver _assemblyResolver;

        public CatalogAggregator(IExtensionAssemblyResolver assemblyResolver)
        {
            _assemblyResolver = assemblyResolver;
            _reader = new GenericExtensionReader();
            _extensions = new List<ExtensionDefinition>();
            _frameworks = new List<FrameworkDefinition>();
            _profilingTypes = new List<ProfilingTypeDefinition>();
            _profilingTargets = new List<ProfilingTargetDefinition>();
            _productivities = new List<ProductivityDefinition>();
            _applicationExtensions = new List<ApplicationExtensionDefinition>();
            _attachments = new List<AttachmentDefinition>();
        }

        public Catalog GetCatalog()
        {
            Catalog catalog = new Catalog(_extensions, _frameworks, _profilingTypes, _profilingTargets, _productivities, _applicationExtensions);
            return catalog;
        }

        public void ReadExtension(string extensionPath)
        {
            ExtensionDefinition extensionDefinition = _reader.ReadExtension(extensionPath);
            _assemblyResolver.RegisterPath(extensionDefinition.BaseDirectory);
            _extensions.Add(extensionDefinition);
            _frameworks.AddRange(extensionDefinition.Frameworks);
            _profilingTypes.AddRange(extensionDefinition.ProfilingTypes);
            _profilingTargets.AddRange(extensionDefinition.ProfilingTargets);
            _productivities.AddRange(extensionDefinition.Productivities);
            _applicationExtensions.AddRange(extensionDefinition.ApplicationExtensions);
            _attachments.AddRange(extensionDefinition.Attachments);
        }

        public void ReadExtensions(IExtensionSettingsCollection extensionsSettings)
        {
            List<FileInfo> files = new List<FileInfo>();
            string xchronexPattern = string.Format("*{0}", Constants.XChronexExtension);
            string jchronexPattern = string.Format("*{0}", Constants.JChronexExtension);
            foreach (IExtensionSettings extensionSettings in extensionsSettings)
            {
                if (!extensionSettings.IsEnabled)
                {
                    continue;
                }
                DirectoryInfo directory = extensionSettings.GetDirectory();
                if (!directory.Exists)
                {
                    continue;
                }
                files.AddRange(directory.GetFiles(xchronexPattern, SearchOption.AllDirectories));
                files.AddRange(directory.GetFiles(jchronexPattern, SearchOption.AllDirectories));
            }
            foreach (FileInfo file in files)
            {
                ReadExtension(file.FullName);
            }
            MergeAttachments();
        }

        private void MergeAttachments()
        {
            foreach (AttachmentDefinition attachment in _attachments)
            {
                AttachmentDefinition temp = attachment;
                ProfilingTypeDefinition profilingType = _profilingTypes.FirstOrDefault(x => x.Uid == temp.TargetUid);
                if (profilingType != null)
                {
                    MergeExports(temp.Exports, profilingType.Exports);
                    MergeDependencies(temp.Dependencies, profilingType.Dependencies);
                    MergeAttributes(temp.Attributes, profilingType.Attributes);
                }

                ProfilingTargetDefinition profilingTarget = _profilingTargets.FirstOrDefault(x => x.Uid == temp.TargetUid);
                if (profilingTarget != null)
                {
                    MergeExports(temp.Exports, profilingTarget.Exports);
                    //MergeDependencies(temp.Dependencies, profilingTarget.Dependencies);
                }

                FrameworkDefinition framework = _frameworks.FirstOrDefault(x => x.Uid == temp.TargetUid);
                if (framework != null)
                {
                    MergeExports(temp.Exports, framework.Exports);
                    //MergeDependencies(temp.Dependencies, framework.Dependencies);
                }

                ProductivityDefinition productivity = _productivities.FirstOrDefault(x => x.Uid == temp.TargetUid);
                if (productivity != null)
                {
                    MergeExports(temp.Exports, productivity.Exports);
                    //MergeDependencies(temp.Dependencies, productivity.Dependencies);
                }
            }
        }

        private void MergeAttributes(AttributeDefinitionCollection source, AttributeDefinitionCollection target)
        {
            foreach (AttributeDefinition export in source)
            {
                target.Add(export);
            }
        }

        private void MergeExports(ExportDefinitionCollection source, ExportDefinitionCollection target)
        {
            foreach (ExportDefinition export in source)
            {
                target.Add(export);
            }
        }

        private void MergeDependencies(DependencyDefinitionCollection source, DependencyDefinitionCollection target)
        {
            foreach (DependencyDefinition dependency in source)
            {
                throw new NotImplementedException();
            }
        }

    }
}
