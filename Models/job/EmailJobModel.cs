using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.job
{
    public class EmailJobModel
    {


        public string EmailTemplate { get; set; }
        public decimal TableRefKeyId { get; set; }
        public string ObjEmail { get; set; }
        public string JobStatus { get; set; }
        public int JobStatusFailCount { get; set; }
        public string ErrorDesc { get; set; }

    }
}
