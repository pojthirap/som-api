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
using MyFirstAzureWebApp.Models.adm;
using System.Data;

namespace MyFirstAzureWebApp.Business.org
{

    public class AdmSessionImp : IAdmSession
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<AdmSession>> Search(SearchCriteriaBase<AdmSessionCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from ADM_SESSION where VALID = 'Y' ");
                AdmSessionCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.TokenNo))
                    {
                        queryBuilder.AppendFormat(" and TOKEN_NO = @TokenNo ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TokenNo", o.TokenNo));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.IpAddress))
                    {
                        queryBuilder.AppendFormat(" and IP_ADDRESS = @IpAddress ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("IpAddress", o.IpAddress));// Add New
                    }
                }
                // For Paging
                queryBuilder.AppendFormat(" ORDER BY TOKEN_NO  ");
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.AdmSession.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<AdmSession> lst = context.AdmSession.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<AdmSession> searchResult = new EntitySearchResultBase<AdmSession>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }


        public async Task<EntitySearchResultBase<SearchAdmSessionForGetSessionCustom>> SearchAdmSessionForGetSession(SearchCriteriaBase<AdmSessionCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchAdmSessionForGetSessionCustom> searchResult = new EntitySearchResultBase<SearchAdmSessionForGetSessionCustom>();
            List<SearchAdmSessionForGetSessionCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                AdmSessionCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select S.SESSION_ID,S.EMP_ID,IIF(UA.ACCESS_TOKEN_ID is null,'N','Y') USER_ACCESS_FLAG ");
                queryBuilder.AppendFormat(" from ADM_SESSION S ");
                queryBuilder.AppendFormat(" left join ADM_USER_ACCESS_TOKEN UA on UA.EMP_ID = S.EMP_ID ");
                queryBuilder.AppendFormat(" WHERE S.VALID = 'Y' ");
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.TokenNo))
                    {
                        queryBuilder.AppendFormat(" and S.TOKEN_NO = @TokenNo ");
                        QueryUtils.addParam(command, "TokenNo", o.TokenNo);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.IpAddress))
                    {
                        queryBuilder.AppendFormat(" S.and IP_ADDRESS = @IpAddress ");
                        QueryUtils.addParam(command, "IpAddress", o.IpAddress);// Add new
                    }
                }


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY TOKEN_NO  ");
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
                    List<SearchAdmSessionForGetSessionCustom> dataRecordList = new List<SearchAdmSessionForGetSessionCustom>();
                    SearchAdmSessionForGetSessionCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchAdmSessionForGetSessionCustom();
                        dataRecord.SessionId = QueryUtils.getValueAsDecimalRequired(record, "SESSION_ID");
                        dataRecord.EmpId = QueryUtils.getValueAsString(record, "EMP_ID");
                        dataRecord.UserAccessFlag = QueryUtils.getValueAsString(record, "USER_ACCESS_FLAG");

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


        public async Task<AdmSession> Add(AdmSessionModel admSessionModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for ADM_SESSION_SEQ", p);
                        var nextVal = (int)p.Value;
                        string code_ = QueryUtils.padLeft(nextVal.ToString(), '0', 4);

                        StringBuilder queryBuilder = new StringBuilder();
                        var sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder.AppendFormat("INSERT INTO  ADM_SESSION (SESSION_ID, TOKEN_NO, EMP_ID, IP_ADDRESS, USER_AGENT, LAST_ACCESS_DTM, TIMEOUT_DTM, VALID, LOGIN_DTM, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@SESSION_ID ,@TOKEN_NO, @EMP_ID, @IP_ADDRESS, @USER_AGENT, dbo.GET_SYSDATETIME(), dateadd(SECOND,@TokenExpireNumber,dbo.GET_SYSDATETIME()), 'Y', dbo.GET_SYSDATETIME(), @EMP_ID, dbo.GET_SYSDATETIME(), @EMP_ID, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("SESSION_ID", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TOKEN_NO", admSessionModel.TokenNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("EMP_ID", admSessionModel.EmpId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("IP_ADDRESS", admSessionModel.IpAddress));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER_AGENT", admSessionModel.UserAgent));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TokenExpire", admSessionModel.TokenExpire));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameterWithDbType("TokenExpireNumber", admSessionModel.TokenExpire, DbType.Int32));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        AdmSession re = new AdmSession();
                        re.SessionId = nextVal;
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

        public async Task<int> Update(AdmSessionModel admSessionModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ADM_SESSION SET  LAST_ACCESS_DTM=dbo.GET_SYSDATETIME(), TIMEOUT_DTM=dateadd(SECOND,@TokenExpireNumber,dbo.GET_SYSDATETIME()), UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE SESSION_ID=@SESSION_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameterWithDbType("TokenExpireNumber", admSessionModel.TokenExpire, DbType.Int32));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", admSessionModel.EmpId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SESSION_ID", admSessionModel.SessionId));// Add New
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



        public async Task<int> Logout(AdmSessionModel admSessionModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ADM_SESSION SET  VALID = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE SESSION_ID=@SESSION_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", admSessionModel.EmpId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SESSION_ID", admSessionModel.SessionId));// Add New
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
