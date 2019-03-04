using System;
using System.Collections.Generic;
using System.Linq;
using MyPhotoBL.Models;
using MyPhotoBL.Models.Base;
using MyPhotoBL.Repositories;
using MyPhotoDb;
using MyPhotoDb.Entities;
using MyPhotoDb.Enums;
using Xunit;

namespace MyPhotoTests
{
    public class PictureRepositoryTest
    {
        private readonly PictureRepository _repository = new PictureRepository();

        [Fact]
        public void TestInsert()
        {
            var model = new PictureDetailModel()
            {
                Name = "Test picture",
                PhotoTakenDate = DateTime.Now,
                Description = "This is tests picture",
                Format = EntityImageFormat.Jpg,
                ResolutionHeight = 100,
                ResolutionWidth = 100,
                PositionCollection = new List<PositionInPictureModel>
                {
                    new PositionInPictureModel
                    {
                        Height = 10,
                        Width = 10,
                        X = 5,
                        Y = 5,
                        PictureObject = new ObjectDetailModel
                        {
                            Description = "Test object",
                            Name = "Object test name",
                            PreviewFileName = "Preview of test object"
                        }
                    }
                }
            };
            var inserted = _repository.Insert(model);
            var founded = _repository.GetById(inserted.Id);

            Assert.NotNull(inserted.Id);
            Assert.Equal(model.Name, inserted.Name);
            Assert.Equal(model.ResolutionHeight, inserted.ResolutionHeight);
            Assert.Equal(model.ResolutionWidth, inserted.ResolutionWidth);
            Assert.Equal(founded.Name, inserted.Name);
            Assert.Equal(founded.Description, inserted.Description);
            //Assert.Equal(founded.PositionCollection.Count, inserted.PositionCollection.Count);
            //Assert.Equal(founded.PositionCollection.First().Id, inserted.PositionCollection.First().Id);
        }

        [Fact]
        public void TestUpdate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestRemove()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestGetById()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestGetAllPictureListModels()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestGetAllPictureListModelsByObjectId()
        {
            var model = new PictureDetailModel()
            {
                Name = "Test picture",
                PhotoTakenDate = DateTime.Now,
                Description = "This is tests picture",
                Format = EntityImageFormat.Jpg,
                ResolutionHeight = 100,
                ResolutionWidth = 100,
                PositionCollection = new List<PositionInPictureModel>
                {
                    new PositionInPictureModel
                    {
                        Height = 10,
                        Width = 10,
                        X = 5,
                        Y = 5,
                        PictureObject = new ObjectDetailModel
                        {
                            Description = "Test object",
                            Name = "Object test name",
                            PreviewFileName = "Preview of test object"
                        }
                    }
                }
            };
            var inserted = _repository.Insert(model);
            var founded = _repository.GetAllPictureModelsByObjectId(inserted.Id);
            
            Assert.NotNull(founded);
            Assert.Equal(inserted.Id, founded.First().Id);
            Assert.Equal(inserted.Name, founded.First().Name);
        }

        [Fact]
        public void TestGetAllPictureListModelsByPersonId()
        {
            var model = new PictureDetailModel()
            {
                Name = "Test picture",
                PhotoTakenDate = DateTime.Now,
                Description = "This is tests picture",
                Format = EntityImageFormat.Jpg,
                ResolutionHeight = 100,
                ResolutionWidth = 100,
                PositionCollection = new List<PositionInPictureModel>
                {
                    new PositionInPictureModel
                    {
                        Height = 10,
                        Width = 10,
                        X = 5,
                        Y = 5,
                        PictureObject = new ObjectDetailModel
                        {
                            Description = "Test object",
                            Name = "Object test name",
                            PreviewFileName = "Preview of test object"
                        }
                    }
                }
            };
            var inserted = _repository.Insert(model);
            var founded = _repository.GetAllPictureModelsByPersonId(inserted.Id);

            Assert.NotNull(founded);
            Assert.Equal(inserted.Id, founded.First().Id);
            Assert.Equal(inserted.Name, founded.First().Name);
        }
    }
}