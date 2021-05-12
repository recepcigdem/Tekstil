using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Channel
{
    public class Channel:BaseModel
    {
        
        public bool IsActive { get; set; }       
        public string Code { get; set; }      
        public string ChannelName { get; set; }       
        public string CurrencyType { get; set; }

        public Channel() :base()
        {
            IsActive = false;
            Code = string.Empty;
            ChannelName = string.Empty;
            CurrencyType = string.Empty;
        }

        public Channel(HttpRequest request, Entities.Concrete.Channel channel, IStringLocalizer _localizerShared) : base(request)
        {
            EntityId = channel.Id;
            CustomerId = channel.CustomerId;
            IsActive = channel.IsActive;
            Code = channel.Code;
            ChannelName = channel.ChannelName;
            CurrencyType = channel.CurrencyType;
        }

        public Entities.Concrete.Channel GetBusinessModel()
        {
            Entities.Concrete.Channel channel = new Entities.Concrete.Channel();
            if (EntityId > 0)
            {
                channel.Id = EntityId;
            }
     
            channel.CustomerId = CustomerId;
            channel.IsActive = IsActive;
            channel.Code = Code;
            channel.ChannelName = ChannelName;
            channel.CurrencyType = CurrencyType;


            return channel;
        }
    }
}
