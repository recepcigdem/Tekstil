using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IPhoneService
    {
        IDataResult<List<Phone>> GetAll();
        IDataResult<Phone> GetById(int phoneId);
        IResult Add(Phone phone);
        IResult Update(Phone phone);
        IResult Delete(Phone phone);
        IResult DeleteByPhoneId(int phoneId);
    }
}
