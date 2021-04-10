using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IHandiworkService
    {
        IDataResult<List<Handiwork>> GetAll();
        IDataResult<Handiwork> GetById(int handiworkId);
        IResult Add(Handiwork handiwork);
        IResult Update(Handiwork handiwork);
        IResult Delete(Handiwork handiwork);
    }
}
