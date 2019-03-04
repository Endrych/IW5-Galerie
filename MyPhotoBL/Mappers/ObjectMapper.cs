using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoBL.Models;
using MyPhotoDb.Entities;

namespace MyPhotoBL.Mappers
{
    public class ObjectMapper
    {

        public ObjectListModel MapToListModel(ObjectEntity entity)
        {
            return new ObjectListModel
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public ObjectDetailModel MapToDetail(ObjectEntity entity)
        {
            return new ObjectDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        public ObjectEntity MapToEntity(ObjectDetailModel detail)
        {
            if (detail == null) return null;
            return new ObjectEntity
            {
                Id = detail.Id,
                Name = detail.Name,
                Description = detail.Description
            };
        }

    }
}
