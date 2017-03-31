using System.Collections.Generic;
using System.Linq;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon
{
	public class RibbonMerger : IRibbonMerger
	{
		public IDocument Merge(IEnumerable<IDocument> documents)
		{
			return documents.First();
		}
	}
}
