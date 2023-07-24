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

    public class MsDistrictImp : IMsDistrict
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<MsDistrict>> Search(SearchCriteriaBase<MsDistrictCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from MS_DISTRICT where 1=1 ");
                MsDistrictCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.DistrictCode))
                    {
                        queryBuilder.AppendFormat(" and DISTRICT_CODE  = @DistrictCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DistrictCode", o.DistrictCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.ProvinceCode))
                    {
                        queryBuilder.AppendFormat(" and PROVINCE_CODE  = @ProvinceCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ProvinceCode", o.ProvinceCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.DistrictNameTh))
                    {
                        queryBuilder.AppendFormat(" and DISTRICT_NAME_TH like @DistrictNameTh  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("DistrictNameTh", o.DistrictNameTh));// Add New
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
                    queryBuilder.AppendFormat(" ORDER BY DISTRICT_NAME_TH  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY DISTRICT_CODE  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.MsDistrict.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<MsDistrict> lst = context.MsDistrict.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<MsDistrict> searchResult = new EntitySearchResultBase<MsDistrict>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }
        public async Task<int> updDistrictByProvinceCode(MsDistrictModel msDistrictModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_DISTRICT SET PROVINCE_CODE = @PROVINCE_CODE, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE DISTRICT_CODE=@DISTRICT_CODE ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROVINCE_CODE", msDistrictModel.ProvinceCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", msDistrictModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DISTRICT_CODE", msDistrictModel.DistrictCode));// Add New
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
