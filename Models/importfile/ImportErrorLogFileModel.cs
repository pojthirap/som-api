using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.importfile
{
    public class ImportErrorLogFileModel : ModelBase
    {

        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public string FileSize { get; set; }
        public string ImportDataType { get; set; }


    }
}
