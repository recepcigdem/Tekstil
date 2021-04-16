﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.Dtos;

namespace Business.Abstract
{
    public interface IStaffEmailService
    {
        IDataResult<List<StaffEmail>> GetAll();
        IDataResult<StaffEmail> GetById(int staffEmailId);
        IResult Add(StaffEmail staffEmail);
        IResult Update(StaffEmail staffEmail);
        IResult Delete(StaffEmail staffEmail);
        IResult DeleteByStaffIdWithEmail(int staffId);
        IResult Save(StaffEmailDto staffEmailDto);
    }
}
