using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.CsNoDeliveryDate
{
    public class CsNoDeliveryDate : BaseModel
    {
        public int SeasonId { get; set; }
        public string Csno { get; set; }
        public DateTime Date { get; set; }

        public List<CsNoDeliveryDateHistory> ListCsNoDeliveryDateHistories { get; set; }

        private ICsNoDeliveryDateHistoryService _csNoDeliveryDateHistoryService;

        public CsNoDeliveryDate() : base()
        {
            SeasonId = 0;
            Csno = string.Empty;
            Date = DateTime.UtcNow;

            ListCsNoDeliveryDateHistories = new List<CsNoDeliveryDateHistory>();
        }
        public CsNoDeliveryDate(HttpRequest request, Entities.Concrete.CsNoDeliveryDate csNoDeliveryDate, IStringLocalizer _localizerShared, ICsNoDeliveryDateHistoryService csNoDeliveryDateHistoryService) : base(request)
        {
            _csNoDeliveryDateHistoryService = csNoDeliveryDateHistoryService;

            EntityId = csNoDeliveryDate.Id;
            CustomerId = Helpers.SessionHelper.GetStaff(request).CustomerId;
            SeasonId = csNoDeliveryDate.SeasonId;
            Csno = csNoDeliveryDate.Csno;
            Date = csNoDeliveryDate.Date;

            #region CsNoDeliveryDateHistory

            ListCsNoDeliveryDateHistories = new List<CsNoDeliveryDateHistory>();

            var csNoDeliveryDateHistoryList = _csNoDeliveryDateHistoryService.GetAllByCsNoDeliveryDate(EntityId);
            if (csNoDeliveryDateHistoryList.Data.Count > 0)
            {
                foreach (var csNoDeliveryDateHistory in csNoDeliveryDateHistoryList.Data)
                {
                    ListCsNoDeliveryDateHistories.Add(csNoDeliveryDateHistory);
                }
            }

            #endregion
        }

        public Entities.Concrete.CsNoDeliveryDate GetBusinessModel()
        {
            Entities.Concrete.CsNoDeliveryDate csNoDeliveryDate = new Entities.Concrete.CsNoDeliveryDate();
            if (EntityId > 0)
            {
                csNoDeliveryDate.Id = EntityId;
            }

            csNoDeliveryDate.CustomerId = CustomerId;
            csNoDeliveryDate.SeasonId = SeasonId;
            csNoDeliveryDate.Csno = Csno;
            csNoDeliveryDate.Date = Date;


            return csNoDeliveryDate;
        }
    }
}
