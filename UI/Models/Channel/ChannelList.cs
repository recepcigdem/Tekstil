using Business.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Channel
{
    public class ChannelList
    {
        public List<ChannelListLine> data { get; set; }

        private IChannelService _channelService;

        public ChannelList(HttpRequest request, IChannelService channelService)
        {
            var customerId = Helpers.SessionHelper.GetStaff(request).CustomerId;
            _channelService = channelService;

            var listGrid = _channelService.GetAll(customerId);

            if (listGrid != null)
            {
                data = new List<ChannelListLine>();

                foreach (var item in listGrid.Data)
                {
                    ChannelListLine line = new ChannelListLine(item);
                    data.Add(line);
                }
            }
        }
    }
}
