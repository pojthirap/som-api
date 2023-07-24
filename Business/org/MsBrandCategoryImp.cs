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

    public class MsBrandCategoryImp : IMsBrandCategory
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<MsBrandCategory>> Search(MsBrandCategorySearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from MS_BRAND_CATEGORY where 1=1 ");
                MsBrandCategoryCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.BrandCateId))
                    {
                        queryBuilder.AppendFormat(" and BRAND_CATE_ID  = @BrandCateId  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BrandCateId", o.BrandCateId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.BrandCateCode))
                    {
                        queryBuilder.AppendFormat(" and BRAND_CATE_CODE  = @BrandCateCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BrandCateCode", o.BrandCateCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.BrandCateNameTh))
                    {
                        queryBuilder.AppendFormat(" and BRAND_CATE_NAME_TH like @BrandCateNameTh  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("BrandCateNameTh", o.BrandCateNameTh));// Add New
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
                    queryBuilder.AppendFormat(" ORDER BY BRAND_CATE_CODE  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.MsBrandCategory.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<MsBrandCategory> lst = context.MsBrandCategory.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<MsBrandCategory> searchResult = new EntitySearchResultBase<MsBrandCategory>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }


        public async Task<MsBrandCategory> Add(MsBrandCategoryModel msBrandCategoryModel)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for MS_BRAND_CATEGORY_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  MS_BRAND_CATEGORY (BRAND_CATE_ID, BRAND_CATE_CODE, BRAND_CATE_NAME_TH, BRAND_CATE_NAME_EN, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, RIGHT('00000' + CAST(ISNULL((MAX(CAST(BRAND_CATE_CODE AS INT)) + 1),1) AS VARCHAR), 5), @BrandCateNameTh, @BrandCateNameEn, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from MS_BRAND_CATEGORY ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BrandCateNameTh", msBrandCategoryModel.BrandCateNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BrandCateNameEn", msBrandCategoryModel.BrandCateNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", msBrandCategoryModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        MsBrandCategory re = new MsBrandCategory();
                        re.BrandCateId = nextVal;
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



        public async Task<int> Update(MsBrandCategoryModel msBrandCategoryModel)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_BRAND_CATEGORY SET  BRAND_CATE_NAME_TH = @BrandCateNameTh,  BRAND_CATE_NAME_EN = @BrandCateNameEn, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE BRAND_CATE_ID=@BrandCateId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("BrandCateNameTh", msBrandCategoryModel.BrandCateNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BrandCateNameEn", msBrandCategoryModel.BrandCateNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", msBrandCategoryModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", msBrandCategoryModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BrandCateId", msBrandCategoryModel.BrandCateId));// Add New
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



        public async Task<int> DeleteUpdate(MsBrandCategoryModel msBrandCategoryModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_BRAND_CATEGORY SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE BRAND_CATE_ID=@BRAND_CATE_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", msBrandCategoryModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BRAND_CATE_ID", msBrandCategoryModel.BrandCateId));// Add New
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
