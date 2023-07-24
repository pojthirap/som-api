using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria
{
    public class SearchProspectVisitHourCriteria
    {
        public string ProspVisitHrId { get; set; }
        public string ProspectId { get; set; }
        public string DaysCode { get; set; }
        public string HourStart { get; set; }
        public string HourEnd { get; set; }
        public string ActiveFlag { get; set; }
    }
}
