using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoBL.Models;
using MyPhotoBL.Repositories;
using MyPhotoDb;
using MyPhotoDb.Entities;
using Xunit;

namespace MyPhotoTests
{
    public class PersonRepositoryTest
    {
        private readonly PersonRepository _repository = new PersonRepository();

        [Fact]
        public void TestInsert()
        {
            var model = new PersonDetailModel()
            {
                Description = "Test model for testing method",
                FirstName = "John",
                LastName = "Doe"

            };
            var inserted = _repository.Insert(model);
            var fouded = _repository.GetById(inserted.Id);

            Assert.Equal(model.Description, inserted.Description);
            Assert.Equal(model.FirstName, inserted.FirstName);
            Assert.Equal(model.LastName, inserted.LastName);
            Assert.NotNull(inserted.Id);
            Assert.Equal(fouded.Id, inserted.Id);
            Assert.Equal(fouded.FirstName, inserted.FirstName);
            Assert.Equal(fouded.LastName, inserted.LastName);
            Assert.Equal(fouded.Description, inserted.Description);
        }

        [Fact]
        public void TestUpdate()
        {
            Guid id;
            using (var context = new MyPhotoDbContext())
            {
                id = context.Persons.Add(new PersonEntity
                {
                    Description = "Skvela osoba",
                    FirstName = "Joe",
                    LastName = "NeSekac"
                }).Id;
                context.SaveChanges();
                var founded = _repository.GetById(id);
                Assert.Equal(founded.Id, id);
                Assert.Equal("Skvela osoba", founded.Description);
                Assert.Equal("Joe", founded.FirstName);
                Assert.Equal("NeSekac", founded.LastName);
            }

            var person = _repository.GetById(id);
            person.LastName = "Sekac";
            _repository.Update(person);    
            var founded_person = _repository.GetById(id);
            Assert.Equal(founded_person.Id, id);
            Assert.Equal("Skvela osoba", founded_person.Description);
            Assert.Equal("Joe", founded_person.FirstName);
            Assert.Equal("Sekac", founded_person.LastName);
           
        }

        [Fact]
        public void TestRemove()
        {
            Guid id;
            using (var context = new MyPhotoDbContext())
            {
                id = context.Persons.Add(new PersonEntity
                {
                    Description = "Skvela osoba",
                    FirstName = "Joe",
                    LastName = "Sekac"
                }).Id;
                context.SaveChanges();
                Assert.NotNull(context.Persons.Find(id));

            }
            _repository.Remove(id);
            using (var context = new MyPhotoDbContext())
            {
                Assert.Null(context.Persons.Find(id));
            }
        }

        [Fact]
        public void TestGetById()
        {
            Guid id;
            using (var context = new MyPhotoDbContext())
            {
                id = context.Persons.Add(new PersonEntity
                {
                    Description = "Skvela osoba",
                    FirstName = "Joe",
                    LastName = "Sekac"
                }).Id;
                context.SaveChanges();
            }

            var founded = _repository.GetById(id);
            Assert.Equal(founded.Id, id);
            Assert.Equal("Skvela osoba", founded.Description);
            Assert.Equal("Joe", founded.FirstName);
            Assert.Equal("Sekac", founded.LastName);
        }

        [Fact]
        public void TestGetPersonListModels()
        {
            List<PersonEntity> entities;
            using (var context = new MyPhotoDbContext())
            {
                if (!context.Persons.Any())
                {
                    context.Persons.Add(new PersonEntity()
                        {
                        Description = "Skvela osoba",
                            FirstName = "Joe",
                            LastName = "Sekac"
                    }
                    );
                    context.SaveChanges();
                }
                entities = context.Persons.ToList();
            }

            var persons = _repository.GetAllPersonListModels().ToList();
            foreach (var entity in entities)
            {
                var person = persons.FirstOrDefault(a => a.Id == entity.Id);
                Assert.NotNull(person);
                Assert.Equal( $"{entity.FirstName} {entity.LastName}", person.Name);
            }
        }
    }
}

