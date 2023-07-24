using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("RECORD_SA_FORM")]
    public partial class RecordSaForm
    {
        [Key]
        [Column("REC_SA_FORM_ID", TypeName = "numeric(10, 0)")]
        public decimal RecSaFormId { get; set; }
        [Column("PLAN_TRIP_TASK_ID", TypeName = "numeric(10, 0)")]
        public decimal? PlanTripTaskId { get; set; }
        [Column("TITLE_COLM_NO1")]
        [StringLength(250)]
        public string TitleColmNo1 { get; set; }
        [Column("TITLE_COLM_NO2")]
        [StringLength(250)]
        public string TitleColmNo2 { get; set; }
        [Column("TITLE_COLM_NO3")]
        [StringLength(250)]
        public string TitleColmNo3 { get; set; }
        [Column("TITLE_COLM_NO4")]
        [StringLength(250)]
        public string TitleColmNo4 { get; set; }
        [Column("TITLE_COLM_NO5")]
        [StringLength(250)]
        public string TitleColmNo5 { get; set; }
        [Column("TITLE_COLM_NO6")]
        [StringLength(250)]
        public string TitleColmNo6 { get; set; }
        [Column("TITLE_COLM_NO7")]
        [StringLength(250)]
        public string TitleColmNo7 { get; set; }
        [Column("TITLE_COLM_NO8")]
        [StringLength(250)]
        public string TitleColmNo8 { get; set; }
        [Column("TITLE_COLM_NO9")]
        [StringLength(250)]
        public string TitleColmNo9 { get; set; }
        [Column("TITLE_COLM_NO10")]
        [StringLength(250)]
        public string TitleColmNo10 { get; set; }
        [Column("TITLE_COLM_NO11")]
        [StringLength(250)]
        public string TitleColmNo11 { get; set; }
        [Column("TITLE_COLM_NO12")]
        [StringLength(250)]
        public string TitleColmNo12 { get; set; }
        [Column("TITLE_COLM_NO13")]
        [StringLength(250)]
        public string TitleColmNo13 { get; set; }
        [Column("TITLE_COLM_NO14")]
        [StringLength(250)]
        public string TitleColmNo14 { get; set; }
        [Column("TITLE_COLM_NO15")]
        [StringLength(250)]
        public string TitleColmNo15 { get; set; }
        [Column("TITLE_COLM_NO16")]
        [StringLength(250)]
        public string TitleColmNo16 { get; set; }
        [Column("TITLE_COLM_NO17")]
        [StringLength(250)]
        public string TitleColmNo17 { get; set; }
        [Column("TITLE_COLM_NO18")]
        [StringLength(250)]
        public string TitleColmNo18 { get; set; }
        [Column("TITLE_COLM_NO19")]
        [StringLength(250)]
        public string TitleColmNo19 { get; set; }
        [Column("TITLE_COLM_NO20")]
        [StringLength(250)]
        public string TitleColmNo20 { get; set; }
        [Column("TITLE_COLM_NO21")]
        [StringLength(250)]
        public string TitleColmNo21 { get; set; }
        [Column("TITLE_COLM_NO22")]
        [StringLength(250)]
        public string TitleColmNo22 { get; set; }
        [Column("TITLE_COLM_NO23")]
        [StringLength(250)]
        public string TitleColmNo23 { get; set; }
        [Column("TITLE_COLM_NO24")]
        [StringLength(250)]
        public string TitleColmNo24 { get; set; }
        [Column("TITLE_COLM_NO25")]
        [StringLength(250)]
        public string TitleColmNo25 { get; set; }
        [Column("TITLE_COLM_NO26")]
        [StringLength(250)]
        public string TitleColmNo26 { get; set; }
        [Column("TITLE_COLM_NO27")]
        [StringLength(250)]
        public string TitleColmNo27 { get; set; }
        [Column("TITLE_COLM_NO28")]
        [StringLength(250)]
        public string TitleColmNo28 { get; set; }
        [Column("TITLE_COLM_NO29")]
        [StringLength(250)]
        public string TitleColmNo29 { get; set; }
        [Column("TITLE_COLM_NO30")]
        [StringLength(250)]
        public string TitleColmNo30 { get; set; }
        [Column("TITLE_COLM_NO31")]
        [StringLength(250)]
        public string TitleColmNo31 { get; set; }
        [Column("TITLE_COLM_NO32")]
        [StringLength(250)]
        public string TitleColmNo32 { get; set; }
        [Column("TITLE_COLM_NO33")]
        [StringLength(250)]
        public string TitleColmNo33 { get; set; }
        [Column("TITLE_COLM_NO34")]
        [StringLength(250)]
        public string TitleColmNo34 { get; set; }
        [Column("TITLE_COLM_NO35")]
        [StringLength(250)]
        public string TitleColmNo35 { get; set; }
        [Column("TITLE_COLM_NO36")]
        [StringLength(250)]
        public string TitleColmNo36 { get; set; }
        [Column("TITLE_COLM_NO37")]
        [StringLength(250)]
        public string TitleColmNo37 { get; set; }
        [Column("TITLE_COLM_NO38")]
        [StringLength(250)]
        public string TitleColmNo38 { get; set; }
        [Column("TITLE_COLM_NO39")]
        [StringLength(250)]
        public string TitleColmNo39 { get; set; }
        [Column("TITLE_COLM_NO40")]
        [StringLength(250)]
        public string TitleColmNo40 { get; set; }
        [Column("TITLE_COLM_NO41")]
        [StringLength(250)]
        public string TitleColmNo41 { get; set; }
        [Column("TITLE_COLM_NO42")]
        [StringLength(250)]
        public string TitleColmNo42 { get; set; }
        [Column("TITLE_COLM_NO43")]
        [StringLength(250)]
        public string TitleColmNo43 { get; set; }
        [Column("TITLE_COLM_NO44")]
        [StringLength(250)]
        public string TitleColmNo44 { get; set; }
        [Column("TITLE_COLM_NO45")]
        [StringLength(250)]
        public string TitleColmNo45 { get; set; }
        [Column("TITLE_COLM_NO46")]
        [StringLength(250)]
        public string TitleColmNo46 { get; set; }
        [Column("TITLE_COLM_NO47")]
        [StringLength(250)]
        public string TitleColmNo47 { get; set; }
        [Column("TITLE_COLM_NO48")]
        [StringLength(250)]
        public string TitleColmNo48 { get; set; }
        [Column("TITLE_COLM_NO49")]
        [StringLength(250)]
        public string TitleColmNo49 { get; set; }
        [Column("TITLE_COLM_NO50")]
        [StringLength(250)]
        public string TitleColmNo50 { get; set; }
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
        [Column("TP_SA_FORM_ID", TypeName = "numeric(10, 0)")]
        public decimal? TpSaFormId { get; set; }
        [Column("PROSP_ID", TypeName = "numeric(10, 0)")]
        public decimal? ProspId { get; set; }
    }
}
