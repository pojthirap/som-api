using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class TemplateStockProductModel : ModelBase
    {

        public string TpStockProdId { get; set; }
        public string TpStockCardId { get; set; }
        public string[] ProdCode { get; set; }
        public string ActiveFlag { get; set; }

    }
}
