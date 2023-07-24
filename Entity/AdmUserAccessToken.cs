using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("ADM_USER_ACCESS_TOKEN")]
    public partial class AdmUserAccessToken
    {
        [Key]
        [Column("ACCESS_TOKEN_ID", TypeName = "numeric(10, 0)")]
        public decimal AccessTokenId { get; set; }
        [Required]
        [Column("ACCESS_TOKEN")]
        [StringLength(64)]
        public string AccessToken { get; set; }
        [Required]
        [Column("ACCESS_TYPE")]
        [StringLength(1)]
        public string AccessType { get; set; }
        [Column("EMP_ID")]
        [StringLength(20)]
        public string EmpId { get; set; }
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
