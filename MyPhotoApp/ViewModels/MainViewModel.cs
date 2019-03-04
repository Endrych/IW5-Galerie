using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyPhotoApp.Commands;
using MyPhotoApp.ViewModels.Base;
using MyPhotoBL;
using MyPhotoBL.Interfaces;
using MyPhotoBL.Messages;
using MyPhotoBL.Models;
using MyPhotoBL.Repositories;

namespace MyPhotoApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly PictureRepository pictureRepository;
        private readonly AlbumRepository albumRepository;
        private readonly ObjectRepository objectRepository;
        private readonly PersonRepository personRepository;

        private readonly IMessenger messenger;

        public ICommand GetAllPicturesCommand { get; set; }
        public ICommand GetPicturesByAlbumIdCommand { get; set; }
        public ICommand GetPicturesByPersonIdCommand { get; set; }
        public ICommand GetPicturesByObjectIdCommand { get; set; }
        public ICommand NewAlbumCommand { get; set; }
        public ICommand RemoveAlbumCommand { get; set; }
        public ICommand EditAlbumCommand { get; set; }
        public ICommand AddPersonCommand { get; set; }
        public ICommand RemovePersonCommand { get; set; }
        public ICommand EditPersonCommand { get; set; }
        public ICommand AddObjectCommand { get; set; }
        public ICommand RemoveObjectCommand { get; set; }
        public ICommand EditObjectCommand { get; set; }
        public ICommand ShowAlbumPictureCommand { get; set; }
        public ICommand ShowPersonPictureCommand { get; set; }
        public ICommand ShowObjectPictureCommand { get; set; }
        public ICommand AddSelectedPictureCommand { get; set; }
        public ICommand RemoveSelectedPictureCommand { get; set; }

        public ObservableCollection<AlbumListModel> Albums
        {
            get => _albums;
            set
            {
                if (Equals(value, _albums)) return;
                _albums = value;
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

        private AlbumListModel _selectedAlbumListModel;
        private PersonListModel _selectePersonListModel;
        private PersonDetailModel _selectedPersonDetail;
        private AlbumDetailModel _selectedAlbumDetail;
        private ObjectListModel _selectedObjectListModel;
        private ObjectDetailModel _selectedObjectDetail;
        private ObservableCollection<AlbumListModel> _albums;
        private ObservableCollection<PersonListModel> _persons;
        private ObservableCollection<ObjectListModel> _objects;
        private string _personFilter = string.Empty;
        private string _objectFilter = string.Empty;
        private string _albumFilter = string.Empty;
        private int Deta;

        public AlbumListModel SelectedAlbum
        {
            get => _selectedAlbumListModel;
            set
            {
                _selectedAlbumListModel = value;
                if (_selectedAlbumListModel != null)
                    SelectedAlbumDetail = albumRepository.GetById(_selectedAlbumListModel.Id);
                GetPictureCollectionByAlbumId();
                OnPropertyChanged();
            }
        }

        public PersonListModel SelectedPerson
        {
            get => _selectePersonListModel;
            set
            {
                _selectePersonListModel = value;
                if (_selectePersonListModel != null)
                    SelectedPersonDetail = personRepository.GetById(_selectePersonListModel.Id);
                GetPictureCollectionByPersonId();
                OnPropertyChanged();
            }
        }

        public ObjectListModel SelectedObject
        {
            get => _selectedObjectListModel;
            set
            {
                _selectedObjectListModel = value;
                if (_selectedObjectListModel != null)
                    SelectedObjectDetail = objectRepository.GetById(_selectedObjectListModel.Id);
                GetPictureCollectionByObjectId();
                OnPropertyChanged();
            }
        }

        public PersonDetailModel SelectedPersonDetail
        {
            get => _selectedPersonDetail;
            set
            {
                if (Equals(value, _selectedPersonDetail)) return;
                _selectedPersonDetail = value;
                OnPropertyChanged();
            }
        }

        public AlbumDetailModel SelectedAlbumDetail
        {
            get => _selectedAlbumDetail;
            set
            {
                if (Equals(value, _selectedAlbumDetail)) return;
                _selectedAlbumDetail = value;
                OnPropertyChanged();
            }
        }

        public ObjectDetailModel SelectedObjectDetail
        {
            get => _selectedObjectDetail;
            set
            {
                if (Equals(value, _selectedObjectDetail)) return;
                _selectedObjectDetail = value;
                OnPropertyChanged();
            }
        }

        public string PersonFilter
        {
            get => _personFilter;
            set
            {
                _personFilter = value;
                LoadPersonCollection(personRepository.GetAllPersonListModels().Where(p => p.Name.ToLower().Contains(_personFilter.ToLower())));
                OnPropertyChanged();
            }
        }

        public string ObjectFilter
        {
            get => _objectFilter;
            set
            {
                _objectFilter = value;
                LoadObjectCollection(objectRepository.GetObjectListModels().Where(o => o.Name.ToLower().Contains(_objectFilter.ToLower())));
                OnPropertyChanged();
            }
        }

        public string AlbumFilter
        {
            get => _albumFilter;
            set
            {
                _albumFilter = value;
                LoadAlbumCollection(albumRepository.GetAlbumListModels().Where(a => a.Name.ToLower().Contains(_albumFilter.ToLower())));
                OnPropertyChanged();
            }
        }


        public MainViewModel(PictureRepository pictureRepository, AlbumRepository albumRepository,
            ObjectRepository objectRepository, PersonRepository personRepository, IMessenger messenger)
        {
            this.pictureRepository = pictureRepository;
            this.albumRepository = albumRepository;
            this.objectRepository = objectRepository;
            this.personRepository = personRepository;
            this.messenger = messenger;

            GetAllPicturesCommand = new RelayCommand(GetAllPictures);
            GetPicturesByAlbumIdCommand = new RelayCommand(GetPictureCollectionByAlbumId);
            GetPicturesByPersonIdCommand = new RelayCommand(GetPictureCollectionByPersonId);
            GetPicturesByObjectIdCommand = new RelayCommand(GetPictureCollectionByObjectId);
            NewAlbumCommand = new RelayCommand(AddNewAlbum);
            RemoveAlbumCommand = new RelayCommand(RemoveAlbum);
            EditAlbumCommand = new RelayCommand(EditAlbum);
            AddPersonCommand = new RelayCommand(AddPerson);
            RemovePersonCommand = new RelayCommand(RemovePerson);
            EditPersonCommand = new RelayCommand(EditPerson);
            AddObjectCommand = new RelayCommand(AddObject);
            EditObjectCommand = new RelayCommand(EditObject);
            RemoveObjectCommand = new RelayCommand(RemoveObject);
            ShowAlbumPictureCommand = new RelayCommand(GetPictureCollectionByAlbumId);
            ShowObjectPictureCommand = new RelayCommand(GetPictureCollectionByObjectId);
            ShowPersonPictureCommand = new RelayCommand(GetPictureCollectionByPersonId);
            AddSelectedPictureCommand = new RelayCommand(AddSelectedPicture);
            RemoveSelectedPictureCommand = new RelayCommand(RemoveSelectedPicture);

            Albums = new ObservableCollection<AlbumListModel>();
            LoadAlbumCollection(albumRepository.GetAlbumListModels());

            Persons = new ObservableCollection<PersonListModel>();
            LoadPersonCollection(personRepository.GetAllPersonListModels());

            Objects = new ObservableCollection<ObjectListModel>();
            LoadObjectCollection(objectRepository.GetObjectListModels());
        }

        private void RemoveSelectedPicture()
        {
            if (SelectedAlbum == null) return;
            messenger.Send(new RemoveSelectedPictureFromAlbumMessage(SelectedAlbum.Id));
            GetPictureCollectionByAlbumId();
        }

        public void AddNewAlbum()
        {
            var album = new AlbumDetailModel()
            {
                Id = Guid.NewGuid(),
                Name = "New album...",
                PictureCollection = new List<PictureDetailModel>()
            };
            albumRepository.Insert(album);
            LoadAlbumCollection(albumRepository.GetAlbumListModels());
        }

        public void AddSelectedPicture()
        {
            if (SelectedAlbum != null)
            {
                messenger.Send(new AddSelectedPictureToAlbum(SelectedAlbum.Id));
            }
        }

        public void AddPerson()
        {
            var person = new PersonDetailModel()
            {
                Id = Guid.NewGuid(),
                FirstName = "Firstname",
                LastName = "Lastname"
            };
            personRepository.Insert(person);
            Persons.Add(new PersonListModel()
            {
                Id = person.Id,
                Name = $"{person.FirstName} {person.LastName}"
            });

            Persons.Clear();
            var persons = personRepository.GetAllPersonListModels();
            foreach (var per in persons)
            {
                Persons.Add(per);
            }
        }

        public void AddObject()
        {
            var obj = new ObjectDetailModel()
            {
                Id = Guid.NewGuid(),
                Name = "Object..."
            };
            objectRepository.Insert(obj);
            Objects.Add(new ObjectListModel()
            {
                Id = obj.Id,
                Name = "Object..."
            });
            LoadObjectCollection(objectRepository.GetObjectListModels());
        }

        public void EditAlbum()
        {
            if (SelectedAlbumDetail == null) return;
            albumRepository.Update(SelectedAlbumDetail);
            Albums.First(x => x.Id == SelectedAlbumDetail.Id).Name = SelectedAlbumDetail.Name;
        }

        public void EditPerson()
        {
            if (SelectedPersonDetail == null) return;
            personRepository.Update(SelectedPersonDetail);
            Persons.First(x => x.Id == SelectedPersonDetail.Id).Name =
                $"{SelectedPersonDetail.FirstName} {SelectedPersonDetail.LastName}";
        }

        public void EditObject()
        {
            if (SelectedObjectDetail == null) return;
            objectRepository.Update(SelectedObjectDetail);
            Objects.First(x => x.Id == SelectedObjectDetail.Id).Name =
                SelectedObjectDetail.Name;
        }

        public void RemoveAlbum()
        {
            if (SelectedAlbum == null) return;
            albumRepository.Remove(SelectedAlbum.Id);
            Albums.Remove(Albums.First(x => x.Id == SelectedAlbum.Id));
            SelectedAlbumDetail = null;
        }

        public void RemovePerson()
        {
            if (SelectedPerson == null) return;
            var person = personRepository.GetById(SelectedPerson.Id); 
            personRepository.Remove(SelectedPerson.Id);
            Persons.Remove(Persons.First(x => x.Id == SelectedPerson.Id));
            SelectedPersonDetail = null;
        }

        public void RemoveObject()
        {
            if (SelectedObject == null) return;
            objectRepository.Remove(SelectedObject.Id);
            Objects.Remove(Objects.First(x => x.Id == SelectedObject.Id));
            SelectedObject = null;
        }
        
        private void GetPictureCollectionByAlbumId()
        {
            if (SelectedAlbum == null) return;
            SelectedPerson = null;
            SelectedObject = null;
            messenger.Send(new SelectedPictureCollectionMessage(albumRepository.GetById(SelectedAlbum.Id).PictureCollection));
        }

        private void GetPictureCollectionByPersonId()
        {
            if (SelectedPerson == null) return;
            SelectedAlbum = null;
            SelectedObject = null;
            messenger.Send(new SelectedPictureCollectionMessage(pictureRepository.GetAllPictureModelsByPersonId(SelectedPerson.Id)));
        }

        private void GetPictureCollectionByObjectId()
        {
            if (SelectedObject == null) return;
            SelectedAlbum = null;
            SelectedPerson = null;
            messenger.Send(new SelectedPictureCollectionMessage(pictureRepository.GetAllPictureModelsByObjectId(SelectedObject.Id)));
        }
        
        private void GetAllPictures()
        {
            SelectedPerson = null;
            SelectedObject = null;
            messenger.Send(new SelectedPictureCollectionMessage(pictureRepository.GetAllPictureModels()));
        }

        private void LoadAlbumCollection(IEnumerable<AlbumListModel> albumListModels)
        {
            Albums.Clear();
            foreach (var alb in albumListModels)
                Albums.Add(alb);
        }

        private void LoadObjectCollection(IEnumerable<ObjectListModel> objectListModels)
        {
            Objects.Clear();
            foreach (var obje in objectListModels)
                Objects.Add(obje);
        }

        private void LoadPersonCollection(IEnumerable<PersonListModel> personListModels)
        {
            Persons.Clear();
            foreach (var obje in personListModels)
                Persons.Add(obje);
        }
    }
}
