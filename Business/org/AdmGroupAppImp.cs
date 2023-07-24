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
using MyFirstAzureWebApp.Utils;
using System.Data;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.Models.adm;
using MyFirstAzureWebApp.Models.profile;

namespace MyFirstAzureWebApp.Business.org
{

    public class AdmGroupAppImp : IAdmGroupApp
    {
        private Logger log = LogManager.GetCurrentClassLogger();



        public async Task<EntitySearchResultBase<SearchAdmGroupAppCustom>> searchAdmGroupApp(SearchCriteriaBase<SearchAdmGroupAppCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchAdmGroupAppCustom> searchResult = new EntitySearchResultBase<SearchAdmGroupAppCustom>();
            List<SearchAdmGroupAppCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchAdmGroupAppCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select G.GROUP_NAME_TH,O.PERM_OBJ_NAME_TH,O.PERM_OBJ_CODE,GA.* ");
                queryBuilder.AppendFormat(" from ADM_GROUP_APP GA ");
                queryBuilder.AppendFormat(" inner join ADM_GROUP G on G.GROUP_ID = GA.GROUP_ID ");
                queryBuilder.AppendFormat(" inner join ADM_PERM_OBJECT O on O.PERM_OBJ_ID = GA.APP_ID ");
                queryBuilder.AppendFormat(" where 1= 1 ");

                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.GroupId))
                    {
                        queryBuilder.AppendFormat(" and GA.GROUP_ID = @GROUP_ID  ");
                        QueryUtils.addParam(command, "GROUP_ID", o.GroupId);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.AppId))
                    {
                        queryBuilder.AppendFormat(" and GA.APP_ID = @APP_ID  ");
                        QueryUtils.addParam(command, "APP_ID", o.AppId);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.ActiveFlag))
                    {
                        queryBuilder.AppendFormat(" and GA.ACTIVE_FLAG  = @ActiveFlag  ");
                        QueryUtils.addParam(command, "ActiveFlag", o.ActiveFlag);// Add new
                    }
                }

                

                // For Paging
                queryBuilder.AppendFormat(" order by GA.UPDATE_DTM DESC  ");
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

                    List<SearchAdmGroupAppCustom> dataRecordList = new List<SearchAdmGroupAppCustom>();
                    SearchAdmGroupAppCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchAdmGroupAppCustom();


                        dataRecord.GroupNameTh = QueryUtils.getValueAsString(record, "GROUP_NAME_TH");
                        dataRecord.PermObjNameTh = QueryUtils.getValueAsString(record, "PERM_OBJ_NAME_TH");
                        dataRecord.PermObjCode = QueryUtils.getValueAsString(record, "PERM_OBJ_CODE");
                        dataRecord.GroupAppId = QueryUtils.getValueAsDecimalRequired(record, "GROUP_APP_ID");
                        dataRecord.GroupId = QueryUtils.getValueAsDecimal(record, "GROUP_ID");
                        dataRecord.AppId = QueryUtils.getValueAsDecimal(record, "APP_ID");
                        dataRecord.EffectiveDate = QueryUtils.getValueAsDateTime(record, "EFFECTIVE_DATE");
                        dataRecord.ExpiryDate = QueryUtils.getValueAsDateTime(record, "EXPIRY_DATE");

                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
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




        public async Task<AdmGroupApp> addAdmGroupApp(AddAdmGroupAppModel addAdmGroupAppModel, UserProfileForBack userProfile)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for ADM_GROUP_APP_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();

                        queryBuilder.AppendFormat(" INSERT INTO ADM_GROUP_APP ([GROUP_APP_ID], [GROUP_ID], [APP_ID], [ACTIVE_FLAG], [EFFECTIVE_DATE], [EXPIRY_DATE], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM])  ");
                        queryBuilder.AppendFormat(" VALUES(@GROUP_APP_ID, @GROUP_ID, @APP_ID, 'Y', @EFFECTIVE_DATE, @EXPIRY_DATE, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME()) ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_APP_ID", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_ID", addAdmGroupAppModel.GroupId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("APP_ID", addAdmGroupAppModel.AppId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("EFFECTIVE_DATE", addAdmGroupAppModel.EffectiveDate));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("EXPIRY_DATE", addAdmGroupAppModel.ExpiryDate));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        AdmGroupApp re = new AdmGroupApp();
                        re.GroupAppId = nextVal;
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


        public async Task<int> updAdmGroupApp(UpdAdmGroupAppModel updAdmGroupAppModel, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" UPDATE ADM_GROUP_APP  ");
                        queryBuilder.AppendFormat(" SET [GROUP_ID]=@GROUP_ID, [APP_ID]=@APP_ID, [ACTIVE_FLAG]=@ACTIVE_FLAG, [EFFECTIVE_DATE]=@EFFECTIVE_DATE, [EXPIRY_DATE]=@EXPIRY_DATE, [UPDATE_USER]=@USER, [UPDATE_DTM]=dbo.GET_SYSDATETIME() ");
                        queryBuilder.AppendFormat(" WHERE GROUP_APP_ID = @GROUP_APP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_ID", updAdmGroupAppModel.GroupId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("APP_ID", updAdmGroupAppModel.AppId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ACTIVE_FLAG", updAdmGroupAppModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("EFFECTIVE_DATE", updAdmGroupAppModel.EffectiveDate));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("EXPIRY_DATE", updAdmGroupAppModel.ExpiryDate));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_APP_ID", updAdmGroupAppModel.GroupAppId));// Add New
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


        public async Task<int> cancelAdmGroupApp(CancelAdmGroupAppModel cancelAdmGroupAppModel, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" UPDATE ADM_GROUP_APP  ");
                        queryBuilder.AppendFormat(" SET [ACTIVE_FLAG]='N', [UPDATE_USER]=@USER, [UPDATE_DTM]=dbo.GET_SYSDATETIME() ");
                        queryBuilder.AppendFormat(" WHERE GROUP_APP_ID = @GROUP_APP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_APP_ID", cancelAdmGroupAppModel.GroupAppId));// Add New
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



        public async Task<EntitySearchResultBase<SearchAdmGroupPermCustom>> searchAdmGroupPerm(SearchCriteriaBase<SearchAdmGroupPermCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchAdmGroupPermCustom> searchResult = new EntitySearchResultBase<SearchAdmGroupPermCustom>();
            List<SearchAdmGroupPermCustom> lst = new List<SearchAdmGroupPermCustom>();
            SearchAdmGroupPermCustom response = new SearchAdmGroupPermCustom();
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchAdmGroupPermCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();


                string VAL_GROUP_APP_ID = null;
                queryBuilder.AppendFormat(" select GROUP_APP_ID From ADM_GROUP_APP where GROUP_ID = @GROUP_ID and APP_ID = @APP_ID ");
                QueryUtils.addParam(command, "GROUP_ID", o.GroupId);// Add new
                QueryUtils.addParam(command, "APP_ID", o.AppId);// Add new

                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    //

                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;


                        VAL_GROUP_APP_ID = QueryUtils.getValueAsString(record, "GROUP_APP_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //
                }

                queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" WITH n(PERM_OBJ_ID, PERM_OBJ_CODE, PERM_OBJ_NAME_TH, level, PARENT_ID, concatenador) AS  ");
                queryBuilder.AppendFormat(" (");
                queryBuilder.AppendFormat("         SELECT PERM_OBJ_ID, PERM_OBJ_CODE, PERM_OBJ_NAME_TH, 1 as level, PARENT_ID, '('+CONVERT(VARCHAR (MAX), PERM_OBJ_ID)+' - '+CONVERT(VARCHAR (MAX), 1)+')' as concatenador ");
                queryBuilder.AppendFormat("         FROM ADM_PERM_OBJECT ");
                queryBuilder.AppendFormat("         WHERE PERM_OBJ_ID = @APP_ID ");
                queryBuilder.AppendFormat("         UNION ALL ");
                queryBuilder.AppendFormat("         SELECT m.PERM_OBJ_ID, m.PERM_OBJ_CODE, m.PERM_OBJ_NAME_TH, n.level + 1, m.PARENT_ID ");
                queryBuilder.AppendFormat("         , n.concatenador + ' * (' + RIGHT('00' + CONVERT(VARCHAR(MAX), case when ISNULL(m.PARENT_ID, 0) = 0 then 0 else m.PERM_OBJ_ID END),3)+' - ' + CONVERT(VARCHAR(MAX), n.level + 1) + ')' as concatenador ");
                queryBuilder.AppendFormat("         FROM ADM_PERM_OBJECT as m, n ");
                queryBuilder.AppendFormat("         WHERE n.PERM_OBJ_ID = m.PARENT_ID ");
                queryBuilder.AppendFormat(" ) ");
                queryBuilder.AppendFormat(" SELECT n.PERM_OBJ_ID,n.PERM_OBJ_CODE, n.PERM_OBJ_NAME_TH,n.level,n.PARENT_ID,IIF(GP.PERM_OBJ_ID is null,'N','Y') SELECTED_FLAG ");
                queryBuilder.AppendFormat(" FROM n  ");
                queryBuilder.AppendFormat(" LEFT JOIN ADM_GROUP_PERM GP ON (GP.PERM_OBJ_ID = n.PERM_OBJ_ID and GP.GROUP_APP_ID = @VAL_GROUP_APP_ID) ");
                queryBuilder.AppendFormat(" ORDER BY concatenador asc ");
                QueryUtils.addParam(command, "VAL_GROUP_APP_ID", VAL_GROUP_APP_ID);// Add new

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

                    List<SearchAdmGroupPermItem> dataRecordList = new List<SearchAdmGroupPermItem>();
                    SearchAdmGroupPermItem dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchAdmGroupPermItem();

                        dataRecord.PermObjId = QueryUtils.getValueAsString(record, "PERM_OBJ_ID");
                        dataRecord.PermObjCode = QueryUtils.getValueAsString(record, "PERM_OBJ_CODE");
                        dataRecord.Level = QueryUtils.getValueAsString(record, "level");
                        dataRecord.ParentId = QueryUtils.getValueAsString(record, "PARENT_ID");
                        dataRecord.SelectedFlag = QueryUtils.getValueAsString(record, "SELECTED_FLAG");
                        dataRecord.PermObjNameTh = QueryUtils.getValueAsString(record, "PERM_OBJ_NAME_TH"); 

                        dataRecordList.Add(dataRecord);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //
                    response.groupAppId = VAL_GROUP_APP_ID;
                    response.Item = dataRecordList;
                    lst.Add(response);
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }


            }
            return searchResult;
        }




        public async Task<int> updAdmGroupPerm(UpdAdmGroupPermModel updAdmGroupPermModel, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" DELETE FROM [ADM_GROUP_PERM] WHERE GROUP_APP_ID = @GROUP_APP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_APP_ID", updAdmGroupPermModel.GroupAppId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        foreach (string objId in updAdmGroupPermModel.PermObjId)
                        {
                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat(" INSERT INTO ADM_GROUP_PERM ([GROUP_PERM_ID], [GROUP_APP_ID], [PERM_OBJ_ID], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM])  ");
                            queryBuilder.AppendFormat(" VALUES(NEXT VALUE FOR ADM_GROUP_PERM_SEQ, @GROUP_APP_ID, @objId, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME()) ");
                            sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_APP_ID", updAdmGroupPermModel.GroupAppId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("objId", objId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
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






    }
}
