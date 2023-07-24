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

    public class MsLocationTypeImp : IMsLocationType
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<MsLocationType>> Search(MsLocationTypeSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from MS_LOCATION_TYPE where 1=1 ");
                MsLocationTypeCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.LocTypeId))
                    {
                        queryBuilder.AppendFormat(" and LOC_TYPE_ID  = @LocTypeId  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocTypeId", o.LocTypeId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.LocTypeCode))
                    {
                        queryBuilder.AppendFormat(" and LOC_TYPE_CODE  = @LocTypeCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocTypeCode", o.LocTypeCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.LocTypeNameTh))
                    {
                        queryBuilder.AppendFormat(" and LOC_TYPE_NAME_TH like @LocTypeNameTh  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("LocTypeNameTh", o.LocTypeNameTh));// Add New
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
                }else if (2 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY LOC_TYPE_NAME_TH  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY LOC_TYPE_CODE  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.MsLocationType.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<MsLocationType> lst = context.MsLocationType.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<MsLocationType> searchResult = new EntitySearchResultBase<MsLocationType>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }


        public async Task<MsLocationType> Add(MsLocationTypeModel msLocationTypeModel)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for MS_LOCATION_TYPE_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  MS_LOCATION_TYPE (LOC_TYPE_ID, LOC_TYPE_CODE, LOC_TYPE_NAME_TH, LOC_TYPE_NAME_EN, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, RIGHT('00000' + CAST(ISNULL((MAX(CAST(LOC_TYPE_CODE AS INT)) + 1),1) AS VARCHAR), 5), @LocTypeNameTh, @LocTypeNameEn, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from MS_LOCATION_TYPE ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocTypeNameTh", msLocationTypeModel.LocTypeNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocTypeNameEn", msLocationTypeModel.LocTypeNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", msLocationTypeModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        MsLocationType re = new MsLocationType();
                        re.LocTypeId = nextVal;
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



        public async Task<int> Update(MsLocationTypeModel msLocationTypeModel)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_LOCATION_TYPE SET LOC_TYPE_NAME_TH = @LocTypeNameTh,  LOC_TYPE_NAME_EN = @LocTypeNameEn, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE LOC_TYPE_ID=@LocTypeId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocTypeNameTh", msLocationTypeModel.LocTypeNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocTypeNameEn", msLocationTypeModel.LocTypeNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", msLocationTypeModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", msLocationTypeModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LocTypeId", msLocationTypeModel.LocTypeId));// Add New
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



        public async Task<int> DeleteUpdate(MsLocationTypeModel msLocationTypeModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_LOCATION_TYPE SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE LOC_TYPE_ID=@LOC_TYPE_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", msLocationTypeModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LOC_TYPE_ID", msLocationTypeModel.LocTypeId));// Add New
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
