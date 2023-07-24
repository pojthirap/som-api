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
using MyFirstAzureWebApp.Entity.custom;
using System.Data;

namespace MyFirstAzureWebApp.Business.org
{

    public class MsProductConversionImp : IMsProductConversion
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<MsProductConversion>> searchProductConversion(SearchCriteriaBase<SearchProductConversionCriteria> searchCriteria)
        {

            EntitySearchResultBase<MsProductConversion> searchResult = new EntitySearchResultBase<MsProductConversion>();
            List<MsProductConversion> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchProductConversionCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select * ");
                queryBuilder.AppendFormat(" from MS_PRODUCT_CONVERSION where 1=1 ");

                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.ProdCode))
                    {
                        queryBuilder.AppendFormat(" and PROD_CODE = @PROD_CODE ", o.ProdCode);
                        QueryUtils.addParam(command, "PROD_CODE", o.ProdCode);// Add New
                    }

                }

                    // For Paging
                    queryBuilder.AppendFormat(" ORDER BY CREATE_DTM desc  ");
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

                    List<MsProductConversion> dataRecordList = new List<MsProductConversion>();
                    MsProductConversion dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new MsProductConversion();


                        dataRecord.ProdConvId = QueryUtils.getValueAsDecimalRequired(record, "PROD_CONV_ID");
                        dataRecord.ProdCode = QueryUtils.getValueAsString(record, "PROD_CODE");
                        dataRecord.AltUnit = QueryUtils.getValueAsString(record, "ALT_UNIT");
                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        dataRecord.Denominator = QueryUtils.getValueAsDecimal(record, "DENOMINATOR");
                        dataRecord.Counter = QueryUtils.getValueAsDecimal(record, "COUNTER");
                        dataRecord.GrossWeight = QueryUtils.getValueAsDecimal(record, "GROSS_WEIGHT");
                        dataRecord.WeightUnit = QueryUtils.getValueAsString(record, "WEIGHT_UNIT");

                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");

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
