using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("TEMPLATE_TITLE")]
    public partial class TemplateTitle
    {
        [Key]
        [Column("TITLE_ID", TypeName = "numeric(10, 0)")]
        public decimal TitleId { get; set; }
        [Required]
        [Column("TITLE_CODE")]
        [StringLength(20)]
        public string TitleCode { get; set; }
        [Required]
        [Column("TITLE_NAME_TH")]
        [StringLength(250)]
        public string TitleNameTh { get; set; }
        [Required]
        [Column("TITLE_NAME_EN")]
        [StringLength(250)]
        public string TitleNameEn { get; set; }
        [Required]
        [Column("TITLE_TYPE")]
        [StringLength(1)]
        public string TitleType { get; set; }
        [Column("TITLE_DATA_TYPE", TypeName = "numeric(2, 0)")]
        public decimal? TitleDataType { get; set; }
        [Column("TITLE_DATA_LOV", TypeName = "numeric(2, 0)")]
        public decimal? TitleDataLov { get; set; }
        [Required]
        [Column("CREATE_USER")]
        [StringLength(50)]
        public string CreateUser { get; set; }
        [Column("CREATE_DTM", TypeName = "datetime")]
        public DateTime CreateDtm { get; set; }
        [Required]
        [Column("UPDATE_USER")]
        [StringLength(50)]
        public string UpdateUser { get; set; }
        [Column("UPDATE_DTM", TypeName = "datetime")]
        public DateTime UpdateDtm { get; set; }
    }
}
