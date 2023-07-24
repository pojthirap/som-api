using MyFirstAzureWebApp.ModelCriteriaReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep15SaleOrderByCompanyResult : OutboundSaleOrderInformation
    {
        public List<Company> Company { get; set; }

    }
}
