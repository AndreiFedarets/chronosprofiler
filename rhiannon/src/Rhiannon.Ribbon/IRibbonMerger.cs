using System.Collections.Generic;
using Rhiannon.Serialization.Xml;

namespace Rhiannon.Ribbon
{
	public interface IRibbonMerger
	{
		IDocument Merge(IEnumerable<IDocument> documents);
	}
}
