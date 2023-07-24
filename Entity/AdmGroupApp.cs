﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("ADM_GROUP_APP")]
    [Index(nameof(GroupId), nameof(AppId), Name = "UK_ADM_GROUP_APP", IsUnique = true)]
    public partial class AdmGroupApp
    {
        [Key]
        [Column("GROUP_APP_ID", TypeName = "numeric(10, 0)")]
        public decimal GroupAppId { get; set; }
        [Column("GROUP_ID", TypeName = "numeric(10, 0)")]
        public decimal? GroupId { get; set; }
        [Column("APP_ID", TypeName = "numeric(10, 0)")]
        public decimal? AppId { get; set; }
        [Column("ACTIVE_FLAG")]
        [StringLength(1)]
        public string ActiveFlag { get; set; }
        [Column("EFFECTIVE_DATE", TypeName = "datetime")]
        public DateTime? EffectiveDate { get; set; }
        [Column("EXPIRY_DATE", TypeName = "datetime")]
        public DateTime? ExpiryDate { get; set; }
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
