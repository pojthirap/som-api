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
using System.Data.Common;

namespace MyFirstAzureWebApp.Business.org
{

    public class MsProductImp : IMsProduct
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<SearchProductCustomCustom>> searchProduct(SearchCriteriaBase<MsProductCriteria> searchCriteria)
        {

            

            EntitySearchResultBase<SearchProductCustomCustom> searchResult = new EntitySearchResultBase<SearchProductCustomCustom>();
            List<SearchProductCustomCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("   ");

                queryBuilder.AppendFormat(" select P.ACTIVE_FLAG AS P_ACTIVE_FLAG, P.CREATE_USER AS P_CREATE_USER, P.CREATE_DTM AS P_CREATE_DTM, P.UPDATE_USER AS P_UPDATE_USER, P.UPDATE_DTM AS P_UPDATE_DTM,P.*, D.ACTIVE_FLAG AS D_ACTIVE_FLAG,D.*, PC.ACTIVE_FLAG AS PC_ACTIVE_FLAG,PC.* ");
                queryBuilder.AppendFormat(" from dbo.MS_PRODUCT P  ");
                queryBuilder.AppendFormat(" inner join dbo.ORG_DIVISION D on D.DIVISION_CODE = P.DIVISION_CODE  ");
                queryBuilder.AppendFormat(" left join MS_PRODUCT_CONVERSION PC on PC.PROD_CONV_ID = P.REPORT_PROD_CONV_ID  ");
                queryBuilder.AppendFormat(" WHERE 1= 1  ");


                MsProductCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.ProdCode))
                    {
                        queryBuilder.AppendFormat(" and P.PROD_CODE like @ProdCode  ", o.ProdCode);
                        QueryUtils.addParamLike(command, "ProdCode", o.ProdCode);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.ProdNameTh))
                    {
                        queryBuilder.AppendFormat(" and P.PROD_NAME_TH like @ProdNameTh ", o.ProdNameTh);
                        QueryUtils.addParamLike(command, "ProdNameTh", o.ProdNameTh);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.DivisionCode))
                    {
                        queryBuilder.AppendFormat(" and P.DIVISION_CODE = @DivisionCode ", o.DivisionCode);
                        QueryUtils.addParam(command, "DivisionCode", o.DivisionCode);// Add new
                    }

                }

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY P.UPDATE_DTM DESC  ");
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
                    lst = searchProductMapRow(reader);
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }

        


        private List<SearchProductCustomCustom> searchProductMapRow(DbDataReader reader)
        {
            List<SearchProductCustomCustom> lst = new List<SearchProductCustomCustom>();
            SearchProductCustomCustom o = null;
            while (reader.Read())
            {
                IDataRecord record = (IDataRecord)reader;
                o = new SearchProductCustomCustom();

                o.ProductActiveFlag = QueryUtils.getValueAsString(record, "P_ACTIVE_FLAG");
                o.CreateUser = QueryUtils.getValueAsString(record, "P_CREATE_USER");
                o.CreateDtm = QueryUtils.getValueAsDateTime(record, "P_CREATE_DTM");
                o.UpdateUser = QueryUtils.getValueAsString(record, "P_UPDATE_USER");
                o.UpdateDtm = QueryUtils.getValueAsDateTime(record, "P_UPDATE_DTM");
                o.ProdCode = QueryUtils.getValueAsString(record, "PROD_CODE");
                o.DivisionCode = QueryUtils.getValueAsString(record, "DIVISION_CODE");
                o.ProdNameTh = QueryUtils.getValueAsString(record, "PROD_NAME_TH");
                o.ProdNameEn = QueryUtils.getValueAsString(record, "PROD_NAME_EN");
                o.ProdType = QueryUtils.getValueAsString(record, "PROD_TYPE");
                o.ProdGroup = QueryUtils.getValueAsString(record, "PROD_GROUP");
                o.IndustrySector = QueryUtils.getValueAsString(record, "INDUSTRY_SECTOR");
                o.OldProdNo = QueryUtils.getValueAsString(record, "OLD_PROD_NO");
                o.BaseUnit = QueryUtils.getValueAsString(record, "BASE_UNIT");
                o.ReportProdConvId = QueryUtils.getValueAsDecimal(record, "REPORT_PROD_CONV_ID");

                o.DivisionActiveFlag = QueryUtils.getValueAsString(record, "D_ACTIVE_FLAG");
                o.DivisionNameTh = QueryUtils.getValueAsString(record, "DIVISION_NAME_TH");
                o.DivisionNameEn = QueryUtils.getValueAsString(record, "DIVISION_NAME_EN");

                o.ProductConversionActiveFlag = QueryUtils.getValueAsString(record, "PC_ACTIVE_FLAG");
                o.ProdConvId = QueryUtils.getValueAsDecimal(record, "PROD_CONV_ID");
                o.AltUnit = QueryUtils.getValueAsString(record, "ALT_UNIT");
                o.Denominator = QueryUtils.getValueAsDecimal(record, "DENOMINATOR");
                o.Counter = QueryUtils.getValueAsDecimal(record, "COUNTER");
                o.GrossWeight = QueryUtils.getValueAsDecimal(record, "GROSS_WEIGHT");
                o.WeightUnit = QueryUtils.getValueAsString(record, "WEIGHT_UNIT");


                lst.Add(o);
            }

            // Call Close when done reading.
            reader.Close();

            return lst;

        }




        public async Task<int> updateReportProductConversion(UpdateReportProductConversionModel updateReportProductConversionModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE MS_PRODUCT SET REPORT_PROD_CONV_ID=@REPORT_PROD_CONV_ID, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PROD_CODE=@PROD_CODE ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("REPORT_PROD_CONV_ID", updateReportProductConversionModel.ReportProdConvId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", updateReportProductConversionModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROD_CODE", updateReportProductConversionModel.ProdCode));// Add New
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


        public async Task<EntitySearchResultBase<SearchProductByPlantCodeCustom>> searchProductByPlantCode(SearchCriteriaBase<SearchProductByPlantCodeCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchProductByPlantCodeCustom> searchResult = new EntitySearchResultBase<SearchProductByPlantCodeCustom>();
            List<SearchProductByPlantCodeCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchProductByPlantCodeCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select P.*,PS.PROD_CATE_CODE ");
                queryBuilder.AppendFormat(" from dbo.MS_PRODUCT P ");
                queryBuilder.AppendFormat(" inner join dbo.MS_PRODUCT_PLANT PP on PP.PROD_CODE = P.PROD_CODE ");
                queryBuilder.AppendFormat(" inner join(select distinct PROD_CODE, PROD_CATE_CODE from MS_PRODUCT_SALE) PS on PS.PROD_CODE = P.PROD_CODE ");
                queryBuilder.AppendFormat(" where 1=1 ");

                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.PlanCode))
                    {
                        queryBuilder.AppendFormat(" and PP.PLANT_CODE = @PLANT_CODE ", o.PlanCode);
                        QueryUtils.addParam(command, "PLANT_CODE", o.PlanCode);// Add New
                    }

                }

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY PROD_NAME_TH  ");
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

                    List<SearchProductByPlantCodeCustom> dataRecordList = new List<SearchProductByPlantCodeCustom>();
                    SearchProductByPlantCodeCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchProductByPlantCodeCustom();


                        dataRecord.ProdCode = QueryUtils.getValueAsString(record, "PROD_CODE");
                        dataRecord.DivisionCode = QueryUtils.getValueAsString(record, "DIVISION_CODE");
                        dataRecord.ProdNameTh = QueryUtils.getValueAsString(record, "PROD_NAME_TH");
                        dataRecord.ProdNameEn = QueryUtils.getValueAsString(record, "PROD_NAME_EN");
                        dataRecord.ProdType = QueryUtils.getValueAsString(record, "PROD_TYPE");
                        dataRecord.ProdGroup = QueryUtils.getValueAsString(record, "PROD_GROUP");
                        dataRecord.IndustrySector = QueryUtils.getValueAsString(record, "INDUSTRY_SECTOR");
                        dataRecord.OldProdNo = QueryUtils.getValueAsString(record, "OLD_PROD_NO");
                        dataRecord.BaseUnit = QueryUtils.getValueAsString(record, "BASE_UNIT");
                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        dataRecord.ReportProdConvId = QueryUtils.getValueAsDecimal(record, "REPORT_PROD_CONV_ID");
                        dataRecord.ProdCateCode = QueryUtils.getValueAsString(record, "PROD_CATE_CODE"); 

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


        public async Task<EntitySearchResultBase<SearchProductConversionByProductCodeCustom>> SearchProductConversionByProductCode(SearchCriteriaBase<SearchProductConversionByProductCodeCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchProductConversionByProductCodeCustom> searchResult = new EntitySearchResultBase<SearchProductConversionByProductCodeCustom>();
            List<SearchProductConversionByProductCodeCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchProductConversionByProductCodeCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select PC.*");
                queryBuilder.AppendFormat(" from dbo.MS_PRODUCT P ");
                queryBuilder.AppendFormat(" inner join dbo.MS_PRODUCT_CONVERSION PC on PC.PROD_CODE = P.PROD_CODE ");
                queryBuilder.AppendFormat(" where 1=1  ");

                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.ProdCode))
                    {
                        queryBuilder.AppendFormat(" and P.PROD_CODE = @PROD_CODE ", o.ProdCode);
                        QueryUtils.addParam(command, "PROD_CODE", o.ProdCode);// Add New
                    }

                }

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY PROD_CODE  ");
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

                    List<SearchProductConversionByProductCodeCustom> dataRecordList = new List<SearchProductConversionByProductCodeCustom>();
                    SearchProductConversionByProductCodeCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchProductConversionByProductCodeCustom();


                        dataRecord.ProdConvId = QueryUtils.getValueAsDecimalRequired(record, "PROD_CONV_ID");
                        dataRecord.ProdCode = QueryUtils.getValueAsString(record, "PROD_CODE");
                        dataRecord.AltUnit = QueryUtils.getValueAsString(record, "ALT_UNIT");
                        dataRecord.Denominator = QueryUtils.getValueAsDecimal(record, "DENOMINATOR");
                        dataRecord.Counter = QueryUtils.getValueAsDecimal(record, "COUNTER");
                        dataRecord.GrossWeight = QueryUtils.getValueAsDecimal(record, "GROSS_WEIGHT");
                        dataRecord.WeightUnit = QueryUtils.getValueAsString(record, "WEIGHT_UNIT");

                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
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




        public async Task<EntitySearchResultBase<MsProduct>> searchProductByCustSaleId(SearchCriteriaBase<SearchProductByCustSaleIdCriteria> searchCriteria)
        {

            EntitySearchResultBase<MsProduct> searchResult = new EntitySearchResultBase<MsProduct>();
            List<MsProduct> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchProductByCustSaleIdCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select P.*,T.PROD_CATE_CODE ");
                queryBuilder.AppendFormat(" from dbo.MS_PRODUCT P ");
                queryBuilder.AppendFormat(" inner join( ");
                queryBuilder.AppendFormat("     select distinct PS.PROD_CODE,PS.PROD_CATE_CODE  ");
                queryBuilder.AppendFormat("     from MS_PRODUCT_SALE PS ");
                queryBuilder.AppendFormat("     inner join MS_PRODUCT MP on MP.PROD_CODE = PS.PROD_CODE ");
                queryBuilder.AppendFormat("     inner join CUSTOMER_SALE CS on CS.ORG_CODE = PS.ORG_CODE and CS.CHANNEL_CODE = PS.CHANNEL_CODE and CS.DIVISION_CODE = MP.DIVISION_CODE ");
                queryBuilder.AppendFormat("     where CS.CUST_SALE_ID = @CUST_SALE_ID ");
                queryBuilder.AppendFormat(" ) T on T.PROD_CODE = P.PROD_CODE ");
                QueryUtils.addParam(command, "CUST_SALE_ID", o.CustSaleId);// Add new
                


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

                    List<MsProduct> dataRecordList = new List<MsProduct>();
                    MsProduct dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new MsProduct();
                        dataRecord.ProdCode = QueryUtils.getValueAsString(record, "PROD_CODE");
                        dataRecord.DivisionCode = QueryUtils.getValueAsString(record, "DIVISION_CODE");
                        dataRecord.ProdNameTh = QueryUtils.getValueAsString(record, "PROD_NAME_TH");
                        dataRecord.ProdNameEn = QueryUtils.getValueAsString(record, "PROD_NAME_EN");
                        dataRecord.ProdType = QueryUtils.getValueAsString(record, "PROD_TYPE");
                        dataRecord.ProdGroup = QueryUtils.getValueAsString(record, "PROD_GROUP");
                        dataRecord.IndustrySector = QueryUtils.getValueAsString(record, "INDUSTRY_SECTOR");
                        dataRecord.OldProdNo = QueryUtils.getValueAsString(record, "OLD_PROD_NO");
                        dataRecord.BaseUnit = QueryUtils.getValueAsString(record, "BASE_UNIT");
                        dataRecord.ReportProdConvId = QueryUtils.getValueAsDecimal(record, "REPORT_PROD_CONV_ID");
                        dataRecord.SyncId = QueryUtils.getValueAsDecimal(record, "SYNC_ID");
                        dataRecord.ProdCateCode = QueryUtils.getValueAsString(record, "PROD_CATE_CODE");

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
