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

        public IDataServiceResult<List<Channel>> GetAll(int customerId)
        {
            var dbResult = _channelDal.GetAll(x => x.CustomerId == customerId);

            return new SuccessDataServiceResult<List<Channel>>(dbResult, true, "Listed");
        }

        
        public IDataServiceResult<Channel> GetById(int channelId)
        {
            var dbResult = _channelDal.Get(p => p.Id == channelId);
            if (dbResult == null)
                return new SuccessDataServiceResult<Channel>(false, "SystemError");

            return new SuccessDataServiceResult<Channel>(dbResult, true, "Listed");
        }
        
        //[SecuredOperation("SuperAdmin,CompanyAdmin,definition")]
        [ValidationAspect(typeof(ChannelValidator))]
        [TransactionScopeAspect]
        public IServiceResult Add(Channel channel)
        {
            ServiceResult result = BusinessRules.Run(CheckIfChannelExists(channel));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _channelDal.Add(channel);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Added");
        }
        
        //[SecuredOperation("SuperAdmin,CompanyAdmin,definition")]
        [ValidationAspect(typeof(ChannelValidator))]
        [TransactionScopeAspect]
        public IServiceResult Update(Channel channel)
        {
            ServiceResult result = BusinessRules.Run(CheckIfChannelExists(channel));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _channelDal.Update(channel);
            if (dbResult == null)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Updated");
        }
        
        //[SecuredOperation("SuperAdmin,CompanyAdmin,definition")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Channel channel)
        {
            ServiceResult result = BusinessRules.Run(CheckIfChannelIsUsed(channel));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var deleteResult = _channelDal.Delete(channel);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "SystemError");

            return new ServiceResult(true, "Delated");
        }

        public IDataServiceResult<Channel> Save(Channel channel)
        {  
                if (channel.Id > 0)
                {
                    Update(channel);
                }
                else
                {
                    Add(channel);
                }
           
            return new SuccessDataServiceResult<Channel>(true, "Saved");
        }

        private ServiceResult CheckIfChannelExists(Channel channel)
        {
            var result = _channelDal.GetAll(x => x.CustomerId == channel.CustomerId && x.Code == channel.Code && x.ChannelName == channel.ChannelName && x.CurrencyType == channel.CurrencyType);

            if (result.Count > 1)
                new ErrorServiceResult(false, "ChannelAlreadyExists");

            return new ServiceResult(true, "");
        }

        private ServiceResult CheckIfChannelIsUsed(Channel channel)
        {
            var result = GetById(channel.Id);
            if (result.Result==true)
                if(result.Data.IsUsed==true)
                new ErrorServiceResult(false, "ChannelIsUsed");

            return new ServiceResult(true, "");
        }
    }
}
