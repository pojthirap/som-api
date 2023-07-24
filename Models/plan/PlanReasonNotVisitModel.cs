using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.plan
{
    public class PlanReasonNotVisitModel : ModelBase
    {

        public string ReasonNotVisitId { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonNameTh { get; set; }
        public string ReasonNameEn { get; set; }
        public string ActiveFlag { get; set; }

    }
}
