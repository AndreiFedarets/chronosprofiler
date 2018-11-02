using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Adenium.Layouting
{
    internal class ViewModelLayout 
    {
        public ViewModelLayout(List<ViewModelAttachment> attachments, MenuCollection menus)
        {
            Attachments = new ReadOnlyCollection<ViewModelAttachment>(attachments);
            Menus = menus;
        }

        public ReadOnlyCollection<ViewModelAttachment> Attachments { get; private set; }

        public MenuCollection Menus { get; private set; }

        public ViewModelAttachment FindAttachment(string attachmentId)
        {
            return Attachments.FirstOrDefault(x => string.Equals(x.Id, attachmentId));
        }
    }
}
