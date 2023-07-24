using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyFirstAzureWebApp.Entity.custom.GetTaskTemplateAppFormForRecordCustom;

namespace MyFirstAzureWebApp.Models.record
{
    public class AddRecordSaFormModel
    {
        public string PlanTripTaskId { get; set; }
        public TemplateSaForm Form { get; set; }
        public string TpSaFormId { get; set; }
        public string ProspId { get; set; }
        public List<TemplateSaTitle> ListTitle { get; set; }
    }
}
