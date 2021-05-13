using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace UI.Models.Test
{
    public class Test : BaseModel
    {    
        public bool IsUsed { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Norm { get; set; }
        public string Value { get; set; }

        public Test() : base()
        {
            IsUsed = false;
            IsActive = true;
            Code = string.Empty;
            Description = string.Empty;
            Norm = string.Empty;
            Value = string.Empty;
        }
        public Test(HttpRequest request, Entities.Concrete.Test test, IStringLocalizer _localizerShared) : base(request)
        {
            EntityId = test.Id;
            CustomerId = test.CustomerId;
            IsActive = test.IsActive;
            IsUsed = test.IsUsed;
            Code = test.Code;
            Description = test.Description;
            Norm = test.Norm;
            Value = test.Value;
        }
        public Entities.Concrete.Test GetBusinessModel()
        {
            Entities.Concrete.Test test = new Entities.Concrete.Test();
            if (EntityId > 0)
            {
                test.Id = EntityId;
            }

            test.CustomerId = CustomerId;
            test.IsActive = IsActive;
            test.IsUsed = IsUsed;
            test.Code = Code;
            test.Description = Description;
            test.Norm = Norm;
            test.Value = Value;

            return test;
        }
    }
}
