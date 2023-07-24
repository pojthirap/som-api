using Entity;
using MyFirstAzureWebApp.Entity.custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria
{
    public class SearchAdmSessionForGetSessionCustom
    {


        public decimal SessionId { get; set; }
        public string EmpId { get; set; }
        public string UserAccessFlag { get; set; }


    }
}
