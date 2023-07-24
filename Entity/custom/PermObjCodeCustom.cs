using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class PermObjCodeCustom
    {
        public string PermObjId { get; set; }
        public string PermObjCode { get; set; }
        public string Level { get; set; }
        public string ParentId { get; set; }
        public string SelectedFlag { get; set; }
        //public string PermObjNameTh { get; set; }

    }
}
