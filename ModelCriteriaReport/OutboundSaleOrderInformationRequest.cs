using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.ModelCriteriaReport
{
    public class OutboundSaleOrderInformationRequest
    {
        public string SOM_Report_ID { get; set; }
        public string Document_from_date { get; set; }
        public string Document_to_date { get; set; }
        public List<Companys> Company { get; set; }
        public List<Sale_Orgs> Sale_Org { get; set; }
        public List<Distribution_Channels> Distribution_Channel { get; set; }
        public List<Divisions> Division { get; set; }
    }
    public class Companys
    {
        public Companys(String Company_code)
        {
            this.Company_code = Company_code;
        }
        public string Company_code { get; set; }
    }
    public class Sale_Orgs
    {
        public Sale_Orgs(String saleOrg)
        {
            this.Sale_Org = saleOrg;
        }
        public string Sale_Org { get; set; }
    }
    public class Distribution_Channels
    {
        public Distribution_Channels(String Distribution_Channel)
        {
            this.Distribution_Channel = Distribution_Channel;
        }
        public string Distribution_Channel { get; set; }
    }
    public class Divisions
    {
        public Divisions(String Division)
        {
            this.Division = Division;
        }
        public string Division { get; set; }
    }
}
