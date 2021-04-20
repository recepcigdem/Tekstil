using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.Dtos;

namespace Business.Abstract
{
    public interface IStaffPhoneService
    {
        IDataResult<List<StaffPhone>> GetAll();
        IDataResult<StaffPhone> GetById(int staffPhoneId);
        IResult Add(StaffPhone staffPhone);
        IResult Update(StaffPhone staffPhone);
        IResult Delete(StaffPhone staffPhone);
        IResult Save(StaffPhoneDto staffPhoneDto);
    }
}
