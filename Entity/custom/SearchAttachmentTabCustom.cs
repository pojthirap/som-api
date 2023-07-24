using Entity;
using MyFirstAzureWebApp.Entity.custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria
{
    public class SearchAttachmentTabCustom
    {
        public String UpdateDtmStr { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public List<RecordAppFormFile> RecordAppFormFileLst { get; set; }



    }
    public class RecordAppFormFileForSearchAttachmentTab : RecordAppFormFile
    {
        public String UpdateDtmStr { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }

}
