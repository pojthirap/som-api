using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep02VisitPlanTransactionResult
    {
        public string SaleRepId { set; get; }
        public string SaleRepName { set; get; }
        //public string StartDate { get; set; }
        //public string EndDate { get; set; }
        public string TotalKmInput { set; get; }
        public string TotalKmSystem { set; get; }

    }
}
