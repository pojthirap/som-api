using Entity;
using MyFirstAzureWebApp.Entity.custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria
{
    public class SearchVisitHourCustom
    {
        public decimal ProspVisitHrId { get; set; }
        public decimal? ProspectId { get; set; }
        public string DaysCode { get; set; }
        public string HourStart { get; set; }
        public string HourEnd { get; set; }
        public string VisitHourActiveFlag { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDtm { get; set; }

        public string LovKeyword { get; set; }
        public decimal LovKeyvalue { get; set; }
        public string LovNameTh { get; set; }
        public string LovNameEn { get; set; }
        public string LovCodeTh { get; set; }
        public string LovCodeEn { get; set; }
        public decimal? LovOrder { get; set; }
        public string LovRemark { get; set; }
        public string LovActiveFlag { get; set; }
        public string Condition1 { get; set; }


    }
}
