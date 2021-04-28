using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IDefinitionTitleService
    {
        IDataServiceResult<List<DefinitionTitle>> GetAll(int customerId);
        IDataServiceResult<DefinitionTitle> GetByValue(int customerId, int value);
        IDataServiceResult<DefinitionTitle> GetById(int definitionTitleId);
        IDataServiceResult<DefinitionTitle> Save(DefinitionTitle definitionTitle);
        IServiceResult Delete(DefinitionTitle definitionTitle);
    }
}
