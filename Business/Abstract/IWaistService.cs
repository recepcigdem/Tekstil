using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IWaistService
    {
        IDataResult<List<Waist>> GetAll();
        IDataResult<Waist> GetById(int waistId);
        IResult Add(Waist waist);
        IResult Update(Waist waist);
        IResult Delete(Waist waist);
    }
}
