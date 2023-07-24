using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class MsPlantShipCustom
    {
        // MsPlantShip
        public decimal PlantShipId { get; set; }
        public String PlantCode { get; set; }
        public String ShipCode { get; set; }
        public String ActiveFlag { get; set; }


        // MsPlant
        public String PlantNameTh { get; set; }
        public String PlantNameEn { get; set; }
        public String PlantVendorNo { get; set; }
        public String PlantCustNo { get; set; }
        public String PurchOrg { get; set; }
        public String FactCalendar { get; set; }
        public String BussPlace { get; set; }
        public string City { get; set; }


        // MsShip
        public String DescriptionTh { get; set; }
        public String DescriptionEn { get; set; }



        public String createUser { get; set; }
        public DateTime? createDtm { get; set; }
        public String updateUser { get; set; }
        public DateTime? updateDtm { get; set; }

    }
}
