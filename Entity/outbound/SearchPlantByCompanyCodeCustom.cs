using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchPlantByCompanyCodeCustom
    {
        public String Update_Date { get; set; }
        public String Update_Time { get; set; }
        public String Status_IND { get; set; }
        public String Company { get; set; }
        public String Plant { get; set; }
        public String Name1 { get; set; }
        public String Name2 { get; set; }
        public String City { get; set; }
        public String Customer_Num_Plant { get; set; }
        public String Factory_Calender { get; set; }
        public String Purch_Org { get; set; }
        public String Business_Place { get; set; }

    }
}
