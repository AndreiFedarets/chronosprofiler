using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ChronosBuildTool.Model
{
    internal class SolutionXmlReader
    {
        private const string SolutionElementName = "Solution";
        private const string BuildElementName = "Build";
        private const string PackageElementName = "Package";
        private const string SourceElementName = "Source";
        private const string ExternalsElementName = "Externals";
        private const string DependenciesElementName = "Dependencies";
        private const string SolutionDependencyElementName = "Solution";
        private const string FileDependencyElementName = "File";

        private const string SolutionNameAttributeName = "Name";
        private const string DebugAttributeName = "Debug";
        private const string ReleaseAttributeName = "Release";

        public Solution ReadSolution(SolutionCollection solutions, FileInfo solutionFile)
        {
            BuildFolder buildFolder = null;
            PackageFolder packageFolder = null;
            ExternalsFolder externalsFolder = null;
            List<SolutionDependency> dependencies = new List<SolutionDependency>();
            string name = string.Empty;
            string source = string.Empty;
            using (FileStream stream = solutionFile.OpenRead())
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    MoveToElement(reader, SolutionElementName);

                    //Read Element attributes
                    while (reader.MoveToNextAttribute())
                    {
                        switch (reader.Name)
                        {
                            case SolutionNameAttributeName:
                                name = reader.ReadContentAsString();
                                break;
                        }
                    }

                    //Move back to Element
                    reader.MoveToElement();

                    //Read Element content
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.EndElement &&
                            string.Equals(SolutionElementName, reader.Name))
                        {
                            break;
                        }
                        if (reader.NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }
                        switch (reader.Name)
                        {
                            case BuildElementName:
                                buildFolder = ReadBuildFolder(reader);
                                break;
                            case PackageElementName:
                                packageFolder = ReadPackageFolder(reader);
                                break;
                            case SourceElementName:
                                source = reader.ReadInnerXml();
                                break;
                            case ExternalsElementName:
                                externalsFolder = ReadExternalsFolder(reader);
                                break;
                            case DependenciesElementName:
                                ReadDependencies(reader, dependencies);
                                break;
                        }
                    }
                }
            }
            Solution solution = new Solution(solutions, solutionFile, name, source, buildFolder, packageFolder, externalsFolder, dependencies);
            return solution;
        }

        private void ReadDependencies(XmlReader reader, List<SolutionDependency> dependencies)
        {
            MoveToElement(reader, DependenciesElementName);

            //Read Element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(DependenciesElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case SolutionDependencyElementName:
                        SolutionDependency solutionDependency = ReadSolutionDependency(reader);
                        dependencies.Add(solutionDependency);
                        break;
                }
            }
        }

        private SolutionDependency ReadSolutionDependency(XmlReader reader)
        {
            MoveToElement(reader, SolutionDependencyElementName);

            string name = string.Empty;
            List<FileDependency> dependencies = new List<FileDependency>();

            //Read Element attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case SolutionNameAttributeName:
                        name = reader.ReadContentAsString();
                        break;
                }
            }

            //Move back to Element
            reader.MoveToElement();

            //Read Element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    string.Equals(SolutionDependencyElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case FileDependencyElementName:
                        FileDependency fileDependency = ReadFileDependency(reader);
                        dependencies.Add(fileDependency);
                        break;
                }
            }
            SolutionDependency solutionDependency = new SolutionDependency(name, dependencies);
            return solutionDependency;
        }

        private FileDependency ReadFileDependency(XmlReader reader)
        {
            MoveToElement(reader, FileDependencyElementName);

            string name = string.Empty;

            //Read Element attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case SolutionNameAttributeName:
                        name = reader.ReadContentAsString();
                        break;
                }
            }

            //Move back to Element
            reader.MoveToElement();

            FileDependency fileDependency = new FileDependency(name);
            return fileDependency;
        }

        private ExternalsFolder ReadExternalsFolder(XmlReader reader)
        {
            MoveToElement(reader, ExternalsElementName);

            string debug = string.Empty;
            string release = string.Empty;

            //Read Element attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case DebugAttributeName:
                        debug = reader.ReadContentAsString();
                        break;
                    case ReleaseAttributeName:
                        release = reader.ReadContentAsString();
                        break;
                }
            }

            //Move back to Element
            reader.MoveToElement();

            ExternalsFolder externalsFolder = new ExternalsFolder(debug, release);
            return externalsFolder;
        }
        
        private BuildFolder ReadBuildFolder(XmlReader reader)
        {
            MoveToElement(reader, BuildElementName);

            string debug = string.Empty;
            string release = string.Empty;

            //Read Element attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case DebugAttributeName:
                        debug = reader.ReadContentAsString();
                        break;
                    case ReleaseAttributeName:
                        release = reader.ReadContentAsString();
                        break;
                }
            }

            //Move back to Element element
            reader.MoveToElement();

            BuildFolder buildFolder = new BuildFolder(debug, release);
            return buildFolder;
        }

        private PackageFolder ReadPackageFolder(XmlReader reader)
        {
            MoveToElement(reader, PackageElementName);

            string debug = string.Empty;
            string release = string.Empty;

            //Read Element attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case DebugAttributeName:
                        debug = reader.ReadContentAsString();
                        break;
                    case ReleaseAttributeName:
                        release = reader.ReadContentAsString();
                        break;
                }
            }

            //Move back to Element element
            reader.MoveToElement();

            PackageFolder packageFolder = new PackageFolder(debug, release);
            return packageFolder;
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
