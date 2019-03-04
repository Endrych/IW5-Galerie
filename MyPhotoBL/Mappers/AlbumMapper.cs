using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MyPhotoBL.Models;
using MyPhotoBL.Repositories;
using MyPhotoDb.Entities;

namespace MyPhotoBL.Mappers
{
    public class AlbumMapper
    {
        private readonly PictureMapper _picMapper = new PictureMapper();

        public AlbumListModel MapToListModel(AlbumEntity entity)
        {
            return new AlbumListModel
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
        
        public AlbumDetailModel MapToDetail(AlbumEntity entity)
        {
            return new AlbumDetailModel
            {
                Id = entity.Id,
                Description = entity.Description,
                Name = entity.Name,
                PictureCollection = entity.PictureCollection.Select(_picMapper.MapToDetail).ToList()
            };
        }

        public AlbumEntity MapToEntity(AlbumDetailModel detail)
        {
            return new AlbumEntity()
            {
                Id = detail.Id,
                Name = detail.Name,
                Description = detail.Description,
                PictureCollection = detail.PictureCollection.Select(_picMapper.MapToEntity).ToList()
            };
        }
    }
}
