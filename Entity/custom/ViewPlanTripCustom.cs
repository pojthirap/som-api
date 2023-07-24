using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class ViewPlanTripCustom
    {
        public PlanTrip PlanTrip { get; set; }
        public List<PlanTripProspect> ListPlanTripProspect { get; set; }


    }
}
