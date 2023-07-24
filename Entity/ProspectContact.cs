using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("PROSPECT_CONTACT")]
    public partial class ProspectContact
    {
        [Key]
        [Column("PROSP_CONTACT_ID", TypeName = "numeric(10, 0)")]
        public decimal ProspContactId { get; set; }
        [Column("PROSPECT_ID", TypeName = "numeric(10, 0)")]
        public decimal? ProspectId { get; set; }
        [Column("PROSP_ACC_ID", TypeName = "numeric(10, 0)")]
        public decimal? ProspAccId { get; set; }
        [Column("FIRST_NAME")]
        [StringLength(250)]
        public string FirstName { get; set; }
        [Column("LAST_NAME")]
        [StringLength(250)]
        public string LastName { get; set; }
        [Column("PHONE_NO")]
        [StringLength(100)]
        public string PhoneNo { get; set; }
        [Column("FAX_NO")]
        [StringLength(20)]
        public string FaxNo { get; set; }
        [Column("MOBILE_NO")]
        [StringLength(100)]
        public string MobileNo { get; set; }
        [Column("EMAIL")]
        [StringLength(250)]
        public string Email { get; set; }
        [Column("MAIN_FLAG")]
        [StringLength(1)]
        public string MainFlag { get; set; }
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
