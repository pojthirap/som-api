using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria
{
    public class GetTaskMeterForRecordCriteria
    {
        public string PlanTripTaskId { get; set; }
        public string ProspId { get; set; }
        public string PlanTripProspId { get; set; }
        public string CustCode { get; set; }
    }
}
