using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoBL.Messages
{
    public class AddSelectedPictureToAlbum
    {
        public Guid Id { get; set; }

        public AddSelectedPictureToAlbum(Guid id)
        {
            this.Id = id;
        }
    }
}
