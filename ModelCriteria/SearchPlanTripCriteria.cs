using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria
{
    public class SearchPlanTripCriteria
    {
        public String Calendar { get; set; }
        public string PlanTripDate { get; set; }
        public string[] Status { get; set; }
        public string AssignEmpId { get; set; }
    }
}
