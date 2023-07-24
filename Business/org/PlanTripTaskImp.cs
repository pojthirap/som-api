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

    public class PlanTripTaskImp : IPlanTripTask
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<PlanTripTask>> viewPlanTripTask(SearchCriteriaBase<ViewPlanTripTaskCriteria> searchCriteria)
        {

            EntitySearchResultBase<PlanTripTask> searchResult = new EntitySearchResultBase<PlanTripTask>();
            List<PlanTripTask> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                ViewPlanTripTaskCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" with W as (  ");
                queryBuilder.AppendFormat("     select distinct TT.PLAN_TRIP_TASK_ID,  ");
                queryBuilder.AppendFormat("     CASE TT.TASK_TYPE    ");
                queryBuilder.AppendFormat("         WHEN 'M' THEN IIF(RM.PLAN_TRIP_TASK_ID is null,'N','Y')  ");
                queryBuilder.AppendFormat("         WHEN 'S' THEN IIF(RC.PLAN_TRIP_TASK_ID is null,'N','Y')  ");
                queryBuilder.AppendFormat("         WHEN 'T' THEN IIF(RS.PLAN_TRIP_TASK_ID is null,'N','Y')  ");
                queryBuilder.AppendFormat("         WHEN 'A' THEN IIF(RA.PLAN_TRIP_TASK_ID is null,'N','Y')  ");
                queryBuilder.AppendFormat("         ELSE 'Y'   ");
                queryBuilder.AppendFormat("     END COMPLETED_FLAG   ");
                queryBuilder.AppendFormat("     from PLAN_TRIP_TASK TT  ");
                queryBuilder.AppendFormat("     left join RECORD_APP_FORM RA on RA.PLAN_TRIP_TASK_ID = TT.PLAN_TRIP_TASK_ID  ");
                queryBuilder.AppendFormat("     left join RECORD_SA_FORM RS on RS.PLAN_TRIP_TASK_ID = TT.PLAN_TRIP_TASK_ID  ");
                queryBuilder.AppendFormat("     left join RECORD_METER RM on RM.PLAN_TRIP_TASK_ID = TT.PLAN_TRIP_TASK_ID  ");
                queryBuilder.AppendFormat("     left join RECORD_STOCK_CARD RC on RC.PLAN_TRIP_TASK_ID = TT.PLAN_TRIP_TASK_ID  ");
                queryBuilder.AppendFormat("     where TT.PLAN_TRIP_PROSP_ID = @PlanTripProspId  ");
                queryBuilder.AppendFormat(" )  ");
                queryBuilder.AppendFormat(" select TT.*,  ");
                queryBuilder.AppendFormat(" CASE TT.TASK_TYPE    ");
                queryBuilder.AppendFormat("     WHEN 'M' THEN 'จดมิเตอร์'  ");
                queryBuilder.AppendFormat("     WHEN 'S' THEN (SELECT SC.TP_NAME_TH FROM TEMPLATE_STOCK_CARD SC WHERE SC.TP_STOCK_CARD_ID = TT.TP_STOCK_CARD_ID)  ");
                queryBuilder.AppendFormat("     WHEN 'T' THEN (SELECT SF.TP_NAME_TH FROM TEMPLATE_SA_FORM SF WHERE SF.TP_SA_FORM_ID = TT.TP_SA_FORM_ID)  ");
                queryBuilder.AppendFormat("     WHEN 'A' THEN (SELECT AF.TP_NAME_TH FROM TEMPLATE_APP_FORM AF WHERE AF.TP_APP_FORM_ID = TT.TP_APP_FORM_ID)  ");
                queryBuilder.AppendFormat("     ELSE 'None Task'   ");
                queryBuilder.AppendFormat(" END TEMPLATE_NAME,W.COMPLETED_FLAG, IIF(TT.TASK_TYPE = 'S',TT.TP_STOCK_CARD_ID,null) STOCK_CARD_CODE  ");
                queryBuilder.AppendFormat(" from W  ");
                queryBuilder.AppendFormat(" inner join PLAN_TRIP_TASK TT on TT.PLAN_TRIP_TASK_ID = W.PLAN_TRIP_TASK_ID  ");
                queryBuilder.AppendFormat(" order by TT.PLAN_TRIP_TASK_ID  ");
                QueryUtils.addParam(command, "PlanTripProspId", o.PlanTripProspId);// Add new

                // For Paging
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

                    List<PlanTripTask> dataRecordList = new List<PlanTripTask>();
                    PlanTripTask dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new PlanTripTask();



                        dataRecord.PlanTripTaskId = QueryUtils.getValueAsDecimalRequired(record, "PLAN_TRIP_TASK_ID");
                        dataRecord.PlanTripProspId = QueryUtils.getValueAsDecimal(record, "PLAN_TRIP_PROSP_ID");
                        dataRecord.TaskType = QueryUtils.getValueAsString(record, "TASK_TYPE");
                        dataRecord.TpStockCardId = QueryUtils.getValueAsDecimal(record, "TP_STOCK_CARD_ID");
                        dataRecord.TpSaFormId = QueryUtils.getValueAsDecimal(record, "TP_SA_FORM_ID");
                        dataRecord.TpAppFormId = QueryUtils.getValueAsDecimal(record, "TP_APP_FORM_ID");
                        dataRecord.RequireFlag = QueryUtils.getValueAsString(record, "REQUIRE_FLAG");
                        dataRecord.OrderNo = QueryUtils.getValueAsDecimal(record, "ORDER_NO");
                        dataRecord.AdhocFlag = QueryUtils.getValueAsString(record, "ADHOC_FLAG");
                        dataRecord.TemplateName = QueryUtils.getValueAsString(record, "TEMPLATE_NAME");
                        dataRecord.CompletedFlag = QueryUtils.getValueAsString(record, "COMPLETED_FLAG");
                        dataRecord.StockCardCode = QueryUtils.getValueAsString(record, "STOCK_CARD_CODE");
                        

                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");


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




        public async Task<PlanTripTask> addPlanTripTaskAdHoc(PlanTripTaskModel planTripTaskModel)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for PLAN_TRIP_TASK_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PLAN_TRIP_TASK (PLAN_TRIP_TASK_ID, PLAN_TRIP_PROSP_ID, TASK_TYPE, TP_STOCK_CARD_ID, TP_SA_FORM_ID, TP_APP_FORM_ID, REQUIRE_FLAG, ORDER_NO, ADHOC_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@PLAN_TRIP_TASK_ID ,@PLAN_TRIP_PROSP_ID, @TASK_TYPE, @TP_STOCK_CARD_ID, @TP_SA_FORM_ID, @TP_APP_FORM_ID, @REQUIRE_FLAG, @ORDER_NO, 'Y', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_TASK_ID", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_PROSP_ID", planTripTaskModel.PlanTripProspId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TASK_TYPE", planTripTaskModel.TaskType));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_STOCK_CARD_ID", planTripTaskModel.TpStockCardId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_SA_FORM_ID", planTripTaskModel.TpSaFormId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_APP_FORM_ID", planTripTaskModel.TpAppFormId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("REQUIRE_FLAG", planTripTaskModel.RequireFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_NO", planTripTaskModel.OrderNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", planTripTaskModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        PlanTripTask re = new PlanTripTask();
                        re.PlanTripTaskId = nextVal;
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





    }
}
