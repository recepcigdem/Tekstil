using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    [Table("CurrentPhone",Schema = "customer")]
    public class CurrentPhone : BaseCustomerEntity
    {
        [Column("currentId")]
        public int CurrentId { get; set; }
        [Column("phoneId")]
        public int PhoneId { get; set; }
        [Column("isMain")]
        public bool IsMain { get; set; }

        public CurrentPhone()
        {
            CurrentId = 0;
            PhoneId = 0;
            IsMain = false;
        }
    }
}
