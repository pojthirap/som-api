using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep17ImageInTemplateResult
    {
        public string SaleRepId { set; get; }
        public string SaleRepName { set; get; }
        public string VisitPlanId { get; set; }
        public string VisitPlanName { get; set; }
        public string VisitDate { set; get; }
        public string TemplateName { set; get; }
        public string ImageUrl { set; get; }
    }
}
