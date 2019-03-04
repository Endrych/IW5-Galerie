using System;
using System.Collections.Generic;
using MyPhotoBL.Models;
using MyPhotoDb.Entities;

namespace MyPhotoBL.Mappers
{
    public class PictureMapper
    {
        private readonly PositionInPictureMapper _posMapper = new PositionInPictureMapper();

        public PictureListModel MapToListModel(PictureEntity entity)
        {
            return new PictureListModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Source = entity.Source
            };
        }

        public PictureDetailModel MapToDetail(PictureEntity entity)
        {
            var HelpVar =  new PictureDetailModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                PhotoTakenDate = entity.PhotoTakenDate,
                Description = entity.Description,
                Format = entity.Format,
                ResolutionHeight = entity.ResolutionHeight,
                ResolutionWidth = entity.ResolutionWidth,
                PositionCollection = new List<PositionInPictureModel>(),
                Source = entity.Source
            };
            foreach (var item in entity.PictureObjectCollection) {
                HelpVar.PositionCollection.Add(_posMapper.MapToModel(item));
            }
            return HelpVar;
        }

        public PictureEntity MapToEntity(PictureDetailModel detail)
        {
            var HelpVar = new PictureEntity()
            {
                Id = detail.Id,
                Name = detail.Name,
                PhotoTakenDate = detail.PhotoTakenDate,
                Description = detail.Description,
                Format = detail.Format,
                ResolutionHeight = detail.ResolutionHeight,
                ResolutionWidth = detail.ResolutionWidth,
                PictureObjectCollection = new List<PositionInPictureEntity>(),
                Source = detail.Source
            };
            foreach (var item in detail.PositionCollection) {
                HelpVar.PictureObjectCollection.Add(_posMapper.MapToEntity(item));
            }
            return HelpVar;
        }

        public PictureEntity MapToEntity(PictureListModel detail)
        {
            throw new NotImplementedException();
        }
    }
}