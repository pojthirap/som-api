using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class GetTaskTemplateSaFormForRecordCustom
    {
        public TemplateSaForm Form { get; set; }
        public List<RecordSaFormFile> ListFile { get; set; }
        public List<TemplateSaTitle> Title { get; set; }
    }
}
