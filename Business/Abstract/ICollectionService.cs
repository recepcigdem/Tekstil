using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICollectionService
    {
        IDataResult<List<Collection>> GetAll();
        IDataResult<Collection> GetById(int collectionId);
        IResult Add(Collection collection);
        IResult Update(Collection collection);
        IResult Delete(Collection collection);
    }
}
