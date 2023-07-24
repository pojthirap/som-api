using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{

    public class SearchProspectRecommendCustom
    {

        //PROSPECT_ACCOUNT
        public ProspectAccount ProspectAccount { get; set; }

        //PROSPECTT
        public Prospect Prospect { get; set; }

        //PROSPECT_CONTACT

        public ProspectContact ProspectContact { get; set; }


        //PROSPECT_ADDRESS

        public ProspectAddress ProspectAddress { get; set; }


        //PROSPECT_RECOMMEND
        public ProspectRecommend ProspectRecommend { get; set; }






    }
}
