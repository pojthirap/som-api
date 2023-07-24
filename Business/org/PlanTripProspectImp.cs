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
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.plan;

namespace MyFirstAzureWebApp.Business.org
{

    public class PlanTripProspectImp : IPlanTripProspect
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<GetLastCheckInCustom>> getLastCheckIn(SearchCriteriaBase<GetLastCheckInCriteria> searchCriteria)
        {

            EntitySearchResultBase<GetLastCheckInCustom> searchResult = new EntitySearchResultBase<GetLastCheckInCustom>();
            List<GetLastCheckInCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                GetLastCheckInCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select TOP 1 VISIT_LATITUDE,VISIT_LONGITUDE  ");
                queryBuilder.AppendFormat(" FROM PLAN_TRIP_PROSPECT  ");
                queryBuilder.AppendFormat(" where PLAN_TRIP_ID = @PlanTripId  ");
                queryBuilder.AppendFormat(" and VISIT_CHECKIN_DTM is not null  ");
                QueryUtils.addParam(command, "PlanTripId", o.PlanTripId);

                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.PlanTripProspId))
                    {
                        queryBuilder.AppendFormat(" and PLAN_TRIP_PROSP_ID != @PlanTripProspId  ");
                        QueryUtils.addParam(command, "PlanTripProspId", o.PlanTripProspId);
                    }
                }

                
                

                


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY VISIT_CHECKIN_DTM desc  ");
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

                    List<GetLastCheckInCustom> dataRecordList = new List<GetLastCheckInCustom>();
                    GetLastCheckInCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new GetLastCheckInCustom();

                        dataRecord.Latitude = QueryUtils.getValueAsString(record, "VISIT_LATITUDE");
                        dataRecord.Longitude = QueryUtils.getValueAsString(record, "VISIT_LONGITUDE");
                        
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




        
        public async Task<checkInPlanTripProspectData> checkInPlanTripProspect(PlanTripProspectModel planTripProspectModel)
        {

            //
            //List<checkInPlanTripProspectData> dataRecordList = new List<checkInPlanTripProspectData>();
            checkInPlanTripProspectData dataRecord = new checkInPlanTripProspectData();
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("  with T as ( ");
                queryBuilder.AppendFormat(" select VISIT_CHECKIN_DTM ");
                queryBuilder.AppendFormat(" ,lag(PLAN_TRIP_PROSP_ID) over (order by VISIT_CHECKIN_DTM) as BEFORE_PLAN_TRIP_PROSP_ID ");
                queryBuilder.AppendFormat(" ,PLAN_TRIP_PROSP_ID ");
                queryBuilder.AppendFormat(" ,lead(PLAN_TRIP_PROSP_ID) over (order by VISIT_CHECKIN_DTM) as NEXT_PLAN_TRIP_PROSP_ID ");
                queryBuilder.AppendFormat(" from PLAN_TRIP_PROSPECT ");
                queryBuilder.AppendFormat(" where VISIT_CALC_KM is not null ");
                queryBuilder.AppendFormat(" and PLAN_TRIP_ID = @PLAN_TRIP_ID ");
                //--order by VISIT_CHECKIN_DTM
                queryBuilder.AppendFormat(" ) ");
                queryBuilder.AppendFormat(" select T.BEFORE_PLAN_TRIP_PROSP_ID,T.NEXT_PLAN_TRIP_PROSP_ID ");
                queryBuilder.AppendFormat(" from T  ");
                queryBuilder.AppendFormat(" where T.PLAN_TRIP_PROSP_ID = @PLAN_TRIP_PROSP_ID ");
                QueryUtils.addParam(command, "PLAN_TRIP_ID", planTripProspectModel.PlanTripId);
                QueryUtils.addParam(command, "PLAN_TRIP_PROSP_ID", planTripProspectModel.PlanTripProspId);
                
                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        //dataRecord = new checkInPlanTripProspectData();
                        dataRecord.BeforePlanTripProspId = QueryUtils.getValueAsString(record, "BEFORE_PLAN_TRIP_PROSP_ID");
                        dataRecord.nextPlanTripProspId = QueryUtils.getValueAsString(record, "NEXT_PLAN_TRIP_PROSP_ID");
                        //dataRecordList.Add(dataRecord);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }

            //

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PLAN_TRIP_PROSPECT SET VISIT_LATITUDE=@VISIT_LATITUDE, VISIT_LONGITUDE=@VISIT_LONGITUDE, VISIT_CHECKIN_MILE_NO=@VISIT_CHECKIN_MILE_NO, VISIT_CHECKIN_DTM=dbo.GET_SYSDATETIME(), VISIT_CALC_KM = NULL, VISIT_CHECKOUT_DTM=NULL, REASON_NOT_VISIT_ID=NULL, REASON_NOT_VISIT_REMARK=NULL, [CONTACT_NAME]= NULL, [CONTACT_MOBILE_NO]= NULL, [CHECKOUT_REMARK]= NULL, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PLAN_TRIP_PROSP_ID=@PLAN_TRIP_PROSP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("VISIT_LATITUDE", planTripProspectModel.VisitLatitude));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("VISIT_LONGITUDE", planTripProspectModel.VisitLongitude));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("VISIT_CHECKIN_MILE_NO", planTripProspectModel.VisitCheckinMileNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_PROSP_ID", planTripProspectModel.PlanTripProspId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", planTripProspectModel.getUserName()));// Add New
                        string queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        //return numberOfRowInserted;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            //return dataRecordList;
            return dataRecord;

        }



        public async Task<int> checkOutPlanTripProspect(CheckOutPlanTripProspectModel checkOutPlanTripProspectModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PLAN_TRIP_PROSPECT SET VISIT_CALC_KM=@CURRENT_VISIT_CALC_KM, VISIT_CHECKOUT_DTM=dbo.GET_SYSDATETIME(), UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME(), [CONTACT_NAME]=@CONTACT_NAME, [CONTACT_MOBILE_NO]=@CONTACT_MOBILE_NO, [CHECKOUT_REMARK]=@CHECKOUT_REMARK  WHERE PLAN_TRIP_PROSP_ID=@CURRENT_PLAN_TRIP_PROSP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("CURRENT_VISIT_CALC_KM", checkOutPlanTripProspectModel.CurrentVisitCalcKm));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", checkOutPlanTripProspectModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CONTACT_NAME", checkOutPlanTripProspectModel.ContactName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CONTACT_MOBILE_NO", checkOutPlanTripProspectModel.ContactMobileNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CHECKOUT_REMARK", checkOutPlanTripProspectModel.CheckoutRemark));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CURRENT_PLAN_TRIP_PROSP_ID", checkOutPlanTripProspectModel.CurrentPlanTripProspId));// Add New
                        string queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        if (!String.IsNullOrEmpty(checkOutPlanTripProspectModel.UpdPlanTripProspId))
                        {


                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat(" UPDATE PLAN_TRIP_PROSPECT ");
                            queryBuilder.AppendFormat(" SET VISIT_CALC_KM = @UPD_VISIT_CALC_KM, UPDATE_USER=@USER, UPDATE_DTM= dbo.GET_SYSDATETIME() ");
                            queryBuilder.AppendFormat(" WHERE PLAN_TRIP_PROSP_ID = @UPD_PLAN_TRIP_PROSP_ID ");
                            sqlParameters.Add(QueryUtils.addSqlParameter("UPD_VISIT_CALC_KM", checkOutPlanTripProspectModel.UpdVisitCalcKm));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("UPD_PLAN_TRIP_PROSP_ID", checkOutPlanTripProspectModel.UpdPlanTripProspId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", checkOutPlanTripProspectModel.getUserName()));// Add New
                            queryStr = queryBuilder.ToString();
                            queryStr = QueryUtils.cutStringNull(queryStr);
                            log.Debug("Query:" + queryStr);
                            Console.WriteLine("Query:" + queryStr);
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
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

        public async Task<int> updReasonNotVisitForProspect(PlanTripProspectModel planTripProspectModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        int numberOfRowInserted = 0;
                        if (String.IsNullOrEmpty(planTripProspectModel.VisitCalcKm))
                        {

                            queryBuilder.AppendFormat("UPDATE PLAN_TRIP_PROSPECT SET REASON_NOT_VISIT_ID=@REASON_NOT_VISIT_ID, REASON_NOT_VISIT_REMARK=@REASON_NOT_VISIT_REMARK, [CONTACT_NAME]=@CONTACT_NAME, [CONTACT_MOBILE_NO]=@CONTACT_MOBILE_NO, [CHECKOUT_REMARK]=@CHECKOUT_REMARK, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PLAN_TRIP_PROSP_ID=@PLAN_TRIP_PROSP_ID ");
                            sqlParameters.Add(QueryUtils.addSqlParameter("REASON_NOT_VISIT_ID", planTripProspectModel.ReasonNotVisitId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("REASON_NOT_VISIT_REMARK", planTripProspectModel.ReasonNotVisitRemark));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("CONTACT_NAME", planTripProspectModel.ContactName));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("CONTACT_MOBILE_NO", planTripProspectModel.ContactMobileNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("CHECKOUT_REMARK", planTripProspectModel.CheckoutRemark));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_PROSP_ID", planTripProspectModel.PlanTripProspId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", planTripProspectModel.getUserName()));// Add New
                            string queryStr = queryBuilder.ToString();
                            queryStr = QueryUtils.cutStringNull(queryStr);
                            log.Debug("Query:" + queryStr);
                            Console.WriteLine("Query:" + queryStr);
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        }
                        else
                        {
                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat(" UPDATE PLAN_TRIP_PROSPECT  ");
                            queryBuilder.AppendFormat(" SET [REASON_NOT_VISIT_ID]=@REASON_NOT_VISIT_ID, [REASON_NOT_VISIT_REMARK]=@REASON_NOT_VISIT_REMARK, [VISIT_CALC_KM] = @VISIT_CALC_KM, [CONTACT_NAME]=@CONTACT_NAME, [CONTACT_MOBILE_NO]=@CONTACT_MOBILE_NO, [CHECKOUT_REMARK]=@CHECKOUT_REMARK, [UPDATE_USER]=@USER, [UPDATE_DTM]=dbo.GET_SYSDATETIME() ");
                            queryBuilder.AppendFormat(" WHERE PLAN_TRIP_PROSP_ID = @PLAN_TRIP_PROSP_ID ");
                            sqlParameters.Add(QueryUtils.addSqlParameter("REASON_NOT_VISIT_ID", planTripProspectModel.ReasonNotVisitId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("REASON_NOT_VISIT_REMARK", planTripProspectModel.ReasonNotVisitRemark));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VISIT_CALC_KM", planTripProspectModel.VisitCalcKm));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("CONTACT_NAME", planTripProspectModel.ContactName));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("CONTACT_MOBILE_NO", planTripProspectModel.ContactMobileNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("CHECKOUT_REMARK", planTripProspectModel.CheckoutRemark));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_PROSP_ID", planTripProspectModel.PlanTripProspId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", planTripProspectModel.getUserName()));// Add New
                            string queryStr = queryBuilder.ToString();
                            queryStr = QueryUtils.cutStringNull(queryStr);
                            log.Debug("Query:" + queryStr);
                            Console.WriteLine("Query:" + queryStr);
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                            if (!String.IsNullOrEmpty(planTripProspectModel.UpdPlanTripProspId))
                            {

                                sqlParameters = new List<SqlParameter>();// Add New
                                queryBuilder = new StringBuilder();
                                queryBuilder.AppendFormat(" UPDATE PLAN_TRIP_PROSPECT ");
                                queryBuilder.AppendFormat(" SET [VISIT_CALC_KM] = @UPD_VISIT_CALC_KM, [UPDATE_USER]=@USER, [UPDATE_DTM]=dbo.GET_SYSDATETIME() ");
                                queryBuilder.AppendFormat(" WHERE PLAN_TRIP_PROSP_ID = @UPD_PLAN_TRIP_PROSP_ID ");
                                sqlParameters.Add(QueryUtils.addSqlParameter("UPD_VISIT_CALC_KM", planTripProspectModel.UpdVisitCalcKm));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("UPD_PLAN_TRIP_PROSP_ID", planTripProspectModel.UpdPlanTripProspId));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("USER", planTripProspectModel.getUserName()));// Add New
                                queryStr = queryBuilder.ToString();
                                queryStr = QueryUtils.cutStringNull(queryStr);
                                log.Debug("Query:" + queryStr);
                                Console.WriteLine("Query:" + queryStr);
                                numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                            }
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



        public async Task<int> updateRemindForProspect(PlanTripProspectModel planTripProspectModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PLAN_TRIP_PROSPECT SET REMIND=@REMIND, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PLAN_TRIP_PROSP_ID=@PLAN_TRIP_PROSP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("REMIND", planTripProspectModel.Remind));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_PROSP_ID", planTripProspectModel.PlanTripProspId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", planTripProspectModel.getUserName()));// Add New
                        string queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
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



        public async Task<EntitySearchResultBase<GetAddressForBestRouteCustom>> getAddressForBestRoute(SearchCriteriaBase<GetAddressForBestRouteCriteria> searchCriteria)
        {

            EntitySearchResultBase<GetAddressForBestRouteCustom> searchResult = new EntitySearchResultBase<GetAddressForBestRouteCustom>();
            List<GetAddressForBestRouteCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                GetAddressForBestRouteCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select ISNULL(PA.ACC_NAME,L.LOC_NAME_TH) ACC_NAME,ISNULL(PD.LATITUDE,L.LATITUDE) LATITUDE,ISNULL(PD.LONGITUDE,L.LONGITUDE) LONGITUDE ");
                queryBuilder.AppendFormat(" from PLAN_TRIP_PROSPECT PP ");
                queryBuilder.AppendFormat(" left  join PROSPECT P on P.PROSPECT_ID = PP.PROSP_ID ");
                queryBuilder.AppendFormat(" left  join PROSPECT_ACCOUNT PA on PA.PROSP_ACC_ID = P.PROSP_ACC_ID ");
                queryBuilder.AppendFormat(" left  join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" left join MS_LOCATION L on L.LOC_ID = PP.LOC_ID ");
                queryBuilder.AppendFormat(" where 1=1 ");

                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.PlanTripId))
                    {
                        queryBuilder.AppendFormat(" and PP.PLAN_TRIP_ID = @PlanTripId ");
                        QueryUtils.addParam(command, "PlanTripId", o.PlanTripId);
                    }
                }


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY PP.PLAN_START_TIME  ");
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

                    List<GetAddressForBestRouteCustom> dataRecordList = new List<GetAddressForBestRouteCustom>();
                    GetAddressForBestRouteCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new GetAddressForBestRouteCustom();

                        dataRecord.Latitude = QueryUtils.getValueAsString(record, "LATITUDE");
                        dataRecord.Longitude = QueryUtils.getValueAsString(record, "LONGITUDE");
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




        public async Task<PlanTripProspect> addPlanTripProspectAdHoc(PlanTripProspectModel planTripProspectModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        planTripProspectModel.OrderNo = "99"; // FIX 99
                        var sqlParameters = new List<SqlParameter>();// Add New
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PLAN_TRIP_PROSPECT_SEQ", p);
                        var nextVal = (int)p.Value;
                        string code_ = QueryUtils.padLeft(nextVal.ToString(), '0', 4);

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PLAN_TRIP_PROSPECT (PLAN_TRIP_PROSP_ID, PLAN_TRIP_ID, PROSP_ID, LOC_ID, LOC_REMARK, PLAN_START_TIME, PLAN_END_TIME, ORDER_NO, ADHOC_FLAG, MERG_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@PLAN_TRIP_PROSP_ID ,@PLAN_TRIP_ID, @PROSP_ID, @LOC_ID, @LOC_REMARK, @PLAN_START_TIME, @PLAN_END_TIME, @ORDER_NO, 'Y', 'N', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_PROSP_ID", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_ID", planTripProspectModel.PlanTripId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_ID", planTripProspectModel.ProspId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LOC_ID", planTripProspectModel.LocId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LOC_REMARK", planTripProspectModel.LocRemark));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_START_TIME", planTripProspectModel.PlanStartTime));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_END_TIME", planTripProspectModel.PlanEndTime));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_NO", planTripProspectModel.OrderNo));// Add New
                        //sqlParameters.Add(QueryUtils.addSqlParameter("VISIT_LATITUDE", planTripProspectModel.VisitLatitude));// Add New
                        //sqlParameters.Add(QueryUtils.addSqlParameter("VISIT_LONGITUDE", planTripProspectModel.VisitLongitude));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", planTripProspectModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        PlanTripProspect re = new PlanTripProspect();
                        re.PlanTripProspId = nextVal;
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


        public async Task<int> updateLocRemarkForProspect(UpdateLocRemarkForProspectModel updateLocRemarkForProspectModel, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" UPDATE PLAN_TRIP_PROSPECT  ");
                        queryBuilder.AppendFormat(" SET [LOC_REMARK]=@LOC_REMARK, [UPDATE_USER]=@USER, [UPDATE_DTM]=dbo.GET_SYSDATETIME() ");
                        queryBuilder.AppendFormat(" WHERE PLAN_TRIP_PROSP_ID = @PLAN_TRIP_PROSP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("LOC_REMARK", updateLocRemarkForProspectModel.LocRemark));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_PROSP_ID", updateLocRemarkForProspectModel.PlanTripProspId));// Add New
                        string queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
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



    public class checkInPlanTripProspectData
        {
            public string BeforePlanTripProspId { get; set; }
            public string nextPlanTripProspId { get; set; }
        }



    }
}
