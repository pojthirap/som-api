using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteriaReport
{
    public class OutboundSaleOrderInformationResponse : OutboundSaleOrderInformation
    {
        public string SO_Status { get; set; }
        public string SO_Message { get; set; }
        public List<Data> Data { get; set; }
    }
    public class Data
    {
        public List<Company> Company { get; set; }
    }
}
