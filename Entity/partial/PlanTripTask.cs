using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity
{
    public partial class PlanTripTask
    {
        [NotMapped]
        public string TemplateName { get; set; }
        [NotMapped]
        public string CompletedFlag { get; set; }
        [NotMapped]
        public string StockCardCode { get; set; }
    }
}
