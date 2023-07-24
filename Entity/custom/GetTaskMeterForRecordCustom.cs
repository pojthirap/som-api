using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class GetTaskMeterForRecordCustom
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
        public string GasCode { get; set; }
        public string FileId { get; set; }
        public string RecMeterId { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public string FileSize { get; set; }
        public string RecRunNo { get; set; }
        public string Remark { get; set; }
        //public string OldRecRunNo { get; set; }
        public string FileUrl { get; set; }
        public string PrevRecRunNo { get; set; }

    }
}
