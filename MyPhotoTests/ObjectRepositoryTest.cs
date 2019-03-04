using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MyPhotoBL.Models;
using MyPhotoBL.Repositories;
using MyPhotoDb;
using MyPhotoDb.Entities;
using Xunit;

namespace MyPhotoTests
{
    public class ObjectRepositoryTest
    {
        private readonly ObjectRepository _repository = new ObjectRepository();

        [Fact]
        public void TestInsert()
        {
            var model = new ObjectDetailModel()
            {
                Description = "Starodavna vyrezavana papezem pomazana kralem prosezena",
                Name = "Zidle",
            };
            var inserted = _repository.Insert(model);
            var fouded = _repository.GetById(inserted.Id);

            Assert.Equal(model.Description, inserted.Description);
            Assert.Equal(model.Name, inserted.Name);
            Assert.NotNull(inserted.Id);
            Assert.Equal(fouded.Id, inserted.Id);
            Assert.Equal(fouded.Name, inserted.Name);
            Assert.Equal(fouded.Description, inserted.Description);
        }

        [Fact]
        public void TestUpdate()
        {
            Guid id;
            using (var context = new MyPhotoDbContext())
            {
                id = context.Objects.Add(new ObjectEntity
                {
                    Description = "Starodavna drevena zidle",
                    Name = "zidle"
                }).Id;
                context.SaveChanges();
                var founded = _repository.GetById(id);
                Assert.Equal(founded.Id, id);
                Assert.Equal("Starodavna drevena zidle", founded.Description);
                Assert.Equal("zidle", founded.Name);
            }

            var _object = _repository.GetById(id);
            _object.Name = "stul";
            _repository.Update(_object);
            var founded_object = _repository.GetById(id);
            Assert.Equal(founded_object.Id, id);
            Assert.Equal("Starodavna drevena zidle", founded_object.Description);
            Assert.Equal("stul", founded_object.Name);

        }

        [Fact]
        public void TestRemove()
        {
            Guid id;
            using (var context = new MyPhotoDbContext())
            {
                id = context.Objects.Add(new ObjectEntity
                {
                    Description = "Starodavna zidle",
                    Name = "zidle",
                }).Id;
                context.SaveChanges();
                Assert.NotNull(context.Objects.Find(id));

            }
            _repository.Remove(id);
            using (var context = new MyPhotoDbContext())
            {
                Assert.Null(context.Objects.Find(id));
            }
        }

        [Fact]
        public void TestGetById()
        {
            Guid id;
            using (var context = new MyPhotoDbContext())
            {
                id = context.Objects.Add(new ObjectEntity
                {
                    Description = "Starodavna drevena zidle",
                    Name = "zidle"
                }).Id;
                context.SaveChanges();
            }

            var founded = _repository.GetById(id);
            Assert.Equal(founded.Id, id);
            Assert.Equal("Starodavna drevena zidle", founded.Description);
            Assert.Equal("zidle", founded.Name);
        }

        [Fact]
        public void TestGetObjectListModels()
        {
            List<ObjectEntity> entities;
            using (var context = new MyPhotoDbContext())
            {
                if (!context.Objects.Any())
                {
                    context.Objects.Add(new ObjectEntity
                    {
                        Description = "Starodavna drevena zidle",
                        Name = "zidle"
                    });
                    context.SaveChanges();
                }
                entities = context.Objects.ToList();
            }

            var _objects = _repository.GetObjectListModels().ToList();
            foreach (var entity in entities)
            {
                var _object = _objects.FirstOrDefault(a => a.Id == entity.Id);
                Assert.NotNull(_object);
                Assert.Equal(entity.Name, _object.Name);
            }
        }
    }
}

