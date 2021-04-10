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
        IDataResult<List<Channel>> GetAll();
        IDataResult<Channel> GetById(int channelId);
        IResult Add(Channel channel);
        IResult Update(Channel channel);
        IResult Delete(Channel channel);
    }
}
