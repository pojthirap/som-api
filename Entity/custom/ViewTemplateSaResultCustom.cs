using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class ViewTemplateSaResultCustom
    {
        public TemplateSaFormCustomForViewTemplateSaResult form { get; set; }
        public List<RecordSaFormFile> listFile { get; set; }
        public List<TemplateSaTitle> title { get; set; }


        public class TemplateSaFormCustomForViewTemplateSaResult : TemplateSaForm
        {
            public string TemplateSaFormCreateUser { get; set; }
            public DateTime? TemplateSaFormCreateDtm { get; set; }
        }
    }
}
