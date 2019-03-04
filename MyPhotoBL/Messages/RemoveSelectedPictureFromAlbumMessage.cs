using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoBL.Messages
{
    public class RemoveSelectedPictureFromAlbumMessage
    {
        public Guid Id { get; set;}

        public RemoveSelectedPictureFromAlbumMessage(Guid id)
        {
            this.Id = id;
        }
    }
}
