using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.plan
{
    public class PlanTripModel : ModelBase
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
        public string StopCalcKm { get; set; }

    }
}
