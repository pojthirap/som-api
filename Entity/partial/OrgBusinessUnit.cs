using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity
{
    public partial class OrgBusinessUnit
    {
        [NotMapped]
        public decimal? ProspRecommId { get; set; }

    }
}
