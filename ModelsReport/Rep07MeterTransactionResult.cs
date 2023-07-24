using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep07MeterTransactionResult
    {
        public string EmpId { set; get; }
        public string SaleName { set; get; }
        public string GroupCode { set; get; }
        public string DescriptionTh { set; get; }
        public string CustCode { set; get; }
        public string CustNameTh { set; get; }
        public string CntDispenserNno { set; get; }
        public string CntNozzle { set; get; }
        public string PrevRecDate { set; get; }
        public string RecDate { set; get; }
        public string PrevPlanTripId { set; get; }
        public string PlanTripId { set; get; }
        public string MeterVisit { set; get; }
        public string MeterSummation { set; get; }

    }
}
