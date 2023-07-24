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
using MyFirstAzureWebApp.Models.pospect;

namespace MyFirstAzureWebApp.Business.org
{

    public class ProspectDedicateTertImp : IProspectDedicateTert
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<List<ProspectDedicateTert>> addProspectDedicated(ProspectDedicateTertModel prospectDedicateTertModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        List<ProspectDedicateTert> prospectDedicateTertList = new List<ProspectDedicateTert>();
                        foreach(string territoryId in prospectDedicateTertModel.TerritoryId) {
                            var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                            p.Direction = System.Data.ParameterDirection.Output;
                            context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_DEDICATE_TERT_SEQ", p);
                            var nextVal = (int)p.Value;

                            StringBuilder queryBuilder = new StringBuilder();
                            var sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder.AppendFormat("INSERT INTO PROSPECT_DEDICATE_TERT (PROSP_DEDICATE_ID, PROSPECT_ID, TERRITORY_ID, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                            queryBuilder.AppendFormat("VALUES(@PROSP_DEDICATE_ID ,@PROSPECT_ID, @TERRITORY_ID, 'Y', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                            sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_DEDICATE_ID", nextVal));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", prospectDedicateTertModel.ProspectId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("TERRITORY_ID", territoryId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", prospectDedicateTertModel.getUserName()));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                            ProspectDedicateTert re = new ProspectDedicateTert();
                            re.ProspDedicateId = nextVal;
                            prospectDedicateTertList.Add(re);
                        }
                        transaction.Commit();
                        return prospectDedicateTertList;


                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }
        




        public async Task<int> delProspectDedicated(ProspectDedicateTertModel prospectDedicateTertModel)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM PROSPECT_DEDICATE_TERT  WHERE PROSP_DEDICATE_ID=@PROSP_DEDICATE_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_DEDICATE_ID", prospectDedicateTertModel.ProspDedicateId));// Add New
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
