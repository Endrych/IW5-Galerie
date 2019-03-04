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

namespace MyPhotoBL.Repositories
{
    public class ObjectRepository : IRepository<ObjectDetailModel>
    {
        private readonly ObjectMapper _mapper = new ObjectMapper();


        public IEnumerable<ObjectListModel> GetObjectListModels()
        {
            using (var context = new MyPhotoDbContext())
            {
                return context.Objects.AsEnumerable().Select(_mapper.MapToListModel).ToList();
            }
        }

        public ObjectDetailModel GetById(Guid id)
        {
            using (var context = new MyPhotoDbContext())
            {
                return _mapper.MapToDetail(context.Objects.Find(id));
            }
        }

        public ObjectDetailModel Insert(ObjectDetailModel item)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = _mapper.MapToEntity(item);
                entity.Id = Guid.NewGuid();
                context.Objects.Add(entity);
                context.SaveChanges();
                return _mapper.MapToDetail(entity);
            }
        }

        public void Update(ObjectDetailModel item)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = context.Objects.First(obj => obj.Id == item.Id);
                entity.Name = item.Name;
                entity.Description = item.Description;
                context.SaveChanges();
            }
        }

        public void Remove(Guid id)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = new ObjectEntity() {Id = id};
                context.Pictures.Include(r => r.PictureObjectCollection).Where(x => x.PictureObjectCollection.Any(y => y.PictureObject.Id == id)).ToList();
                context.Objects.Attach(entity);
                context.Objects.Remove(entity);
                context.SaveChanges();
            }
        }
    }
}
