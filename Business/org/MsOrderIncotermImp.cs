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

    public class MsOrderIncotermImp : IMsOrderIncoterm
    {
        private Logger log = LogManager.GetCurrentClassLogger();



        public async Task<EntitySearchResultBase<MsOrderIncoterm>> searchOrderIncoterm(SearchCriteriaBase<SearchOrderIncotermCriteria> searchCriteria)
        {

            EntitySearchResultBase<MsOrderIncoterm> searchResult = new EntitySearchResultBase<MsOrderIncoterm>();
            List<MsOrderIncoterm> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchOrderIncotermCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select * ");
                queryBuilder.AppendFormat(" from MS_ORDER_INCOTERM ");
                queryBuilder.AppendFormat(" where ACTIVE_FLAG = 'Y' ");

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY DESCRIPTION  ");
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

                    List<MsOrderIncoterm> dataRecordList = new List<MsOrderIncoterm>();
                    MsOrderIncoterm dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new MsOrderIncoterm();


                        dataRecord.IncotermCode = QueryUtils.getValueAsString(record, "INCOTERM_CODE");
                        dataRecord.Description = QueryUtils.getValueAsString(record, "DESCRIPTION");
                        dataRecord.SyncId = QueryUtils.getValueAsDecimal(record, "SYNC_ID");

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
