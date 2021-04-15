using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using UI.Models.Common;

namespace UI.Models.Staff
{
    public class Staff : BaseModel
    {
        public int CustomerId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsLeaving { get; set; }
        public DateTime LeavingDate { get; set; }
        public bool IsSendMail { get; set; }
        public string Photo { get; set; }

        public string RootPath { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsCompanyAdmin { get; set; }
        public bool IsStandartUser { get; set; }
        public bool IsSuperAdminControl { get; set; }
        public bool IsCompanyAdminControl { get; set; }

        public string UserPhotoBase64 { get; set; }

        #region Injections

        private IStaffEmailService _staffEmailService;
        private IStaffPhoneService _staffPhoneService;
        private IStaffAuthorizationService _staffAuthorizationService;
        private IEmailService _emailService;
        private IPhoneService _phoneService;
       
        #endregion

        #region Dtos
        public SubEntity<StaffEmailDto> SubEmail { get; set; }
        public string SubEmailString
        {
            get { return JsonConvert.SerializeObject(SubEmail); }
            set { SubEmail.data = JsonConvert.DeserializeObject<List<StaffEmailDto>>(value); }
        }

        public SubEntity<StaffPhoneDto> SubPhone { get; set; }
        public string SubPhoneString
        {
            get { return JsonConvert.SerializeObject(SubPhone); }
            set { SubPhone.data = JsonConvert.DeserializeObject<List<StaffPhoneDto>>(value); }
        }
        public List<StaffEmailDto> ListStaffEmail { get; set; }
        public List<StaffPhoneDto> ListStaffPhone { get; set; }


        #endregion

        public List<StaffAuthorization> StaffAuthorizations { get; set; }
        public string StaffAuthorizationString
        {
            get { return JsonConvert.SerializeObject(StaffAuthorizations); }
            set { StaffAuthorizations = JsonConvert.DeserializeObject<List<StaffAuthorization>>(value); }
        }
        public List<StaffAuthorization> ListStaffAuthorizations { get; set; }


        public Staff() : base()
        {
            CustomerId = 0;
            DepartmentId = 0;
            IsActive = false;
            Title = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Password = string.Empty;
            PasswordSalt = string.Empty;
            RegisterDate = DateTime.UtcNow;
            IsLeaving = false;
            LeavingDate = DateTime.UtcNow;
            IsSendMail = false;
            Photo = string.Empty;

            RootPath = string.Empty;
            IsStandartUser = false;
            IsSuperAdmin = false;
            IsCompanyAdmin = false;
            IsSuperAdminControl = false;
            IsCompanyAdminControl = false;
            UserPhotoBase64 = string.Empty;

            #region Mail

            SubEmail = new SubEntity<StaffEmailDto>();
            ListStaffEmail = new List<StaffEmailDto>();

            #endregion

            #region Phones

            SubPhone = new SubEntity<StaffPhoneDto>();
            ListStaffPhone = new List<StaffPhoneDto>();

            #endregion

            #region Authorization

            StaffAuthorizations = new List<StaffAuthorization>();
            ListStaffAuthorizations = new List<StaffAuthorization>();

            #endregion
        }

        public Staff(HttpRequest request, Entities.Concrete.Staff staff, IStringLocalizer _localizerShared, string rootPath, IStaffEmailService staffEmailService, IStaffPhoneService staffPhoneService, IStaffAuthorizationService staffAuthorizationService, IEmailService emailService, IPhoneService phoneService) : base(request)
        {

            _staffEmailService = staffEmailService;
            _staffPhoneService = staffPhoneService;
            _staffAuthorizationService = staffAuthorizationService;
            _emailService = emailService;
            _phoneService = phoneService;

            RootPath = rootPath;

            ListStaffEmail = new List<StaffEmailDto>();
            ListStaffPhone = new List<StaffPhoneDto>();
            StaffSession resultStaffSession = Helpers.SessionHelper.GetStaff(request);
            if (resultStaffSession != null)
            {
                this.IsSuperAdminControl = resultStaffSession.IsSuperAdmin;
                this.IsCompanyAdminControl = resultStaffSession.IsCompanyAdmin;
            }
            CustomerId = staff.CustomerId;
            DepartmentId = staff.DepartmentId;
            IsActive = staff.IsActive;
            Title = staff.Title;
            FirstName = staff.FirstName;
            LastName = staff.LastName;
            Password = staff.Password;
            PasswordSalt = staff.PasswordSalt;
            RegisterDate = staff.RegisterDate;
            IsLeaving = staff.IsLeaving;
            LeavingDate = staff.LeavingDate;
            IsSendMail = staff.IsSendMail;
            Photo = staff.Photo;
            IsSuperAdmin = staff.IsSuperAdmin;
            IsCompanyAdmin = staff.IsCompanyAdmin;
            if (staff.IsCompanyAdmin == false && staff.IsSuperAdmin == false)
            {
                IsStandartUser = true;
            }

            if (!string.IsNullOrWhiteSpace(Photo))
            {
                string fileName = rootPath + Photo;
                byte[] binaryContent = File.ReadAllBytes(fileName);
                this.UserPhotoBase64 = Convert.ToBase64String(binaryContent, 0, binaryContent.Length);
            }
            else
            {
                this.Photo = "/assets/images/placeholder.png";
                this.UserPhotoBase64 = string.Empty;
            }

            #region StaffEmail

            var staffMailList = _staffEmailService.GetAll();
            foreach (StaffEmail staffMail in staffMailList.Data)
            {
                var emailServiceList = _emailService.GetById(staffMail.EmailId);

                ListStaffEmail
                    .Add(new StaffEmailDto
                    {
                        Id = staffMail.Id,
                        StaffId = staffMail.StaffId,
                        EmailId = staffMail.EmailId,
                        IsMain = staffMail.IsMain,
                        EmailAddress = emailServiceList.Data.EmailAddress,
                        IsDeleted = emailServiceList.Data.IsDeleted
                    });
            }
            SubEmail = new SubEntity<StaffEmailDto>();
            SubEmail.SetVisibility("StaffId", false);
            SubEmail.SetVisibility("EmailId", false);
            SubEmail.SetVisibility("IsMain", true);
            SubEmail.SetVisibility("EmailAddress", true);

            SubEmail.SetLength("IsMain", 2);
            SubEmail.SetLength("EmailAddress", 10);

            SubEmail.SetHeader("IsMain", _localizerShared.GetString("IsMain"));
            SubEmail.SetHeader("MailAddress", _localizerShared.GetString("MailAddress"));

            foreach (StaffEmailDto staffEmail in ListStaffEmail)
            {
                this.SubEmail.data.Add(staffEmail);
            }
            #endregion

            #region StaffPhone

            var staffPhoneList = _staffPhoneService.GetAll();
            foreach (StaffPhone staffPhone in staffPhoneList.Data)
            {
                var phoneServiceList = _phoneService.GetById(staffPhone.PhoneId);

                ListStaffPhone
                    .Add(new StaffPhoneDto()
                    {
                        Id = staffPhone.Id,
                        StaffId = staffPhone.StaffId,
                        PhoneId = staffPhone.PhoneId,
                        IsMain = staffPhone.IsMain,
                        CountryCode = phoneServiceList.Data.CountryCode,
                        AreaCode = phoneServiceList.Data.AreaCode,
                        PhoneNumber = phoneServiceList.Data.PhoneNumber,
                        IsDeleted = phoneServiceList.Data.IsDeleted
                    });
            }
            SubPhone = new SubEntity<StaffPhoneDto>();
            SubPhone.SetVisibility("StaffId", false);
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

            foreach (StaffPhoneDto staffPhone in ListStaffPhone)
            {
                this.SubPhone.data.Add(staffPhone);
            }
            #endregion

            #region StaffAuthorization

            ListStaffAuthorizations = _staffAuthorizationService.GetAll().Data;
            this.StaffAuthorizations = new List<StaffAuthorization>();
            foreach (var staffAuthorization in ListStaffAuthorizations)
            {
                if (staffAuthorization != null)
                {
                    this.StaffAuthorizations.Add(staffAuthorization);
                }
            }
            #endregion
        }

        public Entities.Concrete.Staff GetBusinessModel()
        {
            Entities.Concrete.Staff staff = new Entities.Concrete.Staff();
            if (EntityId > 0)
            {
                staff.Id = EntityId;
            }
            staff.CustomerId = CustomerId;
            staff.DepartmentId = DepartmentId;
            staff.IsActive = IsActive;
            staff.Title = Title;
            staff.FirstName = FirstName;
            staff.LastName = LastName;
            staff.Password = Password;
            staff.PasswordSalt = PasswordSalt;
            staff.RegisterDate = RegisterDate;
            staff.IsLeaving = IsLeaving;
            staff.LeavingDate = LeavingDate;
            staff.IsSendMail = IsSendMail;
            staff.Photo = Photo;
            staff.IsSuperAdmin = IsSuperAdmin;
            staff.IsCompanyAdmin = IsCompanyAdmin;

            if (!string.IsNullOrWhiteSpace(this.UserPhotoBase64))
            {
                var textBase64 = "data:image/jpeg;base64,";
                bool IsContains = this.UserPhotoBase64.Contains(textBase64);
                if (IsContains)
                {
                    var img = Helpers.ImageHelper.GetImageFromBase64(this.UserPhotoBase64);
                    if (img != null)
                    {
                        string fileName = RootPath + "\\assets\\photos\\" + img.FileName;
                        File.WriteAllBytes(fileName, img.ImageContent);
                        Photo = "\\assets\\photos\\" + img.FileName;
                    }
                }
                else
                {
                    this.UserPhotoBase64 = textBase64 + this.UserPhotoBase64;
                    var img = Helpers.ImageHelper.GetImageFromBase64(this.UserPhotoBase64);
                    if (img != null)
                    {
                        string fileName = RootPath + "\\assets\\photos\\" + img.FileName;
                        File.WriteAllBytes(fileName, img.ImageContent);
                        Photo = "\\assets\\photos\\" + img.FileName;
                    }
                }

            }
            else
                Photo = string.Empty;

            #region Email

            ListStaffEmail = new List<StaffEmailDto>();
            foreach (var item in SubEmail.data)
            {
                StaffEmailDto email = new StaffEmailDto();

                if (item.Id > 0)
                {
                    email.Id = item.Id;
                }

                email.StaffId = item.StaffId;
                email.EmailId = item.EmailId;
                email.IsMain = item.IsMain;
                email.EmailAddress = item.EmailAddress;

                ListStaffEmail.Add(email);
            }

            #endregion

            #region Phone

            ListStaffPhone = new List<StaffPhoneDto>();
            foreach (var item in SubPhone.data)
            {
                StaffPhoneDto phone = new StaffPhoneDto();

                if (item.Id > 0)
                {
                    phone.Id = item.Id;
                }

                phone.StaffId = item.StaffId;
                phone.PhoneId = item.PhoneId;
                phone.IsMain = item.IsMain;
                phone.CountryCode = item.CountryCode;
                phone.AreaCode = item.AreaCode;
                phone.PhoneNumber = item.PhoneNumber;

                ListStaffPhone.Add(phone);
            }

            #endregion

            #region StaffAuthorization

            ListStaffAuthorizations = new List<StaffAuthorization>();
            foreach (var item in StaffAuthorizations)
            {
                StaffAuthorization staffAuthorizations = new StaffAuthorization();


                var service = _staffAuthorizationService.GetById(item.AuthorizationId);
                if (service != null)
                {
                    staffAuthorizations.Id = service.Data.Id;
                }

                staffAuthorizations.StaffId = this.EntityId;
                staffAuthorizations.AuthorizationId = item.AuthorizationId;
                ListStaffAuthorizations.Add(staffAuthorizations);

            }
            #endregion

            return staff;
        }
    }
}
