using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchRecordMeterTabCustom
    {

        public decimal? DispenserNo { get; set; }
        public decimal? NozzleNo { get; set; }
        public string GasNameTh { get; set; }
        public string RecRunNo { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDtm { get; set; }
        public string FileId { get; set; }
        public string UrlFile { get; set; }

        //public string UpdateUser { get; set; }
        //public DateTime UpdateDtm { get; set; }

        /*public decimal RecMeterId { get; set; }
        public decimal PlanTripTaskId { get; set; }
        public decimal MeterId { get; set; }
        public string RecRunNo { get; set; }
        public decimal FileId { get; set; }
        public string RecordMeterCreateUser { get; set; }
        public DateTime RecordMeterCreateDtm { get; set; }
        public string RecordMeterUpdateUser { get; set; }
        public DateTime RecordMeterUpdateDtm { get; set; }
        public string UrlFile { get; set; }




        public decimal GasId { get; set; }
        public string CustCode { get; set; }
        public decimal? DispenserNo { get; set; }
        public decimal? NozzleNo { get; set; }
        public string Qrcode { get; set; }
        public string ActiveFlag { get; set; }
        //public string CreateUser { get; set; }
        //public DateTime CreateDtm { get; set; }
        //public string UpdateUser { get; set; }
        //public DateTime UpdateDtm { get; set; }




        //public decimal FileId { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDtm { get; set; }
        public string FileSize { get; set; }*/


    }
}
