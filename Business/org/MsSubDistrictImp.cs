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
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.Entity.custom;
using System.Data;
using MyFirstAzureWebApp.exception;

namespace MyFirstAzureWebApp.Business.org
{

    public class MsSubDistrictImp : IMsSubDistrict
    {
        private Logger log = LogManager.GetCurrentClassLogger();



        public async Task<EntitySearchResultBase<SearchSubDistrictCustom>> Search(SearchCriteriaBase<MsSubDistrictCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchSubDistrictCustom> searchResult = new EntitySearchResultBase<SearchSubDistrictCustom>();
            List<SearchSubDistrictCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                MsSubDistrictCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select SD.ACTIVE_FLAG as SD_ACTIVE_FLAG, SD.CREATE_USER AS SD_CREATE_USER, SD.CREATE_DTM AS SD_CREATE_DTM, SD.UPDATE_USER AS SD_UPDATE_USER, SD.UPDATE_DTM AS SD_UPDATE_DTM, P.*,D.*,SD.*,SDS.SUBDISTRICT_SOM_ID  from MS_SUBDISTRICT SD ");
                queryBuilder.AppendFormat(" left join MS_SUBDISTRICT_SOM SDS on SDS.SUBDISTRICT_CODE = SD.SUBDISTRICT_CODE ");
                queryBuilder.AppendFormat(" left join MS_DISTRICT D on D.DISTRICT_CODE = SDS.DISTRICT_CODE ");
                queryBuilder.AppendFormat(" left join MS_PROVINCE P on P.PROVINCE_CODE = D.PROVINCE_CODE  ");
                queryBuilder.AppendFormat(" where 1=1   ");
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.SubdistrictNameTh))
                    {
                        queryBuilder.AppendFormat(" and SD.SUBDISTRICT_NAME_TH like @SubdistrictNameTh  ");
                        QueryUtils.addParamLike(command, "SubdistrictNameTh", o.SubdistrictNameTh);// Add new
                    }


                    if (!String.IsNullOrEmpty(o.DistrictCode))
                    {
                        queryBuilder.AppendFormat(" and D.DISTRICT_CODE  = @DistrictCode  ");
                        QueryUtils.addParam(command, "DistrictCode", o.DistrictCode);// Add new
                    }


                    if (!String.IsNullOrEmpty(o.SubdistrictCode))
                    {
                        queryBuilder.AppendFormat(" and SD.SUBDISTRICT_CODE  = @SubdistrictCode  ");
                        QueryUtils.addParam(command, "SubdistrictCode", o.SubdistrictCode);// Add new
                    }

                    if (!String.IsNullOrEmpty(o.ActiveFlag))
                    {
                        queryBuilder.AppendFormat(" and SD.ACTIVE_FLAG  = @ActiveFlag  ");
                        QueryUtils.addParam(command, "ActiveFlag", o.ActiveFlag);// Add new
                    }


                }



                // For Paging
                if (searchCriteria.searchOrder == 1)
                {
                    queryBuilder.AppendFormat(" ORDER BY SD.UPDATE_DTM DESC  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY SD.UPDATE_DTM DESC  ");
                }
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

                    List<SearchSubDistrictCustom> dataRecordList = new List<SearchSubDistrictCustom>();
                    SearchSubDistrictCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchSubDistrictCustom();


                        dataRecord.ProvinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                        dataRecord.CountryCode = QueryUtils.getValueAsString(record, "COUNTRY_CODE");
                        dataRecord.RegionCode = QueryUtils.getValueAsString(record, "REGION_CODE");
                        dataRecord.ProvinceNameTh = QueryUtils.getValueAsString(record, "PROVINCE_NAME_TH");
                        dataRecord.ProvinceNameEn = QueryUtils.getValueAsString(record, "PROVINCE_NAME_EN");
                        dataRecord.DistrictCode = QueryUtils.getValueAsString(record, "DISTRICT_CODE");
                        //dataRecord.ProvinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                        dataRecord.DistrictNameTh = QueryUtils.getValueAsString(record, "DISTRICT_NAME_TH");
                        dataRecord.DistrictNameEn = QueryUtils.getValueAsString(record, "DISTRICT_NAME_EN");
                        dataRecord.SubdistrictCode = QueryUtils.getValueAsString(record, "SUBDISTRICT_CODE");
                        //dataRecord.DistrictCode = QueryUtils.getValueAsString(record, "DISTRICT_CODE");
                        dataRecord.SubdistrictNameTh = QueryUtils.getValueAsString(record, "SUBDISTRICT_NAME_TH");
                        dataRecord.SubdistrictNameEn = QueryUtils.getValueAsString(record, "SUBDISTRICT_NAME_EN");


                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "SD_ACTIVE_FLAG");
                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "SD_CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "SD_CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "SD_UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "SD_UPDATE_DTM");

                        dataRecord.SubdistrictSomId = QueryUtils.getValueAsString(record, "SUBDISTRICT_SOM_ID");
                        

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





        public async Task<EntitySearchResultBase<SearchMsSubDistrictCustom>> searchMsSubDistrict(SearchCriteriaBase<SearchMsSubDistrictCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchMsSubDistrictCustom> searchResult = new EntitySearchResultBase<SearchMsSubDistrictCustom>();
            List<SearchMsSubDistrictCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchMsSubDistrictCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select SD.*");
                queryBuilder.AppendFormat(" from MS_SUBDISTRICT SD  ");
                queryBuilder.AppendFormat(" where 1= 1  ");
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.SubdistrictNameTh))
                    {
                        queryBuilder.AppendFormat(" and SD.SUBDISTRICT_NAME_TH like @SubdistrictNameTh  ");
                        QueryUtils.addParamLike(command, "SubdistrictNameTh", o.SubdistrictNameTh);// Add new
                    }

                    if (!String.IsNullOrEmpty(o.SubdistrictCode))
                    {
                        queryBuilder.AppendFormat(" and SD.SUBDISTRICT_CODE  = @SubdistrictCode  ");
                        QueryUtils.addParam(command, "SubdistrictCode", o.SubdistrictCode);// Add new
                    }

                    if (!String.IsNullOrEmpty(o.ActiveFlag))
                    {
                        queryBuilder.AppendFormat(" and SD.ACTIVE_FLAG  = @ActiveFlag  ");
                        QueryUtils.addParam(command, "ActiveFlag", o.ActiveFlag);// Add new
                    }


                }



                // For Paging
                if (searchCriteria.searchOrder == 1)
                {
                    queryBuilder.AppendFormat(" ORDER BY SD.UPDATE_DTM DESC  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY SD.UPDATE_DTM DESC  ");
                }
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

                    List<SearchMsSubDistrictCustom> dataRecordList = new List<SearchMsSubDistrictCustom>();
                    SearchMsSubDistrictCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchMsSubDistrictCustom();


                        dataRecord.SubdistrictCode = QueryUtils.getValueAsString(record, "SUBDISTRICT_CODE");
                        dataRecord.SubdistrictNameTh = QueryUtils.getValueAsString(record, "SUBDISTRICT_NAME_TH");
                        dataRecord.SubdistrictNameEn = QueryUtils.getValueAsString(record, "SUBDISTRICT_NAME_EN");


                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");



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



        


        
        public async Task<int> updSubDistrictByDistrictCode(MsSubDistrictModel msSubDistrictModel, string language)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {


                        List<SelectMsSubdistricSomCustom> msDistricLst = await getMsSubdistric(msSubDistrictModel.DistrictCode, msSubDistrictModel.SubdistrictCode);
                        string caseStmt = "";
                        if(msDistricLst==null || msDistricLst.Count == 0)
                        {
                            if (!String.IsNullOrEmpty(msSubDistrictModel.SubdistrictSomId))
                            {
                                caseStmt = "U";
                            }
                            else
                            {
                                caseStmt = "I";
                            }
                        }
                        else
                        {
                            if (msDistricLst.ElementAt(0).subdistrictSomId.Equals(msSubDistrictModel.SubdistrictSomId)){
                                caseStmt = "U";
                            }
                            else
                            {
                                if (msDistricLst.ElementAt(0).activeFlag.Equals("Y"))
                                {
                                   Exception e = new ServiceException("W_0014", ObjectFacory.getCultureInfo(language));
                                    throw e;
                                }
                                else
                                {
                                    caseStmt = "UU";
                                }
                            }
                        }

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        int numberOfRowInserted = 0;
                        string queryStr = "";
                        switch (caseStmt)
                        {
                            case "I":
                                sqlParameters = new List<SqlParameter>();// Add New
                                queryBuilder = new StringBuilder();
                                queryBuilder.AppendFormat(" INSERT INTO MS_SUBDISTRICT_SOM ([SUBDISTRICT_SOM_ID], [SUBDISTRICT_CODE], [DISTRICT_CODE], [ACTIVE_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                                queryBuilder.AppendFormat("VALUES(NEXT VALUE FOR MS_SUBDISTRICT_SOM_SEQ, @SUBDISTRICT_CODE, @DISTRICT_CODE, 'Y', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME()) ");
                                sqlParameters.Add(QueryUtils.addSqlParameter("SUBDISTRICT_CODE", msSubDistrictModel.SubdistrictCode));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("DISTRICT_CODE", msSubDistrictModel.DistrictCode));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("USER", msSubDistrictModel.getUserName()));// Add New
                                queryStr = queryBuilder.ToString();
                                queryStr = QueryUtils.cutStringNull(queryStr);
                                log.Debug("Query:" + queryStr);
                                Console.WriteLine("Query:" + queryStr);
                                numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                                break;
                            case "U":
                                sqlParameters = new List<SqlParameter>();// Add New
                                queryBuilder = new StringBuilder();
                                queryBuilder.AppendFormat(" UPDATE MS_SUBDISTRICT_SOM ");
                                queryBuilder.AppendFormat(" SET DISTRICT_CODE = @DISTRICT_CODE , SUBDISTRICT_CODE = @SUBDISTRICT_CODE,ACTIVE_FLAG = 'Y' ");
                                queryBuilder.AppendFormat("     ,UPDATE_DTM = dbo.GET_SYSDATETIME() ");
                                queryBuilder.AppendFormat("     ,UPDATE_USER = @USER ");
                                queryBuilder.AppendFormat(" where SUBDISTRICT_SOM_ID = @SUBDISTRICT_SOM_ID ");
                                sqlParameters.Add(QueryUtils.addSqlParameter("SUBDISTRICT_CODE", msSubDistrictModel.SubdistrictCode));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("DISTRICT_CODE", msSubDistrictModel.DistrictCode));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("USER", msSubDistrictModel.getUserName()));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("SUBDISTRICT_SOM_ID", msSubDistrictModel.SubdistrictSomId));// Add New
                                queryStr = queryBuilder.ToString();
                                queryStr = QueryUtils.cutStringNull(queryStr);
                                log.Debug("Query:" + queryStr);
                                Console.WriteLine("Query:" + queryStr);
                                numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                                break;
                            case "UU":
                                sqlParameters = new List<SqlParameter>();// Add New
                                queryBuilder = new StringBuilder();
                                queryBuilder.AppendFormat(" UPDATE MS_SUBDISTRICT_SOM ");
                                queryBuilder.AppendFormat(" SET ACTIVE_FLAG = 'N' ");
                                queryBuilder.AppendFormat("     ,UPDATE_DTM = dbo.GET_SYSDATETIME() ");
                                queryBuilder.AppendFormat("     ,UPDATE_USER = @USER ");
                                queryBuilder.AppendFormat(" where SUBDISTRICT_SOM_ID = @SUBDISTRICT_SOM_ID ");
                                sqlParameters.Add(QueryUtils.addSqlParameter("USER", msSubDistrictModel.getUserName()));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("SUBDISTRICT_SOM_ID", msSubDistrictModel.SubdistrictSomId));// Add New
                                queryStr = queryBuilder.ToString();
                                queryStr = QueryUtils.cutStringNull(queryStr);
                                log.Debug("Query:" + queryStr);
                                Console.WriteLine("Query:" + queryStr);
                                numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                                sqlParameters = new List<SqlParameter>();// Add New
                                queryBuilder = new StringBuilder();
                                queryBuilder.AppendFormat(" UPDATE MS_SUBDISTRICT_SOM ");
                                queryBuilder.AppendFormat(" SET ACTIVE_FLAG = 'Y' ");
                                queryBuilder.AppendFormat("     ,UPDATE_DTM = dbo.GET_SYSDATETIME() ");
                                queryBuilder.AppendFormat("     ,UPDATE_USER = @USER ");
                                queryBuilder.AppendFormat(" where SUBDISTRICT_SOM_ID = @SUBDISTRICT_SOM_ID ");
                                sqlParameters.Add(QueryUtils.addSqlParameter("USER", msSubDistrictModel.getUserName()));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("SUBDISTRICT_SOM_ID", msSubDistrictModel.SubdistrictSomId));// Add New
                                queryStr = queryBuilder.ToString();
                                queryStr = QueryUtils.cutStringNull(queryStr);
                                log.Debug("Query:" + queryStr);
                                Console.WriteLine("Query:" + queryStr);
                                numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                break;
                        }

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




        public async Task<List<SelectMsSubdistricSomCustom>> getMsSubdistric(string districtCode, string subdistrictCode)
        {

            List<SelectMsSubdistricSomCustom> msDistric = new List<SelectMsSubdistricSomCustom>();
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select ACTIVE_FLAG, SUBDISTRICT_SOM_ID ");
                queryBuilder.AppendFormat(" from MS_SUBDISTRICT_SOM ");
                queryBuilder.AppendFormat(" where DISTRICT_CODE = @DISTRICT_CODE and SUBDISTRICT_CODE = @SUBDISTRICT_CODE ");
                QueryUtils.addParam(command, "DISTRICT_CODE", districtCode);// Add new
                QueryUtils.addParam(command, "SUBDISTRICT_CODE", subdistrictCode);// Add new


                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        SelectMsSubdistricSomCustom o = new SelectMsSubdistricSomCustom();
                        o.activeFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        o.subdistrictSomId = QueryUtils.getValueAsString(record, "SUBDISTRICT_SOM_ID");
                        msDistric.Add(o);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return msDistric;
        }


        // Dell PlanTripTaskAdHoc
        public async Task<int> delSubDistrictSomById(DelSubDistrictSomByIdModel model)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();

                        queryBuilder.AppendFormat("  DELETE MS_SUBDISTRICT_SOM WHERE SUBDISTRICT_SOM_ID  = @SUBDISTRICT_SOM_ID ");

                        sqlParameters.Add(QueryUtils.addSqlParameter("SUBDISTRICT_SOM_ID", model.SubdistrictSomId));// Add
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
