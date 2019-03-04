using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MyPhotoBL.Annotations;
using MyPhotoBL.Models.Base;
using MyPhotoDb.Entities.Base;

namespace MyPhotoBL.Models
{
    public class PositionInPictureModel:INotifyPropertyChanged
    {
        private int _x;
        private int _y;
        private int _width;
        private int _height;
        public Guid Id { get; set; }

        public int X
        {
            get => _x;
            set
            {
                if (value == _x) return;
                _x = value;
                OnPropertyChanged();
            }
        }

        public int Y
        {
            get => _y;
            set
            {
                if (value == _y) return;
                _y = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public PictureObjectDetailModel PictureObject { get; set; }

        public string ListName
        {
            get
            {
                if (PictureObject is PersonDetailModel person)
                {
                    return $"{person.FirstName} {person.LastName}";
                }
                else if (PictureObject is ObjectDetailModel obj)
                {
                    return obj.Name;
                }
                else
                {
                    return "";
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
