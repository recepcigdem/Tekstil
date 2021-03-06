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
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Interceptors;

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
                return new SuccessDataServiceResult<Channel>(false, "Error_SystemError");

            return new SuccessDataServiceResult<Channel>(dbResult, true, "Listed");
        }

        public IServiceResult Add(Channel channel)
        {
            ServiceResult result = BusinessRules.Run(CheckIfChannelExists(channel));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _channelDal.Add(channel);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Added");
        }

        public IServiceResult Update(Channel channel)
        {
            ServiceResult result = BusinessRules.Run(CheckIfChannelExists(channel));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var dbResult = _channelDal.Update(channel);
            if (dbResult == null)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Updated");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.deleted")]
        [TransactionScopeAspect]
        public IServiceResult Delete(Channel channel)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<Channel>(false, message);
            }

            #endregion

            ServiceResult result = BusinessRules.Run(CheckIfChannelIsUsed(channel));
            if (result.Result == false)
                return new ErrorServiceResult(false, result.Message);

            var deleteResult = _channelDal.Delete(channel);
            if (deleteResult == false)
                return new ErrorServiceResult(false, "Error_SystemError");

            return new ServiceResult(true, "Deleted");
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("SuperAdmin,CompanyAdmin,definition.saved")]
        [ValidationAspect(typeof(ChannelValidator))]
        [TransactionScopeAspect]
        public IDataServiceResult<Channel> Save(Channel channel)
        {
            #region AspectControl

            if (MethodInterceptionBaseAttribute.Result == false)
            {
                var message = MethodInterceptionBaseAttribute.Message;
                MethodInterceptionBaseAttribute.Message = "";
                MethodInterceptionBaseAttribute.Result = true;
                return new DataServiceResult<Channel>(false, message);
            }

            #endregion

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
            if (result.Result == true)
                if (result.Data.IsUsed == true)
                    new ErrorServiceResult(false, "ChannelIsUsed");

            return new ServiceResult(true, "");
        }
    }
}
