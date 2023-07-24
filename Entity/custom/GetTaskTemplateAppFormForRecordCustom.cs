using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class GetTaskTemplateAppFormForRecordCustom
    {
        public List<MsAttachCategory> LovAttachCategory { get; set; }
        public List<RecordAppFormFile> ListFile { get; set; }
        public List<ObjectForm> ObjForm { get; set; }



        public class ObjectForm
        {
            public string TemplateId { get; set; }
            public string TemplateCateId { get; set; }
            public string TemplateCateName { get; set; }
            public string TemplateName { get; set; }
            //public List<Question> Question { get; set; }
            public List<AppForm> AppForm { get; set; }

        }


        public class AppForm
        {
            public string QuestionId { get; set; }
            public string RequireFlag { get; set; }
            public string PrerequistOrderNo { get; set; }
            public string QuestionNm { get; set; }
            public string AnsType { get; set; }
            public List<AnsVal> AnsVal { get; set; }
        }

        public class AnsVal
        {
            public string Ans { get; set; }
            public string Val { get; set; }
            public string valExt1 { get; set; }
            

        }
    }
}
