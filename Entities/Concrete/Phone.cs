using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("Phone", Schema = "staff")]
    public class Phone : BaseCustomerEntity
    {
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("countryCode")]
        public string CountryCode { get; set; }
        [Column("areaCode")]
        public string AreaCode { get; set; }
        [Column("phoneNumber")]
        public string PhoneNumber { get; set; }

        public Phone()
        {
            IsActive = false;
            CountryCode = string.Empty;
            AreaCode = string.Empty;
            PhoneNumber= string.Empty;
        }
    }
}
