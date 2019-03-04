using System;

namespace MyPhotoBL.Messages
{
    public class DeletedPersonMessage
    {
        public DeletedPersonMessage(Guid personId)
        {
            PersonId = personId;
        }

        public Guid PersonId { get; set; }
    }
}
