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
using MyFirstAzureWebApp.ModelCriteria.inbound.changeSaleOrder;
using MyFirstAzureWebApp.enumval;
using MyFirstAzureWebApp.ModelCriteria.inbound.cancelSaleOrder;
using static MyFirstAzureWebApp.Entity.custom.GetSaleOrderByOrderIdCustom;

namespace MyFirstAzureWebApp.Business.org
{

    public class SaleOrderImp : ISaleOrder
    {
        private Logger log = LogManager.GetCurrentClassLogger();



        public async Task<EntitySearchResultBase<SearchSaleOrderTabCustom>> searchSaleOrderTab(SearchCriteriaBase<SearchSaleOrderTabCriteria> searchCriteria, UserProfileForBack userProfile)
        {

            SearchSaleOrderTabCriteria o = searchCriteria.model;
            EntitySearchResultBase<SearchSaleOrderTabCustom> searchResult = new EntitySearchResultBase<SearchSaleOrderTabCustom>();
            List<SearchSaleOrderTabCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select O.SOM_ORDER_NO,O.SAP_ORDER_NO,O.DESCRIPTION,O.SOM_ORDER_DTE,O.PRICING_DTM,O.SAP_MSG, ");
                queryBuilder.AppendFormat(" E.FIRST_NAME+' '+E.LAST_NAME SALE_NAME,CF.LOV_NAME_TH ORDER_STATUS, CF.CONDITION1 ");
                queryBuilder.AppendFormat(" from SALE_ORDER O ");
                queryBuilder.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = O.CREATE_USER ");
                queryBuilder.AppendFormat(" inner join ms_config_lov CF on CF.LOV_KEYVALUE = O.ORDER_STATUS and CF.LOV_KEYWORD = 'ORDER_STATUS'  ");
                queryBuilder.AppendFormat(" where O.CREATE_USER = @CREATE_USER ");
                QueryUtils.addParam(command, "CREATE_USER", userProfile.getUserName());// Add new
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.CustCode))
                    {
                        queryBuilder.AppendFormat(" and O.CUST_CODE = @CUST_CODE ");
                        QueryUtils.addParam(command, "CUST_CODE", o.CustCode);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.FromDate))
                    {
                        queryBuilder.AppendFormat(" and O.SOM_ORDER_DTE >= @FromDate ");
                        QueryUtils.addParam(command, "FromDate", o.FromDate);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.ToDate))
                    {
                        queryBuilder.AppendFormat(" and O.SOM_ORDER_DTE <=  @ToDate ");
                        QueryUtils.addParam(command, "ToDate", o.ToDate.Replace("00:00:00", "23:59:59"));// Add new
                    }
                }


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY O.SOM_ORDER_NO  ");
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

                    List<SearchSaleOrderTabCustom> dataRecordList = new List<SearchSaleOrderTabCustom>();
                    SearchSaleOrderTabCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchSaleOrderTabCustom();

                        dataRecord.SomOrderNo = QueryUtils.getValueAsString(record, "SOM_ORDER_NO");
                        dataRecord.Description = QueryUtils.getValueAsString(record, "DESCRIPTION");
                        dataRecord.SomOrderDte = QueryUtils.getValueAsDateTime(record, "SOM_ORDER_DTE");
                        dataRecord.PricingDtm = QueryUtils.getValueAsDateTime(record, "PRICING_DTM");
                        dataRecord.SapMsg = QueryUtils.getValueAsString(record, "SAP_MSG");
                        dataRecord.SaleName = QueryUtils.getValueAsString(record, "SALE_NAME");
                        dataRecord.OrderStatus = QueryUtils.getValueAsString(record, "ORDER_STATUS");
                        dataRecord.SapOrderNo = QueryUtils.getValueAsString(record, "SAP_ORDER_NO");
                        dataRecord.Condition1 = QueryUtils.getValueAsString(record, "CONDITION1");
                        
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


        public async Task<EntitySearchResultBase<SearchSaleOrderCustom>> searchSaleOrder(SearchCriteriaBase<SearchSaleOrderCriteria> searchCriteria, UserProfileForBack userProfile)
        {

            SearchSaleOrderCriteria o = searchCriteria.model;
            EntitySearchResultBase<SearchSaleOrderCustom> searchResult = new EntitySearchResultBase<SearchSaleOrderCustom>();
            List<SearchSaleOrderCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select O.*,DT.DOC_TYPE_NAME_TH,CF.LOV_NAME_TH STATUS, C.CUST_NAME_TH ");
                queryBuilder.AppendFormat(" from SALE_ORDER O ");
                queryBuilder.AppendFormat(" left  join MS_ORDER_DOC_TYPE DT on DT.DOC_TYPE_CODE = O.DOC_TYPE_CODE ");
                queryBuilder.AppendFormat(" inner join CUSTOMER C on C.CUST_CODE = O.CUST_CODE ");
                queryBuilder.AppendFormat(" inner join ms_config_lov CF on CF.LOV_KEYVALUE = O.ORDER_STATUS and CF.LOV_KEYWORD = 'ORDER_STATUS' ");
                queryBuilder.AppendFormat(" where O.CREATE_USER = @CREATE_USER ");
                queryBuilder.AppendFormat(" and O.ORDER_STATUS != @ORDER_STATUS_CANCEL ");
                QueryUtils.addParam(command, "CREATE_USER", userProfile.getUserName());// Add new
                QueryUtils.addParam(command, "ORDER_STATUS_CANCEL", SaleOrderStatus.ORDER_STATUS_CANCEL.ToString("d"));// Add new
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.CustCode))
                    {
                        queryBuilder.AppendFormat(" and O.CUST_CODE = @CustCode ");
                        QueryUtils.addParam(command, "CustCode", o.CustCode);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.SomOrderNo))
                    {
                        queryBuilder.AppendFormat(" and O.SOM_ORDER_NO = @SomOrderNo ");
                        QueryUtils.addParam(command, "SomOrderNo", o.SomOrderNo);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.FromDate))
                    {
                        queryBuilder.AppendFormat(" and O.SOM_ORDER_DTE >= @FromDate ");
                        QueryUtils.addParam(command, "FromDate", o.FromDate);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.ToDate))
                    {
                        queryBuilder.AppendFormat(" and O.SOM_ORDER_DTE <=  @ToDate ");
                        QueryUtils.addParam(command, "ToDate", o.ToDate.Replace("00:00:00", "23:59:59"));// Add new
                    }
                }


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY O.SOM_ORDER_NO  ");
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

                    List<SearchSaleOrderCustom> dataRecordList = new List<SearchSaleOrderCustom>();
                    SearchSaleOrderCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchSaleOrderCustom();

                        dataRecord.OrderId = QueryUtils.getValueAsDecimalRequired(record, "ORDER_ID");
                        dataRecord.QuotationNo = QueryUtils.getValueAsString(record, "QUOTATION_NO");
                        dataRecord.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        dataRecord.SomOrderNo = QueryUtils.getValueAsString(record, "SOM_ORDER_NO");
                        dataRecord.SomOrderDte = QueryUtils.getValueAsDateTime(record, "SOM_ORDER_DTE");
                        dataRecord.SapOrderNo = QueryUtils.getValueAsString(record, "SAP_ORDER_NO");
                        dataRecord.SimulateDtm = QueryUtils.getValueAsDateTime(record, "SIMULATE_DTM");
                        dataRecord.PricingDtm = QueryUtils.getValueAsDateTime(record, "PRICING_DTM");
                        dataRecord.DocTypeCode = QueryUtils.getValueAsString(record, "DOC_TYPE_CODE");
                        dataRecord.ShipToCustPartnerId = QueryUtils.getValueAsDecimal(record, "SHIP_TO_CUST_PARTNER_ID");
                        dataRecord.Description = QueryUtils.getValueAsString(record, "DESCRIPTION");
                        dataRecord.DeliveryDte = QueryUtils.getValueAsDateTime(record, "DELIVERY_DTE");
                        dataRecord.SaleRep = QueryUtils.getValueAsString(record, "SALE_REP");
                        dataRecord.GroupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        dataRecord.OrgCode = QueryUtils.getValueAsString(record, "ORG_CODE");
                        dataRecord.ChannelCode = QueryUtils.getValueAsString(record, "CHANNEL_CODE");
                        dataRecord.DivisionCode = QueryUtils.getValueAsString(record, "DIVISION_CODE");
                        dataRecord.Territory = QueryUtils.getValueAsString(record, "TERRITORY");
                        dataRecord.ContactPerson = QueryUtils.getValueAsString(record, "CONTACT_PERSON");
                        dataRecord.Remark = QueryUtils.getValueAsString(record, "REMARK");
                        dataRecord.ReasonCode = QueryUtils.getValueAsString(record, "REASON_CODE");
                        dataRecord.ReasonReject = QueryUtils.getValueAsString(record, "REASON_REJECT");
                        dataRecord.NetValue = QueryUtils.getValueAsDecimal(record, "NET_VALUE");
                        dataRecord.Tax = QueryUtils.getValueAsDecimal(record, "TAX");
                        dataRecord.Total = QueryUtils.getValueAsDecimal(record, "TOTAL");
                        dataRecord.PaymentTerm = QueryUtils.getValueAsString(record, "PAYMENT_TERM");
                        dataRecord.Incoterm = QueryUtils.getValueAsString(record, "INCOTERM");
                        dataRecord.PlantCode = QueryUtils.getValueAsString(record, "PLANT_CODE");
                        dataRecord.ShipCode = QueryUtils.getValueAsString(record, "SHIP_CODE");
                        dataRecord.SaleSup = QueryUtils.getValueAsString(record, "SALE_SUP");
                        dataRecord.OrderStatus = QueryUtils.getValueAsString(record, "ORDER_STATUS");
                        dataRecord.CreditStatus = QueryUtils.getValueAsString(record, "CREDIT_STATUS");
                        dataRecord.SapMsg = QueryUtils.getValueAsString(record, "SAP_MSG");
                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");
                        dataRecord.SapStatus = QueryUtils.getValueAsString(record, "SAP_STATUS");
                        dataRecord.SapOrderDte = QueryUtils.getValueAsDateTime(record, "SAP_ORDER_DTE");
                        dataRecord.CustSaleId = QueryUtils.getValueAsDecimal(record, "CUST_SALE_ID");
                        dataRecord.ShipToCustCode = QueryUtils.getValueAsString(record, "SHIP_TO_CUST_CODE");
                        dataRecord.DocTypeNameTh = QueryUtils.getValueAsString(record, "DOC_TYPE_NAME_TH");
                        dataRecord.Status = QueryUtils.getValueAsString(record, "STATUS");
                        dataRecord.CustNameTh = QueryUtils.getValueAsString(record, "CUST_NAME_TH");


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




        public async Task<EntitySearchResultBase<GetSaleOrderByOrderIdCustom>> getSaleOrderByOrderId(SearchCriteriaBase<GetSaleOrderByOrderIdCriteria> searchCriteria)
        {

            GetSaleOrderByOrderIdCriteria o = searchCriteria.model;
            EntitySearchResultBase<GetSaleOrderByOrderIdCustom> searchResult = new EntitySearchResultBase<GetSaleOrderByOrderIdCustom>();
            List<GetSaleOrderByOrderIdCustom> lst = null;
            List<SaleOrderProduct> Items = null;
            GetSaleOrderByOrderIdCustom response = new GetSaleOrderByOrderIdCustom();
            SaleOrder saleOrder = null; ;
            List<OrderLogsCustom> orderLogs = null;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select OP.*,P.PROD_NAME_TH,PC.ALT_UNIT ");
                queryBuilder.AppendFormat(" from SALE_ORDER_PRODUCT OP ");
                queryBuilder.AppendFormat(" inner join MS_PRODUCT P on P.PROD_CODE = OP.PROD_CODE ");
                queryBuilder.AppendFormat(" inner join MS_PRODUCT_CONVERSION PC on PC.PROD_CONV_ID = OP.PROD_CONV_ID ");
                queryBuilder.AppendFormat(" where order_id = @OrderId_1 ");
                QueryUtils.addParam(command, "OrderId_1", o.OrderId);// Add new


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY P.PROD_NAME_TH  ");
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

                    lst = new List<GetSaleOrderByOrderIdCustom>();
                    Items = new List<SaleOrderProduct>();
                    
                    SaleOrderProduct dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SaleOrderProduct();

                        dataRecord.OrderProdId = QueryUtils.getValueAsDecimalRequired(record, "ORDER_PROD_ID");
                        dataRecord.OrderId = QueryUtils.getValueAsDecimalRequired(record, "ORDER_ID");
                        dataRecord.ProdCode = QueryUtils.getValueAsString(record, "PROD_CODE");
                        dataRecord.Qty = QueryUtils.getValueAsDecimalRequired(record, "QTY");
                        dataRecord.ProdConvId = QueryUtils.getValueAsDecimalRequired(record, "PROD_CONV_ID");
                        dataRecord.NetPriceEx = QueryUtils.getValueAsDecimal(record, "NET_PRICE_EX");
                        dataRecord.TransferPrice = QueryUtils.getValueAsDecimal(record, "TRANSFER_PRICE");
                        dataRecord.NetPriceInc = QueryUtils.getValueAsDecimal(record, "NET_PRICE_INC");
                        dataRecord.AdditionalPrice = QueryUtils.getValueAsDecimal(record, "ADDITIONAL_PRICE");
                        dataRecord.ItemType = QueryUtils.getValueAsString(record, "ITEM_TYPE");
                        dataRecord.NetValue = QueryUtils.getValueAsDecimal(record, "NET_VALUE");
                        dataRecord.SapItemNo = QueryUtils.getValueAsString(record, "SAP_ITEM_NO");
                        dataRecord.ProdAltUnit = QueryUtils.getValueAsString(record, "PROD_ALT_UNIT");
                        dataRecord.ProdNameTh = QueryUtils.getValueAsString(record, "PROD_NAME_TH");
                        dataRecord.AltUnit = QueryUtils.getValueAsString(record, "ALT_UNIT");
                        dataRecord.ProdCateCode = QueryUtils.getValueAsString(record, "PROD_CATE_CODE");
                        dataRecord.AdditionalPerUnit = QueryUtils.getValueAsDecimal(record, "ADDITIONAL_PER_UNIT");

                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");
                        


                        Items.Add(dataRecord);
                    }
                    response.Items = Items;
                    
                    // Call Close when done reading.
                    reader.Close();

                    //



                }

                queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select O.*,G.DESCRIPTION_TH GROUP_DESC_TH,CG.LOV_NAME_TH ORDER_STATUS_DES_TH ");
                queryBuilder.AppendFormat(" ,OG.ORG_NAME_TH,OC.CHANNEL_NAME_TH,OD.DIVISION_NAME_TH,CS.SHIPPING_COND ");
                queryBuilder.AppendFormat(" ,FORMAT(O.PRICING_DTM, 'yyyyMMdd') PRICE_DATE,FORMAT(O.PRICING_DTM, 'HHmmss') PRICE_TIME ");
                queryBuilder.AppendFormat(" from SALE_ORDER O ");
                queryBuilder.AppendFormat(" inner join ORG_SALE_GROUP G on G.GROUP_CODE = O.GROUP_CODE ");
                queryBuilder.AppendFormat(" inner join ORG_SALE_ORGANIZATION OG on OG.ORG_CODE = O.ORG_CODE ");
                queryBuilder.AppendFormat(" inner join ORG_DIST_CHANNEL OC on OC.CHANNEL_CODE = O.CHANNEL_CODE ");
                queryBuilder.AppendFormat(" inner join ORG_DIVISION OD on OD.DIVISION_CODE = O.DIVISION_CODE ");
                queryBuilder.AppendFormat(" inner join CUSTOMER_SALE CS on CS.CUST_SALE_ID = O.CUST_SALE_ID ");
                queryBuilder.AppendFormat(" inner join MS_CONFIG_LOV CG on CG.LOV_KEYWORD = 'ORDER_STATUS' and CG.LOV_KEYVALUE = O.ORDER_STATUS ");
                queryBuilder.AppendFormat(" where O.ORDER_ID = @OrderId_2 ");
                QueryUtils.addParam(command, "OrderId_2", o.OrderId);// Add new

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        saleOrder = new SaleOrder();

                        saleOrder.OrderId = QueryUtils.getValueAsDecimalRequired(record, "ORDER_ID");
                        saleOrder.QuotationNo = QueryUtils.getValueAsString(record, "QUOTATION_NO");
                        saleOrder.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        saleOrder.PriceList = QueryUtils.getValueAsString(record, "PRICE_LIST");
                        saleOrder.SomOrderNo = QueryUtils.getValueAsString(record, "SOM_ORDER_NO");
                        saleOrder.SomOrderDte = QueryUtils.getValueAsDateTime(record, "SOM_ORDER_DTE");
                        saleOrder.SapOrderNo = QueryUtils.getValueAsString(record, "SAP_ORDER_NO");
                        saleOrder.SimulateDtm = QueryUtils.getValueAsDateTime(record, "SIMULATE_DTM");
                        saleOrder.PricingDtm = QueryUtils.getValueAsDateTime(record, "PRICING_DTM");
                        saleOrder.DocTypeCode = QueryUtils.getValueAsString(record, "DOC_TYPE_CODE");
                        saleOrder.ShipToCustPartnerId = QueryUtils.getValueAsDecimal(record, "SHIP_TO_CUST_PARTNER_ID");
                        saleOrder.Description = QueryUtils.getValueAsString(record, "DESCRIPTION");
                        saleOrder.DeliveryDte = QueryUtils.getValueAsDateTime(record, "DELIVERY_DTE");
                        saleOrder.SaleRep = QueryUtils.getValueAsString(record, "SALE_REP");
                        saleOrder.GroupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        saleOrder.OrgCode = QueryUtils.getValueAsString(record, "ORG_CODE");
                        saleOrder.ChannelCode = QueryUtils.getValueAsString(record, "CHANNEL_CODE");
                        saleOrder.DivisionCode = QueryUtils.getValueAsString(record, "DIVISION_CODE");
                        saleOrder.Territory = QueryUtils.getValueAsString(record, "TERRITORY");
                        saleOrder.ContactPerson = QueryUtils.getValueAsString(record, "CONTACT_PERSON");
                        saleOrder.Remark = QueryUtils.getValueAsString(record, "REMARK");
                        saleOrder.ReasonCode = QueryUtils.getValueAsString(record, "REASON_CODE");
                        saleOrder.ReasonReject = QueryUtils.getValueAsString(record, "REASON_REJECT");
                        saleOrder.NetValue = QueryUtils.getValueAsDecimal(record, "NET_VALUE");
                        saleOrder.Tax = QueryUtils.getValueAsDecimal(record, "TAX");
                        saleOrder.Total = QueryUtils.getValueAsDecimal(record, "TOTAL");
                        saleOrder.PaymentTerm = QueryUtils.getValueAsString(record, "PAYMENT_TERM");
                        saleOrder.Incoterm = QueryUtils.getValueAsString(record, "INCOTERM");
                        saleOrder.PlantCode = QueryUtils.getValueAsString(record, "PLANT_CODE");
                        saleOrder.ShipCode = QueryUtils.getValueAsString(record, "SHIP_CODE");
                        saleOrder.SaleSup = QueryUtils.getValueAsString(record, "SALE_SUP");
                        saleOrder.OrderStatus = QueryUtils.getValueAsString(record, "ORDER_STATUS");
                        saleOrder.CreditStatus = QueryUtils.getValueAsString(record, "CREDIT_STATUS");
                        saleOrder.SapMsg = QueryUtils.getValueAsString(record, "SAP_MSG");
                        saleOrder.SapStatus = QueryUtils.getValueAsString(record, "SAP_STATUS");
                        saleOrder.SapOrderDte = QueryUtils.getValueAsDateTime(record, "SAP_ORDER_DTE");
                        saleOrder.CustSaleId = QueryUtils.getValueAsDecimal(record, "CUST_SALE_ID");
                        saleOrder.ShipToCustCode = QueryUtils.getValueAsString(record, "SHIP_TO_CUST_CODE");
                        saleOrder.GroupDescTh = QueryUtils.getValueAsString(record, "GROUP_DESC_TH");
                        saleOrder.OrderStatusDesTh = QueryUtils.getValueAsString(record, "ORDER_STATUS_DES_TH");
                        saleOrder.Incoterm = QueryUtils.getValueAsString(record, "INCOTERM");
                        saleOrder.CompanyCode = QueryUtils.getValueAsString(record, "COMPANY_CODE");
                        saleOrder.OrgNameTh = QueryUtils.getValueAsString(record, "ORG_NAME_TH");
                        saleOrder.ChannelNameTh = QueryUtils.getValueAsString(record, "CHANNEL_NAME_TH");
                        saleOrder.DivisionNameTh = QueryUtils.getValueAsString(record, "DIVISION_NAME_TH");
                        saleOrder.ShippingCond = QueryUtils.getValueAsString(record, "SHIPPING_COND");

                        saleOrder.PriceDate = QueryUtils.getValueAsString(record, "PRICE_DATE");
                        saleOrder.PriceTime = QueryUtils.getValueAsString(record, "PRICE_TIME");

                        saleOrder.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        saleOrder.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                        saleOrder.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        saleOrder.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");

                        saleOrder.PoNo = QueryUtils.getValueAsString(record, "PO_NO");
                    }

                    // Call Close when done reading.
                    reader.Close();

                    //

                }

                response.SaleOrder = saleOrder;




                queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" SELECT CFG.LOV_NAME_TH ORDER_ACTION,SO.SOM_ORDER_NO,SO.SAP_ORDER_NO,SO.SAP_STATUS,SO.SAP_MSG ");
                queryBuilder.AppendFormat(" from SALE_ORDER_LOG SO ");
                queryBuilder.AppendFormat(" inner join MS_CONFIG_LOV CFG on CFG.LOV_KEYWORD = 'ORDER_ACTION' and CFG.LOV_KEYVALUE = SO.ORDER_ACTION ");
                queryBuilder.AppendFormat(" where SO.ORDER_ID = @OrderId_3 ");
                queryBuilder.AppendFormat(" order by SO.CREATE_DTM DESC ");
                QueryUtils.addParam(command, "OrderId_3", o.OrderId);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();

                using (var reader = command.ExecuteReader())
                {
                    orderLogs = new List<OrderLogsCustom>();

                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        OrderLogsCustom OrderLogsCustom = new OrderLogsCustom();

                        OrderLogsCustom.OrderAction = QueryUtils.getValueAsString(record, "ORDER_ACTION");
                        OrderLogsCustom.SomOrderNo = QueryUtils.getValueAsString(record, "SOM_ORDER_NO");
                        OrderLogsCustom.SapOrderNo = QueryUtils.getValueAsString(record, "SAP_ORDER_NO");
                        OrderLogsCustom.SapStatus = QueryUtils.getValueAsString(record, "SAP_STATUS");
                        OrderLogsCustom.SapMsg = QueryUtils.getValueAsString(record, "SAP_MSG");
                        orderLogs.Add(OrderLogsCustom);
                    }

                    // Call Close when done reading.
                    reader.Close();

                    //

                }

                response.OrderLogs = orderLogs;


                lst.Add(response);
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
            }
            return searchResult;
        }


        public async Task<InboundChangeSaleOrderModelResponse> changeSaleOrder(InboundChangeSaleOrderModel model)
        {
            InboundChangeSaleOrderModelResponse response = new InboundChangeSaleOrderModelResponse();
            StatusSOM_Change statusSOM = new StatusSOM_Change();
            Header_Change header = new Header_Change();
            Status_Change status = new Status_Change();
            status = model.Status;
            header = model.Header;

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        

                        List < InboundGetSaleOrderForChangeModel > lstSaleOrder = await getSaleOrderForChange(model);

                        if (lstSaleOrder == null || lstSaleOrder.Count==0)
                        {

                            statusSOM.SOM_Status = "E";
                            statusSOM.SOM_Message = "Data Not Found - For Change";


                            response.Header = header;
                            response.StatusSOM = statusSOM;
                            response.Status = status;
                            return response;
                        }

                        InboundGetSaleOrderForChangeModel saleOrder = lstSaleOrder.ElementAt(0);

                        // SALE_ORDER_CHANGE_LOG
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO SALE_ORDER_CHANGE_LOG ([LOG_ID], [ORDER_ID], [ORDER_SALE_REP], [CHANGE_TAB_DESC], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM])  ");
                        queryBuilder.AppendFormat(" VALUES(NEXT VALUE FOR SALE_ORDER_CHANGE_LOG_SEQ, @ORDER_ID, @SALE_REP, 'Change Sale Order By SAP', 'SAP_SYSTEM', dbo.GET_SYSDATETIME(), 'SAP_SYSTEM', dbo.GET_SYSDATETIME()) ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", saleOrder.OrderId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SALE_REP", saleOrder.SaleRep));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        // SALE_ORDER
                        string VAL_ORDER_STATUS = SaleOrderStatus.VAL_ORDER_STATUS.ToString("d");
                        string ORDER_ACTION_CHANGE_BY_SAP = SaleOrderAction.ORDER_ACTION_CHANGE_BY_SAP.ToString("d");
                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();

                        queryBuilder.AppendFormat(" UPDATE SALE_ORDER ");
                        queryBuilder.AppendFormat(" SET  ");
                        queryBuilder.AppendFormat(" [DELIVERY_DTE]=@Request_Delivery_date,  ");
                        queryBuilder.AppendFormat(" [ORG_CODE]=@Sale_Org,  ");
                        queryBuilder.AppendFormat(" [CHANNEL_CODE]=@Distribution_Channel,  ");
                        queryBuilder.AppendFormat(" [DIVISION_CODE]=@Division,  ");
                        queryBuilder.AppendFormat(" [PO_NO]= @PO_Number,   ");
                        queryBuilder.AppendFormat(" [PRICE_LIST]= @Price_List,  ");
                        queryBuilder.AppendFormat(" [CUST_SALE_ID]=@custSaleId,   ");
                        queryBuilder.AppendFormat(" [SHIP_TO_CUST_PARTNER_ID]=@custPartnerId,  ");
                        queryBuilder.AppendFormat(" [SHIP_TO_CUST_CODE]=@Ship_to_customer,  ");
                        queryBuilder.AppendFormat(" [PLANT_CODE]=@Plant,  ");
                        queryBuilder.AppendFormat(" [SHIP_CODE]=@Shipping_Point_Receiving_Pt,  ");
                        queryBuilder.AppendFormat(" [ORDER_STATUS]=@VAL_ORDER_STATUS,  ");
                        queryBuilder.AppendFormat(" [ORDER_ACTION]=@ORDER_ACTION_CHANGE_BY_SAP, ");
                        queryBuilder.AppendFormat(" [NET_VALUE]=@Sum_Net_Price_by_SO,  ");
                        queryBuilder.AppendFormat(" [TAX]=@Tax_Amount_by_SO,  ");
                        queryBuilder.AppendFormat(" [TOTAL]=@SUM_Total_NetADDVat,  ");
                        queryBuilder.AppendFormat(" [PAYMENT_TERM]=@Payment_Term,  ");
                        queryBuilder.AppendFormat(" [INCOTERM]=@Incoterms1,  ");
                        queryBuilder.AppendFormat(" [PRICING_DTM]=cast(@Price_Date+ ' ' + substring(@Price_Time, 1, 2)+ ':' + substring(@Price_Time, 3, 2)+ ':' + substring(@Price_Time, 5, 2) as datetime),  ");
                        queryBuilder.AppendFormat(" [CREDIT_STATUS]=@Credit_Status,  ");
                        queryBuilder.AppendFormat(" [SAP_MSG]=@SO_Message,  ");
                        queryBuilder.AppendFormat(" [SAP_STATUS]=@SO_Status,  ");
                        queryBuilder.AppendFormat(" [REASON_REJECT]= @Reason_For_Rejection,  ");
                        queryBuilder.AppendFormat(" [REASON_CODE]= @Order_Reason,  ");
                        queryBuilder.AppendFormat(" [UPDATE_USER]='SAP_SYSTEM', [UPDATE_DTM]=dbo.GET_SYSDATETIME() ");
                        queryBuilder.AppendFormat(" WHERE [ORDER_ID]=@orderId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("Request_Delivery_date", header.Request_Delivery_date));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Sale_Org", header.Sale_Org));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Distribution_Channel", header.Distribution_Channel));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Division", header.Division));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PO_Number", header.PO_Number));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Price_List", header.Price_List));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("custSaleId", saleOrder.CustSaleId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("custPartnerId", saleOrder.CustPartnerId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Ship_to_customer", header.Ship_to_customer));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Plant", header.Item.ElementAt(0).Plant));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Shipping_Point_Receiving_Pt", header.Item.ElementAt(0).Shipping_Point_Receiving_Pt));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ORDER_STATUS", VAL_ORDER_STATUS));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ACTION_CHANGE_BY_SAP", ORDER_ACTION_CHANGE_BY_SAP));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Sum_Net_Price_by_SO", header.Sum_Net_Price_by_SO));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Tax_Amount_by_SO", header.Tax_Amount_by_SO));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SUM_Total_NetADDVat", header.SUM_Total_NetADDVat));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Payment_Term", header.Payment_Term));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Incoterms1", header.Incoterms1));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Price_Date", header.Price_Date));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Price_Time", header.Price_Time));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Credit_Status", status.Credit_Status));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SO_Message", status.SO_Message));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SO_Status", status.SO_Status));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Reason_For_Rejection", header.Reason_For_Rejection));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Order_Reason", header.Order_Reason));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("orderId", saleOrder.OrderId));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);



                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" DELETE FROM SALE_ORDER_PRODUCT WHERE ORDER_ID = @orderId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("orderId", saleOrder.OrderId));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);



                        foreach (Item_Change data in model.Header.Item)
                        {

                            decimal Condition_Total_Value = String.IsNullOrEmpty(data.Condition_Total_Value) ? 0 : Convert.ToDecimal(data.Condition_Total_Value);
                            decimal Condition_Amount = 0;
                            if (data.Condition != null && data.Condition.Count != 0)
                            {
                                List<Condition_Change> filteredList = data.Condition.Where(o => o.Condition_type == "ZL34").ToList();
                                if (filteredList != null && filteredList.Count != 0)
                                {
                                    foreach (Condition_Change c in filteredList)
                                    {
                                        Condition_Amount += decimal.Parse(c.Condition_Amount.ToString());
                                    }

                                }
                            }

                            decimal VAL_NET_PRICE_EX = Condition_Total_Value - Condition_Amount;

                            Condition_Amount = 0;
                            if (data.Condition != null && data.Condition.Count != 0)
                            {
                                List<Condition_Change> filteredList = data.Condition.Where(o => o.Condition_type == "ZL51").ToList();
                                if (filteredList != null && filteredList.Count != 0)
                                {
                                    foreach (Condition_Change c in filteredList)
                                    {
                                        Condition_Amount += decimal.Parse(c.Condition_Amount.ToString());
                                    }

                                }
                            }
                            decimal VAL_TRANSFER_PRICE = Condition_Amount;
                            decimal VAL_NET_PRICE_INC = String.IsNullOrEmpty(data.Condition_Total_Value) ? 0 : Convert.ToDecimal(data.Condition_Total_Value);

                            Condition_Amount = 0;
                            if (data.Condition != null && data.Condition.Count != 0)
                            {
                                List<Condition_Change> filteredList = data.Condition.Where(o => o.Condition_type == "ZL34").ToList();
                                if (filteredList != null && filteredList.Count != 0)
                                {
                                    foreach (Condition_Change c in filteredList)
                                    {
                                        Condition_Amount += decimal.Parse(c.Condition_Amount.ToString());
                                    }

                                }
                            }
                            decimal VAL_ADDITIONAL_PRICE = Condition_Amount;

                            //
                            decimal Condition_Per = 0;
                            if (data.Condition != null && data.Condition.Count != 0)
                            {
                                List<Condition_Change> filteredList = data.Condition.Where(o => o.Condition_type == "ZL34").ToList();
                                if (filteredList != null && filteredList.Count != 0)
                                {
                                    foreach (Condition_Change c in filteredList)
                                    {
                                        Condition_Per += decimal.Parse(c.Condition_Per.ToString());
                                    }

                                }
                            }
                            decimal VAL_ADDITIONAL_PER_UNIT = Condition_Per;
                            //


                            string VAL_NET_VALUE = data.Total_Value;
                            string VAL_ITEM_TYPE = data.Item_Category;
                            string VAL_PROD_CONV_ID = await GetProdConvId(data);



                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat(" INSERT INTO SALE_ORDER_PRODUCT ([ORDER_PROD_ID], [ORDER_ID], [SAP_ITEM_NO], [PROD_CODE], [QTY], [PROD_CONV_ID], [PROD_ALT_UNIT], [ADDITIONAL_PRICE], [ADDITIONAL_PER_UNIT], [NET_PRICE_EX], [TRANSFER_PRICE], [NET_PRICE_INC], [NET_VALUE], [ITEM_TYPE], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM])  ");
                            queryBuilder.AppendFormat(" VALUES(NEXT VALUE FOR SALE_ORDER_PRODUCT_SEQ, @orderId, @Item_no, @Material_Code, @Order_Quantity, @VAL_PROD_CONV_ID, @Unit, @VAL_ADDITIONAL_PRICE, @VAL_ADDITIONAL_PER_UNIT, @VAL_NET_PRICE_EX, @VAL_TRANSFER_PRICE, @VAL_NET_PRICE_INC, @VAL_NET_VALUE, @VAL_ITEM_TYPE, @CreateUser, @createDtm, 'SAP_SYSTEM', dbo.GET_SYSDATETIME()) ");

                            sqlParameters.Add(QueryUtils.addSqlParameter("orderId", saleOrder.OrderId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("Item_no", data.Item_no));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("Material_Code", data.Material_Code));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("Order_Quantity", data.Order_Quantity));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PROD_CONV_ID", VAL_PROD_CONV_ID));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("Unit", data.Unit));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ADDITIONAL_PRICE", VAL_ADDITIONAL_PRICE));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ADDITIONAL_PER_UNIT", VAL_ADDITIONAL_PER_UNIT));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_NET_PRICE_EX", VAL_NET_PRICE_EX));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_TRANSFER_PRICE", VAL_TRANSFER_PRICE));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_NET_PRICE_INC", VAL_NET_PRICE_INC));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_NET_VALUE", VAL_NET_VALUE));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ITEM_TYPE", VAL_ITEM_TYPE));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("CreateUser", saleOrder.CreateUser));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("createDtm", saleOrder.CreateDtm));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);





                        }




                        //

                        decimal VAL_ORDER_LOG_ID = await getNextValueSALE_ORDER_LOG_SEQ();
                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO SALE_ORDER_LOG ([ORDER_LOG_ID], [ORDER_ID], [QUOTATION_NO], [CUST_CODE], [SOM_ORDER_NO], [PO_NO], [SOM_ORDER_DTE], [SAP_ORDER_NO], [SIMULATE_DTM], [PRICING_DTM], [DOC_TYPE_CODE], [SHIP_TO_CUST_PARTNER_ID], [DESCRIPTION], [DELIVERY_DTE], [SALE_REP], [GROUP_CODE], [ORG_CODE], [CHANNEL_CODE], [DIVISION_CODE], [PRICE_LIST], [TERRITORY], [CONTACT_PERSON], [REMARK], [REASON_CODE], [REASON_REJECT], [NET_VALUE], [TAX], [TOTAL], [PAYMENT_TERM], [INCOTERM], [PLANT_CODE], [SHIP_CODE], [SALE_SUP], [ORDER_STATUS], [ORDER_ACTION], [CREDIT_STATUS], [SAP_MSG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_STATUS], [SAP_ORDER_DTE], [CUST_SALE_ID], [SHIP_TO_CUST_CODE], [COMPANY_CODE])  ");
                        queryBuilder.AppendFormat(" SELECT @VAL_ORDER_LOG_ID, [ORDER_ID], [QUOTATION_NO], [CUST_CODE], [SOM_ORDER_NO], [PO_NO], [SOM_ORDER_DTE], [SAP_ORDER_NO], [SIMULATE_DTM], [PRICING_DTM], [DOC_TYPE_CODE], [SHIP_TO_CUST_PARTNER_ID], [DESCRIPTION], [DELIVERY_DTE], [SALE_REP], [GROUP_CODE], [ORG_CODE], [CHANNEL_CODE], [DIVISION_CODE], [PRICE_LIST], [TERRITORY], [CONTACT_PERSON], [REMARK], [REASON_CODE], [REASON_REJECT], [NET_VALUE], [TAX], [TOTAL], [PAYMENT_TERM], [INCOTERM], [PLANT_CODE], [SHIP_CODE], [SALE_SUP], [ORDER_STATUS], [ORDER_ACTION], [CREDIT_STATUS], [SAP_MSG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_STATUS], [SAP_ORDER_DTE], [CUST_SALE_ID], [SHIP_TO_CUST_CODE], [COMPANY_CODE] FROM SALE_ORDER WHERE [ORDER_ID] = @ORDER_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", saleOrder.OrderId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ORDER_LOG_ID", VAL_ORDER_LOG_ID));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();

                        queryBuilder.AppendFormat(" INSERT INTO SALE_ORDER_PRODUCT_LOG ([ORDER_PROD_LOG_ID], [ORDER_LOG_ID], [PROD_CODE], [QTY], [PROD_CONV_ID], [NET_PRICE_EX], [TRANSFER_PRICE], [NET_PRICE_INC], [ADDITIONAL_PRICE], [ADDITIONAL_PER_UNIT], [ITEM_TYPE], [NET_VALUE], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_ITEM_NO], [PROD_ALT_UNIT], [PROD_CATE_CODE])  ");
                        queryBuilder.AppendFormat(" SELECT NEXT VALUE FOR SALE_ORDER_PRODUCT_LOG_SEQ, @VAL_ORDER_LOG_ID, [PROD_CODE], [QTY], [PROD_CONV_ID], [NET_PRICE_EX], [TRANSFER_PRICE], [NET_PRICE_INC], [ADDITIONAL_PRICE], [ADDITIONAL_PER_UNIT], [ITEM_TYPE], [NET_VALUE], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_ITEM_NO], [PROD_ALT_UNIT], [PROD_CATE_CODE] FROM SALE_ORDER_PRODUCT WHERE [ORDER_ID] = @ORDER_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", saleOrder.OrderId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ORDER_LOG_ID", VAL_ORDER_LOG_ID));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);



                        //


                        transaction.Commit();


                        statusSOM.SOM_Status = "S";
                        statusSOM.SOM_Message = "Change Success";
                        status = model.Status;
                        header = model.Header;


                        response.Header = header;
                        response.StatusSOM = statusSOM;
                        response.Status = status;

                        return response;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        statusSOM.SOM_Status = "E";
                        statusSOM.SOM_Message = ex.Message+":"+ ex.ToString();
                        status = model.Status;
                        header = model.Header;


                        response.Header = header;
                        response.StatusSOM = statusSOM;
                        response.Status = status;

                        return response;
                        //throw;
                    }
                }
            }

        }


        public async Task<decimal> getNextValueSALE_ORDER_LOG_SEQ()
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
                        context.Database.ExecuteSqlRaw("set @result = next value for SALE_ORDER_LOG_SEQ", p);
                        var nextVal = (int)p.Value;
                        return Convert.ToDecimal(nextVal);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

        }

        public async Task<String> GetProdConvId(Item_Change data)
        {
            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select PROD_CONV_ID ");
                queryBuilder.AppendFormat(" from MS_PRODUCT_CONVERSION ");
                queryBuilder.AppendFormat(" where PROD_CODE = @Material_Code ");
                queryBuilder.AppendFormat(" and ALT_UNIT = @Unit ");
                QueryUtils.addParam(command, "Material_Code", data.Material_Code);// Add new
                QueryUtils.addParam(command, "Unit", data.Unit);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "PROD_CONV_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }



        private async Task<List<InboundGetSaleOrderForChangeModel>> getSaleOrderForChange(InboundChangeSaleOrderModel model)
        {
            List<InboundGetSaleOrderForChangeModel> lst = new List<InboundGetSaleOrderForChangeModel>();
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();


                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select CS.CUST_SALE_ID,CP.CUST_PARTNER_ID,O.* ");
                queryBuilder.AppendFormat(" from SALE_ORDER O ");
                queryBuilder.AppendFormat(" inner join CUSTOMER_SALE CS on (");
                queryBuilder.AppendFormat("     CS.CUST_CODE = O.CUST_CODE  ");
                queryBuilder.AppendFormat("     and CS.ORG_CODE = @Sale_Org  ");
                queryBuilder.AppendFormat("     and CS.CHANNEL_CODE = @Distribution_Channel  ");
                queryBuilder.AppendFormat("     and CS.DIVISION_CODE = @Division) ");
                queryBuilder.AppendFormat(" inner join CUSTOMER_PARTNER CP on(");
                queryBuilder.AppendFormat("     CP.CUST_CODE = O.CUST_CODE  ");
                queryBuilder.AppendFormat("     and CP.ORG_CODE = CS.ORG_CODE  ");
                queryBuilder.AppendFormat("     and CP.CHANNEL_CODE = CS.CHANNEL_CODE  ");
                queryBuilder.AppendFormat("     and CP.DIVISION_CODE = CS.DIVISION_CODE  ");
                queryBuilder.AppendFormat("     and CP.FUNC_CODE= 'SH'  ");
                queryBuilder.AppendFormat("     and CP.CUST_CODE_PARTNER = @Ship_to_customer) ");
                queryBuilder.AppendFormat(" where SOM_ORDER_NO = @SOM_order_no ");

                QueryUtils.addParam(command, "Sale_Org", model.Header.Sale_Org);
                QueryUtils.addParam(command, "Distribution_Channel", model.Header.Distribution_Channel);
                QueryUtils.addParam(command, "Division", model.Header.Division);
                QueryUtils.addParam(command, "Ship_to_customer", model.Header.Ship_to_customer);
                QueryUtils.addParam(command, "SOM_order_no", model.Header.SOM_order_no);


                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    InboundGetSaleOrderForChangeModel recordData = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        recordData = new InboundGetSaleOrderForChangeModel();
                        recordData.CustSaleId = QueryUtils.getValueAsString(record, "CUST_SALE_ID");
                        recordData.CustPartnerId = QueryUtils.getValueAsString(record, "CUST_PARTNER_ID");
                        recordData.OrderId = QueryUtils.getValueAsDecimalRequired(record, "ORDER_ID");
                        recordData.QuotationNo = QueryUtils.getValueAsString(record, "QUOTATION_NO");
                        recordData.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        recordData.SomOrderNo = QueryUtils.getValueAsString(record, "SOM_ORDER_NO");
                        recordData.SomOrderDte = QueryUtils.getValueAsDateTime(record, "SOM_ORDER_DTE");
                        recordData.SapOrderNo = QueryUtils.getValueAsString(record, "SAP_ORDER_NO");
                        recordData.SimulateDtm = QueryUtils.getValueAsDateTime(record, "SIMULATE_DTM");
                        recordData.PricingDtm = QueryUtils.getValueAsDateTime(record, "PRICING_DTM");
                        recordData.DocTypeCode = QueryUtils.getValueAsString(record, "DOC_TYPE_CODE");
                        recordData.ShipToCustPartnerId = QueryUtils.getValueAsDecimal(record, "SHIP_TO_CUST_PARTNER_ID");
                        recordData.Description = QueryUtils.getValueAsString(record, "DESCRIPTION");
                        recordData.DeliveryDte = QueryUtils.getValueAsDateTime(record, "DELIVERY_DTE");
                        recordData.SaleRep = QueryUtils.getValueAsString(record, "SALE_REP");
                        recordData.GroupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        recordData.OrgCode = QueryUtils.getValueAsString(record, "ORG_CODE");
                        recordData.ChannelCode = QueryUtils.getValueAsString(record, "CHANNEL_CODE");
                        recordData.DivisionCode = QueryUtils.getValueAsString(record, "DIVISION_CODE");
                        recordData.Territory = QueryUtils.getValueAsString(record, "TERRITORY");
                        recordData.ContactPerson = QueryUtils.getValueAsString(record, "CONTACT_PERSON");
                        recordData.Remark = QueryUtils.getValueAsString(record, "REMARK");
                        recordData.ReasonCode = QueryUtils.getValueAsString(record, "REASON_CODE");
                        recordData.ReasonReject = QueryUtils.getValueAsString(record, "REASON_REJECT");
                        recordData.NetValue = QueryUtils.getValueAsDecimal(record, "NET_VALUE");
                        recordData.Tax = QueryUtils.getValueAsDecimal(record, "TAX");
                        recordData.Total = QueryUtils.getValueAsDecimal(record, "TOTAL");
                        recordData.PaymentTerm = QueryUtils.getValueAsString(record, "PAYMENT_TERM");
                        recordData.Incoterm = QueryUtils.getValueAsString(record, "INCOTERM");
                        recordData.PlantCode = QueryUtils.getValueAsString(record, "PLANT_CODE");
                        recordData.ShipCode = QueryUtils.getValueAsString(record, "SHIP_CODE");
                        recordData.SaleSup = QueryUtils.getValueAsString(record, "SALE_SUP");
                        recordData.OrderStatus = QueryUtils.getValueAsString(record, "ORDER_STATUS");
                        recordData.CreditStatus = QueryUtils.getValueAsString(record, "CREDIT_STATUS");
                        recordData.SapMsg = QueryUtils.getValueAsString(record, "SAP_MSG");
                        recordData.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        recordData.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        recordData.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        recordData.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");
                        recordData.SapStatus = QueryUtils.getValueAsString(record, "SAP_STATUS");
                        recordData.SapOrderDte = QueryUtils.getValueAsDateTime(record, "SAP_ORDER_DTE");
                        recordData.CustSaleId = QueryUtils.getValueAsString(record, "CUST_SALE_ID");
                        recordData.ShipToCustCode = QueryUtils.getValueAsString(record, "SHIP_TO_CUST_CODE");
                        lst.Add(recordData);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return lst;
        }



        public async Task<InboundCancelSaleOrderModelResponse> cancelSaleOrder(InboundCancelSaleOrderModel model)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        InboundCancelSaleOrderModelResponse response = new InboundCancelSaleOrderModelResponse();
                        StatusSOM_Cancel statusSOM = new StatusSOM_Cancel();
                        Header_Cancel header = new Header_Cancel();

                        string ORDER_STATUS_CANCEL = SaleOrderStatus.ORDER_STATUS_CANCEL.ToString("d");
                        string ORDER_ACTION_CANCEL_BY_SAP = SaleOrderAction.ORDER_ACTION_CANCEL_BY_SAP.ToString("d");
                       // SALE_ORDER_CHANGE_LOG
                       var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" UPDATE SALE_ORDER  ");
                        queryBuilder.AppendFormat(" SET [ORDER_STATUS]=@ORDER_STATUS_CANCEL, [ORDER_ACTION]=@ORDER_ACTION_CANCEL_BY_SAP, [UPDATE_USER]='SAP_SYSTEM', [UPDATE_DTM]=dbo.GET_SYSDATETIME() ");
                        queryBuilder.AppendFormat(" WHERE SOM_ORDER_NO = @SOM_Order_No ");
                        queryBuilder.AppendFormat(" and SAP_ORDER_NO = @SAP_Sale_Order ");
                        queryBuilder.AppendFormat(" and ORG_CODE = @Sale_Org ");
                        queryBuilder.AppendFormat(" and CHANNEL_CODE = @Distribution_Channel ");
                        queryBuilder.AppendFormat(" and DIVISION_CODE = @Division ");
                        queryBuilder.AppendFormat(" and FORMAT(SOM_ORDER_DTE,'yyyyMMdd')=@Document_Date ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_STATUS_CANCEL", ORDER_STATUS_CANCEL));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ACTION_CANCEL_BY_SAP", ORDER_ACTION_CANCEL_BY_SAP));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SOM_Order_No", model.Header.SOM_Order_No));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SAP_Sale_Order", model.Header.SAP_Sale_Order));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Sale_Org", model.Header.Sale_Org));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Distribution_Channel", model.Header.Distribution_Channel));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Division", model.Header.Division));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("Document_Date", model.Header.Document_Date));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();


                        if (numberOfRowInserted == 0)
                        {

                            statusSOM.SOM_Status = "E";
                            statusSOM.SOM_Message = "Data Not Found - For Cancel";
                            header = model.Header;

                            response.Header = header;
                            response.StatusSOM = statusSOM;
                            return response;
                        }


                        statusSOM.SOM_Status = "S";
                        statusSOM.SOM_Message = "Cancel Success";
                        header = model.Header;


                        response.Header = header;
                        response.StatusSOM = statusSOM;

                        return response;

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