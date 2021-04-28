using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using UI.Models.Authorization;

namespace UI.Models.AgeGroup
{
    public class AgeGroupList
    {
        public List<AgeGroupListLine> data { get; set; }

        private IAgeGroupService _ageGroupService;

        public AgeGroupList(IAgeGroupService ageGroupService)
        {
            _ageGroupService = ageGroupService;
            var listGrid = _ageGroupService.GetAll().Data;
            if (listGrid != null)
            {
                data = new List<AgeGroupListLine>();
                foreach (var item in listGrid)
                {
                    AgeGroupListLine line = new AgeGroupListLine(item);
                    data.Add(line);
                }
            }
        }
    }
}
