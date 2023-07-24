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
using static MyFirstAzureWebApp.SearchCriteria.GetTaskStockCardForRecordCustom;
using MsProductConversion = MyFirstAzureWebApp.SearchCriteria.GetTaskStockCardForRecordCustom.MsProductConversion;
using RecordStockCard = MyFirstAzureWebApp.SearchCriteria.GetTaskStockCardForRecordCustom.RecordStockCard;

namespace MyFirstAzureWebApp.Business.org
{

    public class TemplateStockCardImp : ITemplateStockCard
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<TemplateStockCard>> Search(SearchCriteriaBase<TemplateStockCardCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from TEMPLATE_STOCK_CARD where 1=1 ");
                TemplateStockCardCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.TpStockCardId))
                    {
                        queryBuilder.AppendFormat(" and TP_STOCK_CARD_ID  = @TpStockCardId  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpStockCardId", o.TpStockCardId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.TpCode))
                    {
                        queryBuilder.AppendFormat(" and TP_CODE  = @TpCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpCode", o.TpCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.TpNameTh))
                    {
                        queryBuilder.AppendFormat(" and TP_NAME_TH like @TpNameTh  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("TpNameTh", o.TpNameTh));// Add New
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
                    queryBuilder.AppendFormat(" ORDER BY UPDATE_DTM DESC  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.TemplateStockCard.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<TemplateStockCard> lst = context.TemplateStockCard.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<TemplateStockCard> searchResult = new EntitySearchResultBase<TemplateStockCard>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }

        public async Task<TemplateStockCard> Add(TemplateStockCardModel templateStockCardModel)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for TEMPLATE_STOCK_CARD_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  TEMPLATE_STOCK_CARD (TP_STOCK_CARD_ID, TP_CODE, TP_NAME_TH, TP_NAME_EN, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, RIGHT('00000' + CAST(ISNULL((MAX(CAST(TP_CODE AS INT)) + 1),1) AS VARCHAR), 5), @TpNameTh, @TpNameEn, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from TEMPLATE_STOCK_CARD ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpNameTh", templateStockCardModel.TpNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpNameEn", templateStockCardModel.TpNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", templateStockCardModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        TemplateStockCard re = new TemplateStockCard();
                        re.TpStockCardId = nextVal;
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

        public async Task<int> Update(TemplateStockCardModel templateStockCardModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_STOCK_CARD SET TP_NAME_TH=@TpNameTh, TP_NAME_EN=@TpNameEn, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TP_STOCK_CARD_ID=@TpStockCardId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpNameTh", templateStockCardModel.TpNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpNameEn", templateStockCardModel.TpNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", templateStockCardModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", templateStockCardModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpStockCardId", templateStockCardModel.TpStockCardId));// Add New
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


        public async Task<int> DeleteUpdate(TemplateStockCardModel templateStockCardModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_STOCK_CARD SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TP_STOCK_CARD_ID=@TP_STOCK_CARD_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", templateStockCardModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_STOCK_CARD_ID", templateStockCardModel.TpStockCardId));// Add New
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




        
        public async Task<EntitySearchResultBase<GetTaskStockCardForRecordCustom.Product>> getTaskStockCardForRecord(SearchCriteriaBase<GetTaskStockCardForRecordCriteria> searchCriteria)
        {

            EntitySearchResultBase<GetTaskStockCardForRecordCustom.Product> searchResult = new EntitySearchResultBase<GetTaskStockCardForRecordCustom.Product>();
            List<GetTaskStockCardForRecordCustom.Product> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                GetTaskStockCardForRecordCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select P.ACTIVE_FLAG AS P_ACTIVE_FLAG, P.CREATE_USER AS P_CREATE_USER, P.CREATE_DTM AS P_CREATE_DTM, P.UPDATE_USER AS P_UPDATE_USER, P.UPDATE_DTM AS P_UPDATE_DTM,P.*,PC.PROD_CONV_ID AS PC_PROD_CONV_ID, PC.*, RS.PROD_CONV_ID AS RS_PROD_CONV_ID, RS.* ");
                queryBuilder.AppendFormat(" from TEMPLATE_STOCK_CARD SC ");
                queryBuilder.AppendFormat(" inner join TEMPLATE_STOCK_PRODUCT SP on SP.TP_STOCK_CARD_ID = SC.TP_STOCK_CARD_ID ");
                queryBuilder.AppendFormat(" inner join MS_PRODUCT P on P.PROD_CODE = SP.PROD_CODE ");
                queryBuilder.AppendFormat(" inner join MS_PRODUCT_CONVERSION PC on PC.PROD_CODE = P.PROD_CODE ");
                queryBuilder.AppendFormat(" left join RECORD_STOCK_CARD RS on RS.PROD_CONV_ID = PC.PROD_CONV_ID and RS.PLAN_TRIP_TASK_ID = @PlanTripTaskId ");
                queryBuilder.AppendFormat(" where SP.ACTIVE_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" and SC.TP_STOCK_CARD_ID = @TpStockCardId ");
                QueryUtils.addParam(command, "TpStockCardId", o.TpStockCardId);
                QueryUtils.addParam(command, "PlanTripTaskId", o.PlanTripTaskId);


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY P.PROD_CODE  ");
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
                    MsProductConversion msProductConversion = null;
                    RecordStockCard recordStockCard = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new objectResult();
                        msProductConversion = new MsProductConversion();
                        recordStockCard = new RecordStockCard();


                        dataRecord.ProdCode = QueryUtils.getValueAsString(record, "PROD_CODE");
                        dataRecord.DivisionCode = QueryUtils.getValueAsString(record, "DIVISION_CODE");
                        dataRecord.ProdNameTh = QueryUtils.getValueAsString(record, "PROD_NAME_TH");
                        dataRecord.ProdNameEn = QueryUtils.getValueAsString(record, "PROD_NAME_EN");
                        dataRecord.ProdType = QueryUtils.getValueAsString(record, "PROD_TYPE");
                        dataRecord.ProdGroup = QueryUtils.getValueAsString(record, "PROD_GROUP");
                        dataRecord.IndustrySector = QueryUtils.getValueAsString(record, "INDUSTRY_SECTOR");
                        dataRecord.OldProdNo = QueryUtils.getValueAsString(record, "OLD_PROD_NO");
                        dataRecord.BaseUnit = QueryUtils.getValueAsString(record, "BASE_UNIT");
                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "P_ACTIVE_FLAG");
                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "P_CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTime(record, "P_CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "P_UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTime(record, "P_UPDATE_DTM");
                        dataRecord.ReportProdConvId = QueryUtils.getValueAsDecimal(record, "REPORT_PROD_CONV_ID");

                        msProductConversion.ProdConvId = QueryUtils.getValueAsString(record, "PC_PROD_CONV_ID");
                        msProductConversion.ProdCode = QueryUtils.getValueAsString(record, "PROD_CODE");
                        msProductConversion.AltUnit = QueryUtils.getValueAsString(record, "ALT_UNIT");
                        msProductConversion.Denominator = QueryUtils.getValueAsString(record, "DENOMINATOR");
                        msProductConversion.Counter = QueryUtils.getValueAsString(record, "COUNTER");
                        msProductConversion.GrossWeight = QueryUtils.getValueAsString(record, "GROSS_WEIGHT");
                        msProductConversion.WeightUnit = QueryUtils.getValueAsString(record, "WEIGHT_UNIT");
                        
                        recordStockCard.ProdConvId = QueryUtils.getValueAsString(record, "RS_PROD_CONV_ID");
                        recordStockCard.RecStockId = QueryUtils.getValueAsString(record, "REC_STOCK_ID");
                        recordStockCard.PlanTripTaskId = QueryUtils.getValueAsString(record, "PLAN_TRIP_TASK_ID");
                        recordStockCard.RecQty = QueryUtils.getValueAsString(record, "REC_QTY");

                        dataRecord.MsProductConversion = msProductConversion;
                        dataRecord.RecordStockCard = recordStockCard;

                        dataRecordList.Add(dataRecord);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //
                    //

                    List<GetTaskStockCardForRecordCustom.Product> lstResponse = new List<GetTaskStockCardForRecordCustom.Product>();
                    GetTaskStockCardForRecordCustom.Product resData = null;
                    if (dataRecordList != null && dataRecordList.Count != 0)
                    {
                        var groupedByproductCode = dataRecordList.GroupBy(z => z.ProdCode);
                        foreach (var group in groupedByproductCode)
                        {
                            Console.WriteLine("Users starting with " + group.Key + ":");
                            int i = 0;
                            List<ProductConversion> listProductConversion = new List<ProductConversion>();
                            foreach (var obj in group)
                            {
                                
                                if (i == 0)
                                {
                                    resData = new GetTaskStockCardForRecordCustom.Product();
                                    resData.ProdCode = obj.ProdCode;
                                    resData.DivisionCode = obj.DivisionCode;
                                    resData.ProdNameTh = obj.ProdNameTh;
                                    resData.ProdNameEn = obj.ProdNameEn;
                                    resData.ProdType = obj.ProdType;
                                    resData.ProdGroup = obj.ProdGroup;
                                    resData.IndustrySector = obj.IndustrySector;
                                    resData.OldProdNo = obj.OldProdNo;
                                    resData.BaseUnit = obj.BaseUnit;
                                    resData.ActiveFlag = obj.ActiveFlag;
                                    resData.CreateUser = obj.CreateUser;
                                    resData.CreateDtm = obj.CreateDtm;
                                    resData.UpdateUser = obj.UpdateUser;
                                    resData.UpdateDtm = obj.UpdateDtm;
                                    resData.ReportProdConvId = obj.ReportProdConvId;
                                    lstResponse.Add(resData);
                                    i++;
                                }
                                ProductConversion rq = new ProductConversion();
                                rq.MsProductConversion = obj.MsProductConversion;
                                rq.RecordStockCard = obj.RecordStockCard;
                                listProductConversion.Add(rq);
                                resData.listProductConversion = listProductConversion;
                            }
                            //lstResponse.Add(resData);
                        }
                    }


                    lst = lstResponse;
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }
        







    }
}
