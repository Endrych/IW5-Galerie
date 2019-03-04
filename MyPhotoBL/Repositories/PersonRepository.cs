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
    public class PersonRepository:IRepository<PersonDetailModel>
    {
        private readonly PersonMapper _mapper = new PersonMapper();

        public IEnumerable<PersonListModel> GetAllPersonListModels()
        {
            using (var context = new MyPhotoDbContext())
            {
                return context.Persons.AsEnumerable().Select(_mapper.MapToListModel).ToList();
            }
        }

        public PersonDetailModel GetById(Guid id)
        {
            using (var context = new MyPhotoDbContext())
            {
                return _mapper.MapToDetail(context.Persons.Find(id));
            }
        }

        public PersonDetailModel Insert(PersonDetailModel item)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = _mapper.MapToEntity(item);
                entity.Id = Guid.NewGuid();
                context.Persons.Add(entity);
                context.SaveChanges();
                return _mapper.MapToDetail(entity);
            }
        }

        public void Update(PersonDetailModel item)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = context.Persons.First(person => person.Id == item.Id);
                entity.FirstName = item.FirstName;
                entity.Description = item.Description;
                entity.LastName = item.LastName;
                context.SaveChanges();
            }
        }

        public void Remove(Guid id)
        {
            using (var context = new MyPhotoDbContext())
            {
                var entity = new PersonEntity() { Id = id };
                context.Pictures.Include(r => r.PictureObjectCollection).Where(x => x.PictureObjectCollection.Any(y => y.PictureObject.Id == id)).ToList();
                context.Persons.Attach(entity);
                context.Persons.Remove(entity);
                context.SaveChanges();
            }
        }
    }
}
