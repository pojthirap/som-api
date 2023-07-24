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

namespace MyFirstAzureWebApp.Business.org
{

    public class ProspectVisitHourImpImp : IProspectVisitHour
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<SearchVisitHourCustom>> searchVisitHour(SearchCriteriaBase<SearchProspectVisitHourCriteria> searchCriteria)
        {

            SearchProspectVisitHourCriteria o = searchCriteria.model;
            EntitySearchResultBase<SearchVisitHourCustom> searchResult = new EntitySearchResultBase<SearchVisitHourCustom>();
            List<SearchVisitHourCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select VH.ACTIVE_FLAG AS VH_ACTIVE_FLAG, VH.*, C.ACTIVE_FLAG AS C_ACTIVE_FLAG, C.* from PROSPECT_VISIT_HOUR VH  ");
                queryBuilder.AppendFormat(" inner join MS_CONFIG_LOV C on C.LOV_KEYWORD = 'DAYS_CODE' and C.LOV_KEYVALUE = VH.DAYS_CODE ");
                queryBuilder.AppendFormat(" where 1=1 ");

                if (!String.IsNullOrEmpty(o.ProspectId))
                {
                    queryBuilder.AppendFormat("   AND VH.PROSPECT_ID =@ProspectId   ");
                    QueryUtils.addParam(command, "ProspectId", o.ProspectId);// Add new
                }

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY VH.PROSPECT_ID  ");
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

                    List<SearchVisitHourCustom> VisitHourCustomLst = new List<SearchVisitHourCustom>();
                    SearchVisitHourCustom prospectVisitHour = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        prospectVisitHour = new SearchVisitHourCustom();

                        prospectVisitHour.ProspVisitHrId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_VISIT_HR_ID");
                        prospectVisitHour.ProspectId = QueryUtils.getValueAsDecimal(record, "PROSPECT_ID");
                        prospectVisitHour.DaysCode = QueryUtils.getValueAsString(record, "DAYS_CODE");
                        prospectVisitHour.HourStart = QueryUtils.getValueAsString(record, "HOUR_START");
                        prospectVisitHour.HourEnd = QueryUtils.getValueAsString(record, "HOUR_END");
                        prospectVisitHour.VisitHourActiveFlag = QueryUtils.getValueAsString(record, "VH_ACTIVE_FLAG");
                        prospectVisitHour.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        prospectVisitHour.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        prospectVisitHour.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        prospectVisitHour.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");



                        prospectVisitHour.LovActiveFlag = QueryUtils.getValueAsString(record, "C_ACTIVE_FLAG");
                        prospectVisitHour.LovKeyword = QueryUtils.getValueAsString(record, "LOV_KEYWORD");
                        prospectVisitHour.LovKeyvalue = QueryUtils.getValueAsDecimalRequired(record, "LOV_KEYVALUE");
                        prospectVisitHour.LovNameTh = QueryUtils.getValueAsString(record, "LOV_NAME_TH");
                        prospectVisitHour.LovNameEn = QueryUtils.getValueAsString(record, "LOV_NAME_EN");
                        prospectVisitHour.LovCodeTh = QueryUtils.getValueAsString(record, "LOV_CODE_TH");
                        prospectVisitHour.LovCodeEn = QueryUtils.getValueAsString(record, "LOV_CODE_EN");
                        prospectVisitHour.LovOrder = QueryUtils.getValueAsDecimal(record, "LOV_ORDER");
                        prospectVisitHour.LovRemark = QueryUtils.getValueAsString(record, "LOV_REMARK");
                        prospectVisitHour.Condition1 = QueryUtils.getValueAsString(record, "CONDITION1");

                        VisitHourCustomLst.Add(prospectVisitHour);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //
                    lst = VisitHourCustomLst;
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }


        public async Task<List<ProspectVisitHour>> addVisitHour(ProspectVisitHourModel prospectVisitHourModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        List<ProspectVisitHour> lst = new List<ProspectVisitHour>();
                        if (prospectVisitHourModel.DaysCode.Length != 0)
                        {
                            for (int i = 0; i < prospectVisitHourModel.DaysCode.Length; i++)
                            {
                                var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                                p.Direction = System.Data.ParameterDirection.Output;
                                context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_VISIT_HOUR_SEQ", p);
                                var nextVal = (int)p.Value;

                                var sqlParameters = new List<SqlParameter>();// Add New
                                StringBuilder queryBuilder = new StringBuilder();
                                queryBuilder.AppendFormat("INSERT INTO PROSPECT_VISIT_HOUR (PROSP_VISIT_HR_ID, PROSPECT_ID, DAYS_CODE, HOUR_START, HOUR_END, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                                queryBuilder.AppendFormat("VALUES(@PROSP_VISIT_HR_ID ,@PROSPECT_ID, @DAYS_CODE, @HOUR_START, @HOUR_END, 'Y', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                                sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_VISIT_HR_ID", nextVal));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", prospectVisitHourModel.ProspectId));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("DAYS_CODE", prospectVisitHourModel.DaysCode[i]));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("HOUR_START", prospectVisitHourModel.HourStart));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("HOUR_END", prospectVisitHourModel.HourEnd));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("USER", prospectVisitHourModel.getUserName()));// Add New
                                log.Debug("Query:" + queryBuilder.ToString());
                                Console.WriteLine("Query:" + queryBuilder.ToString());
                                int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                ProspectVisitHour re = new ProspectVisitHour();
                                re.ProspVisitHrId = nextVal;
                                lst.Add(re);
                            }
                            transaction.Commit();
                        }
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







        public async Task<int> delVisitHour(ProspectVisitHourModel prospectVisitHourModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM PROSPECT_VISIT_HOUR  WHERE PROSP_VISIT_HR_ID = @PROSP_VISIT_HR_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_VISIT_HR_ID", prospectVisitHourModel.ProspVisitHrId));// Add New
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



    }
}
