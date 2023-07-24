using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria
{
    public class SearchSaleOrderDocFlowCriteria
    {
        public string SapOrderNo { get; set; }
        public string OrgCode { get; set; }
        public string ChannelCode { get; set; }
        public string DivisionCode { get; set; }
        public string DocTypeCode { get; set; }
        public string SomOrderNo { get; set; }
        public string SomOrderDte { get; set; }
    }
}
