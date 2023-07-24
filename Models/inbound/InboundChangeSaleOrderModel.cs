using MyFirstAzureWebApp.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria.inbound.changeSaleOrder
{
    public class InboundChangeSaleOrderModel
    {
        public Header_Change Header { get; set; }
        public Status_Change Status { get; set; }
    }

    public class Status_Change
    {
        public string SAP_Sale_Order_No { get; set; }
        public string SO_Status { get; set; }
        public string Credit_Status { get; set; }
        public string SO_Message { get; set; }
    }
    public class Header_Change
    {
        public string Sale_Org { get; set; }
        public string Distribution_Channel { get; set; }
        public string Division { get; set; }
        public string Price_List { get; set; }
        public string Sales_Order_Type { get; set; }
        public string SAP_Sale_Order_No { get; set; }
        public string SOM_order_no { get; set; }
        public string PO_Number { get; set; }
        public string Document_date { get; set; }
        public string Request_Delivery_date { get; set; }
        public string Payment_Term { get; set; }
        public string Sold_to_customer { get; set; }
        public string Ship_to_customer { get; set; }
        public string Incoterms1 { get; set; }
        public string Incoterms2 { get; set; }
        public string Sum_Net_Price_by_SO { get; set; }
        public string Tax_Amount_by_SO { get; set; }
        public string SUM_Total_NetADDVat { get; set; }
        public string Price_Date { get; set; }
        public string Price_Time { get; set; }
        public string Reason_For_Rejection { get; set; }
        public string Order_Reason { get; set; }

        public List<Item_Change> Item { get; set; }
    }
    public class Item_Change
    {
        public string Item_no { get; set; }
        public string Material_Code { get; set; }
        public string Item_Category { get; set; }
        public string Plant { get; set; }
        public string Shipping_Point_Receiving_Pt { get; set; }
        public string Order_Quantity { get; set; }
        public string Unit { get; set; }
        public string Net_Price { get; set; }
        public string Net_Value { get; set; }
        public string Tax_Amount { get; set; }
        public string Total_Value { get; set; }
        public string Condition_Total_Value { get; set; }
        public List<Condition_Change> Condition { get; set; }
    }

    public class Condition_Change
    {
        public string Condition_type { get; set; }
        public string Condition_Amount { get; set; }
        public string Condition_Per { get; set; }
        public string Condition_Unit { get; set; }
    }
}
