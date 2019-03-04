using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoDb.Entities;

namespace MyPhotoDb
{
    public class MyPhotoDbContext:DbContext
    {
        public IDbSet<AlbumEntity> Albums { get; set; }
        public IDbSet<ObjectEntity> Objects { get; set; }
        public IDbSet<PersonEntity> Persons { get; set; }
        public IDbSet<PictureEntity> Pictures { get; set; }
        public IDbSet<PositionInPictureEntity> PositionsInPicture { get; set; }

        public MyPhotoDbContext()
            : base("MyPhotoContext")
        {

        }

    }
}
