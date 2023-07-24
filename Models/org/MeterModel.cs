using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.org
{
    public class MeterModel : ModelBase
    {

        public string meterId { get; set; }
        public string gasId { get; set; }
        public string custCode { get; set; }
        public string dispenserNo { get; set; }
        public string nozzleNo { get; set; }
        public string qrcode { get; set; }
        public string activeFlag { get; set; }
    }
}
