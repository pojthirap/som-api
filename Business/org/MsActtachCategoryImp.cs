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

    public class MsActtachCategoryImp : IMsActtachCategory
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<MsAttachCategory>> Search(SearchCriteriaBase<MsActtachCategoryCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from MS_ATTACH_CATEGORY where 1=1 ");
                MsActtachCategoryCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.AttachCateId))
                    {
                        queryBuilder.AppendFormat(" and ATTACH_CATE_ID  = @AttachCateId  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AttachCateId", o.AttachCateId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.AttachCateCode))
                    {
                        queryBuilder.AppendFormat(" and ATTACH_CATE_CODE  = @AttachCateCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AttachCateCode", o.AttachCateCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.AttachCateNameTh))
                    {
                        queryBuilder.AppendFormat(" and ATTACH_CATE_NAME_TH like @AttachCateNameTh  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("AttachCateNameTh", o.AttachCateNameTh));// Add New
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
                } else if (2 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY ATTACH_CATE_NAME_TH  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY ATTACH_CATE_CODE  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.MsAttachCategory.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<MsAttachCategory> lst = context.MsAttachCategory.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<MsAttachCategory> searchResult = new EntitySearchResultBase<MsAttachCategory>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }

        public async Task<MsAttachCategory> Add(MsActtachCategoryModel msActtachCategoryModel)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for MS_ATTACH_CATEGORY_SEQ", p);
                        var nextVal = (int)p.Value;
                        //string code_ = QueryUtils.padLeft(nextVal.ToString(), '0', 4);

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  MS_ATTACH_CATEGORY (ATTACH_CATE_ID, ATTACH_CATE_CODE, ATTACH_CATE_NAME_TH, ATTACH_CATE_NAME_EN, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, RIGHT('00000' + CAST(ISNULL((MAX(CAST(ATTACH_CATE_CODE AS INT)) + 1),1) AS VARCHAR), 5), @AttachCateNameTh, @AttachCateNameEn, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from MS_ATTACH_CATEGORY ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AttachCateNameTh", msActtachCategoryModel.AttachCateNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AttachCateNameEn", msActtachCategoryModel.AttachCateNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", msActtachCategoryModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        MsAttachCategory re = new MsAttachCategory();
                        re.AttachCateId = nextVal;
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

        public async Task<int> Update(MsActtachCategoryModel msActtachCategoryModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_ATTACH_CATEGORY SET  ATTACH_CATE_NAME_TH=@AttachCateNameTh, ATTACH_CATE_NAME_EN=@AttachCateNameEn, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE ATTACH_CATE_ID=@AttachCateId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("AttachCateNameTh", msActtachCategoryModel.AttachCateNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AttachCateNameEn", msActtachCategoryModel.AttachCateNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", msActtachCategoryModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", msActtachCategoryModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AttachCateId", msActtachCategoryModel.AttachCateId));// Add New
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


        public async Task<int> DeleteUpdate(MsActtachCategoryModel msActtachCategoryModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_ATTACH_CATEGORY SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE ATTACH_CATE_ID=@ATTACH_CATE_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", msActtachCategoryModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ATTACH_CATE_ID", msActtachCategoryModel.AttachCateId));// Add New
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
