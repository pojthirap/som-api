using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class ZResultCustom
    {

        public string COMPANY_CODE { get; set; }
        public string PLANT_CODE { get; set; }
        public string PLANT_NAME_TH { get; set; }
        public string PLANT_NAME_EN { get; set; }
        public string CITY { get; set; }
        public string PLANT_VENDOR_NO { get; set; }
        public string PLANT_CUST_NO { get; set; }
        public string PURCH_ORG { get; set; }
        public string FACT_CALENDAR { get; set; }
        public string BUSS_PLACE { get; set; }
        public string ACTIVE_FLAG { get; set; }
        public string CREATE_USER { get; set; }
        public DateTime? CREATE_DTM { get; set; }
        public string UPDATE_USER { get; set; }
        public DateTime? UPDATE_DTM { get; set; }
    }
}
