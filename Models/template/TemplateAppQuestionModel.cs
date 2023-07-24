using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class TemplateAppQuestionModel : ModelBase
    {

        public string TpAppQuestionId { get; set; }
        public string TpAppFormId { get; set; }
        public string QuestionId { get; set; }
        public string OrderNo { get; set; }
        public string PrerequistOrderNo { get; set; }
        public string RequireFlag { get; set; }

    }
}
