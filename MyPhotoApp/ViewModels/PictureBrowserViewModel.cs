using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using MyPhotoApp.Commands;
using MyPhotoApp.ViewModels.Base;
using MyPhotoBL.Messages;
using MyPhotoBL;
using MyPhotoBL.Enums;
using MyPhotoBL.Interfaces;
using MyPhotoBL.Models;
using MyPhotoBL.Repositories;
using MyPhotoDb.Enums;


namespace MyPhotoApp.ViewModels
{
    public class PictureBrowserViewModel : ViewModelBase
    {
        private readonly IMessenger messenger;

        private readonly PictureRepository pictureRepository;
        private readonly AlbumRepository albumRepository;

        public List<string> OrderByOptions { get; } = Enum.GetNames(typeof(PictureOrderByField)).ToList();

        public List<string> CompareRealtionOptions { get; } = Enum.GetNames(typeof(CompareRealtion)).ToList();
        //public IReadOnlyList<CompareRealtion> CompareRealtionOptions { get; } = CustomEnumConverter.GetValues<CompareRealtion>().ToArray();

        public PictureFilter Filter { get; set; }

        public List<string> AvailableFormats
        {
            get
            {
                var res = Filter.SourcePictureCollection.Select(p => p.Format).Distinct().ToList();
                res.Add(EntityImageFormat.None);
                return res.Select(f => Enum.GetName(typeof(EntityImageFormat), f)).ToList();
            }
        }

        public bool IsOrderDesc
        {
            get => Filter?.SortOrderDesc ?? false;
            set
            {
                if (value == Filter?.SortOrderDesc) return;
                Filter.SortOrderDesc = value;
                OnPropertyChanged();
                ReloadFiltered();
            }
        }

        public PictureOrderByField OrderByField
        {
            get => Filter?.OrderByField ?? PictureOrderByField.Name;
            set
            {
                if (value == Filter?.OrderByField) return;
                Filter.OrderByField = value;                
                OnPropertyChanged();
                ReloadFiltered();
            }
        }

        public EntityImageFormat FormatFilter
        {
            get => Filter?.FormatFilter ?? EntityImageFormat.None;
            set
            {
                if (value == Filter?.FormatFilter) return;
                Filter.FormatFilter = value;
                OnPropertyChanged();
                ReloadFiltered();
            }
        }

        public CompareRealtion WidthRelation
        {
            get => _widthRelation;
            set
            {
                _widthRelation = value;
                if (Filter?.ResolutionWidthFilter != null && Filter.ResolutionWidthFilter.Relation != _widthRelation)
                {
                    Filter.ResolutionWidthFilter.Relation = value;
                    ReloadFiltered();
                }
                OnPropertyChanged();
            }
        }

        public CompareRealtion HeightRelation
        {
            get => _heightRelation;
            set
            {
                _heightRelation = value;
                if (Filter?.ResolutionHeightFilter != null && Filter.ResolutionHeightFilter.Relation != _heightRelation)
                {
                    Filter.ResolutionHeightFilter.Relation = value;
                    ReloadFiltered();
                }
                OnPropertyChanged();
            }
        }

        public string WidthString
        {
            get => _widthString;
            set
            {
                _widthString = value;
                if (Filter != null && Filter.SetWidthResolutionFilter(ref _widthString))
                    ReloadFiltered();
                OnPropertyChanged();
            }
        }
        public string HeightString
        {
            get => _heightString;
            set
            {
                _heightString = value;
                if (Filter != null && Filter.SetHeightResolutionFilter(ref _heightString))
                    ReloadFiltered();
                OnPropertyChanged();
            }
        }

        public DateTime? FromDate
        {
            get => Filter?.FromDate;
            set
            {
                if (value == Filter?.FromDate) return;
                Filter.FromDate = value;
                OnPropertyChanged();
                ReloadFiltered();
            }
        }

        public DateTime? ToDate
        {
            get => Filter?.ToDate;
            set
            {
                if (value == Filter?.ToDate) return;
                Filter.ToDate = value;
                OnPropertyChanged();
                ReloadFiltered();
            }
        }

        public string TextFiletr
        {
            get => Filter?.TextFilter ?? string.Empty;
            set
            {
                if (value == Filter?.TextFilter) return;
                Filter.TextFilter = value;
                OnPropertyChanged();
                ReloadFiltered();
            }
        }

        public ObservableCollection<PictureListModel> PictureObservable
        {
            get => _pictureObservable;
            set
            {
                if (Equals(value, _pictureObservable)) return;
                _pictureObservable = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectPictureCommand { get; set; }
        public ICommand RemovePictureFromListViewCommand { get; set; }
        public ICommand AddPictureCommand { get; set; }

        public PictureListModel SelectedListModel { get; set; }

        private ObservableCollection<PictureListModel> _pictureObservable;
        private CompareRealtion _widthRelation = CompareRealtion.Equal;
        private CompareRealtion _heightRelation = CompareRealtion.Equal;
        private string _widthString = string.Empty;
        private string _heightString = string.Empty;

        public PictureBrowserViewModel(PictureRepository pictureRepository, AlbumRepository albumRepository, IMessenger messenger)
        {
            this.pictureRepository = pictureRepository;
            this.albumRepository = albumRepository;
            this.messenger = messenger;

            SelectPictureCommand = new RelayCommand(SelectPicture);
            RemovePictureFromListViewCommand = new RelayCommand(RemovePictureFromListView);
            AddPictureCommand = new RelayCommand(AddPicture);

            this.messenger.Register<SelectedPictureCollectionMessage>(LoadCollection);
            this.messenger.Register<AddSelectedPictureToAlbum>(AddSelectedPictureToAlbum);
            this.messenger.Register<DeletedPictureMessage>(DeletedPictureFromMessege);
            this.messenger.Register<RemoveSelectedPictureFromAlbumMessage>(RemoveSelectedPictureFromAlbum);

            Filter = new PictureFilter(pictureRepository.GetAllPictureModels().ToList());
            PictureObservable = new ObservableCollection<PictureListModel>();
            ReloadFiltered();
            FormatFilter = EntityImageFormat.None;
        }

        private void RemoveSelectedPictureFromAlbum(RemoveSelectedPictureFromAlbumMessage obj)
        {
            if (obj == null) return;
            if (SelectedListModel == null) return;

            var album = albumRepository.GetById(obj.Id);
            var picture = album.PictureCollection.FirstOrDefault(x => x.Id == SelectedListModel.Id);
            if (picture == null) return;
             
            album.PictureCollection.Remove(picture);
            albumRepository.Update(album);
            ReloadFiltered();
        }

        private void LoadCollection(SelectedPictureCollectionMessage message)
        {
            Filter.SourcePictureCollection = message.PictureCollection.ToList();
            OnPropertyChanged(nameof(AvailableFormats));
            ReloadFiltered();
        }

        private void AddSelectedPictureToAlbum(AddSelectedPictureToAlbum obj)
        {
            if (obj != null)
            {
                if (SelectedListModel != null)
                {
                    var album = albumRepository.GetById(obj.Id);
                    album.PictureCollection.Add(pictureRepository.GetById(SelectedListModel.Id));
                    albumRepository.Update(album);
                    ReloadFiltered();
                }
            }
        }

        private void ReloadFiltered()
        {
            PictureObservable.Clear();
            foreach (var item in Filter.GetFilteretPictureListModels())
                PictureObservable.Add(item);
        }

        private void SelectPicture()
        {
            if(SelectedListModel!= null)
            { 
                this.messenger.Send(new SelectedPictureMessage() { Id = SelectedListModel.Id });
            }
        }

        private void RemovePictureFromListView()
        {
            if (SelectedListModel != null)
            {
                var id = SelectedListModel.Id;
                pictureRepository.Remove(id);
                RemovePicture(id);
            }
        }

        private void DeletedPictureFromMessege(DeletedPictureMessage message)
        {
            RemovePicture(message.PictureId);
        }

        private void RemovePicture(Guid id)
        {
            Filter.SourcePictureCollection = Filter.SourcePictureCollection.Where(x => x.Id != id).ToList();
            PictureObservable.Remove(PictureObservable.First(x => x.Id == id));
            OnPropertyChanged(nameof(AvailableFormats));
        }

        private void AddPicture()
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog();
            const string filterBegin = "Image Files (";

            var dialogFilter = "";

            foreach (var x in Enum.GetNames(typeof(EntityImageFormat)).ToList())
            {
                dialogFilter += $"*.{x};";
            }

            fileDialog.Filter = $"{filterBegin}{dialogFilter})|{dialogFilter}";

            var result = fileDialog.ShowDialog();

            if (result != true) return;

            var picture = PiuctureLoader.FromFile(fileDialog.FileName);

            pictureRepository.Insert(picture);
            Filter.SourcePictureCollection.Add(picture);
            ReloadFiltered();
            OnPropertyChanged(nameof(AvailableFormats));
        }
    }
}
