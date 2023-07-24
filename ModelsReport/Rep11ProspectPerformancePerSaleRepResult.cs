using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep11ProspectPerformancePerSaleRepResult
    {
        public string saleRepName { set; get; }
        public string saleRepId { set; get; }
        public string saleGroupDesc { set; get; }
        public string totalProspect { set; get; }
        public string totalProspectChange { set; get; }
        public string performPercent { set; get; }
    }
}
