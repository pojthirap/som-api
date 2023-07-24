using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using MyFirstAzureWebApp.Authentication;
using MyFirstAzureWebApp.Business.org;
using MyFirstAzureWebApp.common;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.enumval;
using MyFirstAzureWebApp.exception;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.ModelCriteria.inbound.cancelSaleOrder;
using MyFirstAzureWebApp.ModelCriteria.inbound.changeSaleOrder;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.adm;
using MyFirstAzureWebApp.Models.job;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Models.plan;
using MyFirstAzureWebApp.Models.pospect;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.record;
using MyFirstAzureWebApp.Models.saleorder;
using MyFirstAzureWebApp.Models.sendGrid;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.SearchCriteria.type2;
using MyFirstAzureWebApp.SearchCriteria.type3;
using MyFirstAzureWebApp.Utils;
using Newtonsoft.Json;
using NLog;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using static MyFirstAzureWebApp.Business.org.PlanTripImp;
using static MyFirstAzureWebApp.Business.org.PlanTripProspectImp;
using static MyFirstAzureWebApp.Business.org.TemplateAppFormImp;
using static MyFirstAzureWebApp.Business.org.TemplateSaFormImp;
using static MyFirstAzureWebApp.SearchCriteria.SearchStockCountTabCustom;

namespace MyFirstAzureWebApp.Controllers
{

    [ApiController]
    public class AllRequestController : BaseController
    {

        private Logger log = LogManager.GetCurrentClassLogger();
        private readonly ISendGridClient sendGridClient;
        private readonly ILogger<SendGridController> logger;
        private IProspectAccount prospectAccountImp;
        private IProspect prospectImp;
        private IOrgTerritory orgTerritoryImp;
        private IProspectDedicateTert prospectDedicateTertImp;
        private IProspectAddress prospectAddressImp;
        private IProspectContact prospectContactImp;
        private IProspectVisitHour prospectVisitHourImp;
        private IRecordAppForm recordAppFormImp;
        private IProspectRecommend prospectRecommendImp;
        private IRecordSaForm recordSaFormImp;
        private ISaleOrder saleOrderImp;
        private IRecordMeter recordMeterImp;
        private IProspectFeed prospectFeedImp;
        private IRecordStockCard recordStockCardImp;
        private ICustomerSale customerSaleImp;
        private IPlanTrip planTripImp;
        private IPlanTripProspect planTripProspectImp;
        private ICallAPI callAPIImp;
        private IMsConfigLov msConfigLovImp;
        private IMeter meterImp;
        private IPlanTripTask planTripTaskImp;
        private ITemplateAppForm templateAppFormImp;
        private IRecordAppFormFile recordAppFormFileImp;
        private ITemplateStockCard templateStockCardImp;
        private ITemplateSaForm templateSaFormImp;
        private IMsOrderDocType msOrderDocTypeImp;
        private IMsOrderReason msOrderReasonImp;
        private IMsProduct msProductImp;
        private IMsProductConversion msProductConversionImp;
        private ISaleOrderChangeLog saleOrderChangeLogImp;
        private IAdmPermObject admPermObjectImp;
        private IAdmGroupApp admGroupAppImp;
        private ICustomerCompany customerCompanyImp;
        private IMsOrderIncoterm msOrderIncotermImp;
        private IEmailJob emailJobImp;


        public AllRequestController(ISendGridClient sendGridClient, ILogger<SendGridController> logger)
        {
            this.sendGridClient = sendGridClient;
            this.logger = logger;
            prospectAccountImp = new ProspectAccountImp();
            prospectImp = new ProspectImp();
            orgTerritoryImp = new OrgTerritoryImp();
            prospectDedicateTertImp = new ProspectDedicateTertImp();
            prospectAddressImp = new ProspectAddressImp();
            prospectContactImp = new ProspectContactImp();
            prospectVisitHourImp = new ProspectVisitHourImpImp();
            recordAppFormImp = new RecordAppFormImp();
            prospectRecommendImp = new ProspectRecommendImp();
            saleOrderImp = new SaleOrderImp();
            recordMeterImp = new RecordMeterImp();
            prospectFeedImp = new ProspectFeedImp();
            recordStockCardImp = new RecordStockCardImp();
            customerSaleImp = new CustomerSaleImp();
            planTripImp = new PlanTripImp();
            recordSaFormImp = new RecordSaFormImp();
            planTripProspectImp = new PlanTripProspectImp();
            callAPIImp = new CallAPIImp();
            msConfigLovImp = new MsConfigLovImp();
            meterImp = new MeterImp();
            planTripTaskImp = new PlanTripTaskImp();
            templateAppFormImp = new TemplateAppFormImp();
            recordAppFormFileImp = new RecordAppFormFileImp();
            templateStockCardImp = new TemplateStockCardImp();
            templateSaFormImp = new TemplateSaFormImp();
            msOrderDocTypeImp = new MsOrderDocTypeImp();
            msOrderReasonImp = new MsOrderReasonImp();
            msProductImp = new MsProductImp();
            msProductConversionImp = new MsProductConversionImp();
            saleOrderChangeLogImp = new SaleOrderChangeLogImp();
            admPermObjectImp = new AdmPermObjectImp();
            admGroupAppImp = new AdmGroupAppImp();
            customerCompanyImp = new CustomerCompanyImp();
            msOrderIncotermImp = new MsOrderIncotermImp();
            emailJobImp = new EmailJobImp();

        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/prospect.php" target="_blank">For Page prospect/prospect.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchMyAccount")]
        public async Task<ResponseResult> searchMyAccount(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ProspectAccountCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchMyAccount", "searchMyAccount", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchMyProspectCustom> entitiySearchResult = await prospectAccountImp.searchMyAccount(criteria, userProfileForBack);
                List<SearchMyProspectCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchMyProspectCustom> searchResult = new SearchResultBase<SearchMyProspectCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchMyAccount", "searchMyAccount", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchMyAccount", "searchMyAccount", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/prospect.php" target="_blank">For Page prospect/prospect.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchProspectRecommend")]
        public async Task<ResponseResult> searchProspectRecommend(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ProspectAccountCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchProspectRecommend", "searchProspectRecommend", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchProspectRecommendCustom> entitiySearchResult = await prospectAccountImp.searchProspectRecommend(criteria, userProfileForBack);
                List<SearchProspectRecommendCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchProspectRecommendCustom> searchResult = new SearchResultBase<SearchProspectRecommendCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchProspectRecommend", "searchProspectRecommend", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchProspectRecommend", "searchProspectRecommend", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/prospect.php" target="_blank">For Page prospect/prospect.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchAccountInTerritory")]
        public async Task<ResponseResult> searchAccountInTerritory(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ProspectAccountCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAccountInTerritory", "searchAccountInTerritory", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchAccountInTerritoryCustom> entitiySearchResult = await prospectAccountImp.searchAccountInTerritory(criteria, userProfileForBack);
                List<SearchAccountInTerritoryCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchAccountInTerritoryCustom> searchResult = new SearchResultBase<SearchAccountInTerritoryCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchAccountInTerritory", "searchAccountInTerritory", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchAccountInTerritory", "searchAccountInTerritory", resR);
                return resR;
            }
        }






        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/otherprospect.php" target="_blank">For Page prospect/otherprospect.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchOtherProspect")]
        public async Task<ResponseResult> searchOtherProspect(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ProspectAccountCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchOtherProspect", "searchOtherProspect", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchOtherProspectCustom> entitiySearchResult = await prospectAccountImp.searchOtherProspect(criteria, userProfileForBack);
                List<SearchOtherProspectCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchOtherProspectCustom> searchResult = new SearchResultBase<SearchOtherProspectCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchOtherProspect", "searchOtherProspect", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchOtherProspect", "searchOtherProspect", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/create.php" target="_blank">For Page prospect/create.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("createProspect")]
        public async Task<ResponseResult> createProspect(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, CreateProspectModel createProspectModel)
        {
            try
            {
                onAfterReceiveRequest("createProspect", "createProspect", createProspectModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                CreateProspectModel model = await prospectAccountImp.createProspect(createProspectModel, userProfileForBack, language);
                SearchResultBase<CreateProspectModel> searchResult = new SearchResultBase<CreateProspectModel>();
                List<CreateProspectModel> list = new List<CreateProspectModel>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("createProspect", "createProspect", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("createProspect", "createProspect", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/create.php" target="_blank">For Page prospect/create.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("delProspect")]
        public async Task<ResponseResult> delProspect(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, DeleteProspectModel deleteProspectModel)
        {
            try
            {
                onAfterReceiveRequest("delProspect", "delProspect", deleteProspectModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await prospectAccountImp.delProspect(deleteProspectModel, userProfileForBack);
                SearchResultBase<ProspectModel> searchResult = new SearchResultBase<ProspectModel>();
                List<ProspectModel> list = new List<ProspectModel>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("delProspect", "delProspect", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("delProspect", "delProspect", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/create.php" target="_blank">For Page prospect/create.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("delPlanTripProspectAdHoc")]
        public async Task<ResponseResult> delPlanTripProspectAdHoc(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, DeletePlanTripProspectAdHocModel deletePlanTripProspectAdHocModel)
        {
            try
            {
                onAfterReceiveRequest("delPlanTripProspectAdHoc", "delPlanTripProspectAdHoc", deletePlanTripProspectAdHocModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await prospectAccountImp.delPlanTripProspectAdHoc(deletePlanTripProspectAdHocModel);
                SearchResultBase<ProspectModel> searchResult = new SearchResultBase<ProspectModel>();
                List<ProspectModel> list = new List<ProspectModel>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("delPlanTripProspectAdHoc", "delPlanTripProspectAdHoc", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("delPlanTripProspectAdHoc", "delPlanTripProspectAdHoc", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/create.php" target="_blank">For Page prospect/create.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("delPlanTripTaskAdHoc")]
        public async Task<ResponseResult> delPlanTripTaskAdHoc(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, DeletePlanTripTaskAdHocModel deletePlanTripTaskAdHocModel)
        {
            try
            {
                onAfterReceiveRequest("delPlanTripTaskAdHoc", "delPlanTripTaskAdHoc", deletePlanTripTaskAdHocModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await prospectAccountImp.delPlanTripTaskAdHoc(deletePlanTripTaskAdHocModel);
                SearchResultBase<ProspectModel> searchResult = new SearchResultBase<ProspectModel>();
                List<ProspectModel> list = new List<ProspectModel>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("delPlanTripTaskAdHoc", "delPlanTripTaskAdHoc", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("delPlanTripTaskAdHoc", "delPlanTripTaskAdHoc", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/create.php" target="_blank">For Page prospect/create.php</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/dbd/" target="_blank">For Page prospect/dbd/</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/basic/" target="_blank">For Page prospect/basic/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchProspectSaTab")]
        public async Task<ResponseResult> searchProspectSaTab(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ProspectCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchProspectSaTab", "searchProspectSaTab", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchProspectSaTabCustom> entitiySearchResult = await prospectAccountImp.searchProspectSaTab(criteria);
                List<SearchProspectSaTabCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchProspectSaTabCustom> searchResult = new SearchResultBase<SearchProspectSaTabCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchProspectSaTab", "searchProspectSaTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchProspectSaTab", "searchProspectSaTab", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/create.php" target="_blank">For Page prospect/create.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updProspectSaTab")]
        public async Task<ResponseResult> updProspectSaTab(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, UpdateProspectModel updateProspectModel)
        {
            try
            {
                onAfterReceiveRequest("updProspectSaTab", "updProspectSaTab", updateProspectModel);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                await prospectImp.updProspectSaTab(updateProspectModel, userProfileForBack);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updProspectSaTab", "updProspectSaTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("updProspectSaTab", "updProspectSaTab", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/salesterritory/" target="_blank">For Page prospect/salesterritory/</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/accountteam/" target="_blank">For Page prospect/accountteam/</a>
        /// <p>Required Paramerer</p>
        /// <para>ProspectId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchSalesTerritoryTab")]
        public async Task<ResponseResult> searchSalesTerritoryTab(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ProspectCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchSalesTerritoryTab", "searchSalesTerritoryTab", criteria);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchSalesTerritoryTabCustom> entitiySearchResult = await orgTerritoryImp.searchSalesTerritoryTab(criteria, userProfileForBack);
                List<SearchSalesTerritoryTabCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchSalesTerritoryTabCustom> searchResult = new SearchResultBase<SearchSalesTerritoryTabCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchSalesTerritoryTab", "searchSalesTerritoryTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchSalesTerritoryTab", "searchSalesTerritoryTab", resR);
                return resR;
            }
        }







        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/salesterritory/" target="_blank">For Page prospect/salesterritory/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getTerritoryForDedicated")]
        public async Task<ResponseResult> getTerritoryForDedicated(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetTerritoryForDedicatedCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getTerritoryForDedicated", "getTerritoryForDedicated", criteria);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<OrgTerritory> entitiySearchResult = await orgTerritoryImp.getTerritoryForDedicated(criteria, userProfileForBack);
                List<OrgTerritory> lst = entitiySearchResult.data;
                SearchResultBase<OrgTerritory> searchResult = new SearchResultBase<OrgTerritory>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getTerritoryForDedicated", "getTerritoryForDedicated", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getTerritoryForDedicated", "getTerritoryForDedicated", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/salesterritory/" target="_blank">For Page prospect/salesterritory/</a>
        /// <p>Required Paramerer</p>
        /// <p>ProspectId</p>
        /// <p>TerritoryId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addProspectDedicated")]
        public async Task<ResponseResult> addProspectDedicated(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, ProspectDedicateTertModel prospectDedicateTertModel)
        {
            try
            {
                onAfterReceiveRequest("addProspectDedicated", "addProspectDedicated", prospectDedicateTertModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                prospectDedicateTertModel.UserProfile = userProfileForBack;
                List<ProspectDedicateTert> list = await prospectDedicateTertImp.addProspectDedicated(prospectDedicateTertModel);
                SearchResultBase<ProspectDedicateTert> searchResult = new SearchResultBase<ProspectDedicateTert>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addProspectDedicated", "addProspectDedicated", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("addProspectDedicated", "addProspectDedicated", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/salesterritory/" target="_blank">For Page prospect/salesterritory/</a>
        /// <p>Required Paramerer</p>
        /// <p>ProspDedicateId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("delProspectDedicated")]
        public async Task<ResponseResult> delProspectDedicated(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, ProspectDedicateTertModel prospectDedicateTertModel)
        {
            try
            {
                onAfterReceiveRequest("delProspectDedicated", "delProspectDedicated", prospectDedicateTertModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                prospectDedicateTertModel.UserProfile = userProfileForBack;
                int model = await prospectDedicateTertImp.delProspectDedicated(prospectDedicateTertModel);
                SearchResultBase<TemplateSaForm> searchResult = new SearchResultBase<TemplateSaForm>();
                List<TemplateSaForm> list = new List<TemplateSaForm>();
                //list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("delProspectDedicated", "delProspectDedicated", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("delProspectDedicated", "delProspectDedicated", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/address/" target="_blank">For Page prospect/address/</a>
        /// <p>Required Paramerer</p>
        /// <para>ProspectId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchProspectAddress")]
        public async Task<ResponseResult> searchProspectAddress(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchProspectAddressCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchProspectAddress", "searchProspectAddress", criteria);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<ProspectAddress> entitiySearchResult = await prospectAddressImp.searchProspectAddress(criteria);
                List<ProspectAddress> lst = entitiySearchResult.data;
                SearchResultBase<ProspectAddress> searchResult = new SearchResultBase<ProspectAddress>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchProspectAddress", "searchProspectAddress", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchProspectAddress", "searchProspectAddress", resR);
                return resR;
            }
        }






        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/contacts/" target="_blank">For Page prospect/contacts/</a>
        /// <p>Required Paramerer</p>
        /// <para>ProspectId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchProspectContact")]
        public async Task<ResponseResult> searchProspectContact(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchProspectContactCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchProspectContact", "searchProspectContact", criteria);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<ProspectContact> entitiySearchResult = await prospectContactImp.searchProspectContact(criteria);
                List<ProspectContact> lst = entitiySearchResult.data;
                SearchResultBase<ProspectContact> searchResult = new SearchResultBase<ProspectContact>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchProspectContact", "searchProspectContact", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchProspectContact", "searchProspectContact", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/contacts/" target="_blank">For Page prospect/contacts/</a>
        /// <p>Required Paramerer</p>
        /// <p>ProspectId</p>
        /// <p>ProspAccId</p>
        /// <p>FirstName</p>
        /// <p>LastName</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addContact")]
        public async Task<ResponseResult> addContact(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, ProspectContactsModel prospectContactModel)
        {
            try
            {
                onAfterReceiveRequest("addContact", "addContact", prospectContactModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                prospectContactModel.UserProfile = userProfileForBack;
                ProspectContact model = await prospectContactImp.addContact(prospectContactModel);
                SearchResultBase<ProspectContact> searchResult = new SearchResultBase<ProspectContact>();
                List<ProspectContact> list = new List<ProspectContact>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addContact", "addContact", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("addContact", "addContact", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/contacts/" target="_blank">For Page prospect/contacts/</a>
        /// <p>Required Paramerer</p>
        /// <p>ProspContactId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updContact")]
        public async Task<ResponseResult> updContact(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, ProspectContactsModel prospectContactModel)
        {
            try
            {
                onAfterReceiveRequest("updContact", "updContact", prospectContactModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                prospectContactModel.UserProfile = userProfileForBack;
                int model = await prospectContactImp.updContact(prospectContactModel);
                SearchResultBase<TemplateSaForm> searchResult = new SearchResultBase<TemplateSaForm>();
                List<TemplateSaForm> list = new List<TemplateSaForm>();
                //list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updContact", "updContact", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("updContact", "updContact", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/contacts/" target="_blank">For Page prospect/contacts/</a>
        /// <p>Required Paramerer</p>
        /// <p>ProspContactId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("delContact")]
        public async Task<ResponseResult> delContact(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, ProspectContactsModel prospectContactModel)
        {
            try
            {
                onAfterReceiveRequest("delContact", "delContact", prospectContactModel);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                prospectContactModel.UserProfile = userProfileForBack;
                await prospectContactImp.delContact(prospectContactModel);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("delContact", "delContact", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("delContact", "delContact", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/visitinghours/" target="_blank">For Page prospect/visitinghours/</a>
        /// <p>Required Paramerer</p>
        /// <para>ProspectId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchVisitHour")]
        public async Task<ResponseResult> searchVisitHour(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchProspectVisitHourCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchVisitHour", "searchVisitHour", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchVisitHourCustom> entitiySearchResult = await prospectVisitHourImp.searchVisitHour(criteria);
                List<SearchVisitHourCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchVisitHourCustom> searchResult = new SearchResultBase<SearchVisitHourCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchVisitHour", "searchVisitHour", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchVisitHour", "searchVisitHour", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/visitinghours/" target="_blank">For Page prospect/visitinghours/</a>
        /// <p>Required Paramerer</p>
        /// <p>ProspectId</p>
        /// <p>ProspAccId</p>
        /// <p>FirstName</p>
        /// <p>LastName</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addVisitHour")]
        public async Task<ResponseResult> addVisitHour(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, ProspectVisitHourModel prospectVisitHourModel)
        {
            try
            {
                onAfterReceiveRequest("addVisitHour", "addVisitHour", prospectVisitHourModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                prospectVisitHourModel.UserProfile = userProfileForBack;
                List<ProspectVisitHour> list = await prospectVisitHourImp.addVisitHour(prospectVisitHourModel);
                SearchResultBase<ProspectVisitHour> searchResult = new SearchResultBase<ProspectVisitHour>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addVisitHour", "addVisitHour", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("addVisitHour", "addVisitHour", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/visitinghours/" target="_blank">For Page prospect/visitinghours/</a>
        /// <p>Required Paramerer</p>
        /// <p>ProspContactId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("delVisitHour")]
        public async Task<ResponseResult> delVisitHour(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, ProspectVisitHourModel prospectVisitHourModel)
        {
            try
            {
                onAfterReceiveRequest("delVisitHour", "delVisitHour", prospectVisitHourModel);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                await prospectVisitHourImp.delVisitHour(prospectVisitHourModel);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("delVisitHour", "delVisitHour", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("addVisitHour", "addVisitHour", resR);
                return resR;
            }
        }






        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/surveyresults/" target="_blank">For Page prospect/surveyresults/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchSurveyResultTab")]
        public async Task<ResponseResult> searchSurveyResultTab(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchSurveyResultTabCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchSurveyResultTab", "searchSurveyResultTab", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchSurveyResultTabCustom> entitiySearchResult = await recordAppFormImp.searchSurveyResultTab(criteria, userProfileForBack);
                List<SearchSurveyResultTabCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchSurveyResultTabCustom> searchResult = new SearchResultBase<SearchSurveyResultTabCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchSurveyResultTab", "searchSurveyResultTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchSurveyResultTab", "searchSurveyResultTab", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/surveyresults/" target="_blank">For Page prospect/surveyresults/</a>
        /// <p>Required Paramerer</p>
        /// <para>RceAppFormId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("viewSurveyResult")]
        public async Task<ResponseResult> viewSurveyResult(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ViewSurveyResultCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("viewSurveyResult", "viewSurveyResult", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<ViewSurveyResultCustom> entitiySearchResult = await recordAppFormImp.viewSurveyResult(criteria, userProfileForBack);
                List<ViewSurveyResultCustom> lst = entitiySearchResult.data;
                if (lst != null && lst.Count != 0) {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        List<RecordAppFormFile> ListFile = lst[i].ListFile;
                        for(int j=0;j< ListFile.Count; j++)
                        {
                            RecordAppFormFile raf = ListFile[j];
                            raf.FileUrl = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_RECORD_APP_FORM_FILE, raf.FileId.ToString());

                        }
                    }
                }



                SearchResultBase<ViewSurveyResultCustom> searchResult = new SearchResultBase<ViewSurveyResultCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("viewSurveyResult", "viewSurveyResult", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("viewSurveyResult", "viewSurveyResult", resR);
                return resR;
            }
        }






        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/recommendbu/" target="_blank">For Page prospect/recommendbu/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchRecommendBuTab")]
        public async Task<ResponseResult> searchRecommendBuTab(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ProspectRecommendCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchRecommendBuTab", "searchRecommendBuTab", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<OrgBusinessUnit> entitiySearchResult = await prospectRecommendImp.searchProspectRecommend(criteria);
                List<OrgBusinessUnit> lst = entitiySearchResult.data;
                SearchResultBase<OrgBusinessUnit> searchResult = new SearchResultBase<OrgBusinessUnit>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchRecommendBuTab", "searchRecommendBuTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchRecommendBuTab", "searchRecommendBuTab", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/recommendbu/" target="_blank">For Page prospect/recommendbu/</a>
        /// <p>Required Paramerer</p>
        /// <p>ProspectId</p>
        /// <p>ProspAccId</p>
        /// <p>FirstName</p>
        /// <p>LastName</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addProspectRecommend")]
        public async Task<ResponseResult> addProspectRecommend(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, ProspectRecommendModel prospectRecommendModel)
        {
            try
            {
                onAfterReceiveRequest("addProspectRecommend", "addProspectRecommend", prospectRecommendModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                prospectRecommendModel.UserProfile = userProfileForBack;
                ProspectRecommend model = null;
                List<ProspectRecommend> list = new List<ProspectRecommend>();
                for (int i = 0; i < prospectRecommendModel.BuIdList.Length; i++)
                {
                    model = await prospectRecommendImp.addProspectRecommend(prospectRecommendModel, i);
                    list.Add(model);
                }
                //ProspectRecommend model = await prospectRecommendImp.addProspectRecommend(prospectRecommendModel);
                SearchResultBase<ProspectRecommend> searchResult = new SearchResultBase<ProspectRecommend>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addProspectRecommend", "addProspectRecommend", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("addProspectRecommend", "addProspectRecommend", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/recommendbu/" target="_blank">For Page prospect/recommendbu/</a>
        /// <p>Required Paramerer</p>
        /// <p>ProspRecommId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("delProspectRecommend")]
        public async Task<ResponseResult> delProspectRecommend(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, ProspectRecommendModel prospectRecommendModel)
        {
            try
            {
                onAfterReceiveRequest("delProspectRecommend", "delProspectRecommend", prospectRecommendModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await prospectRecommendImp.delProspectRecommend(prospectRecommendModel);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("delProspectRecommend", "delProspectRecommend", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("delProspectRecommend", "delProspectRecommend", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/template/" target="_blank">For Page prospect/template/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchTemplateSaResultTab")]
        public async Task<ResponseResult> searchTemplateSaResultTab(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchTemplateSaResultTabCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchTemplateSaResultTab", "searchTemplateSaResultTab", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchTemplateSaResultTabCustom> entitiySearchResult = await recordSaFormImp.searchTemplateSaResultTab(criteria, userProfileForBack);
                List<SearchTemplateSaResultTabCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchTemplateSaResultTabCustom> searchResult = new SearchResultBase<SearchTemplateSaResultTabCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchTemplateSaResultTab", "searchTemplateSaResultTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchTemplateSaResultTab", "searchTemplateSaResultTab", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/template/" target="_blank">For Page prospect/template/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("viewTemplateSaResult")]
        public async Task<ResponseResult> viewTemplateSaResult(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ViewTemplateSaResultCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("viewTemplateSaResult", "viewTemplateSaResult", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<ViewTemplateSaResultCustom> entitiySearchResult = await recordSaFormImp.viewTemplateSaResult(criteria, userProfileForBack);
                List<ViewTemplateSaResultCustom> lst = entitiySearchResult.data;
                foreach(ViewTemplateSaResultCustom v in lst)
                {

                    /*if (v.title.Count != 0)
                    {
                        foreach (TemplateSaTitle t in v.title)
                        {
                            if ("4".Equals(t.AnsValType))
                            {
                                if (!String.IsNullOrEmpty(t.titleColmImagUrl)) {
                                    t.titleColmImagUrl = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_RECORD_SA_FORM_FILE, t.titleColmImagUrl);
                                }
                            }
                        }
                    }*/

                    foreach (RecordSaFormFile f in v.listFile)
                    {

                        if (f.FileId != 0)
                        {
                            f.FileUrl = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_RECORD_SA_FORM_FILE, f.FileId.ToString());
                        }

                    }
                }

                

                SearchResultBase<ViewTemplateSaResultCustom> searchResult = new SearchResultBase<ViewTemplateSaResultCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("viewTemplateSaResult", "viewTemplateSaResult", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("viewTemplateSaResult", "viewTemplateSaResult", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/dbd/" target="_blank">For Page prospect/dbd/</a>
        /// <p>Required Paramerer</p>
        /// <p>ProspectId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updProspectDbdTab")]
        public async Task<ResponseResult> updProspectDbdTab(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, ProspectModel prospectModel)
        {
            try
            {
                onAfterReceiveRequest("updProspectDbdTab", "updProspectDbdTab", prospectModel);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                await prospectImp.updProspectDbdTab(prospectModel, userProfileForBack);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updProspectDbdTab", "updProspectDbdTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("updProspectDbdTab", "updProspectDbdTab", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/basic/" target="_blank">For Page prospect/basic/</a>
        /// <p>Required Paramerer</p>
        /// <p>ProspectId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updProspectBasicTab")]
        public async Task<ResponseResult> updProspectBasicTab(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, UpdateProspectModel updateProspectModel)
        {
            try
            {
                onAfterReceiveRequest("updProspectBasicTab", "updProspectBasicTab", updateProspectModel);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                await prospectImp.updProspectBasicTab(updateProspectModel, userProfileForBack, language);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updProspectBasicTab", "updProspectBasicTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("updProspectBasicTab", "updProspectBasicTab", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/salesorder/index.php" target="_blank">For Page prospect/salesorder/index.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchSaleOrderTab")]
        public async Task<ResponseResult> searchSaleOrderTab(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchSaleOrderTabCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchSaleOrderTab", "searchSaleOrderTab", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchSaleOrderTabCustom> entitiySearchResult = await saleOrderImp.searchSaleOrderTab(criteria, userProfileForBack);
                List<SearchSaleOrderTabCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchSaleOrderTabCustom> searchResult = new SearchResultBase<SearchSaleOrderTabCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchSaleOrderTab", "searchSaleOrderTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchSaleOrderTab", "searchSaleOrderTab", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/template/" target="_blank">For Page prospect/template/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchRecordMeterTab")]
        public async Task<ResponseResult> searchRecordMeterTab(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchRecordMeterTabCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchRecordMeterTab", "searchRecordMeterTab", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchRecordMeterTabCustom> entitiySearchResult = await recordMeterImp.searchRecordMeterTab(criteria);
                List<SearchRecordMeterTabCustom> lst = entitiySearchResult.data;
                foreach (SearchRecordMeterTabCustom v in lst)
                {

                            if (!String.IsNullOrEmpty(v.FileId))
                            {
                                    v.UrlFile = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_RECORD_METER_FILE, v.FileId);
                            }
                }
                SearchResultBase<SearchRecordMeterTabCustom> searchResult = new SearchResultBase<SearchRecordMeterTabCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchRecordMeterTab", "searchRecordMeterTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchRecordMeterTab", "searchRecordMeterTab", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/feed/" target="_blank">For Page prospect/feed/</a>
        /// <p>Required Paramerer</p>
        /// <para>ProspectId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchFeedTab")]
        public async Task<ResponseResult> searchFeedTab(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchFeedTabCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchFeedTab", "searchFeedTab", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchFeedTabCustom> entitiySearchResult = await prospectFeedImp.searchFeedTab(criteria);
                List<SearchFeedTabCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchFeedTabCustom> searchResult = new SearchResultBase<SearchFeedTabCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchFeedTab", "searchFeedTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchFeedTab", "searchFeedTab", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/stockcard/index.php" target="_blank">For Page prospect/stockcard/index.php</a>
        /// <p>Required Paramerer</p>
        /// <para>ProspectId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchStockCountTab")]
        public async Task<ResponseResult> searchStockCountTab(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchStockCountTabCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchStockCountTab", "searchStockCountTab", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<ResponseData> entitiySearchResult = await recordStockCardImp.searchStockCountTab(criteria);
                List<ResponseData> lst = entitiySearchResult.data;
                SearchResultBase<ResponseData> searchResult = new SearchResultBase<ResponseData>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchStockCountTab", "searchStockCountTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchStockCountTab", "searchStockCountTab", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/salesdata/index.php" target="_blank">For Page prospect/salesdata/index.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchSaleDataTab")]
        public async Task<ResponseResult> searchSaleDataTab(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchSaleDataTabCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchSaleDataTab", "searchSaleDataTab", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchSaleDataTabCustom> entitiySearchResult = await customerSaleImp.searchSaleDataTab(criteria);
                List<SearchSaleDataTabCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchSaleDataTabCustom> searchResult = new SearchResultBase<SearchSaleDataTabCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchSaleDataTab", "searchSaleDataTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchSaleDataTab", "searchSaleDataTab", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchPlanTripForSaleVisit")]
        public async Task<ResponseResult> searchPlanTripForSaleVisit(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchPlanTripForSaleVisitCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchPlanTripForSaleVisit", "searchPlanTripForSaleVisit", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchPlanTripForSaleVisitCustom> entitiySearchResult = await planTripImp.searchPlanTripForSaleVisit(criteria, userProfileForBack);
                List<SearchPlanTripForSaleVisitCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchPlanTripForSaleVisitCustom> searchResult = new SearchResultBase<SearchPlanTripForSaleVisitCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchPlanTripForSaleVisit", "searchPlanTripForSaleVisit", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchPlanTripForSaleVisit", "searchPlanTripForSaleVisit", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <p>PlanTripId</p>
        /// <p>StartCheckinLocId</p>
        /// <p>StartCheckinMileNo</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("planTripStart")]
        public async Task<ResponseResult> planTripStart(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, PlanTripModel planTripModel)
        {
            try
            {
                onAfterReceiveRequest("planTripStart", "planTripStart", planTripModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                planTripModel.UserProfile = userProfileForBack;
                await planTripImp.planTripStart(planTripModel);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("planTripStart", "planTripStart", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("planTripStart", "planTripStart", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <p>PlanTripId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("planTripFinish")]
        public async Task<ResponseResult> planTripFinish(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, PlanTripModel planTripModel)
        {
            try
            {
                onAfterReceiveRequest("planTripFinish", "planTripFinish", planTripModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                planTripModel.UserProfile = userProfileForBack;
                // Update table PLAN_TRIP
                await planTripImp.planTripFinish(planTripModel);
                // Get KM_TOTAL
                KmTotalPlanTrip kmTotalPlanTrip = await planTripImp.getKmTotalPlanTripFinish(planTripModel.PlanTripId);
                SearchResultBase<KmTotalPlanTrip> searchResult = new SearchResultBase<KmTotalPlanTrip>();
                List<KmTotalPlanTrip> list = new List<KmTotalPlanTrip>();
                list.Add(kmTotalPlanTrip);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("planTripFinish", "planTripFinish", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("planTripFinish", "planTripFinish", resR);
                return resR;
            }
        }








        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getLastCheckIn")]
        public async Task<ResponseResult> getLastCheckIn(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetLastCheckInCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getLastCheckIn", "getLastCheckIn", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<GetLastCheckInCustom> entitiySearchResult = await planTripProspectImp.getLastCheckIn(criteria);
                List<GetLastCheckInCustom> lst = entitiySearchResult.data;
                SearchResultBase<GetLastCheckInCustom> searchResult = new SearchResultBase<GetLastCheckInCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getLastCheckIn", "getLastCheckIn", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getLastCheckIn", "getLastCheckIn", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getAddressForBestRoute")]
        public async Task<ResponseResult> getAddressForBestRoute(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetAddressForBestRouteCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getAddressForBestRoute", "getAddressForBestRoute", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<GetAddressForBestRouteCustom> entitiySearchResult = await planTripProspectImp.getAddressForBestRoute(criteria);
                List<GetAddressForBestRouteCustom> lst = entitiySearchResult.data;
                SearchResultBase<GetAddressForBestRouteCustom> searchResult = new SearchResultBase<GetAddressForBestRouteCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getAddressForBestRoute", "getAddressForBestRoute", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getAddressForBestRoute", "getAddressForBestRoute", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Plant.php" target="_blank">For Page masterdata/Organizational/Plant.php</a>
        /// <p>Required Paramerer</p>
        /// <para>CompanyCode</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchPlantByCompanyCode_db")]
        public async Task<ResponseResult> searchPlantByCompanyCode_db(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchPlantByCompanyCodeCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchPlantByCompanyCode_db", "searchPlantByCompanyCode_db", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                // Cal use time
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();

                // Begin timing.
                stopwatch.Start();
                //
                EntitySearchResultBase<ZResultCustom> entitiySearchResult = await callAPIImp.Search(criteria);
                // Stop timing.
                stopwatch.Stop();

                // Write result.
                Console.WriteLine("Time elapsed All: "+ stopwatch.Elapsed);
                log.Debug("Time Use All: "+ stopwatch.Elapsed);
                List<ZResultCustom> lst = entitiySearchResult.data;
                SearchResultBase<ZResultCustom> searchResult = new SearchResultBase<ZResultCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                //onBeforeSendResponse("searchPlantByCompanyCode_db", "searchPlantByCompanyCode_db", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Plant.php" target="_blank">For Page masterdata/Organizational/Plant.php</a>
        /// <p>Required Paramerer</p>
        /// <para>CompanyCode</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchPlantByCompanyCode")]
        public async Task<ResponseResult> searchPlantByCompanyCode(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchPlantByCompanyCodeCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchPlantByCompanyCode", "searchPlantByCompanyCode", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                InterfaceSapConfig ic = await getInterfaceSapConfigAsync();
                // Cal use time
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();

                // Begin timing.
                stopwatch.Start();
                //
                EntitySearchResultBase<SearchPlantByCompanyCodeCustom> entitiySearchResult = await callAPIImp.searchPlantByCompanyCode(criteria, ic);
                // Stop timing.
                stopwatch.Stop();

                // Write result.
                Console.WriteLine("Time elapsed All: "+ stopwatch.Elapsed);
                log.Debug("Time Use All: "+ stopwatch.Elapsed);
                List<SearchPlantByCompanyCodeCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchPlantByCompanyCodeCustom> searchResult = new SearchResultBase<SearchPlantByCompanyCodeCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                //onBeforeSendResponse("searchPlantByCompanyCode", "searchPlantByCompanyCode", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/ShippingPoint.php" target="_blank">For Page masterdata/Organizational/ShippingPoint.php</a>
        /// <p>Required Paramerer</p>
        /// <para>PlantCode</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchShippingPointByPlantCode")]
        public async Task<ResponseResult> searchShippingPointByPlantCode(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchShippingPointByPlantCodeCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchShippingPointByPlantCode", "searchShippingPointByPlantCode", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                InterfaceSapConfig ic = await getInterfaceSapConfigAsync();
                // Cal use time
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();

                // Begin timing.
                stopwatch.Start();
                //
                EntitySearchResultBase<SearchShippingPointByPlantCodeCustom> entitiySearchResult = await callAPIImp.searchShippingPointByPlantCode(criteria, ic);
                // Stop timing.
                stopwatch.Stop();

                // Write result.
                Console.WriteLine("Time elapsed All: "+ stopwatch.Elapsed);
                log.Debug("Time Use All: "+ stopwatch.Elapsed);
                List<SearchShippingPointByPlantCodeCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchShippingPointByPlantCodeCustom> searchResult = new SearchResultBase<SearchShippingPointByPlantCodeCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                //onBeforeSendResponse("searchShippingPointByPlantCode", "searchShippingPointByPlantCode", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <p>PlanTripProspId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("checkInPlanTripProspect")]
        public async Task<ResponseResult> checkInPlanTripProspect(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, PlanTripProspectModel planTripProspectModel)
        {
            try
            {
                onAfterReceiveRequest("checkInPlanTripProspect", "checkInPlanTripProspect", planTripProspectModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                planTripProspectModel.UserProfile = userProfileForBack;
                checkInPlanTripProspectData  data = await planTripProspectImp.checkInPlanTripProspect(planTripProspectModel);
                ResponseResult res = ResponseResult.warp(data);
                res.agent = agent;
                onBeforeSendResponse("checkInPlanTripProspect", "checkInPlanTripProspect", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("checkInPlanTripProspect", "checkInPlanTripProspect", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <p>PlanTripProspId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("checkOutPlanTripProspect")]
        public async Task<ResponseResult> checkOutPlanTripProspect(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, CheckOutPlanTripProspectModel checkOutPlanTripProspectModel)
        {
            try
            {
                onAfterReceiveRequest("checkOutPlanTripProspect", "checkOutPlanTripProspect", checkOutPlanTripProspectModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                checkOutPlanTripProspectModel.UserProfile = userProfileForBack;
                await planTripProspectImp.checkOutPlanTripProspect(checkOutPlanTripProspectModel);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("checkOutPlanTripProspect", "checkOutPlanTripProspect", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("checkOutPlanTripProspect", "checkOutPlanTripProspect", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <p>PlanTripProspId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updReasonNotVisitForProspect")]
        public async Task<ResponseResult> updReasonNotVisitForProspect(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, PlanTripProspectModel planTripProspectModel)
        {
            try
            {
                onAfterReceiveRequest("updReasonNotVisitForProspect", "updReasonNotVisitForProspect", planTripProspectModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                planTripProspectModel.UserProfile = userProfileForBack;
                await planTripProspectImp.updReasonNotVisitForProspect(planTripProspectModel);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updReasonNotVisitForProspect", "updReasonNotVisitForProspect", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("updReasonNotVisitForProspect", "updReasonNotVisitForProspect", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <p>PlanTripProspId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updateRemindForProspect")]
        public async Task<ResponseResult> updateRemindForProspect(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, PlanTripProspectModel planTripProspectModel)
        {
            try
            {
                onAfterReceiveRequest("updateRemindForProspect", "updateRemindForProspect", planTripProspectModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                planTripProspectModel.UserProfile = userProfileForBack;
                await planTripProspectImp.updateRemindForProspect(planTripProspectModel);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updateRemindForProspect", "updateRemindForProspect", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("updateRemindForProspect", "updateRemindForProspect", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <p>PlanTripProspId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updateLocRemarkForProspect")]
        public async Task<ResponseResult> updateLocRemarkForProspect(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, UpdateLocRemarkForProspectModel updateLocRemarkForProspectModel)
        {
            try
            {
                onAfterReceiveRequest("updateLocRemarkForProspect", "updateLocRemarkForProspect", updateLocRemarkForProspectModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await planTripProspectImp.updateLocRemarkForProspect(updateLocRemarkForProspectModel, userProfileForBack);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updateLocRemarkForProspect", "updateLocRemarkForProspect", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("updateLocRemarkForProspect", "updateLocRemarkForProspect", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// getConfigLov
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getMasterDataForTemplateSa")]
        public async Task<ResponseResult> getMasterDataForTemplateSa(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetMasterDataForTemplateSaCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getMasterDataForTemplateSa", "getMasterDataForTemplateSa", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<GetMasterDataForTemplateSaCustom> entitiySearchResult = await msConfigLovImp.getMasterDataForTemplateSa(criteria);
                List<GetMasterDataForTemplateSaCustom> lst = entitiySearchResult.data;
                SearchResultBase<GetMasterDataForTemplateSaCustom> searchResult = new SearchResultBase<GetMasterDataForTemplateSaCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getMasterDataForTemplateSa", "getMasterDataForTemplateSa", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getMasterDataForTemplateSa", "getMasterDataForTemplateSa", resR);
                return resR;
            }
        }







        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>checkPercentRangRecordMeter
        /// <para>PlanTripTaskId</para>
        /// <para>ProspId</para>
        /// <para>PlanTripProspId</para>
        /// <para>CustCode</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getTaskMeterForRecord")]
        public async Task<ResponseResult> getTaskMeterForRecord(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetTaskMeterForRecordCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getTaskMeterForRecord", "getTaskMeterForRecord", criteria);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<GetTaskMeterForRecordCustom> entitiySearchResult = await meterImp.getTaskMeterForRecord(criteria);
                List<GetTaskMeterForRecordCustom> lst = entitiySearchResult.data;
                foreach(GetTaskMeterForRecordCustom m in lst)
                {
                    if (!String.IsNullOrEmpty(m.FileId))
                    {
                        m.FileUrl = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_RECORD_METER_FILE, m.FileId.ToString());
                    }
                }
                SearchResultBase<GetTaskMeterForRecordCustom> searchResult = new SearchResultBase<GetTaskMeterForRecordCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getTaskMeterForRecord", "getTaskMeterForRecord", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getTaskMeterForRecord", "getTaskMeterForRecord", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchMeter")]
        public async Task<ResponseResult> searchMeter(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchMeterCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchMeter", "searchMeter", criteria);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchMeterCustom> entitiySearchResult = await meterImp.searchMeter(criteria);
                List<SearchMeterCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchMeterCustom> searchResult = new SearchResultBase<SearchMeterCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchMeter", "searchMeter", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchMeter", "searchMeter", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <para>MeterId</para>
        /// <para>PlanTripTaskId</para>
        /// <para>RecRunNo</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("checkPercentRangRecordMeter")]
        public async Task<ResponseResult> checkPercentRangRecordMeter(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<CheckPercentRangRecordMeterCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("checkPercentRangRecordMeter", "checkPercentRangRecordMeter", criteria);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<CheckPercentRangRecordMeterCustom> entitiySearchResult = await meterImp.checkPercentRangRecordMeter(criteria, language);
                List<CheckPercentRangRecordMeterCustom> lst = entitiySearchResult.data;
                SearchResultBase<CheckPercentRangRecordMeterCustom> searchResult = new SearchResultBase<CheckPercentRangRecordMeterCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("checkPercentRangRecordMeter", "checkPercentRangRecordMeter", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("checkPercentRangRecordMeter", "checkPercentRangRecordMeter", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addRecordMeter")]
        public async Task<ResponseResult> addRecordMeter(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, List<RecordMeterModel> recordMeterModelList)
        {
            try
            {
                onAfterReceiveRequest("addRecordMeter", "addRecordMeter", recordMeterModelList);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                List<RecordMeter> list = await meterImp.addRecordMeter(recordMeterModelList, userProfileForBack);
                SearchResultBase<RecordMeter> searchResult = new SearchResultBase<RecordMeter>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addRecordMeter", "addRecordMeter", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("addRecordMeter", "addRecordMeter", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("viewPlanTrip")]
        public async Task<ResponseResult> viewPlanTrip(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ViewPlanTripCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("viewPlanTrip", "viewPlanTrip", criteria);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<ViewPlanTripCustom> entitiySearchResult = await planTripImp.viewPlanTrip(criteria);
                List<ViewPlanTripCustom> lst = entitiySearchResult.data;
                SearchResultBase<ViewPlanTripCustom> searchResult = new SearchResultBase<ViewPlanTripCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("viewPlanTrip", "viewPlanTrip", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("viewPlanTrip", "viewPlanTrip", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <para>PlanTripProspId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("viewPlanTripTask")]
        public async Task<ResponseResult> viewPlanTripTask(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ViewPlanTripTaskCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("viewPlanTripTask", "viewPlanTripTask", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<PlanTripTask> entitiySearchResult = await planTripTaskImp.viewPlanTripTask(criteria);
                List<PlanTripTask> lst = entitiySearchResult.data;
                SearchResultBase<PlanTripTask> searchResult = new SearchResultBase<PlanTripTask>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("viewPlanTripTask", "viewPlanTripTask", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("viewPlanTrip", "viewPlanTrip", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <para>PlanTripId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getProspectForCreatePlanTripAdHoc")]
        public async Task<ResponseResult> getProspectForCreatePlanTripAdHoc(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetProspectForCreatePlanTripAdHocCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getProspectForCreatePlanTripAdHoc", "getProspectForCreatePlanTripAdHoc", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<GetProspectForCreatePlanTripAdHocCustom> entitiySearchResult = await prospectAccountImp.getProspectForCreatePlanTripAdHoc(criteria, userProfileForBack);
                List<GetProspectForCreatePlanTripAdHocCustom> lst = entitiySearchResult.data;
                SearchResultBase<GetProspectForCreatePlanTripAdHocCustom> searchResult = new SearchResultBase<GetProspectForCreatePlanTripAdHocCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getProspectForCreatePlanTripAdHoc", "getProspectForCreatePlanTripAdHoc", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getProspectForCreatePlanTripAdHoc", "getProspectForCreatePlanTripAdHoc", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <p>PlanTripId</p>
        /// <p>ProspId</p>
        /// <p>PlanStartTime</p>
        /// <p>PlanEndTime</p>
        /// <p>OrderNo</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addPlanTripProspectAdHoc")]
        public async Task<ResponseResult> addPlanTripProspectAdHoc(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, PlanTripProspectModel recordMeterModel)
        {
            try
            {
                onAfterReceiveRequest("addPlanTripProspectAdHoc", "addPlanTripProspectAdHoc", recordMeterModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                recordMeterModel.UserProfile = userProfileForBack;
                PlanTripProspect model = await planTripProspectImp.addPlanTripProspectAdHoc(recordMeterModel);
                SearchResultBase<PlanTripProspect> searchResult = new SearchResultBase<PlanTripProspect>();
                List<PlanTripProspect> list = new List<PlanTripProspect>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addPlanTripProspectAdHoc", "addPlanTripProspectAdHoc", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("addPlanTripProspectAdHoc", "addPlanTripProspectAdHoc", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getTaskTemplateAppFormForCreatPlan")]
        public async Task<ResponseResult> getTaskTemplateAppFormForCreatPlan(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetTaskTemplateAppFormForCreatPlanCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getTaskTemplateAppFormForCreatPlan", "getTaskTemplateAppFormForCreatPlan", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<GetTaskTemplateAppFormForCreatPlanCustom> entitiySearchResult = await templateAppFormImp.getTaskTemplateAppFormForCreatPlan(criteria, userProfileForBack);
                List<GetTaskTemplateAppFormForCreatPlanCustom> lst = entitiySearchResult.data;
                SearchResultBase<GetTaskTemplateAppFormForCreatPlanCustom> searchResult = new SearchResultBase<GetTaskTemplateAppFormForCreatPlanCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getTaskTemplateAppFormForCreatPlan", "getTaskTemplateAppFormForCreatPlan", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getTaskTemplateAppFormForCreatPlan", "getTaskTemplateAppFormForCreatPlan", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getTaskSpecialForCreatPlan")]
        public async Task<ResponseResult> getTaskSpecialForCreatPlan(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetTaskSpecialForCreatPlanCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getTaskSpecialForCreatPlan", "getTaskSpecialForCreatPlan", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<GetTaskSpecialForCreatPlanCustom> entitiySearchResult = await templateAppFormImp.getTaskSpecialForCreatPlan(criteria, userProfileForBack);
                List<GetTaskSpecialForCreatPlanCustom> lst = entitiySearchResult.data;
                SearchResultBase<GetTaskSpecialForCreatPlanCustom> searchResult = new SearchResultBase<GetTaskSpecialForCreatPlanCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getTaskSpecialForCreatPlan", "getTaskSpecialForCreatPlan", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getTaskSpecialForCreatPlan", "getTaskSpecialForCreatPlan", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <p>PlanTripId</p>
        /// <p>ProspId</p>
        /// <p>PlanStartTime</p>
        /// <p>PlanEndTime</p>
        /// <p>OrderNo</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addPlanTripTaskAdHoc")]
        public async Task<ResponseResult> addPlanTripTaskAdHoc(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, PlanTripTaskModel planTripTaskModel)
        {
            try
            {
                onAfterReceiveRequest("addPlanTripTaskAdHoc", "addPlanTripTaskAdHoc", planTripTaskModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                planTripTaskModel.UserProfile = userProfileForBack;
                PlanTripTask model = await planTripTaskImp.addPlanTripTaskAdHoc(planTripTaskModel);
                SearchResultBase<PlanTripTask> searchResult = new SearchResultBase<PlanTripTask>();
                List<PlanTripTask> list = new List<PlanTripTask>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addPlanTripTaskAdHoc", "addPlanTripTaskAdHoc", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("addPlanTripTaskAdHoc", "addPlanTripTaskAdHoc", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addRecordStockCard")]
        public async Task<ResponseResult> addRecordStockCard(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, List<RecordStockCardModel> recordMeterModelList)
        {
            try
            {
                string Host = HttpContext.Request.Headers["Host"];
                onAfterReceiveRequest("addRecordStockCard", "addRecordStockCard", recordMeterModelList);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                List<RecordStockCard> list = await recordStockCardImp.addRecordStockCard(recordMeterModelList, userProfileForBack);
                SearchResultBase<RecordStockCard> searchResult = new SearchResultBase<RecordStockCard>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addRecordStockCard", "addRecordStockCard", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("addRecordStockCard", "addRecordStockCard", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <para>PlanTripTaskId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getTaskTemplateAppFormForRecord")]
        public async Task<ResponseResult> getTaskTemplateAppFormForRecord(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetTaskTemplateAppFormForRecordCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getTaskTemplateAppFormForRecord", "getTaskTemplateAppFormForRecord", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                ResponseResult res = null;

                GetTaskTemplateAppFormForRecordResult result = await templateAppFormImp.getTaskTemplateAppFormForRecord(criteria, userProfileForBack);
                if (!String.IsNullOrEmpty(result.ValRecAppFormId))
                {
                    SearchCriteriaBase<ViewSurveyResultCriteria> criteriaSurvey = new SearchCriteriaBase<ViewSurveyResultCriteria>();
                    ViewSurveyResultCriteria criSurvey = new ViewSurveyResultCriteria();
                    criSurvey.RceAppFormId = result.ValRecAppFormId;
                    criteriaSurvey.model = criSurvey;
                    EntitySearchResultBase<ViewSurveyResultCustom> entitiySearchResult = await recordAppFormImp.viewSurveyResult(criteriaSurvey, userProfileForBack);
                    List<ViewSurveyResultCustom> lst = entitiySearchResult.data;
                    if (lst != null && lst.Count != 0)
                    {
                        for (int i = 0; i < lst.Count; i++)
                        {
                            List<RecordAppFormFile> ListFile = lst[i].ListFile;
                            if (ListFile != null && ListFile.Count != 0)
                            {
                                for (int j = 0; j < ListFile.Count; j++)
                                {
                                    RecordAppFormFile raf = ListFile[j];
                                    raf.FileUrl = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_RECORD_APP_FORM_FILE, raf.FileId.ToString());

                                }
                            }
                        }
                    }



                    SearchResultBase<ViewSurveyResultCustom> searchResult = new SearchResultBase<ViewSurveyResultCustom>();
                    searchResult.totalRecords = entitiySearchResult.totalRecords;
                    searchResult.pageNo = criteria.pageNo;
                    searchResult.recordPerPage = criteria.length;
                    searchResult.records = lst;
                    searchResult.recordStart = criteria.startRecord;
                    searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));
                    res = ResponseResult.warp(searchResult);
                }
                else
                {

                    EntitySearchResultBase<GetTaskTemplateAppFormForRecordCustom> entitiySearchResult = result.Result;
                    List<GetTaskTemplateAppFormForRecordCustom> lst = entitiySearchResult.data;

                    if (lst != null && lst.Count != 0)
                    {
                        for (int i = 0; i < lst.Count; i++)
                        {
                            List<RecordAppFormFile> ListFile = lst[i].ListFile;
                            if (ListFile != null && ListFile.Count != 0)
                            {
                                for (int j = 0; j < ListFile.Count; j++)
                                {
                                    RecordAppFormFile raf = ListFile[j];
                                    raf.FileUrl = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_RECORD_APP_FORM_FILE, raf.FileId.ToString());

                                }
                            }
                        }
                    }

                    SearchResultBase<GetTaskTemplateAppFormForRecordCustom> searchResult = new SearchResultBase<GetTaskTemplateAppFormForRecordCustom>();
                    searchResult.totalRecords = entitiySearchResult.totalRecords;
                    searchResult.pageNo = criteria.pageNo;
                    searchResult.recordPerPage = criteria.length;
                    searchResult.records = lst;
                    searchResult.recordStart = criteria.startRecord;
                    searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));
                    res = ResponseResult.warp(searchResult);
                }
                
                res.agent = agent;
                onBeforeSendResponse("getTaskTemplateAppFormForRecord", "getTaskTemplateAppFormForRecord", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getTaskTemplateAppFormForRecord", "getTaskTemplateAppFormForRecord", resR);
                return resR;
            }
        }






        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <para>PlanTripTaskId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getTaskTemplateSaFormForRecord")]
        public async Task<ResponseResult> getTaskTemplateSaFormForRecord(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetTaskTemplateSaFormForRecordCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getTaskTemplateSaFormForRecord", "getTaskTemplateSaFormForRecord", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                ResponseResult res = null;

                GetTaskTemplateSaFormForRecordResult result = await templateSaFormImp.getTaskTemplateSaFormForRecord(criteria, userProfileForBack);
                if (!String.IsNullOrEmpty(result.ValRecSaFormId))
                {
                    SearchCriteriaBase<ViewTemplateSaResultCriteria> criteriaTemplateSaResult = new SearchCriteriaBase<ViewTemplateSaResultCriteria>();
                    ViewTemplateSaResultCriteria criTemplateSaResult = new ViewTemplateSaResultCriteria();
                    criTemplateSaResult.RecSaFormId = result.ValRecSaFormId;
                    criteriaTemplateSaResult.model = criTemplateSaResult;
                    EntitySearchResultBase<ViewTemplateSaResultCustom> entitiySearchResult = await recordSaFormImp.viewTemplateSaResult(criteriaTemplateSaResult, userProfileForBack);
                    List<ViewTemplateSaResultCustom> lst = entitiySearchResult.data;
                    if (lst != null && lst.Count != 0)
                    {
                        for (int i = 0; i < lst.Count; i++)
                        {
                            List<RecordSaFormFile> ListFile = lst[i].listFile;
                            if (ListFile != null && ListFile.Count != 0)
                            {
                                for (int j = 0; j < ListFile.Count; j++)
                                {
                                    RecordSaFormFile raf = ListFile[j];
                                    raf.FileUrl = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_RECORD_SA_FORM_FILE, raf.FileId.ToString());

                                }
                            }
                            List<TemplateSaTitle> ListTitle = lst[i].title;
                            if (ListTitle != null && ListTitle.Count != 0)
                            {
                                foreach (TemplateSaTitle t in ListTitle)
                                {
                                    if (!String.IsNullOrEmpty(t.titleColmImagUrl))
                                    {
                                        t.titleColmImagUrl = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_RECORD_SA_FORM_FILE, t.titleColmImagUrl);
                                    }
                                }
                            }
                        }

                    }




                    SearchResultBase<ViewTemplateSaResultCustom> searchResult = new SearchResultBase<ViewTemplateSaResultCustom>();
                    searchResult.totalRecords = entitiySearchResult.totalRecords;
                    searchResult.pageNo = criteria.pageNo;
                    searchResult.recordPerPage = criteria.length;
                    searchResult.records = lst;
                    searchResult.recordStart = criteria.startRecord;
                    searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));
                    res = ResponseResult.warp(searchResult);
                }
                else
                {

                    EntitySearchResultBase<GetTaskTemplateSaFormForRecordCustom> entitiySearchResult = result.Result;
                    List<GetTaskTemplateSaFormForRecordCustom> lst = entitiySearchResult.data;


                    if (lst != null && lst.Count != 0)
                    {
                        for (int i = 0; i < lst.Count; i++)
                        {
                            List<RecordSaFormFile> ListFile = lst[i].ListFile;
                            if (ListFile != null && ListFile.Count != 0)
                            {
                                for (int j = 0; j < ListFile.Count; j++)
                                {
                                    RecordSaFormFile raf = ListFile[j];
                                    raf.FileUrl = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_RECORD_SA_FORM_FILE, raf.FileId.ToString());

                                }
                            }
                            List<TemplateSaTitle> ListTitle = lst[i].Title;
                            if (ListTitle != null && ListTitle.Count != 0)
                            {
                                foreach (TemplateSaTitle t in ListTitle)
                                {
                                    if (!String.IsNullOrEmpty(t.titleColmImagUrl))
                                    {
                                        t.titleColmImagUrl = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_RECORD_SA_FORM_FILE, t.titleColmImagUrl);
                                    }
                                }
                            }
                        }

                    }


                    SearchResultBase<GetTaskTemplateSaFormForRecordCustom> searchResult = new SearchResultBase<GetTaskTemplateSaFormForRecordCustom>();
                    searchResult.totalRecords = entitiySearchResult.totalRecords;
                    searchResult.pageNo = criteria.pageNo;
                    searchResult.recordPerPage = criteria.length;
                    searchResult.records = lst;
                    searchResult.recordStart = criteria.startRecord;
                    searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));
                    res = ResponseResult.warp(searchResult);
                }

                res.agent = agent;
                onBeforeSendResponse("getTaskTemplateSaFormForRecord", "getTaskTemplateSaFormForRecord", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getTaskTemplateSaFormForRecord", "getTaskTemplateSaFormForRecord", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addRecordAppForm")]
        public async Task<ResponseResult> addRecordAppForm(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, AddRecordAppFormModel addRecordAppFormModel)
        {
            try
            {
                onAfterReceiveRequest("addRecordAppForm", "addRecordAppForm", addRecordAppFormModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                RecordAppForm model = await templateAppFormImp.addRecordAppForm(addRecordAppFormModel, userProfileForBack);
                SearchResultBase<RecordAppForm> searchResult = new SearchResultBase<RecordAppForm>();
                List<RecordAppForm> list = new List<RecordAppForm>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addRecordAppForm", "addRecordAppForm", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("addRecordAppForm", "addRecordAppForm", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/otherprospect.php" target="_blank">For Page prospect/otherprospect.php</a>
        /// <p>Required Paramerer</p>
        /// <para>ProspectId</para>
        /// <para>FunctionTab[]</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cloneProspect")]
        public async Task<ResponseResult> cloneProspect(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, CloneProspectModel cloneProspectModel)
        {
            try
            {
                onAfterReceiveRequest("cloneProspect", "cloneProspect", cloneProspectModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                Prospect model = await prospectImp.cloneProspect(cloneProspectModel, userProfileForBack, language);
                SearchResultBase<Prospect> searchResult = new SearchResultBase<Prospect>();
                List<Prospect> list = new List<Prospect>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cloneProspect", "cloneProspect", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cloneProspect", "cloneProspect", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/attachments/" target="_blank">For Page prospect/attachments/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchAttachmentTab")]
        public async Task<ResponseResult> searchAttachmentTab(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchAttachmentTabCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAttachmentTab", "searchAttachmentTab", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchAttachmentTabCustom> entitiySearchResult = await recordAppFormFileImp.searchAttachmentTab(criteria);
                List<SearchAttachmentTabCustom> lst = entitiySearchResult.data;

                if (lst != null && lst.Count != 0)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        List<RecordAppFormFile> ListFile = lst[i].RecordAppFormFileLst;
                        for (int j = 0; j < ListFile.Count; j++)
                        {
                            RecordAppFormFile raf = ListFile[j];
                            raf.FileUrl = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_RECORD_APP_FORM_FILE, raf.FileId.ToString());

                        }
                    }
                }
                SearchResultBase<SearchAttachmentTabCustom> searchResult = new SearchResultBase<SearchAttachmentTabCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchAttachmentTab", "searchAttachmentTab", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchAttachmentTab", "searchAttachmentTab", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <para>PlanTripTaskId</para>
        /// <para>TpStockCardId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getTaskStockCardForRecord")]
        public async Task<ResponseResult> getTaskStockCardForRecord(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetTaskStockCardForRecordCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getTaskStockCardForRecord", "getTaskStockCardForRecord", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<GetTaskStockCardForRecordCustom.Product> entitiySearchResult = await templateStockCardImp.getTaskStockCardForRecord(criteria);
                List<GetTaskStockCardForRecordCustom.Product> lst = entitiySearchResult.data;
                SearchResultBase<GetTaskStockCardForRecordCustom.Product> searchResult = new SearchResultBase<GetTaskStockCardForRecordCustom.Product>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getTaskStockCardForRecord", "getTaskStockCardForRecord", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getTaskStockCardForRecord", "getTaskStockCardForRecord", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addRecordSaForm")]
        public async Task<ResponseResult> addRecordSaForm(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, AddRecordSaFormModel addRecordSaFormModel)
        {
            try
            {
                onAfterReceiveRequest("addRecordSaForm", "addRecordSaForm", addRecordSaFormModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                RecordSaForm model = await templateSaFormImp.addRecordSaForm(addRecordSaFormModel, userProfileForBack);
                SearchResultBase<RecordSaForm> searchResult = new SearchResultBase<RecordSaForm>();
                List<RecordSaForm> list = new List<RecordSaForm>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addRecordSaForm", "addRecordSaForm", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("addRecordSaForm", "addRecordSaForm", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisitplan/" target="_blank">For Page salesvisitplan/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchPlanTrip")]
        public async Task<ResponseResult> searchPlanTrip(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchPlanTripCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchPlanTrip", "searchPlanTrip", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<PlanTrip> entitiySearchResult = await planTripImp.searchPlanTrip(criteria, userProfileForBack);
                List<PlanTrip> lst = entitiySearchResult.data;
                SearchResultBase<PlanTrip> searchResult = new SearchResultBase<PlanTrip>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchPlanTrip", "searchPlanTrip", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchPlanTrip", "searchPlanTrip", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisitplan/createplan/" target="_blank">For Page salesvisitplan/createplan/</a>
        /// <p>Required Paramerer</p>
        /// <para>PlanTripId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getProspectForCreatePlanTrip")]
        public async Task<ResponseResult> getProspectForCreatePlanTrip(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetProspectForCreatePlanTripCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getProspectForCreatePlanTrip", "getProspectForCreatePlanTrip", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<GetProspectForCreatePlanTripCustom> entitiySearchResult = await prospectAccountImp.getProspectForCreatePlanTrip(criteria, userProfileForBack);
                List<GetProspectForCreatePlanTripCustom> lst = entitiySearchResult.data;
                SearchResultBase<GetProspectForCreatePlanTripCustom> searchResult = new SearchResultBase<GetProspectForCreatePlanTripCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getProspectForCreatePlanTrip", "getProspectForCreatePlanTrip", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getProspectForCreatePlanTrip", "getProspectForCreatePlanTrip", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisitplan/createplan/" target="_blank">For Page salesvisitplan/createplan/</a>
        /// <p>Required Paramerer</p>
        /// <para>PlanTripId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getEmpForAssignPlanTrip")]
        public async Task<ResponseResult> getEmpForAssignPlanTrip(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetEmpForAssignPlanTripCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getEmpForAssignPlanTrip", "getEmpForAssignPlanTrip", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<AdmEmployee> entitiySearchResult = await planTripImp.getEmpForAssignPlanTrip(criteria, userProfileForBack);
                List<AdmEmployee> lst = entitiySearchResult.data;
                SearchResultBase<AdmEmployee> searchResult = new SearchResultBase<AdmEmployee>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getEmpForAssignPlanTrip", "getEmpForAssignPlanTrip", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getEmpForAssignPlanTrip", "getEmpForAssignPlanTrip", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisitplan/createplan/ " target="_blank">For Page salesvisitplan/createplan/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("createPlanTrip")]
        public async Task<ResponseResult> createPlanTrip(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, CreatePlanTripModel createPlanTripModel)
        {
            try
            {
                onAfterReceiveRequest("createPlanTrip", "createPlanTrip", createPlanTripModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                PlanTrip model = await planTripImp.createPlanTrip(createPlanTripModel, userProfileForBack);
                // Send Mail
                string EMP_ID = null, Subject="", Body="", SenderEmail, ReceiveEmail, EmailTemplate=null, SenderName = userProfileForBack.UserProfileCustom.data[0].FirstName +" "+ userProfileForBack.UserProfileCustom.data[0].LastName;
                string WAITING_FOR_APPROVE = PlanTripStatus.WAITING_FOR_APPROVE.ToString("d");
                string ASSIGN = PlanTripStatus.ASSIGN.ToString("d");
                if (WAITING_FOR_APPROVE.Equals(createPlanTripModel.PlanTrip.Status))
                {
                    EmailTemplate = EmailTemplateStatus.WAITING_FOR_APPROVE.ToString("d");
                    EMP_ID = userProfileForBack.getEmpId();
                    Subject = "Plan Trip : Waiting for approve";
                    Body = "Waiting for approve : มี Plan Trip Name [Name : "+ createPlanTripModel.PlanTrip.PlanTripName+"] จาก [Employee Name : "+ SenderName+ "] ส่งถึงท่านรอการอนุมัติ";
                }else if (ASSIGN.Equals(createPlanTripModel.PlanTrip.Status))
                {
                    EmailTemplate = EmailTemplateStatus.ASSIGN.ToString("d");
                    EMP_ID = createPlanTripModel.PlanTrip.AssignEmpId;
                    Subject = "Plan Trip : Assigned";
                    Body = "Assign : มี Plan Trip Name [Name : " + createPlanTripModel.PlanTrip.PlanTripName + "] จาก [Employee Name :" + SenderName + "] ส่งถึงท่านเพื่อดำเนินการ";
                }
                if (EMP_ID != null)
                {
                    List<GetEmailToSendForCreatePlantripCustom> emailList = await planTripImp.getEmailToSendForCreatePlantrip(EMP_ID);
                    if (emailList != null && emailList.Count != 0)
                    {
                        foreach(GetEmailToSendForCreatePlantripCustom e in emailList)
                        {
                            SenderEmail = e.SenderEmail;
                            if (ASSIGN.Equals(createPlanTripModel.PlanTrip.Status))
                            {
                                ReceiveEmail = e.ReceiveEmailAssign;
                            }
                            else
                            {
                                ReceiveEmail = e.ReceiveEmail;
                            }
                            SendMailModel sendMailModel = new SendMailModel();
                            sendMailModel.SenderName = SenderName;
                            sendMailModel.MailTo = new List<string> { ReceiveEmail };
                            sendMailModel.showAllRecipients = false;
                            sendMailModel.Subject = Subject;
                            sendMailModel.Content = Body;
                            if (ASSIGN.Equals(createPlanTripModel.PlanTrip.Status))
                            {
                                string StartDateTime = createPlanTripModel.PlanTrip.PlanTripDate;
                                string EndDateTime = createPlanTripModel.PlanTrip.PlanTripDate;
                                MeetingModel meeting = new MeetingModel();
                                meeting.Organizer = SenderEmail;
                                meeting.Subject = Subject;
                                meeting.Description = Body;
                                meeting.StartDateTime = StartDateTime;
                                meeting.EndDateTime = EndDateTime;
                                sendMailModel.Meeting = meeting;
                            }
                            bool sendEmailSuccess = false;
                            string JOB_STATUS = "S";
                            string ERROR_DESC = "";
                            int JOB_STATUS_FAIL_COUNT = 0;
                            try
                            {
                                SendMailCustom sendMailCustom = await SendEmail(language, sendMailModel, SenderEmail, sendGridClient, logger);
                                sendEmailSuccess = true;
                                log.Info("Send Email Success!.");
                                log.Info("Send Email Success!. System will Record to Email Job............");
                                Console.WriteLine("Send Email Success!.");
                                Console.WriteLine("Send Email Success!. System will Record to Email Job............");
                                string OBJ_EMAIL = JsonConvert.SerializeObject(sendMailModel);// Convert Object For Send Email -> JsonString
                                log.Info("OBJ_EMAIL :" + OBJ_EMAIL);
                                Console.WriteLine("OBJ_EMAIL :" + OBJ_EMAIL);
                                decimal TABLE_REF_KEY_ID = model.PlanTripId;
                                EmailJobModel emailJobModel = new EmailJobModel();
                                emailJobModel.EmailTemplate = EmailTemplate;
                                emailJobModel.ObjEmail = OBJ_EMAIL;
                                emailJobModel.TableRefKeyId = TABLE_REF_KEY_ID;
                                emailJobModel.JobStatus = JOB_STATUS;
                                emailJobModel.JobStatusFailCount = JOB_STATUS_FAIL_COUNT;
                                emailJobModel.ErrorDesc = ERROR_DESC;
                                EmailJob emailJob = await emailJobImp.Add(emailJobModel, userProfileForBack);
                                log.Info("SSystem Record to Email Job Complete............");
                                Console.WriteLine("SSystem Record to Email Job Complete............");
                            }
                            catch(Exception ex)
                            {
                                if (!sendEmailSuccess)
                                {
                                    JOB_STATUS = "F";
                                    ERROR_DESC = ex.Message + ":" + ex.ToString(); ;
                                    JOB_STATUS_FAIL_COUNT = 1;
                                    log.Info("Send Email Fail!. :" + ex.Message);
                                    log.Info("Send Email Fail!. System will Record to Email Job............");
                                    Console.WriteLine("Send Email Fail!. :" + ex.Message);
                                    Console.WriteLine("Send Email Fail!. System will Record to Email Job............");
                                    string OBJ_EMAIL = JsonConvert.SerializeObject(sendMailModel);// Convert Object For Send Email -> JsonString
                                    log.Info("OBJ_EMAIL :" + OBJ_EMAIL);
                                    Console.WriteLine("OBJ_EMAIL :" + OBJ_EMAIL);
                                    decimal TABLE_REF_KEY_ID = model.PlanTripId;
                                    EmailJobModel emailJobModel = new EmailJobModel();
                                    emailJobModel.EmailTemplate = EmailTemplate;
                                    emailJobModel.ObjEmail = OBJ_EMAIL;
                                    emailJobModel.TableRefKeyId = TABLE_REF_KEY_ID;
                                    emailJobModel.JobStatus = JOB_STATUS;
                                    emailJobModel.JobStatusFailCount = JOB_STATUS_FAIL_COUNT;
                                    emailJobModel.ErrorDesc = ERROR_DESC;
                                    EmailJob emailJob = await emailJobImp.Add(emailJobModel, userProfileForBack);
                                    log.Info("SSystem Record to Email Job Complete............");
                                    Console.WriteLine("SSystem Record to Email Job Complete............");
                                }
                            }
                        }
                    }
                }
                //

                SearchResultBase<PlanTrip> searchResult = new SearchResultBase<PlanTrip>();
                List<PlanTrip> list = new List<PlanTrip>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("createPlanTrip", "createPlanTrip", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("createPlanTrip", "createPlanTrip", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisitplan/createplan/ " target="_blank">For Page salesvisitplan/createplan/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updPlanTrip")]
        public async Task<ResponseResult> updPlanTrip(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, UpdatePlanTripModel updatePlanTripModel)
        {
            try
            {
                onAfterReceiveRequest("updPlanTrip", "updPlanTrip", updatePlanTripModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await planTripImp.updPlanTrip(updatePlanTripModel, userProfileForBack);
                // Send Mail
                string WAITING_FOR_APPROVE = PlanTripStatus.WAITING_FOR_APPROVE.ToString("d");
                if (WAITING_FOR_APPROVE.Equals(updatePlanTripModel.PlanTrip.Status))
                {
                    string EMP_ID = null, Subject = "", Body = "", SenderEmail, ReceiveEmail, SenderName = userProfileForBack.UserProfileCustom.data[0].FirstName + " " + userProfileForBack.UserProfileCustom.data[0].LastName;
                    string EmailTemplate = EmailTemplateStatus.WAITING_FOR_APPROVE.ToString("d");
                    EMP_ID = userProfileForBack.getEmpId();
                    Subject = "Plan Trip : Waiting for approve";
                    Body = "Waiting for approve : มี Plan Trip Name [Name : " + updatePlanTripModel.PlanTrip.PlanTripName + "] จาก [Employee Name : " + SenderName + "] ส่งถึงท่านรอการอนุมัติ";

                    List<GetEmailToSendForUpdatePlantripCustom> emailList = await planTripImp.getEmailToSendForUpdatePlantrip(EMP_ID);
                    if (emailList != null && emailList.Count != 0)
                    {
                        foreach (GetEmailToSendForUpdatePlantripCustom e in emailList)
                        {
                            SenderEmail = e.SenderEmail;
                            ReceiveEmail = e.ReceiveEmail;
                            SendMailModel sendMailModel = new SendMailModel();
                            sendMailModel.SenderName = SenderName;
                            sendMailModel.MailTo = new List<string> { ReceiveEmail };
                            sendMailModel.showAllRecipients = false;
                            sendMailModel.Subject = Subject;
                            sendMailModel.Content = Body;
                            bool sendEmailSuccess = false;
                            string JOB_STATUS = "S";
                            string ERROR_DESC = "";
                            int JOB_STATUS_FAIL_COUNT = 0;
                            try
                            {
                                SendMailCustom sendMailCustom = await SendEmail(language, sendMailModel, SenderEmail, sendGridClient, logger);
                                sendEmailSuccess = true;
                                log.Info("Send Email Success!.");
                                log.Info("Send Email Success!. System will Record to Email Job............");
                                Console.WriteLine("Send Email Success!.");
                                Console.WriteLine("Send Email Success!. System will Record to Email Job............");
                                string OBJ_EMAIL = JsonConvert.SerializeObject(sendMailModel);// Convert Object For Send Email -> JsonString
                                log.Info("OBJ_EMAIL :" + OBJ_EMAIL);
                                Console.WriteLine("OBJ_EMAIL :" + OBJ_EMAIL);
                                decimal TABLE_REF_KEY_ID = Convert.ToDecimal(updatePlanTripModel.PlanTrip.PlanTripId);
                                EmailJobModel emailJobModel = new EmailJobModel();
                                emailJobModel.EmailTemplate = EmailTemplate;
                                emailJobModel.ObjEmail = OBJ_EMAIL;
                                emailJobModel.TableRefKeyId = TABLE_REF_KEY_ID;
                                emailJobModel.JobStatus = JOB_STATUS;
                                emailJobModel.JobStatusFailCount = JOB_STATUS_FAIL_COUNT;
                                emailJobModel.ErrorDesc = ERROR_DESC;
                                EmailJob emailJob = await emailJobImp.Add(emailJobModel, userProfileForBack);
                                log.Info("SSystem Record to Email Job Complete............");
                                Console.WriteLine("SSystem Record to Email Job Complete............");
                            }
                            catch (Exception ex)
                            {
                                if (!sendEmailSuccess)
                                {
                                    JOB_STATUS = "F";
                                    ERROR_DESC = ex.Message + ":" + ex.ToString(); ;
                                    JOB_STATUS_FAIL_COUNT = 1;
                                    log.Info("Send Email Fail!. :" + ex.Message);
                                    log.Info("Send Email Fail!. System will Record to Email Job............");
                                    Console.WriteLine("Send Email Fail!. :" + ex.Message);
                                    Console.WriteLine("Send Email Fail!. System will Record to Email Job............");
                                    string OBJ_EMAIL = JsonConvert.SerializeObject(sendMailModel);// Convert Object For Send Email -> JsonString
                                    log.Info("OBJ_EMAIL :" + OBJ_EMAIL);
                                    Console.WriteLine("OBJ_EMAIL :" + OBJ_EMAIL);
                                    decimal TABLE_REF_KEY_ID = Convert.ToDecimal(updatePlanTripModel.PlanTrip.PlanTripId);
                                    EmailJobModel emailJobModel = new EmailJobModel();
                                    emailJobModel.EmailTemplate = EmailTemplate;
                                    emailJobModel.ObjEmail = OBJ_EMAIL;
                                    emailJobModel.TableRefKeyId = TABLE_REF_KEY_ID;
                                    emailJobModel.JobStatus = JOB_STATUS;
                                    emailJobModel.JobStatusFailCount = JOB_STATUS_FAIL_COUNT;
                                    emailJobModel.ErrorDesc = ERROR_DESC;
                                    EmailJob emailJob = await emailJobImp.Add(emailJobModel, userProfileForBack);
                                    log.Info("SSystem Record to Email Job Complete............");
                                    Console.WriteLine("SSystem Record to Email Job Complete............");
                                }
                            }
                        }
                    }
                }
                //
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updPlanTrip", "updPlanTrip", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("updPlanTrip", "updPlanTrip", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisitplan/createplan/ " target="_blank">For Page salesvisitplan/createplan/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("mergPlanTrip")]
        public async Task<ResponseResult> mergPlanTrip(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MergPlanTripModel mergPlanTripModel)
        {
            try
            {
                onAfterReceiveRequest("mergPlanTrip", "mergPlanTrip", mergPlanTripModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await planTripImp.mergPlanTrip(mergPlanTripModel, userProfileForBack, language);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("mergPlanTrip", "mergPlanTrip", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("mergPlanTrip", "mergPlanTrip", resR);
                return resR;
            }
        }



        
        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisitplan/createplan/ " target="_blank">For Page salesvisitplan/createplan/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelPlanTrip")]
        public async Task<ResponseResult> cancelPlanTrip(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, CancelPlanTripModel cancelPlanTripModel)
        {
            try
            {
                onAfterReceiveRequest("cancelPlanTrip", "cancelPlanTrip", cancelPlanTripModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await planTripImp.cancelPlanTrip(cancelPlanTripModel, userProfileForBack);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelPlanTrip", "cancelPlanTrip", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelPlanTrip", "cancelPlanTrip", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisitplan/createplan/ " target="_blank">For Page salesvisitplan/createplan/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("rejectPlanTrip")]
        public async Task<ResponseResult> rejectPlanTrip(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, RejectPlanTripModel rejectPlanTripModel)
        {
            try
            {
                onAfterReceiveRequest("rejectPlanTrip", "rejectPlanTrip", rejectPlanTripModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await planTripImp.rejectPlanTrip(rejectPlanTripModel, userProfileForBack);
                //Send Mail
                string Subject, Body, SenderEmail, ReceiveEmail, EmailTemplate=null;
                string SenderName = userProfileForBack.UserProfileCustom.data[0].FirstName + " " + userProfileForBack.UserProfileCustom.data[0].LastName; ;
                List<GetEmailToSendForRejectPlantripCustom> emailList = await planTripImp.getEmailToSendForRejectPlanTrip(rejectPlanTripModel.PlanTripId);
                if (emailList != null && emailList.Count != 0)
                {
                    foreach (GetEmailToSendForRejectPlantripCustom e in emailList)
                    {
                        EmailTemplate = EmailTemplateStatus.REJECT.ToString("d");
                        SenderEmail = e.SenderEmail;
                        ReceiveEmail = e.ReceiveEmail;
                        Subject = "Plan Trip : Rejected";
                        Body = "Rejected : Plan Trip [Name : "+e.PlanTripName+"] ไม่ผ่านการอนุมัติ";

                        SendMailModel sendMailModel = new SendMailModel();
                        sendMailModel.SenderName = SenderName;
                        sendMailModel.MailTo = new List<string> { ReceiveEmail };
                        sendMailModel.showAllRecipients = false;
                        sendMailModel.Subject = Subject;
                        sendMailModel.Content = Body;
                        bool sendEmailSuccess = false;
                        string JOB_STATUS = "S";
                        string ERROR_DESC = "";
                        int JOB_STATUS_FAIL_COUNT = 0;
                        try
                        {
                            SendMailCustom sendMailCustom = await SendEmail(language, sendMailModel, SenderEmail, sendGridClient, logger);
                            sendEmailSuccess = true;
                            log.Info("Send Email Success!.");
                            log.Info("Send Email Success!. System will Record to Email Job............");
                            Console.WriteLine("Send Email Success!.");
                            Console.WriteLine("Send Email Success!. System will Record to Email Job............");
                            string OBJ_EMAIL = JsonConvert.SerializeObject(sendMailModel);// Convert Object For Send Email -> JsonString
                            log.Info("OBJ_EMAIL :" + OBJ_EMAIL);
                            Console.WriteLine("OBJ_EMAIL :" + OBJ_EMAIL);
                            decimal TABLE_REF_KEY_ID = Convert.ToDecimal(rejectPlanTripModel.PlanTripId);
                            EmailJobModel emailJobModel = new EmailJobModel();
                            emailJobModel.EmailTemplate = EmailTemplate;
                            emailJobModel.ObjEmail = OBJ_EMAIL;
                            emailJobModel.TableRefKeyId = TABLE_REF_KEY_ID;
                            emailJobModel.JobStatus = JOB_STATUS;
                            emailJobModel.JobStatusFailCount = JOB_STATUS_FAIL_COUNT;
                            emailJobModel.ErrorDesc = ERROR_DESC;
                            EmailJob emailJob = await emailJobImp.Add(emailJobModel, userProfileForBack);
                            log.Info("SSystem Record to Email Job Complete............");
                            Console.WriteLine("SSystem Record to Email Job Complete............");
                        }
                        catch (Exception ex)
                        {
                            if (!sendEmailSuccess)
                            {
                                JOB_STATUS = "F";
                                ERROR_DESC = ex.Message + ":" + ex.ToString(); ;
                                JOB_STATUS_FAIL_COUNT = 1;
                                log.Info("Send Email Fail!. :" + ex.Message);
                                log.Info("Send Email Fail!. System will Record to Email Job............");
                                Console.WriteLine("Send Email Fail!. :" + ex.Message);
                                Console.WriteLine("Send Email Fail!. System will Record to Email Job............");
                                string OBJ_EMAIL = JsonConvert.SerializeObject(sendMailModel);// Convert Object For Send Email -> JsonString
                                log.Info("OBJ_EMAIL :" + OBJ_EMAIL);
                                Console.WriteLine("OBJ_EMAIL :" + OBJ_EMAIL);
                                decimal TABLE_REF_KEY_ID = Convert.ToDecimal(rejectPlanTripModel.PlanTripId);
                                EmailJobModel emailJobModel = new EmailJobModel();
                                emailJobModel.EmailTemplate = EmailTemplate;
                                emailJobModel.ObjEmail = OBJ_EMAIL;
                                emailJobModel.TableRefKeyId = TABLE_REF_KEY_ID;
                                emailJobModel.JobStatus = JOB_STATUS;
                                emailJobModel.JobStatusFailCount = JOB_STATUS_FAIL_COUNT;
                                emailJobModel.ErrorDesc = ERROR_DESC;
                                EmailJob emailJob = await emailJobImp.Add(emailJobModel, userProfileForBack);
                                log.Info("SSystem Record to Email Job Complete............");
                                Console.WriteLine("SSystem Record to Email Job Complete............");
                            }
                        }
                    }
                }

                //
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("rejectPlanTrip", "rejectPlanTrip", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("rejectPlanTrip", "rejectPlanTrip", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisitplan/createplan/ " target="_blank">For Page salesvisitplan/createplan/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("approvePlanTrip")]
        public async Task<ResponseResult> approvePlanTrip(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, ApprovePlanTripModel approvePlanTripModel)
        {
            try
            {
                onAfterReceiveRequest("approvePlanTrip", "approvePlanTrip", approvePlanTripModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await planTripImp.approvePlanTrip(approvePlanTripModel, userProfileForBack, language);
                //Send Mail
                string Subject, Body, SenderEmail, ReceiveEmail, EmailTemplate=null;
                string SenderName = userProfileForBack.UserProfileCustom.data[0].FirstName + " " + userProfileForBack.UserProfileCustom.data[0].LastName; ;
                List<GetEmailToSendForApprovePlanTripCustom> emailList = await planTripImp.getEmailToSendForapprovePlanTrip(approvePlanTripModel.PlanTripId);
                if (emailList != null && emailList.Count != 0)
                {
                    foreach (GetEmailToSendForApprovePlanTripCustom e in emailList)
                    {
                        EmailTemplate = EmailTemplateStatus.APPROVE.ToString("d");
                        SenderEmail = e.SenderEmail;
                        ReceiveEmail = e.ReceiveEmail;
                        Subject = "Plan Trip : Approved";
                        Body = "Approved : Plan Trip [Name : " + e.PlanTripName + "] ได้รับการอนุมัติเรียบร้อยแล้ว";
                        string StartDateTime = e.PlanTripDate.ToString();
                        string EndDateTime = e.PlanTripDate.ToString();

                        SendMailModel sendMailModel = new SendMailModel();
                        sendMailModel.SenderName = SenderName;
                        sendMailModel.MailTo = new List<string> { ReceiveEmail };
                        sendMailModel.showAllRecipients = false;
                        sendMailModel.Subject = Subject;
                        sendMailModel.Content = Body;
                        MeetingModel meeting = new MeetingModel();
                        meeting.Organizer = SenderEmail;
                        meeting.Subject = Subject;
                        meeting.Description = Body;
                        meeting.StartDateTime = StartDateTime;
                        meeting.EndDateTime = EndDateTime;
                        sendMailModel.Meeting = meeting;
                        bool sendEmailSuccess = false;
                        string JOB_STATUS = "S";
                        string ERROR_DESC = "";
                        int JOB_STATUS_FAIL_COUNT = 0;
                        try
                        {
                            SendMailCustom sendMailCustom = await SendEmail(language, sendMailModel, SenderEmail, sendGridClient, logger);
                            sendEmailSuccess = true;
                            log.Info("Send Email Success!.");
                            log.Info("Send Email Success!. System will Record to Email Job............");
                            Console.WriteLine("Send Email Success!");
                            Console.WriteLine("Send Email Success!. System will Record to Email Job............");
                            string OBJ_EMAIL = JsonConvert.SerializeObject(sendMailModel);// Convert Object For Send Email -> JsonString
                            log.Info("OBJ_EMAIL :" + OBJ_EMAIL);
                            Console.WriteLine("OBJ_EMAIL :" + OBJ_EMAIL);
                            decimal TABLE_REF_KEY_ID = Convert.ToDecimal(approvePlanTripModel.PlanTripId);
                            EmailJobModel emailJobModel = new EmailJobModel();
                            emailJobModel.EmailTemplate = EmailTemplate;
                            emailJobModel.ObjEmail = OBJ_EMAIL;
                            emailJobModel.TableRefKeyId = TABLE_REF_KEY_ID;
                            emailJobModel.JobStatus = JOB_STATUS;
                            emailJobModel.JobStatusFailCount = JOB_STATUS_FAIL_COUNT;
                            emailJobModel.ErrorDesc = ERROR_DESC;
                            EmailJob emailJob = await emailJobImp.Add(emailJobModel, userProfileForBack);
                            log.Info("SSystem Record to Email Job Complete............");
                            Console.WriteLine("SSystem Record to Email Job Complete............");
                        }
                        catch (Exception ex)
                        {
                            if (!sendEmailSuccess)
                            {
                                JOB_STATUS = "F";
                                ERROR_DESC = ex.Message + ":" + ex.ToString(); ;
                                JOB_STATUS_FAIL_COUNT = 1;
                                log.Info("Send Email Fail!. :" + ex.Message);
                                log.Info("Send Email Fail!. System will Record to Email Job............");
                                Console.WriteLine("Send Email Fail!. :" + ex.Message);
                                Console.WriteLine("Send Email Fail!. System will Record to Email Job............");
                                string OBJ_EMAIL = JsonConvert.SerializeObject(sendMailModel);// Convert Object For Send Email -> JsonString
                                log.Info("OBJ_EMAIL :" + OBJ_EMAIL);
                                Console.WriteLine("OBJ_EMAIL :" + OBJ_EMAIL);
                                decimal TABLE_REF_KEY_ID = Convert.ToDecimal(approvePlanTripModel.PlanTripId);
                                EmailJobModel emailJobModel = new EmailJobModel();
                                emailJobModel.EmailTemplate = EmailTemplate;
                                emailJobModel.ObjEmail = OBJ_EMAIL;
                                emailJobModel.TableRefKeyId = TABLE_REF_KEY_ID;
                                emailJobModel.JobStatus = JOB_STATUS;
                                emailJobModel.JobStatusFailCount = JOB_STATUS_FAIL_COUNT;
                                emailJobModel.ErrorDesc = ERROR_DESC;
                                EmailJob emailJob = await emailJobImp.Add(emailJobModel, userProfileForBack);
                                log.Info("SSystem Record to Email Job Complete............");
                                Console.WriteLine("SSystem Record to Email Job Complete............");
                            }
                        }
                    }
                }

                //
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("approvePlanTrip", "approvePlanTrip", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("approvePlanTrip", "approvePlanTrip", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisitplan/createplan/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <para>ProspId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getLastRemindPlanTripProspect")]
        public async Task<ResponseResult> getLastRemindPlanTripProspect(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetLastRemindPlanTripProspectCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getLastRemindPlanTripProspect", "getLastRemindPlanTripProspect", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<GetLastRemindPlanTripProspectCustom> entitiySearchResult = await planTripImp.getLastRemindPlanTripProspect(criteria);
                List<GetLastRemindPlanTripProspectCustom> lst = entitiySearchResult.data;
                SearchResultBase<GetLastRemindPlanTripProspectCustom> searchResult = new SearchResultBase<GetLastRemindPlanTripProspectCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getLastRemindPlanTripProspect", "getLastRemindPlanTripProspect", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getLastRemindPlanTripProspect", "getLastRemindPlanTripProspect", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchSaleOrder")]
        public async Task<ResponseResult> searchSaleOrder(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchSaleOrderCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchSaleOrder", "searchSaleOrder", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchSaleOrderCustom> entitiySearchResult = await saleOrderImp.searchSaleOrder(criteria, userProfileForBack);
                List<SearchSaleOrderCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchSaleOrderCustom> searchResult = new SearchResultBase<SearchSaleOrderCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchSaleOrder", "searchSaleOrder", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchSaleOrder", "searchSaleOrder", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchOrderDocType")]
        public async Task<ResponseResult> searchOrderDocType(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchOrderDocTypeCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchOrderDocType", "searchOrderDocType", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsOrderDocType> entitiySearchResult = await msOrderDocTypeImp.searchOrderDocType(criteria);
                List<MsOrderDocType> lst = entitiySearchResult.data;
                SearchResultBase<MsOrderDocType> searchResult = new SearchResultBase<MsOrderDocType>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchOrderDocType", "searchOrderDocType", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchOrderDocType", "searchOrderDocType", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchShipToByCustSaleId")]
        public async Task<ResponseResult> searchShipToByCustSaleId(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchShipToByCustSaleIdCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchShipToByCustSaleId", "searchShipToByCustSaleId", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchShipToByCustSaleIdCustom> entitiySearchResult = await customerSaleImp.searchShipToByCustSaleId(criteria);
                List<SearchShipToByCustSaleIdCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchShipToByCustSaleIdCustom> searchResult = new SearchResultBase<SearchShipToByCustSaleIdCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchShipToByCustSaleId", "searchShipToByCustSaleId", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchShipToByCustSaleId", "searchShipToByCustSaleId", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchCustomerSaleByCustCode")]
        public async Task<ResponseResult> searchCustomerSaleByCustCode(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchCustomerSaleByCustCodeCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchCustomerSaleByCustCode", "searchCustomerSaleByCustCode", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchCustomerSaleByCustCodeCustom> entitiySearchResult = await customerSaleImp.searchCustomerSaleByCustCode(criteria);
                List<SearchCustomerSaleByCustCodeCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchCustomerSaleByCustCodeCustom> searchResult = new SearchResultBase<SearchCustomerSaleByCustCodeCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchCustomerSaleByCustCode", "searchCustomerSaleByCustCode", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchCustomerSaleByCustCode", "searchCustomerSaleByCustCode", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/ " target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("createSaleOrder")]
        public async Task<ResponseResult> createSaleOrder(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, CreateSaleOrderModel createSaleOrderModel)
        {
            try
            {
                onAfterReceiveRequest("createSaleOrder", "createSaleOrder", createSaleOrderModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                SaleOrder model = await customerSaleImp.createSaleOrder(createSaleOrderModel, userProfileForBack);
                SearchResultBase<SaleOrder> searchResult = new SearchResultBase<SaleOrder>();
                List<SaleOrder> list = new List<SaleOrder>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("createSaleOrder", "createSaleOrder", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("createSaleOrder", "createSaleOrder", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/ " target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelSaleOrder")]
        public async Task<ResponseResult> cancelSaleOrder(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, CancelSaleOrderModel cancelSaleOrderModel)
        {
            try
            {
                onAfterReceiveRequest("cancelSaleOrder", "cancelSaleOrder", cancelSaleOrderModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                InterfaceSapConfig ic = await getInterfaceSapConfigAsync();
                await customerSaleImp.cancelSaleOrder(cancelSaleOrderModel, userProfileForBack, language, ic);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelSaleOrder", "cancelSaleOrder", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelSaleOrder", "cancelSaleOrder", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/ " target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// <para>PlantCode</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("createSaleOrderByQuotationNo")]
        public async Task<ResponseResult> createSaleOrderByQuotationNo(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, CreateSaleOrderByQuotationNoCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("createSaleOrderByQuotationNo", "createSaleOrderByQuotationNo", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                InterfaceSapConfig ic = await getInterfaceSapConfigAsync();
                // Cal use time
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();

                // Begin timing.
                stopwatch.Start();
                //
                EntitySearchResultBase<SaleOrder> entitiySearchResult = await callAPIImp.createSaleOrderByQuotationNo(criteria, userProfileForBack, language, ic);
                // Stop timing.
                stopwatch.Stop();

                // Write result.
                Console.WriteLine("Time elapsed All: "+ stopwatch.Elapsed);
                log.Debug("Time Use All: "+ stopwatch.Elapsed);
                List<SaleOrder> lst = entitiySearchResult.data;
                SearchResultBase<SaleOrder> searchResult = new SearchResultBase<SaleOrder>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.records = lst;

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("createSaleOrderByQuotationNo", "createSaleOrderByQuotationNo", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("createSaleOrderByQuotationNo", "createSaleOrderByQuotationNo", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/ " target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// <para>PlantCode</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updSaleOrder")]
        public async Task<ResponseResult> updSaleOrder(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, UpdSaleOrderCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("updSaleOrder", "updSaleOrder", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                InterfaceSapConfig ic = await getInterfaceSapConfigAsync();
                string timeZone = await getConfigAsync("HOUR_TIME_ZONE");
                // Cal use time
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();

                // Begin timing.
                stopwatch.Start();
                //
                StatusType2 SapStatus = await callAPIImp.updSaleOrder(criteria, userProfileForBack, language, ic, Int32.Parse(timeZone));
                // Stop timing.
                stopwatch.Stop();

                // Write result.
                Console.WriteLine("Time elapsed All: "+ stopwatch.Elapsed);
                log.Debug("Time Use All: "+ stopwatch.Elapsed);
                SearchResultBase<StatusType2> searchResult = new SearchResultBase<StatusType2>();
                List<StatusType2> list = new List<StatusType2>();
                list.Add(SapStatus);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updSaleOrder", "updSaleOrder", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("updSaleOrder", "updSaleOrder", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/ " target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// <para>PlantCode</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("delSaleOrder")]
        public async Task<ResponseResult> delSaleOrder(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, UpdSaleOrderCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("delSaleOrder", "delSaleOrder", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                InterfaceSapConfig ic = await getInterfaceSapConfigAsync();
                string timeZone = await getConfigAsync("HOUR_TIME_ZONE");
                // Cal use time
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();

                // Begin timing.
                stopwatch.Start();
                //
                StatusType2 SapStatus = await callAPIImp.delSaleOrder(criteria, userProfileForBack, language, ic, Int32.Parse(timeZone));
                // Stop timing.
                stopwatch.Stop();

                // Write result.
                Console.WriteLine("Time elapsed All: "+ stopwatch.Elapsed);
                log.Debug("Time Use All: "+ stopwatch.Elapsed);
                SearchResultBase<StatusType2> searchResult = new SearchResultBase<StatusType2>();
                List<StatusType2> list = new List<StatusType2>();
                list.Add(SapStatus);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("delSaleOrder", "delSaleOrder", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("delSaleOrder", "delSaleOrder", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchOrderReason")]
        public async Task<ResponseResult> searchOrderReason(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchOrderReasonCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchOrderReason", "searchOrderReason", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsOrderReason> entitiySearchResult = await msOrderReasonImp.searchOrderReason(criteria);
                List<MsOrderReason> lst = entitiySearchResult.data;
                SearchResultBase<MsOrderReason> searchResult = new SearchResultBase<MsOrderReason>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchOrderReason", "searchOrderReason", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchOrderReason", "searchOrderReason", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchProductByPlantCode")]
        public async Task<ResponseResult> searchProductByPlantCode(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchProductByPlantCodeCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchProductByPlantCode", "searchProductByPlantCode", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchProductByPlantCodeCustom> entitiySearchResult = await msProductImp.searchProductByPlantCode(criteria);
                List<SearchProductByPlantCodeCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchProductByPlantCodeCustom> searchResult = new SearchResultBase<SearchProductByPlantCodeCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchProductByPlantCode", "searchProductByPlantCode", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchProductByPlantCode", "searchProductByPlantCode", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchProductByCustSaleId")]
        public async Task<ResponseResult> searchProductByCustSaleId(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchProductByCustSaleIdCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchProductByCustSaleId", "searchProductByCustSaleId", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsProduct> entitiySearchResult = await msProductImp.searchProductByCustSaleId(criteria);
                List<MsProduct> lst = entitiySearchResult.data;
                SearchResultBase<MsProduct> searchResult = new SearchResultBase<MsProduct>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchProductByCustSaleId", "searchProductByCustSaleId", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchProductByCustSaleId", "searchProductByCustSaleId", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchProductConversion")]
        public async Task<ResponseResult> searchProductConversion(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchProductConversionCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchProductConversion", "searchProductConversion", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsProductConversion> entitiySearchResult = await msProductConversionImp.searchProductConversion(criteria);
                List<MsProductConversion> lst = entitiySearchResult.data;
                SearchResultBase<MsProductConversion> searchResult = new SearchResultBase<MsProductConversion>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchProductConversion", "searchProductConversion", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchProductConversion", "searchProductConversion", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// <para>OrderId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getSaleOrderByOrderId")]
        public async Task<ResponseResult> getSaleOrderByOrderId(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetSaleOrderByOrderIdCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getSaleOrderByOrderId", "getSaleOrderByOrderId", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<GetSaleOrderByOrderIdCustom> entitiySearchResult = await saleOrderImp.getSaleOrderByOrderId(criteria);
                List<GetSaleOrderByOrderIdCustom> lst = entitiySearchResult.data;
                SearchResultBase<GetSaleOrderByOrderIdCustom> searchResult = new SearchResultBase<GetSaleOrderByOrderIdCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getSaleOrderByOrderId", "getSaleOrderByOrderId", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getSaleOrderByOrderId", "getSaleOrderByOrderId", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// <para>SomOrderDte ส่งมาเป้น YYYYMMDD เช่่น 20210712</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchSaleOrderDocFlow")]
        public async Task<ResponseResult> searchSaleOrderDocFlow(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchSaleOrderDocFlowCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchSaleOrderDocFlow", "searchSaleOrderDocFlow", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                InterfaceSapConfig ic = await getInterfaceSapConfigAsync();
                // Cal use time
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();

                // Begin timing.
                stopwatch.Start();
                //
                EntitySearchResultBase<ItemType3> entitiySearchResult = await callAPIImp.searchSaleOrderDocFlow(criteria, language, ic);
                // Stop timing.
                stopwatch.Stop();

                // Write result.
                Console.WriteLine("Time elapsed All: "+ stopwatch.Elapsed);
                log.Debug("Time Use All: "+ stopwatch.Elapsed);
                List<ItemType3> lst = entitiySearchResult.data;
                SearchResultBase<ItemType3> searchResult = new SearchResultBase<ItemType3>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                //onBeforeSendResponse("searchSaleOrderDocFlow", "searchSaleOrderDocFlow", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// <para>OrderId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchSaleOrderChangeLog")]
        public async Task<ResponseResult> searchSaleOrderChangeLog(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchSaleOrderChangeLogCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchSaleOrderChangeLog", "searchSaleOrderChangeLog", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchSaleOrderChangeLogCustom> entitiySearchResult = await saleOrderChangeLogImp.searchSaleOrderChangeLog(criteria);
                List<SearchSaleOrderChangeLogCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchSaleOrderChangeLogCustom> searchResult = new SearchResultBase<SearchSaleOrderChangeLogCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchSaleOrderChangeLog", "searchSaleOrderChangeLog", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchSaleOrderChangeLog", "searchSaleOrderChangeLog", resR);
                return resR;
            }
        }



        /*
        /// <summary>
        /// </summary>
        /// <remarks> 
        /// API Service changeSaleOrder SAP API changeSaleOrder Inbound Service
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("API_SOM_SaleOrder_Change")]
        public async Task<InboundChangeSaleOrderModelResponse> API_SOM_SaleOrder_Change(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, InboundChangeSaleOrderModel model)
        {
            try
            {
                onAfterReceiveRequest("API_SOM_SaleOrder_Change", "API_SOM_SaleOrder_Change", model);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                InboundChangeSaleOrderModelResponse resp = await saleOrderImp.changeSaleOrder(model);
                onBeforeSendResponse("API_SOM_SaleOrder_Change", "API_SOM_SaleOrder_Change", resp);
                return resp;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                //resR.agent = agent;
                StatusSOM_Change s = new StatusSOM_Change();
                s.SOM_Status = "E";
                s.SOM_Message = resR.errorMessage;
                InboundChangeSaleOrderModelResponse re = new InboundChangeSaleOrderModelResponse();
                re.StatusSOM = s;
                re.Header = model.Header;
                return re;
            }
        }
        */



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// API Service cancelSaleOrder SAP API cancelSaleOrder Inbound Service
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("API_SOM_SaleOrder_Change")]
        public async Task<InboundChangeSaleOrderModelResponse> API_SOM_SaleOrder_Change(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, InboundChangeSaleOrderModel model)
        {
            try
            {
                onAfterReceiveRequest("API_SOM_SaleOrder_Change", "API_SOM_SaleOrder_Change", model);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                InboundChangeSaleOrderModelResponse resp = await saleOrderImp.changeSaleOrder(model);
                onBeforeSendResponse("API_SOM_SaleOrder_Change", "API_SOM_SaleOrder_Change", resp);
                return resp;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                onBeforeSendResponse("API_SOM_SaleOrder_Change", "API_SOM_SaleOrder_Change", resR);
                //resR.agent = agent;
                StatusSOM_Change s = new StatusSOM_Change();
                s.SOM_Status = "E";
                s.SOM_Message = resR.errorMessage;
                InboundChangeSaleOrderModelResponse re = new InboundChangeSaleOrderModelResponse();
                re.StatusSOM = s;
                re.Header = model.Header;
                return re;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// API Service cancelSaleOrder SAP API cancelSaleOrder Inbound Service
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("API_SOM_SaleOrder_Cancel")]
        public async Task<InboundCancelSaleOrderModelResponse> API_SOM_SaleOrder_Cancel(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, InboundCancelSaleOrderModel model)
        {
            try
            {
                onAfterReceiveRequest("API_SOM_SaleOrder_Cancel", "API_SOM_SaleOrder_Cancel", model);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                InboundCancelSaleOrderModelResponse resp = await saleOrderImp.cancelSaleOrder(model);
                onBeforeSendResponse("API_SOM_SaleOrder_Cancel", "API_SOM_SaleOrder_Cancel", resp);
                return resp;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                onBeforeSendResponse("API_SOM_SaleOrder_Cancel", "API_SOM_SaleOrder_Cancel", resR);
                //resR.agent = agent;
                StatusSOM_Cancel s = new StatusSOM_Cancel();
                s.SOM_Status = "E";
                s.SOM_Message = resR.errorMessage;
                InboundCancelSaleOrderModelResponse re = new InboundCancelSaleOrderModelResponse();
                re.StatusSOM = s;
                re.Header = model.Header;
                return re;
            }
        }







        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Usergroup.php" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchAdmPermObject")]
        public async Task<ResponseResult> searchAdmPermObject(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchAdmPermObjectCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAdmPermObject", "searchAdmPermObject", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<AdmPermObject> entitiySearchResult = await admPermObjectImp.searchAdmPermObject(criteria);
                List<AdmPermObject> lst = entitiySearchResult.data;
                SearchResultBase<AdmPermObject> searchResult = new SearchResultBase<AdmPermObject>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchAdmPermObject", "searchAdmPermObject", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchAdmPermObject", "searchAdmPermObject", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Usergroup.php" target="_blank">For Page masterdata/Organizational/Brand.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchAdmGroupApp")]
        public async Task<ResponseResult> searchAdmGroupApp(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchAdmGroupAppCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAdmGroupApp", "searchAdmGroupApp", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchAdmGroupAppCustom> entitiySearchResult = await admGroupAppImp.searchAdmGroupApp(criteria);
                List<SearchAdmGroupAppCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchAdmGroupAppCustom> searchResult = new SearchResultBase<SearchAdmGroupAppCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchAdmGroupApp", "searchAdmGroupApp", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchAdmGroupApp", "searchAdmGroupApp", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Usergroup.php" target="_blank">For Page masterdata/Organizational/Brand.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addAdmGroupApp")]
        public async Task<ResponseResult> addAdmGroupApp(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, AddAdmGroupAppModel addAdmGroupAppModel)
        {
            try
            {
                onAfterReceiveRequest("addAdmGroupApp", "addAdmGroupApp", addAdmGroupAppModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                AdmGroupApp model = await admGroupAppImp.addAdmGroupApp(addAdmGroupAppModel, userProfileForBack);
                SearchResultBase<AdmGroupApp> searchResult = new SearchResultBase<AdmGroupApp>();
                List<AdmGroupApp> list = new List<AdmGroupApp>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addAdmGroupApp", "addAdmGroupApp", res);
                return res;
            }
            catch (Exception e)
            {
                // Case Duplicate Key for Add/Edit Master Data.
                if (isConstraintError(e))
                {
                    e = new ServiceException("W_0014", ObjectFacory.getCultureInfo(language));
                }
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("addAdmGroupApp", "addAdmGroupApp", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Usergroup.php" target="_blank">For Page masterdata/Organizational/Brand.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updAdmGroupApp")]
        public async Task<ResponseResult> updAdmGroupApp(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, UpdAdmGroupAppModel updAdmGroupAppModel)
        {
            try
            {
                onAfterReceiveRequest("updAdmGroupApp", "updAdmGroupApp", updAdmGroupAppModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await admGroupAppImp.updAdmGroupApp(updAdmGroupAppModel, userProfileForBack);
                SearchResultBase<MsBrand> searchResult = new SearchResultBase<MsBrand>();
                List<MsBrand> list = new List<MsBrand>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updAdmGroupApp", "updAdmGroupApp", res);
                return res;
            }
            catch (Exception e)
            {
                // Case Duplicate Key for Add/Edit Master Data.
                if (isConstraintError(e))
                {
                    e = new ServiceException("W_0014", ObjectFacory.getCultureInfo(language));
                }
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("updAdmGroupApp", "updAdmGroupApp", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Usergroup.php" target="_blank">For Page masterdata/Organizational/Brand.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelAdmGroupApp")]
        public async Task<ResponseResult> cancelAdmGroupApp(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, CancelAdmGroupAppModel cancelAdmGroupAppModel)
        {
            try
            {
                onAfterReceiveRequest("cancelAdmGroupApp", "cancelAdmGroupApp", cancelAdmGroupAppModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await admGroupAppImp.cancelAdmGroupApp(cancelAdmGroupAppModel, userProfileForBack);
                SearchResultBase<MsBrand> searchResult = new SearchResultBase<MsBrand>();
                List<MsBrand> list = new List<MsBrand>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelAdmGroupApp", "cancelAdmGroupApp", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelAdmGroupApp", "cancelAdmGroupApp", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/AccessRights.php" target="_blank">For Page masterdata/Organizational/Brand.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchAdmGroupPerm")]
        public async Task<ResponseResult> searchAdmGroupPerm(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchAdmGroupPermCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAdmGroupPerm", "searchAdmGroupPerm", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchAdmGroupPermCustom> entitiySearchResult = await admGroupAppImp.searchAdmGroupPerm(criteria);
                List<SearchAdmGroupPermCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchAdmGroupPermCustom> searchResult = new SearchResultBase<SearchAdmGroupPermCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchAdmGroupPerm", "searchAdmGroupPerm", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchAdmGroupPerm", "searchAdmGroupPerm", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/AccessRights.php" target="_blank">For Page masterdata/Organizational/Brand.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updAdmGroupPerm")]
        public async Task<ResponseResult> updAdmGroupPerm(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, UpdAdmGroupPermModel updAdmGroupPermModel)
        {
            try
            {
                onAfterReceiveRequest("updAdmGroupPerm", "updAdmGroupPerm", updAdmGroupPermModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await admGroupAppImp.updAdmGroupPerm(updAdmGroupPermModel, userProfileForBack);
                SearchResultBase<MsBrand> searchResult = new SearchResultBase<MsBrand>();
                List<MsBrand> list = new List<MsBrand>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updAdmGroupPerm", "updAdmGroupPerm", res);
                return res;
            }
            catch (Exception e)
            {
                // Case Duplicate Key for Add/Edit Master Data.
                if (isConstraintError(e))
                {
                    e = new ServiceException("W_0014", ObjectFacory.getCultureInfo(language));
                }
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("updAdmGroupPerm", "updAdmGroupPerm", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchCompany")]
        public async Task<ResponseResult> searchCompany(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearCompanyCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchCompany", "searchCompany", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearCompanyCustom> entitiySearchResult = await customerCompanyImp.searCompany(criteria);
                List<SearCompanyCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearCompanyCustom> searchResult = new SearchResultBase<SearCompanyCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchCompany", "searchCompany", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchCompany", "searchCompany", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesorder/" target="_blank">For Page salesorder/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchOrderIncoterm")]
        public async Task<ResponseResult> searchOrderIncoterm(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchOrderIncotermCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchOrderIncoterm", "searchOrderIncoterm", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsOrderIncoterm> entitiySearchResult = await msOrderIncotermImp.searchOrderIncoterm(criteria);
                List<MsOrderIncoterm> lst = entitiySearchResult.data;
                SearchResultBase<MsOrderIncoterm> searchResult = new SearchResultBase<MsOrderIncoterm>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchOrderIncoterm", "searchOrderIncoterm", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchOrderIncoterm", "searchOrderIncoterm", resR);
                return resR;
            }
        }



        [HttpPost("searchTemplateSa")]
        public async Task<ResponseResult> searchTemplateSa(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchTemplateSaCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchTemplateSa", "searchTemplateSa", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<TemplateSaForm> entitiySearchResult = await templateSaFormImp.searchTemplateSa(criteria);
                List<TemplateSaForm> lst = entitiySearchResult.data;
                SearchResultBase<TemplateSaForm> searchResult = new SearchResultBase<TemplateSaForm>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchTemplateSa", "searchTemplateSa", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchTemplateSa", "searchTemplateSa", resR);
                return resR;
            }
        }


        [HttpPost("searchTemplateAppForm")]
        public async Task<ResponseResult> searchTemplateAppForm(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchTemplateAppFormCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchTemplateAppForm", "searchTemplateAppForm", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<TemplateAppForm> entitiySearchResult = await templateAppFormImp.searchTemplateAppForm(criteria);
                List<TemplateAppForm> lst = entitiySearchResult.data;
                SearchResultBase<TemplateAppForm> searchResult = new SearchResultBase<TemplateAppForm>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchTemplateAppForm", "searchTemplateAppForm", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchTemplateAppForm", "searchTemplateAppForm", resR);
                return resR;
            }
        }


        [HttpPost("searchProspectAll")]
        public async Task<ResponseResult> searchProspectAll(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchProspectAllCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchProspectAll", "searchProspectAll", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchProspectAllCustom> entitiySearchResult = await prospectImp.searchProspectAll(criteria);
                List<SearchProspectAllCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchProspectAllCustom> searchResult = new SearchResultBase<SearchProspectAllCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchProspectAll", "searchProspectAll", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchProspectAll", "searchProspectAll", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/index.php" target="_blank">For Page index/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchEmailJobForPlanTrip")]
        public async Task<ResponseResult> searchEmailJobForPlanTrip(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchEmailJobForPlanTripCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchEmailJobForPlanTrip", "searchEmailJobForPlanTrip", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchEmailJobForPlanTripCustom> entitiySearchResult = await emailJobImp.searchEmailJobForPlanTrip(criteria, userProfileForBack);
                List<SearchEmailJobForPlanTripCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchEmailJobForPlanTripCustom> searchResult = new SearchResultBase<SearchEmailJobForPlanTripCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchEmailJobForPlanTrip", "searchEmailJobForPlanTrip", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchEmailJobForPlanTrip", "searchEmailJobForPlanTrip", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/index.php" target="_blank">For Page index/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getNotifyTabOverview")]
        public async Task<ResponseResult> getNotifyTabOverview(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<GetNotifyTabOverviewCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getNotifyTabOverview", "getNotifyTabOverview", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<GetNotifyTabOverviewCustom> entitiySearchResult = await saleOrderChangeLogImp.getNotifyTabOverview(criteria);
                List<GetNotifyTabOverviewCustom> lst = entitiySearchResult.data;
                SearchResultBase<GetNotifyTabOverviewCustom> searchResult = new SearchResultBase<GetNotifyTabOverviewCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getNotifyTabOverview", "getNotifyTabOverview", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getNotifyTabOverview", "getNotifyTabOverview", resR);
                return resR;
            }
        }




    }

}
