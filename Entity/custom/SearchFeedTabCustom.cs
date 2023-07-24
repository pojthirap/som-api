using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchFeedTabCustom
    {

        public decimal FeedId { get; set; }
        public decimal ProspectId { get; set; }
        public decimal FunctionTab { get; set; }
        public string Description { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDtm { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDtm { get; set; }


        
        public string LovNameTh { get; set; }
        public string CreateFullName { get; set; }

    }
}
