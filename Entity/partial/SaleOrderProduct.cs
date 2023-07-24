using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity
{
    public partial class SaleOrderProduct
    {
        [NotMapped]
        public string ProdNameTh { get; set; }
        [NotMapped]
        public string AltUnit { get; set; }

    }
}
