using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep01VisitPlanRawDataResult
    {
        public string prospId { set; get; }
        public string saleRepName { set; get; }
        public string saleRepId { set; get; }
        public string visitPlanId { set; get; }
        public string visitPlanName { set; get; }
        public string visitType { set; get; }
        public string visitDate { set; get; }
        public string accName { set; get; }
        public string prospectType { set; get; }
        public string startMileNo { set; get; }
        public string finishMileNo { set; get; }
        public string totalKmInput { set; get; }
        public string totalKmSystem { set; get; }
        public string planStartTime { set; get; }
        public string planEndTime { set; get; }
        public string visitCheckinDtm { set; get; }
        public string visitCheckoutDtm { set; get; }
        public string reasonNameTh { set; get; }

    }
}
