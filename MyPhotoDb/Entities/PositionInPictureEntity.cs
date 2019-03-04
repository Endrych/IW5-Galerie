using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoDb.Entities.Base;
using MyPhotoDb.Interfaces;

namespace MyPhotoDb.Entities
{
    public class PositionInPictureEntity: IEntity<PositionInPictureEntity>
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public PictureObjectEntity PictureObject { get; set; }
    }
}
