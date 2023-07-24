using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyFirstAzureWebApp.Business.rep;
using MyFirstAzureWebApp.Controllers;
using MyFirstAzureWebApp.ModelCriteriaReport;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.ModelsReport;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Utils;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Controllers.Report
{
    [Route("enq")]
    [ApiController]
    public class EnquiryController : BaseController
    {
        private IReport reportImp;

        public EnquiryController()
        {
            reportImp = new ReportImp();
        }

        [HttpPost("rep01VisitPlanRawData")]
        public async Task<ResponseResult> rep01VisitPlanRawData(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq01VisitPlanRawData", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep01VisitPlanRawDataResult> result = await reportImp.SearchRep01VisitPlanRawData(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq01VisitPlanRawData", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq01VisitPlanRawData", "enq01VisitPlanRawData", resR);
                return resR;
            }
        }

        [HttpPost("rep16Assign")]
        public async Task<ResponseResult> rep16Assign(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq16Assign", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep16AssignResult> result = await reportImp.SearchRep16Assign(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq16Assign", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq16Assign", "enq16Assign", resR);
                return resR;
            }
        }

        [HttpPost("rep03VisitPlanDaily")]
        public async Task<ResponseResult> rep03VisitPlanDaily(
                [FromHeader(Name = "Accept-Language")] string language,
                [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq03VisitPlanDaily", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep03VisitPlanDailyResult> result = await reportImp.Search03VisitPlanDaily(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq03VisitPlanDaily", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq03VisitPlanDaily", "enq03VisitPlanDaily", resR);
                return resR;
            }
        }

        [HttpPost("rep04VisitPlanMonthly")]
        public async Task<ResponseResult> rep04VisitPlanMonthly(
                [FromHeader(Name = "Accept-Language")] string language,
                [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq04VisitPlanMonthly", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep04VisitPlanMonthlyResult> result = await reportImp.Search04VisitPlanMonthly(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq04VisitPlanMonthly", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq04VisitPlanMonthly", "enq04VisitPlanMonthly", resR);
                return resR;
            }
        }

        // BCW

        [HttpPost("rep02VisitPlanTransaction")]
        public async Task<ResponseResult> rep02VisitPlanTransaction(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq02VisitPlanTransaction", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep02VisitPlanTransactionResult> result = await reportImp.SearchRep02VisitPlanTransaction(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq02VisitPlanTransaction", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq02VisitPlanTransaction", "enq02VisitPlanTransaction", resR);
                return resR;
            }
        }

        [HttpPost("rep06MeterRawData")]
        public async Task<ResponseResult> rep06MeterRawData(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq06MeterRawData", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep06MeterRawDataResult> result = await reportImp.SearchRep06MeterRawData(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq06MeterRawData", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq06MeterRawData", "enq06MeterRawData", resR);
                return resR;
            }
        }


        [HttpPost("rep13SaleOrderRawData")]
        public async Task<ResponseResult> rep13SaleOrderRawData(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq13SaleOrderRawData", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep13SaleOrderRawDataResult> result = await reportImp.SearchRep13SaleOrderRawData(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq13SaleOrderRawData", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq13SaleOrderRawData", "enq13SaleOrderRawData", resR);
                return resR;
            }
        }


        [HttpPost("rep07MeterTransaction")]
        public async Task<ResponseResult> rep07MeterTransaction(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq07MeterTransaction", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep07MeterTransactionResult> result = await reportImp.SearchRep07MeterTransaction(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq07MeterTransaction", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq07MeterTransaction", "enq07MeterTransaction", resR);
                return resR;
            }
        }

        [HttpPost("rep08StockCardRawData")]
        public async Task<ResponseResult> rep08StockCardRawData(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq08StockCardRawData", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep08StockCardRawDataResult> result = await reportImp.SearchRep08StockCardRawData(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq08StockCardRawData", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq08StockCardRawData", "enq08StockCardRawData", resR);
                return resR;
            }
        }

        [HttpPost("rep10ProspectRawData")]
        public async Task<ResponseResult> rep10ProspectRawData(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq10ProspectRawData", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep10ProspectRawDataResult> result = await reportImp.SearchRep10ProspectRawData(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq10ProspectRawData", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq10ProspectRawData", "enq10ProspectRawData", resR);
                return resR;
            }
        }
        // BCW

        [HttpPost("rep05VisitPlanActual")]
        public async Task<ResponseResult> rep05VisitPlanActual(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq05VisitPlanActual", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep05VisitPlanActualResult> result = await reportImp.Search05VisitPlanActual(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq05VisitPlanActual", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq05VisitPlanActual", "enq05VisitPlanActual", resR);
                return resR;
            }
        }

        [HttpPost("rep11ProspectPerformancePerSaleRep")]
        public async Task<ResponseResult> rep11ProspectPerformancePerSaleRep(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq11ProspectPerformancePerSaleRep", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep11ProspectPerformancePerSaleRepResult> result = await reportImp.Search11ProspectPerformancePerSaleRep(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq11ProspectPerformancePerSaleRep", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq11ProspectPerformancePerSaleRep", "enq11ProspectPerformancePerSaleRep", resR);
                return resR;
            }
        }

        [HttpPost("rep12ProspectPerformancePerSaleGroup")]
        public async Task<ResponseResult> rep12ProspectPerformancePerSaleGroup(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq12ProspectPerformancePerSaleGroup", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep12ProspectPerformancePerSaleGroupResult> result = await reportImp.Search12ProspectPerformancePerSaleGroup(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq12ProspectPerformancePerSaleGroup", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq12ProspectPerformancePerSaleGroup", "enq12ProspectPerformancePerSaleGroup", resR);
                return resR;
            }
        }

        [HttpPost("rep14SaleOrderByChannel")]
        public async Task<ResponseResult> rep14SaleOrderByChannel(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq14SaleOrderByChannel", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                InterfaceSapConfig ic = await getInterfaceSapConfigAsync();
                //c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep14SaleOrderByChannelResult> result = await reportImp.search14SaleOrderByChannel(c, ic);
                foreach (var r in result.records)
                {
                    r.percentSom = (Math.Round(((new Decimal(int.Parse(r.totalSomOrder)) / new Decimal(int.Parse(r.totalOrder))) * 100), 2)).ToString();
                    r.percentSap = (Math.Round(((new Decimal(int.Parse(r.totalSapOrder)) / new Decimal(int.Parse(r.totalOrder))) * 100), 2)).ToString();
                }

                //result.pageNo = c.pageNo;
                //result.recordPerPage = c.length;
                //result.recordStart = c.startRecord;
                //result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq14SaleOrderByChannel", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.error("E", e.Message);
                resR.agent = agent;
                onBeforeSendResponse("enq14SaleOrderByChannel", "enq14SaleOrderByChannel", resR);
                return resR;
            }
        }

        [HttpPost("rep15SaleOrderByCompany")]
        public async Task<ResponseResult> rep15SaleOrderByCompany(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq15SaleOrderByCompany", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                InterfaceSapConfig ic = await getInterfaceSapConfigAsync();
                //c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep15SaleOrderByCompanyResult> result = await reportImp.search15SaleOrderByCompany(c, ic);

                //result.pageNo = c.pageNo;
                //result.recordPerPage = c.length;
                //result.recordStart = c.startRecord;
                //result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq15SaleOrderByCompany", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.error("E", e.Message);
                resR.agent = agent;
                onBeforeSendResponse("enq15SaleOrderByCompany", "enq15SaleOrderByCompany", resR);
                return resR;
            }
        }

        [HttpPost("rep09StockCardCustomerSummary")]
        public async Task<ResponseResult> rep09StockCardCustomerSummary(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq09StockCardCustomerSummary", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep09Rult> result = await reportImp.search09StockCardCustomerSummary(c);

                //result.pageNo = c.pageNo;
                //result.recordPerPage = c.length;
                //result.recordStart = c.startRecord;
                //result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq09StockCardCustomerSummary", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.error("E", e.Message);
                resR.agent = agent;
                onBeforeSendResponse("enq09StockCardCustomerSummary", "enq09StockCardCustomerSummary", resR);
                return resR;
            }
        }

        [HttpPost("rep17ImageInTemplate")]
        public async Task<ResponseResult> rep17ImageInTemplate(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("enq17ImageInTemplate", nameMapReqRes, c);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                c.startRecord = (c.length * (c.pageNo - 1) + 1);
                SearchResultBase<Rep17ImageInTemplateResult> result = await reportImp.SearchRep17ImageInTemplate(c);

                result.pageNo = c.pageNo;
                result.recordPerPage = c.length;
                result.recordStart = c.startRecord;
                result.totalPages = c.length == 0 ? 0 : result.totalRecords == 0 ? 0 : (result.totalRecords % result.recordPerPage == 0 ? result.totalRecords / result.recordPerPage : ((result.totalRecords / result.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(result);
                res.agent = agent;
                onBeforeSendResponse("enq17ImageInTemplate", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("enq17ImageInTemplate", "enq17ImageInTemplate", resR);
                return resR;
            }
        }
    }

}
