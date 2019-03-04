using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MyPhotoBL.Interfaces;
using MyPhotoBL.Mappers;
using MyPhotoBL.Models;
using MyPhotoDb;
using MyPhotoDb.Entities;

namespace MyPhotoBL.Repositories
{
    public class PictureRepository : IRepository<PictureDetailModel>
    {
        private readonly PictureMapper _mapper = new PictureMapper();
        private readonly PositionInPictureRepository _positionInPictureRepository = new PositionInPictureRepository();

        public IEnumerable<PictureListModel> GetAllPictureListModels()
        {
            using (var context = new MyPhotoDbContext())
            {
                return context.Pictures.AsEnumerable().Select(_mapper.MapToListModel).ToList();
            }
        }

        public IEnumerable<PictureDetailModel> GetAllPictureModels()
        {
            using (var context = new MyPhotoDbContext())
            {
                return context.Pictures.AsEnumerable().Select(_mapper.MapToDetail).ToList();
            }
        }

        public PictureDetailModel GetById(Guid id)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = context.Pictures.Include(x => x.PictureObjectCollection).FirstOrDefault(x => x.Id == id);
                IList<PositionInPictureEntity> positions = new List<PositionInPictureEntity>();
                foreach (var positionInPictureEntity in entity.PictureObjectCollection)
                {
                    positions.Add(context.PositionsInPicture.Include(x => x.PictureObject)
                        .FirstOrDefault(x => x.Id == positionInPictureEntity.Id));
                }

                entity.PictureObjectCollection = positions;

                return _mapper.MapToDetail(entity);
            }
        }

        public PictureDetailModel Insert(PictureDetailModel item)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = _mapper.MapToEntity(item);
                entity.Id = Guid.NewGuid();
                context.Pictures.Add(entity);
                context.SaveChanges();
                return _mapper.MapToDetail(entity);
            }
        }

        public void Update(PictureDetailModel item)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = context.Pictures.Include(x=>x.PictureObjectCollection).First(picture => picture.Id == item.Id);
                entity.Name = item.Name;
                entity.Description = item.Description;
                entity.Format = item.Format;
                entity.PhotoTakenDate = item.PhotoTakenDate;
                entity.ResolutionHeight = item.ResolutionHeight;
                entity.ResolutionWidth = item.ResolutionWidth;
                foreach (var position in item.PositionCollection)
                {
                    if (position.Id == Guid.Empty || context.PositionsInPicture.Include(x => x.PictureObject).FirstOrDefault(x => x.Id == position.Id) == null)
                    {
                        var positionInPictureMapper = new PositionInPictureMapper();
                        var helper = position.PictureObject;

                        if (position.Id == Guid.Empty)
                        {
                            position.Id = Guid.NewGuid();
                        }
                        
                        position.PictureObject = null;
                        entity.PictureObjectCollection.Add(positionInPictureMapper.MapToEntity(position));
                        var personObject = context.Persons.FirstOrDefault(x=> x.Id == helper.Id);
                        if (personObject == null)
                        {
                            var objectInPicture = context.Objects.Find(helper.Id);
                            if (objectInPicture != null)
                            {
                                entity.PictureObjectCollection.First(x => x.Id == position.Id).PictureObject = objectInPicture;
                            }
                        }
                        else
                        {
                            entity.PictureObjectCollection.First(x => x.Id == position.Id).PictureObject = personObject;
                        }
                    }
                    else
                    {
                        _positionInPictureRepository.Update(position);
                    }

                }
                
                var removeCollecction = new List<PositionInPictureEntity>();
                foreach (var positionInPictureEntity in entity.PictureObjectCollection)
                {
                    if (item.PositionCollection.FirstOrDefault(x=>x.Id == positionInPictureEntity.Id) == null)
                    {
                        removeCollecction.Add(positionInPictureEntity);
                    }
                }

                foreach (var positionInPictureEntity in removeCollecction)
                {
                    entity.PictureObjectCollection.Remove(positionInPictureEntity);
                }
                context.SaveChanges();
            }
        }

        public void Remove(Guid id)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = context.Pictures.Include(x => x.PictureObjectCollection).FirstOrDefault(x=>x.Id == id);
                if (entity != null)
                {
                    context.Pictures.Remove(entity);
                }
                context.SaveChanges();
            }
        }

        public IEnumerable<PictureDetailModel> GetAllPictureModelsByPersonId(Guid id)
        {
            using (var context = new MyPhotoDbContext())
            {
                return context.Pictures
                    .Where(a => a.PictureObjectCollection.Any(b => (b.PictureObject is PersonEntity) &&  b.PictureObject.Id == id))
                    .AsEnumerable()
                    .Select(_mapper.MapToDetail).ToList(); ;
            }
        }

        public IEnumerable<PictureDetailModel> GetAllPictureModelsByObjectId(Guid id)
        {
            using (var context = new MyPhotoDbContext())
            {
                return context.Pictures
                    .Where(a => a.PictureObjectCollection.Any(b => (b.PictureObject is ObjectEntity) && b.PictureObject.Id == id))
                    .AsEnumerable()
                    .Select(_mapper.MapToDetail)
                    .ToList();
            }
        }
    }
}