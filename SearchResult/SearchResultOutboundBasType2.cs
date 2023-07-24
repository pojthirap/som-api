using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria.type2
{
    public class SearchResultOutboundBasType2<T>
    {
        public HeaderType2<T> Header { get; set; }
        public StatusType2 Status { get; set; }
    }

    public class StatusType2
    {
        public string SAP_Sale_Order_No { get; set; }
        public string SO_Status { get; set; }
        public string Credit_Status { get; set; }
        public string SO_Message { get; set; }
    }
    public class HeaderType2<T>
    {
        public string Sale_Org { get; set; }
        public string Distribution_Channel { get; set; }
        public string Division { get; set; }
        public string Sales_Order_Type { get; set; }
        public string SAP_Sale_Order_No { get; set; }
        public string SOM_order_no { get; set; }
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
        public List<T> Item { get; set; }
    }


    public class ItemType2
    {
        public string Item_no { get; set; }
        public string Material_Code { get; set; }
        public string Item_Category { get; set; }
        public string Plant { get; set; }
        public string Order_Quantity { get; set; }
        public string Unit { get; set; }
        public string Net_Price { get; set; }
        public string Net_Value { get; set; }
        public string Tax_Amount { get; set; }
        public string Total_Value { get; set; }
        public string Condition_Total_Value { get; set; }
        public List<ConditionType2> Condition { get; set; }

    }
    public class ConditionType2
    {
        public string Condition_type { get; set; }
        public decimal Condition_Amount { get; set; }
        public decimal Condition_Per { get; set; }

    }
}
