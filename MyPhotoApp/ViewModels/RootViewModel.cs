using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoApp.ViewModels.Base;
using MyPhotoBL.Interfaces;
using MyPhotoBL.Messages;
using MyPhotoBL.Repositories;

namespace MyPhotoApp.ViewModels
{
    public class RootViewModel:ViewModelBase
    {
        private readonly IMessenger messenger;
        private readonly PictureRepository _pictureRepository;


        public ViewModelBase SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                if (Equals(value, _selectedViewModel)) return;
                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }

        private ViewModelLocator _locator;
        private ViewModelBase _selectedViewModel;

        public RootViewModel(PictureRepository pictureRepository, IMessenger messenger, ViewModelLocator locator)
        {
            this._locator = locator;
            this.messenger = messenger;
            this.messenger.Register<ClosePictureDetail>(ShowMainViewModel);
            this.messenger.Register<SelectedPictureMessage>(OpenDetail);
            this.SelectedViewModel = locator.MainViewModel;
            _pictureRepository = pictureRepository;
        }

        private void OpenDetail(SelectedPictureMessage obj)
        {
            var model = _locator.PictureDetailViewModel;
            this.SelectedViewModel = model;
            model.Detail = _pictureRepository.GetById(obj.Id);

        }

        private void ShowMainViewModel(ClosePictureDetail message)
        {
            this.SelectedViewModel = _locator.MainViewModel;
        }

    }
}
