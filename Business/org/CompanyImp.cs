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
using MyFirstAzureWebApp.Utils;
using System.Data;

namespace MyFirstAzureWebApp.Business.org
{

    public class CompanyImp : ICompany
    {
        private Logger log = LogManager.GetCurrentClassLogger();



        public async Task<EntitySearchResultBase<OrgCompany>> Search(SearchCriteriaBase<OrgCompanyCriteria> searchCriteria)
        {

            EntitySearchResultBase<OrgCompany> searchResult = new EntitySearchResultBase<OrgCompany>();
            List<OrgCompany> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                OrgCompanyCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from ORG_COMPANY where 1=1 ");
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.companyName))
                    {
                        queryBuilder.AppendFormat(" and COMPANY_NAME_TH like @CompanyName  ");
                        QueryUtils.addParamLike(command, "CompanyName", o.companyName);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.activeFlag))
                    {
                        queryBuilder.AppendFormat(" and ACTIVE_FLAG  = @ActiveFlag  ");
                        QueryUtils.addParam(command, "ActiveFlag", o.activeFlag);// Add new
                    }
                }

                

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY COMPANY_CODE  ");
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = queryBuilder.ToString();// Add New
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(queryBuilder, command, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging
                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    //

                    List<OrgCompany> dataRecordList = new List<OrgCompany>();
                    OrgCompany dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new OrgCompany();


                        dataRecord.CompanyCode = QueryUtils.getValueAsString(record, "COMPANY_CODE");
                        dataRecord.CompanyNameTh = QueryUtils.getValueAsString(record, "COMPANY_NAME_TH");
                        dataRecord.CompanyNameEn = QueryUtils.getValueAsString(record, "COMPANY_NAME_EN");
                        dataRecord.VatRegistNo = QueryUtils.getValueAsString(record, "VAT_REGIST_NO");
                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");

                        dataRecordList.Add(dataRecord);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //
                    lst = dataRecordList;
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }






    }
}
