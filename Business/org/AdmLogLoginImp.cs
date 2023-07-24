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

namespace MyFirstAzureWebApp.Business.org
{

    public class AdmLogloginImp : IAdmLogLogin
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<AdmLogLogin> Add(AdmLogLoginModel admLogLoginModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for ADM_LOG_LOGIN_SEQ", p);
                        var nextVal = (int)p.Value;

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO  ADM_LOG_LOGIN (LOG_LOGIN_ID, USER_NAME, LOGIN_DTM, STATUS, ERROR_DESCRIPTION, IP_ADDRESS, USER_AGENT ) ");
                        queryBuilder.AppendFormat("VALUES(@nextVal ,@UserName, dbo.GET_SYSDATETIME(), @Status, @ErrorDescription, @IpAddress, @UserAgent)");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UserName", admLogLoginModel.UserName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Status", admLogLoginModel.Status));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ErrorDescription", admLogLoginModel.ErrorDescription));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("IpAddress", admLogLoginModel.IpAddress));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UserAgent", admLogLoginModel.UserAgent));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        AdmLogLogin re = new AdmLogLogin();
                        re.LogLoginId = nextVal;
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
