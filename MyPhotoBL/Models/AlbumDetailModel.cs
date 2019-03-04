using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoDb.Entities;

namespace MyPhotoBL.Models
{
    public class AlbumDetailModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public ICollection<PictureDetailModel> PictureCollection { get; set; } = new List<PictureDetailModel>();
    }
}
