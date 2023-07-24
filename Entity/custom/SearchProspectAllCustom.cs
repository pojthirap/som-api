using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchProspectAllCustom
    {

        public decimal ProspectId { get; set; }
        public string CustCode { get; set; }
        public string AccName { get; set; }

    }
}
