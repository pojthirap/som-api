using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.plan
{
    public class PlanTripTaskModel : ModelBase
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


    }
}
