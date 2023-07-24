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

namespace MyFirstAzureWebApp.Business.org
{

    public class MsServiceTypeImp : IMsServiceType
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<MsServiceType>> Search(MsServiceTypeSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from MS_SERVICE_TYPE where 1=1 ");
                MsServiceTypeCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.ServiceTypeId))
                    {
                        queryBuilder.AppendFormat(" and SERVICE_TYPE_ID  = @ServiceTypeId  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ServiceTypeId", o.ServiceTypeId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.ServiceCode))
                    {
                        queryBuilder.AppendFormat(" and SERVICE_CODE  = @ServiceCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ServiceCode", o.ServiceCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.ServiceNameTh))
                    {
                        queryBuilder.AppendFormat(" and SERVICE_NAME_TH like @ServiceNameTh  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("ServiceNameTh", o.ServiceNameTh));// Add New
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
                    queryBuilder.AppendFormat(" ORDER BY SERVICE_CODE  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.MsServiceType.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<MsServiceType> lst = context.MsServiceType.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<MsServiceType> searchResult = new EntitySearchResultBase<MsServiceType>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }

        public async Task<MsServiceType> Add(MsServiceTypeModel msServiceTypeModel)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for MS_SERVICE_TYPE_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  MS_SERVICE_TYPE (SERVICE_TYPE_ID, SERVICE_CODE, SERVICE_NAME_TH, SERVICE_NAME_EN, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, RIGHT('00000' + CAST(ISNULL((MAX(CAST(SERVICE_CODE AS INT)) + 1),1) AS VARCHAR), 5), @ServiceNameTh, @ServiceNameEn, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from MS_SERVICE_TYPE ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ServiceNameTh", msServiceTypeModel.ServiceNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ServiceNameEn", msServiceTypeModel.ServiceNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", msServiceTypeModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        MsServiceType re = new MsServiceType();
                        re.ServiceTypeId = nextVal;
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

        public async Task<int> Update(MsServiceTypeModel msServiceTypeModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_SERVICE_TYPE SET SERVICE_NAME_TH=@ServiceNameTh, SERVICE_NAME_EN=@ServiceNameEn, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE SERVICE_TYPE_ID=@ServiceTypeId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ServiceNameTh", msServiceTypeModel.ServiceNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ServiceNameEn", msServiceTypeModel.ServiceNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", msServiceTypeModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", msServiceTypeModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ServiceTypeId", msServiceTypeModel.ServiceTypeId));// Add New
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


        public async Task<int> DeleteUpdate(MsServiceTypeModel msServiceTypeModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_SERVICE_TYPE SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE SERVICE_TYPE_ID=@SERVICE_TYPE_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", msServiceTypeModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SERVICE_TYPE_ID", msServiceTypeModel.ServiceTypeId));// Add New
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
