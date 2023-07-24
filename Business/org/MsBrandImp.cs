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

    public class MsBrandImp : IMsBrand
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<MsBrand>> Search(MsBrandSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from MS_BRAND where 1=1 ");
                MsBrandCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.BrandId))
                    {
                        queryBuilder.AppendFormat(" and BRAND_ID = @BrandId ", o.BrandId);
                        sqlParameters.Add(QueryUtils.addSqlParameter("BrandId", o.BrandId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.BrandCode))
                    {
                        queryBuilder.AppendFormat(" and BRAND_CODE = @BrandCode ", o.BrandCode);
                        sqlParameters.Add(QueryUtils.addSqlParameter("BrandCode", o.BrandCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.BrandNameTh))
                    {
                        queryBuilder.AppendFormat(" and BRAND_NAME_TH like @BrandNameTh   ", o.BrandNameTh);
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("BrandNameTh",  o.BrandNameTh ));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.ActiveFlag))
                    {
                        queryBuilder.AppendFormat(" and ACTIVE_FLAG = @ActiveFlag ", o.ActiveFlag);
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", o.ActiveFlag));// Add New
                    }
                }
                // For Paging
                if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY UPDATE_DTM DESC  ");
                }else if (2 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY BRAND_NAME_TH  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY BRAND_CODE  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.MsBrand.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();// Add New
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<MsBrand> lst = context.MsBrand.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList(); // Add New
                EntitySearchResultBase<MsBrand> searchResult = new EntitySearchResultBase<MsBrand>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }

        public async Task<MsBrand> Add(MsBrandModel msBrandModel)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for MS_BRAND_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  MS_BRAND (BRAND_ID, BRAND_CODE, BRAND_NAME_TH, BRAND_NAME_EN, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @BRAND_ID, RIGHT('00000' + CAST(ISNULL((MAX(CAST(BRAND_CODE AS INT)) + 1),1) AS VARCHAR), 5), @BRAND_NAME_TH, @BRAND_NAME_EN, 'Y', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME() from MS_BRAND ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("BRAND_ID", nextVal ));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BRAND_NAME_TH", msBrandModel.BrandNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BRAND_NAME_EN", msBrandModel.BrandNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", msBrandModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        MsBrand re = new MsBrand();
                        re.BrandId = nextVal;
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


        public async Task<int> Update(MsBrandModel msBrandModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_BRAND SET BRAND_NAME_TH=@BRAND_NAME_TH, BRAND_NAME_EN=@BRAND_NAME_EN, ACTIVE_FLAG=@ACTIVE_FLAG, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE BRAND_ID=@BRAND_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("BRAND_NAME_TH", msBrandModel.BrandNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BRAND_NAME_EN", msBrandModel.BrandNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ACTIVE_FLAG", msBrandModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", msBrandModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BRAND_ID", msBrandModel.BrandId));// Add New
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

        
        public async Task<int> DeleteUpdate(MsBrandModel msBrandModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_BRAND SET ACTIVE_FLAG = 'N', UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE BRAND_ID=@BRAND_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", msBrandModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BRAND_ID", msBrandModel.BrandId));// Add New
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
