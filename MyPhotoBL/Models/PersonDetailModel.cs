using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoBL.Models.Base;

namespace MyPhotoBL.Models
{
    public class PersonDetailModel : PictureObjectDetailModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
