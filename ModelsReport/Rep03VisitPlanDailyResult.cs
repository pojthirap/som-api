using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep03VisitPlanDailyResult
    {
        public string visitDate { set; get; }
        public string startCheckinMileNo { set; get; }
        public string stopCheckinMileNo { set; get; }
        public string totalKmInput { set; get; }
        public string totalKmSystem { set; get; }
        public string locationName { set; get; }
        public string status { set; get; }
        public string totalKmInputForCalc { set; get; }
        public string totalKmSystemForCalc { set; get; }

    }
}
