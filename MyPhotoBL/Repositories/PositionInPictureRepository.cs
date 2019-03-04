using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoBL.Interfaces;
using MyPhotoBL.Mappers;
using MyPhotoBL.Models;
using MyPhotoDb;
using MyPhotoDb.Entities;
using MyPhotoDb.Entities.Base;

namespace MyPhotoBL.Repositories
{
    public class PositionInPictureRepository : IRepository<PositionInPictureModel>
    {
        private readonly PositionInPictureMapper _mapper = new PositionInPictureMapper();
        private readonly PersonRepository _personRepository = new PersonRepository();
        private readonly ObjectRepository _objRepository = new ObjectRepository();

        public IEnumerable<PositionInPictureModel> GetPositionsInPictureById(Guid pictureId)
        {
            using (var context = new MyPhotoDbContext())
            {
                return context.Pictures
                    .Include(pic => pic.PictureObjectCollection.Select(pos => pos.PictureObject))
                    .First(pic => pic.Id == pictureId)
                    .PictureObjectCollection
                    .Select(_mapper.MapToModel)
                    .ToList();
            }
        }

        public PositionInPictureModel GetById(Guid id)
        {
            using (var context = new MyPhotoDbContext())
            {
                return _mapper.MapToModel(
                    context.PositionsInPicture
                        .Where(p => p.Id == id)
                        .Include(p => p.PictureObject)
                        .FirstOrDefault()
                    );
            }
        }

        public PositionInPictureModel Insert(PositionInPictureModel item)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = _mapper.MapToEntity(item);
                entity.Id = Guid.NewGuid();
                context.PositionsInPicture.Add(entity);
                context.SaveChanges();
                return _mapper.MapToModel(entity);
            }
        }

        public void Update(PositionInPictureModel item)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = context.PositionsInPicture
                    .Include(p => p.PictureObject).First(p => p.Id == item.Id);
                entity.Id = item.Id;
                entity.Height = item.Height;
                entity.Width = item.Width;
                entity.X = item.X;
                entity.Y = item.Y;


                if (item.PictureObject != null && (entity.PictureObject != null && entity.PictureObject.Id == item.PictureObject.Id))
                {
                    switch (item.PictureObject)
                    {
                        case PersonDetailModel person: _personRepository.Update(person); break;
                        case ObjectDetailModel obj: _objRepository.Update(obj); break;
                    }
                    context.SaveChanges();
                }
                else
                {
                    var todel = entity.PictureObject;
                    switch (item.PictureObject)
                    {
                        case PersonDetailModel person:
                            entity.PictureObject = context.Persons.Find(_personRepository.Insert(person).Id);
                            break;
                        case ObjectDetailModel obj:
                            entity.PictureObject = context.Objects.Find(_objRepository.Insert(obj).Id);
                            break;
                    }
                    context.SaveChanges();
                    if (todel == null) return;
                    switch (todel)
                    {
                        case PersonEntity person: _personRepository.Remove(person.Id); break;
                        case ObjectEntity obj: _objRepository.Remove(obj.Id); break;
                    }
                }
            }
        }

        public void Remove(Guid id)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = new PositionInPictureEntity() {Id = id};
                context.PositionsInPicture.Attach(entity);
                context.PositionsInPicture.Remove(entity);
                context.SaveChanges();
            }
        }
    }
}
