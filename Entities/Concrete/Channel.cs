using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    [Table("Channel", Schema = "definition")]
    public class Channel : BaseCustomerEntity
    {
        [Column("isUsed")]
        public bool IsUsed { get; set; }
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("channelName")]
        public string ChannelName { get; set; }
        [Column("currencyType")]
        public string CurrencyType { get; set; }

        public Channel()
        {
            IsActive = false;
            Code = string.Empty;
            ChannelName = string.Empty;
            CurrencyType = string.Empty;
        }
    }
}
