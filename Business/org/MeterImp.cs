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
using System.Data;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.Models.record;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.exception;

namespace MyFirstAzureWebApp.Business.org
{

    public class MeterImp : IMeter
    {
        private Logger log = LogManager.GetCurrentClassLogger();



        public async Task<MsMeter> Add(MeterModel meter)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MsMeter re = new MsMeter();

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_METER SET ACTIVE_FLAG='Y', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE CUST_CODE=@CUST_CODE and DISPENSER_NO=@DISPENSER_NO and NOZZLE_NO=@NOZZLE_NO and ACTIVE_FLAG='N' ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", meter.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CUST_CODE", meter.custCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DISPENSER_NO", meter.dispenserNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("NOZZLE_NO", meter.nozzleNo));// Add New
                        string queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        if (!String.IsNullOrEmpty(meter.meterId)) {
                            re.MeterId = Convert.ToDecimal(meter.meterId);
                        }
                        if (numberOfRowInserted == 0)
                        {
                            var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                            p.Direction = System.Data.ParameterDirection.Output;
                            context.Database.ExecuteSqlRaw("set @result = next value for MS_METER_SEQ ", p);
                            var nextVal = (int)p.Value;

                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat("INSERT INTO  MS_METER (METER_ID, GAS_ID, CUST_CODE, DISPENSER_NO, NOZZLE_NO, QRCODE, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                            queryBuilder.AppendFormat("VALUES(@METER_ID, @GAS_ID, @CUST_CODE, @DISPENSER_NO, @NOZZLE_NO,  RIGHT('000000000000000'+@CUST_CODE, 15)+RIGHT('0'+@DISPENSER_NO, 2)+RIGHT('0'+@NOZZLE_NO, 2), 'Y', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                            sqlParameters.Add(QueryUtils.addSqlParameter("METER_ID", nextVal));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("GAS_ID", meter.gasId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("CUST_CODE", meter.custCode));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("DISPENSER_NO", meter.dispenserNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("NOZZLE_NO", meter.nozzleNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", meter.getUserName()));// Add New
                            queryStr = queryBuilder.ToString();
                            queryStr = QueryUtils.cutStringNull(queryStr);
                            log.Debug("Query:" + queryStr);
                            Console.WriteLine("Query:" + queryStr);
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                            re.MeterId = nextVal;
                        }
                        transaction.Commit();
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

        public async Task<int> Update(MeterModel meterModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_METER SET GAS_ID =@GAS_ID, CUST_CODE=@CUST_CODE, DISPENSER_NO=@DISPENSER_NO, NOZZLE_NO=@NOZZLE_NO, QRCODE=RIGHT('000000000000000'+@CUST_CODE, 15)+RIGHT('0'+@DISPENSER_NO, 2)+RIGHT('0'+@NOZZLE_NO, 2), ACTIVE_FLAG=@ACTIVE_FLAG, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE METER_ID=@METER_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("GAS_ID", meterModel.gasId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CUST_CODE", meterModel.custCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DISPENSER_NO", meterModel.dispenserNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("NOZZLE_NO", meterModel.nozzleNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ACTIVE_FLAG", meterModel.activeFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", meterModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("METER_ID", meterModel.meterId));// Add New
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


        public async Task<int> DeleteUpdate(MeterModel meterModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_METER SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE METER_ID=@METER_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", meterModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("METER_ID", meterModel.meterId));// Add New
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




        public async Task<EntitySearchResultBase<GetTaskMeterForRecordCustom>> getTaskMeterForRecord(SearchCriteriaBase<GetTaskMeterForRecordCriteria> searchCriteria)
        {

            EntitySearchResultBase<GetTaskMeterForRecordCustom> searchResult = new EntitySearchResultBase<GetTaskMeterForRecordCustom>();
            List<GetTaskMeterForRecordCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                GetTaskMeterForRecordCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" with T as ( ");
                queryBuilder.AppendFormat(" SELECT  ");
                queryBuilder.AppendFormat("        REC_RUN_NO AS PREV_REC, ");
                queryBuilder.AppendFormat("        METER_ID, ");
                queryBuilder.AppendFormat("        LAG(REC_RUN_NO) OVER (PARTITION BY METER_ID  ORDER BY REC_METER_ID DESC) AS NEXT_REC ");
                queryBuilder.AppendFormat("     FROM RECORD_METER ");
                queryBuilder.AppendFormat(" ) ");
                queryBuilder.AppendFormat(" select M.ACTIVE_FLAG AS M_ACTIVE_FLAG, M.CREATE_USER AS M_CREATE_USER, M.CREATE_DTM AS M_CREATE_DTM, M.UPDATE_USER AS M_UPDATE_USER, M.UPDATE_DTM AS M_UPDATE_DTM, M.*,G.*,F.*,RM.REC_RUN_NO,RM.FILE_ID AS RM_FILE_ID,RM.REMARK AS MR_REMARK, ISNULL(RM.PREV_REC_RUN_NO,T.PREV_REC) PREV_REC_RUN_NO ");
                queryBuilder.AppendFormat(" from MS_METER M ");
                queryBuilder.AppendFormat(" inner join MS_GASOLINE G on G.GAS_ID = M.GAS_ID ");
                queryBuilder.AppendFormat(" left join T on T.METER_ID = M.METER_ID and T.NEXT_REC IS NULL ");
                queryBuilder.AppendFormat(" left join RECORD_METER RM on RM.METER_ID = M.METER_ID and RM.PLAN_TRIP_TASK_ID = @PlanTripTaskId ");
                queryBuilder.AppendFormat(" left join RECORD_METER_FILE F on F.FILE_ID = RM.FILE_ID ");
                queryBuilder.AppendFormat(" left join PLAN_TRIP_TASK PT on PT.PLAN_TRIP_TASK_ID = RM.PLAN_TRIP_TASK_ID ");
                queryBuilder.AppendFormat(" where M.ACTIVE_FLAG = 'Y' ");

                QueryUtils.addParam(command, "PlanTripTaskId", o.PlanTripTaskId);

                /*queryBuilder.AppendFormat(" select M.ACTIVE_FLAG AS M_ACTIVE_FLAG, M.CREATE_USER AS M_CREATE_USER, M.CREATE_DTM AS M_CREATE_DTM, M.UPDATE_USER AS M_UPDATE_USER, M.UPDATE_DTM AS M_UPDATE_DTM, M.*,G.*,F.*,RM.REC_RUN_NO,RM.FILE_ID AS RM_FILE_ID,RM.REMARK AS MR_REMARK, RM.PREV_REC_RUN_NO ");
                queryBuilder.AppendFormat(" from MS_METER M ");
                queryBuilder.AppendFormat(" inner join MS_GASOLINE G on G.GAS_ID = M.GAS_ID ");
                queryBuilder.AppendFormat(" left join RECORD_METER RM on RM.METER_ID = M.METER_ID and RM.PLAN_TRIP_TASK_ID = @PlanTripTaskId ");
                queryBuilder.AppendFormat(" left join RECORD_METER_FILE F on F.FILE_ID = RM.FILE_ID ");
                queryBuilder.AppendFormat(" left join PLAN_TRIP_TASK PT on PT.PLAN_TRIP_TASK_ID = RM.PLAN_TRIP_TASK_ID ");
                
                queryBuilder.AppendFormat(" where M.ACTIVE_FLAG = 'Y' ");
                QueryUtils.addParam(command, "PlanTripTaskId", o.PlanTripTaskId);*/



                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.CustCode))
                    {
                        queryBuilder.AppendFormat(" and M.CUST_CODE = @CustCode ");
                        QueryUtils.addParam(command, "CustCode", o.CustCode);
                    }
                }



                // For Paging
                if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY M.DISPENSER_NO , M.NOZZLE_NO   ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY M.METER_ID  ");
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

                    List<GetTaskMeterForRecordCustom> dataRecordList = new List<GetTaskMeterForRecordCustom>();
                    GetTaskMeterForRecordCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new GetTaskMeterForRecordCustom();


                        dataRecord.MeterActiveFlag = QueryUtils.getValueAsString(record, "M_ACTIVE_FLAG");
                        dataRecord.MeterCreateUser = QueryUtils.getValueAsString(record, "M_CREATE_USER");
                        dataRecord.MeterCreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "M_CREATE_DTM");
                        dataRecord.MeterUpdateUser = QueryUtils.getValueAsString(record, "M_UPDATE_USER");
                        dataRecord.MeterUpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "M_UPDATE_DTM");

                        dataRecord.MeterId = QueryUtils.getValueAsDecimalRequired(record, "METER_ID");
                        dataRecord.GasId = QueryUtils.getValueAsDecimal(record, "GAS_ID");
                        dataRecord.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        dataRecord.DispenserNo = QueryUtils.getValueAsDecimal(record, "DISPENSER_NO");
                        dataRecord.NozzleNo = QueryUtils.getValueAsDecimal(record, "NOZZLE_NO");
                        dataRecord.Qrcode = QueryUtils.getValueAsString(record, "QRCODE");
                        dataRecord.GasNameTh = QueryUtils.getValueAsString(record, "GAS_NAME_TH");
                        dataRecord.GasNameEn = QueryUtils.getValueAsString(record, "GAS_NAME_EN");
                        dataRecord.GasCode = QueryUtils.getValueAsString(record, "GAS_CODE");
                        dataRecord.FileId = QueryUtils.getValueAsString(record, "RM_FILE_ID");
                        dataRecord.RecMeterId = QueryUtils.getValueAsString(record, "REC_METER_ID");
                        dataRecord.FileName = QueryUtils.getValueAsString(record, "FILE_NAME");
                        dataRecord.FileExt = QueryUtils.getValueAsString(record, "FILE_EXT");
                        dataRecord.FileSize = QueryUtils.getValueAsString(record, "FILE_SIZE");
                        dataRecord.RecRunNo = QueryUtils.getValueAsString(record, "REC_RUN_NO");
                        dataRecord.Remark = QueryUtils.getValueAsString(record, "MR_REMARK");
                        dataRecord.PrevRecRunNo = QueryUtils.getValueAsString(record, "PREV_REC_RUN_NO");
                        //dataRecord.OldRecRunNo = QueryUtils.getValueAsString(record, "PREV_REC_RUN_NO");

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




        public async Task<EntitySearchResultBase<SearchMeterCustom>> searchMeter(SearchCriteriaBase<SearchMeterCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchMeterCustom> searchResult = new EntitySearchResultBase<SearchMeterCustom>();
            List<SearchMeterCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchMeterCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select C.CUST_NAME_TH, M.ACTIVE_FLAG AS M_ACTIVE_FLAG, M.CREATE_USER AS M_CREATE_USER, M.CREATE_DTM AS M_CREATE_DTM, M.UPDATE_USER AS M_UPDATE_USER, M.UPDATE_DTM AS M_UPDATE_DTM, M.*, ");
                queryBuilder.AppendFormat(" G.ACTIVE_FLAG AS G_ACTIVE_FLAG, G.CREATE_USER AS G_CREATE_USER, G.CREATE_DTM AS G_CREATE_DTM, G.UPDATE_USER AS G_UPDATE_USER, G.UPDATE_DTM AS G_UPDATE_DTM, G.* ");
                queryBuilder.AppendFormat(" from MS_METER M ");
                queryBuilder.AppendFormat(" inner join MS_GASOLINE G on G.GAS_ID = M.GAS_ID ");
                queryBuilder.AppendFormat(" inner join CUSTOMER C on C.CUST_CODE = M.CUST_CODE ");
                queryBuilder.AppendFormat(" where 1= 1 ");


                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.QrCode))
                    {
                        queryBuilder.AppendFormat(" and M.QRCODE = @QrCode ");
                        QueryUtils.addParam(command, "QrCode", o.QrCode);
                    }
                    if (!String.IsNullOrEmpty(o.CustCode))
                    {
                        queryBuilder.AppendFormat(" and M.CUST_CODE = @CustCode ");
                        QueryUtils.addParam(command, "CustCode", o.CustCode);
                    }
                    if (!String.IsNullOrEmpty(o.DispenserNo))
                    {
                        queryBuilder.AppendFormat(" and M.DISPENSER_NO = @DispenserNo ");
                        QueryUtils.addParam(command, "DispenserNo", o.DispenserNo);
                    }
                    if (!String.IsNullOrEmpty(o.NozzleNo))
                    {
                        queryBuilder.AppendFormat(" and M.NOZZLE_NO = @NozzleNo ");
                        QueryUtils.addParam(command, "NozzleNo", o.NozzleNo);
                    }
                    if (!String.IsNullOrEmpty(o.ActiveClag))
                    {
                        queryBuilder.AppendFormat(" and M.ACTIVE_FLAG = @ActiveClag ");
                        QueryUtils.addParam(command, "ActiveClag", o.ActiveClag);
                    }
                }



                // For Paging
                queryBuilder.AppendFormat(" ORDER BY G.GAS_NAME_TH  ");
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

                    List<SearchMeterCustom> dataRecordList = new List<SearchMeterCustom>();
                    SearchMeterCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchMeterCustom();


                        dataRecord.MeterActiveFlag = QueryUtils.getValueAsString(record, "M_ACTIVE_FLAG");
                        dataRecord.MeterCreateUser = QueryUtils.getValueAsString(record, "M_CREATE_USER");
                        dataRecord.MeterCreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "M_CREATE_DTM");
                        dataRecord.MeterUpdateUser = QueryUtils.getValueAsString(record, "M_UPDATE_USER");
                        dataRecord.MeterUpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "M_UPDATE_DTM");
                        dataRecord.MeterId = QueryUtils.getValueAsDecimalRequired(record, "METER_ID");
                        dataRecord.GasId = QueryUtils.getValueAsDecimal(record, "GAS_ID");
                        dataRecord.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        dataRecord.DispenserNo = QueryUtils.getValueAsDecimal(record, "DISPENSER_NO");
                        dataRecord.NozzleNo = QueryUtils.getValueAsDecimal(record, "NOZZLE_NO");
                        dataRecord.Qrcode = QueryUtils.getValueAsString(record, "QRCODE");

                        dataRecord.GasActiveFlag = QueryUtils.getValueAsString(record, "G_ACTIVE_FLAG");
                        dataRecord.GasCreateUser = QueryUtils.getValueAsString(record, "G_CREATE_USER");
                        dataRecord.GasCreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "G_CREATE_DTM");
                        dataRecord.GasUpdateUser = QueryUtils.getValueAsString(record, "G_UPDATE_USER");
                        dataRecord.GasUpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "G_UPDATE_DTM");
                        dataRecord.GasNameTh = QueryUtils.getValueAsString(record, "GAS_NAME_TH");
                        dataRecord.GasNameEn = QueryUtils.getValueAsString(record, "GAS_NAME_EN");
                        dataRecord.GasCode = QueryUtils.getValueAsString(record, "GAS_CODE");

                        dataRecord.CustNameTh = QueryUtils.getValueAsString(record, "CUST_NAME_TH");

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





        public async Task<List<RecordMeter>> addRecordMeter(List<RecordMeterModel> recordMeterModelList, UserProfileForBack userProfileForBack)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        List<RecordMeter> lst = new List<RecordMeter>();
                        // Delete
                        //int recordUpdate = await DeleteRecordMeterForAdd(recordMeterModelList.ElementAt(0).PlanTripTaskId.ToString());
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM RECORD_METER  WHERE PLAN_TRIP_TASK_ID=@PlanTripTaskId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PlanTripTaskId", recordMeterModelList.ElementAt(0).PlanTripTaskId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        // Add 
                        for (int i = 0; i < recordMeterModelList.Count; i++)
                        {
                            RecordMeterModel recordMeterModel = recordMeterModelList.ElementAt(i);
                            sqlParameters = new List<SqlParameter>();// Add New
                            var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                            p.Direction = System.Data.ParameterDirection.Output;
                            context.Database.ExecuteSqlRaw("set @result = next value for RECORD_METER_SEQ", p);
                            var nextVal = (int)p.Value;

                            queryBuilder = new StringBuilder();

                            queryBuilder.AppendFormat(" WITH N AS(");
                            queryBuilder.AppendFormat(" select TOP 1 R.REC_METER_ID,M.GAS_ID,ISNULL(R.GAS_ID,M.GAS_ID) PREV_GAS_ID,R.REC_RUN_NO PREV_REC_RUN_NO ");
                            queryBuilder.AppendFormat(" from MS_METER M ");
                            queryBuilder.AppendFormat(" left join RECORD_METER R on R.METER_ID = M.METER_ID ");
                            queryBuilder.AppendFormat(" where M.METER_ID = @METER_ID ");
                            queryBuilder.AppendFormat(" order by R.REC_METER_ID desc ");
                            queryBuilder.AppendFormat(" ) ");
                            queryBuilder.AppendFormat(" INSERT INTO RECORD_METER ([REC_METER_ID], [PLAN_TRIP_TASK_ID], [METER_ID], [REC_RUN_NO], PREV_REC_RUN_NO, [FILE_ID], [REMARK], [GAS_ID], [PREV_GAS_ID], [PREV_REC_METER_ID], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM])  ");
                            queryBuilder.AppendFormat(" SELECT @REC_METER_ID, @PLAN_TRIP_TASK_ID, @METER_ID, @REC_RUN_NO, N.PREV_REC_RUN_NO, @FILE_ID, @REMARK, N.GAS_ID, N.PREV_GAS_ID, N.REC_METER_ID, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME() FROM N ");
                            sqlParameters.Add(QueryUtils.addSqlParameter("METER_ID", recordMeterModel.MeterId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("REC_METER_ID", nextVal));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_TASK_ID", recordMeterModel.PlanTripTaskId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("REC_RUN_NO", recordMeterModel.RecRunNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("FILE_ID", recordMeterModel.FileId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("REMARK", recordMeterModel.Remark));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfileForBack.getUserName()));// Add New
                            
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                            RecordMeter re = new RecordMeter();
                            re.RecMeterId = nextVal;
                            lst.Add(re);
                        }
                        transaction.Commit();
                        return lst;



                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }



        public async Task<int> DeleteRecordMeterForAdd(String PlanTripTaskId)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM RECORD_METER  WHERE PLAN_TRIP_TASK_ID=@PlanTripTaskId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PlanTripTaskId", PlanTripTaskId));// Add New
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


        public async Task<EntitySearchResultBase<CheckPercentRangRecordMeterCustom>> checkPercentRangRecordMeter(SearchCriteriaBase<CheckPercentRangRecordMeterCriteria> searchCriteria, string language)
        {

            EntitySearchResultBase<CheckPercentRangRecordMeterCustom> searchResult = new EntitySearchResultBase<CheckPercentRangRecordMeterCustom>();
            List<CheckPercentRangRecordMeterCustom> lst;
            string VALID_FLAG = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                CheckPercentRangRecordMeterCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" with R AS(");
                queryBuilder.AppendFormat(" select TOP 1 ROW_NUMBER() over (PARTITION BY METER_ID ORDER BY REC_METER_ID DESC) AS Row# ");
                queryBuilder.AppendFormat(" , REC_METER_ID,METER_ID,PREV_REC_RUN_NO ");
                queryBuilder.AppendFormat(" , ISNULL(CAST(REC_RUN_NO as int)-cast(PREV_REC_RUN_NO as int),-1) DIV_REC_RUN_NO ");
                queryBuilder.AppendFormat(" ,(ISNULL(CAST(REC_RUN_NO as int) - cast(PREV_REC_RUN_NO as int), -1) * MI.PARAM_VALUE / 100) + REC_RUN_NO MIN_RECORD ");
                queryBuilder.AppendFormat(" ,(ISNULL(CAST(REC_RUN_NO as int) - cast(PREV_REC_RUN_NO as int), -1) * MX.PARAM_VALUE / 100) + REC_RUN_NO MAX_RECORD ");
                queryBuilder.AppendFormat(" from RECORD_METER  ");
                queryBuilder.AppendFormat(" inner join MS_CONFIG_PARAM MI on MI.PARAM_KEYWORD = 'RECORD_METER_MIN' ");
                queryBuilder.AppendFormat(" inner join MS_CONFIG_PARAM MX on MX.PARAM_KEYWORD = 'RECORD_METER_MAX' ");
                queryBuilder.AppendFormat(" where METER_ID = @METER_ID ");
                queryBuilder.AppendFormat(" and PLAN_TRIP_TASK_ID != @PLAN_TRIP_TASK_ID ");
                queryBuilder.AppendFormat(" ) ");
                queryBuilder.AppendFormat(" select R.* ");
                queryBuilder.AppendFormat(" ,IIF(PREV_REC_RUN_NO IS NOT NULL, CASE WHEN @REC_RUN_NO < R.MIN_RECORD THEN 'MIN' WHEN @REC_RUN_NO > R.MAX_RECORD THEN 'MAX' ELSE '' END, '') VALID_FLAG ");
                queryBuilder.AppendFormat(" from R ");
                //queryBuilder.AppendFormat(" where @REC_RUN_NO between R.MIN_RECORD and R.MAX_RECORD ");
                QueryUtils.addParam(command, "METER_ID", o.MeterId);
                QueryUtils.addParam(command, "PLAN_TRIP_TASK_ID", o.PlanTripTaskId);
                QueryUtils.addParam(command, "REC_RUN_NO", o.RecRunNo);

                //bool isFoundRecord = false;
                // For Paging
                queryBuilder.AppendFormat(" ORDER BY REC_METER_ID  ");
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

                    List<CheckPercentRangRecordMeterCustom> dataRecordList = new List<CheckPercentRangRecordMeterCustom>();
                    
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        VALID_FLAG = QueryUtils.getValueAsString(record, "VALID_FLAG"); 
                        
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //
                    if ("MIN".Equals(VALID_FLAG))
                    {

                        /*List<String> errorParam = new List<string>();
                        errorParam.Add(o.RecRunNo + " Not between % Min - Max");
                        ServiceException se = new ServiceException("E_CUSTOM_MSG_ONLY", errorParam, ObjectFacory.getCultureInfo(language));*/
                        ServiceException se = new ServiceException("W_0006", ObjectFacory.getCultureInfo(language));
                        throw se;
                    }
                    else if("MAX".Equals(VALID_FLAG))
                    {
                        ServiceException se = new ServiceException("W_0007", ObjectFacory.getCultureInfo(language));
                        throw se;
                    }

                    CheckPercentRangRecordMeterCustom dataRecord = new CheckPercentRangRecordMeterCustom();
                    dataRecord.checkPercentRangRecordMeter = true;
                    dataRecordList.Add(dataRecord);

                    lst = dataRecordList;
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }








    }
}
