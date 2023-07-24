using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.pospect
{
    public class CreateProspectModel : ModelBase
    {
        public ProspectAccountModel ProspectAccountModel { get; set; }
        public ProspectModel ProspectModel { get; set; }
        public ProspectAddressModel ProspectAddressModel { get; set; }
        public ProspectContactModel ProspectContactModel { get; set; }


    }
}
