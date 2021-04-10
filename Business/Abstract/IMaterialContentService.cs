using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IMaterialContentService
    {
        IDataResult<List<MaterialContent>> GetAll();
        IDataResult<MaterialContent> GetById(int materialContentId);
        IResult Add(MaterialContent materialContent);
        IResult Update(MaterialContent materialContent);
        IResult Delete(MaterialContent materialContent);
    }
}
