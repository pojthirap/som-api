using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("CUSTOMER_ACCOUNT_GROUP")]
    public partial class CustomerAccountGroup
    {
        [Key]
        [Column("ACC_GROUP_CODE")]
        [StringLength(20)]
        public string AccGroupCode { get; set; }
        [Column("ACC_GROUP_NAME_TH")]
        [StringLength(250)]
        public string AccGroupNameTh { get; set; }
        [Column("ACC_GROUP_NAME_EN")]
        [StringLength(250)]
        public string AccGroupNameEn { get; set; }
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
