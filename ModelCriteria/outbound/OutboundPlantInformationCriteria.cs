using MyFirstAzureWebApp.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria.outbound
{
    public class OutboundPlantInformationCriteria : CallOutboundApiSearchCriteriaBase
    {
        public List<Company> Company { get; set; }
    }


    public class Company
    {
        public string Company_Code { get; set; }
    }
}
