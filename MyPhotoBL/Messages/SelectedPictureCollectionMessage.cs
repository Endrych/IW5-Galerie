using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoBL.Models;

namespace MyPhotoBL.Messages
{
    public class SelectedPictureCollectionMessage
    {

        public SelectedPictureCollectionMessage(IEnumerable<PictureDetailModel> pictureCollection)
        {
            PictureCollection = pictureCollection;
        }

        public IEnumerable<PictureDetailModel> PictureCollection { get; set; }
    }

}
