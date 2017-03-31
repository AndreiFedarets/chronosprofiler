using System.IO;

namespace Chronos.Storage
{
	public interface ISessionStorage
	{
		void Delete();

		void WriteCallstack(uint id, Stream stream);

		Stream ReadCallstack(uint p);
	}
}
