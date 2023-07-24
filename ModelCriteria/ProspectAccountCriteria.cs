using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria
{
    public class ProspectAccountCriteria
    {
        public string EmpId { get; set; }
        public string ProspAccId { get; set; }
        public string AccName { get; set; }
        public string? BrandId { get; set; }
        public string CustCode { get; set; }
        public string IdentifyId { get; set; }
        public string AccGroupRef { get; set; }
        public string SourceType { get; set; }
        public string ActiveFlag { get; set; }
        public string BuId { get; set; }
        public string ProspectStatus { get; set; }
        public string SaleRepId { get; set; }
        public string ProspectType { get; set; }
        public string[] ProspectStatusLst { get; set; }
        public string ProspectId { get; set; }
        public string GroupCode { get; set; }
    }
}
