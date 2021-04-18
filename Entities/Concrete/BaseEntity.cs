using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;

namespace Entities.Concrete
{
    public class BaseEntity : IEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("isDeleted")]
        public bool IsDeleted { get; set; }
        [Column("createdBy")]
        public int CreatedBy { get; set; }
        [Column("dateCreated")]
        public DateTime DateCreated { get; set; }
        [Column("lastModifiedBy")]
        public int LastModifiedBy { get; set; }
        [Column("dateLastModified")]
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
