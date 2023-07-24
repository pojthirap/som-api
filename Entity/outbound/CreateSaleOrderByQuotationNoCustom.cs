using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class CreateSaleOrderByQuotationNoCustom
    {

        public String Item_no { get; set; }
        public String Material_Code { get; set; }
        public String Item_Category { get; set; }
        public String Plant { get; set; }
        public String Shipping_Point_Receiving_Pt { get; set; }
        public String Order_Quantity { get; set; }
        public String Unit { get; set; }
        public String Net_Price { get; set; }
        public String Net_Value { get; set; }
        public String Tax_Amount { get; set; }
        public String Total_Value { get; set; }
        public String Condition_Total_Value { get; set; }
        public List<Condition> Condition { get; set; }

    }

    public class Condition
    {
        public String Condition_type { get; set; }
        public decimal Condition_Amount { get; set; }
    }
}
