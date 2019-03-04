using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoDb.Entities.Base;
using MyPhotoDb.Interfaces;

namespace MyPhotoDb.Entities
{
    public class ObjectEntity: PictureObjectEntity, IEntity<ObjectEntity>
    {
        public string Name { get; set; }

    }
}
