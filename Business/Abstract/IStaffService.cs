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
    public interface IStaffService
    {
        IDataServiceResult<List<Staff>> GetAll();
        IDataServiceResult<Staff> GetById(int staffId);
        IServiceResult Add(Staff staff);
        IServiceResult Update(Staff staff);
        IServiceResult Delete(Staff staff);
        IServiceResult DeleteAll(Staff staff);
        IDataServiceResult<Staff> Save(Staff staff);
        IDataServiceResult<Staff> SaveAll(Staff staff, List<StaffEmailDto> staffEmailDtos, List<StaffPhoneDto> staffPhoneDtos, List<StaffAuthorization> staffAuthorizations, string password);
    }
}
