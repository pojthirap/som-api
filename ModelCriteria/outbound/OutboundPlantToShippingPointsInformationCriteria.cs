using MyFirstAzureWebApp.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria.outbound
{
    public class OutboundPlantToShippingPointsInformationCriteria : CallOutboundApiSearchCriteriaBase
    {
        public List<Plant> Plant { get; set; }
        public List<Shipping_ConditionsClass> Shipping_Conditions { get; set; }
    }


    public class Plant
    {
        public string Plant_Code { get; set; }
    }
    public class Shipping_ConditionsClass
    {
        public string Shipping_Conditions { get; set; }
    }
}
