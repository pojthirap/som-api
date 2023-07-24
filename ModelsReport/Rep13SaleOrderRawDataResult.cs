using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep13SaleOrderRawDataResult
    {
        public string SapOrderNo { set; get; }
        public string SomOrderNo { set; get; }
        public string OrderStatus { set; get; }
        public string OrgNameTh { set; get; }
        public string ChannelNameTh { set; get; }
        public string DivisionNameTh { set; get; }
        public string DescriptionTh { set; get; }
        public string NetValue { set; get; }
        public string CreateDate { set; get; }
        public string EmpName { set; get; }
    }
}
