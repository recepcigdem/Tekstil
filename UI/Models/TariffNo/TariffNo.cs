using Business.Abstract;
using Entities.Concrete;
using Entities.Concrete.Dtos.TariffNo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Models.Common;

namespace UI.Models.TariffNo
{
    public class TariffNo : BaseModel
    {
        public bool IsUsed { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public List<TariffNoDetail> ListTariffNoDetails { get; set; }

        public string SubTariffNoDetailString
        {
            get { return JsonConvert.SerializeObject(ListTariffNoDetails); }
            set { ListTariffNoDetails = JsonConvert.DeserializeObject<List<TariffNoDetail>>(value); }
        }
   
        public List<TariffNoDetail> TariffNoDetails { get; set; }

        private ITariffNoDetailService _tariffNoDetailService;
        private ISeasonService _seasonService;
        private ISeasonCurrencyService _seasonCurrencyService;
        private IDefinitionService _difinitionService;

        public TariffNo() : base()
        {
            IsActive = false;
            IsUsed = false;
            Description = string.Empty;
            Code = string.Empty;

            ListTariffNoDetails = new List<TariffNoDetail>();
            TariffNoDetails = new List<TariffNoDetail>();
            
        }

        public TariffNo(HttpRequest request, Entities.Concrete.TariffNo tariffNo, IStringLocalizer _localizerShared, ISeasonService seasonService, ISeasonCurrencyService seasonCurrencyService, IDefinitionService difinitionService, ITariffNoDetailService tariffNoDetailService) : base(request)
        {
            _seasonService = seasonService;
            _seasonCurrencyService = seasonCurrencyService;
            _difinitionService = difinitionService;
            _tariffNoDetailService = tariffNoDetailService;

            EntityId = tariffNo.Id;
            CustomerId = tariffNo.CustomerId;
            IsActive = tariffNo.IsActive;
            IsUsed = tariffNo.IsUsed;
            Description = tariffNo.Description;
            Code = tariffNo.Code;

            #region TariffNoDetail

            ListTariffNoDetails = new List<TariffNoDetail>();
            TariffNoDetails = new List<TariffNoDetail>();

            var tariffNoDetail = _tariffNoDetailService.GetAllByTariffNo(EntityId);
            if (tariffNoDetail.Result == true)
            {
                foreach (var item in tariffNoDetail.Data)
                {
                    ListTariffNoDetails.Add(item);
                }
            }

            #endregion

        }

        public Entities.Concrete.TariffNo GetBusinessModel()
        {
            Entities.Concrete.TariffNo tariffNo = new Entities.Concrete.TariffNo();
            if (EntityId > 0)
            {
                tariffNo.Id = EntityId;
            }
            tariffNo.CustomerId = CustomerId;
            tariffNo.IsActive = IsActive;
            tariffNo.IsUsed = IsUsed;
            tariffNo.Description = Description;
            tariffNo.Code = Code;

            #region TariffNoDetail

            TariffNoDetails = new List<TariffNoDetail>();
            foreach (var item in ListTariffNoDetails)
            {
                TariffNoDetail tariffNoDetail = new TariffNoDetail();

                if (item.Id > 0)
                {
                    tariffNoDetail.Id = item.Id;
                }

                tariffNoDetail.CustomerId = CustomerId;
                tariffNoDetail.TariffNoId = EntityId;
                tariffNoDetail.SeasonId = item.SeasonId;
                tariffNoDetail.CountryId = item.CountryId;
                tariffNoDetail.SeasonCurrencyId = item.SeasonCurrencyId;
                tariffNoDetail.IsUsed = item.IsUsed;
                tariffNoDetail.Tax = item.Tax;
                tariffNoDetail.Stamp = item.Stamp;
                tariffNoDetail.AzoTest = item.AzoTest;
                tariffNoDetail.DifferentExpense = item.DifferentExpense;
                tariffNoDetail.Commission = item.Commission;
                tariffNoDetail.Kkdf = item.Kkdf;
                tariffNoDetail.AdditionalTax = item.AdditionalTax;
                tariffNoDetail.AdditionalTax1 = item.AdditionalTax1;
                tariffNoDetail.Superintendence = item.Superintendence;
                tariffNoDetail.UnitKg = item.UnitKg;
                tariffNoDetail.KgTranslation = item.KgTranslation;

                TariffNoDetails.Add(tariffNoDetail);
            }

            #endregion

            return tariffNo;
        }
    }
}