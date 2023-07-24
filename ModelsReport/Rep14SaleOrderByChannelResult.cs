using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelsReport
{
    public class Rep14SaleOrderByChannelResult
    {
        
        public string channelDesc { set; get; }
        public string saleGroupDesc { set; get; }
        public string totalOrder { set; get; }
        public string totalSomOrder { set; get; }
        public string totalSapOrder { set; get; }
        public string percentSom { set; get; }
        public string percentSap { set; get; }

        public Rep14SaleOrderByChannelResult(String channelDesc, String saleGroupDesc, String totalOrder, String totalSomOrder, String totalSapOrder, String percentSom, String percentSap)
        {
            this.channelDesc = channelDesc;
            this.saleGroupDesc = saleGroupDesc;
            this.totalOrder = totalOrder;
            this.totalSomOrder = totalSomOrder;
            this.totalSapOrder = totalSapOrder;
            this.percentSom = percentSom;
            this.percentSap = percentSap;
        }

    }
}
