using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MyPhotoApp.Commands;
using MyPhotoApp.ViewModels.Base;
using MyPhotoBL;
using MyPhotoBL.Interfaces;
using MyPhotoBL.Messages;
using MyPhotoBL.Models;
using MyPhotoBL.Repositories;
using MyPhotoDb.Enums;

namespace MyPhotoApp.ViewModels
{
    public class PictureDetailViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly PictureRepository _pictureRepository;
        private PictureDetailModel _detail;
        public List<string> ImageFormatOptions { get; set; } = Enum.GetNames(typeof(EntityImageFormat)).ToList();

        private ObjectDetailModel _objectDetail;
        private PersonDetailModel _personDetail;
        private PositionInPictureModel _positionInPictureDetail;
        private ObservableCollection<PositionInPictureModel> _personsOnPhoto;
        private ObservableCollection<PersonListModel> _persons;
        private readonly PersonRepository _personRepository;
        private readonly ObjectRepository _objectRepository;
        private PositionInPictureModel _selectedPersonOnPhoto;
        private PersonListModel _selectedPerson;
        private ObservableCollection<ObjectListModel> _objects;
        private ObservableCollection<PositionInPictureModel> _objectsOnPhoto;
        private ObjectListModel _selectedObject;
        private PositionInPictureModel _selectedPositionInPictureModel;
        private PositionInPictureModel _selectedObjectOnPhoto1;
        private int _selectedTabIndex;
        private ObservableCollection<RectangleItem> _rectangleItems;

        public PositionInPictureModel PositionInPictureDetail
        {
            get => _positionInPictureDetail;
            set
            {
                if (Equals(value, _positionInPictureDetail)) return;
                _positionInPictureDetail = value;
                OnPropertyChanged();
            }
        }

        public ObjectDetailModel ObjectDetail
        {
            get => _objectDetail;
            set
            {
                if (Equals(value, _objectDetail)) return;
                _objectDetail = value;
                OnPropertyChanged();
            }
        }

        public PictureDetailModel Detail
        {
            get => _detail;
            set
            {
                if (Equals(value, _detail)) return;
                _detail = value;
                FillCollections();
                OnPropertyChanged();
            }
        }

        public PersonDetailModel PersonDetail
        {
            get => _personDetail;
            set
            {
                if (Equals(value, _personDetail)) return;
                _personDetail = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PositionInPictureModel> PersonsOnPhoto
        {
            get => _personsOnPhoto;
            set
            {
                if (Equals(value, _personsOnPhoto)) return;
                _personsOnPhoto = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PersonListModel> Persons
        {
            get => _persons;
            set
            {
                if (Equals(value, _persons)) return;
                _persons = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PositionInPictureModel> ObjectsOnPhoto
        {
            get => _objectsOnPhoto;
            set
            {
                if (Equals(value, _objectsOnPhoto)) return;
                _objectsOnPhoto = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ObjectListModel> Objects
        {
            get => _objects;
            set
            {
                if (Equals(value, _objects)) return;
                _objects = value;
                OnPropertyChanged();
            }
        }

        public PositionInPictureModel SelectedPersonOnPhoto
        {
            get => _selectedPersonOnPhoto;
            set
            {
                if (Equals(value, _selectedPersonOnPhoto)) return;
                _selectedPersonOnPhoto = value;
                RectangleItems.Clear();
                if (_selectedPersonOnPhoto != null)
                    RectangleItems.Add(new RectangleItem
                    {
                        Width = _selectedPersonOnPhoto.Width,
                        Height = _selectedPersonOnPhoto.Height,
                        X = _selectedPersonOnPhoto.X,
                        Y = _selectedPersonOnPhoto.Y
                    });
                OnPropertyChanged();
            }
        }

        public PositionInPictureModel SelectedObjectOnPhoto
        {
            get => _selectedObjectOnPhoto1;
            set
            {
                if (Equals(value, _selectedObjectOnPhoto1)) return;
                _selectedObjectOnPhoto1 = value;
                RectangleItems.Clear();
                if (_selectedObjectOnPhoto1 != null)
                    RectangleItems.Add(new RectangleItem
                    {
                        Width = _selectedObjectOnPhoto1.Width,
                        Height = _selectedObjectOnPhoto1.Height,
                        X = _selectedObjectOnPhoto1.X,
                        Y = _selectedObjectOnPhoto1.Y
                    });
                OnPropertyChanged();
            }
        }

        public PersonListModel SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                if (Equals(value, _selectedPerson)) return;
                _selectedPerson = value;
                OnPropertyChanged();
            }
        }

        public ObjectListModel SelectedObject
        {
            get => _selectedObject;
            set
            {
                if (Equals(value, _selectedObject)) return;
                _selectedObject = value;
                OnPropertyChanged();
            }
        }

        public PositionInPictureModel SelectedPositionInPictureModel
        {
            get => _selectedPositionInPictureModel;
            set
            {
                if (Equals(value, _selectedPositionInPictureModel)) return;
                _selectedPositionInPictureModel = value;
                OnPropertyChanged();
            }
        }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                SelectedPersonOnPhoto = null;
                SelectedObjectOnPhoto = null;
                SelectedPerson = null;
                SelectedObject = null;
                _selectedTabIndex = value;
            }
        }

        public ObservableCollection<RectangleItem> RectangleItems
        {
            get => _rectangleItems;
            set
            {
                if (Equals(value, _rectangleItems)) return;
                _rectangleItems = value;
                OnPropertyChanged();
            }
        }
        
        public int canvasWidth => (int)(Detail.ResolutionWidth);
        public int canvasHeight => (int)(Detail.ResolutionHeight);


        public ICommand SavePictureCommand { get; set; }
        public ICommand DeletePictureCommand { get; set; }
        public ICommand AddObjectPositionCommand { get; set; }
        public ICommand AddPersonPositionCommand { get; set; }
        public ICommand CloseDetailCommand { get; set; }
        public ICommand ShowObjectDetailCommand { get; set; }
        public ICommand ShowPersonDetailCommand { get; set; }
        public ICommand AddPersonToPhotoCommand { get; set; }
        public ICommand RemovePersonFromPhotoCommand { get; set; }
        public ICommand RemoveObjectFromPhotoCommand { get; set; }
        public ICommand AddObjectToPhotoCommand { get; set; }

        public PictureDetailViewModel(PictureRepository pictureRepository, PersonRepository personRepository, ObjectRepository objectRepository, IMessenger messenger)
        {
            this._pictureRepository = pictureRepository;
            this._personRepository = personRepository;
            this._objectRepository = objectRepository;
            this._messenger = messenger;

            DeletePictureCommand = new RelayCommand(DeletePicture);
            SavePictureCommand = new RelayCommand(SavePicture);
            ShowPersonDetailCommand = new RelayCommand(ShowPersonDetail);
            ShowObjectDetailCommand = new RelayCommand(ShowObjectDetail);
            AddObjectPositionCommand = new RelayCommand(AddObjectPosition);
            AddPersonPositionCommand = new RelayCommand(AddPersonPosition);
            CloseDetailCommand = new RelayCommand(ClosePictureDetail);
            AddPersonToPhotoCommand = new RelayCommand(AddPersonToPicture);
            RemovePersonFromPhotoCommand = new RelayCommand(RemovePersonFromPicture);
            AddObjectToPhotoCommand = new RelayCommand(AddObjectToPhoto);
            RemoveObjectFromPhotoCommand = new RelayCommand(RemoveObjectFromPhoto);

            this._messenger.Register<SelectedPictureMessage>(SelectedPicture);
            Persons = new ObservableCollection<PersonListModel>();
            PersonsOnPhoto = new ObservableCollection<PositionInPictureModel>();
            Objects = new ObservableCollection<ObjectListModel>();
            ObjectsOnPhoto = new ObservableCollection<PositionInPictureModel>();
            RectangleItems = new ObservableCollection<RectangleItem>();
        }

        private void RemoveObjectFromPhoto()
        {
            if (SelectedObjectOnPhoto != null)
            {
                var obje =
                    Detail.PositionCollection.FirstOrDefault(x => x.PictureObject != null && x.Id == SelectedObjectOnPhoto.Id);

                if (obje != null)
                {
                    Detail.PositionCollection.Remove(obje);
                }

                ObjectsOnPhoto.Remove(SelectedObjectOnPhoto);
                SelectedPersonOnPhoto = null;
            }
        }

        private void AddObjectToPhoto(object obj)
        {
            if (SelectedObject != null)
            {
                if (SelectedObject != null)
                {
                    ObjectsOnPhoto.Add(new PositionInPictureModel() { Id = Guid.NewGuid(), PictureObject = _objectRepository.GetById(SelectedObject.Id) });
                    SelectedObject = null;
                }
            }
        }

        private void FillCollections()
        {
            Persons.Clear();
            PersonsOnPhoto.Clear();
            Objects.Clear();
            ObjectsOnPhoto.Clear();

            var persons = _personRepository.GetAllPersonListModels();
            foreach (var person in persons)
            {
                Persons.Add(person);
            }

            var objects = _objectRepository.GetObjectListModels();
            foreach (var obj in objects)
            {
                Objects.Add(obj);
            }

            foreach (var positionInPictureModel in Detail.PositionCollection)
            {
                if (positionInPictureModel.PictureObject is PersonDetailModel person)
                {
                    PersonsOnPhoto.Add(positionInPictureModel);
                }
                else if (positionInPictureModel.PictureObject is ObjectDetailModel obje)
                {
                    ObjectsOnPhoto.Add(positionInPictureModel);
                }
            }
        }

        private void AddPersonToPicture()
        {
            if (SelectedPerson != null)
            {
                PersonsOnPhoto.Add(new PositionInPictureModel() { Id= Guid.NewGuid(), PictureObject = _personRepository.GetById(SelectedPerson.Id)});
                SelectedPerson = null;
            }
        }

        private void RemovePersonFromPicture()
        {
            if (SelectedPersonOnPhoto != null)
            {
                var person =
                    Detail.PositionCollection.FirstOrDefault(x => x.PictureObject != null && x.Id == SelectedPersonOnPhoto.Id);

                if (person != null)
                {
                    Detail.PositionCollection.Remove(person);
                }

                PersonsOnPhoto.Remove(SelectedPersonOnPhoto);
                SelectedPersonOnPhoto = null;
            }
        }

        private void SavePicture()
        {
            if (Detail.Id != Guid.Empty)
            {
                foreach (var positionInPictureModel in PersonsOnPhoto)
                {
                    if (Detail.PositionCollection.FirstOrDefault(x => x.Id == positionInPictureModel.Id) == null)
                    {
                        Detail.PositionCollection.Add(positionInPictureModel);
                    }
                }

                foreach (var positionInPictureModel in ObjectsOnPhoto)
                {
                    if (Detail.PositionCollection.FirstOrDefault(x => x.Id == positionInPictureModel.Id) == null)
                    {
                        Detail.PositionCollection.Add(positionInPictureModel);
                    }
                }

                _pictureRepository.Update(Detail);
                _messenger.Send(new SavePictureMessage(Detail.Id));
            }
        }


        private void DeletePicture()
        {
            if (Detail.Id != Guid.Empty)
            {
                var pictureId = Detail.Id;
                Detail = new PictureDetailModel();
                _pictureRepository.Remove(pictureId);
                _messenger.Send(new DeletedPictureMessage(pictureId));
                ClosePictureDetail();
            }
        }


        private void SelectedPicture(SelectedPictureMessage message)
        {
            Detail = _pictureRepository.GetById(message.Id);
        }

        private void ShowPersonDetail(object parametr)
        {
            if (parametr is PositionInPictureModel position)
            {
                PositionInPictureDetail = position;
                PersonDetail = position.PictureObject as PersonDetailModel;
            }
        }

        private void ShowObjectDetail(object parametr)
        {
            if (parametr is PositionInPictureModel position)
            {
                PositionInPictureDetail = position;
                ObjectDetail = position.PictureObject as ObjectDetailModel;
            }
        }

        private void AddObjectPosition()
        {
            var position = new PositionInPictureModel
            {
                Id = Guid.NewGuid(),
                PictureObject = new ObjectDetailModel()
            };

            PositionInPictureDetail = position;
            ObjectDetail = position.PictureObject as ObjectDetailModel;
        }

        private void AddPersonPosition()
        {
            var position = new PositionInPictureModel
            {
                PictureObject = new PersonDetailModel()
            };

            PositionInPictureDetail = position;
            PersonDetail = position.PictureObject as PersonDetailModel;
        }

        private void ClosePictureDetail()
        {
            _messenger.Send(new ClosePictureDetail());
        }
    }
}
