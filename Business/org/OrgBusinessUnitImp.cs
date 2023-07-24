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
using MyFirstAzureWebApp.Models.profile;

namespace MyFirstAzureWebApp.Business.org
{

    public class OrgBusinessUnitImp : IOrgBusinessUnit

    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<OrgBusinessUnit>> Search(OrgBusinessUnitSearchCriteria searchCriteria, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add Neww
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from ORG_BUSINESS_UNIT  BU where 1=1 ");
                OrgBusinessUnitCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.buCode))
                    {
                        queryBuilder.AppendFormat(" and BU_CODE  = @buCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("buCode", o.buCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.activeFlag))
                    {
                        queryBuilder.AppendFormat(" and ACTIVE_FLAG  = @ActiveFlag  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", o.activeFlag));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.buId))
                    {
                        queryBuilder.AppendFormat(" and BU_ID  = @buId  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("buId", o.buId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.buNameTh))
                    {
                        queryBuilder.AppendFormat(" and BU_NAME_TH  like @buNameTh  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("buNameTh", o.buNameTh));// Add New
                    }

                    if (searchCriteria.searchOption == 1 && !String.IsNullOrEmpty(o.prospectId))
                    {
                        queryBuilder.AppendFormat(" and NOT EXISTS(  ");
                        queryBuilder.AppendFormat(" select 1   ");
                        queryBuilder.AppendFormat(" from PROSPECT_RECOMMEND PR   ");
                        queryBuilder.AppendFormat(" where PR.BU_ID = BU.BU_ID   ");
                        queryBuilder.AppendFormat(" and PR.PROSPECT_ID = @prospectId ");
                        queryBuilder.AppendFormat(" ) ");
                        queryBuilder.AppendFormat(" and BU.BU_ID != @buId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("prospectId", o.prospectId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("buId", userProfile.getBuId()));// Add New
                    }
                }
                // For Paging
                if (searchCriteria.searchOrder == 1)
                {
                    queryBuilder.AppendFormat(" ORDER BY UPDATE_DTM DESC  ");
                }
                else if (searchCriteria.searchOrder == 2)
                {
                    queryBuilder.AppendFormat(" ORDER BY BU_NAME_TH  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY BU_NAME_TH  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.OrgBusinessUnit.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<OrgBusinessUnit> lst = context.OrgBusinessUnit.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<OrgBusinessUnit> searchResult = new EntitySearchResultBase<OrgBusinessUnit>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }

        public async Task<OrgBusinessUnit> Add(BusinessUnitModel model)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for ORG_BUSINESS_UNIT_SEQ ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  ORG_BUSINESS_UNIT (BU_ID, BU_CODE, BU_NAME_TH, BU_NAME_EN, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, RIGHT('00000' + CAST(ISNULL((MAX(CAST(bu_code AS INT)) + 1),1) AS VARCHAR), 5), @BuNameTh, @BuNameEn, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from ORG_BUSINESS_UNIT ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BuNameTh", model.BuNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BuNameEn", model.BuNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", model.getUserName()));// Add New
                        string queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        OrgBusinessUnit re = new OrgBusinessUnit();
                        re.BuId = nextVal;
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

        public async Task<int> Update(BusinessUnitModel model)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ORG_BUSINESS_UNIT SET BU_NAME_TH=@BuNameTh, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE BU_ID=@BuId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("BuNameTh", model.BuNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", model.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", model.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BuId", model.BuId));// Add New
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


        public async Task<int> DeleteUpdate(BusinessUnitModel model)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ORG_BUSINESS_UNIT SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE BU_ID=@BU_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", model.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BU_ID", model.BuId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM ADM_GROUP_USER WHERE BU_ID=@BU_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("BU_ID", model.BuId));// Add New
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
