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
using MyFirstAzureWebApp.Models.plan;

namespace MyFirstAzureWebApp.Business.org
{

    public class PlanReasonNotVisitImp : IPlanReasonNotVisit
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<PlanReasonNotVisit>> Search(SearchCriteriaBase<PlanReasonNotVisitCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from PLAN_REASON_NOT_VISIT where 1=1 ");
                PlanReasonNotVisitCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.ReasonNotVisitId))
                    {
                        queryBuilder.AppendFormat(" and REASON_NOT_VISIT_ID  = @ReasonNotVisitId  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ReasonNotVisitId", o.ReasonNotVisitId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.ReasonCode))
                    {
                        queryBuilder.AppendFormat(" and REASON_CODE  = @ReasonCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ReasonCode", o.ReasonCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.ReasonNameTh))
                    {
                        queryBuilder.AppendFormat(" and REASON_NAME_TH like @ReasonNameTh  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("ReasonNameTh", o.ReasonNameTh));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.ActiveFlag))
                    {
                        queryBuilder.AppendFormat(" and ACTIVE_FLAG  = @ActiveFlag  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", o.ActiveFlag));// Add New
                    }
                }
                // For Paging
                if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY UPDATE_DTM DESC  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY REASON_CODE  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.PlanReasonNotVisit.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<PlanReasonNotVisit> lst = context.PlanReasonNotVisit.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<PlanReasonNotVisit> searchResult = new EntitySearchResultBase<PlanReasonNotVisit>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }

        public async Task<PlanReasonNotVisit> Add(PlanReasonNotVisitModel planReasonNotVisitModel)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for PLAN_REASON_NOT_VISIT_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  PLAN_REASON_NOT_VISIT (REASON_NOT_VISIT_ID, REASON_CODE, REASON_NAME_TH, REASON_NAME_EN, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, RIGHT('00000' + CAST(ISNULL((MAX(CAST(REASON_CODE AS INT)) + 1),1) AS VARCHAR), 5), @ReasonNameTh, @ReasonNameEn, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from PLAN_REASON_NOT_VISIT ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ReasonNameTh", planReasonNotVisitModel.ReasonNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ReasonNameEn", planReasonNotVisitModel.ReasonNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", planReasonNotVisitModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        PlanReasonNotVisit re = new PlanReasonNotVisit();
                        re.ReasonNotVisitId = nextVal;
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

        public async Task<int> Update(PlanReasonNotVisitModel planReasonNotVisitModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PLAN_REASON_NOT_VISIT SET REASON_NAME_TH=@ReasonNameTh, REASON_NAME_EN=@ReasonNameEn, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE REASON_NOT_VISIT_ID=@ReasonNotVisitId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ReasonNameTh", planReasonNotVisitModel.ReasonNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ReasonNameEn", planReasonNotVisitModel.ReasonNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", planReasonNotVisitModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", planReasonNotVisitModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ReasonNotVisitId", planReasonNotVisitModel.ReasonNotVisitId));// Add New
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


        public async Task<int> DeleteUpdate(PlanReasonNotVisitModel planReasonNotVisitModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PLAN_REASON_NOT_VISIT SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE REASON_NOT_VISIT_ID=@REASON_NOT_VISIT_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", planReasonNotVisitModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("REASON_NOT_VISIT_ID", planReasonNotVisitModel.ReasonNotVisitId));// Add New
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
