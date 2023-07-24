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
using MyFirstAzureWebApp.Models.job;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.enumval;
using System.Data;
using MyFirstAzureWebApp.Entity.custom;

namespace MyFirstAzureWebApp.Business.org
{

    public class EmailJobImp : IEmailJob
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        

        public async Task<EmailJob> Add(EmailJobModel emailJobModel, UserProfileForBack userProfile)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for EMAIL_JOB_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();

                        queryBuilder.AppendFormat(" INSERT INTO EMAIL_JOB ([JOB_ID], [EMAIL_TEMPLATE_ID], [TABLE_REF_KEY_ID], [OBJ_EMAIL], [JOB_STATUS], [JOB_STATUS_FAIL_COUNT], [ERROR_DESC], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM])  ");
                        queryBuilder.AppendFormat(" VALUES(@JOB_ID, @EmailTemplate, @TABLE_REF_KEY_ID, @OBJ_EMAIL, @JOB_STATUS , @JOB_STATUS_FAIL_COUNT, @ERROR_DESC, @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME()) ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("JOB_ID", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("EmailTemplate", emailJobModel.EmailTemplate));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TABLE_REF_KEY_ID", emailJobModel.TableRefKeyId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("OBJ_EMAIL", emailJobModel.ObjEmail));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("JOB_STATUS", emailJobModel.JobStatus));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("JOB_STATUS_FAIL_COUNT", emailJobModel.JobStatusFailCount));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ERROR_DESC", emailJobModel.ErrorDesc));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        EmailJob re = new EmailJob();
                        re.JobId = nextVal;
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






        public async Task<EntitySearchResultBase<SearchEmailJobForPlanTripCustom>> searchEmailJobForPlanTrip(SearchCriteriaBase<SearchEmailJobForPlanTripCriteria> searchCriteria, UserProfileForBack userProfile)
        {

            EntitySearchResultBase<SearchEmailJobForPlanTripCustom> searchResult = new EntitySearchResultBase<SearchEmailJobForPlanTripCustom>();
            List<SearchEmailJobForPlanTripCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchEmailJobForPlanTripCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select J.EMAIL_TEMPLATE_ID,CFG.LOV_NAME_TH EMAIL_TEMPLATE_NAME,P.PLAN_TRIP_NAME,P.PLAN_TRIP_ID,P.PLAN_TRIP_DATE ");
                queryBuilder.AppendFormat(" ,IIF(P.ASSIGN_EMP_ID is null,SR.FIRST_NAME+' '+SR.LAST_NAME,SRA.FIRST_NAME+' '+SRA.LAST_NAME) SALE_REP_NAME ");
                queryBuilder.AppendFormat(" ,IIF(P.ASSIGN_EMP_ID is null,SS.FIRST_NAME+' '+SS.LAST_NAME,SSA.FIRST_NAME+' '+SSA.LAST_NAME) SALE_SUP_NAME ");
                queryBuilder.AppendFormat(" from EMAIL_JOB J ");
                queryBuilder.AppendFormat(" inner join MS_CONFIG_LOV CFG on CFG.LOV_KEYWORD = 'EMAIL_TEMPLATE' and CFG.LOV_KEYVALUE = J.EMAIL_TEMPLATE_ID ");
                queryBuilder.AppendFormat(" inner join PLAN_TRIP P on P.PLAN_TRIP_ID = J.TABLE_REF_KEY_ID ");
                queryBuilder.AppendFormat(" inner join ADM_EMPLOYEE SR on SR.EMP_ID = P.CREATE_USER ");
                queryBuilder.AppendFormat(" left join ADM_EMPLOYEE SRA on SRA.EMP_ID = P.ASSIGN_EMP_ID ");
                queryBuilder.AppendFormat(" left join ADM_EMPLOYEE SS on SS.EMP_ID = SR.APPROVE_EMP_ID ");
                queryBuilder.AppendFormat(" left join ADM_EMPLOYEE SSA on SSA.EMP_ID = SRA.APPROVE_EMP_ID ");
                queryBuilder.AppendFormat(" where J.JOB_STATUS = 'S' ");
                queryBuilder.AppendFormat(" and P.STATUS != @PLAN_TRIP_STATUS_CANCEL  "); //--Note PLAN_TRIP_STATUS.CANCEL -- ตรงนี้ทำเป็น ENUM Value = '5' Fix
                queryBuilder.AppendFormat(" and(");
                queryBuilder.AppendFormat("  (ISNULL(P.ASSIGN_EMP_ID,P.CREATE_USER) = @USER and J.EMAIL_TEMPLATE_ID in (@ASSIGN,@APPROVE,@REJECT))  ");// 2,3,4 -- ตรงนี้ทำเป็น ENUM Fix by Note
                queryBuilder.AppendFormat("  OR(");
                queryBuilder.AppendFormat("     (SR.APPROVE_EMP_ID = @USER ");
                /*queryBuilder.AppendFormat("         OR EXISTS(");
                queryBuilder.AppendFormat("             select 1 ");
                queryBuilder.AppendFormat("             from ORG_SALE_TERRITORY ST ");
                queryBuilder.AppendFormat("             inner join ORG_TERRITORY T ON T.TERRITORY_ID = ST.TERRITORY_ID ");
                queryBuilder.AppendFormat("             where ST.EMP_ID = ISNULL(P.ASSIGN_EMP_ID,P.CREATE_USER)  ");
                queryBuilder.AppendFormat("             and T.MANAGER_EMP_ID = @USER) ");*/
                queryBuilder.AppendFormat(" OR EXISTS( ");
                queryBuilder.AppendFormat("         select 1 ");
                queryBuilder.AppendFormat("         from ADM_EMPLOYEE E ");
                queryBuilder.AppendFormat("         inner join ORG_SALE_GROUP SG on SG.GROUP_CODE = E.GROUP_CODE ");
                queryBuilder.AppendFormat("         inner join ORG_TERRITORY T ON T.TERRITORY_ID = SG.TERRITORY_ID ");
                queryBuilder.AppendFormat("         where E.EMP_ID = ISNULL(P.ASSIGN_EMP_ID,P.CREATE_USER)  ");
                queryBuilder.AppendFormat("         and T.MANAGER_EMP_ID = @USER) ");
                queryBuilder.AppendFormat("     ) and J.EMAIL_TEMPLATE_ID in (@WAITING_FOR_APPROVE,@WAITING_FOR_APPROVE_ALERT))  ");//--  1,5 ตรงนี้ทำเป็น ENUM Fix by Note
                queryBuilder.AppendFormat("  ) ");

                QueryUtils.addParam(command, "USER", userProfile.getEmpId());// Add new
                QueryUtils.addParam(command, "PLAN_TRIP_STATUS_CANCEL", PlanTripStatus.CANCEL.ToString("d"));// Add new
                QueryUtils.addParam(command, "ASSIGN", EmailTemplateStatus.ASSIGN.ToString("d"));// Add new
                QueryUtils.addParam(command, "APPROVE", EmailTemplateStatus.APPROVE.ToString("d"));// Add new
                QueryUtils.addParam(command, "REJECT", EmailTemplateStatus.REJECT.ToString("d"));// Add new
                QueryUtils.addParam(command, "WAITING_FOR_APPROVE", EmailTemplateStatus.WAITING_FOR_APPROVE.ToString("d"));// Add new
                QueryUtils.addParam(command, "WAITING_FOR_APPROVE_ALERT", EmailTemplateStatus.WAITING_FOR_APPROVE_ALERT.ToString("d"));// Add new


                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.Calendar))
                    {
                        queryBuilder.AppendFormat(" and LEFT(CONVERT(varchar, P.PLAN_TRIP_DATE, 112), 6) = @Calendar  "); //-- Fotmat YYYYMM
                        QueryUtils.addParam(command, "Calendar", o.Calendar);// Add new
                    }
                }

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY PLAN_TRIP_DATE DESC  ");
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

                    List<SearchEmailJobForPlanTripCustom> dataRecordList = new List<SearchEmailJobForPlanTripCustom>();
                    SearchEmailJobForPlanTripCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchEmailJobForPlanTripCustom();

                        dataRecord.EmailTemplateName = QueryUtils.getValueAsString(record, "EMAIL_TEMPLATE_NAME");
                        dataRecord.PlanTripName = QueryUtils.getValueAsString(record, "PLAN_TRIP_NAME");
                        dataRecord.PlanTripId = QueryUtils.getValueAsString(record, "PLAN_TRIP_ID");
                        dataRecord.PlanTripDate = QueryUtils.getValueAsString(record, "PLAN_TRIP_DATE");
                        dataRecord.SaleRepName = QueryUtils.getValueAsString(record, "SALE_REP_NAME");
                        dataRecord.SaleSupName = QueryUtils.getValueAsString(record, "SALE_SUP_NAME");
                        dataRecord.EmailTemplateId = QueryUtils.getValueAsString(record, "EMAIL_TEMPLATE_ID");
                        

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

        /*
        public async Task<int> Update(MsBankModel msBrandModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_BANK SET  BANK_NAME_TH=@BankNameTh, BANK_NAME_EN=@BankNameEn, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE BANK_ID=@BankId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("BankNameTh", msBrandModel.BankNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BankNameEn", msBrandModel.BankNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", msBrandModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", msBrandModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BankId", msBrandModel.BankId));// Add New
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

        }*/







    }
}
