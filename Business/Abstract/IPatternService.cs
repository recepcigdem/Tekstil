using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IPatternService
    {
        IDataResult<List<Pattern>> GetAll();
        IDataResult<Pattern> GetById(int patternId);
        IResult Add(Pattern pattern);
        IResult Update(Pattern pattern);
        IResult Delete(Pattern pattern);
    }
}
