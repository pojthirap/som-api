using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchAdmGroupAppCustom : AdmGroupApp
    {

        public string GroupNameTh { get; set; }
        public string PermObjNameTh { get; set; }
        public string PermObjCode { get; set; }


    }
}
