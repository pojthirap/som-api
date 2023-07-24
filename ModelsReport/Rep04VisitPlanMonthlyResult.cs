using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep04VisitPlanMonthlyResult
    {
        public string visitDate { set; get; }
        public string visitTimeStart { set; get; }
        public string startDheckinMileNo { set; get; }
        public string locStartName { set; get; }
        public string visitTimeStop { set; get; }
        public string stopCheckinMileNo { set; get; }
        public string locEndName { set; get; }
        public string totalKmInput { set; get; }
        public string contactName { set; get; }
        public string address { set; get; }
        public string visitDetail { set; get; }

    }
}
