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

    public class MsBankImp : IMsBank
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<MsBank>> Search(SearchCriteriaBase<MsBankCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from MS_BANK where 1=1 ");
                MsBankCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.BankId))
                    {
                        queryBuilder.AppendFormat(" and BANK_ID  = @BankId  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BankId", o.BankId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.BankCode))
                    {
                        queryBuilder.AppendFormat(" and BANK_CODE  = @BankCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BankCode", o.BankCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.BankNameTh))
                    {
                        queryBuilder.AppendFormat(" and BANK_NAME_TH like @BankNameTh  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("BankNameTh", o.BankNameTh));// Add New
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
                    queryBuilder.AppendFormat(" ORDER BY BANK_CODE  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.MsBank.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);

                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<MsBank> lst = context.MsBank.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<MsBank> searchResult = new EntitySearchResultBase<MsBank>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }

        public async Task<MsBank> Add(MsBankModel msBankdModel)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for MS_BANK_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  MS_BANK (BANK_ID, BANK_CODE, BANK_NAME_TH, BANK_NAME_EN, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, RIGHT('00000' + CAST(ISNULL((MAX(CAST(BANK_CODE AS INT)) + 1),1) AS VARCHAR), 5), @BankNameTh, @BankNameEn, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() FROM MS_BANK ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BankNameTh", msBankdModel.BankNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BankNameEn", msBankdModel.BankNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", msBankdModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        MsBank re = new MsBank();
                        re.BankId = nextVal;
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

        public async Task<int> Update(MsBankModel msBrandModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_BANK SET  BANK_NAME_TH=@BankNameTh, BANK_NAME_EN=@BankNameEn, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE BANK_ID=@BankId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("BankNameTh", msBrandModel.BankNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BankNameEn", msBrandModel.BankNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", msBrandModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", msBrandModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BankId", msBrandModel.BankId));// Add New
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


        public async Task<int> DeleteUpdate(MsBankModel msBrandModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_BANK SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE BANK_ID=@BANK_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", msBrandModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BANK_ID", msBrandModel.BankId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("numberOfRowInserted:" + numberOfRowInserted);
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
