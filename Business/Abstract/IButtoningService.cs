using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IButtoningService
    {
        IDataResult<List<Buttoning>> GetAll();
        IDataResult<Buttoning> GetById(int buttoningId);
        IResult Add(Buttoning buttoning);
        IResult Update(Buttoning buttoning);
        IResult Delete(Buttoning buttoning);

    }
}
