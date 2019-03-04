using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoDb.Entities.Base;
using MyPhotoDb.Interfaces;

namespace MyPhotoDb.Entities
{
    public class PersonEntity:PictureObjectEntity,IEntity<PersonEntity>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
