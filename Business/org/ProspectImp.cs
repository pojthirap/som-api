using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NLog;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Utils;
using MyFirstAzureWebApp.Entity.custom;
using System.Data.Common;
using System.Data;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.Models.pospect;
using MyFirstAzureWebApp.exception;
using MyFirstAzureWebApp.enumval;

namespace MyFirstAzureWebApp.Business.org
{

    public class ProspectImp : IProspect
    {
        private Logger log = LogManager.GetCurrentClassLogger();




        public async Task<int> updProspectSaTab(UpdateProspectModel updateProspectModel, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        ProspectAccountModel prospectAccountModel = updateProspectModel.ProspectAccountModel;
                        ProspectModel prospectModel = updateProspectModel.ProspectModel;
                        ProspectAddressModel prospectAddressModel = updateProspectModel.ProspectAddressModel;
                        ProspectContactModel prospectContactModel = updateProspectModel.ProspectContactModel;

                        string editFlagStr = "Y";// await getEditGeneralDataFlag(prospectModel);

                        StringBuilder queryBuilder = new StringBuilder();

                        // PROSPECT_FEED
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_FEED_SEQ", p);
                        var nextVal = (int)p.Value;

                        decimal VAL_FUNCTION_TAB = 1;
                        var sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PROSPECT_FEED (FEED_ID, PROSPECT_ID, FUNCTION_TAB, DESCRIPTION, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@FEED_ID ,@PROSPECT_ID, @FUNCTION_TAB, @DESCRIPTION, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("FEED_ID", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", prospectModel.ProspectId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FUNCTION_TAB", VAL_FUNCTION_TAB));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DESCRIPTION", prospectModel.ChangeField));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        // PROSPECT
                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PROSPECT SET SERVICES_TYPE_ID=@ServicesTypeId, LOC_TYPE_ID=@LocTypeId, STATION_NAME=@StationName, STATION_OPEN_FLAG=@StationOpenFlag, REASON_CANCEL=@ReasonCancel, BRAND_CATE_ID=@BrandCateId, BRAND_CATE_OTHER=@BrandCateOther, AREA_SQUARE_WA=@AreaSquareWa, AREA_NGAN=@AreaNgan, AREA_RAI=@AreaRai, AREA_WIDTH_METER=@AreaWidthMeter, SHOP_JOINT=@ShopJoint, LICENSE_STATUS=@LicenseStatus, LICENSE_OTHER=@LicenseOther, INTEREST_STATUS=@InterestStatus, INTEREST_OTHER=@InterestOther, SALE_VOLUME=@SaleVolume, SALE_VOLUME_REF=@SaleVolumeRef, PROGRESS_DATE=@PROGRESS_DATE, TERMINATE_DATE=@TERMINATE_DATE, NEAR_BANK_ID=@NearBankId, QUOTA_OIL=@QuotaOil, QUOTA_LUBE=@QuotaLube, DISPENSER_TOTAL=@DispenserTotal, NOZZLE_TOTAL=@NozzleTotal, ADDR_TITLE_DEED_NO=@AddrTitleDeedNo, ADDR_CERT_UTILISATION=@AddrCertUtilisation, ADDR_PARCEL_NO=@AddrParcelNo, ADDR_TAMBON_NO=@AddrTambonNo, PROSPECT_STATUS=@ProspectStatus, PROSPECT_TYPE=@ProspectType, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME() WHERE PROSPECT_ID=@ProspectId  ");

                        sqlParameters.Add(QueryUtils.addSqlParameter("ServicesTypeId", prospectModel.ServicesTypeId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocTypeId", prospectModel.LocTypeId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("StationName", prospectModel.StationName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("StationOpenFlag", prospectModel.StationOpenFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ReasonCancel", prospectModel.ReasonCancel));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BrandCateId", prospectModel.BrandCateId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BrandCateOther", prospectModel.BrandCateOther));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AreaSquareWa", prospectModel.AreaSquareWa));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AreaNgan", prospectModel.AreaNgan));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AreaRai", prospectModel.AreaRai));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AreaWidthMeter", prospectModel.AreaWidthMeter));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ShopJoint", prospectModel.ShopJoint));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LicenseStatus", prospectModel.LicenseStatus));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LicenseOther", prospectModel.LicenseOther));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("InterestStatus", prospectModel.InterestStatus));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("InterestOther", prospectModel.InterestOther));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SaleVolume", prospectModel.SaleVolume));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SaleVolumeRef", prospectModel.SaleVolumeRef));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("NearBankId", prospectModel.NearBankId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("QuotaOil", prospectModel.QuotaOil));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("QuotaLube", prospectModel.QuotaLube));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DispenserTotal", prospectModel.DispenserTotal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("NozzleTotal", prospectModel.NozzleTotal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AddrTitleDeedNo", prospectModel.AddrTitleDeedNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AddrCertUtilisation", prospectModel.AddrCertUtilisation));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AddrParcelNo", prospectModel.AddrParcelNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AddrTambonNo", prospectModel.AddrTambonNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProspectStatus", prospectModel.ProspectStatus));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProspectType", prospectModel.ProspectType));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", userProfile.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProspectId", prospectModel.ProspectId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROGRESS_DATE", prospectModel.ProgressDate));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TERMINATE_DATE", prospectModel.TerminateDate));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        if ("Y".Equals(editFlagStr))
                        {

                            // PROSPECT_ACCOUNT
                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat(" UPDATE PROSPECT_ACCOUNT SET ACC_NAME=@AccName, BRAND_ID=@BrandId, IDENTIFY_ID=@IdentifyId, ACC_GROUP_REF=@AccGroupRef, REMARK=@Remark, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME() WHERE PROSP_ACC_ID=@ProspAccId ");
                            sqlParameters.Add(QueryUtils.addSqlParameter("AccName", prospectAccountModel.AccName));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("BrandId", prospectAccountModel.BrandId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("IdentifyId", prospectAccountModel.IdentifyId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("AccGroupRef", prospectAccountModel.AccGroupRef));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("Remark", prospectAccountModel.Remark));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("User", userProfile.getUserName()));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("ProspAccId", prospectAccountModel.ProspAccId));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                            // PROSPECT_ADDRESS
                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat("UPDATE PROSPECT_ADDRESS SET ADDR_NO=@AddrNo, MOO=@Moo, SOI=@Soi, STREET=@Street, TELL_NO=@TellNo, FAX_NO=@FaxNo, LATITUDE=@Latitude, LONGITUDE=@Longitude, REGION_CODE=@RegionCode, PROVINCE_CODE=@ProvinceCode, PROVINCE_DBD=@ProvinceDbd, DISTRICT_CODE=@DistrictCode, SUBDISTRICT_CODE=@SubdistrictCode, POST_CODE=@PostCode, REMARK=@Remark, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME() WHERE PROSP_ADDR_ID=@ProspAddrId ");
                            sqlParameters.Add(QueryUtils.addSqlParameter("AddrNo", prospectAddressModel.AddrNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("Moo", prospectAddressModel.Moo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("Soi", prospectAddressModel.Soi));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("Street", prospectAddressModel.Street));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("TellNo", prospectAddressModel.TellNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("FaxNo", prospectAddressModel.FaxNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("Latitude", prospectAddressModel.Latitude));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("Longitude", prospectAddressModel.Longitude));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("RegionCode", prospectAddressModel.RegionCode));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("ProvinceCode", prospectAddressModel.ProvinceCode));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("ProvinceDbd", prospectAddressModel.ProvinceDbd));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DistrictCode", prospectAddressModel.DistrictCode));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("SubdistrictCode", prospectAddressModel.SubdistrictCode));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PostCode", prospectAddressModel.PostCode));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("Remark", prospectAddressModel.Remark));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("User", userProfile.getUserName()));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("ProspAddrId", prospectAddressModel.ProspAddrId));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                            // PROSPECT_CONTACT
                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat("UPDATE PROSPECT_CONTACT SET FIRST_NAME=@FirstName, LAST_NAME=@LastName, PHONE_NO=@PhoneNo, FAX_NO=@FaxNo, MOBILE_NO=@MobileNo, EMAIL=@Email, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME() WHERE PROSP_CONTACT_ID=@ProspContactId ");
                            sqlParameters.Add(QueryUtils.addSqlParameter("FirstName", prospectContactModel.FirstName));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("LastName", prospectContactModel.LastName));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PhoneNo", prospectContactModel.PhoneNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("FaxNo", prospectContactModel.FaxNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("MobileNo", prospectContactModel.MobileNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("Email", prospectContactModel.Email));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("User", userProfile.getUserName()));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("ProspContactId", prospectContactModel.ProspContactId));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);



                        }

                        transaction.Commit();
                        return 1;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }








        public async Task<int> updProspectDbdTab(ProspectModel prospectModel, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        string editFlagStr = await getEditGeneralDataFlagForupdProspectDbdTab(prospectModel);

                        if ("Y".Equals(editFlagStr))
                        {

                            StringBuilder queryBuilder = new StringBuilder();

                            // PROSPECT_FEED
                            var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                            p.Direction = System.Data.ParameterDirection.Output;
                            context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_FEED_SEQ", p);
                            var nextVal = (int)p.Value;

                            decimal VAL_FUNCTION_TAB = 3;
                            var sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat("INSERT INTO PROSPECT_FEED (FEED_ID, PROSPECT_ID, FUNCTION_TAB, DESCRIPTION, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                            queryBuilder.AppendFormat("VALUES(@FEED_ID ,@PROSPECT_ID, @FUNCTION_TAB, @DESCRIPTION, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                            sqlParameters.Add(QueryUtils.addSqlParameter("FEED_ID", nextVal));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", prospectModel.ProspectId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("FUNCTION_TAB", VAL_FUNCTION_TAB));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DESCRIPTION", prospectModel.ChangeField));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);



                            //PROSPECT
                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat("UPDATE PROSPECT SET DBD_CODE=@DbdCode, DBD_CORP_TYPE=@DbdCorpType, DBD_JURISTIC_STATUS=@DbdJuristicStatus, DBD_REG_CAPITAL=@DbdRegCapital, DBD_TOTAL_INCOME=@DbdTotalIncome, DBD_PROFIT_LOSS=@DbdProfitLoss, DBD_TOTAL_ASSET=@DbdTotalAsset, DBD_FLEET_CARD=@DbdFleetCard, DBD_CORP_CARD=@DbdCorpCard, DBD_OIL_CONSUPTION=@DbdOilConsuption, DBD_CURRENT_STATION=@DbdCurrentStation, DBD_PAY_CHANNEL=@DbdPayChannel, DBD_CAR_WHEEL4=@DbdCarWheel4, DBD_CAR_WHEEL6=@DbdCarWheel6, DBD_CAR_WHEEL8=@DbdCarWheel8, DBD_CARAVAN=@DbdCaravan, DBD_CAR_TRAILER=@DbdCarTrailer, DBD_CAR_CONTAINER=@DbdCarContainer, DBD_MACHINE=@DbdMachine, DBD_OTHER=@DbdOther, DBD_TANK=@DbdTank, DBD_STATION=@DbdStation, DBD_TYPE2=@DbdType2, DBD_MAINTAIN_CENTER=@DbdMaintainCenter, DBD_GENERAL_GARAGE=@DbdGeneralGarage, DBD_MAINTAIN_DEPT=@DbdMaintainDept, DBD_RECOMMENDER=@DbdRecommender, DBD_SALE=@DbdSale, DBD_SALE_SUPPORT=@DbdSaleSupport, DBD_REMARK=@DbdRemark, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME() WHERE PROSPECT_ID=@ProspectId  ");
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdCode", prospectModel.DbdCode));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdCorpType", prospectModel.DbdCorpType));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdJuristicStatus", prospectModel.DbdJuristicStatus));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdRegCapital", prospectModel.DbdRegCapital));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdTotalIncome", prospectModel.DbdTotalIncome));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdProfitLoss", prospectModel.DbdProfitLoss));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdTotalAsset", prospectModel.DbdTotalAsset));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdFleetCard", prospectModel.DbdFleetCard));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdCorpCard", prospectModel.DbdCorpCard));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdOilConsuption", prospectModel.DbdOilConsuption));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdCurrentStation", prospectModel.DbdCurrentStation));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdPayChannel", prospectModel.DbdPayChannel));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdCarWheel4", prospectModel.DbdCarWheel4));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdCarWheel6", prospectModel.DbdCarWheel6));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdCarWheel8", prospectModel.DbdCarWheel8));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdCaravan", prospectModel.DbdCaravan));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdCarTrailer", prospectModel.DbdCarTrailer));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdCarContainer", prospectModel.DbdCarContainer));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdMachine", prospectModel.DbdMachine));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdOther", prospectModel.DbdOther));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdTank", prospectModel.DbdTank));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdStation", prospectModel.DbdStation));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdType2", prospectModel.DbdType2));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdMaintainCenter", prospectModel.DbdMaintainCenter));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdGeneralGarage", prospectModel.DbdGeneralGarage));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdMaintainDept", prospectModel.DbdMaintainDept));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdRecommender", prospectModel.DbdRecommender));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdSale", prospectModel.DbdSale));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdSaleSupport", prospectModel.DbdSaleSupport));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DbdRemark", prospectModel.DbdRemark));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("User", userProfile.getUserName()));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("ProspectId", prospectModel.ProspectId));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        }

                        transaction.Commit();
                        return 1;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }


        public async Task<String> getEditGeneralDataFlagForupdProspectDbdTab(ProspectModel prospectModel)
        {
            StringBuilder queryBuilder = new StringBuilder();
            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                queryBuilder.AppendFormat(" select IIF(PP.MAIN_FLAG = 'Y','Y','N') EDIT_GENERAL_DATA_FLAG from PROSPECT PP where PP.PROSPECT_ID = @ProspectId ");
                QueryUtils.addParam(command, "ProspectId", prospectModel.ProspectId);// Add new

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "EDIT_GENERAL_DATA_FLAG");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }





        public async Task<int> updProspectBasicTab(UpdateProspectModel updateProspectModel, UserProfileForBack userProfile, string language)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        ProspectAccountModel prospectAccountModel = updateProspectModel.ProspectAccountModel;
                        ProspectModel prospectModel = updateProspectModel.ProspectModel;
                        ProspectAddressModel prospectAddressModel = updateProspectModel.ProspectAddressModel;
                        ProspectContactModel prospectContactModel = updateProspectModel.ProspectContactModel;

                        string foundDataFlagStr = await getFoundDataFlagForUpdProspectBasicTab(prospectModel, prospectAccountModel, prospectAddressModel);
                        if ("Y".Equals(foundDataFlagStr))
                        {
                            ServiceException se = new ServiceException("E_0002", ObjectFacory.getCultureInfo(language));
                            throw se;
                        }

                        StringBuilder queryBuilder = new StringBuilder();


                        // PROSPECT_FEED
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_FEED_SEQ", p);
                        var nextVal = (int)p.Value;

                        decimal VAL_FUNCTION_TAB = 4;
                        var sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PROSPECT_FEED (FEED_ID, PROSPECT_ID, FUNCTION_TAB, DESCRIPTION, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@FEED_ID ,@PROSPECT_ID, @FUNCTION_TAB, @DESCRIPTION, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("FEED_ID", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", prospectModel.ProspectId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FUNCTION_TAB", VAL_FUNCTION_TAB));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DESCRIPTION", prospectModel.ChangeField));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        // PROSPECT_ACCOUNT
                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PROSPECT_ACCOUNT SET ACC_NAME=@AccName, BRAND_ID=@BrandId, IDENTIFY_ID=@IdentifyId, ACC_GROUP_REF=@AccGroupRef, REMARK=@Remark, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME() WHERE PROSP_ACC_ID=@ProspAccId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("AccName", prospectAccountModel.AccName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BrandId", prospectAccountModel.BrandId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("IdentifyId", prospectAccountModel.IdentifyId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AccGroupRef", prospectAccountModel.AccGroupRef));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Remark", prospectAccountModel.Remark));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", userProfile.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProspAccId", prospectAccountModel.ProspAccId));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        // PROSPECT
                        queryBuilder = new StringBuilder();
                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder.AppendFormat("UPDATE PROSPECT SET PROSPECT_TYPE=@PROSPECT_TYPE, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME() WHERE PROSPECT_ID=@PROSPECT_ID  ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_TYPE", prospectModel.ProspectType));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", userProfile.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", prospectModel.ProspectId));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        // PROSPECT_ADDRESS
                        queryBuilder = new StringBuilder();
                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder.AppendFormat("UPDATE PROSPECT_ADDRESS SET ADDR_NO=@AddrNo, MOO=@Moo, SOI=@Soi, STREET=@Street, TELL_NO=@TellNo, FAX_NO=@FaxNo, LATITUDE=@Latitude, LONGITUDE=@Longitude, REGION_CODE=@RegionCode, PROVINCE_CODE=@ProvinceCode, PROVINCE_DBD=@ProvinceDbd, DISTRICT_CODE=@DistrictCode, SUBDISTRICT_CODE=@SubdistrictCode, POST_CODE=@PostCode, REMARK=@Remark, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME() WHERE PROSP_ADDR_ID=@ProspAddrId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("AddrNo", prospectAddressModel.AddrNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Moo", prospectAddressModel.Moo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Soi", prospectAddressModel.Soi));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Street", prospectAddressModel.Street));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TellNo", prospectAddressModel.TellNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FaxNo", prospectAddressModel.FaxNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Latitude", prospectAddressModel.Latitude));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Longitude", prospectAddressModel.Longitude));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("RegionCode", prospectAddressModel.RegionCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProvinceCode", prospectAddressModel.ProvinceCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProvinceDbd", prospectAddressModel.ProvinceDbd));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DistrictCode", prospectAddressModel.DistrictCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SubdistrictCode", prospectAddressModel.SubdistrictCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PostCode", prospectAddressModel.PostCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Remark", prospectAddressModel.Remark));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", userProfile.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProspAddrId", prospectAddressModel.ProspAddrId));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        // PROSPECT_CONTACT
                        queryBuilder = new StringBuilder();
                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder.AppendFormat("UPDATE PROSPECT_CONTACT SET FIRST_NAME=@FirstName, LAST_NAME=@LastName, PHONE_NO=@PhoneNo, FAX_NO=@FaxNo, MOBILE_NO=@MobileNo, EMAIL=@Email, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME() WHERE PROSP_CONTACT_ID=@ProspContactId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("FirstName", prospectContactModel.FirstName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LastName", prospectContactModel.LastName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PhoneNo", prospectContactModel.PhoneNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FaxNo", prospectContactModel.FaxNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("MobileNo", prospectContactModel.MobileNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Email", prospectContactModel.Email));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", userProfile.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProspContactId", prospectContactModel.ProspContactId));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        transaction.Commit();
                        return 1;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }


        public async Task<String> getFoundDataFlagForUpdProspectBasicTab(ProspectModel prospectModel, ProspectAccountModel prospectAccountModel, ProspectAddressModel prospectAddressModel)
        {
            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select IIF(PP.PROSPECT_TYPE=2,'N','Y') FOUND_DATA_FLAG ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT AC ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PA on PA.PROSP_ACC_ID = AC.PROSP_ACC_ID and PA.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = AC.PROSP_ACC_ID ");
                queryBuilder.AppendFormat(" where (IIF(AC.IDENTIFY_ID = '',null,AC.IDENTIFY_ID) = @IDENTIFY_ID or (AC.ACC_NAME = @ACC_NAME and PA.SUBDISTRICT_CODE = @SUBDISTRICT_CODE)) ");
                queryBuilder.AppendFormat(" and PP.PROSPECT_ID != @PROSPECT_ID ");

                QueryUtils.addParam(command, "PROSPECT_ID", prospectModel.ProspectId);// Add new
                QueryUtils.addParam(command, "IDENTIFY_ID", prospectAccountModel.IdentifyId);// Add new
                QueryUtils.addParam(command, "ACC_NAME", prospectAccountModel.AccName);// Add new
                QueryUtils.addParam(command, "SUBDISTRICT_CODE", prospectAddressModel.SubdistrictCode);// Add new

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "FOUND_DATA_FLAG");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }







        public async Task<Prospect> cloneProspect(CloneProspectModel cloneProspectModel, UserProfileForBack userProfile, string language)
        {
            GetProspectAccountForCloneProspectCustom o = await getProspectAccountForCloneProspect(cloneProspectModel.ProspectId, userProfile);
            if (o==null)
            {
                ServiceException se = new ServiceException("R_CANNOT_CLONE_PROSPECT", ObjectFacory.getCultureInfo(language));
                throw se;
            }
            if (!String.IsNullOrEmpty(o.BuId))
            {
                List<String> errorParam = new List<string>();
                errorParam.Add(o.TerritoryName);
                ServiceException se = new ServiceException("W_0012", errorParam, ObjectFacory.getCultureInfo(language));
                throw se;
            }

            string ProspAccId = o.ProspAccId;

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {


                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_SEQ", p);

                        var VAL_PROSPECT_ID = (int)p.Value;
                        var VAL_BU_ID = userProfile.getBuId();
                        var VAL_SALE_REP_ID = userProfile.getEmpId();
                        var VAL_GROUP_CODE = userProfile.getAdmEmployeeGroupCode();
                        var VAL_PROSPECT_TYPE = "0";
                        var VAL_PROSPECT_STATUS = "0";
                        var VAL_MAIN_FLAG = "N";
                        var VAL_ACTIVE = "Y";


                        // PROSPECT_FEED
                        decimal VAL_FUNCTION_TAB = 1;
                        var sqlParameters = new List<SqlParameter>();
                        p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_FEED_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PROSPECT_FEED (FEED_ID, PROSPECT_ID, FUNCTION_TAB, DESCRIPTION, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@FEED_ID ,@PROSPECT_ID, @FUNCTION_TAB, @DESCRIPTION, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("FEED_ID", nextVal));
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", VAL_PROSPECT_ID));
                        sqlParameters.Add(QueryUtils.addSqlParameter("FUNCTION_TAB", VAL_FUNCTION_TAB));
                        sqlParameters.Add(QueryUtils.addSqlParameter("DESCRIPTION", "Clone Prospect"));
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        //PROSPECT
                        sqlParameters = new List<SqlParameter>();
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PROSPECT (PROSPECT_ID, PROSP_ACC_ID, GROUP_CODE, BU_ID, SALE_REP_ID, PROSPECT_TYPE, MAIN_FLAG, PROSPECT_STATUS, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@PROSPECT_ID ,@PROSP_ACC_ID, @GROUP_CODE, @BU_ID, @SALE_REP_ID, @PROSPECT_TYPE, @MAIN_FLAG, @PROSPECT_STATUS, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", VAL_PROSPECT_ID));
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_ACC_ID", ProspAccId));
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_CODE", VAL_GROUP_CODE));
                        sqlParameters.Add(QueryUtils.addSqlParameter("BU_ID", VAL_BU_ID));
                        sqlParameters.Add(QueryUtils.addSqlParameter("SALE_REP_ID", VAL_SALE_REP_ID));
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_TYPE", VAL_PROSPECT_TYPE));
                        sqlParameters.Add(QueryUtils.addSqlParameter("MAIN_FLAG", VAL_MAIN_FLAG));
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_STATUS", VAL_PROSPECT_STATUS));
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        var VAL_XXX = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());

                        string aa= FunctionTab.FUNCTION_TAB_SA.ToString();

                        foreach (string funcStr in cloneProspectModel.FunctionTab)
                        {
                            int func = int.Parse(funcStr);
                            switch (func)
                            {
                                case (int)FunctionTab.FUNCTION_TAB_SA:
                                    //PROSPECT_FEED
                                    VAL_FUNCTION_TAB = 1;
                                    p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                                    p.Direction = System.Data.ParameterDirection.Output;
                                    context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_FEED_SEQ", p);
                                    nextVal = (int)p.Value;
                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" INSERT INTO PROSPECT_FEED (FEED_ID, PROSPECT_ID, FUNCTION_TAB, DESCRIPTION, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                                    queryBuilder.AppendFormat("VALUES(@FEED_ID ,@PROSPECT_ID, @FUNCTION_TAB, @DESCRIPTION, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("FEED_ID", nextVal));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", VAL_PROSPECT_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("FUNCTION_TAB", VAL_FUNCTION_TAB));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("DESCRIPTION", "Clone Prospect - SA"));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                    //UPDATE TU

                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" UPDATE TU ");
                                    queryBuilder.AppendFormat("     SET TU.SERVICES_TYPE_ID=TJ.SERVICES_TYPE_ID, TU.LOC_TYPE_ID=TJ.LOC_TYPE_ID, TU.STATION_NAME=TJ.STATION_NAME,  ");
                                    queryBuilder.AppendFormat("     TU.STATION_OPEN_FLAG=TJ.STATION_OPEN_FLAG, TU.REASON_CANCEL=TJ.REASON_CANCEL, TU.BRAND_CATE_ID=TJ.BRAND_CATE_ID,  ");
                                    queryBuilder.AppendFormat("     TU.BRAND_CATE_OTHER=TJ.BRAND_CATE_OTHER, TU.AREA_SQUARE_WA=TJ.AREA_SQUARE_WA, TU.AREA_NGAN=TJ.AREA_NGAN,  ");
                                    queryBuilder.AppendFormat("     TU.AREA_RAI=TJ.AREA_RAI, TU.AREA_WIDTH_METER=TJ.AREA_WIDTH_METER, TU.SHOP_JOINT=TJ.SHOP_JOINT,  ");
                                    queryBuilder.AppendFormat("     TU.LICENSE_STATUS=TJ.LICENSE_STATUS, TU.LICENSE_OTHER=TJ.LICENSE_OTHER, TU.INTEREST_STATUS=TJ.INTEREST_STATUS,  ");
                                    queryBuilder.AppendFormat("     TU.INTEREST_OTHER=TJ.INTEREST_OTHER, TU.SALE_VOLUME=TJ.SALE_VOLUME, TU.SALE_VOLUME_REF=TJ.SALE_VOLUME_REF,  ");
                                    queryBuilder.AppendFormat("     TU.PROGRESS_DATE=TJ.PROGRESS_DATE, TU.TERMINATE_DATE=TJ.TERMINATE_DATE, TU.NEAR_BANK_ID=TJ.NEAR_BANK_ID, TU.QUOTA_OIL=TJ.QUOTA_OIL,  ");
                                    queryBuilder.AppendFormat("     TU.QUOTA_LUBE=TJ.QUOTA_LUBE, TU.DISPENSER_TOTAL=TJ.DISPENSER_TOTAL, TU.NOZZLE_TOTAL=TJ.NOZZLE_TOTAL,  ");
                                    queryBuilder.AppendFormat("     TU.ADDR_TITLE_DEED_NO=TJ.ADDR_TITLE_DEED_NO, TU.ADDR_CERT_UTILISATION=TJ.ADDR_CERT_UTILISATION,  ");
                                    queryBuilder.AppendFormat("     TU.ADDR_PARCEL_NO=TJ.ADDR_PARCEL_NO, TU.ADDR_TAMBON_NO=TJ.ADDR_TAMBON_NO ");
                                    queryBuilder.AppendFormat(" FROM PROSPECT TU ");
                                    queryBuilder.AppendFormat(" INNER JOIN( ");
                                    queryBuilder.AppendFormat("     SELECT @VAL_PROSPECT_ID PROSPECT_ID,[SERVICES_TYPE_ID],[LOC_TYPE_ID],[STATION_NAME],[STATION_OPEN_FLAG],[REASON_CANCEL],[BRAND_CATE_ID], ");
                                    queryBuilder.AppendFormat("     [BRAND_CATE_OTHER],[AREA_SQUARE_WA],[AREA_NGAN],[AREA_RAI],[AREA_WIDTH_METER],[SHOP_JOINT],[LICENSE_STATUS], ");
                                    queryBuilder.AppendFormat("     [LICENSE_OTHER],[INTEREST_STATUS],[INTEREST_OTHER],[SALE_VOLUME],[SALE_VOLUME_REF],[PROGRESS_DATE], ");
                                    queryBuilder.AppendFormat("     [TERMINATE_DATE],[NEAR_BANK_ID],[QUOTA_OIL],[QUOTA_LUBE],[DISPENSER_TOTAL],[NOZZLE_TOTAL],[ADDR_TITLE_DEED_NO], ");
                                    queryBuilder.AppendFormat("     [ADDR_CERT_UTILISATION],[ADDR_PARCEL_NO],[ADDR_TAMBON_NO] ");
                                    queryBuilder.AppendFormat("     FROM PROSPECT ");
                                    queryBuilder.AppendFormat("     WHERE PROSPECT_ID = @PROSPECT_ID ");
                                    queryBuilder.AppendFormat("     ) TJ ON TJ.PROSPECT_ID = TU.PROSPECT_ID  ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PROSPECT_ID", VAL_PROSPECT_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", cloneProspectModel.ProspectId));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                                    break;
                                case (int)FunctionTab.FUNCTION_TAB_FEED:

                                    //PROSPECT_FEED
                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" INSERT INTO PROSPECT_FEED ([FEED_ID], [PROSPECT_ID], [FUNCTION_TAB], [DESCRIPTION], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                                    queryBuilder.AppendFormat(" SELECT NEXT VALUE FOR PROSPECT_FEED_SEQ, @VAL_PROSPECT_ID , [FUNCTION_TAB], [DESCRIPTION], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM] FROM PROSPECT_FEED WHERE PROSPECT_ID = @PROSPECT_ID ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PROSPECT_ID", VAL_PROSPECT_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", cloneProspectModel.ProspectId));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                    break;
                                case (int)FunctionTab.FUNCTION_TAB_SALE_TERRITORY:
                                    
                                    break;
                                case (int)FunctionTab.FUNCTION_TAB_ADDRESS:
                                    //PROSPECT_ADDRESS
                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" INSERT INTO PROSPECT_ADDRESS ([PROSP_ADDR_ID], [PROSPECT_ID], [PROSP_ACC_ID], [ADDR_NO], [MOO], [SOI], [STREET], [TELL_NO], [FAX_NO], [LATITUDE], [LONGITUDE], [REGION_CODE], [PROVINCE_CODE], [PROVINCE_DBD], [DISTRICT_CODE], [SUBDISTRICT_CODE], [POST_CODE], [REMARK], [MAIN_FLAG], [ACTIVE_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                                    queryBuilder.AppendFormat(" SELECT NEXT VALUE FOR PROSPECT_ADDRESS_SEQ, @VAL_PROSPECT_ID, [PROSP_ACC_ID], [ADDR_NO], [MOO], [SOI], [STREET], [TELL_NO], [FAX_NO], [LATITUDE], [LONGITUDE], [REGION_CODE], [PROVINCE_CODE], [PROVINCE_DBD], [DISTRICT_CODE], [SUBDISTRICT_CODE], [POST_CODE], [REMARK], [MAIN_FLAG], [ACTIVE_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM] FROM PROSPECT_ADDRESS WHERE MAIN_FLAG != 'Y' AND PROSPECT_ID = @PROSPECT_ID ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PROSPECT_ID", VAL_PROSPECT_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", cloneProspectModel.ProspectId));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                    break;
                                case (int)FunctionTab.FUNCTION_TAB_CONTACT:
                                    VAL_FUNCTION_TAB = 2;
                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" INSERT INTO PROSPECT_FEED ([FEED_ID], [PROSPECT_ID], [FUNCTION_TAB], [DESCRIPTION], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                                    queryBuilder.AppendFormat(" VALUES(NEXT VALUE FOR PROSPECT_FEED_SEQ, @PROSPECT_ID, @VAL_FUNCTION_TAB, 'Clone Prospect - Contact', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME()) ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", cloneProspectModel.ProspectId));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_FUNCTION_TAB", VAL_FUNCTION_TAB));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getEmpId()));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                                    //PROSPECT_CONTACT
                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" INSERT INTO PROSPECT_CONTACT ([PROSP_CONTACT_ID], [PROSPECT_ID], [PROSP_ACC_ID], [FIRST_NAME], [LAST_NAME], [PHONE_NO], [FAX_NO], [MOBILE_NO], [EMAIL], [MAIN_FLAG], [ACTIVE_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                                    queryBuilder.AppendFormat(" SELECT NEXT VALUE FOR PROSPECT_CONTACT_SEQ, @VAL_PROSPECT_ID, [PROSP_ACC_ID], [FIRST_NAME], [LAST_NAME], [PHONE_NO], [FAX_NO], [MOBILE_NO], [EMAIL], [MAIN_FLAG], [ACTIVE_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM] FROM PROSPECT_CONTACT WHERE  MAIN_FLAG != 'Y' AND PROSPECT_ID = @PROSPECT_ID  ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PROSPECT_ID", VAL_PROSPECT_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", cloneProspectModel.ProspectId));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                    break;
                                case (int)FunctionTab.FUNCTION_TAB_ATTACHMENT:
                                    
                                    break;
                                case (int)FunctionTab.FUNCTION_TAB_VISITING_HOUR:
                                    //PROSPECT_VISIT_HOUR
                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" INSERT INTO PROSPECT_VISIT_HOUR ([PROSP_VISIT_HR_ID], [PROSPECT_ID], [DAYS_CODE], [HOUR_START], [HOUR_END], [ACTIVE_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM])  ");
                                    queryBuilder.AppendFormat(" SELECT NEXT VALUE FOR PROSPECT_VISIT_HOUR_SEQ, @VAL_PROSPECT_ID, [DAYS_CODE], [HOUR_START], [HOUR_END], [ACTIVE_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM] FROM PROSPECT_VISIT_HOUR WHERE PROSPECT_ID = @PROSPECT_ID  ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PROSPECT_ID", VAL_PROSPECT_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", cloneProspectModel.ProspectId));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                    break;
                                case (int)FunctionTab.FUNCTION_TAB_SURVEY_RESULT:
                                    //RECORD_APP_FORM
                                    p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                                    p.Direction = System.Data.ParameterDirection.Output;
                                    context.Database.ExecuteSqlRaw("set @result = next value for RECORD_APP_FORM_SEQ", p);
                                    var VAL_REC_APP_FORM_ID = (int)p.Value;

                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" INSERT INTO RECORD_APP_FORM ([REC_APP_FORM_ID], [TP_APP_FORM_ID], [PROSP_ID], [APP_FORM], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM])  ");
                                    queryBuilder.AppendFormat(" SELECT @VAL_REC_APP_FORM_ID, [TP_APP_FORM_ID], @VAL_PROSPECT_ID , [APP_FORM], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM] FROM RECORD_APP_FORM WHERE PROSP_ID = @PROSPECT_ID  ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_REC_APP_FORM_ID", VAL_REC_APP_FORM_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PROSPECT_ID", VAL_PROSPECT_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", cloneProspectModel.ProspectId));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" INSERT INTO RECORD_APP_FORM_FILE ([REC_APP_FORM_FILE_ID], [FILE_ID], [REC_APP_FORM_ID], [ATTACH_CATE_ID], [FILE_NAME], [FILE_EXT], [FILE_SIZE], [PHOTO_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                                    queryBuilder.AppendFormat(" SELECT NEXT VALUE FOR RECORD_APP_FORM_FILE_SEQ, [FILE_ID], @VAL_REC_APP_FORM_ID, [ATTACH_CATE_ID], [FILE_NAME], [FILE_EXT], [FILE_SIZE], [PHOTO_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]  ");
                                    queryBuilder.AppendFormat(" FROM RECORD_APP_FORM_FILE FF ");
                                    queryBuilder.AppendFormat(" WHERE EXISTS (SELECT 1 FROM RECORD_APP_FORM AF WHERE AF.REC_APP_FORM_ID = FF.REC_APP_FORM_ID AND AF.PROSP_ID = @PROSPECT_ID) ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_REC_APP_FORM_ID", VAL_REC_APP_FORM_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", cloneProspectModel.ProspectId));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                                    break;
                                case (int)FunctionTab.FUNCTION_TAB_RECOMMMEND_BU:
                                    //PROSPECT_RECOMMEND
                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" INSERT INTO PROSPECT_RECOMMEND ([PROSP_RECOMM_ID], [PROSPECT_ID], [BU_ID], [ACTIVE_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM])  ");
                                    queryBuilder.AppendFormat(" SELECT NEXT VALUE FOR PROSPECT_RECOMMEND_SEQ, @VAL_PROSPECT_ID, [BU_ID], [ACTIVE_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM] FROM PROSPECT_RECOMMEND WHERE PROSPECT_ID = @PROSPECT_ID  ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PROSPECT_ID", VAL_PROSPECT_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", cloneProspectModel.ProspectId));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                    break;
                                case (int)FunctionTab.FUNCTION_TAB_TEMPLATE_FOR_SA:
                                    //RECORD_SA_FORM

                                    p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                                    p.Direction = System.Data.ParameterDirection.Output;
                                    context.Database.ExecuteSqlRaw("set @result = next value for RECORD_SA_FORM_SEQ", p);
                                    var VAL_REC_SA_FORM_ID = (int)p.Value;

                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" INSERT INTO RECORD_SA_FORM ([REC_SA_FORM_ID], [TP_SA_FORM_ID], [PROSP_ID], [TITLE_COLM_NO1], [TITLE_COLM_NO2], [TITLE_COLM_NO3], [TITLE_COLM_NO4], [TITLE_COLM_NO5], [TITLE_COLM_NO6], [TITLE_COLM_NO7], [TITLE_COLM_NO8], [TITLE_COLM_NO9], [TITLE_COLM_NO10], [TITLE_COLM_NO11], [TITLE_COLM_NO12], [TITLE_COLM_NO13], [TITLE_COLM_NO14], [TITLE_COLM_NO15], [TITLE_COLM_NO16], [TITLE_COLM_NO17], [TITLE_COLM_NO18], [TITLE_COLM_NO19], [TITLE_COLM_NO20], [TITLE_COLM_NO21], [TITLE_COLM_NO22], [TITLE_COLM_NO23], [TITLE_COLM_NO24], [TITLE_COLM_NO25], [TITLE_COLM_NO26], [TITLE_COLM_NO27], [TITLE_COLM_NO28], [TITLE_COLM_NO29], [TITLE_COLM_NO30], [TITLE_COLM_NO31], [TITLE_COLM_NO32], [TITLE_COLM_NO33], [TITLE_COLM_NO34], [TITLE_COLM_NO35], [TITLE_COLM_NO36], [TITLE_COLM_NO37], [TITLE_COLM_NO38], [TITLE_COLM_NO39], [TITLE_COLM_NO40], [TITLE_COLM_NO41], [TITLE_COLM_NO42], [TITLE_COLM_NO43], [TITLE_COLM_NO44], [TITLE_COLM_NO45], [TITLE_COLM_NO46], [TITLE_COLM_NO47], [TITLE_COLM_NO48], [TITLE_COLM_NO49], [TITLE_COLM_NO50], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM])  ");
                                    queryBuilder.AppendFormat(" SELECT @VAL_REC_SA_FORM_ID, [TP_SA_FORM_ID], @VAL_PROSPECT_ID, [TITLE_COLM_NO1], [TITLE_COLM_NO2], [TITLE_COLM_NO3], [TITLE_COLM_NO4], [TITLE_COLM_NO5], [TITLE_COLM_NO6], [TITLE_COLM_NO7], [TITLE_COLM_NO8], [TITLE_COLM_NO9], [TITLE_COLM_NO10], [TITLE_COLM_NO11], [TITLE_COLM_NO12], [TITLE_COLM_NO13], [TITLE_COLM_NO14], [TITLE_COLM_NO15], [TITLE_COLM_NO16], [TITLE_COLM_NO17], [TITLE_COLM_NO18], [TITLE_COLM_NO19], [TITLE_COLM_NO20], [TITLE_COLM_NO21], [TITLE_COLM_NO22], [TITLE_COLM_NO23], [TITLE_COLM_NO24], [TITLE_COLM_NO25], [TITLE_COLM_NO26], [TITLE_COLM_NO27], [TITLE_COLM_NO28], [TITLE_COLM_NO29], [TITLE_COLM_NO30], [TITLE_COLM_NO31], [TITLE_COLM_NO32], [TITLE_COLM_NO33], [TITLE_COLM_NO34], [TITLE_COLM_NO35], [TITLE_COLM_NO36], [TITLE_COLM_NO37], [TITLE_COLM_NO38], [TITLE_COLM_NO39], [TITLE_COLM_NO40], [TITLE_COLM_NO41], [TITLE_COLM_NO42], [TITLE_COLM_NO43], [TITLE_COLM_NO44], [TITLE_COLM_NO45], [TITLE_COLM_NO46], [TITLE_COLM_NO47], [TITLE_COLM_NO48], [TITLE_COLM_NO49], [TITLE_COLM_NO50], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM] FROM RECORD_SA_FORM WHERE PROSP_ID = @PROSPECT_ID  ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_REC_SA_FORM_ID", VAL_REC_SA_FORM_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PROSPECT_ID", VAL_PROSPECT_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", cloneProspectModel.ProspectId));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" INSERT INTO [dbo].[RECORD_SA_FORM_FILE]([REC_SA_FORM_FILE_ID], [FILE_ID], [REC_SA_FORM_ID], [ATTACH_CATE_ID], [FILE_NAME], [FILE_EXT], [FILE_SIZE], [PHOTO_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM])  ");
                                    queryBuilder.AppendFormat(" SELECT NEXT VALUE FOR RECORD_SA_FORM_FILE_SEQ, [FILE_ID], @VAL_REC_SA_FORM_ID, [ATTACH_CATE_ID], [FILE_NAME], [FILE_EXT], [FILE_SIZE], [PHOTO_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]   ");
                                    queryBuilder.AppendFormat(" FROM RECORD_SA_FORM_FILE FF  ");
                                    queryBuilder.AppendFormat(" WHERE EXISTS (SELECT 1 FROM RECORD_SA_FORM AF WHERE AF.REC_SA_FORM_ID = FF.REC_SA_FORM_ID AND AF.PROSP_ID =  @PROSPECT_ID ) ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_REC_SA_FORM_ID", VAL_REC_SA_FORM_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", cloneProspectModel.ProspectId));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                                    break;
                                case (int)FunctionTab.FUNCTION_TAB_DBD:

                                    VAL_FUNCTION_TAB = 3;
                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" INSERT INTO PROSPECT_FEED ([FEED_ID], [PROSPECT_ID], [FUNCTION_TAB], [DESCRIPTION], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                                    queryBuilder.AppendFormat(" VALUES(NEXT VALUE FOR PROSPECT_FEED_SEQ, @PROSPECT_ID , @VAL_FUNCTION_TAB, 'Clone Prospect - DBD', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME()) ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", cloneProspectModel.ProspectId));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_FUNCTION_TAB", VAL_FUNCTION_TAB));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getEmpId()));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                                    //PROSPECT_CONTACT
                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();

                                    queryBuilder.AppendFormat(" UPDATE TU  ");
                                    queryBuilder.AppendFormat("     SET TU.DBD_CODE=TJ.DBD_CODE, TU.DBD_CORP_TYPE=TJ.DBD_CORP_TYPE, TU.DBD_JURISTIC_STATUS=TJ.DBD_JURISTIC_STATUS,   ");
                                    queryBuilder.AppendFormat("     TU.DBD_REG_CAPITAL=TJ.DBD_REG_CAPITAL,TU.DBD_TOTAL_INCOME=TJ.DBD_TOTAL_INCOME, TU.DBD_PROFIT_LOSS=TJ.DBD_PROFIT_LOSS,   ");
                                    queryBuilder.AppendFormat("     TU.DBD_TOTAL_ASSET=TJ.DBD_TOTAL_ASSET, TU.DBD_FLEET_CARD=TJ.DBD_FLEET_CARD, TU.DBD_CORP_CARD=TJ.DBD_CORP_CARD,   ");
                                    queryBuilder.AppendFormat("     TU.DBD_OIL_CONSUPTION=TJ.DBD_OIL_CONSUPTION, TU.DBD_CURRENT_STATION=TJ.DBD_CURRENT_STATION,   ");
                                    queryBuilder.AppendFormat("     TU.DBD_PAY_CHANNEL=TJ.DBD_PAY_CHANNEL, TU.DBD_CAR_WHEEL4=TJ.DBD_CAR_WHEEL4, TU.DBD_CAR_WHEEL6=TJ.DBD_CAR_WHEEL6,   ");
                                    queryBuilder.AppendFormat("     TU.DBD_CAR_WHEEL8=TJ.DBD_CAR_WHEEL8, TU.DBD_CARAVAN=TJ.DBD_CARAVAN, TU.DBD_CAR_TRAILER=TJ.DBD_CAR_TRAILER,   ");
                                    queryBuilder.AppendFormat("     TU.DBD_CAR_CONTAINER=TJ.DBD_CAR_CONTAINER, TU.DBD_MACHINE=TJ.DBD_MACHINE, TU.DBD_OTHER=TJ.DBD_OTHER, TU.DBD_TANK=TJ.DBD_TANK,   ");
                                    queryBuilder.AppendFormat("     TU.DBD_STATION=TJ.DBD_STATION, TU.DBD_TYPE2=TJ.DBD_TYPE2, TU.DBD_MAINTAIN_CENTER=TJ.DBD_MAINTAIN_CENTER,   ");
                                    queryBuilder.AppendFormat("     TU.DBD_GENERAL_GARAGE=TJ.DBD_GENERAL_GARAGE, TU.DBD_MAINTAIN_DEPT=TJ.DBD_MAINTAIN_DEPT, TU.DBD_RECOMMENDER=TJ.DBD_RECOMMENDER,   ");
                                    queryBuilder.AppendFormat("     TU.DBD_SALE=TJ.DBD_SALE, TU.DBD_SALE_SUPPORT=TJ.DBD_SALE_SUPPORT, TU.DBD_REMARK=TJ.DBD_REMARK  ");
                                    queryBuilder.AppendFormat(" FROM PROSPECT TU  ");
                                    queryBuilder.AppendFormat(" INNER JOIN(");
                                    queryBuilder.AppendFormat("     SELECT @VAL_PROSPECT_ID PROSPECT_ID, [DBD_CODE], [DBD_CORP_TYPE], [DBD_JURISTIC_STATUS], [DBD_REG_CAPITAL], [DBD_TOTAL_INCOME], [DBD_PROFIT_LOSS], ");
                                    queryBuilder.AppendFormat("     [DBD_TOTAL_ASSET], [DBD_FLEET_CARD], [DBD_CORP_CARD], [DBD_OIL_CONSUPTION], [DBD_CURRENT_STATION], [DBD_PAY_CHANNEL], ");
                                    queryBuilder.AppendFormat("     [DBD_CAR_WHEEL4], [DBD_CAR_WHEEL6], [DBD_CAR_WHEEL8], [DBD_CARAVAN], [DBD_CAR_TRAILER], [DBD_CAR_CONTAINER], [DBD_MACHINE], ");
                                    queryBuilder.AppendFormat("     [DBD_OTHER], [DBD_TANK], [DBD_STATION], [DBD_TYPE2], [DBD_MAINTAIN_CENTER], [DBD_GENERAL_GARAGE], [DBD_MAINTAIN_DEPT], ");
                                    queryBuilder.AppendFormat("     [DBD_RECOMMENDER], [DBD_SALE], [DBD_SALE_SUPPORT], [DBD_REMARK]  ");
                                    queryBuilder.AppendFormat("     FROM PROSPECT  ");
                                    queryBuilder.AppendFormat("     WHERE PROSPECT_ID = @PROSPECT_ID  ");
                                    queryBuilder.AppendFormat("     ) TJ ON TJ.PROSPECT_ID = TU.PROSPECT_ID   ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PROSPECT_ID", VAL_PROSPECT_ID));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", cloneProspectModel.ProspectId));
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                    break;
                                case (int)FunctionTab.FUNCTION_TAB_ACCOUNT_TEAM:
                                    
                                    break;
                                default:
                                    break;
                            }
                        }

                        transaction.Commit();
                        Prospect re = new Prospect();
                        re.ProspectId = VAL_PROSPECT_ID;
                        return re;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }


        public async Task<GetProspectAccountForCloneProspectCustom> getProspectAccountForCloneProspect(string prospectId, UserProfileForBack userProfile)
        {
            GetProspectAccountForCloneProspectCustom o = null;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New

                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select TOP 1  P2.PROSP_ACC_ID, P3.BU_ID, ");
                queryBuilder.AppendFormat(" (SELECT STRING_AGG(TERRITORY_NAME_TH,'|')  WITHIN GROUP ( ORDER BY TERRITORY_ID ASC)  AS Result FROM [ORG_TERRITORY] WHERE BU_ID = P3.BU_ID) TERRITORY_NAME ");
                queryBuilder.AppendFormat(" from PROSPECT P1 ");
                queryBuilder.AppendFormat(" inner join PROSPECT P2 on P2.PROSP_ACC_ID = P1.PROSP_ACC_ID ");
                queryBuilder.AppendFormat(" left join PROSPECT P3 on P3.PROSPECT_ID = P2.PROSPECT_ID and P3.BU_ID = @BuId ");
                queryBuilder.AppendFormat(" where P1.PROSPECT_ID = @ProspectId ");
                queryBuilder.AppendFormat(" order by P3.BU_ID desc ");
                QueryUtils.addParam(command, "ProspectId", prospectId);
                QueryUtils.addParam(command, "BuId", userProfile.getBuId());
                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        o = new GetProspectAccountForCloneProspectCustom();
                        o.ProspAccId = QueryUtils.getValueAsString(record, "PROSP_ACC_ID");
                        o.BuId = QueryUtils.getValueAsString(record, "BU_ID");
                        o.TerritoryName = QueryUtils.getValueAsString(record, "TERRITORY_NAME");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }


        public async Task<EntitySearchResultBase<SearchProspectAllCustom>> searchProspectAll(SearchCriteriaBase<SearchProspectAllCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchProspectAllCustom> searchResult = new EntitySearchResultBase<SearchProspectAllCustom>();
            List<SearchProspectAllCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchProspectAllCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select p.PROSPECT_ID,pa.CUST_CODE,pa.ACC_NAME ");
                queryBuilder.AppendFormat(" from prospect p ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ACCOUNT pa on p.PROSP_ACC_ID = pa.PROSP_ACC_ID ");
                queryBuilder.AppendFormat(" where p.PROSPECT_STATUS > -1 ");


                // For Paging
                queryBuilder.AppendFormat(" order by pa.ACC_NAME  ");
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = queryBuilder.ToString();// Add New
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(queryBuilder, command, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging
                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    //

                    List<SearchProspectAllCustom> dataRecordList = new List<SearchProspectAllCustom>();
                    SearchProspectAllCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchProspectAllCustom();

                        dataRecord.ProspectId = QueryUtils.getValueAsDecimalRequired(record, "PROSPECT_ID");
                        dataRecord.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        dataRecord.AccName = QueryUtils.getValueAsString(record, "ACC_NAME");
                        
                        dataRecordList.Add(dataRecord);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //
                    lst = dataRecordList;
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }








    }
    public class GetProspectAccountForCloneProspectCustom
    {
        public string ProspAccId { get; set; }
        public string BuId { get; set; }
        public string TerritoryName { get; set; }
}
}