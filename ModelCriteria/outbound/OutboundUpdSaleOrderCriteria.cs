using MyFirstAzureWebApp.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria.outbound.updatesaleorder
{
    public class OutboundUpdSaleOrderCriteria
    {
        public Header Header { get; set; }
    }

    public class Header
    {
        public string Simulate_Indicator { get; set; }
        public string SAP_Sale_Order_No { get; set; }
        public string Sale_Org { get; set; }
        public string Distribution_Channel { get; set; }
        public string Division { get; set; }
        public string Sales_Order_Type { get; set; }
        public string SOM_order_no { get; set; }
        public string PO_Number { get; set; }
        public string Document_date { get; set; }
        public string Reference_Qoutation_Document { get; set; }
        public string Sold_to_customer { get; set; }
        public string Ship_to_customer { get; set; }
        public string Request_Delivery_date { get; set; }
        public string Incoterms1 { get; set; }
        public string Incoterms2 { get; set; }
        public string SO_Cancel_Indicator { get; set; }
        public string Price_Date { get; set; }
        public string Price_Time { get; set; }
        public string Price_List { get; set; }
        public string Reason_For_Rejection { get; set; }
        public string Order_Reason { get; set; }
        public List<Item> Item { get; set; }
    }
    public class Item
    {
        public string Item_no { get; set; }
        public string Material_Code { get; set; }
        public string Item_Category { get; set; }
        public string Plant { get; set; }
        public string Shipping_Point_Receiving_Pt { get; set; }
        public string Storage_Location { get; set; }
        public string Order_Quantity { get; set; }
        public string Unit { get; set; }
        public string Price_per_Unit { get; set; }
        public List<Condition> Condition { get; set; }
    }

    public class Condition
    {
        public string Condition_type { get; set; }
        public string Condition_Amount { get; set; }
        public string Condition_Per { get; set; }
        public string Condition_Unit { get; set; }
    }
}
