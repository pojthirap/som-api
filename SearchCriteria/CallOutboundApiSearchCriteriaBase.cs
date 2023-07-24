using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria
{
    public class CallOutboundApiSearchCriteriaBase
    {
        public Input Input { get; set; }
    }

    public class Input
    {
        public string Interface_ID { get; set; }
        public string Table_Object { get; set; }
    }
}
