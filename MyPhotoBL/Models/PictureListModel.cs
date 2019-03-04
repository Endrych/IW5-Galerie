using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoBL.Models
{
    public class PictureListModel
    {
        public Guid Id { get; set; }

        public string Source { get; set; }

        public string Name { get; set; }
    }
}
