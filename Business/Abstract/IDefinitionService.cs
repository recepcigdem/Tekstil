using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IDefinitionService
    {
        IDataServiceResult<List<Definition>> GetAll(int customerId);
        IDataServiceResult<List<Definition>> GetAllByCustomerIdAndDefinitionTitleId(int customerId,int definitionTitleId);
        IDataServiceResult<Definition> GetById(int definitionId);
        IDataServiceResult<Definition> GetByDefinitionTitleId(int definitionTitleId);
        IDataServiceResult<Definition> Save(Definition definition);
        IServiceResult Delete(Definition definition);
    }
}
