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
        IDataResult<List<StaffAuthorization>> GetAll();
        IDataResult<StaffAuthorization> GetById(int staffAuthorizationId);
        IResult Add(StaffAuthorization staffAuthorization);
        IResult Update(StaffAuthorization staffAuthorization);
        IResult Delete(StaffAuthorization staffAuthorization);
    }
}
