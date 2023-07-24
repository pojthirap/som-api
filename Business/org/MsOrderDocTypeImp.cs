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

namespace MyFirstAzureWebApp.Business.org
{

    public class MsOrderDocTypeImp : IMsOrderDocType
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<MsOrderDocType>> searchOrderDocType(SearchCriteriaBase<SearchOrderDocTypeCriteria> searchCriteria)
        {

            EntitySearchResultBase<MsOrderDocType> searchResult = new EntitySearchResultBase<MsOrderDocType>();
            List<MsOrderDocType> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchOrderDocTypeCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from MS_ORDER_DOC_TYPE where 1=1 ");
                /*if (o != null)
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
                }*/



                // For Paging
                queryBuilder.AppendFormat(" ORDER BY DOC_TYPE_NAME_TH  ");
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

                    List<MsOrderDocType> dataRecordList = new List<MsOrderDocType>();
                    MsOrderDocType dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new MsOrderDocType();


                        dataRecord.DocTypeCode = QueryUtils.getValueAsString(record, "DOC_TYPE_CODE");
                        dataRecord.DocTypeNameTh = QueryUtils.getValueAsString(record, "DOC_TYPE_NAME_TH");
                        dataRecord.DocTypeNameEn = QueryUtils.getValueAsString(record, "DOC_TYPE_NAME_EN");
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
