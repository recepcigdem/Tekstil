using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IEmailService
    {
        IDataResult<List<Email>> GetAll();
        IDataResult<Email> GetById(int emailId);
        IDataResult<Email> GetByEmail(string email);
        IResult Add(Email email);
        IResult Update(Email email);
        IResult Delete(Email email);
        IResult DeleteByEmailId(int emailId);
        IResult Save(Email email);
    }
}
