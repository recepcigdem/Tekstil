using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    [Table("DefinitionTitle", Schema = "definition")]
    public class DefinitionTitle : BaseCustomerEntity
    {
        [Column("title")]
        public string Title { get; set; }
        [Column("value")]
        public int Value { get; set; }

        public DefinitionTitle()
        {
            Title = string.Empty;
            Value = 0;
        }
    }
}
