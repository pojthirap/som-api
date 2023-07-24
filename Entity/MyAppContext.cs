using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MyFirstAzureWebApp.Entity.custom;

#nullable disable

namespace Entity
{
    public partial class MyAppContext : DbContext
    {
        public MyAppContext()
        {
        }

        public MyAppContext(DbContextOptions<MyAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<SchemaVersion> SchemaVersions { get; set; }
        public virtual DbSet<Table1> Table1s { get; set; }
        public virtual DbSet<Table2> Table2s { get; set; }
        public virtual DbSet<OrgCompany> OrgCompany { get; set; }
        public virtual DbSet<MsRegion> MsRegion { get; set; }
        public virtual DbSet<MsProvince> MsProvince { get; set; }
        public virtual DbSet<OrgSaleOrganization> OrgSaleOrganization { get; set; }
        public virtual DbSet<SaleOrgCustom> SaleOrgCustom { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<MsMeter> MsMeter { get; set; }
        public virtual DbSet<MsGasoline> MsGasoline { get; set; }
        public virtual DbSet<OrgDistChannel> OrgDistChannel { get; set; }
        public virtual DbSet<OrgDivision> OrgDivision { get; set; }
        public virtual DbSet<OrgSaleOffice> OrgSaleOffice { get; set; }
        public virtual DbSet<OrgSaleOfficeArea> OrgSaleOfficeArea { get; set; }
        public virtual DbSet<OrgSaleGroup> OrgSaleGroup { get; set; }
        public virtual DbSet<AdmEmployee> AdmEmployee { get; set; }
        public virtual DbSet<AdmGroupUser> AdmGroupUser { get; set; }
        public virtual DbSet<AdmGroup> AdmGroup { get; set; }
        public virtual DbSet<OrgSaleArea> OrgSaleArea { get; set; }
        public virtual DbSet<OrgBusinessArea> OrgBusinessArea { get; set; }
        public virtual DbSet<OrgBusinessUnit> OrgBusinessUnit { get; set; }
        public virtual DbSet<MsPlantShip> MsPlantShip { get; set; }
        public virtual DbSet<MsPlant> MsPlant { get; set; }
        public virtual DbSet<MsShip> MsShip { get; set; }
        public virtual DbSet<OrgTerritory> OrgTerritory { get; set; }
        public virtual DbSet<OrgSaleTerritory> OrgSaleTerritory { get; set; }
        public virtual DbSet<MsBrand> MsBrand { get; set; }
        public virtual DbSet<MsLocationType> MsLocationType { get; set; }
        public virtual DbSet<MsLocation> MsLocation { get; set; }
        public virtual DbSet<MsBrandCategory> MsBrandCategory { get; set; }
        public virtual DbSet<MsServiceType> MsServiceType { get; set; }
        public virtual DbSet<MsService> MsService { get; set; }
        public virtual DbSet<MsBank> MsBank { get; set; }
        public virtual DbSet<TemplateCategory> TemplateCategory { get; set; }
        public virtual DbSet<TemplateAppForm> TemplateAppForm { get; set; }
        public virtual DbSet<TemplateQuestion> TemplateQuestion { get; set; }
        public virtual DbSet<TemplateAppQuestion> TemplateAppQuestion { get; set; }
        public virtual DbSet<MsAttachCategory> MsAttachCategory { get; set; }
        public virtual DbSet<TemplateStockCard> TemplateStockCard { get; set; }
        public virtual DbSet<TemplateStockProduct> TemplateStockProduct { get; set; }
        public virtual DbSet<MsProduct> MsProduct { get; set; }
        public virtual DbSet<TemplateSaForm> TemplateSaForm { get; set; }
        public virtual DbSet<TemplateSaTitle> TemplateSaTitle { get; set; }
        public virtual DbSet<PlanReasonNotVisit> PlanReasonNotVisit { get; set; }
        public virtual DbSet<MsDistrict> MsDistrict { get; set; }
        public virtual DbSet<MsSubdistrict> MsSubdistrict { get; set; }
        public virtual DbSet<MsConfigLov> MsConfigLov { get; set; }
        public virtual DbSet<MsConfigParam> MsConfigParam { get; set; }
        public virtual DbSet<AdmSession> AdmSession { get; set; }
        public virtual DbSet<AdmLogLogin> AdmLogLogin { get; set; }
        public virtual DbSet<ProspectAddress> ProspectAddress { get; set; }
        public virtual DbSet<Prospect> Prospect { get; set; }
        public virtual DbSet<ProspectAccount> ProspectAccount { get; set; }
        public virtual DbSet<ProspectContact> ProspectContact { get; set; }
        public virtual DbSet<ProspectDedicateTert> ProspectDedicateTert { get; set; }
        public virtual DbSet<ProspectRecommend> ProspectRecommend { get; set; }
        public virtual DbSet<ProspectVisitHour> ProspectVisitHour { get; set; }
        public virtual DbSet<AdmUserAccessToken> AdmUserAccessToken { get; set; }
        public virtual DbSet<AdmPermObject> AdmPermObject { get; set; }
        public virtual DbSet<SaleOrder> SaleOrder { get; set; }
        








        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                var DatabaseConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
                optionsBuilder.UseSqlServer(DatabaseConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasOne(d => d.ReportsToNavigation)
                    .WithMany(p => p.InverseReportsToNavigation)
                    .HasForeignKey(d => d.ReportsTo)
                    .HasConstraintName("FK_Employees_Employees");
            });

            modelBuilder.Entity<Table1>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Table2>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
