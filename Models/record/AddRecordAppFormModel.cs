using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyFirstAzureWebApp.Entity.custom.GetTaskTemplateAppFormForRecordCustom;

namespace MyFirstAzureWebApp.Models.record
{
    public class AddRecordAppFormModel
    {

        public string planTripTaskId { get; set; }
        public ObjectForm objForm { get; set; }
        public string TpAppFormId { get; set; }
        public string ProspId { get; set; }



    }
}
