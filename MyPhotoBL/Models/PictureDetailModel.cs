using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoDb.Entities;
using MyPhotoDb.Enums;

namespace MyPhotoBL.Models
{
    public class PictureDetailModel
    {
        public Guid Id { get; set; }

        public string Source { get; set; }

        public string Name { get; set; }

        public DateTime? PhotoTakenDate { get; set; } = null;

        public string Description { get; set; }

        public EntityImageFormat Format { get; set; }

        public int ResolutionHeight { get; set; }

        public int ResolutionWidth { get; set; }

        public ICollection<PositionInPictureModel> PositionCollection { get; set; }
    }
}
