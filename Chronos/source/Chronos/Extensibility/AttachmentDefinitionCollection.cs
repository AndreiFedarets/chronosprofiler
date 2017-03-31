using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chronos.Extensibility
{
    public sealed class AttachmentDefinitionCollection : ReadOnlyCollection<AttachmentDefinition>
    {
        internal AttachmentDefinitionCollection(IList<AttachmentDefinition> list)
            : base(list)
        {
        }

        public AttachmentDefinition this[Guid targetUid]
        {
            get
            {
                AttachmentDefinition definition = Items.FirstOrDefault(x => x.TargetUid == targetUid);
                if (definition == null)
                {
                    throw new AttachmentNotFoundException(targetUid);
                }
                return definition;
            }
        }

        public bool Contains(Guid targetUid)
        {
            return Items.Any(x => x.TargetUid == targetUid);
        }
    }
}
