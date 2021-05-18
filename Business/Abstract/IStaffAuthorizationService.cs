using Core.Utilities.Results;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IStaffAuthorizationService
    {
        IDataServiceResult<List<StaffAuthorization>> GetAll(int customerId);
        IDataServiceResult<List<StaffAuthorization>> GetAllByStaffId(int staffId);
        IDataServiceResult<List<StaffAuthorization>> GetAllByAuthorizationId(int authorizationId);
        IDataServiceResult<StaffAuthorization> GetById(int staffAuthorizationId);
        IServiceResult GetByStaffId(int staffId);
        IServiceResult Add(StaffAuthorization staffAuthorization);
        IServiceResult Update(StaffAuthorization staffAuthorization);
        IServiceResult Delete(StaffAuthorization staffAuthorization);
        IServiceResult DeleteByStaff(Staff staff);
        IDataServiceResult<StaffAuthorization> Save(Staff staff, List<StaffAuthorization> staffAuthorizations);
    }
}
