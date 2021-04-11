using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Concrete;

namespace UI.Models.AgeGroup
{
    public class AgeGroupList
    {
        public List<AgeGroupListLine> data { get; set; }

        private AgeGroupManager _ageGroupManager;

        public AgeGroupList(AgeGroupManager ageGroupManager)
        {
            _ageGroupManager = ageGroupManager;
            var listGrid = _ageGroupManager.GetAll().Data;
            data = new List<AgeGroupListLine>();
            foreach (var item in listGrid)
            {
                AgeGroupListLine line = new AgeGroupListLine(item);
                data.Add(line);
            }
        }
    }
}
