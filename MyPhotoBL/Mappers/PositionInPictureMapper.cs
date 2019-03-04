using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoBL.Models;
using MyPhotoBL.Models.Base;
using MyPhotoDb.Entities;
using MyPhotoDb.Entities.Base;

namespace MyPhotoBL.Mappers
{
    public class PositionInPictureMapper
    {
        private readonly ObjectMapper _objMapper = new ObjectMapper();
        private readonly PersonMapper _personMapper = new PersonMapper();

        public PositionInPictureModel MapToModel(PositionInPictureEntity entity)
        {
            return new PositionInPictureModel
            {
                Id = entity.Id,
                Height = entity.Height,
                Width = entity.Width,
                X = entity.X,
                Y = entity.Y,
                PictureObject = (entity.PictureObject != null)
                    ? MapPicObjEntityToModel(entity.PictureObject)
                    : null
            };
        }

        public PositionInPictureEntity MapToEntity(PositionInPictureModel model)
        {
            return new PositionInPictureEntity
            {
                Id = model.Id,
                Height = model.Height,
                Width = model.Width,
                X = model.X,
                Y = model.Y,
                PictureObject = MapPicObjModelToEntity(model.PictureObject)
            };
        }

        public PictureObjectEntity MapPicObjModelToEntity(PictureObjectDetailModel pictureObject)
        {
            if (pictureObject is PersonDetailModel model)
                return _personMapper.MapToEntity(model);
            return _objMapper.MapToEntity((ObjectDetailModel) pictureObject);
        }

        public PictureObjectDetailModel MapPicObjEntityToModel(PictureObjectEntity pictureObject)
        {
            if (pictureObject is PersonEntity entity)
                return _personMapper.MapToDetail(entity);
            return _objMapper.MapToDetail((ObjectEntity) pictureObject);
        }
    }
}
