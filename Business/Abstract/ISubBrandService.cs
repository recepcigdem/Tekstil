using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ISubBrandService
    {
        IDataResult<List<SubBrand>> GetAll();
        IDataResult<SubBrand> GetById(int subBrandId);
        IResult Add(SubBrand subBrand);
        IResult Update(SubBrand subBrand);
        IResult Delete(SubBrand subBrand);
    }
}
