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

namespace MyFirstAzureWebApp.Business.org
{

    public class ProvinceImp : IProvince
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<MsProvince>> Search(ProvinceSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from MS_PROVINCE where 1=1 ");
                ProvinceCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.regionCode))
                    {
                        queryBuilder.AppendFormat(" and REGION_CODE  = @regionCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("regionCode", o.regionCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.provinceCode))
                    {
                        queryBuilder.AppendFormat(" and PROVINCE_CODE  = @provinceCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("provinceCode", o.provinceCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.provinceName))
                    {
                        queryBuilder.AppendFormat(" and PROVINCE_NAME_TH  like @provinceName  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("provinceName", o.provinceName));// Add New

                    }
                    if (!String.IsNullOrEmpty(o.activeFlag))
                    {
                        queryBuilder.AppendFormat(" and ACTIVE_FLAG  = @ActiveFlag  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", o.activeFlag));// Add New
                    }
                }
                // For Paging
                if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY UPDATE_DTM DESC  ");
                }else if (2 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY PROVINCE_NAME_TH  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY PROVINCE_CODE  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.MsProvince.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<MsProvince> lst = context.MsProvince.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<MsProvince> searchResult = new EntitySearchResultBase<MsProvince>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }


        public async Task<EntitySearchResultBase<MsProvince>> searchProvinceForMapRegion(ProvinceSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from MS_PROVINCE  where REGION_CODE IS NULL ");
                ProvinceCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.provinceCode))
                    {
                        queryBuilder.AppendFormat(" and PROVINCE_CODE = @provinceCode ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("provinceCode", o.provinceCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.provinceName))
                    {

                        queryBuilder.AppendFormat(" and PROVINCE_NAME_TH like @provinceName   ");
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("provinceName", o.provinceName));// Add New

                    }
                    if (!String.IsNullOrEmpty(o.activeFlag))
                    {
                        queryBuilder.AppendFormat(" and ACTIVE_FLAG  = @ActiveFlag  ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", o.activeFlag));// Add New
                    }
                }
                // For Paging
                queryBuilder.AppendFormat(" ORDER BY PROVINCE_CODE  ");
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.MsProvince.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<MsProvince> lst = context.MsProvince.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<MsProvince> searchResult = new EntitySearchResultBase<MsProvince>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }

        public async Task<int> mapRegionToProvince(RegionModel msRegion)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_REGION  SET [UPDATE_USER]= @UPDATE_USER, [UPDATE_DTM] = dbo.GET_SYSDATETIME() WHERE REGION_CODE = @REGION_CODE ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", msRegion.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("REGION_CODE", msRegion.regionCode));// Add New
                        string queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        String provinceCodeStr = "";
                        int length_ = msRegion.provinceCodeList.Length;
                        String[] provinceCodeList = new String[length_];
                        for (int i = 0; i < length_; i++)
                        {
                            provinceCodeList[i] = msRegion.provinceCodeList[i];
                        }
                        provinceCodeStr = String.Join(",", provinceCodeList);
                        queryBuilder = new StringBuilder();
                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder.AppendFormat("UPDATE MS_PROVINCE SET REGION_CODE = @REGION_CODE, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PROVINCE_CODE in (" + QueryUtils.getParamIn("provinceCodeStr", provinceCodeStr) + ") ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("REGION_CODE", msRegion.regionCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", msRegion.getUserName()));// Add New
                        QueryUtils.addParamIn(sqlParameters, "provinceCodeStr", provinceCodeStr);
                        queryStr = queryBuilder.ToString();
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
