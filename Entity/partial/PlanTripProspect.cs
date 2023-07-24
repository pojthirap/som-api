using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity
{
    public partial class PlanTripProspect
    {
        [NotMapped]
        public string AccName { get; set; }
        [NotMapped]
        public string CustCode { get; set; }
        [NotMapped]
        public string Latitude { get; set; }
        [NotMapped]
        public string Longitude { get; set; }
        [NotMapped]
        public string OpenFlag { get; set; }
        //public string LocRemark { get; set; }


    }
}
