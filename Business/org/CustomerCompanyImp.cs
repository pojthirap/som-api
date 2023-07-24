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
using MyFirstAzureWebApp.Entity.custom;

namespace MyFirstAzureWebApp.Business.org
{

    public class CustomerCompanyImp : ICustomerCompany
    {
        private Logger log = LogManager.GetCurrentClassLogger();



        public async Task<EntitySearchResultBase<SearCompanyCustom>> searCompany(SearchCriteriaBase<SearCompanyCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearCompanyCustom> searchResult = new EntitySearchResultBase<SearCompanyCustom>();
            List<SearCompanyCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearCompanyCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select OC.COMPANY_CODE,OC.COMPANY_NAME_TH ");
                queryBuilder.AppendFormat(" from CUSTOMER_COMPANY CC ");
                queryBuilder.AppendFormat(" inner join ORG_COMPANY OC on OC.COMPANY_CODE = CC.COMPANY_CODE ");
                queryBuilder.AppendFormat(" where 1= 1 ");
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.CustCode))
                    {
                        queryBuilder.AppendFormat(" and CC.CUST_CODE  = @CustCode  ");
                        QueryUtils.addParam(command, "CustCode", o.CustCode);// Add new
                    }
                }

                

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY COMPANY_NAME_TH  ");
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

                    List<SearCompanyCustom> dataRecordList = new List<SearCompanyCustom>();
                    SearCompanyCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearCompanyCustom();


                        dataRecord.CompanyCode = QueryUtils.getValueAsString(record, "COMPANY_CODE");
                        dataRecord.CompanyNameTh = QueryUtils.getValueAsString(record, "COMPANY_NAME_TH");

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
