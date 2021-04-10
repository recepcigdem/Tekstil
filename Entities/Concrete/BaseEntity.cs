using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime DateLastModified { get; set; }

        public BaseEntity()
        {
            Id = 0;
            IsDeleted = false;
            CreatedBy = 0;
            DateCreated = DateTime.UtcNow;
            LastModifiedBy = 0;
            DateLastModified = DateTime.UtcNow;
        }
    }

}
