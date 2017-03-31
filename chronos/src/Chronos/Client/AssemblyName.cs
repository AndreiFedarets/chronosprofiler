using System.IO;

namespace Chronos.Client
{
	public class AssemblyName
	{
		public AssemblyName(System.Reflection.AssemblyName assemblyName)
		{
			Name = Path.GetFileNameWithoutExtension(assemblyName.Name);
		}

        public AssemblyName(string assemblyName)
	    {
            Name = assemblyName;
	    }

		public AssemblyName()
		{
			
		}

		public string Name { get; set; }

	}
}
