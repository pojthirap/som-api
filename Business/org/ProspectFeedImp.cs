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

    public class ProspectFeedImp : IProspectFeed
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<SearchFeedTabCustom>> searchFeedTab(SearchCriteriaBase<SearchFeedTabCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchFeedTabCustom> searchResult = new EntitySearchResultBase<SearchFeedTabCustom>();
            List<SearchFeedTabCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchFeedTabCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select F.*,CF.LOV_NAME_TH,E.TITLE_NAME+E.FIRST_NAME+' '+E.LAST_NAME CREATE_FULL_NAME ");
                queryBuilder.AppendFormat(" from PROSPECT_FEED F ");
                queryBuilder.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = F.CREATE_USER ");
                queryBuilder.AppendFormat(" inner join ms_config_lov CF on CF.LOV_KEYVALUE = F.FUNCTION_TAB and CF.LOV_KEYWORD = 'FUNCTION_TAB'  ");
                queryBuilder.AppendFormat(" where F.PROSPECT_ID = @PROSPECT_ID ");
                QueryUtils.addParam(command, "PROSPECT_ID", o.ProspectId);


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY F.FEED_ID desc  ");
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

                    List<SearchFeedTabCustom> dataRecordList = new List<SearchFeedTabCustom>();
                    SearchFeedTabCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchFeedTabCustom();


                        dataRecord.FeedId = QueryUtils.getValueAsDecimalRequired(record, "FEED_ID");
                        dataRecord.ProspectId = QueryUtils.getValueAsDecimalRequired(record, "PROSPECT_ID");
                        dataRecord.FunctionTab = QueryUtils.getValueAsDecimalRequired(record, "FUNCTION_TAB");
                        dataRecord.Description = QueryUtils.getValueAsString(record, "DESCRIPTION");
                        dataRecord.LovNameTh = QueryUtils.getValueAsString(record, "LOV_NAME_TH");
                        dataRecord.CreateFullName = QueryUtils.getValueAsString(record, "CREATE_FULL_NAME");
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
