using System;

namespace MyPhotoBL.Messages
{
    public class DeletedAlbumMessage
    {
        public DeletedAlbumMessage(Guid albumId)
        {
            AlbumId = albumId;
        }

        public Guid AlbumId { get; set; }
    }
}
