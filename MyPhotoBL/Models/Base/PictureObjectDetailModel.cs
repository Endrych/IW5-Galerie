using System;

namespace MyPhotoBL.Models.Base
{
    public abstract class PictureObjectDetailModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string PreviewFileName { get; set; }
    }
}
