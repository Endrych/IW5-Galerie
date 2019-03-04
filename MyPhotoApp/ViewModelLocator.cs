using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoApp.ViewModels;
using MyPhotoBL;
using MyPhotoBL.Repositories;

namespace MyPhotoApp
{
    public class ViewModelLocator
    {
        private readonly Messenger messenger = new Messenger();
        private readonly PictureRepository pictureRepository = new PictureRepository();
        private readonly AlbumRepository albumRepository = new AlbumRepository();
        private readonly ObjectRepository objectRepository = new ObjectRepository();
        private readonly PersonRepository personRepository = new PersonRepository();

        private MainViewModel _mainViewModel;
        public MainViewModel MainViewModel
        {
            get
            {
                if (_mainViewModel == null)
                {
                    _mainViewModel = CreateMainViewModel();
                }

                return _mainViewModel;
            }
        }

        private PictureDetailViewModel _pictrueDetailViewModel;
        public PictureDetailViewModel PictureDetailViewModel
        {
            get
            {
                if (_pictrueDetailViewModel == null)
                {
                    _pictrueDetailViewModel = CreatePictureDetailViewModel();
                }

                return _pictrueDetailViewModel;
            }
        }

        public RootViewModel RootViewModel => CreateRootViewModel();
        public PictureBrowserViewModel PictureBrowserViewModel => CreatePictureViewModel();

        private PictureBrowserViewModel CreatePictureViewModel()
        {
            return new PictureBrowserViewModel(pictureRepository, albumRepository, messenger);
        }

        private RootViewModel CreateRootViewModel()
        {
            return new RootViewModel(pictureRepository, this.messenger, this);
        }

        private MainViewModel CreateMainViewModel()
        {
            return new MainViewModel(pictureRepository, albumRepository, objectRepository, personRepository, messenger);
        }


        private PictureDetailViewModel CreatePictureDetailViewModel()
        {
            return new PictureDetailViewModel(pictureRepository, personRepository,objectRepository, messenger);
        }
    }
}
