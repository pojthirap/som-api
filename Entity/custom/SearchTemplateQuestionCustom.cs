using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchTemplateQuestionCustom
    {
        public decimal? QuestionId { get; set; }
        public string QuestionCode { get; set; }
        public string QuestionNameTh { get; set; }
        public string QuestionNameEn { get; set; }
        public decimal? AnsType { get; set; }
        public string AnsValues { get; set; }
        public string PublicFlag { get; set; }
        public string ActiveFlag { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDtm { get; set; }


        public string LovKeyword { get; set; }
        public decimal? LovKeyvalue { get; set; }
        public string LovNameTh { get; set; }
        public string LovNameEn { get; set; }
        public string LovCodeTh { get; set; }
        public string LovCodeEn { get; set; }
        public decimal? LovOrder { get; set; }
        public string LovRemark { get; set; }



        public string EditFlag { get; set; }


    }
}
