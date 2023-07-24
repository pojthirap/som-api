using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearchProspectSaTabCustom
    {

        public string EditGeneralDataFlag { get; set; }

        // Account
        public decimal ProspAccId { get; set; }
        public string AccName { get; set; }
        public decimal? BrandId { get; set; }
        public string CustCode { get; set; }
        public string IdentifyId { get; set; }
        public string AccGroupRef { get; set; }
        public string AccountRemark { get; set; }
        public string SourceType { get; set; }
        public string AccountActiveFlag { get; set; }



        // Pospect


        public decimal ProspectId { get; set; }
        //public decimal? ProspAccId { get; set; }
        public decimal? BuId { get; set; }
        public string ServicesTypeId { get; set; }
        public decimal? LocTypeId { get; set; }
        public string SaleRepId { get; set; }
        public string ProspectType { get; set; }
        public string StationName { get; set; }
        public string StationOpenFlag { get; set; }
        public string ReasonCancel { get; set; }
        public decimal? BrandCateId { get; set; }
        public string BrandCateOther { get; set; }
        public decimal? AreaSquareWa { get; set; }
        public decimal? AreaNgan { get; set; }
        public decimal? AreaRai { get; set; }
        public decimal? AreaWidthMeter { get; set; }
        public string ShopJoint { get; set; }
        public string LicenseStatus { get; set; }
        public string LicenseOther { get; set; }
        public string InterestStatus { get; set; }
        public string InterestOther { get; set; }
        public string? SaleVolumeRef { get; set; }
        public decimal? SaleVolume { get; set; }
        public DateTime? ProgressDate { get; set; }
        public DateTime? TerminateDate { get; set; }
        public decimal? NearBankId { get; set; }
        public decimal? QuotaOil { get; set; }
        public decimal? QuotaLube { get; set; }
        public decimal? DispenserTotal { get; set; }
        public decimal? NozzleTotal { get; set; }
        public string AddrTitleDeedNo { get; set; }
        public string AddrCertUtilisation { get; set; }
        public string AddrParcelNo { get; set; }
        public string AddrTambonNo { get; set; }
        public string DbdCode { get; set; }
        public string DbdCorpType { get; set; }
        public string DbdJuristicStatus { get; set; }
        public decimal? DbdRegCapital { get; set; }
        public decimal? DbdTotalIncome { get; set; }
        public decimal? DbdProfitLoss { get; set; }
        public decimal? DbdTotalAsset { get; set; }
        public string DbdFleetCard { get; set; }
        public string DbdCorpCard { get; set; }
        public string DbdOilConsuption { get; set; }
        public string DbdCurrentStation { get; set; }
        public string DbdPayChannel { get; set; }
        public string DbdCarWheel4 { get; set; }
        public string DbdCarWheel6 { get; set; }
        public string DbdCarWheel8 { get; set; }
        public string DbdCaravan { get; set; }
        public string DbdCarTrailer { get; set; }
        public string DbdCarContainer { get; set; }
        public string DbdOther { get; set; }
        public string DbdTank { get; set; }
        public string DbdStation { get; set; }
        public string DbdType2 { get; set; }
        public string DbdMaintainCenter { get; set; }
        public string DbdGeneralGarage { get; set; }
        public string DbdMaintainDept { get; set; }
        public string DbdRecommender { get; set; }
        public string DbdSale { get; set; }
        public string DbdSaleSupport { get; set; }
        public string DbdRemark { get; set; }
        public string ProspectMainFlag { get; set; }
        public string ProspectStatus { get; set; }
        public string DbdMachine { get; set; }


        // Contact
        public decimal ProspContactId { get; set; }
        //public decimal? ProspectId { get; set; }
        //public decimal? ProspAccId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string ContactFaxNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string ContactMainFlag { get; set; }
        public string ContactActiveFlag { get; set; }








        // Address
        public decimal ProspAddrId { get; set; }
        //public decimal? ProspectId { get; set; }
        //public decimal? ProspAccId { get; set; }
        public string AddrNo { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Street { get; set; }
        public string TellNo { get; set; }
        public string AddressFaxNo { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string RegionCode { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceDbd { get; set; }
        public string DistrictCode { get; set; }
        public string SubdistrictCode { get; set; }
        public string PostCode { get; set; }
        public string AddressRemark { get; set; }
        public string AddressMainFlag { get; set; }
        public string AddressActiveFlag { get; set; }


        //PROVINCE
        
       public string ProvinceNameTh { get; set; }



    }
}
