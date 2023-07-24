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
using MyFirstAzureWebApp.Models.profile;
using System.Data;

namespace MyFirstAzureWebApp.Business.org
{

    public class SaleOrderChangeLogImp : ISaleOrderChangeLog
    {
        private Logger log = LogManager.GetCurrentClassLogger();



        public async Task<EntitySearchResultBase<SearchSaleOrderChangeLogCustom>> searchSaleOrderChangeLog(SearchCriteriaBase<SearchSaleOrderChangeLogCriteria> searchCriteria)
        {

            SearchSaleOrderChangeLogCriteria o = searchCriteria.model;
            EntitySearchResultBase<SearchSaleOrderChangeLogCustom> searchResult = new EntitySearchResultBase<SearchSaleOrderChangeLogCustom>();
            List<SearchSaleOrderChangeLogCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select OC.CREATE_DTM CHANGE_DTM,OC.CHANGE_TAB_DESC ");
                queryBuilder.AppendFormat(" ,ISNULL(E.FIRST_NAME+' '+E.LAST_NAME,OC.CREATE_USER) CHANGE_USER ");
                queryBuilder.AppendFormat(" ,IIF(E.FIRST_NAME IS NULL,'',OC.ORDER_SALE_REP) ORDER_SALE_REP ");
                queryBuilder.AppendFormat(" from SALE_ORDER_CHANGE_LOG OC ");
                queryBuilder.AppendFormat(" left  join ADM_EMPLOYEE E on E.EMP_ID = OC.CREATE_USER ");
                queryBuilder.AppendFormat(" where OC.ORDER_ID = @ORDER_ID ");
                QueryUtils.addParam(command, "ORDER_ID", o.OrderId);// Add new

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY OC.CREATE_DTM desc  ");
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    command.CommandText = queryBuilder.ToString();
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

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();

                using (var reader = command.ExecuteReader())
                {
                    //

                    List<SearchSaleOrderChangeLogCustom> dataRecordList = new List<SearchSaleOrderChangeLogCustom>();
                    SearchSaleOrderChangeLogCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchSaleOrderChangeLogCustom();

                        dataRecord.ChangeDtm = QueryUtils.getValueAsDateTimeRequired(record, "CHANGE_DTM");
                        dataRecord.ChangeTabDesc = QueryUtils.getValueAsString(record, "CHANGE_TAB_DESC");
                        dataRecord.ChangeUser = QueryUtils.getValueAsString(record, "CHANGE_USER");
                        dataRecord.OrderSaleRep = QueryUtils.getValueAsString(record, "ORDER_SALE_REP");
                        
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






        public async Task<EntitySearchResultBase<GetNotifyTabOverviewCustom>> getNotifyTabOverview(SearchCriteriaBase<GetNotifyTabOverviewCriteria> searchCriteria)
        {

            GetNotifyTabOverviewCriteria o = searchCriteria.model;
            EntitySearchResultBase<GetNotifyTabOverviewCustom> searchResult = new EntitySearchResultBase<GetNotifyTabOverviewCustom>();
            List<GetNotifyTabOverviewCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select O.ORDER_ACTION,L.LOV_NAME_TH ORDER_ACTION_NAME,O.SOM_ORDER_NO,O.SAP_ORDER_NO,O.SAP_STATUS,O.SAP_MSG,O.CREATE_DTM,O.UPDATE_DTM ");
                queryBuilder.AppendFormat(" from SALE_ORDER_LOG O ");
                queryBuilder.AppendFormat(" inner join MS_CONFIG_LOV L on L.LOV_KEYWORD = 'ORDER_ACTION' and L.LOV_KEYVALUE = O.ORDER_ACTION ");
                queryBuilder.AppendFormat(" where O.ORDER_ID = @ORDER_ID ");
                QueryUtils.addParam(command, "ORDER_ID", o.OrderId);// Add new

                // For Paging
                queryBuilder.AppendFormat(" order by O.CREATE_DTM DESC  ");
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    command.CommandText = queryBuilder.ToString();
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

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();

                using (var reader = command.ExecuteReader())
                {
                    //

                    List<GetNotifyTabOverviewCustom> dataRecordList = new List<GetNotifyTabOverviewCustom>();
                    GetNotifyTabOverviewCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new GetNotifyTabOverviewCustom();

                        dataRecord.OrderAction = QueryUtils.getValueAsString(record, "ORDER_ACTION");
                        dataRecord.OrderActionName = QueryUtils.getValueAsString(record, "ORDER_ACTION_NAME");
                        dataRecord.SomOrderNo = QueryUtils.getValueAsString(record, "SOM_ORDER_NO");
                        dataRecord.SapOrderNo = QueryUtils.getValueAsString(record, "SAP_ORDER_NO");
                        dataRecord.SapStatus = QueryUtils.getValueAsString(record, "SAP_STATUS");
                        dataRecord.SapMsg = QueryUtils.getValueAsString(record, "SAP_MSG");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        dataRecord.CpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");

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
