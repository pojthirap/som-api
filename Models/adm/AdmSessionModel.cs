using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.adm
{
    public class AdmSessionModel
    {

        public decimal SessionId { get; set; }
        public string TokenNo { get; set; }
        public string EmpId { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public DateTime? LastAccessDtm { get; set; }
        public string Valid { get; set; }
        public DateTime? LoginDtm { get; set; }
        public DateTime? LogoutDtm { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDtm { get; set; }
        public DateTime UpdateDtm { get; set; }
        public string TokenExpire { get; set; }


    }
}
