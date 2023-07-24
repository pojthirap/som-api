using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class GetProspectForCreatePlanTripAdHocCustom
    {
        /*public decimal ProspAccId { get; set; }
        public string AccName { get; set; }
        public decimal? BrandId { get; set; }
        public string CustCode { get; set; }
        public string IdentifyId { get; set; }
        public string AccGroupRef { get; set; }
        public string Remark { get; set; }
        public string SourceType { get; set; }
        public string ActiveFlag { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDtm { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ProspectId { get; set; }*/



        public string AccName { get; set; }
        public string CustCode { get; set; }
        public decimal ProspAccId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ProspectId { get; set; }
        public string AddressFullnm { get; set; }

    }
}
