using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IImportedLocalService
    {
        IDataResult<List<ImportedLocal>> GetAll();
        IDataResult<ImportedLocal> GetById(int importedLocalId);
        IResult Add(ImportedLocal importedLocal);
        IResult Update(ImportedLocal importedLocal);
        IResult Delete(ImportedLocal importedLocal);
    }
}
