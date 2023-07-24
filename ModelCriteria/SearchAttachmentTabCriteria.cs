using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria
{
    public class SearchAttachmentTabCriteria
    {
        public string ProspId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string PhotoFlag { get; set; }
        public string[] AttachCateId { get; set; }
    }
}
