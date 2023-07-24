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

    public class TemplateCategoryImp : ITemplateCategory
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<TemplateCategory>> Search(SearchCriteriaBase<TemplateCategoryCriteria> searchCriteria)
        {
            var sqlParameters = new List<SqlParameter>();// Add New

            using (var context = new MyAppContext())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from TEMPLATE_CATEGORY where 1=1 ");
                TemplateCategoryCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.TpCateId))
                    {
                        queryBuilder.AppendFormat(" and TP_CATE_ID  = @TpCateId  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpCateId", o.TpCateId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.TpCateCode))
                    {
                        queryBuilder.AppendFormat(" and TP_CATE_CODE  = @TpCateCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpCateCode", o.TpCateCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.TpCateNameTh))
                    {
                        queryBuilder.AppendFormat(" and TP_CATE_NAME_TH like @TpCateNameTh  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("TpCateNameTh", o.TpCateNameTh));// Add New
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
                    queryBuilder.AppendFormat(" ORDER BY TP_CATE_CODE  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.TemplateCategory.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<TemplateCategory> lst = context.TemplateCategory.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<TemplateCategory> searchResult = new EntitySearchResultBase<TemplateCategory>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }

        public async Task<TemplateCategory> Add(TemplateCategoryModel templateCategoryModel)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for TEMPLATE_CATEGORY_SEQ", p);
                        var nextVal = (int)p.Value;
                        //string code_ = QueryUtils.padLeft(nextVal.ToString(), '0', 4);

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  TEMPLATE_CATEGORY (TP_CATE_ID, TP_CATE_CODE, TP_CATE_NAME_TH, TP_CATE_NAME_EN, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, RIGHT('00000' + CAST(ISNULL((MAX(CAST(TP_CATE_CODE AS INT)) + 1),1) AS VARCHAR), 5), @TpCateNameTh, @TpCateNameEn, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from TEMPLATE_CATEGORY ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpCateNameTh", templateCategoryModel.TpCateNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpCateNameEn", templateCategoryModel.TpCateNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", templateCategoryModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        TemplateCategory re = new TemplateCategory();
                        re.TpCateId = nextVal;
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

        public async Task<int> Update(TemplateCategoryModel templateCategoryModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_CATEGORY SET TP_CATE_NAME_TH=@TpCateNameTh, TP_CATE_NAME_EN=@TpCateNameEn, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TP_CATE_ID=@TpCateId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpCateNameTh", templateCategoryModel.TpCateNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpCateNameEn", templateCategoryModel.TpCateNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", templateCategoryModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", templateCategoryModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpCateId", templateCategoryModel.TpCateId));// Add New
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


        public async Task<int> DeleteUpdate(TemplateCategoryModel templateCategoryModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_CATEGORY SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TP_CATE_ID=@TP_CATE_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", templateCategoryModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_CATE_ID", templateCategoryModel.TpCateId));// Add New
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
