using System;

namespace MyPhotoBL.Messages
{
    public class DeletedObjectMessage
    {
        public DeletedObjectMessage(Guid objectId)
        {
            ObjectId = objectId;
        }

        public Guid ObjectId { get; set; }
    }
}
