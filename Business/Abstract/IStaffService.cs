using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using Entities.Concrete.Dtos.Staff;

namespace Business.Abstract
{
    public interface IStaffService
    {
        IDataServiceResult<List<Staff>> GetAll(int customerId);
        IDataServiceResult<List<Staff>> GetAllByDepartmentId(int customerId,int departmentId);
        IDataServiceResult<Staff> GetById(int staffId);
        IServiceResult Add(Staff staff);
        IServiceResult Update(Staff staff);
        IServiceResult Delete(Staff staff);
        IServiceResult DeleteAll(Staff staff);
        IDataServiceResult<Staff> Save(Staff staff);
        IDataServiceResult<Staff> SaveAll(Staff staff, List<StaffEmailDto> staffEmailDtos, List<StaffPhoneDto> staffPhoneDtos, List<StaffAuthorization> staffAuthorizations, string password);
    }
}
