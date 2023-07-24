using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.plan
{
    public class CheckOutPlanTripProspectModel : ModelBase
    {

        public string PlanTripProspId { get; set; }
        public string PlanTripId { get; set; }
        public string ProspId { get; set; }
        public string PlanStartTime { get; set; }
        public string PlanEndTime { get; set; }
        public string OrderNo { get; set; }
        public string AdhocFlag { get; set; }
        public string MergFlag { get; set; }
        public string VisitLatitude { get; set; }
        public string VisitLongitude { get; set; }
        public string VisitCheckinMileNo { get; set; }
        public string VisitCheckinDtm { get; set; }
        public string VisitCheckoutDtm { get; set; }
        public string Remind { get; set; }
        public string ReasonNotVisitId { get; set; }
        public string ReasonNotVisitRemark { get; set; }
        public string CreateUser { get; set; }
        public string CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public string UpdateDtm { get; set; }
        public string VisitCalcKm { get; set; }
        public string MergPlanTripId { get; set; }
        public string LocId { get; set; }
        public string LocRemark { get; set; }


        public string CurrentPlanTripProspId { get; set; }
        public string CurrentVisitCalcKm { get; set; }
        public string UpdPlanTripProspId { get; set; }
        public string UpdVisitCalcKm { get; set; }




        public string ContactName { get; set; }
        public string ContactMobileNo { get; set; }
        public string CheckoutRemark { get; set; }

    }
}
