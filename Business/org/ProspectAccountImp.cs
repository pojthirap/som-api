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

namespace MyFirstAzureWebApp.Business.org
{

    public class ProspectAccountImp : IProspectAccount
    {
        private Logger log = LogManager.GetCurrentClassLogger();




        public async Task<EntitySearchResultBase<SearchMyProspectCustom>> searchMyAccount(SearchCriteriaBase<ProspectAccountCriteria> searchCriteria, UserProfileForBack userProfileForBack)
        {

            EntitySearchResultBase<SearchMyProspectCustom> searchResult = new EntitySearchResultBase<SearchMyProspectCustom>();
            List<SearchMyProspectCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();


                ProspectAccountCriteria o = searchCriteria.model;
                String ProspectStatusStr = "";
                if (o != null && o.ProspectStatusLst != null)
                {


                    int length_ = o.ProspectStatusLst.Length;
                    String[] ProspectStatusList = new String[length_];
                    for (int i = 0; i < length_; i++)
                    {
                        ProspectStatusList[i] = o.ProspectStatusLst[i];
                    }
                    ProspectStatusStr = String.Join(",", ProspectStatusList);
                }
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select PP.PROSPECT_ID,PP.PROSP_ACC_ID,PP.PROSPECT_STATUS,PP.UPDATE_DTM ");
                queryBuilder.AppendFormat(" ,PA.ACC_NAME,PA.CUST_CODE ");
                queryBuilder.AppendFormat(" ,PD.TELL_NO,PD.FAX_NO,PD.LATITUDE,PD.LONGITUDE ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT PA ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" where PP.PROSPECT_STATUS > -1  "); 
                if (o == null)
                {
                    o = new ProspectAccountCriteria();
                }
                o.EmpId = userProfileForBack.getEmpId();
                o.GroupCode = userProfileForBack.getAdmEmployeeGroupCode();
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.ProspectType))
                    {
                        queryBuilder.AppendFormat(" and PP.PROSPECT_TYPE  = @ProspectType  ");
                        QueryUtils.addParam(command, "ProspectType", o.ProspectType);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.EmpId))
                    {
                        queryBuilder.AppendFormat(" and PP.SALE_REP_ID  = @EmpId  ");
                        QueryUtils.addParam(command, "EmpId", o.EmpId);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.GroupCode))
                    {
                        queryBuilder.AppendFormat(" and PP.GROUP_CODE = @GroupCode  ");
                        QueryUtils.addParam(command, "GroupCode", o.GroupCode);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.AccName))
                    {
                        queryBuilder.AppendFormat(" and PA.ACC_NAME like @AccName  ");
                        QueryUtils.addParamLike(command, "AccName", o.AccName);// Add new
                    }
                    if (o.ProspectStatusLst != null && o.ProspectStatusLst.Length != 0)
                    {
                        queryBuilder.AppendFormat(" and  PP.PROSPECT_STATUS IN (" + QueryUtils.getParamIn("ProspectStatusStr", ProspectStatusStr) + ") " );
                        QueryUtils.addParamIn(command, "ProspectStatusStr", ProspectStatusStr);
                    }
                    if (!String.IsNullOrEmpty(o.ProspectId))
                    {
                        queryBuilder.AppendFormat(" and PP.PROSPECT_ID = @ProspectId ");
                        QueryUtils.addParam(command, "ProspectId", o.ProspectId);// Add new
                    }
                }


                // For Paging
                if (searchCriteria.searchOrder == 1)
                {
                    queryBuilder.AppendFormat(" ORDER BY PP.UPDATE_DTM DESC ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY PP.PROSP_ACC_ID  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    command.CommandText = queryBuilder.ToString();
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

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();

                using (var reader = command.ExecuteReader())
                {
                    lst = searchTemplateQuestionMapRow(reader);
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }

        private List<SearchMyProspectCustom> searchTemplateQuestionMapRow(DbDataReader reader)
        {
            List<SearchMyProspectCustom> lst = new List<SearchMyProspectCustom>();
            SearchMyProspectCustom o = null;
            while (reader.Read())
            {
                IDataRecord record = (IDataRecord)reader;
                o = new SearchMyProspectCustom();

                ProspectAccount prospectAccount;
                Prospect prospect;
                ProspectContact prospectContact;
                ProspectAddress prospectAddress;
                //ProspectRecommend prospectRecommend;

                    prospectAccount = new ProspectAccount();
                    prospect = new Prospect();
                    prospectContact = new ProspectContact();
                    prospectAddress = new ProspectAddress();
                    //prospectRecommend = new ProspectRecommend();



                    prospectAddress.ProspectId = QueryUtils.getValueAsDecimal(record, "PROSPECT_ID");
                    prospectAddress.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");

                    prospectContact.ProspectId = QueryUtils.getValueAsDecimal(record, "PROSPECT_ID");
                    prospectContact.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");

                    prospect.ProspectStatus = QueryUtils.getValueAsString(record, "PROSPECT_STATUS");
                    prospect.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");
                    prospect.ProspectId = QueryUtils.getValueAsDecimalRequired(record, "PROSPECT_ID");
                    prospect.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");

                    prospectAccount.ProspAccId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ACC_ID");
                    prospectAccount.AccName = QueryUtils.getValueAsString(record, "ACC_NAME");
                    prospectAccount.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");

                    prospectAddress.TellNo = QueryUtils.getValueAsString(record, "TELL_NO");
                    prospectAddress.FaxNo = QueryUtils.getValueAsString(record, "FAX_NO");
                    prospectAddress.Latitude = QueryUtils.getValueAsString(record, "LATITUDE");
                    prospectAddress.Longitude = QueryUtils.getValueAsString(record, "LONGITUDE");



                /*
                    
                    prospectAccount.BrandId = QueryUtils.getValueAsDecimal(record, "BRAND_ID");
                    prospectAccount.IdentifyId = QueryUtils.getValueAsString(record, "IDENTIFY_ID");
                    prospectAccount.AccGroupRef = QueryUtils.getValueAsString(record, "ACC_GROUP_REF");
                    prospectAccount.Remark = QueryUtils.getValueAsString(record, "PA_REMARK");
                    prospectAccount.SourceType = QueryUtils.getValueAsString(record, "SOURCE_TYPE");
                    prospectAccount.ActiveFlag = QueryUtils.getValueAsString(record, "PA_ACTIVE_FLAG");
                    prospectAccount.CreateUser = QueryUtils.getValueAsString(record, "PA_CREATE_USER");
                    prospectAccount.CreateDtm = QueryUtils.getValueAsDateTime(record, "PA_CREATE_DTM");
                    prospectAccount.UpdateUser = QueryUtils.getValueAsString(record, "PA_UPDATE_USER");
                    prospectAccount.UpdateDtm = QueryUtils.getValueAsDateTime(record, "PA_UPDATE_DTM");



                    prospect.BuId = QueryUtils.getValueAsDecimal(record, "BU_ID");
                    prospect.ServicesTypeId = QueryUtils.getValueAsString(record, "SERVICES_TYPE_ID");
                    prospect.LocTypeId = QueryUtils.getValueAsDecimal(record, "LOC_TYPE_ID");
                    prospect.SaleRepId = QueryUtils.getValueAsString(record, "SALE_REP_ID");
                    prospect.StationName = QueryUtils.getValueAsString(record, "STATION_NAME");
                    prospect.StationOpenFlag = QueryUtils.getValueAsString(record, "STATION_OPEN_FLAG");
                    prospect.ReasonCancel = QueryUtils.getValueAsString(record, "REASON_CANCEL");
                    prospect.BrandCateId = QueryUtils.getValueAsDecimal(record, "BRAND_CATE_ID");
                    prospect.BrandCateOther = QueryUtils.getValueAsString(record, "BRAND_CATE_OTHER");
                    prospect.AreaSquareWa = QueryUtils.getValueAsDecimal(record, "AREA_SQUARE_WA");
                    prospect.AreaNgan = QueryUtils.getValueAsDecimal(record, "AREA_NGAN");
                    prospect.AreaRai = QueryUtils.getValueAsDecimal(record, "AREA_RAI");
                    prospect.AreaWidthMeter = QueryUtils.getValueAsDecimal(record, "AREA_WIDTH_METER");
                    prospect.ShopJoint = QueryUtils.getValueAsString(record, "SHOP_JOINT");
                    prospect.LicenseStatus = QueryUtils.getValueAsString(record, "LICENSE_STATUS");
                    prospect.LicenseOther = QueryUtils.getValueAsString(record, "LICENSE_OTHER");
                    prospect.InterestStatus = QueryUtils.getValueAsString(record, "INTEREST_STATUS");
                    prospect.InterestOther = QueryUtils.getValueAsString(record, "INTEREST_OTHER");
                    prospect.SaleVolumeRef = QueryUtils.getValueAsString(record, "SALE_VOLUME_REF");
                    prospect.SaleVolume = QueryUtils.getValueAsDecimal(record, "SALE_VOLUME");
                    prospect.ProgressDate = QueryUtils.getValueAsDateTime(record, "PROGRESS_DATE");
                    prospect.TerminateDate = QueryUtils.getValueAsDateTime(record, "TERMINATE_DATE");
                    prospect.NearBankId = QueryUtils.getValueAsDecimal(record, "NEAR_BANK_ID");
                    prospect.QuotaOil = QueryUtils.getValueAsDecimal(record, "QUOTA_OIL");
                    prospect.QuotaLube = QueryUtils.getValueAsDecimal(record, "QUOTA_LUBE");
                    prospect.DispenserTotal = QueryUtils.getValueAsDecimal(record, "DISPENSER_TOTAL");
                    prospect.NozzleTotal = QueryUtils.getValueAsDecimal(record, "NOZZLE_TOTAL");
                    prospect.AddrTitleDeedNo = QueryUtils.getValueAsString(record, "ADDR_TITLE_DEED_NO");
                    prospect.AddrCertUtilisation = QueryUtils.getValueAsString(record, "ADDR_CERT_UTILISATION");
                    prospect.AddrParcelNo = QueryUtils.getValueAsString(record, "ADDR_PARCEL_NO");
                    prospect.AddrTambonNo = QueryUtils.getValueAsString(record, "ADDR_TAMBON_NO");
                    prospect.DbdCode = QueryUtils.getValueAsString(record, "DBD_CODE");
                    prospect.DbdCorpType = QueryUtils.getValueAsString(record, "DBD_CORP_TYPE");
                    prospect.DbdJuristicStatus = QueryUtils.getValueAsString(record, "DBD_JURISTIC_STATUS");
                    prospect.DbdRegCapital = QueryUtils.getValueAsDecimal(record, "DBD_REG_CAPITAL");
                    prospect.DbdTotalIncome = QueryUtils.getValueAsDecimal(record, "DBD_TOTAL_INCOME");
                    prospect.DbdProfitLoss = QueryUtils.getValueAsDecimal(record, "DBD_PROFIT_LOSS");
                    prospect.DbdTotalAsset = QueryUtils.getValueAsDecimal(record, "DBD_TOTAL_ASSET");
                    prospect.DbdFleetCard = QueryUtils.getValueAsString(record, "DBD_FLEET_CARD");
                    prospect.DbdCorpCard = QueryUtils.getValueAsString(record, "DBD_CORP_CARD");
                    prospect.DbdOilConsuption = QueryUtils.getValueAsString(record, "DBD_OIL_CONSUPTION");
                    prospect.DbdCurrentStation = QueryUtils.getValueAsString(record, "DBD_CURRENT_STATION");
                    prospect.DbdPayChannel = QueryUtils.getValueAsString(record, "DBD_PAY_CHANNEL");
                    prospect.DbdCarWheel4 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL4");
                    prospect.DbdCarWheel6 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL6");
                    prospect.DbdCarWheel8 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL8");
                    prospect.DbdCaravan = QueryUtils.getValueAsString(record, "DBD_CARAVAN");
                    prospect.DbdCarTrailer = QueryUtils.getValueAsString(record, "DBD_CAR_TRAILER");
                    prospect.DbdCarContainer = QueryUtils.getValueAsString(record, "DBD_CAR_CONTAINER");
                    prospect.DbdOther = QueryUtils.getValueAsString(record, "DBD_OTHER");
                    prospect.DbdTank = QueryUtils.getValueAsString(record, "DBD_TANK");
                    prospect.DbdStation = QueryUtils.getValueAsString(record, "DBD_STATION");
                    prospect.DbdType2 = QueryUtils.getValueAsString(record, "DBD_TYPE2");
                    prospect.DbdMaintainCenter = QueryUtils.getValueAsString(record, "DBD_MAINTAIN_CENTER");
                    prospect.DbdGeneralGarage = QueryUtils.getValueAsString(record, "DBD_GENERAL_GARAGE");
                    prospect.DbdMaintainDept = QueryUtils.getValueAsString(record, "DBD_MAINTAIN_DEPT");
                    prospect.DbdRecommender = QueryUtils.getValueAsString(record, "DBD_RECOMMENDER");
                    prospect.DbdSale = QueryUtils.getValueAsString(record, "DBD_SALE");
                    prospect.DbdSaleSupport = QueryUtils.getValueAsString(record, "DBD_SALE_SUPPORT");
                    prospect.DbdRemark = QueryUtils.getValueAsString(record, "DBD_REMARK");
                    prospect.MainFlag = QueryUtils.getValueAsString(record, "PP_MAIN_FLAG");
                    prospect.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                    prospect.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                    prospect.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                    prospect.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");



                    prospectContact.ProspContactId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_CONTACT_ID");
                    prospectContact.FirstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                    prospectContact.LastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                    prospectContact.PhoneNo = QueryUtils.getValueAsString(record, "PHONE_NO");
                    prospectContact.FaxNo = QueryUtils.getValueAsString(record, "PC_FAX_NO");
                    prospectContact.MobileNo = QueryUtils.getValueAsString(record, "MOBILE_NO");
                    prospectContact.Email = QueryUtils.getValueAsString(record, "EMAIL");
                    prospectContact.MainFlag = QueryUtils.getValueAsString(record, "PC_MAIN_FLAG");
                    prospectContact.ActiveFlag = QueryUtils.getValueAsString(record, "PC_ACTIVE_FLAG");



                    prospectAddress.ProspAddrId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ADDR_ID");
                    prospectAddress.AddrNo = QueryUtils.getValueAsString(record, "ADDR_NO");
                    prospectAddress.Moo = QueryUtils.getValueAsString(record, "MOO");
                    prospectAddress.Soi = QueryUtils.getValueAsString(record, "SOI");
                    prospectAddress.Street = QueryUtils.getValueAsString(record, "STREET");
                    prospectAddress.RegionCode = QueryUtils.getValueAsString(record, "REGION_CODE");
                    prospectAddress.ProvinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                    prospectAddress.ProvinceDbd = QueryUtils.getValueAsString(record, "PROVINCE_DBD");
                    prospectAddress.DistrictCode = QueryUtils.getValueAsString(record, "DISTRICT_CODE");
                    prospectAddress.SubdistrictCode = QueryUtils.getValueAsString(record, "SUBDISTRICT_CODE");
                    prospectAddress.PostCode = QueryUtils.getValueAsString(record, "POST_CODE");
                    prospectAddress.Remark = QueryUtils.getValueAsString(record, "PD_REMARK");
                    prospectAddress.MainFlag = QueryUtils.getValueAsString(record, "PD_MAIN_FLAG");
                    prospectAddress.ActiveFlag = QueryUtils.getValueAsString(record, "PD_ACTIVE_FLAG");
                */


                    o.ProspectAccount = prospectAccount;
                    o.Prospect = prospect;
                    o.ProspectContact = prospectContact;
                    o.ProspectAddress = prospectAddress;
                    //o.ProspectRecommend = prospectRecommend;


                lst.Add(o);
            }

            // Call Close when done reading.
            reader.Close();

            return lst;

        }




        public async Task<EntitySearchResultBase<SearchProspectRecommendCustom>> searchProspectRecommend(SearchCriteriaBase<ProspectAccountCriteria> searchCriteria, UserProfileForBack userProfileForBack)
        {
            

            EntitySearchResultBase<SearchProspectRecommendCustom> searchResult = new EntitySearchResultBase<SearchProspectRecommendCustom>();
            List<SearchProspectRecommendCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                ProspectAccountCriteria o = searchCriteria.model;
                String ProspectStatusStr = "";
                if (o != null && o.ProspectStatusLst != null)
                {
                    int length_ = o.ProspectStatusLst.Length;
                    String[] ProspectStatusList = new String[length_];
                    for (int i = 0; i < length_; i++)
                    {
                        ProspectStatusList[i] = o.ProspectStatusLst[i];
                    }
                    ProspectStatusStr = String.Join(",", ProspectStatusList);
                }
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select PP.PROSPECT_ID,PP.PROSP_ACC_ID,PP.PROSPECT_STATUS,PP.UPDATE_DTM ");
                queryBuilder.AppendFormat(" ,PA.ACC_NAME,PA.CUST_CODE ");
                queryBuilder.AppendFormat(" ,PD.TELL_NO,PD.FAX_NO,PD.LATITUDE,PD.LONGITUDE ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT PA ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" inner join PROSPECT_RECOMMEND PR on PR.PROSPECT_ID = PP.PROSPECT_ID ");
                queryBuilder.AppendFormat(" where PP.PROSPECT_STATUS > -1 and PP.PROSPECT_TYPE = '0'  ");


                if (o == null)
                {
                    o = new ProspectAccountCriteria();
                }
                //o.BuId =  userProfileForBack.getBuId().Equals(null) ? null : userProfileForBack.getBuId().ToString();
                o.BuId = userProfileForBack.getBuId() == 0 ? null : userProfileForBack.getBuId().ToString();
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.BuId))
                    {
                        queryBuilder.AppendFormat(" and PR.BU_ID = @BuId ");
                        QueryUtils.addParam(command, "BuId", o.BuId);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.AccName))
                    {
                        queryBuilder.AppendFormat(" and PA.ACC_NAME like @AccName  ");
                        QueryUtils.addParamLike(command, "AccName", o.AccName);// Add new
                    }
                    if (o.ProspectStatusLst != null && o.ProspectStatusLst.Length != 0)
                    {
                        queryBuilder.AppendFormat(" and  PP.PROSPECT_STATUS IN (" + QueryUtils.getParamIn("ProspectStatusStr", ProspectStatusStr) + ") ");
                        QueryUtils.addParamIn(command, "ProspectStatusStr", ProspectStatusStr);
                    }
                }


                // For Paging
                if (searchCriteria.searchOrder == 1)
                {
                    queryBuilder.AppendFormat(" ORDER BY PP.UPDATE_DTM DESC  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY PP.PROSP_ACC_ID  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    command.CommandText = queryBuilder.ToString();
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

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();

                using (var reader = command.ExecuteReader())
                {
                    lst = searchTemplateQuestionMapRowSearchProspectRecommendCustom(reader);
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }

        private List<SearchProspectRecommendCustom> searchTemplateQuestionMapRowSearchProspectRecommendCustom(DbDataReader reader)
        {
            List<SearchProspectRecommendCustom> lst = new List<SearchProspectRecommendCustom>();
            SearchProspectRecommendCustom o = null;
            while (reader.Read())
            {
                IDataRecord record = (IDataRecord)reader;
                o = new SearchProspectRecommendCustom();

                ProspectAccount prospectAccount;
                Prospect prospect;
                ProspectContact prospectContact;
                ProspectAddress prospectAddress;
                ProspectRecommend prospectRecommend;

                    prospectAccount = new ProspectAccount();
                    prospect = new Prospect();
                    prospectContact = new ProspectContact();
                    prospectAddress = new ProspectAddress();
                    prospectRecommend = new ProspectRecommend();


                    prospectAddress.ProspectId = QueryUtils.getValueAsDecimal(record, "PROSPECT_ID");
                    prospectAddress.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");
                    prospectAddress.TellNo = QueryUtils.getValueAsString(record, "TELL_NO");
                    prospectAddress.FaxNo = QueryUtils.getValueAsString(record, "FAX_NO");
                    prospectAddress.Latitude = QueryUtils.getValueAsString(record, "LATITUDE");
                    prospectAddress.Longitude = QueryUtils.getValueAsString(record, "LONGITUDE");


                    prospectContact.ProspectId = QueryUtils.getValueAsDecimal(record, "PROSPECT_ID");
                    prospectContact.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");


                    prospect.ProspectStatus = QueryUtils.getValueAsString(record, "PROSPECT_STATUS");
                    prospect.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");
                    prospect.ProspectId = QueryUtils.getValueAsDecimalRequired(record, "PROSPECT_ID");
                    prospect.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");

                    prospectAccount.ProspAccId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ACC_ID");
                    prospectAccount.AccName = QueryUtils.getValueAsString(record, "ACC_NAME");
                    prospectAccount.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");


                    prospectRecommend.ProspectId = QueryUtils.getValueAsDecimal(record, "PROSPECT_ID");



                /*
                prospectAccount.BrandId = QueryUtils.getValueAsDecimal(record, "BRAND_ID");
                prospectAccount.IdentifyId = QueryUtils.getValueAsString(record, "IDENTIFY_ID");
                prospectAccount.AccGroupRef = QueryUtils.getValueAsString(record, "ACC_GROUP_REF");
                prospectAccount.Remark = QueryUtils.getValueAsString(record, "PA_REMARK");
                prospectAccount.SourceType = QueryUtils.getValueAsString(record, "SOURCE_TYPE");
                prospectAccount.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                prospectAccount.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                prospectAccount.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                prospectAccount.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                prospectAccount.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");


                prospect.BuId = QueryUtils.getValueAsDecimal(record, "BU_ID");
                prospect.ServicesTypeId = QueryUtils.getValueAsString(record, "SERVICES_TYPE_ID");
                prospect.LocTypeId = QueryUtils.getValueAsDecimal(record, "LOC_TYPE_ID");
                prospect.SaleRepId = QueryUtils.getValueAsString(record, "SALE_REP_ID");
                prospect.StationName = QueryUtils.getValueAsString(record, "STATION_NAME");
                prospect.StationOpenFlag = QueryUtils.getValueAsString(record, "STATION_OPEN_FLAG");
                prospect.ReasonCancel = QueryUtils.getValueAsString(record, "REASON_CANCEL");
                prospect.BrandCateId = QueryUtils.getValueAsDecimal(record, "BRAND_CATE_ID");
                prospect.BrandCateOther = QueryUtils.getValueAsString(record, "BRAND_CATE_OTHER");
                prospect.AreaSquareWa = QueryUtils.getValueAsDecimal(record, "AREA_SQUARE_WA");
                prospect.AreaNgan = QueryUtils.getValueAsDecimal(record, "AREA_NGAN");
                prospect.AreaRai = QueryUtils.getValueAsDecimal(record, "AREA_RAI");
                prospect.AreaWidthMeter = QueryUtils.getValueAsDecimal(record, "AREA_WIDTH_METER");
                prospect.ShopJoint = QueryUtils.getValueAsString(record, "SHOP_JOINT");
                prospect.LicenseStatus = QueryUtils.getValueAsString(record, "LICENSE_STATUS");
                prospect.LicenseOther = QueryUtils.getValueAsString(record, "LICENSE_OTHER");
                prospect.InterestStatus = QueryUtils.getValueAsString(record, "INTEREST_STATUS");
                prospect.InterestOther = QueryUtils.getValueAsString(record, "INTEREST_OTHER");
                prospect.SaleVolumeRef = QueryUtils.getValueAsString(record, "SALE_VOLUME_REF");
                prospect.SaleVolume = QueryUtils.getValueAsDecimal(record, "SALE_VOLUME");
                prospect.ProgressDate = QueryUtils.getValueAsDateTime(record, "PROGRESS_DATE");
                prospect.TerminateDate = QueryUtils.getValueAsDateTime(record, "TERMINATE_DATE");
                prospect.NearBankId = QueryUtils.getValueAsDecimal(record, "NEAR_BANK_ID");
                prospect.QuotaOil = QueryUtils.getValueAsDecimal(record, "QUOTA_OIL");
                prospect.QuotaLube = QueryUtils.getValueAsDecimal(record, "QUOTA_LUBE");
                prospect.DispenserTotal = QueryUtils.getValueAsDecimal(record, "DISPENSER_TOTAL");
                prospect.NozzleTotal = QueryUtils.getValueAsDecimal(record, "NOZZLE_TOTAL");
                prospect.AddrTitleDeedNo = QueryUtils.getValueAsString(record, "ADDR_TITLE_DEED_NO");
                prospect.AddrCertUtilisation = QueryUtils.getValueAsString(record, "ADDR_CERT_UTILISATION");
                prospect.AddrParcelNo = QueryUtils.getValueAsString(record, "ADDR_PARCEL_NO");
                prospect.AddrTambonNo = QueryUtils.getValueAsString(record, "ADDR_TAMBON_NO");
                prospect.DbdCode = QueryUtils.getValueAsString(record, "DBD_CODE");
                prospect.DbdCorpType = QueryUtils.getValueAsString(record, "DBD_CORP_TYPE");
                prospect.DbdJuristicStatus = QueryUtils.getValueAsString(record, "DBD_JURISTIC_STATUS");
                prospect.DbdRegCapital = QueryUtils.getValueAsDecimal(record, "DBD_REG_CAPITAL");
                prospect.DbdTotalIncome = QueryUtils.getValueAsDecimal(record, "DBD_TOTAL_INCOME");
                prospect.DbdProfitLoss = QueryUtils.getValueAsDecimal(record, "DBD_PROFIT_LOSS");
                prospect.DbdTotalAsset = QueryUtils.getValueAsDecimal(record, "DBD_TOTAL_ASSET");
                prospect.DbdFleetCard = QueryUtils.getValueAsString(record, "DBD_FLEET_CARD");
                prospect.DbdCorpCard = QueryUtils.getValueAsString(record, "DBD_CORP_CARD");
                prospect.DbdOilConsuption = QueryUtils.getValueAsString(record, "DBD_OIL_CONSUPTION");
                prospect.DbdCurrentStation = QueryUtils.getValueAsString(record, "DBD_CURRENT_STATION");
                prospect.DbdPayChannel = QueryUtils.getValueAsString(record, "DBD_PAY_CHANNEL");
                prospect.DbdCarWheel4 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL4");
                prospect.DbdCarWheel6 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL6");
                prospect.DbdCarWheel8 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL8");
                prospect.DbdCaravan = QueryUtils.getValueAsString(record, "DBD_CARAVAN");
                prospect.DbdCarTrailer = QueryUtils.getValueAsString(record, "DBD_CAR_TRAILER");
                prospect.DbdCarContainer = QueryUtils.getValueAsString(record, "DBD_CAR_CONTAINER");
                prospect.DbdOther = QueryUtils.getValueAsString(record, "DBD_OTHER");
                prospect.DbdTank = QueryUtils.getValueAsString(record, "DBD_TANK");
                prospect.DbdStation = QueryUtils.getValueAsString(record, "DBD_STATION");
                prospect.DbdType2 = QueryUtils.getValueAsString(record, "DBD_TYPE2");
                prospect.DbdMaintainCenter = QueryUtils.getValueAsString(record, "DBD_MAINTAIN_CENTER");
                prospect.DbdGeneralGarage = QueryUtils.getValueAsString(record, "DBD_GENERAL_GARAGE");
                prospect.DbdMaintainDept = QueryUtils.getValueAsString(record, "DBD_MAINTAIN_DEPT");
                prospect.DbdRecommender = QueryUtils.getValueAsString(record, "DBD_RECOMMENDER");
                prospect.DbdSale = QueryUtils.getValueAsString(record, "DBD_SALE");
                prospect.DbdSaleSupport = QueryUtils.getValueAsString(record, "DBD_SALE_SUPPORT");
                prospect.DbdRemark = QueryUtils.getValueAsString(record, "DBD_REMARK");
                prospect.MainFlag = QueryUtils.getValueAsString(record, "MAIN_FLAG");
                prospect.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                prospect.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                prospect.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");



                prospectContact.ProspContactId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_CONTACT_ID");
                prospectContact.FirstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                prospectContact.LastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                prospectContact.PhoneNo = QueryUtils.getValueAsString(record, "PHONE_NO");
                prospectContact.FaxNo = QueryUtils.getValueAsString(record, "PC_FAX_NO");
                prospectContact.MobileNo = QueryUtils.getValueAsString(record, "MOBILE_NO");
                prospectContact.Email = QueryUtils.getValueAsString(record, "EMAIL");
                prospectContact.MainFlag = QueryUtils.getValueAsString(record, "MAIN_FLAG");
                prospectContact.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");



                prospectAddress.ProspAddrId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ADDR_ID");
                prospectAddress.AddrNo = QueryUtils.getValueAsString(record, "ADDR_NO");
                prospectAddress.Moo = QueryUtils.getValueAsString(record, "MOO");
                prospectAddress.Soi = QueryUtils.getValueAsString(record, "SOI");
                prospectAddress.Street = QueryUtils.getValueAsString(record, "STREET");
                prospectAddress.RegionCode = QueryUtils.getValueAsString(record, "REGION_CODE");
                prospectAddress.ProvinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                prospectAddress.ProvinceDbd = QueryUtils.getValueAsString(record, "PROVINCE_DBD");
                prospectAddress.DistrictCode = QueryUtils.getValueAsString(record, "DISTRICT_CODE");
                prospectAddress.SubdistrictCode = QueryUtils.getValueAsString(record, "SUBDISTRICT_CODE");
                prospectAddress.PostCode = QueryUtils.getValueAsString(record, "POST_CODE");
                prospectAddress.Remark = QueryUtils.getValueAsString(record, "PD_REMARK");
                prospectAddress.MainFlag = QueryUtils.getValueAsString(record, "MAIN_FLAG");
                prospectAddress.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");



                prospectRecommend.ProspRecommId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_RECOMM_ID");
                prospectRecommend.BuId = QueryUtils.getValueAsDecimal(record, "BU_ID");
            */


                o.ProspectAccount = prospectAccount;
                    o.Prospect = prospect;
                    o.ProspectContact = prospectContact;
                    o.ProspectAddress = prospectAddress;
                    o.ProspectRecommend = prospectRecommend;


                lst.Add(o);
            }

            // Call Close when done reading.
            reader.Close();

            return lst;

        }







        public async Task<EntitySearchResultBase<SearchAccountInTerritoryCustom>> searchAccountInTerritory(SearchCriteriaBase<ProspectAccountCriteria> searchCriteria, UserProfileForBack userProfileForBack)
        {

            EntitySearchResultBase<SearchAccountInTerritoryCustom> searchResult = new EntitySearchResultBase<SearchAccountInTerritoryCustom>();
            List<SearchAccountInTerritoryCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();


                ProspectAccountCriteria o = searchCriteria.model;
                if (o == null)
                {
                    o = new ProspectAccountCriteria();
                }
                o.EmpId = userProfileForBack.getEmpId();
                String ProspectStatusStr = "";
                if (o != null && o.ProspectStatusLst != null)
                {
                    int length_ = o.ProspectStatusLst.Length;
                    String[] ProspectStatusList = new String[length_];
                    for (int i = 0; i < length_; i++)
                    {
                        ProspectStatusList[i] = o.ProspectStatusLst[i];
                    }
                    ProspectStatusStr = String.Join(",", ProspectStatusList);
                }


                /*String territoryIdStr = "";
                if (userProfileForBack.OrgTerritory.data != null && userProfileForBack.OrgTerritory.data.Count != 0)
                {
                    int length_ = userProfileForBack.OrgTerritory.data.Count;
                    String[] territoryIdList = new String[length_];
                    for (int i = 0; i < length_; i++)
                    {
                        territoryIdList[i] = userProfileForBack.OrgTerritory.data.ElementAt(i).TerritoryId.ToString();
                    }
                    territoryIdStr = String.Join(",", territoryIdList);
                }*/

                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select PP.PROSPECT_ID,PP.PROSP_ACC_ID,PP.PROSPECT_STATUS,PP.UPDATE_DTM ");
                queryBuilder.AppendFormat(" ,PA.ACC_NAME,PA.CUST_CODE ");
                queryBuilder.AppendFormat(" ,PD.TELL_NO,PD.FAX_NO,PD.LATITUDE,PD.LONGITUDE ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT PA ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                //queryBuilder.AppendFormat(" where PP.PROSPECT_STATUS > -1 and exists (select 1 from ORG_SALE_TERRITORY OST where OST.EMP_ID = PP.SALE_REP_ID and OST.TERRITORY_ID IN (" + QueryUtils.getParamIn("territoryIdStr", territoryIdStr) + "))");
                queryBuilder.AppendFormat(" where PP.PROSPECT_STATUS > -1 and exists (select 1 from ORG_SALE_GROUP SG where PP.GROUP_CODE = SG.GROUP_CODE and SG.TERRITORY_ID = @orgSaleGroup_territoryId) ");
                queryBuilder.AppendFormat(" and PP.PROSPECT_TYPE= @ProspectType  ");
                queryBuilder.AppendFormat(" and PP.SALE_REP_ID != @SALE_REP_ID "); 
                QueryUtils.addParam(command, "orgSaleGroup_territoryId", userProfileForBack.getSaleGroupSaleOffice().TerritoryId);// Add new
                //QueryUtils.addParamIn(command, "territoryIdStr", territoryIdStr);
                QueryUtils.addParam(command, "ProspectType", o.ProspectType);// Add new
                QueryUtils.addParam(command, "SALE_REP_ID", userProfileForBack.getEmpId());// Add new
                if (!String.IsNullOrEmpty(o.AccName))
                {
                    queryBuilder.AppendFormat(" and PA.ACC_NAME like @AccName  ");
                    QueryUtils.addParamLike(command, "AccName", o.AccName);// Add new
                }
                if (o.ProspectStatusLst != null && o.ProspectStatusLst.Length != 0)
                {
                    queryBuilder.AppendFormat(" and  PP.PROSPECT_STATUS IN (" + QueryUtils.getParamIn("ProspectStatusStr", ProspectStatusStr) + ") ");
                    QueryUtils.addParamIn(command, "ProspectStatusStr", ProspectStatusStr);
                }
                queryBuilder.AppendFormat(" union ");
                queryBuilder.AppendFormat(" select PP.PROSPECT_ID,PP.PROSP_ACC_ID,PP.PROSPECT_STATUS,PP.UPDATE_DTM ");
                queryBuilder.AppendFormat(" ,PA.ACC_NAME,PA.CUST_CODE ");
                queryBuilder.AppendFormat(" ,PD.TELL_NO,PD.FAX_NO,PD.LATITUDE,PD.LONGITUDE ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT PA ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                //queryBuilder.AppendFormat(" where PP.PROSPECT_STATUS > -1 and exists (select 1 from PROSPECT_DEDICATE_TERT DT where DT.PROSPECT_ID = PP.PROSPECT_ID and DT.TERRITORY_ID IN (" + QueryUtils.getParamIn("territoryIdStr", territoryIdStr) + "))");
                queryBuilder.AppendFormat(" where PP.PROSPECT_STATUS > -1 and exists (select 1 from PROSPECT_DEDICATE_TERT DT where DT.PROSPECT_ID = PP.PROSPECT_ID and DT.TERRITORY_ID = @orgSaleGroup_territoryId) ");
                queryBuilder.AppendFormat(" and PP.PROSPECT_TYPE= @ProspectType  ");
                queryBuilder.AppendFormat(" and PP.SALE_REP_ID != @SALE_REP_ID ");
                //QueryUtils.addParamIn(command, "territoryIdStr", territoryIdStr);// '@territoryIdStr' has already been declared
                //QueryUtils.addParam(command, "ProspectType", o.ProspectType);// '@ProspectType' has already been declared
                //QueryUtils.addParam(command, "SALE_REP_ID", userProfileForBack.getEmpId());// '@SALE_REP_ID' has already been declared
                if (!String.IsNullOrEmpty(o.AccName))
                {
                    queryBuilder.AppendFormat(" and PA.ACC_NAME like @AccName  ");
                    //QueryUtils.addParamLike(command, "AccName", o.AccName);// '@AccName' has already been declared
                }
                if (o.ProspectStatusLst != null && o.ProspectStatusLst.Length != 0)
                {
                    queryBuilder.AppendFormat(" and  PP.PROSPECT_STATUS IN (" + QueryUtils.getParamIn("ProspectStatusStr", ProspectStatusStr) + ") ");
                    //QueryUtils.addParamIn(command, "ProspectStatusStr", ProspectStatusStr);// 'ProspectStatusStr' has already been declared
                }


                // For Paging
                if (searchCriteria.searchOrder == 1)
                {
                    queryBuilder.AppendFormat(" ORDER BY PP.UPDATE_DTM DESC  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY PP.PROSP_ACC_ID  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    command.CommandText = queryBuilder.ToString();
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

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();

                using (var reader = command.ExecuteReader())
                {
                    lst = searchTemplateQuestionMapRowSearchAccountInTerritoryCustom(reader);
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }

        private List<SearchAccountInTerritoryCustom> searchTemplateQuestionMapRowSearchAccountInTerritoryCustom(DbDataReader reader)
        {
            List<SearchAccountInTerritoryCustom> lst = new List<SearchAccountInTerritoryCustom>();
            SearchAccountInTerritoryCustom o = null;
            while (reader.Read())
            {
                IDataRecord record = (IDataRecord)reader;
                o = new SearchAccountInTerritoryCustom();

                ProspectAccount prospectAccount;
                Prospect prospect;
                ProspectContact prospectContact;
                ProspectAddress prospectAddress;
                //ProspectRecommend prospectRecommend;

                    prospectAccount = new ProspectAccount();
                    prospect = new Prospect();
                    prospectContact = new ProspectContact();
                    prospectAddress = new ProspectAddress();
                //prospectRecommend = new ProspectRecommend();


                    prospectAddress.ProspectId = QueryUtils.getValueAsDecimal(record, "PROSPECT_ID");
                    prospectAddress.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");
                    prospectAddress.TellNo = QueryUtils.getValueAsString(record, "TELL_NO");
                    prospectAddress.FaxNo = QueryUtils.getValueAsString(record, "FAX_NO");
                    prospectAddress.Latitude = QueryUtils.getValueAsString(record, "LATITUDE");
                    prospectAddress.Longitude = QueryUtils.getValueAsString(record, "LONGITUDE");

                    prospectContact.ProspectId = QueryUtils.getValueAsDecimal(record, "PROSPECT_ID");
                    prospectContact.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");


                    prospect.ProspectStatus = QueryUtils.getValueAsString(record, "PROSPECT_STATUS");
                    prospect.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");
                    prospect.ProspectId = QueryUtils.getValueAsDecimalRequired(record, "PROSPECT_ID");
                    prospect.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");


                    prospectAccount.AccName = QueryUtils.getValueAsString(record, "ACC_NAME");
                    prospectAccount.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                    prospectAccount.ProspAccId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ACC_ID");


                /*
                prospectAccount.BrandId = QueryUtils.getValueAsDecimal(record, "BRAND_ID");
                prospectAccount.IdentifyId = QueryUtils.getValueAsString(record, "IDENTIFY_ID");
                prospectAccount.AccGroupRef = QueryUtils.getValueAsString(record, "ACC_GROUP_REF");
                prospectAccount.Remark = QueryUtils.getValueAsString(record, "PA_REMARK");
                prospectAccount.SourceType = QueryUtils.getValueAsString(record, "SOURCE_TYPE");
                prospectAccount.ActiveFlag = QueryUtils.getValueAsString(record, "PA_ACTIVE_FLAG");
                prospectAccount.CreateUser = QueryUtils.getValueAsString(record, "PA_CREATE_USER");
                prospectAccount.CreateDtm = QueryUtils.getValueAsDateTime(record, "PA_CREATE_DTM");
                prospectAccount.UpdateUser = QueryUtils.getValueAsString(record, "PA_UPDATE_USER");
                prospectAccount.UpdateDtm = QueryUtils.getValueAsDateTime(record, "PA_UPDATE_DTM");


                prospect.BuId = QueryUtils.getValueAsDecimal(record, "BU_ID");
                prospect.ServicesTypeId = QueryUtils.getValueAsString(record, "SERVICES_TYPE_ID");
                prospect.LocTypeId = QueryUtils.getValueAsDecimal(record, "LOC_TYPE_ID");
                prospect.SaleRepId = QueryUtils.getValueAsString(record, "SALE_REP_ID");
                prospect.StationName = QueryUtils.getValueAsString(record, "STATION_NAME");
                prospect.StationOpenFlag = QueryUtils.getValueAsString(record, "STATION_OPEN_FLAG");
                prospect.ReasonCancel = QueryUtils.getValueAsString(record, "REASON_CANCEL");
                prospect.BrandCateId = QueryUtils.getValueAsDecimal(record, "BRAND_CATE_ID");
                prospect.BrandCateOther = QueryUtils.getValueAsString(record, "BRAND_CATE_OTHER");
                prospect.AreaSquareWa = QueryUtils.getValueAsDecimal(record, "AREA_SQUARE_WA");
                prospect.AreaNgan = QueryUtils.getValueAsDecimal(record, "AREA_NGAN");
                prospect.AreaRai = QueryUtils.getValueAsDecimal(record, "AREA_RAI");
                prospect.AreaWidthMeter = QueryUtils.getValueAsDecimal(record, "AREA_WIDTH_METER");
                prospect.ShopJoint = QueryUtils.getValueAsString(record, "SHOP_JOINT");
                prospect.LicenseStatus = QueryUtils.getValueAsString(record, "LICENSE_STATUS");
                prospect.LicenseOther = QueryUtils.getValueAsString(record, "LICENSE_OTHER");
                prospect.InterestStatus = QueryUtils.getValueAsString(record, "INTEREST_STATUS");
                prospect.InterestOther = QueryUtils.getValueAsString(record, "INTEREST_OTHER");
                prospect.SaleVolumeRef = QueryUtils.getValueAsString(record, "SALE_VOLUME_REF");
                prospect.SaleVolume = QueryUtils.getValueAsDecimal(record, "SALE_VOLUME");
                prospect.ProgressDate = QueryUtils.getValueAsDateTime(record, "PROGRESS_DATE");
                prospect.TerminateDate = QueryUtils.getValueAsDateTime(record, "TERMINATE_DATE");
                prospect.NearBankId = QueryUtils.getValueAsDecimal(record, "NEAR_BANK_ID");
                prospect.QuotaOil = QueryUtils.getValueAsDecimal(record, "QUOTA_OIL");
                prospect.QuotaLube = QueryUtils.getValueAsDecimal(record, "QUOTA_LUBE");
                prospect.DispenserTotal = QueryUtils.getValueAsDecimal(record, "DISPENSER_TOTAL");
                prospect.NozzleTotal = QueryUtils.getValueAsDecimal(record, "NOZZLE_TOTAL");
                prospect.AddrTitleDeedNo = QueryUtils.getValueAsString(record, "ADDR_TITLE_DEED_NO");
                prospect.AddrCertUtilisation = QueryUtils.getValueAsString(record, "ADDR_CERT_UTILISATION");
                prospect.AddrParcelNo = QueryUtils.getValueAsString(record, "ADDR_PARCEL_NO");
                prospect.AddrTambonNo = QueryUtils.getValueAsString(record, "ADDR_TAMBON_NO");
                prospect.DbdCode = QueryUtils.getValueAsString(record, "DBD_CODE");
                prospect.DbdCorpType = QueryUtils.getValueAsString(record, "DBD_CORP_TYPE");
                prospect.DbdJuristicStatus = QueryUtils.getValueAsString(record, "DBD_JURISTIC_STATUS");
                prospect.DbdRegCapital = QueryUtils.getValueAsDecimal(record, "DBD_REG_CAPITAL");
                prospect.DbdTotalIncome = QueryUtils.getValueAsDecimal(record, "DBD_TOTAL_INCOME");
                prospect.DbdProfitLoss = QueryUtils.getValueAsDecimal(record, "DBD_PROFIT_LOSS");
                prospect.DbdTotalAsset = QueryUtils.getValueAsDecimal(record, "DBD_TOTAL_ASSET");
                prospect.DbdFleetCard = QueryUtils.getValueAsString(record, "DBD_FLEET_CARD");
                prospect.DbdCorpCard = QueryUtils.getValueAsString(record, "DBD_CORP_CARD");
                prospect.DbdOilConsuption = QueryUtils.getValueAsString(record, "DBD_OIL_CONSUPTION");
                prospect.DbdCurrentStation = QueryUtils.getValueAsString(record, "DBD_CURRENT_STATION");
                prospect.DbdPayChannel = QueryUtils.getValueAsString(record, "DBD_PAY_CHANNEL");
                prospect.DbdCarWheel4 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL4");
                prospect.DbdCarWheel6 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL6");
                prospect.DbdCarWheel8 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL8");
                prospect.DbdCaravan = QueryUtils.getValueAsString(record, "DBD_CARAVAN");
                prospect.DbdCarTrailer = QueryUtils.getValueAsString(record, "DBD_CAR_TRAILER");
                prospect.DbdCarContainer = QueryUtils.getValueAsString(record, "DBD_CAR_CONTAINER");
                prospect.DbdOther = QueryUtils.getValueAsString(record, "DBD_OTHER");
                prospect.DbdTank = QueryUtils.getValueAsString(record, "DBD_TANK");
                prospect.DbdStation = QueryUtils.getValueAsString(record, "DBD_STATION");
                prospect.DbdType2 = QueryUtils.getValueAsString(record, "DBD_TYPE2");
                prospect.DbdMaintainCenter = QueryUtils.getValueAsString(record, "DBD_MAINTAIN_CENTER");
                prospect.DbdGeneralGarage = QueryUtils.getValueAsString(record, "DBD_GENERAL_GARAGE");
                prospect.DbdMaintainDept = QueryUtils.getValueAsString(record, "DBD_MAINTAIN_DEPT");
                prospect.DbdRecommender = QueryUtils.getValueAsString(record, "DBD_RECOMMENDER");
                prospect.DbdSale = QueryUtils.getValueAsString(record, "DBD_SALE");
                prospect.DbdSaleSupport = QueryUtils.getValueAsString(record, "DBD_SALE_SUPPORT");
                prospect.DbdRemark = QueryUtils.getValueAsString(record, "DBD_REMARK");
                prospect.MainFlag = QueryUtils.getValueAsString(record, "PP_MAIN_FLAG");
                prospect.CreateUser = QueryUtils.getValueAsString(record, "PP_CREATE_USER");
                prospect.CreateDtm = QueryUtils.getValueAsDateTime(record, "PP_CREATE_DTM");
                prospect.UpdateUser = QueryUtils.getValueAsString(record, "PP_UPDATE_USER");



                prospectContact.ProspContactId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_CONTACT_ID");
                prospectContact.FirstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                prospectContact.LastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                prospectContact.PhoneNo = QueryUtils.getValueAsString(record, "PHONE_NO");
                prospectContact.FaxNo = QueryUtils.getValueAsString(record, "PC_FAX_NO");
                prospectContact.MobileNo = QueryUtils.getValueAsString(record, "MOBILE_NO");
                prospectContact.Email = QueryUtils.getValueAsString(record, "EMAIL");
                prospectContact.MainFlag = QueryUtils.getValueAsString(record, "PC_MAIN_FLAG");
                prospectContact.ActiveFlag = QueryUtils.getValueAsString(record, "PC_ACTIVE_FLAG");



                prospectAddress.ProspAddrId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ADDR_ID");
                prospectAddress.AddrNo = QueryUtils.getValueAsString(record, "ADDR_NO");
                prospectAddress.Moo = QueryUtils.getValueAsString(record, "MOO");
                prospectAddress.Soi = QueryUtils.getValueAsString(record, "SOI");
                prospectAddress.Street = QueryUtils.getValueAsString(record, "STREET");
                prospectAddress.RegionCode = QueryUtils.getValueAsString(record, "REGION_CODE");
                prospectAddress.ProvinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                prospectAddress.ProvinceDbd = QueryUtils.getValueAsString(record, "PROVINCE_DBD");
                prospectAddress.DistrictCode = QueryUtils.getValueAsString(record, "DISTRICT_CODE");
                prospectAddress.SubdistrictCode = QueryUtils.getValueAsString(record, "SUBDISTRICT_CODE");
                prospectAddress.PostCode = QueryUtils.getValueAsString(record, "POST_CODE");
                prospectAddress.Remark = QueryUtils.getValueAsString(record, "PD_REMARK");
                prospectAddress.MainFlag = QueryUtils.getValueAsString(record, "PD_MAIN_FLAG");
                prospectAddress.ActiveFlag = QueryUtils.getValueAsString(record, "PD_ACTIVE_FLAG");
                */




                o.ProspectAccount = prospectAccount;
                    o.Prospect = prospect;
                    o.ProspectContact = prospectContact;
                    o.ProspectAddress = prospectAddress;
                    //o.ProspectRecommend = prospectRecommend;


                lst.Add(o);
            }

            // Call Close when done reading.
            reader.Close();

            return lst;

        }



        public async Task<EntitySearchResultBase<SearchOtherProspectCustom>> searchOtherProspect(SearchCriteriaBase<ProspectAccountCriteria> searchCriteria, UserProfileForBack userProfileForBack)
        {

            EntitySearchResultBase<SearchOtherProspectCustom> searchResult = new EntitySearchResultBase<SearchOtherProspectCustom>();
            List<SearchOtherProspectCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                ProspectAccountCriteria o = searchCriteria.model;
                String ProspectStatusStr = "";
                if (o != null && o.ProspectStatusLst != null)
                {
                    int length_ = o.ProspectStatusLst.Length;
                    String[] ProspectStatusList = new String[length_];
                    for (int i = 0; i < length_; i++)
                    {
                        ProspectStatusList[i] = o.ProspectStatusLst[i];
                    }
                    ProspectStatusStr = String.Join(",", ProspectStatusList);
                }
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select PP.PROSPECT_ID,PP.BU_ID,PP.PROSP_ACC_ID,PP.PROSPECT_STATUS,PP.UPDATE_DTM ");
                queryBuilder.AppendFormat(" ,PA.ACC_NAME,PA.CUST_CODE ");
                queryBuilder.AppendFormat(" ,PD.TELL_NO,PD.FAX_NO,PD.LATITUDE,PD.LONGITUDE ");
                queryBuilder.AppendFormat(" ,BU.BU_NAME_TH ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT PA ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID ");
                queryBuilder.AppendFormat(" inner join ORG_BUSINESS_UNIT BU on BU.BU_ID = PP.BU_ID ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" where PP.PROSPECT_STATUS > -1 and PP.SALE_REP_ID != @SALE_REP_ID ");
                /*queryBuilder.AppendFormat(" and NOT EXISTS(");
                queryBuilder.AppendFormat("     select 1 ");
                queryBuilder.AppendFormat("     from ORG_SALE_TERRITORY OST ");
                queryBuilder.AppendFormat("     where exists( ");
                queryBuilder.AppendFormat("     select ST.TERRITORY_ID ");
                queryBuilder.AppendFormat("     from ORG_TERRITORY T ");
                queryBuilder.AppendFormat("     inner ");
                queryBuilder.AppendFormat("     join ORG_SALE_TERRITORY ST on ST.TERRITORY_ID = T.TERRITORY_ID ");
                queryBuilder.AppendFormat("     where ST.EMP_ID = @EMP_ID ");
                queryBuilder.AppendFormat("     and ST.TERRITORY_ID = OST.TERRITORY_ID ");
                queryBuilder.AppendFormat("     and OST.EMP_ID = PP.SALE_REP_ID) ");
                queryBuilder.AppendFormat(" ) ");*/
                queryBuilder.AppendFormat(" and NOT EXISTS(");
                queryBuilder.AppendFormat("     select 1 ");
                queryBuilder.AppendFormat("     from ORG_SALE_GROUP OSG ");
                queryBuilder.AppendFormat("     where OSG.GROUP_CODE = PP.GROUP_CODE ");
                queryBuilder.AppendFormat("     and OSG.TERRITORY_ID = @orgSaleGroup_territoryId ");
                queryBuilder.AppendFormat(" ) ");

                QueryUtils.addParam(command, "SALE_REP_ID", userProfileForBack.getEmpId());// Add new
                //QueryUtils.addParam(command, "EMP_ID", userProfileForBack.getEmpId());// Add new
                QueryUtils.addParam(command, "orgSaleGroup_territoryId", userProfileForBack.getSaleGroupSaleOffice().TerritoryId);// Add new


                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.AccName))
                    {
                        queryBuilder.AppendFormat(" and PA.ACC_NAME like @AccName  ");
                        QueryUtils.addParamLike(command, "AccName", o.AccName);// Add new
                    }
                    if (o.ProspectStatusLst != null && o.ProspectStatusLst.Length != 0)
                    {
                        queryBuilder.AppendFormat(" and  PP.PROSPECT_STATUS IN (" + QueryUtils.getParamIn("ProspectStatusStr", ProspectStatusStr) + ") ");
                        QueryUtils.addParamIn(command, "ProspectStatusStr", ProspectStatusStr);
                    }
                }


                // For Paging
                if (searchCriteria.searchOrder == 1)
                {
                    queryBuilder.AppendFormat(" ORDER BY PP.UPDATE_DTM DESC  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY PP.PROSP_ACC_ID  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    command.CommandText = queryBuilder.ToString();
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

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();

                using (var reader = command.ExecuteReader())
                {
                    lst = searchTemplateQuestionMapRowSearchOtherProspectCustom(reader);
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }

        private List<SearchOtherProspectCustom> searchTemplateQuestionMapRowSearchOtherProspectCustom(DbDataReader reader)
        {
            List<SearchOtherProspectCustom> lst = new List<SearchOtherProspectCustom>();
            SearchOtherProspectCustom o = null;
            while (reader.Read())
            {
                IDataRecord record = (IDataRecord)reader;
                o = new SearchOtherProspectCustom();

                ProspectAccount prospectAccount;
                Prospect prospect;
                ProspectContact prospectContact;
                ProspectAddress prospectAddress;
                //ProspectRecommend prospectRecommend;

                    prospectAccount = new ProspectAccount();
                    prospect = new Prospect();
                    prospectContact = new ProspectContact();
                    prospectAddress = new ProspectAddress();
                    //prospectRecommend = new ProspectRecommend();


                    prospectAddress.ProspectId = QueryUtils.getValueAsDecimal(record, "PROSPECT_ID");
                    prospectAddress.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");


                    prospectContact.ProspectId = QueryUtils.getValueAsDecimal(record, "PROSPECT_ID");
                    prospectContact.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");


                    prospect.BuId = QueryUtils.getValueAsDecimal(record, "BU_ID");
                    prospect.ProspectStatus = QueryUtils.getValueAsString(record, "PROSPECT_STATUS");
                    prospect.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");
                    prospect.ProspectId = QueryUtils.getValueAsDecimalRequired(record, "PROSPECT_ID");
                    prospect.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");


                    prospectAccount.AccName = QueryUtils.getValueAsString(record, "ACC_NAME");
                    prospectAccount.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                    prospectAccount.ProspAccId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ACC_ID");


                    prospectAddress.TellNo = QueryUtils.getValueAsString(record, "TELL_NO");
                    prospectAddress.FaxNo = QueryUtils.getValueAsString(record, "FAX_NO");
                    prospectAddress.Latitude = QueryUtils.getValueAsString(record, "LATITUDE");
                    prospectAddress.Longitude = QueryUtils.getValueAsString(record, "LONGITUDE");


                    o.BuNameTh = QueryUtils.getValueAsString(record, "BU_NAME_TH");



                    /*
                    prospectAccount.BrandId = QueryUtils.getValueAsDecimal(record, "BRAND_ID");
                    prospectAccount.IdentifyId = QueryUtils.getValueAsString(record, "IDENTIFY_ID");
                    prospectAccount.AccGroupRef = QueryUtils.getValueAsString(record, "ACC_GROUP_REF");
                    prospectAccount.Remark = QueryUtils.getValueAsString(record, "PA_REMARK");
                    prospectAccount.SourceType = QueryUtils.getValueAsString(record, "SOURCE_TYPE");
                    prospectAccount.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                    prospectAccount.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                    prospectAccount.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                    prospectAccount.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                    prospectAccount.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");


                    prospect.ServicesTypeId = QueryUtils.getValueAsString(record, "SERVICES_TYPE_ID");
                    prospect.LocTypeId = QueryUtils.getValueAsDecimal(record, "LOC_TYPE_ID");
                    prospect.SaleRepId = QueryUtils.getValueAsString(record, "SALE_REP_ID");
                    prospect.StationName = QueryUtils.getValueAsString(record, "STATION_NAME");
                    prospect.StationOpenFlag = QueryUtils.getValueAsString(record, "STATION_OPEN_FLAG");
                    prospect.ReasonCancel = QueryUtils.getValueAsString(record, "REASON_CANCEL");
                    prospect.BrandCateId = QueryUtils.getValueAsDecimal(record, "BRAND_CATE_ID");
                    prospect.BrandCateOther = QueryUtils.getValueAsString(record, "BRAND_CATE_OTHER");
                    prospect.AreaSquareWa = QueryUtils.getValueAsDecimal(record, "AREA_SQUARE_WA");
                    prospect.AreaNgan = QueryUtils.getValueAsDecimal(record, "AREA_NGAN");
                    prospect.AreaRai = QueryUtils.getValueAsDecimal(record, "AREA_RAI");
                    prospect.AreaWidthMeter = QueryUtils.getValueAsDecimal(record, "AREA_WIDTH_METER");
                    prospect.ShopJoint = QueryUtils.getValueAsString(record, "SHOP_JOINT");
                    prospect.LicenseStatus = QueryUtils.getValueAsString(record, "LICENSE_STATUS");
                    prospect.LicenseOther = QueryUtils.getValueAsString(record, "LICENSE_OTHER");
                    prospect.InterestStatus = QueryUtils.getValueAsString(record, "INTEREST_STATUS");
                    prospect.InterestOther = QueryUtils.getValueAsString(record, "INTEREST_OTHER");
                    prospect.SaleVolumeRef = QueryUtils.getValueAsString(record, "SALE_VOLUME_REF");
                    prospect.SaleVolume = QueryUtils.getValueAsDecimal(record, "SALE_VOLUME");
                    prospect.ProgressDate = QueryUtils.getValueAsDateTime(record, "PROGRESS_DATE");
                    prospect.TerminateDate = QueryUtils.getValueAsDateTime(record, "TERMINATE_DATE");
                    prospect.NearBankId = QueryUtils.getValueAsDecimal(record, "NEAR_BANK_ID");
                    prospect.QuotaOil = QueryUtils.getValueAsDecimal(record, "QUOTA_OIL");
                    prospect.QuotaLube = QueryUtils.getValueAsDecimal(record, "QUOTA_LUBE");
                    prospect.DispenserTotal = QueryUtils.getValueAsDecimal(record, "DISPENSER_TOTAL");
                    prospect.NozzleTotal = QueryUtils.getValueAsDecimal(record, "NOZZLE_TOTAL");
                    prospect.AddrTitleDeedNo = QueryUtils.getValueAsString(record, "ADDR_TITLE_DEED_NO");
                    prospect.AddrCertUtilisation = QueryUtils.getValueAsString(record, "ADDR_CERT_UTILISATION");
                    prospect.AddrParcelNo = QueryUtils.getValueAsString(record, "ADDR_PARCEL_NO");
                    prospect.AddrTambonNo = QueryUtils.getValueAsString(record, "ADDR_TAMBON_NO");
                    prospect.DbdCode = QueryUtils.getValueAsString(record, "DBD_CODE");
                    prospect.DbdCorpType = QueryUtils.getValueAsString(record, "DBD_CORP_TYPE");
                    prospect.DbdJuristicStatus = QueryUtils.getValueAsString(record, "DBD_JURISTIC_STATUS");
                    prospect.DbdRegCapital = QueryUtils.getValueAsDecimal(record, "DBD_REG_CAPITAL");
                    prospect.DbdTotalIncome = QueryUtils.getValueAsDecimal(record, "DBD_TOTAL_INCOME");
                    prospect.DbdProfitLoss = QueryUtils.getValueAsDecimal(record, "DBD_PROFIT_LOSS");
                    prospect.DbdTotalAsset = QueryUtils.getValueAsDecimal(record, "DBD_TOTAL_ASSET");
                    prospect.DbdFleetCard = QueryUtils.getValueAsString(record, "DBD_FLEET_CARD");
                    prospect.DbdCorpCard = QueryUtils.getValueAsString(record, "DBD_CORP_CARD");
                    prospect.DbdOilConsuption = QueryUtils.getValueAsString(record, "DBD_OIL_CONSUPTION");
                    prospect.DbdCurrentStation = QueryUtils.getValueAsString(record, "DBD_CURRENT_STATION");
                    prospect.DbdPayChannel = QueryUtils.getValueAsString(record, "DBD_PAY_CHANNEL");
                    prospect.DbdCarWheel4 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL4");
                    prospect.DbdCarWheel6 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL6");
                    prospect.DbdCarWheel8 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL8");
                    prospect.DbdCaravan = QueryUtils.getValueAsString(record, "DBD_CARAVAN");
                    prospect.DbdCarTrailer = QueryUtils.getValueAsString(record, "DBD_CAR_TRAILER");
                    prospect.DbdCarContainer = QueryUtils.getValueAsString(record, "DBD_CAR_CONTAINER");
                    prospect.DbdOther = QueryUtils.getValueAsString(record, "DBD_OTHER");
                    prospect.DbdTank = QueryUtils.getValueAsString(record, "DBD_TANK");
                    prospect.DbdStation = QueryUtils.getValueAsString(record, "DBD_STATION");
                    prospect.DbdType2 = QueryUtils.getValueAsString(record, "DBD_TYPE2");
                    prospect.DbdMaintainCenter = QueryUtils.getValueAsString(record, "DBD_MAINTAIN_CENTER");
                    prospect.DbdGeneralGarage = QueryUtils.getValueAsString(record, "DBD_GENERAL_GARAGE");
                    prospect.DbdMaintainDept = QueryUtils.getValueAsString(record, "DBD_MAINTAIN_DEPT");
                    prospect.DbdRecommender = QueryUtils.getValueAsString(record, "DBD_RECOMMENDER");
                    prospect.DbdSale = QueryUtils.getValueAsString(record, "DBD_SALE");
                    prospect.DbdSaleSupport = QueryUtils.getValueAsString(record, "DBD_SALE_SUPPORT");
                    prospect.DbdRemark = QueryUtils.getValueAsString(record, "DBD_REMARK");
                    prospect.MainFlag = QueryUtils.getValueAsString(record, "MAIN_FLAG");
                    prospect.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                    prospect.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                    prospect.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");



                    prospectContact.ProspContactId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_CONTACT_ID");
                    prospectContact.FirstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                    prospectContact.LastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                    prospectContact.PhoneNo = QueryUtils.getValueAsString(record, "PHONE_NO");
                    prospectContact.FaxNo = QueryUtils.getValueAsString(record, "PC_FAX_NO");
                    prospectContact.MobileNo = QueryUtils.getValueAsString(record, "MOBILE_NO");
                    prospectContact.Email = QueryUtils.getValueAsString(record, "EMAIL");
                    prospectContact.MainFlag = QueryUtils.getValueAsString(record, "MAIN_FLAG");
                    prospectContact.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");



                    prospectAddress.ProspAddrId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ADDR_ID");
                    prospectAddress.AddrNo = QueryUtils.getValueAsString(record, "ADDR_NO");
                    prospectAddress.Moo = QueryUtils.getValueAsString(record, "MOO");
                    prospectAddress.Soi = QueryUtils.getValueAsString(record, "SOI");
                    prospectAddress.Street = QueryUtils.getValueAsString(record, "STREET");
                    prospectAddress.RegionCode = QueryUtils.getValueAsString(record, "REGION_CODE");
                    prospectAddress.ProvinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                    prospectAddress.ProvinceDbd = QueryUtils.getValueAsString(record, "PROVINCE_DBD");
                    prospectAddress.DistrictCode = QueryUtils.getValueAsString(record, "DISTRICT_CODE");
                    prospectAddress.SubdistrictCode = QueryUtils.getValueAsString(record, "SUBDISTRICT_CODE");
                    prospectAddress.PostCode = QueryUtils.getValueAsString(record, "POST_CODE");
                    prospectAddress.Remark = QueryUtils.getValueAsString(record, "PD_REMARK");
                    prospectAddress.MainFlag = QueryUtils.getValueAsString(record, "MAIN_FLAG");
                    prospectAddress.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                    */

                




                    o.ProspectAccount = prospectAccount;
                    o.Prospect = prospect;
                    o.ProspectContact = prospectContact;
                    o.ProspectAddress = prospectAddress;
                    //o.ProspectRecommend = prospectRecommend;


                lst.Add(o);
            }

            // Call Close when done reading.
            reader.Close();

            return lst;

        }






        public async Task<CreateProspectModel> createProspect(CreateProspectModel createProspectModel, UserProfileForBack userProfile, string language)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {


                        string foundDataFlag = await getFoundDataFlag(createProspectModel);
                        if (foundDataFlag=="Y")
                        {
                            ServiceException se = new ServiceException("E_0002", ObjectFacory.getCultureInfo(language));
                            throw se;
                        }


                        ProspectAccountModel prospectAccountModel = createProspectModel.ProspectAccountModel;
                        ProspectModel prospectModel = createProspectModel.ProspectModel;
                        ProspectAddressModel prospectAddressModel = createProspectModel.ProspectAddressModel;
                        ProspectContactModel prospectContactModel = createProspectModel.ProspectContactModel;

                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_ACCOUNT_SEQ", p);
                        var nextValPospectAccId = (int)p.Value;

                        // PROSPECT_ACCOUNT
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PROSPECT_ACCOUNT (PROSP_ACC_ID, ACC_NAME, BRAND_ID, IDENTIFY_ID, ACC_GROUP_REF, REMARK, SOURCE_TYPE, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@PROSP_ACC_ID ,@AccName, @BrandId, @IdentifyId, @AccGroupRef, @Remark, @SourceType, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_ACC_ID", nextValPospectAccId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AccName", prospectAccountModel.AccName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BrandId", prospectAccountModel.BrandId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("IdentifyId", prospectAccountModel.IdentifyId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AccGroupRef", prospectAccountModel.AccGroupRef));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Remark", prospectAccountModel.Remark));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SourceType", prospectAccountModel.SourceType));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        // PROSPECT
                        string buId = userProfile.getBuId() == 0 ? "null" : userProfile.getBuId().ToString();
                        string empId = userProfile.getEmpId();
                        string GROUP_CODE = userProfile.getAdmEmployeeGroupCode();
                        p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_SEQ", p);
                        var nextValPospectId = (int)p.Value;

                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PROSPECT (PROSPECT_ID, PROSP_ACC_ID, GROUP_CODE, BU_ID, SALE_REP_ID, PROSPECT_TYPE, MAIN_FLAG, PROSPECT_STATUS, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@PROSPECT_ID ,@PROSP_ACC_ID, @GROUP_CODE, @BU_ID, @SALE_REP_ID, '0', 'Y', '0', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", nextValPospectId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_ACC_ID", nextValPospectAccId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_CODE", GROUP_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BU_ID", buId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SALE_REP_ID", empId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        // PROSPECT_ADDRESS
                        p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_ADDRESS_SEQ", p);
                        var nextValPospectAddressId = (int)p.Value;

                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PROSPECT_ADDRESS (PROSP_ADDR_ID, PROSPECT_ID, PROSP_ACC_ID, ADDR_NO, MOO, SOI, STREET, TELL_NO, FAX_NO, LATITUDE, LONGITUDE, REGION_CODE, PROVINCE_CODE, PROVINCE_DBD, DISTRICT_CODE, SUBDISTRICT_CODE, POST_CODE, REMARK, MAIN_FLAG, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@nextValPospectAddressId ,@nextValPospectId, @nextValPospectAccId, @AddrNo, @Moo, @Soi, @Street, @TellNo, @FaxNo, @Latitude, @Longitude, @RegionCode, @ProvinceCode, @ProvinceDbd, @DistrictCode, @SubdistrictCode, @PostCode, @Remark, 'Y', 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextValPospectAddressId", nextValPospectAddressId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextValPospectId", nextValPospectId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextValPospectAccId", nextValPospectAccId));// Add New
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
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);




                        // PROSPECT_CONTACT
                        p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_CONTACT_SEQ", p);
                        var nextValPospectContactId = (int)p.Value;

                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PROSPECT_CONTACT (PROSP_CONTACT_ID, PROSPECT_ID, PROSP_ACC_ID, FIRST_NAME, LAST_NAME, PHONE_NO, FAX_NO, MOBILE_NO, EMAIL, MAIN_FLAG, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@nextValPospectContactId ,@nextValPospectId, @nextValPospectAccId, @FirstName, @LastName, @PhoneNo, @FaxNo, @MobileNo, @Email, 'Y', 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextValPospectContactId", nextValPospectContactId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextValPospectId", nextValPospectId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextValPospectAccId", nextValPospectAccId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FirstName", prospectContactModel.FirstName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LastName", prospectContactModel.LastName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PhoneNo", prospectContactModel.PhoneNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FaxNo", prospectContactModel.FaxNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("MobileNo", prospectContactModel.MobileNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Email", prospectContactModel.Email));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);





                        // PROSPECT_FEED
                        int functionTab = 1;
                        p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_FEED_SEQ", p);
                        var nextValFeedId = (int)p.Value;

                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PROSPECT_FEED (FEED_ID, PROSPECT_ID, FUNCTION_TAB, DESCRIPTION, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@FEED_ID ,@PROSPECT_ID, @FUNCTION_TAB, 'New Prospect', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("FEED_ID", nextValFeedId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", nextValPospectId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FUNCTION_TAB", functionTab));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        transaction.Commit();
                        CreateProspectModel re = new CreateProspectModel();
                        ProspectAccountModel reProspectAccountModel = new ProspectAccountModel();
                        ProspectModel reProspectModel = new ProspectModel();
                        ProspectAddressModel reProspectAddressModel = new ProspectAddressModel();
                        ProspectContactModel reProspectContactModel = new ProspectContactModel();
                        reProspectAccountModel.ProspAccId = nextValPospectAccId.ToString();
                        reProspectModel.ProspectId = nextValPospectId.ToString();
                        reProspectAddressModel.ProspAddrId = nextValPospectAddressId.ToString();
                        reProspectContactModel.ProspContactId = nextValPospectContactId.ToString();

                        re.ProspectAccountModel = reProspectAccountModel;
                        re.ProspectModel = reProspectModel;
                        re.ProspectAddressModel = reProspectAddressModel;
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


        public async Task<String> getFoundDataFlag(CreateProspectModel createProspectModel)
        {
            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select 'Y' FOUND_DATA_FLAG ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT AC ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PA on PA.PROSP_ACC_ID = AC.PROSP_ACC_ID and PA.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" where  IIF(AC.IDENTIFY_ID = '',null,AC.IDENTIFY_ID)  = @IdentifyId or (AC.ACC_NAME = @AccName and PA.SUBDISTRICT_CODE = @SubdistrictCode)  ");
                QueryUtils.addParam(command, "IdentifyId", createProspectModel.ProspectAccountModel.IdentifyId);// Add new
                QueryUtils.addParam(command, "AccName", createProspectModel.ProspectAccountModel.AccName);// Add new
                QueryUtils.addParam(command, "SubdistrictCode", createProspectModel.ProspectAddressModel.SubdistrictCode);// Add new
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







        public async Task<EntitySearchResultBase<SearchProspectSaTabCustom>> searchProspectSaTab(SearchCriteriaBase<ProspectCriteria> searchCriteria)
        {



            EntitySearchResultBase<SearchProspectSaTabCustom> searchResult = new EntitySearchResultBase<SearchProspectSaTabCustom>();
            List<SearchProspectSaTabCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                //queryBuilder.AppendFormat(" select IIF(PP.MAIN_FLAG = 'Y' and PA.CUST_CODE is null, 'Y', 'N') EDIT_GENERAL_DATA_FLAG, PA.REMARK AS PA_REMARK, PA.ACTIVE_FLAG AS PA_ACTIVE_FLAG, PA.*, PP.MAIN_FLAG AS PP_MAIN_FLAG, PP.*, PD.FAX_NO AS PD_FAX_NO, PD.REMARK AS PD_REMARK, PD.MAIN_FLAG AS PD_MAIN_FLAG, PD.ACTIVE_FLAG AS PD_ACTIVE_FLAG, PD.*, PC.MAIN_FLAG AS PC_MAIN_FLAG, PC.FAX_NO AS PC_FAX_NO, PC.ACTIVE_FLAG AS PC_ACTIVE_FLAG, PC.*, MP.PROVINCE_NAME_TH from PROSPECT_ACCOUNT PA inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID inner join PROSPECT_CONTACT PC on PC.PROSP_ACC_ID = PA.PROSP_ACC_ID and PC.MAIN_FLAG = 'Y' inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y'  left join MS_PROVINCE MP on MP.PROVINCE_CODE = PD.PROVINCE_CODE ");
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select  IIF(PP.MAIN_FLAG = 'Y' and PA.CUST_CODE is null, 'Y', 'N') EDIT_GENERAL_DATA_FLAG, PA.REMARK AS PA_REMARK, PA.ACTIVE_FLAG AS PA_ACTIVE_FLAG, PA.*, PP.MAIN_FLAG AS PP_MAIN_FLAG, PP.*, PD.FAX_NO AS PD_FAX_NO, PD.REMARK AS PD_REMARK, PD.MAIN_FLAG AS PD_MAIN_FLAG, PD.ACTIVE_FLAG AS PD_ACTIVE_FLAG, PD.*, PC.MAIN_FLAG AS PC_MAIN_FLAG, PC.FAX_NO AS PC_FAX_NO, PC.ACTIVE_FLAG AS PC_ACTIVE_FLAG, PC.*, MP.PROVINCE_NAME_TH ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT PA ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID ");
                queryBuilder.AppendFormat(" inner join PROSPECT_CONTACT PC on PC.PROSP_ACC_ID = PA.PROSP_ACC_ID and PC.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" left join MS_PROVINCE MP on MP.PROVINCE_CODE = PD.PROVINCE_CODE where 1=1 ");



                ProspectCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.ProspectId))
                    {
                        queryBuilder.AppendFormat(" and PP.PROSPECT_ID = @ProspectId ");
                        QueryUtils.addParam(command, "ProspectId", o.ProspectId);// Add new
                    }
                }


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY PA.ACC_NAME  ");
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    command.CommandText = queryBuilder.ToString();
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

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();

                using (var reader = command.ExecuteReader())
                {
                    lst = searchProspectSaTabMapRow(reader);
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }



        private List<SearchProspectSaTabCustom> searchProspectSaTabMapRow(DbDataReader reader)
        {
            List<SearchProspectSaTabCustom> lst = new List<SearchProspectSaTabCustom>();
            SearchProspectSaTabCustom o = null;
            while (reader.Read())
            {
                IDataRecord record = (IDataRecord)reader;
                o = new SearchProspectSaTabCustom();
                    o.EditGeneralDataFlag = QueryUtils.getValueAsString(record, "EDIT_GENERAL_DATA_FLAG");

                    // Account
                    o.ProspAccId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ACC_ID");
                    o.AccName = QueryUtils.getValueAsString(record, "ACC_NAME");
                    o.BrandId = QueryUtils.getValueAsDecimal(record, "BRAND_ID");
                    o.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                    o.IdentifyId = QueryUtils.getValueAsString(record, "IDENTIFY_ID");
                    o.AccGroupRef = QueryUtils.getValueAsString(record, "ACC_GROUP_REF");
                    o.AccountRemark = QueryUtils.getValueAsString(record, "PA_REMARK");
                    o.SourceType = QueryUtils.getValueAsString(record, "SOURCE_TYPE");
                    o.AccountActiveFlag = QueryUtils.getValueAsString(record, "PA_ACTIVE_FLAG");

                    // Pospect

                    o.ProspectId = QueryUtils.getValueAsDecimalRequired(record, "PROSPECT_ID");
                    o.BuId = QueryUtils.getValueAsDecimal(record, "BU_ID");
                    o.ServicesTypeId = QueryUtils.getValueAsString(record, "SERVICES_TYPE_ID");
                    o.LocTypeId = QueryUtils.getValueAsDecimal(record, "LOC_TYPE_ID");
                    o.SaleRepId = QueryUtils.getValueAsString(record, "SALE_REP_ID");
                    o.ProspectType = QueryUtils.getValueAsString(record, "PROSPECT_TYPE");
                    o.StationName = QueryUtils.getValueAsString(record, "STATION_NAME");
                    o.StationOpenFlag = QueryUtils.getValueAsString(record, "STATION_OPEN_FLAG");
                    o.ReasonCancel = QueryUtils.getValueAsString(record, "REASON_CANCEL");
                    o.BrandCateId = QueryUtils.getValueAsDecimal(record, "BRAND_CATE_ID");
                    o.BrandCateOther = QueryUtils.getValueAsString(record, "BRAND_CATE_OTHER");
                    o.AreaSquareWa = QueryUtils.getValueAsDecimal(record, "AREA_SQUARE_WA");
                    o.AreaNgan = QueryUtils.getValueAsDecimal(record, "AREA_NGAN");
                    o.AreaRai = QueryUtils.getValueAsDecimal(record, "AREA_RAI");
                    o.AreaWidthMeter = QueryUtils.getValueAsDecimal(record, "AREA_WIDTH_METER");
                    o.ShopJoint = QueryUtils.getValueAsString(record, "SHOP_JOINT");
                    o.LicenseStatus = QueryUtils.getValueAsString(record, "LICENSE_STATUS");
                    o.LicenseOther = QueryUtils.getValueAsString(record, "LICENSE_OTHER");
                    o.InterestStatus = QueryUtils.getValueAsString(record, "INTEREST_STATUS");
                    o.InterestOther = QueryUtils.getValueAsString(record, "INTEREST_OTHER");
                    o.SaleVolumeRef = QueryUtils.getValueAsString(record, "SALE_VOLUME_REF");
                    o.SaleVolume = QueryUtils.getValueAsDecimal(record, "SALE_VOLUME");
                    o.ProgressDate = QueryUtils.getValueAsDateTime(record, "PROGRESS_DATE");
                    o.TerminateDate = QueryUtils.getValueAsDateTime(record, "TERMINATE_DATE");
                    o.NearBankId = QueryUtils.getValueAsDecimal(record, "NEAR_BANK_ID");
                    o.QuotaOil = QueryUtils.getValueAsDecimal(record, "QUOTA_OIL");
                    o.QuotaLube = QueryUtils.getValueAsDecimal(record, "QUOTA_LUBE");
                    o.DispenserTotal = QueryUtils.getValueAsDecimal(record, "DISPENSER_TOTAL");
                    o.NozzleTotal = QueryUtils.getValueAsDecimal(record, "NOZZLE_TOTAL");
                    o.AddrTitleDeedNo = QueryUtils.getValueAsString(record, "ADDR_TITLE_DEED_NO");
                    o.AddrCertUtilisation = QueryUtils.getValueAsString(record, "ADDR_CERT_UTILISATION");
                    o.AddrParcelNo = QueryUtils.getValueAsString(record, "ADDR_PARCEL_NO");
                    o.AddrTambonNo = QueryUtils.getValueAsString(record, "ADDR_TAMBON_NO");
                    o.DbdCode = QueryUtils.getValueAsString(record, "DBD_CODE");
                    o.DbdCorpType = QueryUtils.getValueAsString(record, "DBD_CORP_TYPE");
                    o.DbdJuristicStatus = QueryUtils.getValueAsString(record, "DBD_JURISTIC_STATUS");
                    o.DbdRegCapital = QueryUtils.getValueAsDecimal(record, "DBD_REG_CAPITAL");
                    o.DbdTotalIncome = QueryUtils.getValueAsDecimal(record, "DBD_TOTAL_INCOME");
                    o.DbdProfitLoss = QueryUtils.getValueAsDecimal(record, "DBD_PROFIT_LOSS");
                    o.DbdTotalAsset = QueryUtils.getValueAsDecimal(record, "DBD_TOTAL_ASSET");
                    o.DbdFleetCard = QueryUtils.getValueAsString(record, "DBD_FLEET_CARD");
                    o.DbdCorpCard = QueryUtils.getValueAsString(record, "DBD_CORP_CARD");
                    o.DbdOilConsuption = QueryUtils.getValueAsString(record, "DBD_OIL_CONSUPTION");
                    o.DbdCurrentStation = QueryUtils.getValueAsString(record, "DBD_CURRENT_STATION");
                    o.DbdPayChannel = QueryUtils.getValueAsString(record, "DBD_PAY_CHANNEL");
                    o.DbdCarWheel4 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL4");
                    o.DbdCarWheel6 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL6");
                    o.DbdCarWheel8 = QueryUtils.getValueAsString(record, "DBD_CAR_WHEEL8");
                    o.DbdCaravan = QueryUtils.getValueAsString(record, "DBD_CARAVAN");
                    o.DbdCarTrailer = QueryUtils.getValueAsString(record, "DBD_CAR_TRAILER");
                    o.DbdCarContainer = QueryUtils.getValueAsString(record, "DBD_CAR_CONTAINER");
                    o.DbdOther = QueryUtils.getValueAsString(record, "DBD_OTHER");
                    o.DbdTank = QueryUtils.getValueAsString(record, "DBD_TANK");
                    o.DbdStation = QueryUtils.getValueAsString(record, "DBD_STATION");
                    o.DbdType2 = QueryUtils.getValueAsString(record, "DBD_TYPE2");
                    o.DbdMaintainCenter = QueryUtils.getValueAsString(record, "DBD_MAINTAIN_CENTER");
                    o.DbdGeneralGarage = QueryUtils.getValueAsString(record, "DBD_GENERAL_GARAGE");
                    o.DbdMaintainDept = QueryUtils.getValueAsString(record, "DBD_MAINTAIN_DEPT");
                    o.DbdRecommender = QueryUtils.getValueAsString(record, "DBD_RECOMMENDER");
                    o.DbdSale = QueryUtils.getValueAsString(record, "DBD_SALE");
                    o.DbdSaleSupport = QueryUtils.getValueAsString(record, "DBD_SALE_SUPPORT");
                    o.DbdRemark = QueryUtils.getValueAsString(record, "DBD_REMARK");
                    o.ProspectMainFlag = QueryUtils.getValueAsString(record, "PP_MAIN_FLAG");
                    o.ProspectStatus = QueryUtils.getValueAsString(record, "PROSPECT_STATUS");
                    o.DbdMachine = QueryUtils.getValueAsString(record, "DBD_MACHINE");


                // Address
                    o.ProspAddrId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ADDR_ID");
                    o.AddrNo = QueryUtils.getValueAsString(record, "ADDR_NO");
                    o.Moo = QueryUtils.getValueAsString(record, "MOO");
                    o.Soi = QueryUtils.getValueAsString(record, "SOI");
                    o.Street = QueryUtils.getValueAsString(record, "STREET");
                    o.TellNo = QueryUtils.getValueAsString(record, "TELL_NO");
                    o.AddressFaxNo = QueryUtils.getValueAsString(record, "PD_FAX_NO");
                    o.Latitude = QueryUtils.getValueAsString(record, "LATITUDE");
                    o.Longitude = QueryUtils.getValueAsString(record, "LONGITUDE");
                    o.RegionCode = QueryUtils.getValueAsString(record, "REGION_CODE");
                    o.ProvinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                    o.ProvinceDbd = QueryUtils.getValueAsString(record, "PROVINCE_DBD");
                    o.DistrictCode = QueryUtils.getValueAsString(record, "DISTRICT_CODE");
                    o.SubdistrictCode = QueryUtils.getValueAsString(record, "SUBDISTRICT_CODE");
                    o.PostCode = QueryUtils.getValueAsString(record, "POST_CODE");
                    o.AddressRemark = QueryUtils.getValueAsString(record, "PD_REMARK");
                    o.AddressMainFlag = QueryUtils.getValueAsString(record, "PD_MAIN_FLAG");
                    o.AddressActiveFlag = QueryUtils.getValueAsString(record, "PD_ACTIVE_FLAG");

                    // Contact
                    o.ProspContactId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_CONTACT_ID");
                    o.FirstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                    o.LastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                    o.PhoneNo = QueryUtils.getValueAsString(record, "PHONE_NO");
                    o.ContactFaxNo = QueryUtils.getValueAsString(record, "PC_FAX_NO");
                    o.MobileNo = QueryUtils.getValueAsString(record, "MOBILE_NO");
                    o.Email = QueryUtils.getValueAsString(record, "EMAIL");
                    o.ContactMainFlag = QueryUtils.getValueAsString(record, "PC_MAIN_FLAG");
                    o.ContactActiveFlag = QueryUtils.getValueAsString(record, "PC_ACTIVE_FLAG");

                    //PROVINCE
                    o.ProvinceNameTh = QueryUtils.getValueAsString(record, "PROVINCE_NAME_TH");



                lst.Add(o);
            }

            // Call Close when done reading.
            reader.Close();

            return lst;

        }

        public async Task<EntitySearchResultBase<GetProspectForCreatePlanTripAdHocCustom>> getProspectForCreatePlanTripAdHoc(SearchCriteriaBase<GetProspectForCreatePlanTripAdHocCriteria> searchCriteria, UserProfileForBack userProfile)
        {

            EntitySearchResultBase<GetProspectForCreatePlanTripAdHocCustom> searchResult = new EntitySearchResultBase<GetProspectForCreatePlanTripAdHocCustom>();
            List<GetProspectForCreatePlanTripAdHocCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                GetProspectForCreatePlanTripAdHocCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                /*queryBuilder.AppendFormat(" select PA.*, PD.LATITUDE,PD.LONGITUDE ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT PA ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID and PP.PROSPECT_STATUS not in ('3','4','5') ");
                queryBuilder.AppendFormat(" inner join(     ");
                queryBuilder.AppendFormat("     select distinct EMP_ID ");
                queryBuilder.AppendFormat("     from ORG_SALE_TERRITORY OST ");
                queryBuilder.AppendFormat("     where exists(");
                queryBuilder.AppendFormat("         select ST.TERRITORY_ID ");
                queryBuilder.AppendFormat("         from ORG_TERRITORY T ");
                queryBuilder.AppendFormat("         inner join ORG_SALE_TERRITORY ST on ST.TERRITORY_ID = T.TERRITORY_ID ");
                queryBuilder.AppendFormat("         where ST.EMP_ID = @EmpId ");
                queryBuilder.AppendFormat("         and ST.TERRITORY_ID = OST.TERRITORY_ID) ");
                queryBuilder.AppendFormat("     ) T on T.EMP_ID = PP.SALE_REP_ID ");
                queryBuilder.AppendFormat(" where not exists (select 1 from PLAN_TRIP_PROSPECT TP where TP.PROSP_ID = PP.PROSPECT_ID and TP.PLAN_TRIP_ID = @PlanTripId) ");
                queryBuilder.AppendFormat(" union  ");
                queryBuilder.AppendFormat(" select PA.*, PD.LATITUDE,PD.LONGITUDE ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT PA ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID and PP.PROSPECT_STATUS not in ('3','4','5') ");
                queryBuilder.AppendFormat(" inner join PROSPECT_DEDICATE_TERT DT on DT.PROSPECT_ID = PP.PROSPECT_ID ");
                queryBuilder.AppendFormat(" where not exists (select 1 from PLAN_TRIP_PROSPECT TP where TP.PROSP_ID = PP.PROSPECT_ID and TP.PLAN_TRIP_ID = @PlanTripId) ");
                queryBuilder.AppendFormat(" and exists( ");
                queryBuilder.AppendFormat("     select ST.TERRITORY_ID ");
                queryBuilder.AppendFormat("     from ORG_TERRITORY T ");
                queryBuilder.AppendFormat("     inner join ORG_SALE_TERRITORY ST on ST.TERRITORY_ID = T.TERRITORY_ID ");
                queryBuilder.AppendFormat("     where ST.EMP_ID = @EmpId ");
                queryBuilder.AppendFormat("     and ST.TERRITORY_ID = DT.TERRITORY_ID) ");
                QueryUtils.addParam(command, "EmpId", userProfile.getEmpId());
                QueryUtils.addParam(command, "PlanTripId", o.PlanTripId);*/

                /*string territoryIdStr = "";
                if (userProfile.OrgTerritory != null && userProfile.OrgTerritory.data != null && userProfile.OrgTerritory.data.Count != 0)
                {
                    List<string> territoryIdList = new List<string>();
                    foreach(OrgTerritory t in userProfile.OrgTerritory.data)
                    {
                        territoryIdList.Add(t.TerritoryId.ToString());
                    }
                    territoryIdStr = String.Join(",", territoryIdList);
                }*/


                //queryBuilder.AppendFormat(" select PA.*,PD.LATITUDE,PD.LONGITUDE,PP.PROSPECT_ID ");
                queryBuilder.AppendFormat(" select PA.ACC_NAME,IIF(PP.PROSPECT_TYPE=2,PA.CUST_CODE,cast(PP.PROSPECT_ID as varchar))  CUST_CODE,PA.PROSP_ACC_ID,PD.LATITUDE,PD.LONGITUDE,PP.PROSPECT_ID ");
                queryBuilder.AppendFormat(" ,TRIM(IIF(PD.ADDR_NO is null,'',PD.ADDR_NO+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.MOO is null,'',PD.MOO+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.SOI is null,'',PD.SOI+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.STREET is null,'',PD.STREET +' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.SUBDISTRICT_CODE is null,'',S.SUBDISTRICT_NAME_TH+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.DISTRICT_CODE is null,'',D.DISTRICT_NAME_TH+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.PROVINCE_CODE is null,'',P.PROVINCE_NAME_TH)) ADDRESS_FULLNM ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT PA ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID and PP.PROSPECT_STATUS not in ('3','4','5') ");
                queryBuilder.AppendFormat(" left join MS_PROVINCE P on P.PROVINCE_CODE = PD.PROVINCE_CODE ");
                queryBuilder.AppendFormat(" left join MS_DISTRICT D on D.DISTRICT_CODE = PD.DISTRICT_CODE ");
                queryBuilder.AppendFormat(" left join MS_SUBDISTRICT S on S.SUBDISTRICT_CODE = PD.SUBDISTRICT_CODE ");
                queryBuilder.AppendFormat("                  where not exists (select 1 from PLAN_TRIP_PROSPECT TP where TP.PROSP_ID = PP.PROSPECT_ID and TP.PLAN_TRIP_ID = @PlanTripId) ");
                //queryBuilder.AppendFormat(" and exists (select 1 from ORG_SALE_TERRITORY OST where OST.EMP_ID = PP.SALE_REP_ID and OST.TERRITORY_ID IN (" + QueryUtils.getParamIn("territoryIdStr", territoryIdStr) + ")) ");
                queryBuilder.AppendFormat(" and exists (select 1 from ORG_SALE_GROUP OSG where OSG.GROUP_CODE = PP.GROUP_CODE and OSG.TERRITORY_ID = @orgSaleGroup_territoryId) ");
                queryBuilder.AppendFormat(" union  ");
                //queryBuilder.AppendFormat(" select PA.*,PD.LATITUDE,PD.LONGITUDE,PP.PROSPECT_ID ");
                queryBuilder.AppendFormat(" select PA.ACC_NAME,IIF(PP.PROSPECT_TYPE=2,PA.CUST_CODE,cast(PP.PROSPECT_ID as varchar)) CUST_CODE,PA.PROSP_ACC_ID,PD.LATITUDE,PD.LONGITUDE,PP.PROSPECT_ID ");
                queryBuilder.AppendFormat(" ,TRIM(IIF(PD.ADDR_NO is null,'',PD.ADDR_NO+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.MOO is null,'',PD.MOO+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.SOI is null,'',PD.SOI+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.STREET is null,'',PD.STREET +' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.SUBDISTRICT_CODE is null,'',S.SUBDISTRICT_NAME_TH+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.DISTRICT_CODE is null,'',D.DISTRICT_NAME_TH+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.PROVINCE_CODE is null,'',P.PROVINCE_NAME_TH)) ADDRESS_FULLNM ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT PA ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID and PP.PROSPECT_STATUS not in ('3','4','5') ");
                queryBuilder.AppendFormat(" inner join PROSPECT_DEDICATE_TERT DT on DT.PROSPECT_ID = PP.PROSPECT_ID ");
                queryBuilder.AppendFormat(" left join MS_PROVINCE P on P.PROVINCE_CODE = PD.PROVINCE_CODE ");
                queryBuilder.AppendFormat(" left join MS_DISTRICT D on D.DISTRICT_CODE = PD.DISTRICT_CODE ");
                queryBuilder.AppendFormat(" left join MS_SUBDISTRICT S on S.SUBDISTRICT_CODE = PD.SUBDISTRICT_CODE ");
                queryBuilder.AppendFormat(" where not exists (select 1 from PLAN_TRIP_PROSPECT TP where TP.PROSP_ID = PP.PROSPECT_ID and TP.PLAN_TRIP_ID = @PlanTripId) ");
                //queryBuilder.AppendFormat("     and DT.TERRITORY_ID in (" + QueryUtils.getParamIn("territoryIdStr", territoryIdStr) + ") ");
                queryBuilder.AppendFormat(" and DT.TERRITORY_ID = @orgSaleGroup_territoryId ");
                QueryUtils.addParam(command, "PlanTripId", o.PlanTripId);
                QueryUtils.addParam(command, "orgSaleGroup_territoryId", userProfile.getSaleGroupSaleOffice().TerritoryId);
                //QueryUtils.addParamIn(command, "territoryIdStr", territoryIdStr);
                // For Paging
                queryBuilder.AppendFormat(" ORDER BY PA.PROSP_ACC_ID  ");
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

                    List<GetProspectForCreatePlanTripAdHocCustom> dataRecordList = new List<GetProspectForCreatePlanTripAdHocCustom>();
                    GetProspectForCreatePlanTripAdHocCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new GetProspectForCreatePlanTripAdHocCustom();

                        /*
                        dataRecord.ProspAccId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ACC_ID");
                        dataRecord.AccName = QueryUtils.getValueAsString(record, "ACC_NAME");
                        dataRecord.BrandId = QueryUtils.getValueAsDecimal(record, "BRAND_ID");
                        dataRecord.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        dataRecord.IdentifyId = QueryUtils.getValueAsString(record, "IDENTIFY_ID");
                        dataRecord.AccGroupRef = QueryUtils.getValueAsString(record, "ACC_GROUP_REF");
                        dataRecord.Remark = QueryUtils.getValueAsString(record, "REMARK");
                        dataRecord.SourceType = QueryUtils.getValueAsString(record, "SOURCE_TYPE");
                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");
                        dataRecord.Latitude = QueryUtils.getValueAsString(record, "LATITUDE");
                        dataRecord.Longitude = QueryUtils.getValueAsString(record, "LONGITUDE");
                        dataRecord.ProspectId = QueryUtils.getValueAsString(record, "PROSPECT_ID");*/



                        dataRecord.AccName = QueryUtils.getValueAsString(record, "ACC_NAME");
                        dataRecord.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        dataRecord.ProspAccId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ACC_ID");
                        dataRecord.Latitude = QueryUtils.getValueAsString(record, "LATITUDE");
                        dataRecord.Longitude = QueryUtils.getValueAsString(record, "LONGITUDE");
                        dataRecord.ProspectId = QueryUtils.getValueAsString(record, "PROSPECT_ID");
                        dataRecord.AddressFullnm = QueryUtils.getValueAsString(record, "ADDRESS_FULLNM");

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





        public async Task<EntitySearchResultBase<GetProspectForCreatePlanTripCustom>> getProspectForCreatePlanTrip(SearchCriteriaBase<GetProspectForCreatePlanTripCriteria> searchCriteria, UserProfileForBack userProfile)
        {

            EntitySearchResultBase<GetProspectForCreatePlanTripCustom> searchResult = new EntitySearchResultBase<GetProspectForCreatePlanTripCustom>();
            List<GetProspectForCreatePlanTripCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                GetProspectForCreatePlanTripCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();

                /*string territoryIdStr = "";
                if (userProfile.OrgTerritory != null && userProfile.OrgTerritory.data != null && userProfile.OrgTerritory.data.Count != 0)
                {
                    List<string> territoryIdList = new List<string>();
                    foreach (OrgTerritory t in userProfile.OrgTerritory.data)
                    {
                        territoryIdList.Add(t.TerritoryId.ToString());
                    }
                    territoryIdStr = String.Join(",", territoryIdList);
                }*/

                //queryBuilder.AppendFormat(" select PA.*,PD.LATITUDE,PD.LONGITUDE,PP.PROSPECT_ID ");
                queryBuilder.AppendFormat(" select PA.ACC_NAME,IIF(PP.PROSPECT_TYPE=2,PA.CUST_CODE,cast(PP.PROSPECT_ID as varchar)) CUST_CODE,PA.PROSP_ACC_ID,PD.LATITUDE,PD.LONGITUDE,PP.PROSPECT_ID ");
                queryBuilder.AppendFormat(" ,TRIM(IIF(PD.ADDR_NO is null,'',PD.ADDR_NO+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.MOO is null,'',PD.MOO+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.SOI is null,'',PD.SOI+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.STREET is null,'',PD.STREET +' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.SUBDISTRICT_CODE is null,'',S.SUBDISTRICT_NAME_TH+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.DISTRICT_CODE is null,'',D.DISTRICT_NAME_TH+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.PROVINCE_CODE is null,'',P.PROVINCE_NAME_TH)) ADDRESS_FULLNM ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT PA ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID and PP.PROSPECT_STATUS not in ('3','4','5') ");
                queryBuilder.AppendFormat(" left join MS_PROVINCE P on P.PROVINCE_CODE = PD.PROVINCE_CODE ");
                queryBuilder.AppendFormat(" left join MS_DISTRICT D on D.DISTRICT_CODE = PD.DISTRICT_CODE ");
                queryBuilder.AppendFormat(" left join MS_SUBDISTRICT S on S.SUBDISTRICT_CODE = PD.SUBDISTRICT_CODE ");
                //queryBuilder.AppendFormat("                 where exists (select 1 from ORG_SALE_TERRITORY OST where OST.EMP_ID = PP.SALE_REP_ID and OST.TERRITORY_ID IN (" + QueryUtils.getParamIn("territoryId", territoryIdStr) + ")) ");
                queryBuilder.AppendFormat("  where exists (select 1 from ORG_SALE_GROUP OSG where OSG.GROUP_CODE = PP.GROUP_CODE and OSG.TERRITORY_ID = @orgSaleGroup_territoryId) ");
                queryBuilder.AppendFormat(" union  ");
                //queryBuilder.AppendFormat(" select PA.*,PD.LATITUDE,PD.LONGITUDE,PP.PROSPECT_ID ");
                queryBuilder.AppendFormat(" select PA.ACC_NAME,IIF(PP.PROSPECT_TYPE=2,PA.CUST_CODE,cast(PP.PROSPECT_ID as varchar)) CUST_CODE,PA.PROSP_ACC_ID,PD.LATITUDE,PD.LONGITUDE,PP.PROSPECT_ID ");
                queryBuilder.AppendFormat(" ,TRIM(IIF(PD.ADDR_NO is null,'',PD.ADDR_NO+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.MOO is null,'',PD.MOO+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.SOI is null,'',PD.SOI+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.STREET is null,'',PD.STREET +' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.SUBDISTRICT_CODE is null,'',S.SUBDISTRICT_NAME_TH+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.DISTRICT_CODE is null,'',D.DISTRICT_NAME_TH+' ') + ");
                queryBuilder.AppendFormat(" IIF(PD.PROVINCE_CODE is null,'',P.PROVINCE_NAME_TH)) ADDRESS_FULLNM ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT PA ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID and PP.PROSPECT_STATUS not in ('3','4','5') ");
                queryBuilder.AppendFormat(" inner join PROSPECT_DEDICATE_TERT DT on DT.PROSPECT_ID = PP.PROSPECT_ID ");
                queryBuilder.AppendFormat(" left join MS_PROVINCE P on P.PROVINCE_CODE = PD.PROVINCE_CODE ");
                queryBuilder.AppendFormat(" left join MS_DISTRICT D on D.DISTRICT_CODE = PD.DISTRICT_CODE ");
                queryBuilder.AppendFormat(" left join MS_SUBDISTRICT S on S.SUBDISTRICT_CODE = PD.SUBDISTRICT_CODE ");
                //queryBuilder.AppendFormat(" where DT.TERRITORY_ID in ("+ QueryUtils.getParamIn("territoryId", territoryIdStr)+") ");
                queryBuilder.AppendFormat(" where DT.TERRITORY_ID = @orgSaleGroup_territoryId ");
                QueryUtils.addParam(command, "orgSaleGroup_territoryId", userProfile.getSaleGroupSaleOffice().TerritoryId);
                //QueryUtils.addParamIn(command, "territoryId", territoryIdStr);


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY PA.PROSP_ACC_ID  ");
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

                    List<GetProspectForCreatePlanTripCustom> dataRecordList = new List<GetProspectForCreatePlanTripCustom>();
                    GetProspectForCreatePlanTripCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new GetProspectForCreatePlanTripCustom();


                        /*dataRecord.ProspAccId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ACC_ID");
                        dataRecord.AccName = QueryUtils.getValueAsString(record, "ACC_NAME");
                        dataRecord.BrandId = QueryUtils.getValueAsDecimal(record, "BRAND_ID");
                        dataRecord.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        dataRecord.IdentifyId = QueryUtils.getValueAsString(record, "IDENTIFY_ID");
                        dataRecord.AccGroupRef = QueryUtils.getValueAsString(record, "ACC_GROUP_REF");
                        dataRecord.Remark = QueryUtils.getValueAsString(record, "REMARK");
                        dataRecord.Latitude = QueryUtils.getValueAsString(record, "LATITUDE");
                        dataRecord.Longitude = QueryUtils.getValueAsString(record, "LONGITUDE");
                        dataRecord.SourceType = QueryUtils.getValueAsString(record, "SOURCE_TYPE");
                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");
                        dataRecord.ProspectId = QueryUtils.getValueAsString(record, "PROSPECT_ID");*/


                        dataRecord.AccName = QueryUtils.getValueAsString(record, "ACC_NAME");
                        dataRecord.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        dataRecord.ProspAccId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ACC_ID");
                        dataRecord.Latitude = QueryUtils.getValueAsString(record, "LATITUDE");
                        dataRecord.Longitude = QueryUtils.getValueAsString(record, "LONGITUDE");
                        dataRecord.ProspectId = QueryUtils.getValueAsString(record, "PROSPECT_ID");
                        dataRecord.AddressFullnm = QueryUtils.getValueAsString(record, "ADDRESS_FULLNM");

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



        // Dell Prospect
        public async Task<int> delProspect(DeleteProspectModel model, UserProfileForBack userProfile)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();

                        // ตาม spec เดิม ให้ update เป็น -1 แต่พอลง Database แล้วมันเป็น * พอเอาไป where PROSPECT_STATUS > -1  มันตาย เพราะ convert เป็น Number ไม่ได้
                        //queryBuilder.AppendFormat(" update PROSPECT set PROSPECT_STATUS = -1,[UPDATE_USER] = @User, [UPDATE_DTM] = dbo.GET_SYSDATETIME()  ");
                        // แก้ให้เป็น null ไปก่อน
                        queryBuilder.AppendFormat(" update PROSPECT set PROSPECT_STATUS = null,[UPDATE_USER] = @User, [UPDATE_DTM] = dbo.GET_SYSDATETIME()  ");
                        queryBuilder.AppendFormat(" where PROSPECT_ID = @PROSPECT_ID ");

                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", model.ProspectId));// Add
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", userProfile.getUserName()));// Add NewNew
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        return numberOfRowInserted;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        // Dell PlanTripProspectAdHoc
        public async Task<int> delPlanTripProspectAdHoc(DeletePlanTripProspectAdHocModel model)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" delete PLAN_TRIP_TASK ");
                        queryBuilder.AppendFormat(" where PLAN_TRIP_PROSP_ID = @PLAN_TRIP_PROSP_ID ");

                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_PROSP_ID", model.PlanTripProspId));// Add
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);



                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();

                        queryBuilder.AppendFormat(" delete PLAN_TRIP_PROSPECT ");
                        queryBuilder.AppendFormat(" where PLAN_TRIP_PROSP_ID = @PLAN_TRIP_PROSP_ID ");

                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_PROSP_ID", model.PlanTripProspId));// Add
                        queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        transaction.Commit();
                        return numberOfRowInserted;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }




        // Dell PlanTripTaskAdHoc
        public async Task<int> delPlanTripTaskAdHoc(DeletePlanTripTaskAdHocModel model)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();

                        queryBuilder.AppendFormat(" delete PLAN_TRIP_TASK ");
                        queryBuilder.AppendFormat(" where PLAN_TRIP_TASK_ID = @PLAN_TRIP_TASK_ID ");

                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_TASK_ID", model.PlanTripTaskId));// Add
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        return numberOfRowInserted;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }



    }
}





