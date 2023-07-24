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
using System.Data;
using MyFirstAzureWebApp.Models.pospect;

namespace MyFirstAzureWebApp.Business.org
{

    public class ProspectRecommendImp : IProspectRecommend
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<OrgBusinessUnit>> searchProspectRecommend(SearchCriteriaBase<ProspectRecommendCriteria> searchCriteria)
        {

            ProspectRecommendCriteria o = searchCriteria.model;

            EntitySearchResultBase<OrgBusinessUnit> searchResult = new EntitySearchResultBase<OrgBusinessUnit>();
            List<OrgBusinessUnit> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("  select BU.*, PR.PROSP_RECOMM_ID from PROSPECT_RECOMMEND PR inner join ORG_BUSINESS_UNIT BU on BU.BU_ID = PR.BU_ID where 1=1  ");
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.ProspectId))
                    {
                        queryBuilder.AppendFormat(" AND PROSPECT_ID = @ProspectId ");
                        QueryUtils.addParam(command, "ProspectId", o.ProspectId);// Add new
                    }
                }


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY BU_ID  ");
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
                    //

                    List<OrgBusinessUnit> dataRecordList = new List<OrgBusinessUnit>();
                    OrgBusinessUnit dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new OrgBusinessUnit();


                        dataRecord.BuId = QueryUtils.getValueAsDecimalRequired(record, "BU_ID");
                        dataRecord.ProspRecommId = QueryUtils.getValueAsDecimal(record, "PROSP_RECOMM_ID");
                        dataRecord.BuCode = QueryUtils.getValueAsString(record, "BU_CODE");
                        dataRecord.BuNameTh = QueryUtils.getValueAsString(record, "BU_NAME_TH");
                        dataRecord.BuNameEn = QueryUtils.getValueAsString(record, "BU_NAME_EN");
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



        public async Task<ProspectRecommend> addProspectRecommend(ProspectRecommendModel prospectRecommendModel, int buIdIndex)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_RECOMMEND_SEQ", p);
                        var nextVal = (int)p.Value;

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO  PROSPECT_RECOMMEND (PROSP_RECOMM_ID, PROSPECT_ID, BU_ID, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@PROSP_RECOMM_ID ,@PROSPECT_ID, @BU_ID, 'Y', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_RECOMM_ID", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", prospectRecommendModel.ProspectId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BU_ID", prospectRecommendModel.BuIdList[buIdIndex]));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", prospectRecommendModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        ProspectRecommend re = new ProspectRecommend();
                        re.ProspRecommId = nextVal;
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


        public async Task<int> delProspectRecommend(ProspectRecommendModel prospectRecommendModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM PROSPECT_RECOMMEND WHERE PROSP_RECOMM_ID=@PROSP_RECOMM_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_RECOMM_ID", prospectRecommendModel.ProspRecommId));// Add New
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
