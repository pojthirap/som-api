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
using System.Data;
using MyFirstAzureWebApp.Entity.custom;

namespace MyFirstAzureWebApp.Business.org
{

    public class CustomerImp : ICustomer
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<SearchCustomerCustom>> searchCustomer(SearchCriteriaBase<CustomerCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchCustomerCustom> searchResult = new EntitySearchResultBase<SearchCustomerCustom>();
            List<SearchCustomerCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                CustomerCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select C.CUST_CODE,C.CUST_NAME_TH,C.CUST_NAME_EN ");
                queryBuilder.AppendFormat(" ,TRIM(IIF(C.STREET is null,'',C.STREET+' ') + ");
                queryBuilder.AppendFormat(" IIF(C.SUBDISTRICT_NAME is null,'',C.SUBDISTRICT_NAME+' ') + ");
                queryBuilder.AppendFormat(" IIF(C.DISTRICT_NAME is null,'',C.DISTRICT_NAME+' ') + ");
                queryBuilder.AppendFormat(" IIF(C.PROVINCE_CODE is null,'',P.PROVINCE_NAME_TH)) ADDRESS_FULLNM ");
                queryBuilder.AppendFormat(" from CUSTOMER C ");
                queryBuilder.AppendFormat(" left join MS_PROVINCE P on P.PROVINCE_CODE = C.PROVINCE_CODE ");
                queryBuilder.AppendFormat(" where substring(C.CUST_CODE,1,1)!= '9' ");


                // For Paging
                if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY CUST_NAME_TH  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY CUST_CODE  ");
                }
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

                    List<SearchCustomerCustom> dataRecordList = new List<SearchCustomerCustom>();
                    SearchCustomerCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchCustomerCustom();


                        dataRecord.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        dataRecord.CustNameTh = QueryUtils.getValueAsString(record, "CUST_NAME_TH");
                        dataRecord.CustNameEn = QueryUtils.getValueAsString(record, "CUST_NAME_EN");
                        dataRecord.AddressFullnm = QueryUtils.getValueAsString(record, "ADDRESS_FULLNM");

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


        /*
        public async Task<EntitySearchResultBase<Customer>> searchCustomer(SearchCriteriaBase<CustomerCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from CUSTOMER where 1=1 ");
                CustomerCriteria o = searchCriteria.model;

                // For Paging
                if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY CUST_NAME_TH  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY CUST_CODE  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.Customer.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<Customer> lst = context.Customer.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<Customer> searchResult = new EntitySearchResultBase<Customer>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }
        */








    }
}
