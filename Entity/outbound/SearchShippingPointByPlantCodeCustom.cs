using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchShippingPointByPlantCodeCustom
    {

        public String Update_Date { get; set; }
        public String Update_Time { get; set; }
        public String Status_IND { get; set; }
        public String Plant { get; set; }
        public String Plant_Name { get; set; }
        public String Shipping_Point_Receiving_Pt { get; set; }
        public String Shipping_Point_Receiving_Pt_Name { get; set; }

    }
}
