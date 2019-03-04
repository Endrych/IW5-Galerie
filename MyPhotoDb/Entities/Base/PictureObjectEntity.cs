using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDb.Entities.Base
{
    public abstract class PictureObjectEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Description { get; set; }

        public string PreviewFileName { get; set; }
    }
}
