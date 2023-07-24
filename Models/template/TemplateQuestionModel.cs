using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class TemplateQuestionModel : ModelBase
    {

        public string QuestionId { get; set; }
        public string QuestionCode { get; set; }
        public string QuestionNameTh { get; set; }
        public string QuestionNameEn { get; set; }
        public string AnsType { get; set; }
        public string AnsValues { get; set; }
        public string PublicFlag { get; set; }
        public string ActiveFlag { get; set; }

    }
}
