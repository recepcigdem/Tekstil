using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Entities.Concrete.Dtos.Current;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using UI.Models.Common;

namespace UI.Models.Current
{
    public class Current : BaseModel
    {
        public int? CustomerId { get; set; }
        public int? CustomerTypeId { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }

        #region Injections

        private ICurrentEmailService _currentEmailService;
        private ICurrentPhoneService _currentPhoneService;
        private IEmailService _emailService;
        private IPhoneService _phoneService;

        #endregion

        #region Dtos
        public SubEntity<CurrentEmailDto> SubEmail { get; set; }
        public string SubEmailString
        {
            get { return JsonConvert.SerializeObject(SubEmail); }
            set { SubEmail.data = JsonConvert.DeserializeObject<List<CurrentEmailDto>>(value); }
        }

        public SubEntity<CurrentPhoneDto> SubPhone { get; set; }
        public string SubPhoneString
        {
            get { return JsonConvert.SerializeObject(SubPhone); }
            set { SubPhone.data = JsonConvert.DeserializeObject<List<CurrentPhoneDto>>(value); }
        }

        public List<CurrentEmailDto> ListCurrentEmail { get; set; }
        public List<CurrentPhoneDto> ListCurrentPhone { get; set; }

        #endregion

        public Current()
        {
            CustomerId = 0;
            CustomerTypeId = 0;
            IsCurrent = false;
            IsActive = false;
            Code = string.Empty;
            CustomerName = string.Empty;

            #region Mail

            SubEmail = new SubEntity<CurrentEmailDto>();
            ListCurrentEmail = new List<CurrentEmailDto>();

            #endregion

            #region Phones

            SubPhone = new SubEntity<CurrentPhoneDto>();
            ListCurrentPhone = new List<CurrentPhoneDto>();

            #endregion
        }
        public Current(HttpRequest request, Entities.Concrete.Customer customer, IStringLocalizer _localizerShared, ICurrentEmailService currentEmailService, ICurrentPhoneService currentPhoneService = null, IEmailService emailService = null, IPhoneService phoneService = null) : base(request)
        {
            _currentEmailService = currentEmailService;
            _currentPhoneService = currentPhoneService;
            _emailService = emailService;
            _phoneService = phoneService;

            ListCurrentEmail = new List<CurrentEmailDto>();
            ListCurrentPhone = new List<CurrentPhoneDto>();

            EntityId = customer.Id;
            CustomerId = customer.CustomerId;
            CustomerTypeId = customer.CustomerTypeId;
            IsCurrent = customer.IsCurrent;
            IsActive = customer.IsActive;
            Code = customer.Code;
            CustomerName = customer.CustomerName;


            #region CurrentEmail

            var currentMailList = _currentEmailService.GetAllByCurrentId(EntityId);
            foreach (CurrentEmail currentMail in currentMailList.Data)
            {
                var emailServiceList = _emailService.GetById(currentMail.EmailId);

                ListCurrentEmail
                    .Add(new CurrentEmailDto
                    {
                        Id = currentMail.Id,
                        CurrentId = currentMail.CurrentId,
                        EmailId = currentMail.EmailId,
                        IsMain = currentMail.IsMain,
                        EmailAddress = emailServiceList.Data.EmailAddress,
                        IsDeleted = emailServiceList.Data.IsDeleted
                    });
            }

            SubEmail = new SubEntity<CurrentEmailDto>();
            SubEmail.SetVisibility("CurrentId", false);
            SubEmail.SetVisibility("EmailId", false);
            SubEmail.SetVisibility("IsMain", true);
            SubEmail.SetVisibility("EmailAddress", true);

            SubEmail.SetLength("IsMain", 2);
            SubEmail.SetLength("EmailAddress", 10);

            SubEmail.SetHeader("IsMain", _localizerShared.GetString("IsMain"));
            SubEmail.SetHeader("MailAddress", _localizerShared.GetString("MailAddress"));

            foreach (CurrentEmailDto currentEmail in ListCurrentEmail)
            {
                this.SubEmail.data.Add(currentEmail);
            }
            #endregion

            #region CurrentPhone

            var currentPhoneList = _currentPhoneService.GetAllByCurrentId(EntityId);
            foreach (CurrentPhone currentPhone in currentPhoneList.Data)
            {
                var phoneServiceList = _phoneService.GetById(currentPhone.PhoneId);

                ListCurrentPhone
                    .Add(new CurrentPhoneDto()
                    {
                        Id = currentPhone.Id,
                        CurrentId = currentPhone.CurrentId,
                        PhoneId = currentPhone.PhoneId,
                        IsMain = currentPhone.IsMain,
                        CountryCode = phoneServiceList.Data.CountryCode,
                        AreaCode = phoneServiceList.Data.AreaCode,
                        PhoneNumber = phoneServiceList.Data.PhoneNumber,
                        IsDeleted = phoneServiceList.Data.IsDeleted
                    });
            }
            SubPhone = new SubEntity<CurrentPhoneDto>();
            SubPhone.SetVisibility("CurrentId", false);
            SubPhone.SetVisibility("PhoneId", false);
            SubPhone.SetVisibility("IsMain", true);
            SubPhone.SetVisibility("CountryCode", true);
            SubPhone.SetVisibility("AreaCode", true);
            SubPhone.SetVisibility("PhoneNumber", true);

            SubPhone.SetLength("IsMain", 1);
            SubPhone.SetLength("CountryCode", 3);
            SubPhone.SetLength("AreaCode", 3);
            SubPhone.SetLength("PhoneNumber", 5);

            SubPhone.SetHeader("IsMain", _localizerShared.GetString("IsMain"));
            SubPhone.SetHeader("CountryCode", _localizerShared.GetString("CountryCode"));
            SubPhone.SetHeader("AreaCode", _localizerShared.GetString("AreaCode"));
            SubPhone.SetHeader("PhoneNumber", _localizerShared.GetString("PhoneNumber"));

            foreach (CurrentPhoneDto currentPhone in ListCurrentPhone)
            {
                this.SubPhone.data.Add(currentPhone);
            }
            #endregion

        }
        public Entities.Concrete.Customer GetBusinessModel()
        {
            Entities.Concrete.Customer customer = new Entities.Concrete.Customer();
            if (EntityId > 0)
            {
                customer.Id = EntityId;
            }

            customer.CustomerId = CustomerId;
            customer.CustomerTypeId = CustomerTypeId;
            customer.IsCurrent = true;
            customer.IsActive = IsActive;
            customer.Code = Code;
            customer.CustomerName = CustomerName;

            #region Email

            ListCurrentEmail = new List<CurrentEmailDto>();
            foreach (var item in SubEmail.data)
            {
                CurrentEmailDto email = new CurrentEmailDto();

                if (item.Id > 0)
                {
                    email.Id = item.Id;
                }

                email.CurrentId = this.EntityId;
                email.EmailId = item.EmailId;
                email.IsMain = item.IsMain;
                email.IsActive = item.IsActive;
                email.EmailAddress = item.EmailAddress;

                ListCurrentEmail.Add(email);
            }

            #endregion

            #region Phone

            ListCurrentPhone = new List<CurrentPhoneDto>();
            foreach (var item in SubPhone.data)
            {
                CurrentPhoneDto phone = new CurrentPhoneDto();

                if (item.Id > 0)
                {
                    phone.Id = item.Id;
                }

                phone.CurrentId = this.EntityId;
                phone.PhoneId = item.PhoneId;
                phone.IsMain = item.IsMain;
                phone.IsActive = item.IsActive;
                phone.CountryCode = item.CountryCode;
                phone.AreaCode = item.AreaCode;
                phone.PhoneNumber = item.PhoneNumber;

                ListCurrentPhone.Add(phone);
            }

            #endregion


            return customer;
        }
    }
}