using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchPlanTripForSaleVisitCustom
    {
        public decimal PlanTripId { get; set; }
        public string PlanTripName { get; set; }
        public DateTime PlanTripDate { get; set; }
        public string AssignEmpId { get; set; }
        public string Remark { get; set; }
        public decimal? StartCheckinLocId { get; set; }
        public decimal? StartCheckinMileNo { get; set; }
        public DateTime? StartCheckinDtm { get; set; }
        public DateTime? StartCheckoutDtm { get; set; }
        public decimal? StopCheckinLocId { get; set; }
        public decimal? StopCheckinMileNo { get; set; }
        public DateTime? StopCheckinDtm { get; set; }
        public string Status { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDtm { get; set; }
        public decimal? StopCalcKm { get; set; }



        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StatusDesc { get; set; }
        public string TotalKmSystem { get; set; }

    }
}
