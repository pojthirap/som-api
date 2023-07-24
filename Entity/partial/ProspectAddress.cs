using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity
{
    public partial class ProspectAddress
    {

        [NotMapped]
        public string EditGeneralDataFlag { get; set; }
        [NotMapped]
        public string AddressFullName { get; set; }
        [NotMapped]
        public string ShiftToFlag { get; set; }
        [NotMapped]
        public string MainAddressFlag { get; set; }
        [NotMapped]
        public string BillToFlag { get; set; }
    }
}
