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
    public interface IStaffEmailService
    {
        IDataServiceResult<List<StaffEmail>> GetAll();
        IDataServiceResult<StaffEmail> GetById(int staffEmailId);
        IDataServiceResult<StaffEmail> GetByEmailId(int emailId);
        IDataServiceResult<List<StaffEmail>> GetAllByStaffId(int staffId);
        IServiceResult Add(StaffEmail staffEmail);
        IServiceResult Update(StaffEmail staffEmail);
        IServiceResult Delete(StaffEmail staffEmail);
        IServiceResult DeleteByStaff(Staff staff);
        IDataServiceResult<StaffEmail> Save(StaffEmailDto staffEmailDto);
    }
}
