using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ILoginService
    {
        DataServiceResult<Staff> DbLoginControl(string email, string password);
        ServiceResult CreateLoginToken(string email);
        DataServiceResult<Staff> Login(string email, string password);
        ServiceResult ForgotPassword(string email);
        ServiceResult EMailAddressControl(string email);
        ServiceResult PasswordControl(string password);

    }
}
