using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IChannelService
    {
        IDataServiceResult<List<Channel>> GetAll(int customerId);
        IDataServiceResult<Channel> GetById(int channelId);
        IServiceResult Delete(Channel channel);
        IDataServiceResult<Channel> Save(Channel channel);
    }
}
