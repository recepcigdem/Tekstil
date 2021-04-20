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
    public interface IStaffService
    {
        IDataResult<List<Staff>> GetAll();
        IDataResult<Staff> GetById(int staffId);
        IResult Add(Staff staff);
        IResult Update(Staff staff);
        IResult Delete(Staff staff);
        IDataResult<Staff> SaveAll(Staff staff, List<StaffEmailDto> staffEmailDtos, List<StaffPhoneDto> staffPhoneDtos, List<StaffAuthorization> staffAuthorizations);
    }
}
