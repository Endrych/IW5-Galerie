using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MyPhotoBL.Mappers;
using MyPhotoBL.Models;
using MyPhotoBL.Repositories;
using MyPhotoDb;
using MyPhotoDb.Entities;
using Xunit;
using Xunit.Sdk;

namespace MyPhotoTests
{
    public class AlbumBlTest
    {
        private readonly AlbumRepository _repository = new AlbumRepository();
        private readonly PictureRepository _pictureRepository = new PictureRepository();

        [Fact]
        public void TestInsert()
        {
            var model = new AlbumDetailModel
            {
                Description = "Test model for testing method",
                Name = "Test Model"
            };
            var inserted = _repository.Insert(model);
            var fouded = _repository.GetById(inserted.Id);

            Assert.Equal(model.Description, inserted.Description);
            Assert.Equal(model.Name, inserted.Name);
            Assert.NotNull(inserted.Id);
            Assert.Equal(fouded.Id, inserted.Id);
            Assert.Equal(fouded.Name, inserted.Name);
            Assert.Equal(fouded.Description, inserted.Description);
            Assert.Equal(fouded.PictureCollection.Count, inserted.PictureCollection.Count);
        }

        [Fact]
        public void TestUpdate()
        {
            Guid id;
            var picIds = new List<Guid>();
            using (var context = new MyPhotoDbContext())
            {
                for (var i = 0; i < 4; i++)
                {
                    picIds.Add(context.Pictures.Add(new PictureEntity
                        {
                            Description = $"picture num {i}",
                            Name = $"picture num {i}",
                        }
                    ).Id);
                }
                id = context.Albums.Add(new AlbumEntity
                    {
                        Name = "album update test",
                        Description = "album update test desc"
                    }
                ).Id;
                context.SaveChanges();
            }
            picIds.Sort();
            var album = _repository.GetById(id);
            album.Description = album.Description + " updated";
            _repository.Update(album);
            var fouded = _repository.GetById(album.Id);
            Assert.NotNull(fouded);
            Assert.Equal(album.Id, fouded.Id);
            Assert.Equal(album.Name, fouded.Name);
            Assert.Equal(album.Description, fouded.Description);
            Assert.Equal(0, fouded.PictureCollection.Count);

            album = fouded;
            album.PictureCollection = _pictureRepository.GetAllPictureModels().Where(pic => picIds.Take(3).Contains(pic.Id)).ToList();
            Assert.Equal(3, album.PictureCollection.Count);
            _repository.Update(album);
            fouded = _repository.GetById(album.Id);
            Assert.NotNull(fouded);
            Assert.Equal(album.Id, fouded.Id);
            Assert.Equal(3, fouded.PictureCollection.Count);
            Assert.Equal(3, 
                album.PictureCollection.Join(
                    fouded.PictureCollection, 
                    a => a.Id, 
                    f => f.Id, 
                    (a, f) => a
                ).Count());

            album = fouded;
            var toremove = album.PictureCollection.First();
            album.PictureCollection.Remove(toremove);
            album.PictureCollection.Add(_pictureRepository.GetAllPictureModels().First(p => p.Id == picIds[3]));
            _repository.Update(album);
            fouded = _repository.GetById(album.Id);
            Assert.NotNull(fouded);
            Assert.Equal(album.Id, fouded.Id);
            Assert.Equal(3, fouded.PictureCollection.Count);
            Assert.DoesNotContain(fouded.PictureCollection, pic => pic.Id == toremove.Id);
            Assert.Equal(3,
                album.PictureCollection.Join(
                    fouded.PictureCollection,
                    a => a.Id,
                    f => f.Id,
                    (a, f) => a
                ).Count());
        }

        [Fact]
        public void TestRemove()
        {
            Guid id;
            using (var context = new MyPhotoDbContext())
            {
                id = context.Albums.Add(new AlbumEntity
                    {
                        Name = "album test",
                        Description = "album test desc"
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
                id = context.Albums.Add(new AlbumEntity
                    {
                        Name = "album test",
                        Description = "album test desc"
                    }
                ).Id;
                context.SaveChanges();
            }
            var founded = _repository.GetById(id);
            Assert.Equal(founded.Id, id);
            Assert.Equal("album test", founded.Name);
            Assert.Equal("album test desc", founded.Description);
        }

        [Fact]
        public void TestGetAlbumListModels()
        {
            List<AlbumEntity> entities;
            using (var context = new MyPhotoDbContext())
            {
                if (!context.Albums.Any())
                {
                    context.Albums.Add(new AlbumEntity
                        {
                            Name = "album test",
                            Description = "album test desc"
                        }
                    );
                    context.SaveChanges();
                }
                entities = context.Albums.ToList();
            }

            var albums = _repository.GetAlbumListModels().ToList();
            foreach (var entity in entities)
            {
                var album = albums.FirstOrDefault(a => a.Id == entity.Id);
                Assert.NotNull(album);
                Assert.Equal(entity.Name, album.Name);
            }
        }
    }
}
