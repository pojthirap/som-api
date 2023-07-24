using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity
{
    public partial class SaleOrder
    {
        [NotMapped]
        public string? GroupDescTh { get; set; }
        [NotMapped]
        public string? OrderStatusDesTh { get; set; }
        [NotMapped]
        public string? OrgNameTh { get; set; }
        [NotMapped]
        public string? ChannelNameTh { get; set; }
        [NotMapped]
        public string? DivisionNameTh { get; set; }
        [NotMapped]
        public string? ShippingCond { get; set; }
        [NotMapped]
        public string? PriceDate { get; set; }
        [NotMapped]
        public string? PriceTime { get; set; }

    }
}
