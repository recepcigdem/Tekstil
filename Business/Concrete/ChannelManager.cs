using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class ChannelManager : IChannelService
    {
        private IChannelDal _channelDal;

        public ChannelManager(IChannelDal channelDal)
        {
            _channelDal = channelDal;
        }

        public IDataResult<List<Channel>> GetAll()
        {
            return new SuccessDataResult<List<Channel>>(true, "Listed", _channelDal.GetAll());
        }

        public IDataResult<Channel> GetById(int channelId)
        {
            return new SuccessDataResult<Channel>(true, "Listed", _channelDal.Get(p => p.Id == channelId));
        }

        [SecuredOperation("admin,definition.add")]
        [ValidationAspect(typeof(ChannelValidator))]
        [TransactionScopeAspect]
        public IResult Add(Channel channel)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(channel), CheckIfDescriptionExists(channel));

            if (result != null)
                return result;

            _channelDal.Add(channel);

            return new SuccessResult("Added");

        }

        [SecuredOperation("admin,definition.updated")]
        [ValidationAspect(typeof(ChannelValidator))]
        [TransactionScopeAspect]
        public IResult Update(Channel channel)
        {
            IResult result = BusinessRules.Run(CheckIfCodeExists(channel), CheckIfDescriptionExists(channel));

            if (result != null)
                return result;

            _channelDal.Add(channel);

            return new SuccessResult("Updated");
        }

        [SecuredOperation("admin,definition.deleted")]
        [TransactionScopeAspect]
        public IResult Delete(Channel channel)
        {
            _channelDal.Delete(channel);

            return new SuccessResult("Deleted");
        }

        private IResult CheckIfDescriptionExists(Channel channel)
        {
            var result = _channelDal.GetAll(x => x.ChannelName == channel.ChannelName).Any();

            if (result)
                new ErrorResult("DescriptionAlreadyExists");

            return new SuccessResult();
        }
        private IResult CheckIfCodeExists(Channel channel)
        {
            var result = _channelDal.GetAll(x => x.Code == channel.Code).Any();

            if (result)
                new ErrorResult("CodeAlreadyExists");

            return new SuccessResult();
        }
    }
}
