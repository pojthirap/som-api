using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteria
{
    public class AdmSessionCriteria
    {
        public decimal SessionId { get; set; }
        public string TokenNo { get; set; }
        public string EmpId { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string Valid { get; set; }
    }
}
