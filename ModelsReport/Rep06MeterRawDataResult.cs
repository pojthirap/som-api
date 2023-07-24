using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep06MeterRawDataResult
    {
        public string CustCode { set; get; }
        public string CustNameTh { set; get; }
        public string PlanTripId { set; get; }
        public string PlanTripName { set; get; }
        public string CreateDtm { set; get; }
        public string RecordDtm { set; get; }
        public string PrevGasCode { set; get; }
        public string PrevGasNameTh { set; get; }
        public string GasCode { set; get; }
        public string GasNameTh { set; get; }
        public string DispenserNo { set; get; }
        public string NozzleNo { set; get; }
        public string RecRunNo { set; get; }
        public string CntDispenserNo { set; get; }
        public string CntNozzle { set; get; }
        public string VisitLatitude { set; get; }
        public string VisitLongitude { set; get; }
        public string SaleRepId { set; get; }
        public string SaleRepName { set; get; }
    }
}
