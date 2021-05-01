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
    public interface IStaffPhoneService
    {
        IDataServiceResult<List<StaffPhone>> GetAll(int customerId);
        IDataServiceResult<StaffPhone> GetById(int staffPhoneId);
        IDataServiceResult<StaffPhone> GetByPhoneId(int phoneId);
        IDataServiceResult<List<StaffPhone>> GetAllByStaffId(int staffId);
        IServiceResult Add(StaffPhone staffPhone);
        IServiceResult Update(StaffPhone staffPhone);
        IServiceResult Delete(StaffPhone staffPhone);
        IServiceResult DeleteByStaff(Staff staff);
        IDataServiceResult<StaffPhone> Save(Staff staff, List<StaffPhoneDto> staffPhoneDtos);
    }
}
