using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep09Rult
    {
        public List<Rep09SaleGroup> listSaleGroup { get; set; }
        public Dictionary<String, String[]> mapColmn { get; set; }
    }

    public class Rep09SaleGroup
    {
        public string groupCode { set; get; }
        public string groupDesc { set; get; }
        public List<Rep09SaleRep> listSaleRep { get; set; }
    }

    public class Rep09SaleRep
    {
        public string empName { set; get; }
        public string empId { set; get; }
        public List<Rep09Cust> listCust { get; set; }

    }
    public class Rep09Cust
    {
        public string custNameTh { set; get; }
        public string custCode { set; get; }
        public List<Rep09Month> listMonth { get; set; }
    }

    public class Rep09Month
    {
        public string mon { set; get; }
        public List<Rep09Qty> listQty { get; set; }
    }
    public class Rep09Qty
    {
        public string recQty { set; get; }
        public string keyColmName { set; get; }
        public string colmNo { set; get; }
    }

    public class Rep09StockCardCustomerSummaryResult
    {
        public string groupCode { set; get; }
        public string groupDesc { set; get; }
        public string empName { set; get; }
        public string empId { set; get; }
        public string custNameTh { set; get; }
        public string custCode { set; get; }
        public string mon { set; get; }
        public string keyColmName { set; get; }
        public string colmName { set; get; }
        public string recQry { set; get; }
        public string reportUnit { set; get; }

    }
}
