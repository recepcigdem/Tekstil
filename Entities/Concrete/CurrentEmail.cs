using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    [Table("CurrentEmail",Schema = "customer")]
    public class CurrentEmail : BaseCustomerEntity
    {
        [Column("currentId")]
        public int CurrentId { get; set; }
        [Column("emailId")]
        public int EmailId { get; set; }
        [Column("isMain")]
        public bool IsMain { get; set; }

        public CurrentEmail()
        {
            CurrentId = 0;
            EmailId = 0;
            IsMain = false;
        }
    }
}
