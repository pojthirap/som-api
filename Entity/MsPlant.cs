using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("MS_PLANT")]
    public partial class MsPlant
    {
        [Key]
        [Column("PLANT_CODE")]
        [StringLength(10)]
        public string PlantCode { get; set; }
        [Column("PLANT_NAME_TH")]
        [StringLength(250)]
        public string PlantNameTh { get; set; }
        [Column("PLANT_NAME_EN")]
        [StringLength(250)]
        public string PlantNameEn { get; set; }
        [Column("CITY")]
        [StringLength(250)]
        public string City { get; set; }
        [Column("PLANT_VENDOR_NO")]
        [StringLength(10)]
        public string PlantVendorNo { get; set; }
        [Column("PLANT_CUST_NO")]
        [StringLength(10)]
        public string PlantCustNo { get; set; }
        [Column("PURCH_ORG")]
        [StringLength(10)]
        public string PurchOrg { get; set; }
        [Column("FACT_CALENDAR")]
        [StringLength(10)]
        public string FactCalendar { get; set; }
        [Column("BUSS_PLACE")]
        [StringLength(10)]
        public string BussPlace { get; set; }
        [Column("ACTIVE_FLAG")]
        [StringLength(1)]
        public string ActiveFlag { get; set; }
        [Column("CREATE_USER")]
        [StringLength(20)]
        public string CreateUser { get; set; }
        [Column("CREATE_DTM", TypeName = "datetime")]
        public DateTime? CreateDtm { get; set; }
        [Column("UPDATE_USER")]
        [StringLength(20)]
        public string UpdateUser { get; set; }
        [Column("UPDATE_DTM", TypeName = "datetime")]
        public DateTime? UpdateDtm { get; set; }
    }
}
