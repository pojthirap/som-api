using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class GetNotifyTabOverviewCustom
    {
        public string OrderAction { get; set; }
        public string OrderActionName { get; set; }
        public string SomOrderNo { get; set; }
        public string SapOrderNo { get; set; }
        public string SapStatus { get; set; }
        public string SapMsg { get; set; }
        public DateTime? CreateDtm { get; set; }
        public DateTime? CpdateDtm { get; set; }

    }
}
