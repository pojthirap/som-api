using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Entity
{
    public partial class sqlsaleonmobiledevContext : DbContext
    {
        public sqlsaleonmobiledevContext()
        {
        }

        public sqlsaleonmobiledevContext(DbContextOptions<sqlsaleonmobiledevContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdmEmployee> AdmEmployees { get; set; }
        public virtual DbSet<AdmEmployeeHrm> AdmEmployeeHrms { get; set; }
        public virtual DbSet<AdmGroup> AdmGroups { get; set; }
        public virtual DbSet<AdmGroupApp> AdmGroupApps { get; set; }
        public virtual DbSet<AdmGroupPerm> AdmGroupPerms { get; set; }
        public virtual DbSet<AdmGroupUser> AdmGroupUsers { get; set; }
        public virtual DbSet<AdmLogLogin> AdmLogLogins { get; set; }
        public virtual DbSet<AdmPermObject> AdmPermObjects { get; set; }
        public virtual DbSet<AdmSession> AdmSessions { get; set; }
        public virtual DbSet<AdmUserAccessToken> AdmUserAccessTokens { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerAccountGroup> CustomerAccountGroups { get; set; }
        public virtual DbSet<CustomerCompany> CustomerCompanies { get; set; }
        public virtual DbSet<CustomerPartner> CustomerPartners { get; set; }
        public virtual DbSet<CustomerSale> CustomerSales { get; set; }
        public virtual DbSet<EmailJob> EmailJobs { get; set; }
        public virtual DbSet<ImportErrorFileLog> ImportErrorFileLogs { get; set; }
        public virtual DbSet<MsAttachCategory> MsAttachCategories { get; set; }
        public virtual DbSet<MsBank> MsBanks { get; set; }
        public virtual DbSet<MsBrand> MsBrands { get; set; }
        public virtual DbSet<MsBrandCategory> MsBrandCategories { get; set; }
        public virtual DbSet<MsConfigLov> MsConfigLovs { get; set; }
        public virtual DbSet<MsConfigParam> MsConfigParams { get; set; }
        public virtual DbSet<MsCountry> MsCountries { get; set; }
        public virtual DbSet<MsDistrict> MsDistricts { get; set; }
        public virtual DbSet<MsGasoline> MsGasolines { get; set; }
        public virtual DbSet<MsLocation> MsLocations { get; set; }
        public virtual DbSet<MsLocationType> MsLocationTypes { get; set; }
        public virtual DbSet<MsMeter> MsMeters { get; set; }
        public virtual DbSet<MsOrderDocType> MsOrderDocTypes { get; set; }
        public virtual DbSet<MsOrderDocTypeArea> MsOrderDocTypeAreas { get; set; }
        public virtual DbSet<MsOrderIncoterm> MsOrderIncoterms { get; set; }
        public virtual DbSet<MsOrderItemType> MsOrderItemTypes { get; set; }
        public virtual DbSet<MsOrderReason> MsOrderReasons { get; set; }
        public virtual DbSet<MsPlant> MsPlants { get; set; }
        public virtual DbSet<MsPlantProduct> MsPlantProducts { get; set; }
        public virtual DbSet<MsPlantShip> MsPlantShips { get; set; }
        public virtual DbSet<MsPlantTemp> MsPlantTemps { get; set; }
        public virtual DbSet<MsProduct> MsProducts { get; set; }
        public virtual DbSet<MsProductConversion> MsProductConversions { get; set; }
        public virtual DbSet<MsProductPlant> MsProductPlants { get; set; }
        public virtual DbSet<MsProductSale> MsProductSales { get; set; }
        public virtual DbSet<MsProvince> MsProvinces { get; set; }
        public virtual DbSet<MsRegion> MsRegions { get; set; }
        public virtual DbSet<MsServiceType> MsServiceTypes { get; set; }
        public virtual DbSet<MsShip> MsShips { get; set; }
        public virtual DbSet<MsSubdistrict> MsSubdistricts { get; set; }
        public virtual DbSet<OrgBusinessArea> OrgBusinessAreas { get; set; }
        public virtual DbSet<OrgBusinessUnit> OrgBusinessUnits { get; set; }
        public virtual DbSet<OrgCompany> OrgCompanies { get; set; }
        public virtual DbSet<OrgDistChannel> OrgDistChannels { get; set; }
        public virtual DbSet<OrgDivision> OrgDivisions { get; set; }
        public virtual DbSet<OrgSaleArea> OrgSaleAreas { get; set; }
        public virtual DbSet<OrgSaleGroup> OrgSaleGroups { get; set; }
        public virtual DbSet<OrgSaleOffice> OrgSaleOffices { get; set; }
        public virtual DbSet<OrgSaleOfficeArea> OrgSaleOfficeAreas { get; set; }
        public virtual DbSet<OrgSaleOrganization> OrgSaleOrganizations { get; set; }
        public virtual DbSet<OrgSaleTerritory> OrgSaleTerritories { get; set; }
        public virtual DbSet<OrgTerritory> OrgTerritories { get; set; }
        public virtual DbSet<PlanReasonNotVisit> PlanReasonNotVisits { get; set; }
        public virtual DbSet<PlanTrip> PlanTrips { get; set; }
        public virtual DbSet<PlanTripProspect> PlanTripProspects { get; set; }
        public virtual DbSet<PlanTripTask> PlanTripTasks { get; set; }
        public virtual DbSet<Prospect> Prospects { get; set; }
        public virtual DbSet<ProspectAccount> ProspectAccounts { get; set; }
        public virtual DbSet<ProspectAddress> ProspectAddresses { get; set; }
        public virtual DbSet<ProspectContact> ProspectContacts { get; set; }
        public virtual DbSet<ProspectDedicateTert> ProspectDedicateTerts { get; set; }
        public virtual DbSet<ProspectFeed> ProspectFeeds { get; set; }
        public virtual DbSet<ProspectRecommend> ProspectRecommends { get; set; }
        public virtual DbSet<ProspectVisitHour> ProspectVisitHours { get; set; }
        public virtual DbSet<RecordAppForm> RecordAppForms { get; set; }
        public virtual DbSet<RecordAppFormFile> RecordAppFormFiles { get; set; }
        public virtual DbSet<RecordMeter> RecordMeters { get; set; }
        public virtual DbSet<RecordMeterFile> RecordMeterFiles { get; set; }
        public virtual DbSet<RecordSaForm> RecordSaForms { get; set; }
        public virtual DbSet<RecordSaFormFile> RecordSaFormFiles { get; set; }
        public virtual DbSet<RecordStockCard> RecordStockCards { get; set; }
        public virtual DbSet<SaleOrder> SaleOrders { get; set; }
        public virtual DbSet<SaleOrderChangeLog> SaleOrderChangeLogs { get; set; }
        public virtual DbSet<SaleOrderLog> SaleOrderLogs { get; set; }
        public virtual DbSet<SaleOrderProduct> SaleOrderProducts { get; set; }
        public virtual DbSet<SaleOrderProductLog> SaleOrderProductLogs { get; set; }
        public virtual DbSet<SyncDataLog> SyncDataLogs { get; set; }
        public virtual DbSet<TemplateAppForm> TemplateAppForms { get; set; }
        public virtual DbSet<TemplateAppQuestion> TemplateAppQuestions { get; set; }
        public virtual DbSet<TemplateCategory> TemplateCategories { get; set; }
        public virtual DbSet<TemplateQuestion> TemplateQuestions { get; set; }
        public virtual DbSet<TemplateSaForm> TemplateSaForms { get; set; }
        public virtual DbSet<TemplateSaTitle> TemplateSaTitles { get; set; }
        public virtual DbSet<TemplateStockCard> TemplateStockCards { get; set; }
        public virtual DbSet<TemplateStockProduct> TemplateStockProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Initial Catalog = XXXX; Data Source =XXXX; User ID=XXXX; Password=XXXX; Integrated Security = false; Pooling = true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Thai_CI_AS");

            modelBuilder.Entity<AdmEmployee>(entity =>
            {
                entity.Property(e => e.EmpId).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ApproveEmpId).IsUnicode(false);

                entity.Property(e => e.CompanyCode).IsUnicode(false);

                entity.Property(e => e.CountryName).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DistrictName).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.Gender)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.GroupCode).IsUnicode(false);

                entity.Property(e => e.JobTitle).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.PostCode).IsUnicode(false);

                entity.Property(e => e.ProvinceCode).IsUnicode(false);

                entity.Property(e => e.ProvinceName).IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Street).IsUnicode(false);

                entity.Property(e => e.SubdistrictName).IsUnicode(false);

                entity.Property(e => e.TellNo).IsUnicode(false);

                entity.Property(e => e.TitleName).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<AdmEmployeeHrm>(entity =>
            {
                entity.Property(e => e.EmpId).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ApproveEmpId).IsUnicode(false);

                entity.Property(e => e.CompanyCode).IsUnicode(false);

                entity.Property(e => e.CountryName).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DistrictName).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.Gender)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.GroupCode).IsUnicode(false);

                entity.Property(e => e.JobTitle).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.PostCode).IsUnicode(false);

                entity.Property(e => e.ProvinceCode).IsUnicode(false);

                entity.Property(e => e.ProvinceName).IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Street).IsUnicode(false);

                entity.Property(e => e.SubdistrictName).IsUnicode(false);

                entity.Property(e => e.TellNo).IsUnicode(false);

                entity.Property(e => e.TitleName).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<AdmGroup>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.GroupCode).IsUnicode(false);

                entity.Property(e => e.GroupNameEn).IsUnicode(false);

                entity.Property(e => e.GroupNameTh).IsUnicode(false);

                entity.Property(e => e.SystemFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<AdmGroupApp>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<AdmGroupPerm>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<AdmGroupUser>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.EmpId).IsUnicode(false);

                entity.Property(e => e.GroupUserType)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<AdmLogLogin>(entity =>
            {
                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.IpAddress).IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UserAgent).IsUnicode(false);

                entity.Property(e => e.UserName).IsUnicode(false);
            });

            modelBuilder.Entity<AdmPermObject>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.PermObjCode).IsUnicode(false);

                entity.Property(e => e.PermObjNameEn).IsUnicode(false);

                entity.Property(e => e.PermObjNameTh).IsUnicode(false);

                entity.Property(e => e.PermType)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<AdmSession>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.EmpId).IsUnicode(false);

                entity.Property(e => e.IpAddress).IsUnicode(false);

                entity.Property(e => e.TokenNo).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);

                entity.Property(e => e.UserAgent).IsUnicode(false);

                entity.Property(e => e.Valid)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<AdmUserAccessToken>(entity =>
            {
                entity.Property(e => e.AccessToken).IsUnicode(false);

                entity.Property(e => e.AccessType)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.EmpId).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustCode).IsUnicode(false);

                entity.Property(e => e.AccGroupCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CountryCode)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.CustNameEn).IsUnicode(false);

                entity.Property(e => e.CustNameTh).IsUnicode(false);

                entity.Property(e => e.DistrictCode).IsUnicode(false);

                entity.Property(e => e.DistrictName).IsUnicode(false);

                entity.Property(e => e.PostCode).IsUnicode(false);

                entity.Property(e => e.ProvinceCode)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SearchTerm).IsUnicode(false);

                entity.Property(e => e.Street).IsUnicode(false);

                entity.Property(e => e.SubdistrictCode).IsUnicode(false);

                entity.Property(e => e.SubdistrictName).IsUnicode(false);

                entity.Property(e => e.TaxNo).IsUnicode(false);

                entity.Property(e => e.TellNo).IsUnicode(false);

                entity.Property(e => e.TransportZone).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);

                entity.Property(e => e.VatNo).IsUnicode(false);
            });

            modelBuilder.Entity<CustomerAccountGroup>(entity =>
            {
                entity.Property(e => e.AccGroupCode).IsUnicode(false);

                entity.Property(e => e.AccGroupNameEn).IsUnicode(false);

                entity.Property(e => e.AccGroupNameTh).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<CustomerCompany>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CompanyCode).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.CustCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<CustomerPartner>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ChannelCode).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.CustCode).IsUnicode(false);

                entity.Property(e => e.CustCodePartner).IsUnicode(false);

                entity.Property(e => e.DivisionCode).IsUnicode(false);

                entity.Property(e => e.FuncCode)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OrgCode).IsUnicode(false);

                entity.Property(e => e.PartnerCounter).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<CustomerSale>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ChannelCode).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.CustCode).IsUnicode(false);

                entity.Property(e => e.CustGroup).IsUnicode(false);

                entity.Property(e => e.DivisionCode).IsUnicode(false);

                entity.Property(e => e.GroupCode).IsUnicode(false);

                entity.Property(e => e.Incoterm).IsUnicode(false);

                entity.Property(e => e.OfficeCode).IsUnicode(false);

                entity.Property(e => e.OrgCode).IsUnicode(false);

                entity.Property(e => e.PaymentTerm).IsUnicode(false);

                entity.Property(e => e.ShippingCond).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<EmailJob>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.JobStatus)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TableRefKeyId).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<ImportErrorFileLog>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PK_PROSPECT_LOG_ERROR_FILE");

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.FileExt).IsUnicode(false);

                entity.Property(e => e.FileName).IsUnicode(false);

                entity.Property(e => e.FileSize).IsUnicode(false);

                entity.Property(e => e.ImportDataType)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsAttachCategory>(entity =>
            {
                entity.HasKey(e => e.AttachCateId)
                    .HasName("PK_MS_ACTTACH_CATEGORY");

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AttachCateCode).IsUnicode(false);

                entity.Property(e => e.AttachCateNameEn).IsUnicode(false);

                entity.Property(e => e.AttachCateNameTh).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsBank>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BankCode).IsUnicode(false);

                entity.Property(e => e.BankNameEn).IsUnicode(false);

                entity.Property(e => e.BankNameTh).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsBrand>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BrandCode).IsUnicode(false);

                entity.Property(e => e.BrandNameEn).IsUnicode(false);

                entity.Property(e => e.BrandNameTh).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsBrandCategory>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BrandCateCode).IsUnicode(false);

                entity.Property(e => e.BrandCateNameEn).IsUnicode(false);

                entity.Property(e => e.BrandCateNameTh).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsConfigLov>(entity =>
            {
                entity.HasKey(e => new { e.LovKeyword, e.LovKeyvalue });

                entity.Property(e => e.LovKeyword).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Condition1).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.LovCodeEn).IsUnicode(false);

                entity.Property(e => e.LovCodeTh).IsUnicode(false);

                entity.Property(e => e.LovNameEn).IsUnicode(false);

                entity.Property(e => e.LovNameTh).IsUnicode(false);

                entity.Property(e => e.LovRemark).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsConfigParam>(entity =>
            {
                entity.Property(e => e.ParamKeyword).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.ParamDesc).IsUnicode(false);

                entity.Property(e => e.ParamNameEn).IsUnicode(false);

                entity.Property(e => e.ParamNameTh).IsUnicode(false);

                entity.Property(e => e.ParamValue).IsUnicode(false);

                entity.Property(e => e.SystemFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsCountry>(entity =>
            {
                entity.Property(e => e.CountryCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CountryNameEn).IsUnicode(false);

                entity.Property(e => e.CountryNameTh).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.LangKey).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsDistrict>(entity =>
            {
                entity.Property(e => e.DistrictCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DistrictNameEn).IsUnicode(false);

                entity.Property(e => e.DistrictNameTh).IsUnicode(false);

                entity.Property(e => e.ProvinceCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsGasoline>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.GasCode).IsUnicode(false);

                entity.Property(e => e.GasNameEn).IsUnicode(false);

                entity.Property(e => e.GasNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsLocation>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.Latitude).IsUnicode(false);

                entity.Property(e => e.LocCode).IsUnicode(false);

                entity.Property(e => e.LocNameEn).IsUnicode(false);

                entity.Property(e => e.LocNameTh).IsUnicode(false);

                entity.Property(e => e.Longitude).IsUnicode(false);

                entity.Property(e => e.ProvinceCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsLocationType>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.LocTypeCode).IsUnicode(false);

                entity.Property(e => e.LocTypeNameEn).IsUnicode(false);

                entity.Property(e => e.LocTypeNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsMeter>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.CustCode).IsUnicode(false);

                entity.Property(e => e.Qrcode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsOrderDocType>(entity =>
            {
                entity.Property(e => e.DocTypeCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DocTypeNameEn).IsUnicode(false);

                entity.Property(e => e.DocTypeNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsOrderDocTypeArea>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DocTypeCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsOrderIncoterm>(entity =>
            {
                entity.Property(e => e.IncotermCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsOrderItemType>(entity =>
            {
                entity.Property(e => e.ItemTypeCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.ItemTypeNameEn).IsUnicode(false);

                entity.Property(e => e.ItemTypeNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsOrderReason>(entity =>
            {
                entity.Property(e => e.ReasonCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.ReasonNameEn).IsUnicode(false);

                entity.Property(e => e.ReasonNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsPlant>(entity =>
            {
                entity.Property(e => e.PlantCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BussPlace).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.FactCalendar).IsUnicode(false);

                entity.Property(e => e.PlantCustNo).IsUnicode(false);

                entity.Property(e => e.PlantNameEn).IsUnicode(false);

                entity.Property(e => e.PlantNameTh).IsUnicode(false);

                entity.Property(e => e.PlantVendorNo).IsUnicode(false);

                entity.Property(e => e.PurchOrg).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsPlantProduct>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.PlantCode).IsUnicode(false);

                entity.Property(e => e.ProdCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsPlantShip>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.PlantCode).IsUnicode(false);

                entity.Property(e => e.ShipCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsPlantTemp>(entity =>
            {
                entity.Property(e => e.PlantCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BussPlace).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.CompanyCode).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.FactCalendar).IsUnicode(false);

                entity.Property(e => e.PlantCustNo).IsUnicode(false);

                entity.Property(e => e.PlantNameEn).IsUnicode(false);

                entity.Property(e => e.PlantNameTh).IsUnicode(false);

                entity.Property(e => e.PlantVendorNo).IsUnicode(false);

                entity.Property(e => e.PurchOrg).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsProduct>(entity =>
            {
                entity.Property(e => e.ProdCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BaseUnit).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DivisionCode).IsUnicode(false);

                entity.Property(e => e.IndustrySector)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OldProdNo).IsUnicode(false);

                entity.Property(e => e.ProdGroup).IsUnicode(false);

                entity.Property(e => e.ProdNameEn).IsUnicode(false);

                entity.Property(e => e.ProdNameTh).IsUnicode(false);

                entity.Property(e => e.ProdType).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsProductConversion>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AltUnit).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.ProdCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);

                entity.Property(e => e.WeightUnit).IsUnicode(false);
            });

            modelBuilder.Entity<MsProductPlant>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.PlantCode).IsUnicode(false);

                entity.Property(e => e.ProdCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsProductSale>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ChannelCode).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.OrgCode).IsUnicode(false);

                entity.Property(e => e.ProdCateCode).IsUnicode(false);

                entity.Property(e => e.ProdCateDesc).IsUnicode(false);

                entity.Property(e => e.ProdCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsProvince>(entity =>
            {
                entity.Property(e => e.ProvinceCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CountryCode).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.ProvinceNameEn).IsUnicode(false);

                entity.Property(e => e.ProvinceNameTh).IsUnicode(false);

                entity.Property(e => e.RegionCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsRegion>(entity =>
            {
                entity.Property(e => e.RegionCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.RegionNameEn).IsUnicode(false);

                entity.Property(e => e.RegionNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsServiceType>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.ServiceCode).IsUnicode(false);

                entity.Property(e => e.ServiceNameEn).IsUnicode(false);

                entity.Property(e => e.ServiceNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsShip>(entity =>
            {
                entity.Property(e => e.ShipCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DescriptionEn).IsUnicode(false);

                entity.Property(e => e.DescriptionTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<MsSubdistrict>(entity =>
            {
                entity.Property(e => e.SubdistrictCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DistrictCode).IsUnicode(false);

                entity.Property(e => e.SubdistrictNameEn).IsUnicode(false);

                entity.Property(e => e.SubdistrictNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<OrgBusinessArea>(entity =>
            {
                entity.Property(e => e.BussAreaCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BussAreaNameEn).IsUnicode(false);

                entity.Property(e => e.BussAreaNameTh).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<OrgBusinessUnit>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BuCode).IsUnicode(false);

                entity.Property(e => e.BuNameEn).IsUnicode(false);

                entity.Property(e => e.BuNameTh).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<OrgCompany>(entity =>
            {
                entity.Property(e => e.CompanyCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CompanyNameEn).IsUnicode(false);

                entity.Property(e => e.CompanyNameTh).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);

                entity.Property(e => e.VatRegistNo).IsUnicode(false);
            });

            modelBuilder.Entity<OrgDistChannel>(entity =>
            {
                entity.Property(e => e.ChannelCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ChannelNameEn).IsUnicode(false);

                entity.Property(e => e.ChannelNameTh).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<OrgDivision>(entity =>
            {
                entity.Property(e => e.DivisionCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DivisionNameEn).IsUnicode(false);

                entity.Property(e => e.DivisionNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<OrgSaleArea>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BussAreaCode).IsUnicode(false);

                entity.Property(e => e.ChannelCode).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DivisionCode).IsUnicode(false);

                entity.Property(e => e.OrgCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<OrgSaleGroup>(entity =>
            {
                entity.Property(e => e.GroupCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DescriptionEn).IsUnicode(false);

                entity.Property(e => e.DescriptionTh).IsUnicode(false);

                entity.Property(e => e.ManagerEmpId).IsUnicode(false);

                entity.Property(e => e.OfficeCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<OrgSaleOffice>(entity =>
            {
                entity.Property(e => e.OfficeCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DescriptionEn).IsUnicode(false);

                entity.Property(e => e.DescriptionTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<OrgSaleOfficeArea>(entity =>
            {
                entity.HasKey(e => e.OfficeAreaId)
                    .HasName("PK_ORG_SALE_AREA_OFFICE");

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ChannelCode).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DivisionCode).IsUnicode(false);

                entity.Property(e => e.OfficeCode).IsUnicode(false);

                entity.Property(e => e.OrgCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<OrgSaleOrganization>(entity =>
            {
                entity.Property(e => e.OrgCode).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CompanyCode).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.Currency).IsUnicode(false);

                entity.Property(e => e.OrgNameEn).IsUnicode(false);

                entity.Property(e => e.OrgNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<OrgSaleTerritory>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.EmpId).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<OrgTerritory>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.ManagerEmpId).IsUnicode(false);

                entity.Property(e => e.TerritoryCode).IsUnicode(false);

                entity.Property(e => e.TerritoryNameEn).IsUnicode(false);

                entity.Property(e => e.TerritoryNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<PlanReasonNotVisit>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.ReasonCode).IsUnicode(false);

                entity.Property(e => e.ReasonNameEn).IsUnicode(false);

                entity.Property(e => e.ReasonNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<PlanTrip>(entity =>
            {
                entity.Property(e => e.AssignEmpId).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.PlanTripName).IsUnicode(false);

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<PlanTripProspect>(entity =>
            {
                entity.Property(e => e.AdhocFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.LocRemark).IsUnicode(false);

                entity.Property(e => e.MergFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ReasonNotVisitRemark).IsUnicode(false);

                entity.Property(e => e.Remind).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);

                entity.Property(e => e.VisitLatitude).IsUnicode(false);

                entity.Property(e => e.VisitLongitude).IsUnicode(false);
            });

            modelBuilder.Entity<PlanTripTask>(entity =>
            {
                entity.Property(e => e.AdhocFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.RequireFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TaskType)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<Prospect>(entity =>
            {
                entity.Property(e => e.AddrCertUtilisation).IsUnicode(false);

                entity.Property(e => e.AddrParcelNo).IsUnicode(false);

                entity.Property(e => e.AddrTambonNo).IsUnicode(false);

                entity.Property(e => e.AddrTitleDeedNo).IsUnicode(false);

                entity.Property(e => e.BrandCateOther).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DbdCarContainer).IsUnicode(false);

                entity.Property(e => e.DbdCarTrailer).IsUnicode(false);

                entity.Property(e => e.DbdCarWheel4).IsUnicode(false);

                entity.Property(e => e.DbdCarWheel6).IsUnicode(false);

                entity.Property(e => e.DbdCarWheel8).IsUnicode(false);

                entity.Property(e => e.DbdCaravan).IsUnicode(false);

                entity.Property(e => e.DbdCode).IsUnicode(false);

                entity.Property(e => e.DbdCorpCard).IsUnicode(false);

                entity.Property(e => e.DbdCorpType).IsUnicode(false);

                entity.Property(e => e.DbdCurrentStation).IsUnicode(false);

                entity.Property(e => e.DbdFleetCard).IsUnicode(false);

                entity.Property(e => e.DbdGeneralGarage).IsUnicode(false);

                entity.Property(e => e.DbdJuristicStatus).IsUnicode(false);

                entity.Property(e => e.DbdMachine).IsUnicode(false);

                entity.Property(e => e.DbdMaintainCenter).IsUnicode(false);

                entity.Property(e => e.DbdMaintainDept).IsUnicode(false);

                entity.Property(e => e.DbdOilConsuption).IsUnicode(false);

                entity.Property(e => e.DbdOther).IsUnicode(false);

                entity.Property(e => e.DbdPayChannel)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DbdRecommender).IsUnicode(false);

                entity.Property(e => e.DbdRemark).IsUnicode(false);

                entity.Property(e => e.DbdSale).IsUnicode(false);

                entity.Property(e => e.DbdSaleSupport).IsUnicode(false);

                entity.Property(e => e.DbdStation).IsUnicode(false);

                entity.Property(e => e.DbdTank).IsUnicode(false);

                entity.Property(e => e.DbdType2).IsUnicode(false);

                entity.Property(e => e.InterestOther).IsUnicode(false);

                entity.Property(e => e.InterestStatus)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.LicenseOther).IsUnicode(false);

                entity.Property(e => e.LicenseStatus)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.MainFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ProspectStatus)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ProspectType)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ReasonCancel).IsUnicode(false);

                entity.Property(e => e.SaleRepId).IsUnicode(false);

                entity.Property(e => e.SaleVolumeRef).IsUnicode(false);

                entity.Property(e => e.ServicesTypeId).IsUnicode(false);

                entity.Property(e => e.ShopJoint).IsUnicode(false);

                entity.Property(e => e.StationName).IsUnicode(false);

                entity.Property(e => e.StationOpenFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<ProspectAccount>(entity =>
            {
                entity.Property(e => e.AccGroupRef).IsUnicode(false);

                entity.Property(e => e.AccName).IsUnicode(false);

                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.CustCode).IsUnicode(false);

                entity.Property(e => e.IdentifyId).IsUnicode(false);

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.Property(e => e.SourceType)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<ProspectAddress>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AddrNo).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DistrictCode).IsUnicode(false);

                entity.Property(e => e.FaxNo).IsUnicode(false);

                entity.Property(e => e.Latitude).IsUnicode(false);

                entity.Property(e => e.Longitude).IsUnicode(false);

                entity.Property(e => e.MainFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Moo).IsUnicode(false);

                entity.Property(e => e.PostCode).IsUnicode(false);

                entity.Property(e => e.ProvinceCode).IsUnicode(false);

                entity.Property(e => e.ProvinceDbd).IsUnicode(false);

                entity.Property(e => e.RegionCode).IsUnicode(false);

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.Property(e => e.Soi).IsUnicode(false);

                entity.Property(e => e.Street).IsUnicode(false);

                entity.Property(e => e.SubdistrictCode).IsUnicode(false);

                entity.Property(e => e.TellNo).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<ProspectContact>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FaxNo).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.MainFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.MobileNo).IsUnicode(false);

                entity.Property(e => e.PhoneNo).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<ProspectDedicateTert>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<ProspectFeed>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<ProspectRecommend>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<ProspectVisitHour>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.DaysCode)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.HourEnd)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.HourStart)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<RecordAppForm>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<RecordAppFormFile>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.FileExt).IsUnicode(false);

                entity.Property(e => e.FileName).IsUnicode(false);

                entity.Property(e => e.FileSize).IsUnicode(false);

                entity.Property(e => e.PhotoFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<RecordMeter>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.PrevRecRunNo)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.RecRunNo)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<RecordMeterFile>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.FileExt).IsUnicode(false);

                entity.Property(e => e.FileName).IsUnicode(false);

                entity.Property(e => e.FileSize).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<RecordSaForm>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.TitleColmNo1).IsUnicode(false);

                entity.Property(e => e.TitleColmNo10).IsUnicode(false);

                entity.Property(e => e.TitleColmNo11).IsUnicode(false);

                entity.Property(e => e.TitleColmNo12).IsUnicode(false);

                entity.Property(e => e.TitleColmNo13).IsUnicode(false);

                entity.Property(e => e.TitleColmNo14).IsUnicode(false);

                entity.Property(e => e.TitleColmNo15).IsUnicode(false);

                entity.Property(e => e.TitleColmNo16).IsUnicode(false);

                entity.Property(e => e.TitleColmNo17).IsUnicode(false);

                entity.Property(e => e.TitleColmNo18).IsUnicode(false);

                entity.Property(e => e.TitleColmNo19).IsUnicode(false);

                entity.Property(e => e.TitleColmNo2).IsUnicode(false);

                entity.Property(e => e.TitleColmNo20).IsUnicode(false);

                entity.Property(e => e.TitleColmNo21).IsUnicode(false);

                entity.Property(e => e.TitleColmNo22).IsUnicode(false);

                entity.Property(e => e.TitleColmNo23).IsUnicode(false);

                entity.Property(e => e.TitleColmNo24).IsUnicode(false);

                entity.Property(e => e.TitleColmNo25).IsUnicode(false);

                entity.Property(e => e.TitleColmNo26).IsUnicode(false);

                entity.Property(e => e.TitleColmNo27).IsUnicode(false);

                entity.Property(e => e.TitleColmNo28).IsUnicode(false);

                entity.Property(e => e.TitleColmNo29).IsUnicode(false);

                entity.Property(e => e.TitleColmNo3).IsUnicode(false);

                entity.Property(e => e.TitleColmNo30).IsUnicode(false);

                entity.Property(e => e.TitleColmNo31).IsUnicode(false);

                entity.Property(e => e.TitleColmNo32).IsUnicode(false);

                entity.Property(e => e.TitleColmNo33).IsUnicode(false);

                entity.Property(e => e.TitleColmNo34).IsUnicode(false);

                entity.Property(e => e.TitleColmNo35).IsUnicode(false);

                entity.Property(e => e.TitleColmNo36).IsUnicode(false);

                entity.Property(e => e.TitleColmNo37).IsUnicode(false);

                entity.Property(e => e.TitleColmNo38).IsUnicode(false);

                entity.Property(e => e.TitleColmNo39).IsUnicode(false);

                entity.Property(e => e.TitleColmNo4).IsUnicode(false);

                entity.Property(e => e.TitleColmNo40).IsUnicode(false);

                entity.Property(e => e.TitleColmNo41).IsUnicode(false);

                entity.Property(e => e.TitleColmNo42).IsUnicode(false);

                entity.Property(e => e.TitleColmNo43).IsUnicode(false);

                entity.Property(e => e.TitleColmNo44).IsUnicode(false);

                entity.Property(e => e.TitleColmNo45).IsUnicode(false);

                entity.Property(e => e.TitleColmNo46).IsUnicode(false);

                entity.Property(e => e.TitleColmNo47).IsUnicode(false);

                entity.Property(e => e.TitleColmNo48).IsUnicode(false);

                entity.Property(e => e.TitleColmNo49).IsUnicode(false);

                entity.Property(e => e.TitleColmNo5).IsUnicode(false);

                entity.Property(e => e.TitleColmNo50).IsUnicode(false);

                entity.Property(e => e.TitleColmNo6).IsUnicode(false);

                entity.Property(e => e.TitleColmNo7).IsUnicode(false);

                entity.Property(e => e.TitleColmNo8).IsUnicode(false);

                entity.Property(e => e.TitleColmNo9).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<RecordSaFormFile>(entity =>
            {
                entity.HasKey(e => e.RecSaFormFileId)
                    .HasName("PK_RECORD_SA_FROM_FILE");

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.FileExt).IsUnicode(false);

                entity.Property(e => e.FileName).IsUnicode(false);

                entity.Property(e => e.FileSize).IsUnicode(false);

                entity.Property(e => e.PhotoFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<RecordStockCard>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<SaleOrder>(entity =>
            {
                entity.Property(e => e.ChannelCode).IsUnicode(false);

                entity.Property(e => e.CompanyCode).IsUnicode(false);

                entity.Property(e => e.ContactPerson).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.CreditStatus).IsUnicode(false);

                entity.Property(e => e.CustCode).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.DivisionCode).IsUnicode(false);

                entity.Property(e => e.DocTypeCode).IsUnicode(false);

                entity.Property(e => e.GroupCode).IsUnicode(false);

                entity.Property(e => e.Incoterm).IsUnicode(false);

                entity.Property(e => e.OrderAction)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OrderStatus)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OrgCode).IsUnicode(false);

                entity.Property(e => e.PaymentTerm).IsUnicode(false);

                entity.Property(e => e.PlantCode).IsUnicode(false);

                entity.Property(e => e.PriceList).IsUnicode(false);

                entity.Property(e => e.QuotationNo).IsUnicode(false);

                entity.Property(e => e.ReasonCode).IsUnicode(false);

                entity.Property(e => e.ReasonReject).IsUnicode(false);

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.Property(e => e.SaleRep).IsUnicode(false);

                entity.Property(e => e.SaleSup).IsUnicode(false);

                entity.Property(e => e.SapMsg).IsUnicode(false);

                entity.Property(e => e.SapOrderNo).IsUnicode(false);

                entity.Property(e => e.SapStatus).IsUnicode(false);

                entity.Property(e => e.ShipCode).IsUnicode(false);

                entity.Property(e => e.ShipToCustCode).IsUnicode(false);

                entity.Property(e => e.SomOrderNo).IsUnicode(false);

                entity.Property(e => e.Territory).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<SaleOrderChangeLog>(entity =>
            {
                entity.Property(e => e.ChangeTabDesc).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.OrderSaleRep).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<SaleOrderLog>(entity =>
            {
                entity.Property(e => e.ChannelCode).IsUnicode(false);

                entity.Property(e => e.CompanyCode).IsUnicode(false);

                entity.Property(e => e.ContactPerson).IsUnicode(false);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.CreditStatus).IsUnicode(false);

                entity.Property(e => e.CustCode).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.DivisionCode).IsUnicode(false);

                entity.Property(e => e.DocTypeCode).IsUnicode(false);

                entity.Property(e => e.GroupCode).IsUnicode(false);

                entity.Property(e => e.Incoterm).IsUnicode(false);

                entity.Property(e => e.OrderAction)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OrderStatus)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OrgCode).IsUnicode(false);

                entity.Property(e => e.PaymentTerm).IsUnicode(false);

                entity.Property(e => e.PlantCode).IsUnicode(false);

                entity.Property(e => e.PriceList).IsUnicode(false);

                entity.Property(e => e.QuotationNo).IsUnicode(false);

                entity.Property(e => e.ReasonCode).IsUnicode(false);

                entity.Property(e => e.ReasonReject).IsUnicode(false);

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.Property(e => e.SaleRep).IsUnicode(false);

                entity.Property(e => e.SaleSup).IsUnicode(false);

                entity.Property(e => e.SapMsg).IsUnicode(false);

                entity.Property(e => e.SapOrderNo).IsUnicode(false);

                entity.Property(e => e.SapStatus).IsUnicode(false);

                entity.Property(e => e.ShipCode).IsUnicode(false);

                entity.Property(e => e.ShipToCustCode).IsUnicode(false);

                entity.Property(e => e.SomOrderNo).IsUnicode(false);

                entity.Property(e => e.Territory).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<SaleOrderProduct>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.ItemType).IsUnicode(false);

                entity.Property(e => e.ProdAltUnit).IsUnicode(false);

                entity.Property(e => e.ProdCateCode).IsUnicode(false);

                entity.Property(e => e.ProdCode).IsUnicode(false);

                entity.Property(e => e.SapItemNo).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<SaleOrderProductLog>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.ItemType).IsUnicode(false);

                entity.Property(e => e.ProdAltUnit).IsUnicode(false);

                entity.Property(e => e.ProdCateCode).IsUnicode(false);

                entity.Property(e => e.ProdCode).IsUnicode(false);

                entity.Property(e => e.SapItemNo).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<SyncDataLog>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.InterfaceId).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<TemplateAppForm>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.PublicFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TpCode).IsUnicode(false);

                entity.Property(e => e.TpNameEn).IsUnicode(false);

                entity.Property(e => e.TpNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);

                entity.Property(e => e.UsedFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<TemplateAppQuestion>(entity =>
            {
                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.RequireFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<TemplateCategory>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.TpCateCode).IsUnicode(false);

                entity.Property(e => e.TpCateNameEn).IsUnicode(false);

                entity.Property(e => e.TpCateNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<TemplateQuestion>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.PublicFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.QuestionCode).IsUnicode(false);

                entity.Property(e => e.QuestionNameEn).IsUnicode(false);

                entity.Property(e => e.QuestionNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<TemplateSaForm>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.TpCode).IsUnicode(false);

                entity.Property(e => e.TpNameEn).IsUnicode(false);

                entity.Property(e => e.TpNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);

                entity.Property(e => e.UsedFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<TemplateSaTitle>(entity =>
            {
                entity.Property(e => e.AnsType)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.TitleNameEn).IsUnicode(false);

                entity.Property(e => e.TitleNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<TemplateStockCard>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.TpCode).IsUnicode(false);

                entity.Property(e => e.TpNameEn).IsUnicode(false);

                entity.Property(e => e.TpNameTh).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.Entity<TemplateStockProduct>(entity =>
            {
                entity.Property(e => e.ActiveFlag)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreateUser).IsUnicode(false);

                entity.Property(e => e.ProdCode).IsUnicode(false);

                entity.Property(e => e.UpdateUser).IsUnicode(false);
            });

            modelBuilder.HasSequence("ADM_GROUP_APP_SEQ")
                .StartsAt(32)
                .HasMin(1);

            modelBuilder.HasSequence("ADM_GROUP_PERM_SEQ")
                .StartsAt(716)
                .HasMin(1);

            modelBuilder.HasSequence("ADM_GROUP_SEQ")
                .StartsAt(156)
                .HasMin(1);

            modelBuilder.HasSequence("ADM_GROUP_USER_SEQ")
                .StartsAt(117)
                .HasMin(1);

            modelBuilder.HasSequence("ADM_LOG_LOGIN_SEQ")
                .StartsAt(10324)
                .HasMin(1);

            modelBuilder.HasSequence("ADM_SESSION_SEQ")
                .StartsAt(10244)
                .HasMin(1);

            modelBuilder.HasSequence("CUSTOMER_COMPANY_SEQ")
                .StartsAt(10001)
                .HasMin(1);

            modelBuilder.HasSequence("CUSTOMER_PARTNER_SEQ")
                .StartsAt(5501)
                .HasMin(1);

            modelBuilder.HasSequence("CUSTOMER_SALE_SEQ")
                .StartsAt(15001)
                .HasMin(1);

            modelBuilder.HasSequence("EMAIL_JOB_SEQ").HasMin(1);

            modelBuilder.HasSequence("hibernate_sequence");

            modelBuilder.HasSequence("IMPORT_ERROR_FILE_LOG_SEQ")
                .StartsAt(10001)
                .HasMin(1);

            modelBuilder.HasSequence("MS_ACTTACH_CATEGORY_SEQ")
                .StartsAt(3)
                .HasMin(1);

            modelBuilder.HasSequence("MS_ATTACH_CATEGORY_SEQ")
                .StartsAt(102)
                .HasMin(1);

            modelBuilder.HasSequence("MS_BANK_SEQ")
                .StartsAt(69)
                .HasMin(1);

            modelBuilder.HasSequence("MS_BRAND_CATEGORY_SEQ")
                .StartsAt(1620)
                .HasMin(1);

            modelBuilder.HasSequence("MS_BRAND_SEQ")
                .StartsAt(131)
                .HasMin(1);

            modelBuilder.HasSequence("MS_GASOLINE_SEQ")
                .StartsAt(90)
                .HasMin(1);

            modelBuilder.HasSequence("MS_LOCATION_SEQ")
                .StartsAt(243)
                .HasMin(1);

            modelBuilder.HasSequence("MS_LOCATION_TYPE_SEQ")
                .StartsAt(1683)
                .HasMin(1);

            modelBuilder.HasSequence("MS_METER_SEQ")
                .StartsAt(487)
                .HasMin(1);

            modelBuilder.HasSequence("MS_ORDER_DOC_TYPE_AREA_SEQ")
                .StartsAt(4734)
                .HasMin(1);

            modelBuilder.HasSequence("MS_PLANT_SHIP_SEQ")
                .StartsAt(257726)
                .HasMin(1);

            modelBuilder.HasSequence("MS_PRODUCT_CONVERSION_SEQ")
                .StartsAt(850)
                .HasMin(1);

            modelBuilder.HasSequence("MS_PRODUCT_PLANT_SEQ")
                .StartsAt(1285)
                .HasMin(1);

            modelBuilder.HasSequence("MS_PRODUCT_SALE_SEQ")
                .StartsAt(86)
                .HasMin(1);

            modelBuilder.HasSequence("MS_REGION_SEQ")
                .StartsAt(79)
                .HasMin(1);

            modelBuilder.HasSequence("MS_SERVICE_TYPE_SEQ")
                .StartsAt(99)
                .HasMin(1);

            modelBuilder.HasSequence("ORG_BUSINESS_UNIT_SEQ")
                .StartsAt(142)
                .HasMin(1);

            modelBuilder.HasSequence("ORG_SALE_AREA_SEQ")
                .StartsAt(592)
                .HasMin(1);

            modelBuilder.HasSequence("ORG_SALE_OFFICE_AREA_SEQ").HasMin(1);

            modelBuilder.HasSequence("ORG_SALE_TERRITORY_SEQ")
                .StartsAt(113)
                .HasMin(1);

            modelBuilder.HasSequence("ORG_TERRITORY_SEQ")
                .StartsAt(143)
                .HasMin(1);

            modelBuilder.HasSequence("PLAN_REASON_NOT_VISIT_SEQ")
                .StartsAt(139)
                .HasMin(1);

            modelBuilder.HasSequence("PLAN_TRIP_PROSPECT_SEQ")
                .StartsAt(618)
                .HasMin(1);

            modelBuilder.HasSequence("PLAN_TRIP_SEQ")
                .StartsAt(248)
                .HasMin(1);

            modelBuilder.HasSequence("PLAN_TRIP_TASK_SEQ")
                .StartsAt(510)
                .HasMin(1);

            modelBuilder.HasSequence("PROSPECT_ACCOUNT_SEQ")
                .StartsAt(817)
                .HasMin(1);

            modelBuilder.HasSequence("PROSPECT_ADDRESS_SEQ")
                .StartsAt(776)
                .HasMin(1);

            modelBuilder.HasSequence("PROSPECT_CONTACT_SEQ")
                .StartsAt(1077)
                .HasMin(1);

            modelBuilder.HasSequence("PROSPECT_DEDICATE_TERT_SEQ")
                .StartsAt(213)
                .HasMin(1);

            modelBuilder.HasSequence("PROSPECT_FEED_SEQ")
                .StartsAt(3236)
                .HasMin(1);

            modelBuilder.HasSequence("PROSPECT_LOG_ERROR_FILE_SEQ").HasMin(1);

            modelBuilder.HasSequence("PROSPECT_RECOMMEND_SEQ")
                .StartsAt(440)
                .HasMin(1);

            modelBuilder.HasSequence("PROSPECT_SEQ")
                .StartsAt(921)
                .HasMin(1);

            modelBuilder.HasSequence("PROSPECT_VISIT_HOUR_SEQ")
                .StartsAt(397)
                .HasMin(1);

            modelBuilder.HasSequence("RECORD_APP_FORM_FILE_SEQ")
                .StartsAt(10020)
                .HasMin(1);

            modelBuilder.HasSequence("RECORD_APP_FORM_SEQ")
                .StartsAt(86)
                .HasMin(1);

            modelBuilder.HasSequence("RECORD_APP_FROM_FILE_SEQ").HasMin(1);

            modelBuilder.HasSequence("RECORD_METER_FILE_SEQ")
                .StartsAt(10001)
                .HasMin(1);

            modelBuilder.HasSequence("RECORD_METER_SEQ")
                .StartsAt(100)
                .HasMin(1);

            modelBuilder.HasSequence("RECORD_SA_FORM_FILE_SEQ")
                .StartsAt(10001)
                .HasMin(1);

            modelBuilder.HasSequence("RECORD_SA_FORM_SEQ")
                .StartsAt(39)
                .HasMin(1);

            modelBuilder.HasSequence("RECORD_STOCK_CARD_SEQ")
                .StartsAt(32)
                .HasMin(1);

            modelBuilder.HasSequence("SALE_ORDER_CHANGE_LOG_SEQ")
                .StartsAt(170)
                .HasMin(1);

            modelBuilder.HasSequence("SALE_ORDER_LOG_SEQ").HasMin(1);

            modelBuilder.HasSequence("SALE_ORDER_PRODUCT_LOG_SEQ").HasMin(1);

            modelBuilder.HasSequence("SALE_ORDER_PRODUCT_SEQ")
                .StartsAt(135)
                .HasMin(1);

            modelBuilder.HasSequence("SALE_ORDER_SEQ")
                .StartsAt(115)
                .HasMin(1);

            modelBuilder.HasSequence("SYNC_DATA_LOG_SEQ")
                .StartsAt(156)
                .HasMin(1);

            modelBuilder.HasSequence("TEMPLATE_APP_FORM_SEQ")
                .StartsAt(147)
                .HasMin(1);

            modelBuilder.HasSequence("TEMPLATE_APP_QUESTION_SEQ")
                .StartsAt(1001)
                .HasMin(1);

            modelBuilder.HasSequence("TEMPLATE_CATEGORY_SEQ")
                .StartsAt(76)
                .HasMin(1);

            modelBuilder.HasSequence("TEMPLATE_QUESTION_SEQ")
                .StartsAt(272)
                .HasMin(1);

            modelBuilder.HasSequence("TEMPLATE_SA_FORM_SEQ")
                .StartsAt(73)
                .HasMin(1);

            modelBuilder.HasSequence("TEMPLATE_SA_TITLE_SEQ")
                .StartsAt(4)
                .HasMin(1);

            modelBuilder.HasSequence("TEMPLATE_STOCK_CARD_SEQ")
                .StartsAt(248)
                .HasMin(1);

            modelBuilder.HasSequence("TEMPLATE_STOCK_PRODUCT_SEQ")
                .StartsAt(172)
                .HasMin(1);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
