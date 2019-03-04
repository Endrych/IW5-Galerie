using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MyPhotoBL.Interfaces;
using MyPhotoBL.Mappers;
using MyPhotoBL.Models;
using MyPhotoDb;
using MyPhotoDb.Entities;

namespace MyPhotoBL.Repositories
{
    public class AlbumRepository : IRepository<AlbumDetailModel>
    {
        private readonly AlbumMapper _mapper = new AlbumMapper();
        private readonly PictureMapper _picMapper = new PictureMapper();

        public IEnumerable<AlbumListModel> GetAlbumListModels()
        {
            using (var context = new MyPhotoDbContext())
            {
                return context.Albums.AsEnumerable().Select(_mapper.MapToListModel).ToList();
            }
        }
        
        public AlbumDetailModel GetById(Guid id)
        {
            using (var context = new MyPhotoDbContext())
            {
                return _mapper.MapToDetail(
                    context.Albums
                        .Where(a => a.Id == id)
                        .Include(a => a.PictureCollection)
                        .FirstOrDefault()
                    );
            }
        }

        public AlbumDetailModel Insert(AlbumDetailModel item)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = _mapper.MapToEntity(item);
                entity.Id = Guid.NewGuid();
                context.Albums.Add(entity);
                context.SaveChanges();
                return _mapper.MapToDetail(entity);
            }
        }

        public void Update(AlbumDetailModel item)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = context.Albums.Include(a => a.PictureCollection).First(album => album.Id == item.Id);
                entity.Name = item.Name;
                entity.Description = item.Description;
                entity.PictureCollection = item.PictureCollection
                    .Select(detailModel =>
                        {
                            var pictureEntity = _picMapper.MapToEntity(detailModel);
                            if (context.Pictures.Any(p => p.Id == pictureEntity.Id))
                                pictureEntity = context.Pictures.Find(pictureEntity.Id);
                            return pictureEntity;
                        }
                ).ToList();
                context.SaveChanges();
            }
        }

        public void Remove(Guid id)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = new AlbumEntity() { Id = id };
                context.Albums.Attach(entity);
                context.Albums.Remove(entity);
                context.SaveChanges();
            }
        }
    }
}
