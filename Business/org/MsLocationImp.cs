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
using MyFirstAzureWebApp.Models.ms;

namespace MyFirstAzureWebApp.Business.org
{

    public class MsLocationImp : IMsLocation
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<SearchLocationCustom>> Search(MsLocationSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                MsLocationCriteria criteria = searchCriteria.model;
                var queryCommane = (from lo in context.MsLocation
                                    join lt in context.MsLocationType
                                    on lo.LocTypeId equals lt.LocTypeId
                                    join pv in context.MsProvince
                                    on lo.ProvinceCode equals pv.ProvinceCode

                                    where ((criteria.LocNameTh == null ? 1 == 1 : lo.LocNameTh.Contains(criteria.LocNameTh)))
                                    where ((criteria.LocTypeId == null ? 1 == 1 : lo.LocTypeId == Convert.ToDecimal(criteria.LocTypeId)))
                                    where ((criteria.ProvinceCode == null ? 1 == 1 : lo.ProvinceCode == criteria.ProvinceCode))
                                    where ((criteria.ActiveFlag == null ? 1 == 1 : lo.ActiveFlag == criteria.ActiveFlag))
                                    orderby (searchCriteria.searchOrder == 1 ? lo.UpdateDtm : lo.UpdateDtm ) descending
                                    select new
                                    {
                                        LocTypeId = lt.LocTypeId,
                                        LocTypeCode = lt.LocTypeCode,
                                        LocTypeNameTh = lt.LocTypeNameTh,
                                        LocTypeNameEn = lt.LocTypeNameEn,
                                        LocId = lo.LocId,
                                        ActiveFlag = lo.ActiveFlag,
                                        LocCode = lo.LocCode,
                                        LocNameTh = lo.LocNameTh,
                                        LocNameEn = lo.LocNameEn,
                                        ProvinceCode = lo.ProvinceCode,
                                        Latitude = lo.Latitude,
                                        Longitude = lo.Longitude,
                                        CreateUser = lo.CreateUser,
                                        CreateDtm = lo.CreateDtm,
                                        UpdateUser = lo.UpdateUser,
                                        UpdateDtm = lo.UpdateDtm,
                                        CountryCode = pv.CountryCode,
                                        RegionCode = pv.RegionCode,
                                        ProvinceNameTh = pv.ProvinceNameTh,
                                        ProvinceNameEn = pv.ProvinceNameEn

                                    });
                //}).ToList();
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<SearchLocationCustom> searchResult = new EntitySearchResultBase<SearchLocationCustom>();
                searchResult.totalRecords = query.Count();



                List<SearchLocationCustom> saleLst = new List<SearchLocationCustom>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    SearchLocationCustom s = new SearchLocationCustom();
                    s.LocTypeId = item.LocTypeId;
                    s.LocTypeCode = item.LocTypeCode;
                    s.LocTypeNameTh = item.LocTypeNameTh;
                    s.LocTypeNameEn = item.LocTypeNameEn;
                    s.LocId = item.LocId;
                    s.ActiveFlag = item.ActiveFlag;
                    s.LocCode = item.LocCode;
                    s.LocNameTh = item.LocNameTh;
                    s.LocNameEn = item.LocNameEn;
                    s.ProvinceCode = item.ProvinceCode;
                    s.Latitude = item.Latitude;
                    s.Longitude = item.Longitude;
                    s.CreateUser = item.CreateUser;
                    s.CreateDtm = item.CreateDtm;
                    s.UpdateUser = item.UpdateUser;
                    s.UpdateDtm = item.UpdateDtm;
                    s.CountryCode = item.CountryCode;
                    s.RegionCode = item.RegionCode;
                    s.ProvinceNameTh = item.ProvinceNameTh;
                    s.ProvinceNameEn = item.ProvinceNameEn;
                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;
            }

        }



        public async Task<MsLocation> Add(MsLocationModel msLocationModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for MS_LOCATION_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  MS_LOCATION (LOC_ID, LOC_TYPE_ID, LOC_CODE, LOC_NAME_TH, LOC_NAME_EN, PROVINCE_CODE, LATITUDE, LONGITUDE, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, @LocTypeId, RIGHT('00000' + CAST(ISNULL((MAX(CAST(LOC_CODE AS INT)) + 1),1) AS VARCHAR), 5), @LocNameTh, @LocNameEn, @ProvinceCode, @Latitude, @Longitude, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from MS_LOCATION  ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocTypeId", msLocationModel.LocTypeId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocNameTh", msLocationModel.LocNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocNameEn", msLocationModel.LocNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProvinceCode", msLocationModel.ProvinceCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Latitude", msLocationModel.Latitude));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Longitude", msLocationModel.Longitude));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", msLocationModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        MsLocation re = new MsLocation();
                        re.LocId = nextVal;
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


        public async Task<int> Update(MsLocationModel msLocationModel)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_LOCATION SET LOC_TYPE_ID = @LocTypeId, LOC_NAME_TH = @LocNameTh,  LOC_NAME_EN = @LocNameEn,  PROVINCE_CODE = @ProvinceCode,  LATITUDE = @Latitude,  LONGITUDE = @Longitude, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE LOC_ID=@LocId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocTypeId", msLocationModel.LocTypeId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocNameTh", msLocationModel.LocNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocNameEn", msLocationModel.LocNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProvinceCode", msLocationModel.ProvinceCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Latitude", msLocationModel.Latitude));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Longitude", msLocationModel.Longitude));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", msLocationModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", msLocationModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocId", msLocationModel.LocId));// Add New
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

        public async Task<int> DeleteUpdate(MsLocationModel msLocationModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_LOCATION SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE LOC_ID=@LOC_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", msLocationModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LOC_ID", msLocationModel.LocId));// Add New
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
