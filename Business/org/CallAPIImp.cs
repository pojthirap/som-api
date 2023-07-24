using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using NLog;
using System.Net.Http;
using MyFirstAzureWebApp.common;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using MyFirstAzureWebApp.exception;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.ModelCriteria.outbound;
using MyFirstAzureWebApp.Entity.custom;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Data;
using MyFirstAzureWebApp.Utils;
using MyFirstAzureWebApp.SearchCriteria.type1;
using MyFirstAzureWebApp.Models.profile;
using Microsoft.Data.SqlClient;
using MyFirstAzureWebApp.ModelCriteria.outbound.updatesaleorder;
using Condition = MyFirstAzureWebApp.Entity.custom.Condition;
using MyFirstAzureWebApp.SearchCriteria.type2;
using MyFirstAzureWebApp.enumval;
using MyFirstAzureWebApp.SearchCriteria.type3;
using MyFirstAzureWebApp.ModelCriteria.outbound.searchorderdocflow;
using System.Net;
using System.Xml;
using System.IO;
using MyFirstAzureWebApp.Controllers;

namespace MyFirstAzureWebApp.Business.org
{

    public class CallAPIImp : ICallAPI
    {
        private Logger log = LogManager.GetCurrentClassLogger();




        public async Task<EntitySearchResultBase<ZResultCustom>> Search(SearchCriteriaBase<SearchPlantByCompanyCodeCriteria> searchCriteria)
        {

            EntitySearchResultBase<ZResultCustom> searchResult = new EntitySearchResultBase<ZResultCustom>();
            List<ZResultCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchPlantByCompanyCodeCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from MS_PLANT_TEMP  ");
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.CompanyCode))
                    {
                        queryBuilder.AppendFormat(" where COMPANY_CODE  = @CompanyCode  ");
                        QueryUtils.addParam(command, "CompanyCode", o.CompanyCode);// Add new
                    }
                }

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY COMPANY_CODE  ");
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
                command.CommandText = queryBuilder.ToString();// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                using (var reader = command.ExecuteReader())
                {
                    //

                    List<ZResultCustom> dataRecordList = new List<ZResultCustom>();
                    ZResultCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new ZResultCustom();


                        dataRecord.COMPANY_CODE = QueryUtils.getValueAsString(record, "COMPANY_CODE");
                        dataRecord.PLANT_CODE = QueryUtils.getValueAsString(record, "PLANT_CODE");
                        dataRecord.PLANT_NAME_TH = QueryUtils.getValueAsString(record, "PLANT_NAME_TH");
                        dataRecord.PLANT_NAME_EN = QueryUtils.getValueAsString(record, "PLANT_NAME_EN");


                        dataRecord.CITY = QueryUtils.getValueAsString(record, "CITY");
                        dataRecord.PLANT_VENDOR_NO = QueryUtils.getValueAsString(record, "PLANT_VENDOR_NO");
                        dataRecord.PLANT_CUST_NO = QueryUtils.getValueAsString(record, "PLANT_CUST_NO");
                        dataRecord.PURCH_ORG = QueryUtils.getValueAsString(record, "PURCH_ORG");
                        dataRecord.FACT_CALENDAR = QueryUtils.getValueAsString(record, "FACT_CALENDAR");
                        dataRecord.BUSS_PLACE = QueryUtils.getValueAsString(record, "BUSS_PLACE");
                        dataRecord.ACTIVE_FLAG = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        dataRecord.CREATE_USER = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CREATE_DTM = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        dataRecord.UPDATE_USER = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UPDATE_DTM = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");

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



        public async Task<EntitySearchResultBase<SearchPlantByCompanyCodeCustom>> searchPlantByCompanyCode(SearchCriteriaBase<SearchPlantByCompanyCodeCriteria> searchCriteria, InterfaceSapConfig ic)
        {
            using (var client = new HttpClient())
            {
                SearchPlantByCompanyCodeCriteria criteria = searchCriteria.model;
                OutboundPlantInformationCriteria reqData = new OutboundPlantInformationCriteria();
                MyFirstAzureWebApp.SearchCriteria.Input input = new MyFirstAzureWebApp.SearchCriteria.Input();
                input.Interface_ID = "ZSOMI008";
                input.Table_Object = "V_T001W";
                reqData.Input = input;
                List<Company> companyList = new List<Company>();
                Company company = new Company();
                company.Company_Code = criteria.CompanyCode;
                companyList.Add(company);
                reqData.Company = companyList;

                client.BaseAddress = new Uri(ic.InterfaceSapUrl + CommonConstant.API_OutboundPlantInformation);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{ic.InterfaceSapUser}:{ic.InterfaceSapPwd}")));    
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", CommonConstant.USER_AGEN);
                client.DefaultRequestHeaders.Add(CommonConstant.ReqKey, ic.ReqKey);
                client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(CommonConstant.API_TIMEOUT));
                string fullPath = ic.InterfaceSapUrl + CommonConstant.API_OutboundPlantInformation;
                var jsonVal = JsonConvert.SerializeObject(reqData);
                var content = new StringContent(jsonVal, Encoding.UTF8, "application/json");
                log.Debug("Call Outbound Service:"+ ic.InterfaceSapUrl + CommonConstant.API_OutboundPlantInformation);
                log.Debug("=========== jsonVal ================");
                log.Debug("REQUEST:"+jsonVal);
                log.Debug("REQUEST:" + JObject.Parse(jsonVal));

                Console.WriteLine("Call Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundPlantInformation);
                Console.WriteLine("=========== jsonVal ================");
                Console.WriteLine("REQUEST:" + jsonVal);
                Console.WriteLine("REQUEST:" + JObject.Parse(jsonVal));

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
                Console.WriteLine("Time elapsed: " + stopwatch.Elapsed);
                log.Debug("Time Use: "+ stopwatch.Elapsed);

                SearchResultOutboundBas <SearchPlantByCompanyCodeCustom> resultOutbound = new SearchResultOutboundBas<SearchPlantByCompanyCodeCustom>();

                List<SearchPlantByCompanyCodeCustom> lst = new List<SearchPlantByCompanyCodeCustom>();
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    var dynamicData = JObject.Parse(result);
                    Console.WriteLine("Response Data");
                    Console.WriteLine(dynamicData);
                    log.Debug("Response Data");
                    log.Debug(dynamicData);

                    resultOutbound = await response.Content.ReadAsAsync<SearchResultOutboundBas<SearchPlantByCompanyCodeCustom>>();
                    lst = resultOutbound.Data;
                }
                else
                {
                    string result_error = response.Content.ReadAsStringAsync().Result;
                    Exception ex = new Exception(result_error);
                    throw ex;
                }

                log.Debug("Response Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundPlantInformation);
                log.Debug("Response:" + JsonConvert.SerializeObject(resultOutbound));

                EntitySearchResultBase<SearchPlantByCompanyCodeCustom> searchResult = new EntitySearchResultBase<SearchPlantByCompanyCodeCustom>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : lst.Count();
                searchResult.data = lst;
                return searchResult;



            }

        }





        public async Task<EntitySearchResultBase<SearchShippingPointByPlantCodeCustom>> searchShippingPointByPlantCode(SearchCriteriaBase<SearchShippingPointByPlantCodeCriteria> searchCriteria, InterfaceSapConfig ic)
        {
            using (var client = new HttpClient())
            {
                SearchShippingPointByPlantCodeCriteria criteria = searchCriteria.model;
                OutboundPlantToShippingPointsInformationCriteria reqData = new OutboundPlantToShippingPointsInformationCriteria();
                MyFirstAzureWebApp.SearchCriteria.Input input = new MyFirstAzureWebApp.SearchCriteria.Input();
                input.Interface_ID = "ZSOMI010";
                input.Table_Object = "TVSWZ";
                reqData.Input = input;
                List<Plant> plantList = new List<Plant>();
                Plant plant = new Plant();
                plant.Plant_Code = criteria.PlantCode;
                plantList.Add(plant);
                reqData.Plant = plantList;


                ;
                List<Shipping_ConditionsClass> shipping_ConditionsClassList = new List<Shipping_ConditionsClass>();
                Shipping_ConditionsClass shipping_ConditionsClass = new Shipping_ConditionsClass();
                shipping_ConditionsClass.Shipping_Conditions = criteria.ShippingConditions;
                shipping_ConditionsClassList.Add(shipping_ConditionsClass);
                reqData.Shipping_Conditions = shipping_ConditionsClassList;

                client.BaseAddress = new Uri(ic.InterfaceSapUrl + CommonConstant.API_OutboundPlantToShippingPointsInformation);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{ic.InterfaceSapUser}:{ic.InterfaceSapPwd}")));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", CommonConstant.USER_AGEN);
                client.DefaultRequestHeaders.Add(CommonConstant.ReqKey, ic.ReqKey);
                client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(CommonConstant.API_TIMEOUT));
                string fullPath = ic.InterfaceSapUrl + CommonConstant.API_OutboundPlantToShippingPointsInformation;
                var jsonVal = JsonConvert.SerializeObject(reqData);
                var content = new StringContent(jsonVal, Encoding.UTF8, "application/json");
                log.Debug("Call Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundPlantToShippingPointsInformation);
                log.Debug("=========== jsonVal ================");
                log.Debug("REQUEST:" + jsonVal);
                log.Debug("REQUEST:" + JObject.Parse(jsonVal));

                Console.WriteLine("Call Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundPlantToShippingPointsInformation);
                Console.WriteLine("=========== jsonVal ================");
                Console.WriteLine("REQUEST:" + jsonVal);
                Console.WriteLine("REQUEST:" + JObject.Parse(jsonVal));

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

                SearchResultOutboundBas <SearchShippingPointByPlantCodeCustom> resultOutbound = new SearchResultOutboundBas<SearchShippingPointByPlantCodeCustom>();

                List<SearchShippingPointByPlantCodeCustom> lst = new List<SearchShippingPointByPlantCodeCustom>();
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    var dynamicData = JObject.Parse(result);
                    Console.WriteLine("Response Data");
                    Console.WriteLine(dynamicData);
                    log.Debug("Response Data");
                    log.Debug(dynamicData);

                    resultOutbound = await response.Content.ReadAsAsync<SearchResultOutboundBas<SearchShippingPointByPlantCodeCustom>>();
                    lst = resultOutbound.Data;
                }
                else
                {
                    string result_error = response.Content.ReadAsStringAsync().Result;
                    Exception ex = new Exception(result_error);
                    throw ex;
                }

                log.Debug("Response Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundPlantToShippingPointsInformation);
                log.Debug("Response:" + JsonConvert.SerializeObject(resultOutbound));

                EntitySearchResultBase<SearchShippingPointByPlantCodeCustom> searchResult = new EntitySearchResultBase<SearchShippingPointByPlantCodeCustom>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : lst==null ? 0: lst.Count();
                searchResult.data = lst==null ? new List<SearchShippingPointByPlantCodeCustom>() : lst;
                return searchResult;



            }

        }






        public async Task<EntitySearchResultBase<SaleOrder>> createSaleOrderByQuotationNo(CreateSaleOrderByQuotationNoCriteria searchCriteria, UserProfileForBack userProfile, string language, InterfaceSapConfig ic)
        {
            using (var client = new HttpClient())
            {
                CreateSaleOrderByQuotationNoCriteria criteria = searchCriteria;
                OutboundCreateSaleOrderByQuotationNoCriteria reqData = new OutboundCreateSaleOrderByQuotationNoCriteria();
                reqData.Ref_Doc = criteria.QuotationNo;
                reqData.Doc_Date = DateTime.Now.ToString("yyyyMMdd");
                //reqData.Doc_Date = "20210731";//DateTime.Now.ToString("yyyyMMdd");

                client.BaseAddress = new Uri(ic.InterfaceSapUrl + CommonConstant.API_OutboundSaveSaleOrder);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{ic.InterfaceSapUser}:{ic.InterfaceSapPwd}")));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", CommonConstant.USER_AGEN);
                client.DefaultRequestHeaders.Add(CommonConstant.ReqKey, ic.ReqKey);
                client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(CommonConstant.API_TIMEOUT));
                string fullPath = ic.InterfaceSapUrl + CommonConstant.API_OutboundSaveSaleOrder;
                var jsonVal = JsonConvert.SerializeObject(reqData);
                var content = new StringContent(jsonVal, Encoding.UTF8, "application/json");
                log.Debug("Call Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundSaveSaleOrder);
                log.Debug("=========== jsonVal ================");
                log.Debug("REQUEST:" + jsonVal);
                log.Debug("REQUEST:" + JObject.Parse(jsonVal));

                Console.WriteLine("Call Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundSaveSaleOrder);
                Console.WriteLine("=========== jsonVal ================");
                Console.WriteLine("REQUEST:" + jsonVal);
                Console.WriteLine("REQUEST:" + JObject.Parse(jsonVal));

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

                SearchResultOutboundBasType1<CreateSaleOrderByQuotationNoCustom> resultOutbound = new SearchResultOutboundBasType1<CreateSaleOrderByQuotationNoCustom>();

                List<CreateSaleOrderByQuotationNoCustom> lst = new List<CreateSaleOrderByQuotationNoCustom>();
                List<SaleOrder> listInsert = new List<SaleOrder>();
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    var dynamicData = JObject.Parse(result);
                    Console.WriteLine("Response Data");
                    Console.WriteLine(dynamicData);
                    log.Debug("Response Data");
                    log.Debug(dynamicData);

                    resultOutbound = await response.Content.ReadAsAsync<SearchResultOutboundBasType1<CreateSaleOrderByQuotationNoCustom>>();
                    if (resultOutbound.Header != null)
                    {
                        lst = resultOutbound.Header.Item;
                    }

                    if (!"S".Equals(resultOutbound.Status.SO_Status))
                    {
                        List<String> errorParam = new List<string>();
                        errorParam.Add(resultOutbound.Status.SO_Msg);
                        ServiceException se = new ServiceException("E_CUSTOM_MSG_ONLY", errorParam, ObjectFacory.getCultureInfo(language));
                        throw se;
                    }

                    SaleOrder saleOrder = await InsertSaleOrderByQuotationNo(resultOutbound, userProfile);
                    saleOrder = await getSaleOrderByOrderId(saleOrder.OrderId);
                    listInsert.Add(saleOrder);

                }
                else
                {
                    string result_error = response.Content.ReadAsStringAsync().Result;
                    Exception ex = new Exception(result_error);
                    throw ex;
                }

                log.Debug("Response Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundSaveSaleOrder);
                log.Debug("Response:" + JsonConvert.SerializeObject(resultOutbound));

                EntitySearchResultBase<SaleOrder> searchResult = new EntitySearchResultBase<SaleOrder>();
                searchResult.totalRecords = listInsert.Count();
                searchResult.data = listInsert;
                return searchResult;



            }

        }

        public async Task<SaleOrder> getSaleOrderByOrderId(decimal orderId)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select * from SALE_ORDER where ORDER_ID = @orderId ");
                sqlParameters.Add(QueryUtils.addSqlParameter("orderId", orderId));// Add New
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<SaleOrder> lst = context.SaleOrder.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                return lst.ElementAt(0);

            }

        }

        public async Task<SaleOrder> InsertSaleOrderByQuotationNo(SearchResultOutboundBasType1<CreateSaleOrderByQuotationNoCustom> resultOutbound, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Header<CreateSaleOrderByQuotationNoCustom> Header = resultOutbound.Header;
                        Status Status = resultOutbound.Status;

                        var ORDER_STATUS_CREATE_ORDER = "1";
                        var sqlParameters = new List<SqlParameter>();// Add New
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for SALE_ORDER_SEQ", p);
                        var ORDER_ID = (int)p.Value;
                        string COMPANY_CODE = await GetCompanyCodeBySaleOrg(Header);
                        string CUST_CODE = Header.Sold_to;
                        string PO_NO = Header.PO_Number;
                        //string DOC_TYPE_CODE = Header.SO_Type;
                        string DOC_TYPE_CODE = "";// --FIX
                        string SHIP_TO_CUST_PARTNER_ID = await GetShipToCustPartnerId(Header);
                        SHIP_TO_CUST_PARTNER_ID = String.IsNullOrEmpty(SHIP_TO_CUST_PARTNER_ID) ? null : SHIP_TO_CUST_PARTNER_ID;
                        string SHIP_TO_CUST_CODE = Header.Ship_to;
                        string PLANT_CODE = null;
                        if(Header.Item!=null && Header.Item.Count != 0)
                        {
                            PLANT_CODE = Header.Item[0].Plant;
                        }
                        string SHIP_CODE = null;
                        if (Header.Item != null && Header.Item.Count != 0)
                        {
                            SHIP_CODE = Header.Item[0].Shipping_Point_Receiving_Pt;
                        }
                        string DELIVERY_DTE = Header.Delivery_date;
                        string SALE_REP = userProfile.getEmpId();
                        string GROUP_CODE = null;
                        if(userProfile.SaleGroupSaleOfficeCustom!=null && userProfile.SaleGroupSaleOfficeCustom.data!=null && userProfile.SaleGroupSaleOfficeCustom.data.Count != 0)
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
                        string ORG_CODE = Header.Sale_Org;
                        string CHANNEL_CODE = Header.Dist_Channel;
                        string DIVISION_CODE = Header.Division;
                        string CUST_SALE_ID = await GetCustSaleId(Header);
                        CUST_SALE_ID = String.IsNullOrEmpty(CUST_SALE_ID) ? null : CUST_SALE_ID;
                        string SALE_SUP = null;
                        if(userProfile.SaleGroupSaleOfficeCustom !=null && userProfile.SaleGroupSaleOfficeCustom.data != null && userProfile.SaleGroupSaleOfficeCustom.data.Count != 0)
                        {
                            SALE_SUP =  userProfile.SaleGroupSaleOfficeCustom.data[0].ManagerEmpId?.ToString();
                        }
                        string ORDER_STATUS = ORDER_STATUS_CREATE_ORDER;
                        string INCOTERM = Header.Inco1;
                        string NET_VALUE = Header.Sum_Net;
                        string TAX = Header.Sum_Tax;
                        string TOTAL = Header.Sum_Total;
                        string PAYMENT_TERM = Header.Payment_Term;
                        string QUOTATION_NO = Status.Quotation_No;
                        string SAP_MSG = Status.SO_Msg;
                        string SAP_STATUS = Status.SO_Status;
                        string CREATE_USER = userProfile.getUserName();
                        string UPDATE_USER = userProfile.getUserName();

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO  SALE_ORDER (ORDER_ID, CUST_CODE, COMPANY_CODE, SOM_ORDER_NO, PO_NO, SOM_ORDER_DTE, DOC_TYPE_CODE, SHIP_TO_CUST_PARTNER_ID, SHIP_TO_CUST_CODE, PLANT_CODE, SHIP_CODE, DELIVERY_DTE, SALE_REP, GROUP_CODE, TERRITORY, ORG_CODE, CHANNEL_CODE, DIVISION_CODE, CUST_SALE_ID, SALE_SUP, ORDER_STATUS, INCOTERM, SIMULATE_DTM, NET_VALUE, TAX, TOTAL, PAYMENT_TERM, QUOTATION_NO, SAP_MSG, SAP_STATUS, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@ORDER_ID ,@CUST_CODE, @COMPANY_CODE, 'SOM'+FORMAT (dbo.GET_SYSDATETIME(), 'yyyyMMddHHmmss'), @PO_NO , dbo.GET_SYSDATETIME() , @DOC_TYPE_CODE, @SHIP_TO_CUST_PARTNER_ID, @SHIP_TO_CUST_CODE, @PLANT_CODE, @SHIP_CODE, @DELIVERY_DTE, @SALE_REP, @GROUP_CODE, @TERRITORY, @ORG_CODE, @CHANNEL_CODE, @DIVISION_CODE, @CUST_SALE_ID, @SALE_SUP, @ORDER_STATUS, @INCOTERM,  dbo.GET_SYSDATETIME() , @NET_VALUE, @TAX, @TOTAL, @PAYMENT_TERM, @QUOTATION_NO, @SAP_MSG, @SAP_STATUS, @CREATE_USER , dbo.GET_SYSDATETIME() ,  @UPDATE_USER , dbo.GET_SYSDATETIME() )");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", ORDER_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CUST_CODE", CUST_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("COMPANY_CODE", COMPANY_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PO_NO", PO_NO));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DOC_TYPE_CODE", DOC_TYPE_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SHIP_TO_CUST_PARTNER_ID", SHIP_TO_CUST_PARTNER_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SHIP_TO_CUST_CODE", SHIP_TO_CUST_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLANT_CODE", PLANT_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SHIP_CODE", SHIP_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DELIVERY_DTE", DELIVERY_DTE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SALE_REP", SALE_REP));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_CODE", GROUP_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TERRITORY", TERRITORY));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORG_CODE", ORG_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CHANNEL_CODE", CHANNEL_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DIVISION_CODE", DIVISION_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CUST_SALE_ID", CUST_SALE_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SALE_SUP", SALE_SUP));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_STATUS", ORDER_STATUS));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("INCOTERM", INCOTERM));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("NET_VALUE", NET_VALUE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TAX", TAX));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TOTAL", TOTAL));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PAYMENT_TERM", PAYMENT_TERM));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("QUOTATION_NO", QUOTATION_NO));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SAP_MSG", SAP_MSG));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SAP_STATUS", SAP_STATUS));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CREATE_USER", CREATE_USER));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", UPDATE_USER));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                        Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        foreach (CreateSaleOrderByQuotationNoCustom data in Header.Item)
                        {
                            string SAP_ITEM_NO = data.Item_no;
                            string PROD_CODE = data.Material_Code;
                            string QTY = data.Order_Quantity;
                            string PROD_CONV_ID = await GetProdConvId(data);
                            string PROD_ALT_UNIT = data.Unit;

                            decimal Condition_Total_Value = 0;
                            if (!String.IsNullOrEmpty(data.Condition_Total_Value))
                            {
                                Condition_Total_Value = decimal.Parse(data.Condition_Total_Value);
                            }
                            decimal Condition_Amount = 0;
                            if (data.Condition!=null && data.Condition.Count!=0)
                            {
                                List<Condition> filteredList =  data.Condition.Where(o => o.Condition_type == "ZL34").ToList();
                                if(filteredList!=null && filteredList.Count != 0)
                                {
                                    foreach(Condition c in filteredList)
                                    {
                                        Condition_Amount += decimal.Parse(c.Condition_Amount.ToString());
                                    }
                                    
                                }
                            }

                            decimal NET_PRICE_EX = Condition_Total_Value - Condition_Amount;

                            Condition_Amount = 0;
                            if (data.Condition != null && data.Condition.Count != 0)
                            {
                                List<Condition> filteredList = data.Condition.Where(o => o.Condition_type == "ZL51").ToList();
                                if (filteredList != null && filteredList.Count != 0)
                                {
                                    foreach (Condition c in filteredList)
                                    {
                                        Condition_Amount += decimal.Parse(c.Condition_Amount.ToString());
                                    }

                                }
                            }
                            decimal TRANSFER_PRICE = Condition_Amount;
                            string NET_PRICE_INC = data.Condition_Total_Value;

                            Condition_Amount = 0;
                            if (data.Condition != null && data.Condition.Count != 0)
                            {
                                List<Condition> filteredList = data.Condition.Where(o => o.Condition_type == "ZL34").ToList();
                                if (filteredList != null && filteredList.Count != 0)
                                {
                                    foreach (Condition c in filteredList)
                                    {
                                        Condition_Amount += decimal.Parse(c.Condition_Amount.ToString());
                                    }

                                }
                            }
                            decimal ADDITIONAL_PRICE = Condition_Amount;

                            string NET_VALUE_ = data.Total_Value;
                            //string ITEM_TYPE = data.Item_Category;
                            string ITEM_TYPE = "";// --FIX
                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat("INSERT INTO  SALE_ORDER_PRODUCT (ORDER_PROD_ID, ORDER_ID, SAP_ITEM_NO, PROD_CODE, QTY, PROD_CONV_ID, PROD_ALT_UNIT, NET_PRICE_EX, TRANSFER_PRICE, NET_PRICE_INC, ADDITIONAL_PRICE, NET_VALUE, ITEM_TYPE, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                            queryBuilder.AppendFormat("VALUES(NEXT VALUE FOR SALE_ORDER_PRODUCT_SEQ , @ORDER_ID, @SAP_ITEM_NO, @PROD_CODE, @QTY, @PROD_CONV_ID, @PROD_ALT_UNIT, @NET_PRICE_EX, @TRANSFER_PRICE, @NET_PRICE_INC, @ADDITIONAL_PRICE, @NET_VALUE, @ITEM_TYPE, @CREATE_USER , dbo.GET_SYSDATETIME() ,  @UPDATE_USER , dbo.GET_SYSDATETIME() )");
                            sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", ORDER_ID));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("SAP_ITEM_NO", SAP_ITEM_NO));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PROD_CODE", PROD_CODE));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("QTY", QTY));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PROD_CONV_ID", PROD_CONV_ID));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PROD_ALT_UNIT", PROD_ALT_UNIT));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("NET_PRICE_EX", NET_PRICE_EX));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("TRANSFER_PRICE", TRANSFER_PRICE));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("NET_PRICE_INC", NET_PRICE_INC));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("ADDITIONAL_PRICE", ADDITIONAL_PRICE));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("NET_VALUE", NET_VALUE_));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("ITEM_TYPE", ITEM_TYPE));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("CREATE_USER", CREATE_USER));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", UPDATE_USER));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                            Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        }

                        transaction.Commit();
                        SaleOrder re = new SaleOrder();
                        re.OrderId = ORDER_ID;
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



        public async Task<String> GetCompanyCodeBySaleOrg(Header<CreateSaleOrderByQuotationNoCustom> Header)
        {
            String o = null;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select COMPANY_CODE ");
                queryBuilder.AppendFormat(" from ORG_SALE_ORGANIZATION ");
                queryBuilder.AppendFormat(" where ORG_CODE = @Sale_Org ");
                QueryUtils.addParam(command, "Sale_Org", Header.Sale_Org);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "COMPANY_CODE");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }


        public async Task<String> GetShipToCustPartnerId(Header<CreateSaleOrderByQuotationNoCustom> Header)
        {
            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select CUST_PARTNER_ID ");
                queryBuilder.AppendFormat(" from CUSTOMER_PARTNER ");
                queryBuilder.AppendFormat(" where CUST_CODE = @Sold_to ");
                queryBuilder.AppendFormat(" and ORG_CODE = @Sale_Org ");
                queryBuilder.AppendFormat(" and CHANNEL_CODE = @Dist_Channel ");
                queryBuilder.AppendFormat(" and DIVISION_CODE = @Division ");
                queryBuilder.AppendFormat(" and FUNC_CODE = 'SH' ");
                queryBuilder.AppendFormat(" and CUST_CODE_PARTNER = @Ship_to ");
                QueryUtils.addParam(command, "Sold_to", Header.Sold_to);// Add new
                QueryUtils.addParam(command, "Sale_Org", Header.Sale_Org);// Add new
                QueryUtils.addParam(command, "Dist_Channel", Header.Dist_Channel);// Add new
                QueryUtils.addParam(command, "Division", Header.Division);// Add new
                QueryUtils.addParam(command, "Ship_to", Header.Ship_to);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "CUST_PARTNER_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }

        public async Task<String> GetCustSaleId(Header<CreateSaleOrderByQuotationNoCustom> Header)
        {
            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select CUST_SALE_ID ");
                queryBuilder.AppendFormat(" from CUSTOMER_SALE ");
                queryBuilder.AppendFormat(" where CUST_CODE = @Sold_to ");
                queryBuilder.AppendFormat(" and ORG_CODE = @Sale_Org ");
                queryBuilder.AppendFormat(" and CHANNEL_CODE = @Dist_Channel ");
                queryBuilder.AppendFormat(" and DIVISION_CODE = @Division ");
                QueryUtils.addParam(command, "Sold_to", Header.Sold_to);// Add new
                QueryUtils.addParam(command, "Sale_Org", Header.Sale_Org);// Add new
                QueryUtils.addParam(command, "Dist_Channel", Header.Dist_Channel);// Add new
                QueryUtils.addParam(command, "Division", Header.Division);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "CUST_SALE_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }




        public async Task<String> GetProdConvId(CreateSaleOrderByQuotationNoCustom data)
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





        public async Task<StatusType2> updSaleOrder(UpdSaleOrderCriteria searchCriteria, UserProfileForBack userProfile, string language, InterfaceSapConfig ic, int timeZone)
        {

            StatusType2 SAP_STATUS = null;
            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    bool isNotRollback = false;
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO SALE_ORDER_CHANGE_LOG (LOG_ID, ORDER_ID, ORDER_SALE_REP, CHANGE_TAB_DESC, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(NEXT VALUE FOR SALE_ORDER_CHANGE_LOG_SEQ, @ORDER_ID ,@ORDER_SALE_REP, @CHANGE_TAB_DESC, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_SALE_REP", searchCriteria.SaleOrder.SaleRep));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CHANGE_TAB_DESC", searchCriteria.ChangeTabDesc));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);



                        var VAL_ORDER_STATUS = 2;

                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" UPDATE SALE_ORDER ");
                        queryBuilder.AppendFormat(" SET PO_NO=@PO_NO, INCOTERM=@INCOTERM, COMPANY_CODE=@COMPANY_CODE,  DESCRIPTION=@DESCRIPTION, DELIVERY_DTE=@DELIVERY_DTE, ORG_CODE=@ORG_CODE, CHANNEL_CODE=@CHANNEL_CODE, DIVISION_CODE=@DIVISION_CODE, PRICE_LIST=@PRICE_LIST, CUST_SALE_ID=@CUST_SALE_ID, SHIP_TO_CUST_PARTNER_ID=@SHIP_TO_CUST_PARTNER_ID, SHIP_TO_CUST_CODE=@SHIP_TO_CUST_CODE, CONTACT_PERSON=@CONTACT_PERSON, REMARK=@REMARK, REASON_CODE=@REASON_CODE, REASON_REJECT=@REASON_REJECT, PLANT_CODE=@PLANT_CODE, SHIP_CODE=@SHIP_CODE, ORDER_STATUS=@ORDER_STATUS, ORDER_ACTION=@ORDER_ACTION, [DOC_TYPE_CODE]=@DOC_TYPE_CODE, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME() ");
                        queryBuilder.AppendFormat(" WHERE ORDER_ID=@ORDER_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PO_NO", searchCriteria.SaleOrder.PoNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("INCOTERM", searchCriteria.SaleOrder.Incoterm));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("COMPANY_CODE", searchCriteria.SaleOrder.CompanyCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DESCRIPTION", searchCriteria.SaleOrder.Description));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DELIVERY_DTE", searchCriteria.SaleOrder.DeliveryDte));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORG_CODE", searchCriteria.SaleOrder.OrgCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CHANNEL_CODE", searchCriteria.SaleOrder.ChannelCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DIVISION_CODE", searchCriteria.SaleOrder.DivisionCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PRICE_LIST", searchCriteria.SaleOrder.PriceList));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CUST_SALE_ID", searchCriteria.SaleOrder.CustSaleId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SHIP_TO_CUST_PARTNER_ID", searchCriteria.SaleOrder.ShipToCustPartnerId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SHIP_TO_CUST_CODE", searchCriteria.SaleOrder.ShipToCustCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CONTACT_PERSON", searchCriteria.SaleOrder.ContactPerson));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("REMARK", searchCriteria.SaleOrder.Remark));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("REASON_CODE", searchCriteria.SaleOrder.ReasonCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("REASON_REJECT", searchCriteria.SaleOrder.ReasonReject));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLANT_CODE", searchCriteria.SaleOrder.PlantCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SHIP_CODE", searchCriteria.SaleOrder.ShipCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_STATUS", VAL_ORDER_STATUS));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ACTION", searchCriteria.TypeSubmit));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DOC_TYPE_CODE", searchCriteria.SaleOrder.DocTypeCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);



                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM SALE_ORDER_PRODUCT WHERE ORDER_ID=@ORDER_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);



                        int i = 1;
                        foreach(SaleOrderProductForUpdateSaleOrder product  in searchCriteria.Items)
                        {
                            int VAL_SAP_ITEM_NO = i * 10;

                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat("INSERT INTO SALE_ORDER_PRODUCT (ORDER_PROD_ID, ORDER_ID, SAP_ITEM_NO, PROD_CODE, PROD_CATE_CODE, QTY, PROD_CONV_ID, PROD_ALT_UNIT, ADDITIONAL_PRICE, ADDITIONAL_PER_UNIT, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                            queryBuilder.AppendFormat("VALUES(NEXT VALUE FOR SALE_ORDER_PRODUCT_SEQ, @ORDER_ID ,@SAP_ITEM_NO, @PROD_CODE, @PROD_CATE_CODE, @QTY, @PROD_CONV_ID, @PROD_ALT_UNIT, @ADDITIONAL_PRICE, @ADDITIONAL_PER_UNIT, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                            sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", product.OrderId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("SAP_ITEM_NO", VAL_SAP_ITEM_NO));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PROD_CODE", product.ProdCode));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PROD_CATE_CODE", product.ProdCateCode));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("QTY", product.Qty));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PROD_CONV_ID", product.ProdConvId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PROD_ALT_UNIT", product.ProdAltUnit));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("ADDITIONAL_PRICE", product.AdditionalPrice));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("ADDITIONAL_PER_UNIT", product.AdditionalPerUnit));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                            product.SapItemNo = VAL_SAP_ITEM_NO.ToString();
                            i++;
                        }




                       string TYPE_SUBMIT_ORDER = "1";
                       string TYPE_SUBMIT_SIMULATE = "2";
                       string TYPE_SUBMIT_CHANGE = "3";


                       string VAL_SAP_USER = null;


                        // Call API
                        using (var client = new HttpClient())
                        {
                            UpdSaleOrderCriteria criteria = searchCriteria;
                            OutboundUpdSaleOrderCriteria reqData = new OutboundUpdSaleOrderCriteria();
                            ModelCriteria.outbound.updatesaleorder.Header header = new ModelCriteria.outbound.updatesaleorder.Header();

                            header.Simulate_Indicator = "";
                            header.SAP_Sale_Order_No=searchCriteria.SaleOrder.SapOrderNo;
                            header.Sale_Org = searchCriteria.SaleOrder.OrgCode;
                            header.Distribution_Channel = searchCriteria.SaleOrder.ChannelCode;
                            header.Division = searchCriteria.SaleOrder.DivisionCode;
                            header.Sales_Order_Type = searchCriteria.SaleOrder.DocTypeCode;
                            header.SOM_order_no = searchCriteria.SaleOrder.SomOrderNo;
                            header.PO_Number = searchCriteria.SaleOrder.PoNo;
                            if (!String.IsNullOrEmpty(searchCriteria.SaleOrder.SomOrderDte))
                            {
                                string Document_dateStr = String.Join("", searchCriteria.SaleOrder.SomOrderDte.Split("T")[0].Split("-"));
                                header.Document_date = Document_dateStr;
                            }
                            header.Reference_Qoutation_Document = searchCriteria.SaleOrder.QuotationNo;
                            header.Sold_to_customer = searchCriteria.SaleOrder.CustCode;
                            header.Ship_to_customer = searchCriteria.SaleOrder.ShipToCustCode;
                            if (!String.IsNullOrEmpty(searchCriteria.SaleOrder.DeliveryDte))
                            {
                                string Request_Delivery_dateStr = String.Join("", searchCriteria.SaleOrder.DeliveryDte.Split("T")[0].Split("-"));
                                header.Request_Delivery_date = Request_Delivery_dateStr;
                            }
                            header.Incoterms1 = searchCriteria.SaleOrder.Incoterm;
                            header.Incoterms2 = searchCriteria.SaleOrder.Incoterm;

                            header.SO_Cancel_Indicator = "";

                            //
                            if (!String.IsNullOrEmpty(searchCriteria.SaleOrder.PriceDate))
                            {
                                header.Price_Date = searchCriteria.SaleOrder.PriceDate;
                                header.Price_Time = searchCriteria.SaleOrder.PriceTime;
                            }
                            else
                            {
                                header.Price_Date = DateTime.Now.ToString("yyyyMMdd");
                                header.Price_Time = DateTime.Now.AddHours(timeZone).ToString("HHmmss");
                            }
                            //

                            header.Price_List = searchCriteria.SaleOrder.PriceList;
                            header.Reason_For_Rejection = searchCriteria.SaleOrder.ReasonReject;
                            header.Order_Reason = searchCriteria.SaleOrder.ReasonCode;

                            List<Item> ItemList=null;
                            Item item = null;
                            if (searchCriteria.Items!=null && searchCriteria.Items.Count != 0)
                            {
                                ItemList = new List<Item>();
                                foreach(SaleOrderProductForUpdateSaleOrder product in searchCriteria.Items)
                                {
                                    item = new Item();
                                    item.Item_no = product.SapItemNo;
                                    item.Material_Code = product.ProdCode;
                                    //item.Item_Category = product.ItemType;
                                    item.Item_Category = ""; //--Fix
                                    item.Plant = product.PlantCode;
                                    item.Shipping_Point_Receiving_Pt = product.ShipCode;
                                    item.Storage_Location="";
                                    item.Order_Quantity = product.Qty;
                                    item.Unit = product.ProdAltUnit;
                                    item.Price_per_Unit="";
                                    if (!String.IsNullOrEmpty(product.AdditionalPrice))
                                    {
                                        List<ModelCriteria.outbound.updatesaleorder.Condition> conditionList = null;
                                        ModelCriteria.outbound.updatesaleorder.Condition condition_ = null;
                                        if (Convert.ToDecimal(product.AdditionalPrice) > 0)
                                        {
                                            conditionList = new List<ModelCriteria.outbound.updatesaleorder.Condition>();
                                            condition_ = new ModelCriteria.outbound.updatesaleorder.Condition();
                                            condition_.Condition_type = "ZL34";
                                            //condition_.Condition_Amount = Convert.ToDecimal(product.AdditionalPrice);
                                            condition_.Condition_Amount = product.AdditionalPrice;
                                            condition_.Condition_Per = product.AdditionalPerUnit;
                                            condition_.Condition_Unit = product.ProdAltUnit;
                                            conditionList.Add(condition_);
                                            item.Condition = conditionList;
                                        }
                                        else
                                        {

                                            conditionList = new List<ModelCriteria.outbound.updatesaleorder.Condition>();
                                            condition_ = new ModelCriteria.outbound.updatesaleorder.Condition();
                                            condition_.Condition_type = "";
                                            condition_.Condition_Amount = "";
                                            condition_.Condition_Per = "";
                                            condition_.Condition_Unit = "";
                                            conditionList.Add(condition_);
                                            item.Condition = conditionList;
                                        }
                                    }

                                    ItemList.Add(item);
                                }
                            }

                            header.Item = ItemList;
                            reqData.Header = header;


                            string urlService = "";
                            if (searchCriteria.TypeSubmit.Equals(TYPE_SUBMIT_ORDER))
                            {
                                VAL_SAP_USER = userProfile.getUserName();
                                urlService = ic.InterfaceSapUrl + CommonConstant.API_OutboundSAP_ZSOMI014;
                            }
                            else if (searchCriteria.TypeSubmit.Equals(TYPE_SUBMIT_SIMULATE))
                            {
                                header.Simulate_Indicator = "X";
                                VAL_SAP_USER = userProfile.getUserName();
                                urlService = ic.InterfaceSapUrl + CommonConstant.API_OutboundSAP_ZSOMI015;
                            }
                            else if (searchCriteria.TypeSubmit.Equals(TYPE_SUBMIT_CHANGE))
                            {
                                VAL_SAP_USER = userProfile.getUserName();
                                urlService = ic.InterfaceSapUrl + CommonConstant.API_OutboundSAP_ZSOMI021;
                            }

                            client.BaseAddress = new Uri(urlService);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{ic.InterfaceSapUser}:{ic.InterfaceSapPwd}")));
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Add("User-Agent", CommonConstant.USER_AGEN);
                            client.DefaultRequestHeaders.Add(CommonConstant.ReqKey, ic.ReqKey);
                            client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(CommonConstant.API_TIMEOUT));
                            string fullPath = urlService;
                            var jsonVal = JsonConvert.SerializeObject(reqData);
                            var content = new StringContent(jsonVal, Encoding.UTF8, "application/json");
                            log.Debug("Call Outbound Service:" + urlService);
                            log.Debug("=========== jsonVal ================");
                            log.Debug("REQUEST:" + jsonVal);
                            log.Debug("REQUEST:" + JObject.Parse(jsonVal));

                            Console.WriteLine("Call Outbound Service:" + urlService);
                            Console.WriteLine("=========== jsonVal ================");
                            Console.WriteLine("REQUEST:" + jsonVal);
                            Console.WriteLine("REQUEST:" + JObject.Parse(jsonVal));

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

                            SearchResultOutboundBasType2<ItemType2> resultOutbound = new SearchResultOutboundBasType2<ItemType2>();

                            List<ItemType2> lst = new List<ItemType2>();
                            if (response.IsSuccessStatusCode)
                            {
                                string result = response.Content.ReadAsStringAsync().Result;
                                var dynamicData = JObject.Parse(result);
                                Console.WriteLine("Response Data");
                                Console.WriteLine(dynamicData);
                                log.Debug("Response Data");
                                log.Debug(dynamicData);

                                resultOutbound = await response.Content.ReadAsAsync<SearchResultOutboundBasType2<ItemType2>>();
                                if (resultOutbound.Header != null)
                                {
                                    lst = resultOutbound.Header.Item;
                                }
                                SAP_STATUS = resultOutbound.Status;
                                StatusType2 Status = resultOutbound.Status;
                                HeaderType2<ItemType2> Header = resultOutbound.Header;
                                decimal VAL_ORDER_LOG_ID = await getNextValueSALE_ORDER_LOG_SEQ();
                                string VAL_ORDER_LOG_ID_OLD = await GetOrderLogIdOld(searchCriteria);

                                if (!"S".Equals(resultOutbound.Status.SO_Status))
                                {
                                    VAL_ORDER_STATUS = (int)SaleOrderStatus.ORDER_STATUS_FAIL;
                                    sqlParameters = new List<SqlParameter>();// Add New
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat(" UPDATE SALE_ORDER   ");
                                    queryBuilder.AppendFormat(" SET [CREDIT_STATUS]=@Credit_Status, [SAP_MSG]=@SO_Message, [SAP_STATUS]=@SO_Status, [ORDER_STATUS]=@ORDER_STATUS, [UPDATE_USER]=@UPDATE_USER, [UPDATE_DTM]=dbo.GET_SYSDATETIME()  ");
                                    queryBuilder.AppendFormat(" WHERE [ORDER_ID]=@ORDER_ID   ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("Credit_Status", Status.Credit_Status));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("SO_Message", Status.SO_Message));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("SO_Status", Status.SO_Status));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_STATUS", VAL_ORDER_STATUS));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", VAL_SAP_USER));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                    Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                                    
                                    //transaction.Commit();
                                    //isNotRollback = true;

                                    /*List<String> errorParam = new List<string>();
                                    errorParam.Add(resultOutbound.Status.SO_Message);
                                    ServiceException se = new ServiceException("E_CUSTOM_MSG_ONLY", errorParam, ObjectFacory.getCultureInfo(language));
                                    throw se;*/

                                }else
                                {
                                    // Update Data
                                    if (searchCriteria.TypeSubmit.Equals(TYPE_SUBMIT_SIMULATE))
                                    {
                                        sqlParameters = new List<SqlParameter>();// Add New
                                        queryBuilder = new StringBuilder();
                                        queryBuilder.AppendFormat(" UPDATE SALE_ORDER SET SAP_ORDER_NO=@SAP_ORDER_NO, SAP_ORDER_DTE=@SAP_ORDER_DTE, NET_VALUE=@NET_VALUE, TAX=@TAX, TOTAL=@TOTAL, PAYMENT_TERM=@PAYMENT_TERM, INCOTERM=@INCOTERM, SIMULATE_DTM=dbo.GET_SYSDATETIME(), CREDIT_STATUS=@CREDIT_STATUS, SAP_MSG=@SAP_MSG, SAP_STATUS=@SAP_STATUS, ORDER_STATUS=@ORDER_STATUS, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME() ");
                                        queryBuilder.AppendFormat(" WHERE ORDER_ID=@ORDER_ID  ");
                                        sqlParameters.Add(QueryUtils.addSqlParameter("SAP_ORDER_NO", Status.SAP_Sale_Order_No));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("SAP_ORDER_DTE", /*Header.Document_date*/searchCriteria.SaleOrder.SomOrderDte));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("NET_VALUE", Header.Sum_Net_Price_by_SO));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("TAX", Header.Tax_Amount_by_SO));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("TOTAL", Header.SUM_Total_NetADDVat));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("PAYMENT_TERM", Header.Payment_Term));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("INCOTERM", Header.Incoterms1));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("CREDIT_STATUS", Status.Credit_Status));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("SAP_MSG", Status.SO_Message));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("SAP_STATUS", Status.SO_Status));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_STATUS", VAL_ORDER_STATUS));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", VAL_SAP_USER));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                                        log.Debug("Query:" + queryBuilder.ToString());
                                        Console.WriteLine("Query:" + queryBuilder.ToString());
                                        log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                        Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                    }
                                    else
                                    {
                                        sqlParameters = new List<SqlParameter>();// Add New
                                        queryBuilder = new StringBuilder();
                                        queryBuilder.AppendFormat(" UPDATE SALE_ORDER SET SAP_ORDER_NO=@SAP_ORDER_NO, SAP_ORDER_DTE=@SAP_ORDER_DTE, NET_VALUE=@NET_VALUE, TAX=@TAX, TOTAL=@TOTAL, PAYMENT_TERM=@PAYMENT_TERM, INCOTERM=@INCOTERM, PRICING_DTM=cast(@PRICE_DATE+ ' ' + substring(@PRICE_TIME, 1, 2)+ ':' + substring(@PRICE_TIME, 3, 2)+ ':' + substring(@PRICE_TIME, 5, 2) as datetime), CREDIT_STATUS=@CREDIT_STATUS, SAP_MSG=@SAP_MSG, SAP_STATUS=@SAP_STATUS, ORDER_STATUS=@ORDER_STATUS, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME() ");
                                        queryBuilder.AppendFormat(" WHERE ORDER_ID=@ORDER_ID  ");
                                        sqlParameters.Add(QueryUtils.addSqlParameter("SAP_ORDER_NO", Status.SAP_Sale_Order_No));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("SAP_ORDER_DTE", Header.Document_date));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("NET_VALUE", Header.Sum_Net_Price_by_SO));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("TAX", Header.Tax_Amount_by_SO));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("TOTAL", Header.SUM_Total_NetADDVat));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("PAYMENT_TERM", Header.Payment_Term));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("INCOTERM", Header.Incoterms1));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("CREDIT_STATUS", Status.Credit_Status));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("PRICE_DATE", Header.Price_Date));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("PRICE_TIME", Header.Price_Time));// Add New
                                        
                                        sqlParameters.Add(QueryUtils.addSqlParameter("SAP_MSG", Status.SO_Message));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("SAP_STATUS", Status.SO_Status));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_STATUS", VAL_ORDER_STATUS));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", VAL_SAP_USER));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                                        log.Debug("Query:" + queryBuilder.ToString());
                                        Console.WriteLine("Query:" + queryBuilder.ToString());
                                        log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                        Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                                    }


                                    if (lst != null && lst.Count != 0)
                                    {
                                        foreach (ItemType2 data in lst)
                                        {
                                            decimal Condition_Total_Value = String.IsNullOrEmpty(data.Condition_Total_Value) ? 0 : Convert.ToDecimal(data.Condition_Total_Value);
                                            decimal Condition_Amount = 0;
                                            decimal Condition_Per = 0;
                                            if (data.Condition != null && data.Condition.Count != 0)
                                            {
                                                List<ConditionType2> filteredList = data.Condition.Where(o => o.Condition_type == "ZL34").ToList();
                                                if (filteredList != null && filteredList.Count != 0)
                                                {
                                                    foreach (ConditionType2 c in filteredList)
                                                    {
                                                        Condition_Amount += decimal.Parse(c.Condition_Amount.ToString());
                                                        Condition_Per += decimal.Parse(c.Condition_Per.ToString());
                                                    }

                                                }
                                            }
                                            decimal VAL_NET_PRICE_EX = 0;
                                            if (Condition_Per == 0)
                                            {
                                                VAL_NET_PRICE_EX = Condition_Total_Value - (Condition_Amount);
                                            }
                                            else
                                            {
                                                VAL_NET_PRICE_EX = Condition_Total_Value - (Condition_Amount / Condition_Per);
                                            }
                                            ///

                                            Condition_Amount = 0;
                                            if (data.Condition != null && data.Condition.Count != 0)
                                            {
                                                List<ConditionType2> filteredList = data.Condition.Where(o => o.Condition_type == "ZL51").ToList();
                                                if (filteredList != null && filteredList.Count != 0)
                                                {
                                                    foreach (ConditionType2 c in filteredList)
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
                                                List<ConditionType2> filteredList = data.Condition.Where(o => o.Condition_type == "ZL34").ToList();
                                                if (filteredList != null && filteredList.Count != 0)
                                                {
                                                    foreach (ConditionType2 c in filteredList)
                                                    {
                                                        Condition_Amount += decimal.Parse(c.Condition_Amount.ToString());
                                                    }

                                                }
                                            }
                                            decimal VAL_ADDITIONAL_PRICE = Condition_Amount;

                                            decimal VAL_NET_VALUE = String.IsNullOrEmpty(data.Total_Value) ? 0 : Convert.ToDecimal(data.Total_Value);
                                            string VAL_ITEM_TYPE = data.Item_Category;
                                            decimal VAL_ITEM_NO = String.IsNullOrEmpty(data.Item_no) ? 0 : Convert.ToDecimal(data.Item_no);


                                            string VAL_PROD_CODE = data.Material_Code;
                                            string VAL_PROD_ALT_UNIT = data.Unit;
                                            ///


                                            sqlParameters = new List<SqlParameter>();// Add New
                                            queryBuilder = new StringBuilder();
                                            queryBuilder.AppendFormat(" UPDATE SALE_ORDER_PRODUCT   ");
                                            queryBuilder.AppendFormat(" SET NET_PRICE_EX=@NET_PRICE_EX, TRANSFER_PRICE=@TRANSFER_PRICE, NET_PRICE_INC=@NET_PRICE_INC, ADDITIONAL_PRICE=@ADDITIONAL_PRICE, NET_VALUE=@NET_VALUE, ITEM_TYPE=@ITEM_TYPE, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  ");
                                            queryBuilder.AppendFormat(" WHERE ORDER_ID=@ORDER_ID  ");
                                            queryBuilder.AppendFormat(" and PROD_CODE = @VAL_PROD_CODE ");
                                            queryBuilder.AppendFormat(" and PROD_ALT_UNIT = @VAL_PROD_ALT_UNIT ");
                                            //queryBuilder.AppendFormat(" and SAP_ITEM_NO=@ITEM_NO   ");
                                            sqlParameters.Add(QueryUtils.addSqlParameter("NET_PRICE_EX", VAL_NET_PRICE_EX));// Add New
                                            sqlParameters.Add(QueryUtils.addSqlParameter("TRANSFER_PRICE", VAL_TRANSFER_PRICE));// Add New
                                            sqlParameters.Add(QueryUtils.addSqlParameter("NET_PRICE_INC", VAL_NET_PRICE_INC));// Add New
                                            sqlParameters.Add(QueryUtils.addSqlParameter("ADDITIONAL_PRICE", VAL_ADDITIONAL_PRICE));// Add New
                                            sqlParameters.Add(QueryUtils.addSqlParameter("NET_VALUE", VAL_NET_VALUE));// Add New
                                            sqlParameters.Add(QueryUtils.addSqlParameter("ITEM_TYPE", VAL_ITEM_TYPE));// Add New
                                            sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", VAL_SAP_USER));// Add New
                                            sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                                            //sqlParameters.Add(QueryUtils.addSqlParameter("ITEM_NO", VAL_ITEM_NO));// Add New
                                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PROD_CODE", VAL_PROD_CODE));// Add New
                                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PROD_ALT_UNIT", VAL_PROD_ALT_UNIT));// Add New
                                            log.Debug("Query:" + queryBuilder.ToString());
                                            Console.WriteLine("Query:" + queryBuilder.ToString());
                                            log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                            Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                        }
                                    }

                                    // Update Data
                                }



                                //Insert SALE_ORDER_LOG
                                sqlParameters = new List<SqlParameter>();// Add New
                                queryBuilder = new StringBuilder();
                                queryBuilder.AppendFormat(" INSERT INTO SALE_ORDER_LOG ([ORDER_LOG_ID], [ORDER_ID], [QUOTATION_NO], [CUST_CODE], [SOM_ORDER_NO], [PO_NO], [SOM_ORDER_DTE], [SAP_ORDER_NO], [SIMULATE_DTM], [PRICING_DTM], [DOC_TYPE_CODE], [SHIP_TO_CUST_PARTNER_ID], [DESCRIPTION], [DELIVERY_DTE], [SALE_REP], [GROUP_CODE], [ORG_CODE], [CHANNEL_CODE], [DIVISION_CODE], [PRICE_LIST], [TERRITORY], [CONTACT_PERSON], [REMARK], [REASON_CODE], [REASON_REJECT], [NET_VALUE], [TAX], [TOTAL], [PAYMENT_TERM], [INCOTERM], [PLANT_CODE], [SHIP_CODE], [SALE_SUP], [ORDER_STATUS], [ORDER_ACTION], [CREDIT_STATUS], [SAP_MSG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_STATUS], [SAP_ORDER_DTE], [CUST_SALE_ID], [SHIP_TO_CUST_CODE], [COMPANY_CODE])  ");
                                queryBuilder.AppendFormat(" SELECT @VAL_ORDER_LOG_ID, [ORDER_ID], [QUOTATION_NO], [CUST_CODE], [SOM_ORDER_NO], [PO_NO], [SOM_ORDER_DTE], [SAP_ORDER_NO], [SIMULATE_DTM], [PRICING_DTM], [DOC_TYPE_CODE], [SHIP_TO_CUST_PARTNER_ID], [DESCRIPTION], [DELIVERY_DTE], [SALE_REP], [GROUP_CODE], [ORG_CODE], [CHANNEL_CODE], [DIVISION_CODE], [PRICE_LIST], [TERRITORY], [CONTACT_PERSON], [REMARK], [REASON_CODE], [REASON_REJECT], [NET_VALUE], [TAX], [TOTAL], [PAYMENT_TERM], [INCOTERM], [PLANT_CODE], [SHIP_CODE], [SALE_SUP], [ORDER_STATUS], [ORDER_ACTION], [CREDIT_STATUS], [SAP_MSG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_STATUS], [SAP_ORDER_DTE], [CUST_SALE_ID], [SHIP_TO_CUST_CODE], [COMPANY_CODE] FROM SALE_ORDER WHERE [ORDER_ID] = @ORDER_ID ");
                                sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ORDER_LOG_ID", VAL_ORDER_LOG_ID));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                                log.Debug("Query:" + queryBuilder.ToString());
                                Console.WriteLine("Query:" + queryBuilder.ToString());
                                log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                                //Insert SALE_ORDER_PRODUCT_LOG
                                sqlParameters = new List<SqlParameter>();// Add New
                                queryBuilder = new StringBuilder();
                                queryBuilder.AppendFormat(" INSERT INTO SALE_ORDER_PRODUCT_LOG ([ORDER_PROD_LOG_ID], [ORDER_LOG_ID], [PROD_CODE], [QTY], [PROD_CONV_ID], [NET_PRICE_EX], [TRANSFER_PRICE], [NET_PRICE_INC], [ADDITIONAL_PRICE], [ADDITIONAL_PER_UNIT], [ITEM_TYPE], [NET_VALUE], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_ITEM_NO], [PROD_ALT_UNIT], [PROD_CATE_CODE])  ");
                                queryBuilder.AppendFormat(" SELECT NEXT VALUE FOR SALE_ORDER_PRODUCT_LOG_SEQ, @VAL_ORDER_LOG_ID, [PROD_CODE], [QTY], [PROD_CONV_ID], [NET_PRICE_EX], [TRANSFER_PRICE], [NET_PRICE_INC], [ADDITIONAL_PRICE], [ADDITIONAL_PER_UNIT], [ITEM_TYPE], [NET_VALUE], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_ITEM_NO], [PROD_ALT_UNIT], [PROD_CATE_CODE] FROM SALE_ORDER_PRODUCT WHERE [ORDER_ID] = @ORDER_ID ");
                                sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ORDER_LOG_ID", VAL_ORDER_LOG_ID));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                                log.Debug("Query:" + queryBuilder.ToString());
                                Console.WriteLine("Query:" + queryBuilder.ToString());
                                log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);



                                if (!"S".Equals(resultOutbound.Status.SO_Status))
                                {
                                    if (!String.IsNullOrEmpty(VAL_ORDER_LOG_ID_OLD))
                                    {

                                        //

                                        // Update SALE_ORDER 
                                        sqlParameters = new List<SqlParameter>();// Add New
                                        queryBuilder = new StringBuilder();

                                        queryBuilder.AppendFormat("  UPDATE O ");
                                        queryBuilder.AppendFormat(" SET  ");
                                        queryBuilder.AppendFormat("     O.ORDER_ID=L.ORDER_ID , ");
                                        queryBuilder.AppendFormat("     O.QUOTATION_NO=L.QUOTATION_NO , ");
                                        queryBuilder.AppendFormat("     O.CUST_CODE=L.CUST_CODE , ");
                                        queryBuilder.AppendFormat("     O.SOM_ORDER_NO=L.SOM_ORDER_NO , ");
                                        queryBuilder.AppendFormat("     O.PO_NO=L.PO_NO , ");
                                        queryBuilder.AppendFormat("     O.SOM_ORDER_DTE=L.SOM_ORDER_DTE , ");
                                        queryBuilder.AppendFormat("     O.SAP_ORDER_NO=L.SAP_ORDER_NO , ");
                                        queryBuilder.AppendFormat("     O.SIMULATE_DTM=L.SIMULATE_DTM , ");
                                        queryBuilder.AppendFormat("     O.PRICING_DTM=L.PRICING_DTM , ");
                                        queryBuilder.AppendFormat("     O.DOC_TYPE_CODE=L.DOC_TYPE_CODE , ");
                                        queryBuilder.AppendFormat("     O.SHIP_TO_CUST_PARTNER_ID=L.SHIP_TO_CUST_PARTNER_ID , ");
                                        queryBuilder.AppendFormat("     O.DESCRIPTION=L.DESCRIPTION , ");
                                        queryBuilder.AppendFormat("     O.DELIVERY_DTE=L.DELIVERY_DTE , ");
                                        queryBuilder.AppendFormat("     O.SALE_REP=L.SALE_REP , ");
                                        queryBuilder.AppendFormat("     O.GROUP_CODE=L.GROUP_CODE , ");
                                        queryBuilder.AppendFormat("     O.ORG_CODE=L.ORG_CODE , ");
                                        queryBuilder.AppendFormat("     O.CHANNEL_CODE=L.CHANNEL_CODE , ");
                                        queryBuilder.AppendFormat("     O.DIVISION_CODE=L.DIVISION_CODE , ");
                                        queryBuilder.AppendFormat("     O.TERRITORY=L.TERRITORY , ");
                                        queryBuilder.AppendFormat("     O.CONTACT_PERSON=L.CONTACT_PERSON , ");
                                        queryBuilder.AppendFormat("     O.REMARK=L.REMARK , ");
                                        queryBuilder.AppendFormat("     O.REASON_CODE=L.REASON_CODE , ");
                                        queryBuilder.AppendFormat("     O.REASON_REJECT=L.REASON_REJECT , ");
                                        queryBuilder.AppendFormat("     O.NET_VALUE=L.NET_VALUE , ");
                                        queryBuilder.AppendFormat("     O.TAX=L.TAX , ");
                                        queryBuilder.AppendFormat("     O.TOTAL=L.TOTAL , ");
                                        queryBuilder.AppendFormat("     O.PAYMENT_TERM=L.PAYMENT_TERM , ");
                                        queryBuilder.AppendFormat("     O.INCOTERM=L.INCOTERM , ");
                                        queryBuilder.AppendFormat("     O.PLANT_CODE=L.PLANT_CODE , ");
                                        queryBuilder.AppendFormat("     O.SHIP_CODE=L.SHIP_CODE , ");
                                        queryBuilder.AppendFormat("     O.SALE_SUP=L.SALE_SUP , ");
                                        queryBuilder.AppendFormat("     O.ORDER_STATUS=L.ORDER_STATUS , ");
                                        queryBuilder.AppendFormat("     O.CREDIT_STATUS=L.CREDIT_STATUS , ");
                                        queryBuilder.AppendFormat("     O.SAP_MSG=L.SAP_MSG , ");
                                        queryBuilder.AppendFormat("     O.CREATE_USER=L.CREATE_USER , ");
                                        queryBuilder.AppendFormat("     O.CREATE_DTM=L.CREATE_DTM , ");
                                        queryBuilder.AppendFormat("     O.UPDATE_USER=L.UPDATE_USER , ");
                                        queryBuilder.AppendFormat("     O.UPDATE_DTM=L.UPDATE_DTM , ");
                                        queryBuilder.AppendFormat("     O.SAP_STATUS=L.SAP_STATUS , ");
                                        queryBuilder.AppendFormat("     O.SAP_ORDER_DTE=L.SAP_ORDER_DTE , ");
                                        queryBuilder.AppendFormat("     O.CUST_SALE_ID=L.CUST_SALE_ID , ");
                                        queryBuilder.AppendFormat("     O.SHIP_TO_CUST_CODE=L.SHIP_TO_CUST_CODE , ");
                                        queryBuilder.AppendFormat("     O.COMPANY_CODE=L.COMPANY_CODE , ");
                                        queryBuilder.AppendFormat("     O.ORDER_ACTION=L.ORDER_ACTION , ");
                                        queryBuilder.AppendFormat("     O.PRICE_LIST=L.PRICE_LIST  ");
                                        queryBuilder.AppendFormat(" FROM SALE_ORDER O ");
                                        queryBuilder.AppendFormat(" INNER JOIN( ");
                                        queryBuilder.AppendFormat(" SELECT [ORDER_LOG_ID], [ORDER_ID], [QUOTATION_NO], [CUST_CODE], [SOM_ORDER_NO], [PO_NO], [SOM_ORDER_DTE], [SAP_ORDER_NO], [SIMULATE_DTM], ");
                                        queryBuilder.AppendFormat(" [PRICING_DTM], [DOC_TYPE_CODE], [SHIP_TO_CUST_PARTNER_ID], [DESCRIPTION], [DELIVERY_DTE], [SALE_REP], [GROUP_CODE], [ORG_CODE], ");
                                        queryBuilder.AppendFormat(" [CHANNEL_CODE], [DIVISION_CODE], [PRICE_LIST], [TERRITORY], [CONTACT_PERSON], [REMARK], [REASON_CODE], [REASON_REJECT], [NET_VALUE], ");
                                        queryBuilder.AppendFormat(" [TAX], [TOTAL], [PAYMENT_TERM], [INCOTERM], [PLANT_CODE], [SHIP_CODE], [SALE_SUP], [ORDER_STATUS], [ORDER_ACTION], [CREDIT_STATUS], ");
                                        queryBuilder.AppendFormat(" [SAP_MSG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_STATUS], [SAP_ORDER_DTE], [CUST_SALE_ID], [SHIP_TO_CUST_CODE], ");
                                        queryBuilder.AppendFormat(" [COMPANY_CODE]  ");
                                        queryBuilder.AppendFormat(" FROM SALE_ORDER_LOG ) L on L.ORDER_ID = O.ORDER_ID AND L.ORDER_LOG_ID = @VAL_ORDER_LOG_ID_OLD ");
                                        sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ORDER_LOG_ID_OLD", VAL_ORDER_LOG_ID_OLD));// Add New
                                        log.Debug("Query:" + queryBuilder.ToString());
                                        Console.WriteLine("Query:" + queryBuilder.ToString());
                                        log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                        Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                        //

                                        // Delete SALE_ORDER_PRODUCT
                                        sqlParameters = new List<SqlParameter>();// Add New
                                        queryBuilder = new StringBuilder();
                                        queryBuilder.AppendFormat(" DELETE FROM SALE_ORDER_PRODUCT WHERE [ORDER_ID]=@ORDER_ID ");
                                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                                        log.Debug("Query:" + queryBuilder.ToString());
                                        Console.WriteLine("Query:" + queryBuilder.ToString());
                                        log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                        Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                                        // Inser SALE_ORDER_PRODUCT
                                        sqlParameters = new List<SqlParameter>();// Add New
                                        queryBuilder = new StringBuilder();
                                        queryBuilder.AppendFormat(" INSERT INTO SALE_ORDER_PRODUCT ([ORDER_PROD_ID], [ORDER_ID], [PROD_CODE], [QTY], [PROD_CONV_ID], [NET_PRICE_EX], [TRANSFER_PRICE], [NET_PRICE_INC], [ADDITIONAL_PRICE], [ITEM_TYPE], [NET_VALUE], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_ITEM_NO], [PROD_ALT_UNIT], [PROD_CATE_CODE], [ADDITIONAL_PER_UNIT])  ");
                                        queryBuilder.AppendFormat(" SELECT NEXT VALUE FOR SALE_ORDER_PRODUCT_SEQ, @ORDER_ID, [PROD_CODE], [QTY], [PROD_CONV_ID], [NET_PRICE_EX], [TRANSFER_PRICE], [NET_PRICE_INC], [ADDITIONAL_PRICE], [ITEM_TYPE], [NET_VALUE], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_ITEM_NO], [PROD_ALT_UNIT], [PROD_CATE_CODE], [ADDITIONAL_PER_UNIT] FROM SALE_ORDER_PRODUCT_LOG WHERE [ORDER_LOG_ID] = @VAL_ORDER_LOG_ID_OLD ");
                                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ORDER_LOG_ID_OLD", VAL_ORDER_LOG_ID_OLD));// Add New
                                        log.Debug("Query:" + queryBuilder.ToString());
                                        Console.WriteLine("Query:" + queryBuilder.ToString());
                                        log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                        Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                                        //

                                    }

                                    List<String> errorParam = new List<string>();
                                    errorParam.Add(resultOutbound.Status.SO_Message);
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
                                transaction.Commit();
                                isNotRollback = true;
                                throw ex;
                            }

                            log.Debug("Response Outbound Service:" + urlService);
                            log.Debug("Response:" + JsonConvert.SerializeObject(resultOutbound));


                        }

                        // Call API


                        transaction.Commit();
                        return SAP_STATUS;



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


        public async Task<EntitySearchResultBase<ItemType3>> searchSaleOrderDocFlow(SearchCriteriaBase<SearchSaleOrderDocFlowCriteria> searchCriteria, string language, InterfaceSapConfig ic)
        {
            using (var client = new HttpClient())
            {
                SearchSaleOrderDocFlowCriteria criteria = searchCriteria.model;
                OutboundSearchSaleOrderDocFlowCriteria reqData = new OutboundSearchSaleOrderDocFlowCriteria();
                MyFirstAzureWebApp.ModelCriteria.outbound.searchorderdocflow.Input input = new MyFirstAzureWebApp.ModelCriteria.outbound.searchorderdocflow.Input();
                input.SAP_Sale_Order = criteria.SapOrderNo;
                input.Sale_Org = criteria.OrgCode;
                input.Distribution_Channel = criteria.ChannelCode;
                input.Division = criteria.DivisionCode;
                input.Sales_Order_Type = criteria.DocTypeCode;
                input.SOM_order_no = criteria.SomOrderNo;
                input.Document_Date = criteria.SomOrderDte;
                reqData.Input=input;

                client.BaseAddress = new Uri(ic.InterfaceSapUrl + CommonConstant.API_OutboundSearchSaleOrderDocFlow);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{ic.InterfaceSapUser}:{ic.InterfaceSapPwd}")));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", CommonConstant.USER_AGEN);
                client.DefaultRequestHeaders.Add(CommonConstant.ReqKey, ic.ReqKey);
                client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(CommonConstant.API_TIMEOUT));
                string fullPath = ic.InterfaceSapUrl + CommonConstant.API_OutboundSearchSaleOrderDocFlow;
                var jsonVal = JsonConvert.SerializeObject(reqData);
                var content = new StringContent(jsonVal, Encoding.UTF8, "application/json");
                log.Debug("Call Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundSearchSaleOrderDocFlow);
                log.Debug("=========== jsonVal ================");
                log.Debug("REQUEST:" + jsonVal);
                log.Debug("REQUEST:" + JObject.Parse(jsonVal));

                Console.WriteLine("Call Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundSearchSaleOrderDocFlow);
                Console.WriteLine("=========== jsonVal ================");
                Console.WriteLine("REQUEST:" + jsonVal);
                Console.WriteLine("REQUEST:" + JObject.Parse(jsonVal));

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

                SearchResultOutboundBasType3<ItemType3> resultOutbound = new SearchResultOutboundBasType3<ItemType3>();

                List<ItemType3> lst = new List<ItemType3>();
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    var dynamicData = JObject.Parse(result);
                    Console.WriteLine("Response Data");
                    Console.WriteLine(dynamicData);
                    log.Debug("Response Data");
                    log.Debug(dynamicData);

                    resultOutbound = await response.Content.ReadAsAsync<SearchResultOutboundBasType3<ItemType3>>();
                    if (resultOutbound.Header != null)
                    {
                        lst = resultOutbound.Header.Item;
                    }

                    if (!"S".Equals(resultOutbound.Status.SO_Status))
                    {
                        List<String> errorParam = new List<string>();
                        errorParam.Add(resultOutbound.Status.SO_Message);
                        ServiceException se = new ServiceException("E_CUSTOM_MSG_ONLY", errorParam, ObjectFacory.getCultureInfo(language));
                        throw se;
                    }
                }
                else
                {
                    string result_error = response.Content.ReadAsStringAsync().Result;
                    Exception ex = new Exception(result_error);
                    throw ex;
                }

                log.Debug("Response Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundSearchSaleOrderDocFlow);
                log.Debug("Response:" + JsonConvert.SerializeObject(resultOutbound));

                EntitySearchResultBase<ItemType3> searchResult = new EntitySearchResultBase<ItemType3>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : lst.Count();
                searchResult.data = lst;
                return searchResult;



            }

        }




        public async Task<LoginLdapResult> LoginLdap(LoginLdapCriteria searchCriteria)
        {
            LoginLdapResult loginLdapResult = null;
            LoginLdapCriteria reqData = searchCriteria;
            //Calling CreateSOAPWebRequest method  
            HttpWebRequest request = CreateSOAPWebRequest();

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request  

            string reqString = @"<?xml version=""1.0"" encoding=""utf-8""?> 
                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                   <soapenv:Header/>
                   <soapenv:Body>
                      <tem:LDAP_HRIS_USER_AUTHEN2>
                         <tem:userID>" + reqData.Username + @"</tem:userID>
                         <tem:password>" + reqData.Password + @"</tem:password>
                      </tem:LDAP_HRIS_USER_AUTHEN2>
                   </soapenv:Body>
                </soapenv:Envelope>";

            SOAPReqBody.LoadXml(reqString);


            log.Debug("Call Outbound Service:" + CommonConstant.API_OutboundLoginLdap);
            Console.WriteLine("Call Outbound Service:" + CommonConstant.API_OutboundLoginLdap);

            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request  
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream  
                    var ServiceResult = rd.ReadToEnd();
                    //writting stream result on console  
                    Console.WriteLine(ServiceResult);

                    string LDAP_HRIS_USER_AUTHEN2Result = ServiceResult.Split("LDAP_HRIS_USER_AUTHEN2Result")[1];
                    int open_ = LDAP_HRIS_USER_AUTHEN2Result.IndexOf("{");
                    int close_ = LDAP_HRIS_USER_AUTHEN2Result.IndexOf("}");
                    LDAP_HRIS_USER_AUTHEN2Result = LDAP_HRIS_USER_AUTHEN2Result.Substring(open_, close_);
                    Console.WriteLine(LDAP_HRIS_USER_AUTHEN2Result);

                    loginLdapResult = JsonConvert.DeserializeObject<LoginLdapResult>(LDAP_HRIS_USER_AUTHEN2Result);


                }
            }

            return loginLdapResult;

        }

        public HttpWebRequest CreateSOAPWebRequest()
        {
            //Making Web Request  
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@""+ CommonConstant.API_OutboundLoginLdap);
            //SOAPAction  
            //Req.Headers.Add(@"SOAPAction:http://tempuri.org/Addition");
            Req.Headers.Add(@"SOAP:Action");
            //Content_type  
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method  
            Req.Method = "POST";

            //Req.MediaType = "text/xml";
            //return HttpWebRequest  
            return Req;
        }


        public async Task<List<MsConfigParam>> SearchInterfaceSapConfig(SearchCriteriaBase<MsConfigParamCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from MS_CONFIG_PARAM where ACTIVE_FLAG = 'Y' ");
                MsConfigParamCriteria o = searchCriteria.model;
                if (o != null)
                {

                    if (!String.IsNullOrEmpty(o.ParamKeyword))
                    {
                        queryBuilder.AppendFormat(" and PARAM_KEYWORD = @ParamKeyword ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ParamKeyword", o.ParamKeyword));// Add New
                    }
                }

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<MsConfigParam> lst = context.MsConfigParam.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                return lst;

            }

        }








        public async Task<StatusType2> delSaleOrder(UpdSaleOrderCriteria searchCriteria, UserProfileForBack userProfile, string language, InterfaceSapConfig ic, int timeZone)
        {
            StatusType2 SAP_STATUS = null;

            string ORDER_ACTION_CANCEL = SaleOrderAction.ORDER_ACTION_CANCEL.ToString("d");
            string ORDER_STATUS_CANCEL = SaleOrderStatus.ORDER_STATUS_CANCEL.ToString("d");

            string VAL_ORDER_ACTION = ORDER_ACTION_CANCEL;
            string VAL_ORDER_STATUS = searchCriteria.SaleOrder.OrderStatus;
            string VAL_SAP_STATUS = "";
            string VAL_SAP_MSG = "";

            using (var client = new HttpClient())
            {
                if (!String.IsNullOrEmpty(searchCriteria.SaleOrder.SapOrderNo))
                {


                    // Same updSaleOrder; Copy updSaleOrder

                    OutboundUpdSaleOrderCriteria reqData = new OutboundUpdSaleOrderCriteria();
                    ModelCriteria.outbound.updatesaleorder.Header header = new ModelCriteria.outbound.updatesaleorder.Header();

                    header.SO_Cancel_Indicator = "X";// this line Different updSaleOrder

                    header.Simulate_Indicator = "";
                    header.SAP_Sale_Order_No = searchCriteria.SaleOrder.SapOrderNo;
                    header.Sale_Org = searchCriteria.SaleOrder.OrgCode;
                    header.Distribution_Channel = searchCriteria.SaleOrder.ChannelCode;
                    header.Division = searchCriteria.SaleOrder.DivisionCode;
                    header.Sales_Order_Type = searchCriteria.SaleOrder.DocTypeCode;
                    header.SOM_order_no = searchCriteria.SaleOrder.SomOrderNo;
                    if (!String.IsNullOrEmpty(searchCriteria.SaleOrder.SomOrderDte))
                    {
                        string Document_dateStr = String.Join("", searchCriteria.SaleOrder.SomOrderDte.Split("T")[0].Split("-"));
                        header.Document_date = Document_dateStr;
                    }
                    header.Reference_Qoutation_Document = searchCriteria.SaleOrder.QuotationNo;
                    header.Sold_to_customer = searchCriteria.SaleOrder.CustCode;
                    header.Ship_to_customer = searchCriteria.SaleOrder.ShipToCustCode;
                    if (!String.IsNullOrEmpty(searchCriteria.SaleOrder.DeliveryDte))
                    {
                        string Request_Delivery_dateStr = String.Join("", searchCriteria.SaleOrder.DeliveryDte.Split("T")[0].Split("-"));
                        header.Request_Delivery_date = Request_Delivery_dateStr;
                    }
                    header.Incoterms1 = searchCriteria.SaleOrder.Incoterm;
                    header.Incoterms2 = searchCriteria.SaleOrder.Incoterm;

                    //
                    if (!String.IsNullOrEmpty(searchCriteria.SaleOrder.PriceDate))
                    {
                        header.Price_Date = searchCriteria.SaleOrder.PriceDate;
                        header.Price_Time = searchCriteria.SaleOrder.PriceTime;
                    }
                    else
                    {
                        header.Price_Date = DateTime.Now.ToString("yyyyMMdd");
                        header.Price_Time = DateTime.Now.AddHours(timeZone).ToString("HHmmss");
                    }
                    //


                    header.Price_List = searchCriteria.SaleOrder.PriceList;
                    header.Reason_For_Rejection = searchCriteria.SaleOrder.ReasonReject;
                    header.Order_Reason = searchCriteria.SaleOrder.ReasonCode;

                    List<Item> ItemList = null;
                    Item item = null;
                    if (searchCriteria.Items != null && searchCriteria.Items.Count != 0)
                    {
                        ItemList = new List<Item>();
                        foreach (SaleOrderProductForUpdateSaleOrder product in searchCriteria.Items)
                        {
                            item = new Item();
                            item.Item_no = product.SapItemNo;
                            item.Material_Code = product.ProdCode;
                            //item.Item_Category = product.ItemType;
                            item.Item_Category = ""; //--Fix
                            item.Plant = product.PlantCode;
                            item.Shipping_Point_Receiving_Pt = product.ShipCode;
                            item.Storage_Location = "";
                            item.Order_Quantity = product.Qty;
                            item.Unit = product.ProdAltUnit;
                            item.Price_per_Unit = "";
                            if (!String.IsNullOrEmpty(product.AdditionalPrice))
                            {
                                List<ModelCriteria.outbound.updatesaleorder.Condition> conditionList = null;
                                ModelCriteria.outbound.updatesaleorder.Condition condition_ = null;
                                if (Convert.ToDecimal(product.AdditionalPrice) > 0)
                                {
                                    conditionList = new List<ModelCriteria.outbound.updatesaleorder.Condition>();
                                    condition_ = new ModelCriteria.outbound.updatesaleorder.Condition();
                                    condition_.Condition_type = "ZL34";
                                    //condition_.Condition_Amount = Convert.ToDecimal(product.AdditionalPrice);
                                    condition_.Condition_Amount = product.AdditionalPrice;
                                    condition_.Condition_Per = product.AdditionalPerUnit;
                                    condition_.Condition_Unit = product.ProdAltUnit;
                                    conditionList.Add(condition_);
                                    item.Condition = conditionList;
                                }
                                else
                                {

                                    conditionList = new List<ModelCriteria.outbound.updatesaleorder.Condition>();
                                    condition_ = new ModelCriteria.outbound.updatesaleorder.Condition();
                                    condition_.Condition_type = "";
                                    condition_.Condition_Amount = "";
                                    condition_.Condition_Per = "";
                                    condition_.Condition_Unit = "";
                                    conditionList.Add(condition_);
                                    item.Condition = conditionList;
                                }
                            }

                            ItemList.Add(item);
                        }
                    }

                    header.Item = ItemList;
                    reqData.Header = header;

                    // Same updSaleOrder; Copy updSaleOrder


                    client.BaseAddress = new Uri(ic.InterfaceSapUrl + CommonConstant.API_OutboundChangeSaleOrder);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{ic.InterfaceSapUser}:{ic.InterfaceSapPwd}")));
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("User-Agent", CommonConstant.USER_AGEN);
                    client.DefaultRequestHeaders.Add(CommonConstant.ReqKey, ic.ReqKey);
                    client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(CommonConstant.API_TIMEOUT));
                    string fullPath = ic.InterfaceSapUrl + CommonConstant.API_OutboundChangeSaleOrder;
                    var jsonVal = JsonConvert.SerializeObject(reqData);
                    var content = new StringContent(jsonVal, Encoding.UTF8, "application/json");
                    log.Debug("Call Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundChangeSaleOrder);
                    log.Debug("=========== jsonVal ================");
                    log.Debug("REQUEST:" + jsonVal);
                    log.Debug("REQUEST:" + JObject.Parse(jsonVal));

                    Console.WriteLine("Call Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundChangeSaleOrder);
                    Console.WriteLine("=========== jsonVal ================");
                    Console.WriteLine("REQUEST:" + jsonVal);
                    Console.WriteLine("REQUEST:" + JObject.Parse(jsonVal));

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

                    SearchResultOutboundBasType2<ItemType2> resultOutbound = new SearchResultOutboundBasType2<ItemType2>();

                    List<ItemType2> lst = new List<ItemType2>();
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        var dynamicData = JObject.Parse(result);
                        Console.WriteLine("Response Data");
                        Console.WriteLine(dynamicData);
                        log.Debug("Response Data");
                        log.Debug(dynamicData);

                        resultOutbound = await response.Content.ReadAsAsync<SearchResultOutboundBasType2<ItemType2>>();
                        if (resultOutbound.Header != null)
                        {
                            lst = resultOutbound.Header.Item;
                        }


                        if ("S".Equals(resultOutbound.Status.SO_Status))
                        {
                            VAL_ORDER_STATUS = ORDER_STATUS_CANCEL;
                        }
                        VAL_SAP_STATUS = resultOutbound.Status.SO_Status;
                        VAL_SAP_MSG = resultOutbound.Status.SO_Message;

                        
                        SAP_STATUS = resultOutbound.Status;
                        /*StatusType2 Status = resultOutbound.Status;
                        HeaderType2<ItemType2> Header = resultOutbound.Header;
                        */


                    }
                    else
                    {
                        string result_error = response.Content.ReadAsStringAsync().Result;
                        Exception ex = new Exception(result_error);
                        throw ex;
                    }

                    log.Debug("Response Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundChangeSaleOrder);
                    log.Debug("Response:" + JsonConvert.SerializeObject(resultOutbound));
                }else
                {
                    VAL_ORDER_STATUS = ORDER_STATUS_CANCEL;
                }


                // Process Data

                decimal VAL_ORDER_LOG_ID = await getNextValueSALE_ORDER_LOG_SEQ();
                using (var context = new MyAppContext())
                {

                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var sqlParameters = new List<SqlParameter>();// Add New
                            StringBuilder queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat(" UPDATE SALE_ORDER  ");
                            queryBuilder.AppendFormat(" SET [ORDER_STATUS]=@VAL_ORDER_STATUS , [ORDER_ACTION]=@VAL_ORDER_ACTION , [SAP_STATUS]=@VAL_SAP_STATUS , [SAP_MSG]=@VAL_SAP_MSG,  [UPDATE_USER]=@USER, [UPDATE_DTM]=dbo.GET_SYSDATETIME() ");
                            queryBuilder.AppendFormat(" WHERE [ORDER_ID]=@ORDER_ID ");
                            sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ORDER_STATUS", VAL_ORDER_STATUS));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ORDER_ACTION", VAL_ORDER_ACTION));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_SAP_STATUS", VAL_SAP_STATUS));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_SAP_MSG", VAL_SAP_MSG));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getEmpId()));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                            Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                            int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat(" INSERT INTO SALE_ORDER_LOG ([ORDER_LOG_ID], [ORDER_ID], [QUOTATION_NO], [CUST_CODE], [SOM_ORDER_NO], [PO_NO], [SOM_ORDER_DTE], [SAP_ORDER_NO], [SIMULATE_DTM], [PRICING_DTM], [DOC_TYPE_CODE], [SHIP_TO_CUST_PARTNER_ID], [DESCRIPTION], [DELIVERY_DTE], [SALE_REP], [GROUP_CODE], [ORG_CODE], [CHANNEL_CODE], [DIVISION_CODE], [PRICE_LIST], [TERRITORY], [CONTACT_PERSON], [REMARK], [REASON_CODE], [REASON_REJECT], [NET_VALUE], [TAX], [TOTAL], [PAYMENT_TERM], [INCOTERM], [PLANT_CODE], [SHIP_CODE], [SALE_SUP], [ORDER_STATUS], [ORDER_ACTION], [CREDIT_STATUS], [SAP_MSG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_STATUS], [SAP_ORDER_DTE], [CUST_SALE_ID], [SHIP_TO_CUST_CODE], [COMPANY_CODE])  ");
                            queryBuilder.AppendFormat(" SELECT @VAL_ORDER_LOG_ID, [ORDER_ID], [QUOTATION_NO], [CUST_CODE], [SOM_ORDER_NO], [PO_NO], [SOM_ORDER_DTE], [SAP_ORDER_NO], [SIMULATE_DTM], [PRICING_DTM], [DOC_TYPE_CODE], [SHIP_TO_CUST_PARTNER_ID], [DESCRIPTION], [DELIVERY_DTE], [SALE_REP], [GROUP_CODE], [ORG_CODE], [CHANNEL_CODE], [DIVISION_CODE], [PRICE_LIST], [TERRITORY], [CONTACT_PERSON], [REMARK], [REASON_CODE], [REASON_REJECT], [NET_VALUE], [TAX], [TOTAL], [PAYMENT_TERM], [INCOTERM], [PLANT_CODE], [SHIP_CODE], [SALE_SUP], [ORDER_STATUS], [ORDER_ACTION], [CREDIT_STATUS], [SAP_MSG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_STATUS], [SAP_ORDER_DTE], [CUST_SALE_ID], [SHIP_TO_CUST_CODE], [COMPANY_CODE] FROM SALE_ORDER WHERE [ORDER_ID] = @ORDER_ID ");
                            sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ORDER_LOG_ID", VAL_ORDER_LOG_ID));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                            Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat(" INSERT INTO SALE_ORDER_PRODUCT_LOG ([ORDER_PROD_LOG_ID], [ORDER_LOG_ID], [PROD_CODE], [QTY], [PROD_CONV_ID], [NET_PRICE_EX], [TRANSFER_PRICE], [NET_PRICE_INC], [ADDITIONAL_PRICE], [ADDITIONAL_PER_UNIT], [ITEM_TYPE], [NET_VALUE], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_ITEM_NO], [PROD_ALT_UNIT], [PROD_CATE_CODE])  ");
                            queryBuilder.AppendFormat(" SELECT NEXT VALUE FOR SALE_ORDER_PRODUCT_LOG_SEQ, @VAL_ORDER_LOG_ID, [PROD_CODE], [QTY], [PROD_CONV_ID], [NET_PRICE_EX], [TRANSFER_PRICE], [NET_PRICE_INC], [ADDITIONAL_PRICE], [ADDITIONAL_PER_UNIT], [ITEM_TYPE], [NET_VALUE], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM], [SAP_ITEM_NO], [PROD_ALT_UNIT], [PROD_CATE_CODE] FROM SALE_ORDER_PRODUCT WHERE [ORDER_ID] = @ORDER_ID ");
                            sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_ID", searchCriteria.SaleOrder.OrderId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("VAL_ORDER_LOG_ID", VAL_ORDER_LOG_ID));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            log.Debug("sqlParameters.ToArray():" + sqlParameters.ToArray());
                            Console.WriteLine("sqlParameters.ToArray():" + sqlParameters.ToArray());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                            transaction.Commit();

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                // Process Data




                if (SAP_STATUS!= null && !"S".Equals(SAP_STATUS.SO_Status))
                {
                    List<String> errorParam = new List<string>();
                    errorParam.Add(SAP_STATUS.SO_Message);
                    ServiceException se = new ServiceException("E_CUSTOM_MSG_ONLY", errorParam, ObjectFacory.getCultureInfo(language));
                    throw se;
                }

                return SAP_STATUS;



            }

        }




        public async Task<String> GetOrderLogIdOld(UpdSaleOrderCriteria searchCriteria)
        {
            
            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select max(ORDER_LOG_ID) ORDER_LOG_ID from SALE_ORDER_LOG where SAP_STATUS ='S' and ORDER_ID = @orderId  ");
                QueryUtils.addParam(command, "orderId", searchCriteria.SaleOrder.OrderId);

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "ORDER_LOG_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }




    }
}