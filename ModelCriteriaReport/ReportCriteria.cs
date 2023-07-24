using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteriaReport
{
    public class ReportCriteria
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string saleRepId { get; set; }
        public string saleRepName { get; set; }
        public string prospectId { get; set; }
        public string custCode { get; set; }
        public string meterNegativeFalg { get; set; }
        public string reportUnitFalg { get; set; }
        public string tpSaFormId { get; set; }
        public string tpAppFormId { get; set; }
        public string saleOrg { get; set; }
        public string saleChannel { get; set; }
        public string saleDivision { get; set; }
        public string prospectType { get; set; }
        public string groupCode { get; set; }
        public string buId { get; set; }
        public List<String> listSaleOrg { get; set; }
        public List<String> listCompany { get; set; }
    }
}
