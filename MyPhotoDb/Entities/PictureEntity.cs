using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyPhotoDb.Interfaces;
using MyPhotoDb.Enums;

namespace MyPhotoDb.Entities
{
    public class PictureEntity : IEntity<PictureEntity>
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Source { get; set; }

        public string Name { get; set; }

        public DateTime? PhotoTakenDate { get; set; } = null;

        public string Description { get; set; }

        public EntityImageFormat Format { get; set; }

        public int ResolutionHeight { get; set; }

        public int ResolutionWidth { get; set; }

        public ICollection<PositionInPictureEntity> PictureObjectCollection { get; set; } = new List<PositionInPictureEntity>();

        public ICollection<AlbumEntity> AlbumCollection { get; set; } = new List<AlbumEntity>();

    }
}