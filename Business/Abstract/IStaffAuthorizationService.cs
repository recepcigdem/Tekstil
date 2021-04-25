using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IStaffAuthorizationService
    {
        IDataServiceResult<List<StaffAuthorization>> GetAll();
        IDataServiceResult<List<StaffAuthorization>> GetAllByStaffId(int staffId);
        IDataServiceResult<StaffAuthorization> GetById(int ageGroupId);
        IServiceResult Add(StaffAuthorization staffAuthorization);
        IServiceResult Update(StaffAuthorization staffAuthorization);
        IServiceResult Delete(StaffAuthorization staffAuthorization);
        IServiceResult DeleteByStaff(Staff staff);
        IDataServiceResult<StaffAuthorization> Save(StaffAuthorization staffAuthorization);
    }
}
