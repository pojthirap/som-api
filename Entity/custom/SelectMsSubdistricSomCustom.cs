using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SelectMsSubdistricSomCustom
    {

        public string activeFlag { get; set; }
        public string subdistrictSomId { get; set; }
    }
}
