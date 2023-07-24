using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.org
{
    public class OrgSaleAreaModel : ModelBase
    {

        public string AreaId { get; set; }
        public string OrgCode { get; set; }
        public string ChannelCode { get; set; }
        public string DivisionCode { get; set; }
        public string BussAreaCode { get; set; }
        public string BuId { get; set; }
        public string ActiveFlag { get; set; }
    }
}
