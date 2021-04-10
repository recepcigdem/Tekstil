using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Channel : BaseEntity
    {
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string ChannelName { get; set; }
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
