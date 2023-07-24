using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep08StockCardRawDataResult
    {
        public string PlanTripName { set; get; }
        public string PlanTripId { set; get; }
        public string TerritoryId { set; get; }
        public string TerritoryNameTh { set; get; }
        public string VisitCheckinDtm { set; get; }
        public string VisitCheckoutDtm { set; get; }
        public string EmpName { set; get; }
        public string EmpId { set; get; }
        public string CustNameTh { set; get; }
        public string CustCode { set; get; }
        public string ProdCateCode { set; get; }
        public string ProdCateDesc { set; get; }
        public string ProdNameTh { set; get; }
        public string ProdCode { set; get; }
        public string RecQty { set; get; }
        public string AltUnit { set; get; }

    }
}
