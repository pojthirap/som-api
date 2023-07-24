using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchEmailJobForPlanTripCustom
    {

        public string EmailTemplateName { get; set; }
        public string PlanTripName { get; set; }
        public string PlanTripId { get; set; }
        public string PlanTripDate { get; set; }
        public string SaleRepName { get; set; }
        public string SaleSupName { get; set; }
        public string EmailTemplateId { get; set; }


    }
}
