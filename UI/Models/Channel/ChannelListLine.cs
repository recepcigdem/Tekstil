using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Channel
{
    public class ChannelListLine
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string ChannelName { get; set; }
        public string CurrencyType { get; set; }

        public ChannelListLine() : base()
        {
            Id = 0;
            CustomerId = 0;
            IsActive = false;
            Code = string.Empty;
            ChannelName = string.Empty;
            CurrencyType = string.Empty;
        }
        public ChannelListLine(Entities.Concrete.Channel channel)
        {
            Id = channel.Id;
            CustomerId = channel.CustomerId;
            IsActive = channel.IsActive;
            Code = channel.Code;
            ChannelName = channel.ChannelName;
            CurrencyType = channel.CurrencyType;
        }
    }
}
