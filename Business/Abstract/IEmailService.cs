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
        IDataServiceResult<List<Email>> GetAll();
        IDataServiceResult<Email> GetById(int emailId);
        IDataServiceResult<Email> GetByEmail(string email);
        IServiceResult Add(Email email);
        IServiceResult Update(Email email);
        IServiceResult Delete(Email email);
        IDataServiceResult<Email> Save(Email email);
    }
}
