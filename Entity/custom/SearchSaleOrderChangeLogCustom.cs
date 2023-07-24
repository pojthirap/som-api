using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchSaleOrderChangeLogCustom
    {
        public DateTime ChangeDtm { get; set; }
        public string ChangeTabDesc { get; set; }
        public string ChangeUser { get; set; }
        public string OrderSaleRep { get; set; }

    }
}
