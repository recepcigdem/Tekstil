using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("Authorization", Schema = "staff")]
    public class Authorization : BaseEntity
    {
        [Column("authorizationName")]
        public string AuthorizationName { get; set; }

        public Authorization()
        {
            AuthorizationName = string.Empty;
        }
    }
}
