using Business.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Test
{
    public class TestList
    {
        public List<TestListLine> data { get; set; }

        private ITestService _testService;

        public TestList(HttpRequest request, ITestService testService)
        {
            var customerId = Helpers.SessionHelper.GetStaff(request).CustomerId;
            _testService = testService;

            var listGrid = _testService.GetAll(customerId);

            if (listGrid != null)
            {
                data = new List<TestListLine>();

                foreach (var item in listGrid.Data)
                {
                    TestListLine line = new TestListLine(item);
                    data.Add(line);
                }
            }
        }
    }
}
