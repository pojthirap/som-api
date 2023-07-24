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
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.adm;
using MyFirstAzureWebApp.Utils;

namespace MyFirstAzureWebApp.Business.org
{

    public class AdmGroupImp : IAdmGroup
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<AdmGroup>> Search(SearchCriteriaBase<AdmGroupCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New


                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from ADM_GROUP where 1=1 ");
                AdmGroupCriteria o = searchCriteria.model;
                if (o != null)
                {

                    if (!String.IsNullOrEmpty(o.name))
                    {
                        queryBuilder.AppendFormat(" and GROUP_NAME_TH  like @GroupName  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("GroupName", o.name));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.activeFlag))
                    {
                        queryBuilder.AppendFormat(" and ACTIVE_FLAG  = @ActiveFlag  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", o.activeFlag));// Add New
                    }
                }
                // For Paging
                if (searchCriteria.searchOrder == 1)
                {
                    queryBuilder.AppendFormat(" ORDER BY UPDATE_DTM DESC  ");
                }
                else if (searchCriteria.searchOrder == 2)
                {
                    queryBuilder.AppendFormat(" ORDER BY GROUP_NAME_TH  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY UPDATE_DTM DESC  ");
                }
                
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.AdmGroup.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();// Add New
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                // Edit New
                List<AdmGroup> lst = context.AdmGroup.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList(); // Add New
                EntitySearchResultBase<AdmGroup> searchResult = new EntitySearchResultBase<AdmGroup>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }
        }
        /*public async Task<EntitySearchResultBase<AdmGroup>> Search(AdmGroupSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                AdmGroupCriteria criteria = searchCriteria.model;
                var queryCommane = (from g in context.AdmGroup
                                    where ((criteria.activeFlag == null ? 1 == 1 : g.ActiveFlag == criteria.activeFlag))
                                    where ((criteria.name == null ? 1 == 1 : g.GroupNameTh.Contains(criteria.name)))
                                    //orderby (searchCriteria.searchOrder==1 ? g.UpdateDtm : g.UpdateDtm) descending
                                    select g
                                    );
                if (searchCriteria.searchOrder == 1)
                {
                    queryCommane = queryCommane.OrderByDescending(o => o.UpdateDtm);
                }else if (searchCriteria.searchOrder == 2)
                {
                    queryCommane = queryCommane.OrderBy(o => o.GroupNameTh);
                }
                else
                {
                    queryCommane = queryCommane.OrderByDescending(o => o.UpdateDtm);
                }
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<AdmGroup> searchResult = new EntitySearchResultBase<AdmGroup>();
                searchResult.totalRecords = query.Count();

                List<AdmGroup> saleLst = new List<AdmGroup>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    AdmGroup s = new AdmGroup();
                    s.GroupId = item.GroupId;
                    s.GroupCode = item.GroupCode;
                    s.GroupNameTh = item.GroupNameTh;
                    s.GroupNameEn = item.GroupNameEn;
                    s.ActiveFlag = item.ActiveFlag;
                    s.EffectiveDate = item.EffectiveDate;
                    s.ExpiryDate = item.ExpiryDate;
                    s.CreateUser = item.CreateUser;
                    s.CreateDtm = item.CreateDtm;
                    s.UpdateUser = item.UpdateUser;
                    s.UpdateDtm = item.UpdateDtm;
                    saleLst.Add(s);

                }
                searchResult.data = saleLst;
                return searchResult;
            }

        }*/


        public async Task<AdmGroup> Add(AdmGroupModel model)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for ADM_GROUP_SEQ ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  ADM_GROUP (GROUP_ID, GROUP_CODE, GROUP_NAME_TH, GROUP_NAME_EN, ACTIVE_FLAG, EFFECTIVE_DATE, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, RIGHT('00000' + CAST(ISNULL((MAX(CAST(group_code AS INT)) + 1),1) AS VARCHAR), 5), @GroupNameTh, @GroupNameEn, 'Y', @EffectiveDate, @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from ADM_GROUP where system_flag is null ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GroupNameTh", model.GroupNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GroupNameEn", model.GroupNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("EffectiveDate", model.EffectiveDate));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", model.getUserName()));// Add New
                        string queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        AdmGroup re = new AdmGroup();
                        re.GroupId = nextVal;
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


        public async Task<int> Update(AdmGroupModel model)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ADM_GROUP SET GROUP_NAME_TH = @GroupNameTh, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE GROUP_ID=@GroupId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("GroupNameTh", model.GroupNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", model.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", model.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GroupId", model.GroupId));// Add New
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

        public async Task<int> DeleteUate(AdmGroupModel model)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ADM_GROUP SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE GROUP_ID=@GROUP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", model.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_ID", model.GroupId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM ADM_GROUP_USER WHERE GROUP_ID=@GROUP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_ID", model.GroupId));// Add New
                        queryStr = queryBuilder.ToString();
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
