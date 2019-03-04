using System;
using System.Collections.Generic;
using System.Linq;
using MyPhotoBL.Models;
using MyPhotoBL.Repositories;
using MyPhotoDb;
using MyPhotoDb.Entities;
using MyPhotoDb.Entities.Base;
using Xunit;

namespace MyPhotoTests
{
    public class PositionInPictureBLTest
    {
        private readonly PositionInPictureRepository _repository = new PositionInPictureRepository();

        [Fact]
        public void TestInsert()
        {
            var model = new PositionInPictureModel
            {
                Height = 10,
                Width = 10,
                X = 10,
                Y = 10,
                PictureObject = new PersonDetailModel
                {
                    Description = "testovaci osoba",
                    LastName = "john",
                    FirstName = "doe"
                }
            };
            var inserted = _repository.Insert(model);
            var founded = _repository.GetById(inserted.Id);

            Assert.NotNull(inserted.Id);
            Assert.Equal(founded.Id, inserted.Id);
            Assert.Equal(founded.Height, inserted.Height);
            Assert.Equal(founded.Width, inserted.Width);
            Assert.Equal(founded.X, inserted.X);
            Assert.Equal(founded.PictureObject.Id, inserted.PictureObject.Id);
            Assert.Equal(founded.PictureObject.Description, inserted.PictureObject.Description);
            Assert.True(founded.PictureObject is PersonDetailModel);
        }

        [Fact]
        public void TestUpdate()
        {
            Guid id;
            using (var context = new MyPhotoDbContext())
            {
                id = context.PositionsInPicture.Add(new PositionInPictureEntity
                    {
                        Height = 10,
                        Width = 10,
                        X = 10,
                        Y = 10
                    }
                ).Id;
                context.SaveChanges();
            }
            var pos = _repository.GetById(id);
            pos.Height = pos.Height + 10;
            _repository.Update(pos);
            var founded = _repository.GetById(pos.Id);
            Assert.NotNull(founded);
            Assert.Equal(founded.Id, pos.Id);
            Assert.Equal(founded.Height, pos.Height);
            Assert.Equal(founded.Width, pos.Width);
            Assert.Equal(founded.X, pos.X);
            Assert.Null(founded.PictureObject);

            pos.PictureObject = new ObjectDetailModel
            {
                Name = "test obj",
                Description = "test obj desc",
                PreviewFileName = "file ... "
            };
            _repository.Update(pos);
            founded = _repository.GetById(pos.Id);
            Assert.NotNull(founded);
            Assert.True(founded.PictureObject is ObjectDetailModel);
            Assert.Equal("test obj desc", founded.PictureObject.Description);
            Assert.Equal("test obj", ((ObjectDetailModel) founded.PictureObject).Name);

            pos = founded;

            pos.PictureObject.Description = "asd";
            _repository.Update(pos);
            founded = _repository.GetById(pos.Id);
            Assert.NotNull(founded);
            Assert.Equal(founded.Id, pos.Id);
            Assert.True(founded.PictureObject is ObjectDetailModel);
            Assert.Equal("asd", founded.PictureObject.Description);
            Assert.Equal("test obj", ((ObjectDetailModel)founded.PictureObject).Name);

            pos = founded;
            
            pos.PictureObject = new PersonDetailModel
            {
                Description = "testperson",
                FirstName = "pepa",
                LastName = "novak"
            };
            _repository.Update(pos);
            founded = _repository.GetById(pos.Id);
            Assert.NotNull(founded);
            Assert.Equal(founded.Id, pos.Id);
            Assert.True(founded.PictureObject is PersonDetailModel);
            Assert.Equal("pepa", ((PersonDetailModel) founded.PictureObject).FirstName);
        }

        [Fact]
        public void TestRemove()
        {
            Guid id;
            using (var context = new MyPhotoDbContext())
            {
                id = context.PositionsInPicture.Add(new PositionInPictureEntity
                    {
                        Height = 10,
                        Width = 10,
                        X = 10,
                        Y = 10,
                        PictureObject = new ObjectEntity
                        {
                            Description = "test object",
                            Name = "test obj"
                        }
                    }
                ).Id;
                context.SaveChanges();
            }
            _repository.Remove(id);
            using (var context = new MyPhotoDbContext())
            {
                Assert.Null(context.Albums.Find(id));
            }
        }

        [Fact]
        public void TestGetById()
        {
            Guid id;
            using (var context = new MyPhotoDbContext())
            {
                id = context.PositionsInPicture.Add(new PositionInPictureEntity
                    {
                        Height = 10,
                        Width = 10,
                        X = 10,
                        Y = 10,
                        PictureObject = new ObjectEntity
                        {
                            Description = "test object",
                            Name = "test obj"
                        }
                    }
                ).Id;
                context.SaveChanges();
            }
            var founded = _repository.GetById(id);
            Assert.Equal(founded.Id, id);
            Assert.Equal(10, founded.Height);
            Assert.Equal(10, founded.Width);
            Assert.Equal(10, founded.X);
            Assert.Equal("test object", founded.PictureObject.Description);
            Assert.True(founded.PictureObject is ObjectDetailModel);
        }

        [Fact]
        public void TestGetPositionsInPictureById()
        {
            Guid id;
            using (var context = new MyPhotoDbContext())
            {
                id = context.Pictures.Add(new PictureEntity
                {
                    Description = "testpic",
                    PictureObjectCollection = new List<PositionInPictureEntity>
                    {
                        new PositionInPictureEntity
                        {
                            Height = 10,
                            Width = 10,
                            X = 10,
                            Y = 10,
                            PictureObject = new ObjectEntity
                            {
                                Description = "test object",
                                Name = "test obj"
                            }
                        },
                        new PositionInPictureEntity
                        {
                            Height = 10,
                            Width = 10,
                            X = 50,
                            Y = 50,
                            PictureObject = new PersonEntity
                            {
                                Description = "test object",
                                FirstName = "honza",
                                LastName = "tester"
                            }
                        }
                    }
                }
                ).Id;
                context.SaveChanges();
            }

            var positions = _repository.GetPositionsInPictureById(id).OrderBy(p => p.X).ToList();
            Assert.Equal(2, positions.Count());
            Assert.True(positions[0].PictureObject is ObjectDetailModel);
            Assert.True(positions[1].PictureObject is PersonDetailModel);
        }
    }
}
