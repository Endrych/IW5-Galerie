using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoBL.Messages
{
    public class DeletedPictureMessage
    {
        public DeletedPictureMessage(Guid pictureId)
        {
            PictureId = pictureId;
        }

        public Guid PictureId { get; set; }
    }
}
