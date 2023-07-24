using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchMeterCustom
    {


        public decimal MeterId { get; set; }
        public decimal? GasId { get; set; }
        public string CustCode { get; set; }
        public decimal? DispenserNo { get; set; }
        public decimal? NozzleNo { get; set; }
        public string Qrcode { get; set; }
        public string MeterActiveFlag { get; set; }
        public string MeterCreateUser { get; set; }
        public DateTime MeterCreateDtm { get; set; }
        public string MeterUpdateUser { get; set; }
        public DateTime MeterUpdateDtm { get; set; }




        public string GasNameTh { get; set; }
        public string GasNameEn { get; set; }
        public string GasActiveFlag { get; set; }
        public string GasCreateUser { get; set; }
        public DateTime GasCreateDtm { get; set; }
        public string GasUpdateUser { get; set; }
        public DateTime GasUpdateDtm { get; set; }
        public string GasCode { get; set; }

        public string CustNameTh { get; set; }

    }
}
