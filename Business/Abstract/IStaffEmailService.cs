using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IStaffEmailService
    {
        IDataResult<List<StaffEmail>> GetAll();
        IDataResult<StaffEmail> GetById(int staffEmailId);
        IResult Add(StaffEmail staffEmail);
        IResult Update(StaffEmail staffEmail);
        IResult Delete(StaffEmail staffEmail);
    }
}
