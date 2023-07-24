using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.plan
{
    public class UpdatePlanTripModel
    {
        public PlanTripForUpdatePlanTrip PlanTrip { get; set; }
        public List<PlanTripProspectUpdateModel> ListProspect { get; set; }

        public class PlanTripProspectUpdateModel
        {
            public List<PlanTripTaskForUpdatePlanTrip> ListTask { get; set; }

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
            public string MergPlanTripProspId { get; set; }
            public string LocId { get; set; }
            public string LocRemark { get; set; }

            public string AccName { get; set; }
            public string CustCode { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string OpenFlag { get; set; }
        }


    }
    public class PlanTripTaskForUpdatePlanTrip
    {
        public string PlanTripTaskId { get; set; }
        public string PlanTripProspId { get; set; }
        public string TaskType { get; set; }
        public string TpStockCardId { get; set; }
        public string TpSaFormId { get; set; }
        public string TpAppFormId { get; set; }
        public string RequireFlag { get; set; }
        public string OrderNo { get; set; }
        public string AdhocFlag { get; set; }
        public string CreateUser { get; set; }
        public string CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public string UpdateDtm { get; set; }
        public string TemplateName { get; set; }
        public string CompletedFlag { get; set; }
    }

    public class PlanTripForUpdatePlanTrip
    {

        public string PlanTripId { get; set; }
        public string PlanTripName { get; set; }
        public string PlanTripDate { get; set; }
        public string AssignEmpId { get; set; }
        public string Remark { get; set; }
        public string StartCheckinLocId { get; set; }
        public string StartCheckinMileNo { get; set; }
        public string StartCheckinDtm { get; set; }
        public string StartCheckoutDtm { get; set; }
        public string StopCheckinLocId { get; set; }
        public string StopCheckinMileNo { get; set; }
        public string StopCheckinDtm { get; set; }
        public string Status { get; set; }
        public string CreateUser { get; set; }
        public string CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public string UpdateDtm { get; set; }
        public string StopCalcKm { get; set; }
        public string MergPlanTripId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TitleName { get; set; }
    }


}
