using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Entity
{
    [Table("PROSPECT")]
    public partial class Prospect
    {
        [Key]
        [Column("PROSPECT_ID", TypeName = "numeric(10, 0)")]
        public decimal ProspectId { get; set; }
        [Column("PROSP_ACC_ID", TypeName = "numeric(10, 0)")]
        public decimal? ProspAccId { get; set; }
        [Column("BU_ID", TypeName = "numeric(10, 0)")]
        public decimal? BuId { get; set; }
        [Column("SERVICES_TYPE_ID")]
        [StringLength(250)]
        public string ServicesTypeId { get; set; }
        [Column("LOC_TYPE_ID", TypeName = "numeric(10, 0)")]
        public decimal? LocTypeId { get; set; }
        [Column("SALE_REP_ID")]
        [StringLength(20)]
        public string SaleRepId { get; set; }
        [Column("PROSPECT_TYPE")]
        [StringLength(1)]
        public string ProspectType { get; set; }
        [Column("STATION_NAME")]
        [StringLength(250)]
        public string StationName { get; set; }
        [Column("STATION_OPEN_FLAG")]
        [StringLength(1)]
        public string StationOpenFlag { get; set; }
        [Column("REASON_CANCEL")]
        [StringLength(250)]
        public string ReasonCancel { get; set; }
        [Column("BRAND_CATE_ID", TypeName = "numeric(10, 0)")]
        public decimal? BrandCateId { get; set; }
        [Column("BRAND_CATE_OTHER")]
        [StringLength(250)]
        public string BrandCateOther { get; set; }
        [Column("AREA_SQUARE_WA", TypeName = "numeric(5, 0)")]
        public decimal? AreaSquareWa { get; set; }
        [Column("AREA_NGAN", TypeName = "numeric(5, 0)")]
        public decimal? AreaNgan { get; set; }
        [Column("AREA_RAI", TypeName = "numeric(5, 0)")]
        public decimal? AreaRai { get; set; }
        [Column("AREA_WIDTH_METER", TypeName = "numeric(5, 0)")]
        public decimal? AreaWidthMeter { get; set; }
        [Column("SHOP_JOINT")]
        [StringLength(250)]
        public string ShopJoint { get; set; }
        [Column("LICENSE_STATUS")]
        [StringLength(1)]
        public string LicenseStatus { get; set; }
        [Column("LICENSE_OTHER")]
        [StringLength(250)]
        public string LicenseOther { get; set; }
        [Column("INTEREST_STATUS")]
        [StringLength(1)]
        public string InterestStatus { get; set; }
        [Column("INTEREST_OTHER")]
        [StringLength(250)]
        public string InterestOther { get; set; }
        [Column("SALE_VOLUME", TypeName = "numeric(10, 0)")]
        public decimal? SaleVolume { get; set; }
        [Column("PROGRESS_DATE", TypeName = "datetime")]
        public DateTime? ProgressDate { get; set; }
        [Column("TERMINATE_DATE", TypeName = "datetime")]
        public DateTime? TerminateDate { get; set; }
        [Column("NEAR_BANK_ID", TypeName = "numeric(10, 0)")]
        public decimal? NearBankId { get; set; }
        [Column("QUOTA_OIL", TypeName = "numeric(10, 0)")]
        public decimal? QuotaOil { get; set; }
        [Column("QUOTA_LUBE", TypeName = "numeric(10, 0)")]
        public decimal? QuotaLube { get; set; }
        [Column("DISPENSER_TOTAL", TypeName = "numeric(2, 0)")]
        public decimal? DispenserTotal { get; set; }
        [Column("NOZZLE_TOTAL", TypeName = "numeric(2, 0)")]
        public decimal? NozzleTotal { get; set; }
        [Column("ADDR_TITLE_DEED_NO")]
        [StringLength(20)]
        public string AddrTitleDeedNo { get; set; }
        [Column("ADDR_CERT_UTILISATION")]
        [StringLength(250)]
        public string AddrCertUtilisation { get; set; }
        [Column("ADDR_PARCEL_NO")]
        [StringLength(20)]
        public string AddrParcelNo { get; set; }
        [Column("ADDR_TAMBON_NO")]
        [StringLength(20)]
        public string AddrTambonNo { get; set; }
        [Column("DBD_CODE")]
        [StringLength(20)]
        public string DbdCode { get; set; }
        [Column("DBD_CORP_TYPE")]
        [StringLength(250)]
        public string DbdCorpType { get; set; }
        [Column("DBD_JURISTIC_STATUS")]
        [StringLength(50)]
        public string DbdJuristicStatus { get; set; }
        [Column("DBD_REG_CAPITAL", TypeName = "numeric(20, 2)")]
        public decimal? DbdRegCapital { get; set; }
        [Column("DBD_TOTAL_INCOME", TypeName = "numeric(20, 2)")]
        public decimal? DbdTotalIncome { get; set; }
        [Column("DBD_PROFIT_LOSS", TypeName = "numeric(20, 2)")]
        public decimal? DbdProfitLoss { get; set; }
        [Column("DBD_TOTAL_ASSET", TypeName = "numeric(20, 2)")]
        public decimal? DbdTotalAsset { get; set; }
        [Column("DBD_FLEET_CARD")]
        [StringLength(20)]
        public string DbdFleetCard { get; set; }
        [Column("DBD_CORP_CARD")]
        [StringLength(20)]
        public string DbdCorpCard { get; set; }
        [Column("DBD_OIL_CONSUPTION")]
        [StringLength(50)]
        public string DbdOilConsuption { get; set; }
        [Column("DBD_CURRENT_STATION")]
        [StringLength(20)]
        public string DbdCurrentStation { get; set; }
        [Column("DBD_PAY_CHANNEL")]
        [StringLength(1)]
        public string DbdPayChannel { get; set; }
        [Column("DBD_CAR_WHEEL4")]
        [StringLength(250)]
        public string DbdCarWheel4 { get; set; }
        [Column("DBD_CAR_WHEEL6")]
        [StringLength(250)]
        public string DbdCarWheel6 { get; set; }
        [Column("DBD_CAR_WHEEL8")]
        [StringLength(250)]
        public string DbdCarWheel8 { get; set; }
        [Column("DBD_CARAVAN")]
        [StringLength(250)]
        public string DbdCaravan { get; set; }
        [Column("DBD_CAR_TRAILER")]
        [StringLength(250)]
        public string DbdCarTrailer { get; set; }
        [Column("DBD_CAR_CONTAINER")]
        [StringLength(250)]
        public string DbdCarContainer { get; set; }
        [Column("DBD_OTHER")]
        [StringLength(250)]
        public string DbdOther { get; set; }
        [Column("DBD_TANK")]
        [StringLength(250)]
        public string DbdTank { get; set; }
        [Column("DBD_STATION")]
        [StringLength(250)]
        public string DbdStation { get; set; }
        [Column("DBD_TYPE2")]
        [StringLength(250)]
        public string DbdType2 { get; set; }
        [Column("DBD_MAINTAIN_CENTER")]
        [StringLength(250)]
        public string DbdMaintainCenter { get; set; }
        [Column("DBD_GENERAL_GARAGE")]
        [StringLength(250)]
        public string DbdGeneralGarage { get; set; }
        [Column("DBD_MAINTAIN_DEPT")]
        [StringLength(250)]
        public string DbdMaintainDept { get; set; }
        [Column("DBD_RECOMMENDER")]
        [StringLength(250)]
        public string DbdRecommender { get; set; }
        [Column("DBD_SALE")]
        [StringLength(250)]
        public string DbdSale { get; set; }
        [Column("DBD_SALE_SUPPORT")]
        [StringLength(250)]
        public string DbdSaleSupport { get; set; }
        [Column("DBD_REMARK")]
        [StringLength(250)]
        public string DbdRemark { get; set; }
        [Column("MAIN_FLAG")]
        [StringLength(1)]
        public string MainFlag { get; set; }
        [Column("PROSPECT_STATUS")]
        [StringLength(1)]
        public string ProspectStatus { get; set; }
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
        [Column("SALE_VOLUME_REF")]
        [StringLength(250)]
        public string SaleVolumeRef { get; set; }
        [Column("DBD_MACHINE")]
        [StringLength(250)]
        public string DbdMachine { get; set; }
    }
}
