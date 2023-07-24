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
using MyFirstAzureWebApp.Models.saleorder;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.plan;
using MyFirstAzureWebApp.enumval;
using Newtonsoft.Json;
using MyFirstAzureWebApp.common;
using MyFirstAzureWebApp.exception;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using MyFirstAzureWebApp.ModelCriteria.outbound.cancelsaleorder;
using System.Net.Http.Headers;
using MyFirstAzureWebApp.Controllers;

namespace MyFirstAzureWebApp.Business.org
{

    public class CustomerSaleImp : ICustomerSale
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<SearchSaleDataTabCustom>> searchSaleDataTab(SearchCriteriaBase<SearchSaleDataTabCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchSaleDataTabCustom> searchResult = new EntitySearchResultBase<SearchSaleDataTabCustom>();
            List<SearchSaleDataTabCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchSaleDataTabCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select SO.ORG_NAME_TH,DC.CHANNEL_NAME_TH,D.DIVISION_NAME_TH,O.DESCRIPTION_TH OFFICE_NAME_TH,G.DESCRIPTION_TH GROUP_NAME_TH, ");
                queryBuilder.AppendFormat(" S.CUST_GROUP,S.PAYMENT_TERM,S.INCOTERM ");
                queryBuilder.AppendFormat(" from CUSTOMER_SALE S ");
                queryBuilder.AppendFormat(" inner join ORG_SALE_ORGANIZATION SO on SO.ORG_CODE = S.ORG_CODE ");
                queryBuilder.AppendFormat(" inner join ORG_DIST_CHANNEL DC on DC.CHANNEL_CODE = S.CHANNEL_CODE ");
                queryBuilder.AppendFormat(" inner join ORG_DIVISION D on D.DIVISION_CODE = S.DIVISION_CODE ");
                queryBuilder.AppendFormat(" inner join ORG_SALE_OFFICE O on O.OFFICE_CODE = S.OFFICE_CODE ");
                queryBuilder.AppendFormat(" inner join ORG_SALE_GROUP G on G.GROUP_CODE = S.GROUP_CODE ");
                queryBuilder.AppendFormat(" where 1= 1  ");
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.CustCode))
                    {
                        queryBuilder.AppendFormat(" and  S.CUST_CODE = @CustCode  ");
                        QueryUtils.addParam(command, "CustCode", o.CustCode);// Add new
                    }
                }



                // For Paging
                queryBuilder.AppendFormat(" ORDER BY SO.ORG_NAME_TH  ");
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

                    List<SearchSaleDataTabCustom> dataRecordList = new List<SearchSaleDataTabCustom>();
                    SearchSaleDataTabCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchSaleDataTabCustom();


                        dataRecord.OrgNameTh = QueryUtils.getValueAsString(record, "ORG_NAME_TH");
                        dataRecord.ChannelNameTh = QueryUtils.getValueAsString(record, "CHANNEL_NAME_TH");
                        dataRecord.DivisionNameTh = QueryUtils.getValueAsString(record, "DIVISION_NAME_TH");
                        dataRecord.OfficeNameTh = QueryUtils.getValueAsString(record, "OFFICE_NAME_TH");
                        dataRecord.GroupNameTh = QueryUtils.getValueAsString(record, "GROUP_NAME_TH");
                        dataRecord.CustGroup = QueryUtils.getValueAsString(record, "CUST_GROUP");
                        dataRecord.PaymentTerm = QueryUtils.getValueAsString(record, "PAYMENT_TERM");
                        dataRecord.Incoterm = QueryUtils.getValueAsString(record, "INCOTERM");

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




        public async Task<EntitySearchResultBase<SearchShipToByCustSaleIdCustom>> searchShipToByCustSaleId(SearchCriteriaBase<SearchShipToByCustSaleIdCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchShipToByCustSaleIdCustom> searchResult = new EntitySearchResultBase<SearchShipToByCustSaleIdCustom>();
            List<SearchShipToByCustSaleIdCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchShipToByCustSaleIdCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select CP.CUST_PARTNER_ID,C.CUST_CODE,C.CUST_NAME_TH  + ' ' + isnull(C.CUST_NAME_EN,'') CUST_NAME_TH   ");
                queryBuilder.AppendFormat(" from CUSTOMER_SALE CS,CUSTOMER_PARTNER CP,CUSTOMER C ");
                queryBuilder.AppendFormat(" WHERE CS.CUST_SALE_ID = @CustSaleId ");
                queryBuilder.AppendFormat(" and CP.CUST_CODE = CS.CUST_CODE ");
                queryBuilder.AppendFormat(" and CS.ORG_CODE = CP.ORG_CODE ");
                queryBuilder.AppendFormat(" and CS.CHANNEL_CODE = CP.CHANNEL_CODE ");
                queryBuilder.AppendFormat(" and CS.DIVISION_CODE = CP.DIVISION_CODE ");
                queryBuilder.AppendFormat(" and CP.FUNC_CODE = 'SH' ");
                queryBuilder.AppendFormat(" and CP.CUST_CODE_PARTNER = C.CUST_CODE ");
                QueryUtils.addParam(command, "CustSaleId", o.CustSaleId);
                


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY CUST_NAME_TH  ");
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

                    List<SearchShipToByCustSaleIdCustom> dataRecordList = new List<SearchShipToByCustSaleIdCustom>();
                    SearchShipToByCustSaleIdCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchShipToByCustSaleIdCustom();


                        dataRecord.CustPartnerId = QueryUtils.getValueAsString(record, "CUST_PARTNER_ID");
                        dataRecord.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
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





        public async Task<EntitySearchResultBase<SearchCustomerSaleByCustCodeCustom>> searchCustomerSaleByCustCode(SearchCriteriaBase<SearchCustomerSaleByCustCodeCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchCustomerSaleByCustCodeCustom> searchResult = new EntitySearchResultBase<SearchCustomerSaleByCustCodeCustom>();
            List<SearchCustomerSaleByCustCodeCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchCustomerSaleByCustCodeCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select CS.CUST_SALE_ID,OG.ORG_NAME_TH,OC.CHANNEL_NAME_TH,OV.DIVISION_NAME_TH ");
                queryBuilder.AppendFormat(" ,OG.ORG_CODE,OC.CHANNEL_CODE,OV.DIVISION_CODE,CS.SHIPPING_COND ");
                queryBuilder.AppendFormat(" from CUSTOMER_SALE CS ");
                queryBuilder.AppendFormat(" inner join ORG_SALE_ORGANIZATION OG on OG.ORG_CODE = CS.ORG_CODE ");
                queryBuilder.AppendFormat(" inner join ORG_DIST_CHANNEL OC on OC.CHANNEL_CODE = CS.CHANNEL_CODE ");
                queryBuilder.AppendFormat(" inner join ORG_DIVISION OV on OV.DIVISION_CODE = CS.DIVISION_CODE ");
                queryBuilder.AppendFormat(" where CS.CUST_CODE = @CustCode ");
                QueryUtils.addParam(command, "CustCode", o.CustCode);



                // For Paging
                queryBuilder.AppendFormat(" ORDER BY ORG_NAME_TH  ");
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

                    List<SearchCustomerSaleByCustCodeCustom> dataRecordList = new List<SearchCustomerSaleByCustCodeCustom>();
                    SearchCustomerSaleByCustCodeCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchCustomerSaleByCustCodeCustom();


                        dataRecord.CustSaleId = QueryUtils.getValueAsString(record, "CUST_SALE_ID");
                        dataRecord.OrgNameTh = QueryUtils.getValueAsString(record, "ORG_NAME_TH");
                        dataRecord.ChannelNameTh = QueryUtils.getValueAsString(record, "CHANNEL_NAME_TH");
                        dataRecord.DivisionNameTh = QueryUtils.getValueAsString(record, "DIVISION_NAME_TH");
                        dataRecord.OrgCode = QueryUtils.getValueAsString(record, "ORG_CODE");
                        dataRecord.ChannelCode = QueryUtils.getValueAsString(record, "CHANNEL_CODE");
                        dataRecord.DivisionCode = QueryUtils.getValueAsString(record, "DIVISION_CODE");
                        dataRecord.ShippingCond = QueryUtils.getValueAsString(record, "SHIPPING_COND");

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


        public async Task<SaleOrder> createSaleOrder(CreateSaleOrderModel createSaleOrderModel, UserProfileForBack userProfile)
        {
            int ORDER_ID = -1;
            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        

                        var sqlParameters = new List<SqlParameter>();// Add New
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for SALE_ORDER_SEQ", p);
                        ORDER_ID = (int)p.Value;
                        string ORDER_STATUS_CREATE_ORDER = "1";
                        //string SOM_ORDER_NO = " 'SOM' + FORMAT(dbo.GET_SYSDATETIME(), 'yyyyMMddHHmmss') ";
                        //string SOM_ORDER_DTE = " dbo.GET_SYSDATETIME() ";
                        string SALE_REP = userProfile.getEmpId();
                        string GROUP_CODE = null;
                        if (userProfile.SaleGroupSaleOfficeCustom != null && userProfile.SaleGroupSaleOfficeCustom.data != null && userProfile.SaleGroupSaleOfficeCustom.data[0] != null)
                        {
                            GROUP_CODE = userProfile.SaleGroupSaleOfficeCustom.data[0].GroupCode;
                        }
                        string TERRITORY = userProfile.OrgTerritoryObject.TerritoryNameTh;
                        /*if (userProfile.OrgTerritory != null && userProfile.OrgTerritory.data != null)
                        {
                            List<string> territoryList = new List<string>();
                            foreach (OrgTerritory t in userProfile.OrgTerritory.data)
                            {
                                territoryList.Add(t.TerritoryNameTh);
                            }
                            TERRITORY = String.Join(",", territoryList);
                        }*/
                        string SALE_SUP = null;
                        if (userProfile.SaleGroupSaleOfficeCustom != null && userProfile.SaleGroupSaleOfficeCustom.data != null && userProfile.SaleGroupSaleOfficeCustom.data[0] != null)
                        {
                            SALE_SUP = userProfile.SaleGroupSaleOfficeCustom.data[0].ManagerEmpId?.ToString();
                        }
                        string ORDER_STATUS = ORDER_STATUS_CREATE_ORDER;


                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO SALE_ORDER (ORDER_ID, CUST_CODE, SOM_ORDER_NO, PO_NO, SOM_ORDER_DTE, DOC_TYPE_CODE, SHIP_TO_CUST_PARTNER_ID, SHIP_TO_CUST_CODE, DESCRIPTION, DELIVERY_DTE, SALE_REP, GROUP_CODE, TERRITORY, ORG_CODE, CHANNEL_CODE, DIVISION_CODE, CUST_SALE_ID, CONTACT_PERSON, REMARK, SALE_SUP, ORDER_STATUS, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@ORDER_ID ,@CUST_CODE,  'SOM' + FORMAT(dbo.GET_SYSDATETIME(), 'yyyyMMddHHmmss'), @PO_NO ,  dbo.GET_SYSDATETIME() , @DOC_TYPE_CODE, @SHIP_TO_CUST_PARTNER_ID, @SHIP_TO_CUST_CODE, @DESCRIPTION, @DELIVERY_DTE, @SALE_REP, @GROUP_CODE, @TERRITORY, @ORG_CODE, @CHANNEL_CODE, @DIVISION_CODE, @CUST_SALE_ID, @CONTACT_PERSON, @REMARK, @SALE_SUP, @ORDER_STATUS, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", ORDER_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CUST_CODE", createSaleOrderModel.SaleOrder.CustCode));// Add New
                        //sqlParameters.Add(QueryUtils.addSqlParameter("SOM_ORDER_NO", SOM_ORDER_NO));// Add New
                        //sqlParameters.Add(QueryUtils.addSqlParameter("SOM_ORDER_DTE", SOM_ORDER_DTE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PO_NO", createSaleOrderModel.SaleOrder.PoNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DOC_TYPE_CODE", createSaleOrderModel.SaleOrder.DocTypeCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SHIP_TO_CUST_PARTNER_ID", createSaleOrderModel.SaleOrder.ShipToCustPartnerId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SHIP_TO_CUST_CODE", createSaleOrderModel.SaleOrder.ShipToCustCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DESCRIPTION", createSaleOrderModel.SaleOrder.Description));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DELIVERY_DTE", createSaleOrderModel.SaleOrder.DeliveryDte));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SALE_REP", SALE_REP));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_CODE", GROUP_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TERRITORY", TERRITORY));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORG_CODE", createSaleOrderModel.SaleOrder.OrgCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CHANNEL_CODE", createSaleOrderModel.SaleOrder.ChannelCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DIVISION_CODE", createSaleOrderModel.SaleOrder.DivisionCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CUST_SALE_ID", createSaleOrderModel.SaleOrder.CustSaleId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CONTACT_PERSON", createSaleOrderModel.SaleOrder.ContactPerson));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("REMARK", createSaleOrderModel.SaleOrder.Remark));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SALE_SUP", SALE_SUP));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_STATUS", ORDER_STATUS));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        //SaleOrder re = new SaleOrder();
                        //re.CustSaleId = ORDER_ID;
                        //return re;



                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                SaleOrder re = await getSaleOrder(ORDER_ID);
                return re;
            }

        }
        private async Task<SaleOrder> getSaleOrder(int orderId)
        {
            if (orderId==-1)
            {
                return null;
            }

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from SALE_ORDER where ORDER_ID=@orderId ");
                sqlParameters.Add(QueryUtils.addSqlParameter("orderId", orderId));// Add New
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<SaleOrder> lst = context.SaleOrder.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                return lst.ElementAt(0);

            }

        }


        public async Task<int> cancelSaleOrder(CancelSaleOrderModel cancelSaleOrderModel, UserProfileForBack userProfile, string language, InterfaceSapConfig ic)
        {

            using (var context = new MyAppContext())
            {
                bool isNotRollback = false;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        int numberOfRowInserted=0;
                        if (!String.IsNullOrEmpty(cancelSaleOrderModel.SapOrderNo))
                        {
                            SaleOrder saleOrder = await getSaleOrderForCancelSaleOrder(cancelSaleOrderModel.OrderId);

                            using (var client = new HttpClient())
                            {
                                OutboundCancelSaleOrderCriteria reqData = new OutboundCancelSaleOrderCriteria();

                                reqData.Delete_Indicator = "X";
                                reqData.SAP_Sale_Order = saleOrder.SapOrderNo;
                                reqData.Sale_Org = saleOrder.OrgCode;
                                reqData.Distribution_Channel = saleOrder.ChannelCode;
                                reqData.Division = saleOrder.DivisionCode;
                                reqData.Sales_Order_Type = saleOrder.DocTypeCode;
                                reqData.SOM_Order_No = saleOrder.SomOrderNo;
                                string Document_DateStr = null;
                                if (saleOrder.SomOrderDte != null)
                                {
                                    string[] dateArr = saleOrder.SomOrderDte.ToString().Split(" ")[0].Split("/");
                                    Document_DateStr = dateArr[2] + dateArr[1].PadLeft(2, '0') + dateArr[0].PadLeft(2, '0');
                                }
                                reqData.Document_Date = Document_DateStr;

                                client.BaseAddress = new Uri(ic.InterfaceSapUrl + CommonConstant.API_OutboundCancelSaleOrder);
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{ic.InterfaceSapUser}:{ic.InterfaceSapPwd}")));
                                client.DefaultRequestHeaders.Accept.Clear();
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                client.DefaultRequestHeaders.Add("User-Agent", CommonConstant.USER_AGEN);
                                client.DefaultRequestHeaders.Add(CommonConstant.ReqKey, ic.ReqKey);
                                client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(CommonConstant.API_TIMEOUT));
                                string fullPath = ic.InterfaceSapUrl + CommonConstant.API_OutboundCancelSaleOrder;
                                var jsonVal = JsonConvert.SerializeObject(reqData);
                                var content = new StringContent(jsonVal, Encoding.UTF8, "application/json");
                                log.Debug("Call Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundCancelSaleOrder);
                                log.Debug("=========== jsonVal ================");
                                log.Debug("REQUEST:" + jsonVal);
                                log.Debug("REQUEST:" + JObject.Parse(jsonVal));

                                // Cal use time
                                // Create new stopwatch.
                                Stopwatch stopwatch = new Stopwatch();

                                // Begin timing.
                                stopwatch.Start();
                                //
                                HttpResponseMessage response = await client.PostAsync(fullPath, content);

                                // Stop timing.
                                stopwatch.Stop();

                                // Write result.
                                Console.WriteLine("Time elapsed: "+ stopwatch.Elapsed);
                                log.Debug("Time Use: "+ stopwatch.Elapsed);

                                CancelSaleOrderCustom resultOutbound = null;
                                if (response.IsSuccessStatusCode)
                                {
                                    resultOutbound = await response.Content.ReadAsAsync<CancelSaleOrderCustom>();

                                    //
                                    string ORDER_STATUS_CANCEL = SaleOrderStatus.ORDER_STATUS_CANCEL.ToString("d");
                                    string ORDER_ACTION_CANCEL = SaleOrderAction.ORDER_ACTION_CANCEL.ToString("d");
                                    var sqlParameters = new List<SqlParameter>();// Add New
                                    StringBuilder queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" UPDATE SALE_ORDER  ");
                                    queryBuilder.AppendFormat(" SET [ORDER_STATUS]=@ORDER_STATUS_CANCEL, [ORDER_ACTION]=@ORDER_ACTION_CANCEL, [SAP_STATUS]=@SO_Status, [SAP_MSG]=@SO_Message, [UPDATE_USER]=@USER, [UPDATE_DTM]= dbo.GET_SYSDATETIME() ");
                                    queryBuilder.AppendFormat(" WHERE [ORDER_ID]=@ORDER_ID ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getEmpId()));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_STATUS_CANCEL", ORDER_STATUS_CANCEL));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ACTION_CANCEL", ORDER_ACTION_CANCEL));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("SO_Status", resultOutbound.SO_Status));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("SO_Message", resultOutbound.SO_Message));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", cancelSaleOrderModel.OrderId));// Add New
                                    string queryStr = queryBuilder.ToString();
                                    log.Debug("Query:" + queryStr);
                                    Console.WriteLine("Query:" + queryStr);
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                                    //

                                    if (!"S".Equals(resultOutbound.SO_Status))
                                    {
                                        List<String> errorParam = new List<string>();
                                        errorParam.Add(resultOutbound.SO_Message);
                                        ServiceException se = new ServiceException("E_CUSTOM_MSG_ONLY", errorParam, ObjectFacory.getCultureInfo(language));
                                        transaction.Commit();
                                        isNotRollback = true;
                                        throw se;
                                    }

                                }
                                else
                                {
                                    string result_error = response.Content.ReadAsStringAsync().Result;
                                    Exception ex = new Exception(result_error);
                                    throw ex;
                                }

                                log.Debug("Response Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundCancelSaleOrder);
                                log.Debug("Response:" + JsonConvert.SerializeObject(resultOutbound));

                            }
                        }
                        /*string ORDER_STATUS_CANCEL = SaleOrderStatus.ORDER_STATUS_CANCEL.ToString("d");
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE SALE_ORDER SET ORDER_STATUS =@ORDER_STATUS_CANCEL, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE ORDER_ID=@ORDER_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getEmpId()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_STATUS_CANCEL", ORDER_STATUS_CANCEL));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", cancelSaleOrderModel.OrderId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        */

                        transaction.Commit();
                        return numberOfRowInserted;

                    }
                    catch (Exception ex)
                    {
                        if (!isNotRollback)
                        {
                            transaction.Rollback();
                        }
                        throw;
                    }
                }
            }

        }


        public async Task<SaleOrder> getSaleOrderForCancelSaleOrder(string orderId)
        {
            SaleOrder o = null;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select* from SALE_ORDER where ORDER_ID = @ORDER_ID ");
                QueryUtils.addParam(command, "ORDER_ID", orderId);// Add new

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        o = new SaleOrder();
                        o.OrderId = QueryUtils.getValueAsDecimalRequired(record, "ORDER_ID");
                        o.QuotationNo = QueryUtils.getValueAsString(record, "QUOTATION_NO");
                        o.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        o.SomOrderNo = QueryUtils.getValueAsString(record, "SOM_ORDER_NO");
                        o.SomOrderDte = QueryUtils.getValueAsDateTime(record, "SOM_ORDER_DTE");
                        o.SapOrderNo = QueryUtils.getValueAsString(record, "SAP_ORDER_NO");
                        o.SapOrderDte = QueryUtils.getValueAsDateTime(record, "SIMULATE_DTM");
                        o.PricingDtm = QueryUtils.getValueAsDateTime(record, "PRICING_DTM");
                        o.DocTypeCode = QueryUtils.getValueAsString(record, "DOC_TYPE_CODE");
                        o.ShipToCustPartnerId = QueryUtils.getValueAsDecimal(record, "SHIP_TO_CUST_PARTNER_ID");
                        o.Description = QueryUtils.getValueAsString(record, "DESCRIPTION");
                        o.DeliveryDte = QueryUtils.getValueAsDateTime(record, "DELIVERY_DTE");
                        o.SaleRep = QueryUtils.getValueAsString(record, "SALE_REP");
                        o.GroupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        o.OrgCode = QueryUtils.getValueAsString(record, "ORG_CODE");
                        o.ChannelCode = QueryUtils.getValueAsString(record, "CHANNEL_CODE");
                        o.DivisionCode = QueryUtils.getValueAsString(record, "DIVISION_CODE");
                        o.Territory = QueryUtils.getValueAsString(record, "TERRITORY");
                        o.ContactPerson = QueryUtils.getValueAsString(record, "CONTACT_PERSON");
                        o.Remark = QueryUtils.getValueAsString(record, "REMARK");
                        o.ReasonCode = QueryUtils.getValueAsString(record, "REASON_CODE");
                        o.ReasonReject = QueryUtils.getValueAsString(record, "REASON_REJECT");
                        o.NetValue = QueryUtils.getValueAsDecimal(record, "NET_VALUE");
                        o.Total = QueryUtils.getValueAsDecimal(record, "TOTAL");
                        o.PaymentTerm = QueryUtils.getValueAsString(record, "PAYMENT_TERM");
                        o.Incoterm = QueryUtils.getValueAsString(record, "INCOTERM");
                        o.PlantCode = QueryUtils.getValueAsString(record, "PLANT_CODE");
                        o.ShipCode = QueryUtils.getValueAsString(record, "SHIP_CODE");
                        o.SaleSup = QueryUtils.getValueAsString(record, "SALE_SUP");
                        o.OrderStatus = QueryUtils.getValueAsString(record, "ORDER_STATUS");
                        o.CreditStatus = QueryUtils.getValueAsString(record, "CREDIT_STATUS");
                        o.SapMsg = QueryUtils.getValueAsString(record, "SAP_MSG");
                        o.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        o.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        o.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        o.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");
                        o.SapStatus = QueryUtils.getValueAsString(record, "SAP_STATUS");
                        o.SapOrderDte = QueryUtils.getValueAsDateTime(record, "SAP_ORDER_DTE");
                        o.CustSaleId = QueryUtils.getValueAsDecimal(record, "CUST_SALE_ID");
                        o.ShipToCustCode = QueryUtils.getValueAsString(record, "SHIP_TO_CUST_CODE");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }





    }
}
