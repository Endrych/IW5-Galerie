using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoBL.Models;
using MyPhotoDb.Entities;

namespace MyPhotoBL.Mappers
{
    public class PersonMapper
    {
        public PersonDetailModel MapToDetail(PersonEntity entity)
        {
            return new PersonDetailModel
            {
                FirstName = entity.FirstName,
                Id = entity.Id,
                LastName = entity.LastName,
                Description = entity.Description
            };
        }

        public PersonEntity MapToEntity(PersonDetailModel detail)
        {
            return new PersonEntity
            {
                Id = detail.Id,
                FirstName = detail.FirstName,
                LastName = detail.LastName,
                Description = detail.Description
            };
        }

        public PersonListModel MapToListModel(PersonEntity entity)
        {
            return new PersonListModel
            {
                Id = entity.Id,
                Name = $"{entity.FirstName} {entity.LastName}"
            };
        }
       
    }
}
