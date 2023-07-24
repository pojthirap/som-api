using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchTemplateAppQuestionByIdCustom
    {
        public decimal TpAppQuestionId { get; set; }
        public decimal? TpAppFormId { get; set; }
        public decimal? QuestionId { get; set; }
        public decimal? OrderNo { get; set; }
        public decimal? PrerequistOrderNo { get; set; }
        public string RequireFlag { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDtm { get; set; }



        public string QuestionCode { get; set; }
        public string QuestionNameTh { get; set; }
        public string QuestionNameEn { get; set; }
        public decimal AnsType { get; set; }
        public string AnsValues { get; set; }
        public string PublicFlag { get; set; }
        public string ActiveFlag { get; set; }




    }
}
