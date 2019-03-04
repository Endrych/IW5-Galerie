using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MyPhotoDb.Interfaces;

namespace MyPhotoDb.Entities
{
    public class AlbumEntity: IEntity<AlbumEntity>
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Description { get; set; }

        public string Name { get; set; }

        public ICollection<PictureEntity> PictureCollection { get; set;  } = new List<PictureEntity>();
    }
}
