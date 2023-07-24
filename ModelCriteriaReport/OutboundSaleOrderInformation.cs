using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteriaReport
{
    public class OutboundSaleOrderInformation
    {
        
    }
    
    public class Company
    {
        public string Company_code { get; set; }
        public string Company_Name_EN { get; set; }
        public List<Distribution> Distribution { get; set; }
    }
    public class Distribution
    {
        public string Distribution_Channel { get; set; }
        public string Distribution_Channel_Name { get; set; }
        public List<Sale_Order_Report> Sale_Order_Report { get; set; }
    }
    public class Sale_Order_Report
    {
        public string Sale_Group { get; set; }
        public string Sale_Group_Name { get; set; }
        public string Total_Sale_Order { get; set; }
        public string SO_SAP { get; set; }
        public string SO_SOM { get; set; }
        public string SO_div_SAP { get; set; }
        public string SO_div_SOM { get; set; }
    }
   
}
