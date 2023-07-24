using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity
{
    public partial class PlanTrip
    {
        [NotMapped]
        public string FirstName { get; set; }
        [NotMapped]
        public string LastName { get; set; }
        [NotMapped]
        public string TitleName { get; set; }
        [NotMapped]
        public string CompletFlag { get; set; }

        
    }
}
