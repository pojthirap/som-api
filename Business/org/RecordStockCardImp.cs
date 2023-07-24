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
using static MyFirstAzureWebApp.SearchCriteria.SearchStockCountTabCustom;
using System.Data;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.record;

namespace MyFirstAzureWebApp.Business.org
{

    public class RecordStockCardImp : IRecordStockCard
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<ResponseData>> searchStockCountTab(SearchCriteriaBase<SearchStockCountTabCriteria> searchCriteria)
        {

            EntitySearchResultBase<ResponseData> searchResult = new EntitySearchResultBase<ResponseData>();
            List<ResponseData> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchStockCountTabCriteria o = searchCriteria.model;

                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat("  select SC.*,PC.*,P.* ");
                queryBuilder.AppendFormat("  from RECORD_STOCK_CARD SC ");
                queryBuilder.AppendFormat("  inner join MS_PRODUCT_CONVERSION PC on PC.PROD_CONV_ID = SC.PROD_CONV_ID ");
                queryBuilder.AppendFormat("  inner join MS_PRODUCT P on P.PROD_CODE = PC.PROD_CODE ");
                queryBuilder.AppendFormat(" where SC.PLAN_TRIP_TASK_ID = (");
                queryBuilder.AppendFormat(" select top 1 TT.PLAN_TRIP_TASK_ID ");
                queryBuilder.AppendFormat(" from PLAN_TRIP_PROSPECT TP ");
                queryBuilder.AppendFormat(" inner join PLAN_TRIP_TASK TT on TT.TASK_TYPE = 'S' and TT.PLAN_TRIP_PROSP_ID = TP.PLAN_TRIP_PROSP_ID ");
                queryBuilder.AppendFormat(" and TP.VISIT_CHECKOUT_DTM is not null ");
                queryBuilder.AppendFormat(" and TP.PROSP_ID = @PROSP_ID  ");
                queryBuilder.AppendFormat(" ORDER BY TP.VISIT_CHECKOUT_DTM DESC ");
                queryBuilder.AppendFormat(" ) ");
                QueryUtils.addParam(command, "PROSP_ID", o.ProspId);// Add new




                // For Paging
                queryBuilder.AppendFormat(" ORDER BY SC.REC_STOCK_ID  ");
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

                    List<objectResult> dataRecordList = new List<objectResult>();
                    objectResult dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new objectResult();

                        dataRecord.productCode = QueryUtils.getValueAsString(record, "PROD_CODE");
                        dataRecord.productName = QueryUtils.getValueAsString(record, "PROD_NAME_EN");
                        dataRecord.baseUnit = QueryUtils.getValueAsString(record, "BASE_UNIT");
                        dataRecord.qry = QueryUtils.getValueAsString(record, "REC_QTY");
                        dataRecord.altUnit = QueryUtils.getValueAsString(record, "ALT_UNIT");

                        dataRecordList.Add(dataRecord);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //

                    List<ResponseData> lstResponse = new List<ResponseData>();
                    ResponseData resData = null;
                    if (dataRecordList!=null && dataRecordList.Count != 0)
                    {
                        var groupedByproductCode = dataRecordList.GroupBy(z => z.productCode);
                        foreach (var group in groupedByproductCode)
                        {
                            Console.WriteLine("Users starting with " + group.Key + ":");
                            int i = 0;
                            List<RecQty> listRecQty = new List<RecQty>();
                            foreach (var obj in group)
                            {
                                
                                if (i == 0) {
                                    resData = new ResponseData();
                                    resData.productCode = obj.productCode;
                                    resData.productName = obj.productName;
                                    resData.baseUnit = obj.baseUnit;
                                    resData.listRecQty = listRecQty;
                                    lstResponse.Add(resData);
                                    i++;
                                }
                                RecQty rq = new RecQty();
                                rq.altUnit = obj.altUnit;
                                rq.qry = obj.qry;
                                listRecQty.Add(rq);
                                
                               

                                Console.WriteLine("* " + obj.productCode);
                            }
                        }
                    }

                    lst = lstResponse;
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }






        public async Task<List<RecordStockCard>> addRecordStockCard(List<RecordStockCardModel> recordStockCardList, UserProfileForBack userProfileForBack)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        List<RecordStockCard> lst = new List<RecordStockCard>();
                        // Delete
                        string DelPlanTripTaskId = recordStockCardList.ElementAt(0).PlanTripTaskId.ToString();
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM RECORD_STOCK_CARD  WHERE PLAN_TRIP_TASK_ID=@PlanTripTaskId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PlanTripTaskId", DelPlanTripTaskId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        //int recordUpdate = await DeleteRecordStockCardForAdd(lst.ElementAt(0).PlanTripTaskId.ToString());
                        // Add 
                        for (int i = 0; i < recordStockCardList.Count; i++)
                        {
                            RecordStockCardModel recordMeterModel = recordStockCardList.ElementAt(i);
                            sqlParameters = new List<SqlParameter>();// Add New
                            var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                            p.Direction = System.Data.ParameterDirection.Output;
                            context.Database.ExecuteSqlRaw("set @result = next value for RECORD_STOCK_CARD_SEQ", p);
                            var nextVal = (int)p.Value;

                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat("INSERT INTO  RECORD_STOCK_CARD (REC_STOCK_ID, PLAN_TRIP_TASK_ID, PROD_CONV_ID, REC_QTY, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                            queryBuilder.AppendFormat("VALUES(@REC_STOCK_ID ,@PLAN_TRIP_TASK_ID, @PROD_CONV_ID, @REC_QTY, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                            sqlParameters.Add(QueryUtils.addSqlParameter("REC_STOCK_ID", nextVal));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_TASK_ID", recordMeterModel.PlanTripTaskId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PROD_CONV_ID", recordMeterModel.ProdConvId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("REC_QTY", recordMeterModel.RecQty));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfileForBack.getUserName()));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                            RecordStockCard re = new RecordStockCard();
                            re.RecStockId = nextVal;
                            lst.Add(re);
                        }
                        transaction.Commit();
                        return lst;



                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }



        public async Task<int> DeleteRecordStockCardForAdd(String PlanTripTaskId)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM RECORD_STOCK_CARD  WHERE PLAN_TRIP_TASK_ID=@PlanTripTaskId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PlanTripTaskId", PlanTripTaskId));// Add New
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









    }
}
