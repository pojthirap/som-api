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

    public class ProspectContactImp : IProspectContact
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<ProspectContact>> searchProspectContact(SearchCriteriaBase<SearchProspectContactCriteria> searchCriteria)
        {

            EntitySearchResultBase<ProspectContact> searchResult = new EntitySearchResultBase<ProspectContact>();
            List<ProspectContact> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                SearchProspectContactCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select IIF(PC.PROSPECT_ID=@ProspectId and PA.CUST_CODE is null,'Y','N') EDIT_GENERAL_DATA_FLAG,   ");
                queryBuilder.AppendFormat(" PC.* ");
                queryBuilder.AppendFormat(" from PROSPECT_CONTACT PC   ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ACCOUNT PA on PA.PROSP_ACC_ID = PC.PROSP_ACC_ID   ");
                queryBuilder.AppendFormat(" where PC.ACTIVE_FLAG = 'Y' ");
                QueryUtils.addParam(command, "ProspectId", o.ProspectId);// Add new

                if (!String.IsNullOrEmpty(o.ProsAccId))
                {
                    queryBuilder.AppendFormat(" AND  (PC.PROSPECT_ID = @ProspectId OR (PC.PROSP_ACC_ID = @ProsAccId and  PC.MAIN_FLAG = 'Y'))   ");
                    QueryUtils.addParam(command, "ProsAccId", o.ProsAccId);// Add new
                }
                // For Paging
                queryBuilder.AppendFormat(" ORDER BY PROSP_CONTACT_ID  ");
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

                    List<ProspectContact> prospectAddressLst = new List<ProspectContact>();
                    ProspectContact prospectContact = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        prospectContact = new ProspectContact();

                        prospectContact.EditGeneralDataFlag = QueryUtils.getValueAsString(record, "EDIT_GENERAL_DATA_FLAG");
                        prospectContact.ProspContactId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_CONTACT_ID");
                        prospectContact.ProspectId = QueryUtils.getValueAsDecimal(record, "PROSPECT_ID");
                        prospectContact.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");
                        prospectContact.FirstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                        prospectContact.LastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                        prospectContact.PhoneNo = QueryUtils.getValueAsString(record, "PHONE_NO");
                        prospectContact.FaxNo = QueryUtils.getValueAsString(record, "FAX_NO");
                        prospectContact.MobileNo = QueryUtils.getValueAsString(record, "MOBILE_NO");
                        prospectContact.Email = QueryUtils.getValueAsString(record, "EMAIL");
                        prospectContact.MainFlag = QueryUtils.getValueAsString(record, "MAIN_FLAG");
                        prospectContact.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        prospectContact.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        prospectContact.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        prospectContact.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        prospectContact.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");

                        prospectAddressLst.Add(prospectContact);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //
                    lst = prospectAddressLst;
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }


        public async Task<ProspectContact> addContact(ProspectContactsModel prospectContactModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        // PROSPECT_CONTACT
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_CONTACT_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PROSPECT_CONTACT (PROSP_CONTACT_ID, PROSPECT_ID, PROSP_ACC_ID, FIRST_NAME, LAST_NAME, PHONE_NO, FAX_NO, MOBILE_NO, EMAIL, MAIN_FLAG, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@nextVal ,@ProspectId, @ProspAccId, @FirstName, @LastName, @PhoneNo, @FaxNo, @MobileNo, @Email, 'N', 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProspectId", prospectContactModel.ProspectId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProspAccId", prospectContactModel.ProspAccId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FirstName", prospectContactModel.FirstName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LastName", prospectContactModel.LastName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PhoneNo", prospectContactModel.PhoneNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FaxNo", prospectContactModel.FaxNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("MobileNo", prospectContactModel.MobileNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Email", prospectContactModel.Email));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", prospectContactModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        // PROSPECT_FEED
                        p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_FEED_SEQ", p);
                        nextVal = (int)p.Value;

                        decimal VAL_FUNCTION_TAB = 2;
                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PROSPECT_FEED (FEED_ID, PROSPECT_ID, FUNCTION_TAB, DESCRIPTION, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@FEED_ID ,@PROSPECT_ID, @FUNCTION_TAB, 'New Contact', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("FEED_ID", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", prospectContactModel.ProspectId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FUNCTION_TAB", VAL_FUNCTION_TAB));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", prospectContactModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        transaction.Commit();
                        ProspectContact re = new ProspectContact();
                        re.ProspContactId = nextVal;
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





        public async Task<int> updContact(ProspectContactsModel prospectContactModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        StringBuilder queryBuilder = new StringBuilder();


                        // PROSPECT_FEED
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_FEED_SEQ", p);
                        var nextVal = (int)p.Value;

                        decimal VAL_FUNCTION_TAB = 2;
                        var sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PROSPECT_FEED (FEED_ID, PROSPECT_ID, FUNCTION_TAB, DESCRIPTION, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@FEED_ID ,@PROSPECT_ID, @FUNCTION_TAB, @DESCRIPTION, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("FEED_ID", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", prospectContactModel.ProspectId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FUNCTION_TAB", VAL_FUNCTION_TAB));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DESCRIPTION", prospectContactModel.ChangeField));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", prospectContactModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        // PROSPECT_CONTACT

                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PROSPECT_CONTACT SET FIRST_NAME = @FirstName, LAST_NAME = @LastName, PHONE_NO = @PhoneNo, FAX_NO = @FaxNo, MOBILE_NO = @MobileNo, EMAIL = @Email, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PROSP_CONTACT_ID = @ProspContactId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("FirstName", prospectContactModel.FirstName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LastName", prospectContactModel.LastName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PhoneNo", prospectContactModel.PhoneNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FaxNo", prospectContactModel.FaxNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("MobileNo", prospectContactModel.MobileNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Email", prospectContactModel.Email));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", prospectContactModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProspContactId", prospectContactModel.ProspContactId));// Add New
                        string queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
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






        public async Task<int> delContact(ProspectContactsModel prospectContactModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        StringBuilder queryBuilder = new StringBuilder();

                        // PROSPECT_FEED
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_FEED_SEQ", p);
                        var nextVal = (int)p.Value;

                        decimal VAL_FUNCTION_TAB = 2;
                        var sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PROSPECT_FEED (FEED_ID, PROSPECT_ID, FUNCTION_TAB, DESCRIPTION, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@FEED_ID ,@PROSPECT_ID, @FUNCTION_TAB, @DESCRIPTION, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("FEED_ID", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", prospectContactModel.ProspectId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FUNCTION_TAB", VAL_FUNCTION_TAB));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DESCRIPTION", prospectContactModel.ChangeField));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", prospectContactModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PROSPECT_CONTACT SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PROSP_CONTACT_ID = @PROSP_CONTACT_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", prospectContactModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_CONTACT_ID", prospectContactModel.ProspContactId));// Add New
                        string queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
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

