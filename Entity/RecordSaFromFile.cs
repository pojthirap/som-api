﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("RECORD_SA_FROM_FILE")]
    public partial class RecordSaFromFile
    {
        [Key]
        [Column("FILE_ID", TypeName = "numeric(10, 0)")]
        public decimal FileId { get; set; }
        [Column("ATTACH_CATE_ID", TypeName = "numeric(10, 0)")]
        public decimal? AttachCateId { get; set; }
        [Column("REC_SA_FORM_ID", TypeName = "numeric(10, 0)")]
        public decimal RecSaFormId { get; set; }
        [Required]
        [Column("FILE_NAME")]
        [StringLength(20)]
        public string FileName { get; set; }
        [Required]
        [Column("FILE_EXT")]
        [StringLength(10)]
        public string FileExt { get; set; }
        [Required]
        [Column("CREATE_USER")]
        [StringLength(20)]
        public string CreateUser { get; set; }
        [Column("CREATE_DTM", TypeName = "datetime")]
        public DateTime CreateDtm { get; set; }
        [Required]
        [Column("UPDATE_USER")]
        [StringLength(20)]
        public string UpdateUser { get; set; }
        [Column("UPDATE_DTM", TypeName = "datetime")]
        public DateTime UpdateDtm { get; set; }
        [Column("FILE_SIZE")]
        [StringLength(20)]
        public string FileSize { get; set; }
    }
}
