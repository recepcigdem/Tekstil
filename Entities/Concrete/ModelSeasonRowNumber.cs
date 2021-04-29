using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ModelSeasonRowNumber : BaseCustomerEntity
    {
        public int SeasonId { get; set; }
        public int ProductGroupId { get; set; }
        public int RowNumber { get; set; }
        public bool IsActive { get; set; }

        public ModelSeasonRowNumber()
        {
            SeasonId = 0;
            ProductGroupId = 0;
            RowNumber = 0;
            IsActive = false;
        }
    }
}
