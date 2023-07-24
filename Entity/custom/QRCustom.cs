using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class QRCustom
    {

        public decimal meterId { get; set; }
        public decimal gasId { get; set; }
        public string custCode { get; set; }
        public decimal? dispenserNo { get; set; }
        public decimal? nozzleNo { get; set; }
        public string qrcode { get; set; }
        public string activeFlagMeter { get; set; }
        public string activeFlagGas { get; set; }
        public string createUser { get; set; }
        public DateTime createDtm { get; set; }
        public string updateUser { get; set; }
        public DateTime updateDtm { get; set; }


        public string custNameTh { get; set; }
        public string custNameEn { get; set; }


        public string GasNameTh { get; set; }
        public string GasNameEn { get; set; }
        public string GasCode { get; set; }

    }
}
