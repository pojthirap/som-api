using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Keyless]
    [Table("KNA1_General_Data")]
    public partial class Kna1GeneralDatum
    {
        [Column("Account_group")]
        [StringLength(255)]
        public string AccountGroup { get; set; }
        [StringLength(255)]
        public string Customer { get; set; }
        [StringLength(255)]
        public string Country { get; set; }
        [Column("Name_1")]
        [StringLength(255)]
        public string Name1 { get; set; }
        [Column("Name_2")]
        [StringLength(255)]
        public string Name2 { get; set; }
        [Column("Search_term")]
        [StringLength(255)]
        public string SearchTerm { get; set; }
        [Column("Telephone_1")]
        [StringLength(255)]
        public string Telephone1 { get; set; }
        [StringLength(255)]
        public string Street { get; set; }
        [StringLength(255)]
        public string District { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [Column("Postal_Code")]
        [StringLength(255)]
        public string PostalCode { get; set; }
        [StringLength(255)]
        public string Region { get; set; }
        [StringLength(255)]
        public string Country1 { get; set; }
        [Column("Transportation_Zone")]
        [StringLength(255)]
        public string TransportationZone { get; set; }
        [Column("Tax_Number_3")]
        [StringLength(255)]
        public string TaxNumber3 { get; set; }
        [Column("VAT_Registration_No.")]
        [StringLength(255)]
        public string VatRegistrationNo { get; set; }
        [Column("Location_code_(Prospect_ID)")]
        public double? LocationCodeProspectId { get; set; }
        [Column("Deletion_flag")]
        [StringLength(255)]
        public string DeletionFlag { get; set; }
    }
}
