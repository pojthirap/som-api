using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using MyFirstAzureWebApp.Authentication;
using MyFirstAzureWebApp.Business.org;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.exception;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Models.plan;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Controllers
{
    [Route("org")]
    [ApiController]
    public class ORGController : BaseController
    {

        private Logger log = LogManager.GetCurrentClassLogger();
        private ICompany companyImp;
        private IRegion regionImp;
        private ICustomSpacial customSpacialImp;
        private IProvince provinceImp;
        private IGasOnline gasOnlineImp;
        private IMeter meterImp;
        private IOrgDistChannel orgDistChannelImp;
        private IOrgDivision orgDivisionImp;
        private IOrgSaleOffice orgSaleOfficeImp;
        private IOrgSaleGroup orgSaleGroupImp;
        private IOrgSaleArea orgSaleAreaImp;
        private IOrgBusinessUnit orgBusinessUnitImp;
        private IMsPlantShip msPlantShipImp;
        private IOrgTerritory orgTerritoryImp;
        private IMsBrand msBrandImp;
        private IMsLocationType msLocationTypeImp;
        private IMsLocation msLocationImp;
        private IMsBrandCategory msBrandCategoryImp;
        private IMsServiceType msServiceTypeImp;
        private IMsBank msBankImp;
        private ITemplateCategory templateCategoryImp;
        private ITemplateAppForm templateAppFormImp;
        private ITemplateAppQuestion templateAppQuestionImp;
        private ITemplateQuestion templateQuestionImp;
        private IMsActtachCategory msActtachCategoryImp;
        private ITemplateStockCard templateStockCardImp;
        private ITemplateStockProduct templateStockProductImp;
        private IMsProduct msProductImp;
        private IMsProductConversion msProductConversionImp;
        private ITemplateSaForm templateSaFormImp;
        private ITemplateSaTitle templateSaTitleImp;
        private IMsGasoline msGasolineImp;
        private IPlanReasonNotVisit planReasonNotVisitImp;
        private IMsDistrict msDistrictImp;
        private IMsSubDistrict msSubDistrictImp;
        private IMsConfigLov msConfigLovImp;
        private IMsConfigParam msConfigParamImp;
        private ICustomer customerImp;


        public ORGController()
        {
            companyImp = new CompanyImp();
            regionImp = new RegionImp();
            customSpacialImp = new CustomSpacialImp();
            provinceImp = new ProvinceImp();
            gasOnlineImp = new GasOnlineImp();
            meterImp = new MeterImp();
            orgDistChannelImp = new OrgDistChannelImp();
            orgDivisionImp = new OrgDivisionImp();
            orgSaleOfficeImp = new OrgSaleOfficeImp();
            orgSaleGroupImp = new OrgSaleGroupImp();
            orgSaleAreaImp = new OrgSaleAreaImp();
            orgBusinessUnitImp = new OrgBusinessUnitImp();
            msPlantShipImp = new MsPlantShipImp();
            orgTerritoryImp = new OrgTerritoryImp();
            msBrandImp = new MsBrandImp();
            msLocationTypeImp = new MsLocationTypeImp();
            msLocationImp = new MsLocationImp();
            msBrandCategoryImp = new MsBrandCategoryImp();
            msServiceTypeImp = new MsServiceTypeImp();
            msBankImp = new MsBankImp();
            templateCategoryImp = new TemplateCategoryImp();
            templateAppFormImp = new TemplateAppFormImp();
            templateAppQuestionImp = new TemplateAppQuestionImp();
            templateQuestionImp = new TemplateQuestionImp();
            msActtachCategoryImp = new MsActtachCategoryImp();
            templateStockCardImp = new TemplateStockCardImp();
            templateStockProductImp = new TemplateStockProductImp();
            msProductImp = new MsProductImp();
            msProductConversionImp = new MsProductConversionImp();
            templateSaFormImp = new TemplateSaFormImp();
            templateSaTitleImp = new TemplateSaTitleImp();
            msGasolineImp = new MsGasolineImp();
            planReasonNotVisitImp = new PlanReasonNotVisitImp();
            msDistrictImp = new MsDistrictImp();
            msSubDistrictImp = new MsSubDistrictImp();
            msConfigLovImp = new MsConfigLovImp();
            msConfigParamImp = new MsConfigParamImp();
            customerImp = new CustomerImp();
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/company.php" target="_blank">For Page masterdata/Organizational/company.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchCompany")]
        public async Task<ResponseResult> searchCompany(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, OrgCompanySearchCriteria criteria)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("searchCompany", nameMapReqRes, criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<OrgCompany> entitiySearchResult = await companyImp.Search(criteria);
                List<OrgCompany> lst = entitiySearchResult.data;
                SearchResultBase<OrgCompany> searchResult = new SearchResultBase<OrgCompany>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) +1));
                
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchCompany", nameMapReqRes, res);
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
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Region.php" target="_blank">For Page masterdata/Organizational/Region.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchRegion")]
        public async Task<ResponseResult> searchRegion(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, RegionSearchCriteria criteria)
        {
            try
            {
                string nameMapReqRes = getNameMaping();
                onAfterReceiveRequest("searchRegion", nameMapReqRes, criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1)+1);
                EntitySearchResultBase<MsRegion> entitiySearchResult = await regionImp.Search(criteria);
                List<MsRegion> lst = entitiySearchResult.data;
                SearchResultBase<MsRegion> searchResult = new SearchResultBase<MsRegion>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchRegion", nameMapReqRes, res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchRegion", "searchRegion", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Region.php" target="_blank">For Page masterdata/Organizational/Region.php</a>
        /// <p>Required Paramerer</p>
        /// <p>regionCoder</p>
        /// <p>regionNameTh</p>
        /// <p>regionNameEn</p>
        /// <p>activeFlag</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addRegion")]
     public async Task<ResponseResult> addRegion(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, RegionModel regionModel)
        {
            try
            {
                onAfterReceiveRequest("AddRegion", "AddRegion", regionModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                regionModel.UserProfile = userProfileForBack;
                MsRegion model = await regionImp.Add(regionModel);
                SearchResultBase<MsRegion> searchResult = new SearchResultBase<MsRegion>();
                List<MsRegion> list = new List<MsRegion>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("AddRegion", "AddRegion", res);
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
                onBeforeSendResponse("AddRegion", "AddRegion", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Region.php" target="_blank">For Page masterdata/Organizational/Region.php</a>
        /// <p>Required Paramerer</p>
        /// <p>regionCoder</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updRegion")]
        public async Task<ResponseResult> updRegion(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, RegionModel regionModel)
        {
            try
            {
                onAfterReceiveRequest("updRegion", "updRegion", regionModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                regionModel.UserProfile = userProfileForBack;
                await regionImp.Update(regionModel);
                SearchResultBase<MsRegion> searchResult = new SearchResultBase<MsRegion>();
                List<MsRegion> list = new List<MsRegion>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updRegion", "updRegion", res);
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
                onBeforeSendResponse("updRegion", "updRegion", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Region.php" target="_blank">For Page masterdata/Organizational/Region.php</a>
        /// <p>Required Paramerer</p>
        /// <p>regionCoder</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelRegioin")]
        public async Task<ResponseResult> cancelRegioin(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, RegionModel regionModel)
        {
            try
            {
                onAfterReceiveRequest("cancelRegioin", "cancelRegioin", regionModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                regionModel.UserProfile = userProfileForBack;
                await regionImp.DeleteUpdate(regionModel);
                SearchResultBase<MsRegion> searchResult = new SearchResultBase<MsRegion>();
                List<MsRegion> list = new List<MsRegion>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelRegioin", "cancelRegioin", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelRegioin", "cancelRegioin", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Region-2.php" target="_blank">For Page masterdata/Organizational/Region-2.php</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/basic/" target="_blank">For Page prospect/basic/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchProvince")]
        public async Task<ResponseResult> searchProvince(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, ProvinceSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchProvince", "searchProvince", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsProvince> entitiySearchResult = await provinceImp.Search(criteria);
                List<MsProvince> lst = entitiySearchResult.data;
                SearchResultBase<MsProvince> searchResult = new SearchResultBase<MsProvince>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchProvince", "searchProvince", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchProvince", "searchProvince", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Region-2.php" target="_blank">For Page masterdata/Organizational/Region-2.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchProvinceForMapRegion")]
        public async Task<ResponseResult> searchProvinceForMapRegion(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, ProvinceSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchProvinceForMapRegion", "searchProvinceForMapRegion", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsProvince> entitiySearchResult = await provinceImp.searchProvinceForMapRegion(criteria);
                List<MsProvince> lst = entitiySearchResult.data;
                SearchResultBase<MsProvince> searchResult = new SearchResultBase<MsProvince>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchProvinceForMapRegion", "searchProvinceForMapRegion", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchProvinceForMapRegion", "searchProvinceForMapRegion", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Region-2.php" target="_blank">For Page masterdata/Organizational/Region-2.php</a>
        /// <p>Required Paramerer</p>
        /// <p>regionCode</p>
        /// <p>provinceCodeList[]</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updProvince")]
        public async Task<ResponseResult> updProvince(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, RegionModel regionModel)
        {
            try
            {
                onAfterReceiveRequest("updProvince", "updProvince", regionModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                regionModel.UserProfile = userProfileForBack;
                int model = await provinceImp.mapRegionToProvince(regionModel);
                SearchResultBase<MsRegion> searchResult = new SearchResultBase<MsRegion>();
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updProvince", "updProvince", res);
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
                onBeforeSendResponse("updProvince", "updProvince", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleOrg.php" target="_blank">For Page masterdata/Organizational/SaleOrg.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchOrg")]
        public async Task<ResponseResult> searchOrg(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SaleOrgCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchOrg", "searchOrg", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SaleOrgCustom> entitiySearchResult = await customSpacialImp.SearchSaleOrg(criteria);
                List<SaleOrgCustom> lst = entitiySearchResult.data;
                SearchResultBase<SaleOrgCustom> searchResult = new SearchResultBase<SaleOrgCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchOrg", "searchOrg", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchOrg", "searchOrg", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/QRMaster-2.php" target="_blank">For Page masterdata/Organizational/QRMaster-2.php</a>
        /// <p>Required Paramerer</p>
        /// <p>custCode</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addMeter")]
        public async Task<ResponseResult> addMeter(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MeterModel meterModel)
        {
            try
            {
                onAfterReceiveRequest("addMeter", "addMeter", meterModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                meterModel.UserProfile = userProfileForBack;
                MsMeter model = await meterImp.Add(meterModel);
                SearchResultBase<MsMeter> searchResult = new SearchResultBase<MsMeter>();
                List<MsMeter> list = new List<MsMeter>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addMeter", "addMeter", res);
                return res;
            }
            catch (Exception e)
            {
                if (isConstraintError(e))
                {
                    e = new ServiceException("W_0001", ObjectFacory.getCultureInfo(language));
                }
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("addMeter", "addMeter", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/QRMaster-2.php" target="_blank">For Page masterdata/Organizational/QRMaster-2.php</a>
        /// <p>Required Paramerer</p>
        /// <p>meterId</p>
        /// <p>custCode</p>
        /// <p>activeFlag</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updMeter")]
        public async Task<ResponseResult> updMeter(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MeterModel meterModel)
        {
            try
            {
                onAfterReceiveRequest("updMeter", "updMeter", meterModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                meterModel.UserProfile = userProfileForBack;
                await meterImp.Update(meterModel);
                SearchResultBase<MsMeter> searchResult = new SearchResultBase<MsMeter>();
                List<MsMeter> list = new List<MsMeter>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updMeter", "updMeter", res);
                return res;
            }
            catch (Exception e)
            {
                if (isConstraintError(e))
                {
                    e = new ServiceException("W_0001", ObjectFacory.getCultureInfo(language));
                }
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("updMeter", "updMeter", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/QRMaster-2.php" target="_blank">For Page masterdata/Organizational/QRMaster-2.php</a>
        /// <p>Required Paramerer</p>
        /// <p>meterId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelMeter")]
        public async Task<ResponseResult> cancelMeter(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MeterModel meterModel)
        {
            try
            {
                onAfterReceiveRequest("cancelMeter", "cancelMeter", meterModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                meterModel.UserProfile = userProfileForBack;
                await meterImp.DeleteUpdate(meterModel);
                SearchResultBase<MsMeter> searchResult = new SearchResultBase<MsMeter>();
                List<MsMeter> list = new List<MsMeter>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelMeter", "cancelMeter", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelMeter", "cancelMeter", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Channel.php" target="_blank">For Page masterdata/Organizational/Channel.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchChannel")]
        public async Task<ResponseResult> searchChannel(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, OrgDistChannelSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchChannel", "searchChannel", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<OrgDistChannel> entitiySearchResult = await orgDistChannelImp.Search(criteria);
                List<OrgDistChannel> lst = entitiySearchResult.data;
                SearchResultBase<OrgDistChannel> searchResult = new SearchResultBase<OrgDistChannel>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchChannel", "searchChannel", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchChannel", "searchChannel", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Division.php" target="_blank">For Page masterdata/Organizational/Division.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchDivision")]
        public async Task<ResponseResult> searchDivision(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, OrgDivisionSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchDivision", "searchDivision", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<OrgDivision> entitiySearchResult = await orgDivisionImp.Search(criteria);
                List<OrgDivision> lst = entitiySearchResult.data;
                SearchResultBase<OrgDivision> searchResult = new SearchResultBase<OrgDivision>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchDivision", "searchDivision", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchDivision", "searchDivision", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleOffice.php" target="_blank">For Page masterdata/Organizational/SaleOffice.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchOffice")]
        public async Task<ResponseResult> searchOffice(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, OrgSaleOfficeSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchOffice", "searchOffice", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<OrgSaleOffice> entitiySearchResult = await orgSaleOfficeImp.Search(criteria);
                List<OrgSaleOffice> lst = entitiySearchResult.data;
                SearchResultBase<OrgSaleOffice> searchResult = new SearchResultBase<OrgSaleOffice>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchOffice", "searchOffice", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchOffice", "searchOffice", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleGroup.php" target="_blank">For Page masterdata/Organizational/SaleGroup.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchSaleGroup")]
        public async Task<ResponseResult> searchSaleGroup(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, OrgSaleGroupSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchSaleGroup", "searchSaleGroup", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<OrgSaleGroup> entitiySearchResult = await orgSaleGroupImp.Search(criteria);
                List<OrgSaleGroup> lst = entitiySearchResult.data;
                SearchResultBase<OrgSaleGroup> searchResult = new SearchResultBase<OrgSaleGroup>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchSaleGroup", "searchSaleGroup", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchSaleGroup", "searchSaleGroup", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleGroup.php" target="_blank">For Page masterdata/Organizational/SaleGroup.php</a>
        /// <p>Required Paramerer</p>
        /// <p>managerEmpId</p>
        /// <p>groupCode</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updrSaleGroupByManagerEmpId")]
        public async Task<ResponseResult> updrSaleGroupByManagerEmpId(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, OrgSaleGroupModel orgSaleGroupModel)
        {
            try
            {
                onAfterReceiveRequest("updrSaleGroupByManagerEmpId", "updrSaleGroupByManagerEmpId", orgSaleGroupModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                orgSaleGroupModel.UserProfile = userProfileForBack;
                await orgSaleGroupImp.UpdateManagerSaleGroup(orgSaleGroupModel);
                SearchResultBase<OrgSaleGroup> searchResult = new SearchResultBase<OrgSaleGroup>();
                List<OrgSaleGroup> list = new List<OrgSaleGroup>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updrSaleGroupByManagerEmpId", "updrSaleGroupByManagerEmpId", res);
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
                onBeforeSendResponse("updrSaleGroupByManagerEmpId", "updrSaleGroupByManagerEmpId", resR);
                return resR;
            }
        }


        [HttpPost("updTerritorySaleGroup")]
        public async Task<ResponseResult> updTerritorySaleGroup(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, UpdateTerritorySaleGroupModel updateTerritorySaleGroupModel)
        {
            try
            {
                onAfterReceiveRequest("updTerritorySaleGroup", "updTerritorySaleGroup", updateTerritorySaleGroupModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await orgSaleGroupImp.updTerritorySaleGroup(updateTerritorySaleGroupModel, userProfileForBack);
                SearchResultBase<OrgSaleGroup> searchResult = new SearchResultBase<OrgSaleGroup>();
                List<OrgSaleGroup> list = new List<OrgSaleGroup>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updTerritorySaleGroup", "updTerritorySaleGroup", res);
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
                onBeforeSendResponse("updTerritorySaleGroup", "updTerritorySaleGroup", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleArea.php" target="_blank">For Page masterdata/Organizational/SaleArea.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchSaleArea")]
        public async Task<ResponseResult> searchSaleArea(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, OrgSaleAreaSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchSaleArea", "searchSaleArea", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<OrgSaleAreaCustom> entitiySearchResult = await orgSaleAreaImp.Search(criteria);
                List<OrgSaleAreaCustom> lst = entitiySearchResult.data;
                SearchResultBase<OrgSaleAreaCustom> searchResult = new SearchResultBase<OrgSaleAreaCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchSaleArea", "searchSaleArea", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchSaleArea", "searchSaleArea", resR);
                return resR;
            }
        }


        [HttpPost("updBusinessUnitSaleArea")]
        public async Task<ResponseResult> updBusinessUnitSaleArea(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, UpdBusinessUnitSaleAreaModel updBusinessUnitSaleAreaModel)
        {
            try
            {
                onAfterReceiveRequest("updBusinessUnitSaleArea", "updBusinessUnitSaleArea", updBusinessUnitSaleAreaModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                await orgSaleGroupImp.updBusinessUnitSaleArea(updBusinessUnitSaleAreaModel, userProfileForBack);
                SearchResultBase<OrgSaleArea> searchResult = new SearchResultBase<OrgSaleArea>();
                List<OrgSaleArea> list = new List<OrgSaleArea>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updBusinessUnitSaleArea", "updBusinessUnitSaleArea", res);
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
                onBeforeSendResponse("updBusinessUnitSaleArea", "updBusinessUnitSaleArea", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleArea.php" target="_blank">For Page masterdata/Organizational/SaleArea.php</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/BusinessUnit.php" target="_blank">For Page masterdata/Organizational/BusinessUnit.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchBusinessUnit")]
        public async Task<ResponseResult> searchBusinessUnit(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, OrgBusinessUnitSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchBusinessUnit", "searchBusinessUnit", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<OrgBusinessUnit> entitiySearchResult = await orgBusinessUnitImp.Search(criteria, userProfileForBack);
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
                onBeforeSendResponse("searchBusinessUnit", "searchBusinessUnit", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchBusinessUnit", "searchBusinessUnit", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleArea.php" target="_blank">For Page masterdata/Organizational/SaleArea.php</a>
        /// <p>Required Paramerer</p>
        /// <para>buId</para>
        /// <p>areaId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("mapBU")]
        public async Task<ResponseResult> mapBU(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, OrgSaleAreaModel orgSaleAreamodel)
        {
            try
            {
                onAfterReceiveRequest("mapBU", "mapBU", orgSaleAreamodel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                orgSaleAreamodel.UserProfile = userProfileForBack;
                await orgSaleAreaImp.MapBU(orgSaleAreamodel);
                SearchResultBase<OrgSaleArea> searchResult = new SearchResultBase<OrgSaleArea>();
                List<OrgSaleArea> list = new List<OrgSaleArea>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("mapBU", "mapBU", res);
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
                onBeforeSendResponse("mapBU", "mapBU", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/BusinessUnit.php" target="_blank">For Page masterdata/Organizational/BusinessUnit.php</a>
        /// <p>Required Paramerer</p>
        /// <para>BuCode</para>
        /// <p>buNameTh</p>
        /// <p>buNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addBusinessUnit")]
        public async Task<ResponseResult> addBusinessUnit(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, BusinessUnitModel gusinessUnitModel)
        {
            try
            {
                onAfterReceiveRequest("addBusinessUnit", "addBusinessUnit", gusinessUnitModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                gusinessUnitModel.UserProfile = userProfileForBack;
                OrgBusinessUnit model = await orgBusinessUnitImp.Add(gusinessUnitModel);
                SearchResultBase<OrgBusinessUnit> searchResult = new SearchResultBase<OrgBusinessUnit>();
                List<OrgBusinessUnit> list = new List<OrgBusinessUnit>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addBusinessUnit", "addBusinessUnit", res);
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
                onBeforeSendResponse("addBusinessUnit", "addBusinessUnit", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/BusinessUnit.php" target="_blank">For Page masterdata/Organizational/BusinessUnit.php</a>
        /// <p>Required Paramerer</p>
        /// <para>buId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updBusinessUnit")]
        public async Task<ResponseResult> updBusinessUnit(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, BusinessUnitModel gusinessUnitModel)
        {
            try
            {
                onAfterReceiveRequest("updBusinessUnit", "updBusinessUnit", gusinessUnitModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                gusinessUnitModel.UserProfile = userProfileForBack;
                await orgBusinessUnitImp.Update(gusinessUnitModel);
                SearchResultBase<OrgBusinessUnit> searchResult = new SearchResultBase<OrgBusinessUnit>();
                List<OrgBusinessUnit> list = new List<OrgBusinessUnit>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updBusinessUnit", "updBusinessUnit", res);
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
                onBeforeSendResponse("updBusinessUnit", "updBusinessUnit", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/BusinessUnit.php" target="_blank">For Page masterdata/Organizational/BusinessUnit.php</a>
        /// <p>Required Paramerer</p>
        /// <para>buId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelBusinessUnit")]
        public async Task<ResponseResult> cancelBusinessUnit(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, BusinessUnitModel gusinessUnitModel)
        {
            try
            {
                onAfterReceiveRequest("cancelBusinessUnit", "cancelBusinessUnit", gusinessUnitModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                gusinessUnitModel.UserProfile = userProfileForBack;
                await orgBusinessUnitImp.DeleteUpdate(gusinessUnitModel);
                SearchResultBase<OrgBusinessUnit> searchResult = new SearchResultBase<OrgBusinessUnit>();
                List<OrgBusinessUnit> list = new List<OrgBusinessUnit>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("deleteBusinessUnit", "deleteBusinessUnit", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("deleteBusinessUnit", "deleteBusinessUnit", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Plant.php" target="_blank">For Page masterdata/Organizational/Plant.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchPlantShip")]
        public async Task<ResponseResult> searchPlantShip(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, MsPlantShipSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchPlantShip", "searchPlantShip", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsPlantShipCustom> entitiySearchResult = await msPlantShipImp.SearchForPlant(criteria);
                List<MsPlantShipCustom> lst = entitiySearchResult.data;
                SearchResultBase<MsPlantShipCustom> searchResult = new SearchResultBase<MsPlantShipCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchPlantShip", "searchPlantShip", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchPlantShip", "searchPlantShip", resR);
                return resR;
            }
        }

        /*
        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/ShippingPoint.php" target="_blank">For Page masterdata/Organizational/ShippingPoint.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchForShippingPoint")]
        public async Task<ResponseResult> searchForShippingPoint(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, MsPlantShipSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchForShippingPoint", "searchForShippingPoint", criteria);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsPlantShipCustom> entitiySearchResult = await msPlantShipImp.SearchForShippingPoint(criteria);
                List<MsPlantShipCustom> lst = entitiySearchResult.data;
                SearchResultBase<MsPlantShipCustom> searchResult = new SearchResultBase<MsPlantShipCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchForShippingPoint", "searchForShippingPoint", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchForShippingPoint", "searchForShippingPoint", resR);
                return resR;
            }
        }
        */



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Territory.php" target="_blank">For Page masterdata/Organizational/Territory.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchTerritory")]
        public async Task<ResponseResult> searchTerritory(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, OrgTerritorySearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchTerritory", "searchTerritory", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<OrgTerritory> entitiySearchResult = await orgTerritoryImp.Search(criteria);
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
                onBeforeSendResponse("searchTerritory", "searchTerritory", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchTerritory", "searchTerritory", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Territory.php" target="_blank">For Page masterdata/Organizational/Territory.php</a>
        /// <p>Required Paramerer</p>
        /// <para>TerritoryCode</para>
        /// <p>TerritoryNameTh</p>
        /// <p>TerritoryNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addTerritory")]
        public async Task<ResponseResult> addTerritory(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, OrgTerritoryModel orgTerritoryModel)
        {
            try
            {
                onAfterReceiveRequest("addTerritory", "addTerritory", orgTerritoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                orgTerritoryModel.UserProfile = userProfileForBack;
                OrgTerritory model = await orgTerritoryImp.Add(orgTerritoryModel);
                SearchResultBase<OrgTerritory> searchResult = new SearchResultBase<OrgTerritory>();
                List<OrgTerritory> list = new List<OrgTerritory>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addTerritory", "addTerritory", res);
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
                onBeforeSendResponse("addTerritory", "addTerritory", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Territory.php" target="_blank">For Page masterdata/Organizational/Territory.php</a>
        /// <p>Required Paramerer</p>
        /// <para>territoryId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updTerritory")]
        public async Task<ResponseResult> updTerritory(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, OrgTerritoryModel orgTerritoryModel)
        {
            try
            {
                onAfterReceiveRequest("updTerritory", "updTerritory", orgTerritoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                orgTerritoryModel.UserProfile = userProfileForBack;
                await orgTerritoryImp.Update(orgTerritoryModel);
                SearchResultBase<OrgTerritory> searchResult = new SearchResultBase<OrgTerritory>();
                List<OrgTerritory> list = new List<OrgTerritory>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updTerritory", "updTerritory", res);
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
                onBeforeSendResponse("updTerritory", "updTerritory", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Territory.php" target="_blank">For Page masterdata/Organizational/Territory.php</a>
        /// <p>Required Paramerer</p>
        /// <para>territoryId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelTerritory")]
        public async Task<ResponseResult> cancelTerritory(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, OrgTerritoryModel orgTerritoryModel)
        {
            try
            {
                onAfterReceiveRequest("cancelTerritory", "cancelTerritory", orgTerritoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                orgTerritoryModel.UserProfile = userProfileForBack;
                await orgTerritoryImp.DeleteUate(orgTerritoryModel);
                SearchResultBase<OrgTerritory> searchResult = new SearchResultBase<OrgTerritory>();
                List<OrgTerritory> list = new List<OrgTerritory>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelTerritory", "cancelTerritory", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelTerritory", "cancelTerritory", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Territory.php" target="_blank">For Page masterdata/Organizational/Territory.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchEmpByTerritoryId")]
        public async Task<ResponseResult> searchEmpByTerritoryId(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, OrgSaleTerritorySearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchEmpByTerritoryId", "searchEmpByTerritoryId", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchInitialSaleRepCustom> entitiySearchResult = await orgTerritoryImp.SearchInitialSaleRep(criteria);
                List<SearchInitialSaleRepCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchInitialSaleRepCustom> searchResult = new SearchResultBase<SearchInitialSaleRepCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchEmpByTerritoryId", "searchEmpByTerritoryId", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchEmpByTerritoryId", "searchEmpByTerritoryId", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Territory.php" target="_blank">For Page masterdata/Organizational/Territory.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchEmpForMapSaleTerritory")]
        public async Task<ResponseResult> searchEmpForMapSaleTerritory(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, OrgSearchRepSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchEmpForMapSaleTerritory", "searchEmpForMapSaleTerritory", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<OrgSearchRepCustom> entitiySearchResult = await orgTerritoryImp.SearchRep(criteria);
                List<OrgSearchRepCustom> lst = entitiySearchResult.data;
                SearchResultBase<OrgSearchRepCustom> searchResult = new SearchResultBase<OrgSearchRepCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchEmpForMapSaleTerritory", "searchEmpForMapSaleTerritory", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchEmpForMapSaleTerritory", "searchEmpForMapSaleTerritory", resR);
                return resR;
            }
        }


        [HttpPost("searchSaleGroupForMapSaleTerritory")]
        public async Task<ResponseResult> searchSaleGroupForMapSaleTerritory(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchSaleGroupForMapSaleTerritoryCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchSaleGroupForMapSaleTerritory", "searchSaleGroupForMapSaleTerritory", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchSaleGroupForMapSaleTerritoryCustom> entitiySearchResult = await orgTerritoryImp.searchSaleGroupForMapSaleTerritory(criteria);
                List<SearchSaleGroupForMapSaleTerritoryCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchSaleGroupForMapSaleTerritoryCustom> searchResult = new SearchResultBase<SearchSaleGroupForMapSaleTerritoryCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchSaleGroupForMapSaleTerritory", "searchSaleGroupForMapSaleTerritory", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchSaleGroupForMapSaleTerritory", "searchSaleGroupForMapSaleTerritory", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Territory.php" target="_blank">For Page masterdata/Organizational/Territory.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TerritoryId</p>
        /// <p>EmpId[]</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addSaleTerritory")]
        public async Task<ResponseResult> addSaleTerritory(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, OrgMapSaleRepModel orgMapSaleRepModel)
        {
            try
            {
                onAfterReceiveRequest("addSaleTerritory", "addSaleTerritory", orgMapSaleRepModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                orgMapSaleRepModel.UserProfile = userProfileForBack;
                List<OrgSaleTerritory> list = await orgTerritoryImp.MapSaleRep(orgMapSaleRepModel);
                SearchResultBase<OrgSaleTerritory> searchResult = new SearchResultBase<OrgSaleTerritory>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addSaleTerritory", "addSaleTerritory", res);
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
                onBeforeSendResponse("addSaleTerritory", "addSaleTerritory", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Territory.php" target="_blank">For Page masterdata/Organizational/Territory.php</a>
        /// <p>Required Paramerer</p>
        /// <para>saleTerritoryId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("delSaleTerritory")]
        public async Task<ResponseResult> delSaleTerritory(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, OrgSaleTerritoryModel orgSaleTerritoryModel)
        {
            try
            {
                onAfterReceiveRequest("delSaleTerritory", "delSaleTerritory", orgSaleTerritoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                orgSaleTerritoryModel.UserProfile = userProfileForBack;
                int model = await orgTerritoryImp.DellMapSaleRep(orgSaleTerritoryModel, userProfileForBack);
                SearchResultBase<OrgSaleTerritory> searchResult = new SearchResultBase<OrgSaleTerritory>();
                List<OrgSaleTerritory> list = new List<OrgSaleTerritory> ();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("delSaleTerritory", "delSaleTerritory", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("delSaleTerritory", "delSaleTerritory", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Brand.php" target="_blank">For Page masterdata/Organizational/Brand.php</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/basic/" target="_blank">For Page prospect/basic/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchBrand")]
        public async Task<ResponseResult> searchBrand(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, MsBrandSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchBrand", "searchBrand", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsBrand> entitiySearchResult = await msBrandImp.Search(criteria);
                List<MsBrand> lst = entitiySearchResult.data;
                SearchResultBase<MsBrand> searchResult = new SearchResultBase<MsBrand>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchBrand", "searchBrand", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchBrand", "searchBrand", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Brand.php" target="_blank">For Page masterdata/Organizational/Brand.php</a>
        /// <p>Required Paramerer</p>
        /// <p>brandCode</p>
        /// <p>brandNameTh</p>
        /// <p>brandNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addBrand")]
        public async Task<ResponseResult> addBrand(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, MsBrandModel msBrandModel)
        {
            try
            {
                onAfterReceiveRequest("addBrand", "addBrand", msBrandModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msBrandModel.UserProfile = userProfileForBack;
                MsBrand model = await msBrandImp.Add(msBrandModel);
                SearchResultBase<MsBrand> searchResult = new SearchResultBase<MsBrand>();
                List<MsBrand> list = new List<MsBrand>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addBrand", "addBrand", res);
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
                onBeforeSendResponse("addBrand", "addBrand", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Brand.php" target="_blank">For Page masterdata/Organizational/Brand.php</a>
        /// <p>Required Paramerer</p>
        /// <p>brandId</p>
        /// <p>brandCode</p>
        /// <p>brandNameTh</p>
        /// <p>brandNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updBrand")]
        public async Task<ResponseResult> updBrand(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsBrandModel msBrandModel)
        {
            try
            {
                onAfterReceiveRequest("updBrand", "updBrand", msBrandModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msBrandModel.UserProfile = userProfileForBack;
                await msBrandImp.Update(msBrandModel);
                SearchResultBase<MsBrand> searchResult = new SearchResultBase<MsBrand>();
                List<MsBrand> list = new List<MsBrand>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updBrand", "updBrand", res);
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
                onBeforeSendResponse("updBrand", "updBrand", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Brand.php" target="_blank">For Page masterdata/Organizational/Brand.php</a>
        /// <p>Required Paramerer</p>
        /// <p>brandId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelBrand")]
        public async Task<ResponseResult> cancelBrand(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsBrandModel msBrandModel)
        {
            try
            {
                onAfterReceiveRequest("cancelBrand", "cancelBrand", msBrandModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msBrandModel.UserProfile = userProfileForBack;
                await msBrandImp.DeleteUpdate(msBrandModel);
                SearchResultBase<MsBrand> searchResult = new SearchResultBase<MsBrand>();
                List<MsBrand> list = new List<MsBrand>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelBrand", "cancelBrand", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelBrand", "cancelBrand", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Location.php" target="_blank">For Page masterdata/Organizational/Location.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchLocType")]
        public async Task<ResponseResult> searchLocType(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, MsLocationTypeSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchLocType", "searchLocType", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsLocationType> entitiySearchResult = await msLocationTypeImp.Search(criteria);
                List<MsLocationType> lst = entitiySearchResult.data;
                SearchResultBase<MsLocationType> searchResult = new SearchResultBase<MsLocationType>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchLocType", "searchLocType", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchLocType", "searchLocType", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Location.php" target="_blank">For Page masterdata/Organizational/Location.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchLoc")]
        public async Task<ResponseResult> searchLoc(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, MsLocationSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchLoc", "searchLoc", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchLocationCustom> entitiySearchResult = await msLocationImp.Search(criteria);
                List<SearchLocationCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchLocationCustom> searchResult = new SearchResultBase<SearchLocationCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchLoc", "searchLoc", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchLoc", "searchLoc", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Location.php" target="_blank">For Page masterdata/Organizational/Location.php</a>
        /// <p>Required Paramerer</p>
        /// <p>LocTypeId</p>
        /// <p>LocCode</p>
        /// <p>LocNameTh</p>
        /// <p>LocNameEn</p>
        /// <p>ProvinceCode</p>
        /// <p>Latitude</p>
        /// <p>Longitude</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addLoc")]
        public async Task<ResponseResult> addLoc(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, MsLocationModel msLocationModel)
        {
            try
            {
                onAfterReceiveRequest("addLoc", "addLoc", msLocationModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msLocationModel.UserProfile = userProfileForBack;
                MsLocation model = await msLocationImp.Add(msLocationModel);
                SearchResultBase<MsLocation> searchResult = new SearchResultBase<MsLocation>();
                List<MsLocation> list = new List<MsLocation>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addLoc", "addLoc", res);
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
                onBeforeSendResponse("addLoc", "addLoc", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Location.php" target="_blank">For Page masterdata/Organizational/Location.php</a>
        /// <p>Required Paramerer</p>
        /// <p>LocId</p>
        /// <p>LocTypeId</p>
        /// <p>LocCode</p>
        /// <p>LocNameTh</p>
        /// <p>LocNameEn</p>
        /// <p>ProvinceCode</p>
        /// <p>Latitude</p>
        /// <p>Longitude</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updLoc")]
        public async Task<ResponseResult> updLoc(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsLocationModel msLocationModel)
        {
            try
            {
                onAfterReceiveRequest("updLoc", "updLoc", msLocationModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msLocationModel.UserProfile = userProfileForBack;
                await msLocationImp.Update(msLocationModel);
                SearchResultBase< MsLocation> searchResult = new SearchResultBase<MsLocation>();
                List<MsLocation> list = new List<MsLocation>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updLoc", "updLoc", res);
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
                onBeforeSendResponse("updLoc", "updLoc", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Location.php" target="_blank">For Page masterdata/Organizational/Location.php</a>
        /// <p>Required Paramerer</p>
        /// <p>LocId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelLoc")]
        public async Task<ResponseResult> cancelLoc(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsLocationModel msLocationModel)
        {
            try
            {
                onAfterReceiveRequest("cancelLoc", "cancelLoc", msLocationModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msLocationModel.UserProfile = userProfileForBack;
                await msLocationImp.DeleteUpdate(msLocationModel);
                SearchResultBase<MsLocation> searchResult = new SearchResultBase<MsLocation>();
                List<MsLocation> list = new List<MsLocation>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelLoc", "cancelLoc", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelLoc", "cancelLoc", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/LocationType.php" target="_blank">For Page masterdata/Organizational/LocationType.php</a>
        /// <p>Required Paramerer</p>
        /// <p>LocTypeCode</p>
        /// <p>LocTypeNameTh</p>
        /// <p>LocTypeNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addLocType")]
        public async Task<ResponseResult> addLocType(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, MsLocationTypeModel msLocationTypeModel)
        {
            try
            {
                onAfterReceiveRequest("addLocType", "addLocType", msLocationTypeModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msLocationTypeModel.UserProfile = userProfileForBack;
                MsLocationType model = await msLocationTypeImp.Add(msLocationTypeModel);
                SearchResultBase< MsLocationType> searchResult = new SearchResultBase<MsLocationType>();
                List<MsLocationType> list = new List<MsLocationType>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addLocType", "addLocType", res);
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
                onBeforeSendResponse("addLocType", "addLocType", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/LocationType.php" target="_blank">For Page masterdata/Organizational/LocationType.php</a>
        /// <p>Required Paramerer</p>
        /// <p>LocTypeId</p>
        /// <p>LocTypeCode</p>
        /// <p>LocTypeNameTh</p>
        /// <p>LocTypeNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updLocType")]
        public async Task<ResponseResult> updLocType(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsLocationTypeModel msLocationTypeModel)
        {
            try
            {
                onAfterReceiveRequest("updLocType", "updLocType", msLocationTypeModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msLocationTypeModel.UserProfile = userProfileForBack;
                await msLocationTypeImp.Update(msLocationTypeModel);
                SearchResultBase<MsLocationType> searchResult = new SearchResultBase<MsLocationType>();
                List<MsLocationType> list = new List<MsLocationType>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updLocType", "updLocType", res);
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
                onBeforeSendResponse("updLocType", "updLocType", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/LocationType.php" target="_blank">For Page masterdata/Organizational/LocationType.php</a>
        /// <p>Required Paramerer</p>
        /// <p>LocTypeId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelLocType")]
        public async Task<ResponseResult> cancelLocType(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsLocationTypeModel msLocationTypeModel)
        {
            try
            {
                onAfterReceiveRequest("cancelLocType", "cancelLocType", msLocationTypeModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msLocationTypeModel.UserProfile = userProfileForBack;
                await msLocationTypeImp.DeleteUpdate(msLocationTypeModel);
                SearchResultBase<MsLocationType> searchResult = new SearchResultBase<MsLocationType>();
                List<MsLocationType> list = new List<MsLocationType>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelLocType", "cancelLocType", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelLocType", "cancelLocType", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Categories.php" target="_blank">For Page masterdata/Organizational/Categories.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchBrandCate")]
        public async Task<ResponseResult> searchBrandCate(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, MsBrandCategorySearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchBrandCate", "searchBrandCate", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsBrandCategory> entitiySearchResult = await msBrandCategoryImp.Search(criteria);
                List<MsBrandCategory> lst = entitiySearchResult.data;
                SearchResultBase< MsBrandCategory> searchResult = new SearchResultBase<MsBrandCategory>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchBrandCate", "searchBrandCate", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchBrandCate", "searchBrandCate", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Categories.php" target="_blank">For Page masterdata/Organizational/Categories.php</a>
        /// <p>Required Paramerer</p>
        /// <p>BrandCateCode</p>
        /// <p>BrandCateNameTh</p>
        /// <p>BrandCateNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addBrandCate")]
        public async Task<ResponseResult> addBrandCate(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, MsBrandCategoryModel msBrandCategoryModel)
        {
            try
            {
                onAfterReceiveRequest("addBrandCate", "addBrandCate", msBrandCategoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msBrandCategoryModel.UserProfile = userProfileForBack;
                MsBrandCategory model = await msBrandCategoryImp.Add(msBrandCategoryModel);
                SearchResultBase< MsBrandCategory> searchResult = new SearchResultBase<MsBrandCategory>();
                List<MsBrandCategory> list = new List<MsBrandCategory>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addBrandCate", "addBrandCate", res);
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
                onBeforeSendResponse("addBrandCate", "addBrandCate", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Categories.php" target="_blank">For Page masterdata/Organizational/Categories.php</a>
        /// <p>Required Paramerer</p>
        /// <p>BrandCateId</p>
        /// <p>BrandCateCode</p>
        /// <p>BrandCateNameTh</p>
        /// <p>BrandCateNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updBrandCate")]
        public async Task<ResponseResult> updBrandCate(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsBrandCategoryModel msBrandCategoryModel)
        {
            try
            {
                onAfterReceiveRequest("updBrandCate", "updBrandCate", msBrandCategoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msBrandCategoryModel.UserProfile = userProfileForBack;
                await msBrandCategoryImp.Update(msBrandCategoryModel);
                SearchResultBase<MsBrandCategory> searchResult = new SearchResultBase<MsBrandCategory>();
                List<MsBrandCategory> list = new List<MsBrandCategory>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updBrandCate", "updBrandCate", res);
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
                onBeforeSendResponse("updBrandCate", "updBrandCate", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Categories.php" target="_blank">For Page masterdata/Organizational/Categories.php</a>
        /// <p>Required Paramerer</p>
        /// <p>BrandCateId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelBrandCate")]
        public async Task<ResponseResult> cancelBrandCate(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsBrandCategoryModel msBrandCategoryModel)
        {
            try
            {
                onAfterReceiveRequest("cancelBrandCate", "cancelBrandCate", msBrandCategoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msBrandCategoryModel.UserProfile = userProfileForBack;
                await msBrandCategoryImp.DeleteUpdate(msBrandCategoryModel);
                SearchResultBase<MsBrandCategory> searchResult = new SearchResultBase<MsBrandCategory>();
                List<MsBrandCategory> list = new List<MsBrandCategory>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelBrandCate", "cancelBrandCate", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelBrandCate", "cancelBrandCate", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Service.php" target="_blank">For Page masterdata/Organizational/Service.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchServiceType")]
        public async Task<ResponseResult> searchServiceType(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, MsServiceTypeSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchServiceType", "searchServiceType", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsServiceType> entitiySearchResult = await msServiceTypeImp.Search(criteria);
                List<MsServiceType> lst = entitiySearchResult.data;
                SearchResultBase< MsServiceType> searchResult = new SearchResultBase<MsServiceType>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchServiceType", "searchServiceType", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchServiceType", "searchServiceType", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Service.php" target="_blank">For Page masterdata/Organizational/Service.php</a>
        /// <p>Required Paramerer</p>
        /// <p>ServiceCode</p>
        /// <p>ServiceNameTh</p>
        /// <p>ServiceNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addServiceType")]
        public async Task<ResponseResult> addServiceType(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, MsServiceTypeModel msServiceTypeModel)
        {
            try
            {
                onAfterReceiveRequest("addServiceType", "addServiceType", msServiceTypeModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msServiceTypeModel.UserProfile = userProfileForBack;
                MsServiceType model = await msServiceTypeImp.Add(msServiceTypeModel);
                SearchResultBase<MsServiceType> searchResult = new SearchResultBase<MsServiceType>();
                List<MsServiceType> list = new List<MsServiceType>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addServiceType", "addServiceType", res);
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
                onBeforeSendResponse("addServiceType", "addServiceType", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Service.php" target="_blank">For Page masterdata/Organizational/Service.php</a>
        /// <p>Required Paramerer</p>
        /// <p>ServiceId</p>
        /// <p>ServiceCode</p>
        /// <p>ServiceNameTh</p>
        /// <p>ServiceNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updServiceType")]
        public async Task<ResponseResult> updServiceType(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsServiceTypeModel msServiceTypeModel)
        {
            try
            {
                onAfterReceiveRequest("updServiceType", "updServiceType", msServiceTypeModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msServiceTypeModel.UserProfile = userProfileForBack;
                await msServiceTypeImp.Update(msServiceTypeModel);
                SearchResultBase<MsServiceType> searchResult = new SearchResultBase<MsServiceType>();
                List<MsServiceType> list = new List<MsServiceType>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updServiceType", "updServiceType", res);
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
                onBeforeSendResponse("updServiceType", "updServiceType", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Service.php" target="_blank">For Page masterdata/Organizational/Service.php</a>
        /// <p>Required Paramerer</p>
        /// <p>ServiceId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelServiceType")]
        public async Task<ResponseResult> cancelServiceType(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsServiceTypeModel msServiceTypeModel)
        {
            try
            {
                onAfterReceiveRequest("cancelServiceType", "cancelServiceType", msServiceTypeModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msServiceTypeModel.UserProfile = userProfileForBack;
                await msServiceTypeImp.DeleteUpdate(msServiceTypeModel);
                SearchResultBase<MsServiceType> searchResult = new SearchResultBase<MsServiceType>();
                List<MsServiceType> list = new List<MsServiceType>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelServiceType", "cancelServiceType", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelServiceType", "cancelServiceType", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Bank.php" target="_blank">For Page masterdata/Organizational/Bank.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchBank")]
        public async Task<ResponseResult> searchBank(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<MsBankCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchBank", "searchBank", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsBank> entitiySearchResult = await msBankImp.Search(criteria);
                List<MsBank> lst = entitiySearchResult.data;
                SearchResultBase<MsBank> searchResult = new SearchResultBase<MsBank>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchBank", "searchBank", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchBank", "searchBank", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Bank.php" target="_blank">For Page masterdata/Organizational/Bank.php</a>
        /// <p>Required Paramerer</p>
        /// <p>BankCode</p>
        /// <p>BankNameTh</p>
        /// <p>BankNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addBank")]
        public async Task<ResponseResult> addBank(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, MsBankModel msBankModel)
        {
            try
            {
                onAfterReceiveRequest("addBank", "addBank", msBankModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msBankModel.UserProfile = userProfileForBack;
                MsBank model = await msBankImp.Add(msBankModel);
                SearchResultBase<MsBank> searchResult = new SearchResultBase<MsBank>();
                List<MsBank> list = new List<MsBank>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addBank", "addBank", res);
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
                onBeforeSendResponse("addBank", "addBank", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Bank.php" target="_blank">For Page masterdata/Organizational/Bank.php</a>
        /// <p>Required Paramerer</p>
        /// <p>BankId</p>
        /// <p>BankCode</p>
        /// <p>BankNameTh</p>
        /// <p>BankNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updBank")]
        public async Task<ResponseResult> updBank(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsBankModel msBankModel)
        {
            try
            {
                onAfterReceiveRequest("updBank", "updBank", msBankModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msBankModel.UserProfile = userProfileForBack;
                await msBankImp.Update(msBankModel);
                SearchResultBase<MsBank> searchResult = new SearchResultBase<MsBank>();
                List<MsBank> list = new List<MsBank>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updBank", "updBank", res);
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
                onBeforeSendResponse("updBank", "updBank", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Bank.php" target="_blank">For Page masterdata/Organizational/Bank.php</a>
        /// <p>Required Paramerer</p>
        /// <p>BankId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelBank")]
        public async Task<ResponseResult> cancelBank(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsBankModel msBankModel)
        {
            try
            {
                onAfterReceiveRequest("cancelBank", "cancelBank", msBankModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msBankModel.UserProfile = userProfileForBack;
                await msBankImp.DeleteUpdate(msBankModel);
                SearchResultBase<MsBank> searchResult = new SearchResultBase<MsBank>();
                List<MsBank> list = new List<MsBank>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelBank", "cancelBank", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelBank", "cancelBank", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateCategory.php" target="_blank">For Page masterdata/Organizational/TemplateCategory.php</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Template.php" target="_blank">For Page masterdata/Organizational/Template.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchTemplateCate")]
        public async Task<ResponseResult> searchTemplateCate(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<TemplateCategoryCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchTemplateCate", "searchTemplateCate", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<TemplateCategory> entitiySearchResult = await templateCategoryImp.Search(criteria);
                List<TemplateCategory> lst = entitiySearchResult.data;
                SearchResultBase<TemplateCategory> searchResult = new SearchResultBase<TemplateCategory>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchTemplateCate", "searchTemplateCate", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchTemplateCate", "searchTemplateCate", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateCategory.php" target="_blank">For Page masterdata/Organizational/TemplateCategory.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpCateCode</p>
        /// <p>TpCateNameTh</p>
        /// <p>TpCateNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addTemplateCate")]
        public async Task<ResponseResult> addTemplateCate(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, TemplateCategoryModel templateCategoryModel)
        {
            try
            {
                onAfterReceiveRequest("addTemplateCate", "addTemplateCate", templateCategoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateCategoryModel.UserProfile = userProfileForBack;
                TemplateCategory model = await templateCategoryImp.Add(templateCategoryModel);
                SearchResultBase<TemplateCategory> searchResult = new SearchResultBase<TemplateCategory>();
                List<TemplateCategory> list = new List<TemplateCategory>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addTemplateCate", "addTemplateCate", res);
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
                onBeforeSendResponse("addTemplateCate", "addTemplateCate", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateCategory.php" target="_blank">For Page masterdata/Organizational/TemplateCategory.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpCateId</p>
        /// <p>TpCateCode</p>
        /// <p>TpCateNameTh</p>
        /// <p>TpCateNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updTemplateCate")]
        public async Task<ResponseResult> updTemplateCate(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateCategoryModel templateCategoryModel)
        {
            try
            {
                onAfterReceiveRequest("updTemplateCate", "updTemplateCate", templateCategoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateCategoryModel.UserProfile = userProfileForBack;
                await templateCategoryImp.Update(templateCategoryModel);
                SearchResultBase<TemplateCategory> searchResult = new SearchResultBase<TemplateCategory>();
                List<TemplateCategory> list = new List<TemplateCategory>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updTemplateCate", "updTemplateCate", res);
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
                onBeforeSendResponse("updTemplateCate", "updTemplateCate", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateCategory.php" target="_blank">For Page masterdata/Organizational/TemplateCategory.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpCateId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelTemplateCate")]
        public async Task<ResponseResult> cancelTemplateCate(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateCategoryModel templateCategoryModel)
        {
            try
            {
                onAfterReceiveRequest("cancelTemplateCate", "cancelTemplateCate", templateCategoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateCategoryModel.UserProfile = userProfileForBack;
                await templateCategoryImp.DeleteUpdate(templateCategoryModel);
                SearchResultBase<TemplateCategory> searchResult = new SearchResultBase<TemplateCategory>();
                List<TemplateCategory> list = new List<TemplateCategory>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelTemplateCate", "cancelTemplateCate", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelTemplateCate", "cancelTemplateCate", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Template.php" target="_blank">For Page masterdata/Organizational/Template.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchTemplateAppForm")]
        public async Task<ResponseResult> searchTemplateAppForm(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<TemplateAppFormCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchTemplateAppForm", "searchTemplateAppForm", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<TemplateAppForm> entitiySearchResult = await templateAppFormImp.Search(criteria, userProfileForBack);
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


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Template.php" target="_blank">For Page masterdata/Organizational/Template.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpCode</p>
        /// <p>TpNameTh</p>
        /// <p>TpNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addTemplateAppForm")]
        public async Task<ResponseResult> addTemplateAppForm(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, TemplateAppFormModel templateAppFormModel)
        {
            try
            {
                onAfterReceiveRequest("addTemplateAppForm", "addTemplateAppForm", templateAppFormModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateAppFormModel.UserProfile = userProfileForBack;
                TemplateAppForm model = await templateAppFormImp.Add(templateAppFormModel);
                SearchResultBase<TemplateAppForm> searchResult = new SearchResultBase<TemplateAppForm>();
                List<TemplateAppForm> list = new List<TemplateAppForm>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addTemplateAppForm", "addTemplateAppForm", res);
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
                onBeforeSendResponse("addTemplateAppForm", "addTemplateAppForm", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Template.php" target="_blank">For Page masterdata/Organizational/Template.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpAppFormId</p>
        /// <p>TpCode</p>
        /// <p>TpNameTh</p>
        /// <p>TpNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updTemplateAppForm")]
        public async Task<ResponseResult> updTemplateAppForm(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateAppFormModel templateAppFormModel)
        {
            try
            {
                onAfterReceiveRequest("updTemplateAppForm", "updTemplateAppForm", templateAppFormModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateAppFormModel.UserProfile = userProfileForBack;
                await templateAppFormImp.Update(templateAppFormModel);
                SearchResultBase<TemplateAppForm> searchResult = new SearchResultBase<TemplateAppForm>();
                List<TemplateAppForm> list = new List<TemplateAppForm>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updTemplateAppForm", "updTemplateAppForm", res);
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
                onBeforeSendResponse("updTemplateAppForm", "updTemplateAppForm", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Template.php" target="_blank">For Page masterdata/Organizational/Template.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpAppFormId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelTemplateAppForm")]
        public async Task<ResponseResult> cancelTemplateAppForm(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateAppFormModel templateAppFormModel)
        {
            try
            {
                onAfterReceiveRequest("cancelTemplateAppForm", "cancelTemplateAppForm", templateAppFormModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateAppFormModel.UserProfile = userProfileForBack;
                await templateAppFormImp.DeleteUpdate(templateAppFormModel);
                SearchResultBase<TemplateAppForm> searchResult = new SearchResultBase<TemplateAppForm>();
                List<TemplateAppForm> list = new List<TemplateAppForm>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelTemplateAppForm", "cancelTemplateAppForm", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelTemplateAppForm", "cancelTemplateAppForm", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Template.php" target="_blank">For Page masterdata/Organizational/Template.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchTemplateAppQuestionById")]
        public async Task<ResponseResult> searchTemplateAppQuestionById(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<TemplateAppQuestionCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchTemplateAppQuestionById", "searchTemplateAppQuestionById", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchTemplateAppQuestionByIdCustom> entitiySearchResult = await templateAppQuestionImp.searchTemplateAppQuestionById(criteria);
                List<SearchTemplateAppQuestionByIdCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchTemplateAppQuestionByIdCustom> searchResult = new SearchResultBase<SearchTemplateAppQuestionByIdCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchTemplateAppQuestionById", "searchTemplateAppQuestionById", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchTemplateAppQuestionById", "searchTemplateAppQuestionById", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Template.php" target="_blank">For Page masterdata/Organizational/Template.php</a>
        /// <p>Required Paramerer</p>
        /// TemplateAppQuestionModel[]
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addTemplateAppQuestion")]
        public async Task<ResponseResult> addTemplateAppQuestion(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, List<TemplateAppQuestionModel> templateAppQuestionModel)
        {
            try
            {
                onAfterReceiveRequest("addTemplateAppQuestion", "addTemplateAppQuestion", templateAppQuestionModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                List<TemplateAppQuestion> list = await templateAppQuestionImp.Add(templateAppQuestionModel, userProfileForBack);
                SearchResultBase<TemplateAppQuestion> searchResult = new SearchResultBase<TemplateAppQuestion>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addTemplateAppQuestion", "addTemplateAppQuestion", res);
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
                onBeforeSendResponse("addTemplateAppQuestion", "addTemplateAppQuestion", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Template.php" target="_blank">For Page masterdata/Organizational/Template.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpAppQuestionId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updTemplateAppQuestion")]
        public async Task<ResponseResult> updTemplateAppQuestion(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateAppQuestionModel templateAppQuestionModel)
        {
            try
            {
                onAfterReceiveRequest("updTemplateAppQuestion", "updTemplateAppQuestion", templateAppQuestionModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateAppQuestionModel.UserProfile = userProfileForBack;
                await templateAppQuestionImp.Update(templateAppQuestionModel);
                SearchResultBase<TemplateAppQuestion> searchResult = new SearchResultBase<TemplateAppQuestion>();
                List<TemplateAppQuestion> list = new List<TemplateAppQuestion>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updTemplateAppQuestion", "updTemplateAppQuestion", res);
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
                onBeforeSendResponse("updTemplateAppQuestion", "updTemplateAppQuestion", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Template.php" target="_blank">For Page masterdata/Organizational/Template.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpAppQuestionId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelTemplateAppQuestion")]
        public async Task<ResponseResult> cancelTemplateAppQuestion(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateAppQuestionModel templateAppQuestionModel)
        {
            try
            {
                onAfterReceiveRequest("cancelTemplateAppQuestion", "cancelTemplateAppQuestion", templateAppQuestionModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateAppQuestionModel.UserProfile = userProfileForBack;
                await templateAppQuestionImp.DeleteUpdate(templateAppQuestionModel);
                SearchResultBase<TemplateAppQuestion> searchResult = new SearchResultBase<TemplateAppQuestion>();
                List<TemplateAppQuestion> list = new List<TemplateAppQuestion>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelTemplateAppQuestion", "cancelTemplateAppQuestion", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelTemplateAppQuestion", "cancelTemplateAppQuestion", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Question.php" target="_blank">For Page masterdata/Organizational/Question.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchTemplateQuestion")]
        public async Task<ResponseResult> searchTemplateQuestion(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<TemplateQuestionCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchTemplateQuestion", "searchTemplateQuestion", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchTemplateQuestionCustom> entitiySearchResult = await templateQuestionImp.searchTemplateQuestion(criteria, userProfileForBack);
                List<SearchTemplateQuestionCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchTemplateQuestionCustom> searchResult = new SearchResultBase<SearchTemplateQuestionCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchTemplateQuestion", "searchTemplateQuestion", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchTemplateQuestion", "searchTemplateQuestion", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Question.php" target="_blank">For Page masterdata/Organizational/Question.php</a>
        /// <p>Required Paramerer</p>
        /// <p>QuestionCode</p>
        /// <p>QuestionNameTh</p>
        /// <p>QuestionNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addTemplateQuestion")]
        public async Task<ResponseResult> addTemplateQuestion(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, TemplateQuestionModel templateQuestionModel)
        {
            try
            {
                onAfterReceiveRequest("addTemplateQuestion", "addTemplateQuestion", templateQuestionModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateQuestionModel.UserProfile = userProfileForBack;
                TemplateQuestion model = await templateQuestionImp.Add(templateQuestionModel);
                SearchResultBase<TemplateQuestion> searchResult = new SearchResultBase<TemplateQuestion>();
                List<TemplateQuestion> list = new List<TemplateQuestion>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addTemplateQuestion", "addTemplateQuestion", res);
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
                onBeforeSendResponse("addTemplateQuestion", "addTemplateQuestion", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Question.php" target="_blank">For Page masterdata/Organizational/Question.php</a>
        /// <p>Required Paramerer</p>
        /// <p>QuestionId</p>
        /// <p>QuestionCode</p>
        /// <p>QuestionNameTh</p>
        /// <p>QuestionNameEn</p>
        /// <p>ActiveFlag</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updTemplateQuestion")]
        public async Task<ResponseResult> updTemplateQuestion(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateQuestionModel templateQuestionModel)
        {
            try
            {
                onAfterReceiveRequest("updTemplateQuestion", "updTemplateQuestion", templateQuestionModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateQuestionModel.UserProfile = userProfileForBack;
                await templateQuestionImp.Update(templateQuestionModel);
                SearchResultBase<TemplateQuestion> searchResult = new SearchResultBase<TemplateQuestion>();
                List<TemplateQuestion> list = new List<TemplateQuestion>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updTemplateQuestion", "updTemplateQuestion", res);
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
                onBeforeSendResponse("updTemplateQuestion", "updTemplateQuestion", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Question.php" target="_blank">For Page masterdata/Organizational/Question.php</a>
        /// <p>Required Paramerer</p>
        /// <p>QuestionId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelTemplateQuestion")]
        public async Task<ResponseResult> cancelTemplateQuestion(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateQuestionModel templateQuestionModel)
        {
            try
            {
                onAfterReceiveRequest("cancelTemplateQuestion", "cancelTemplateQuestion", templateQuestionModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateQuestionModel.UserProfile = userProfileForBack;
                await templateQuestionImp.DeleteUpdate(templateQuestionModel);
                SearchResultBase<TemplateQuestion> searchResult = new SearchResultBase<TemplateQuestion>();
                List<TemplateQuestion> list = new List<TemplateQuestion>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelTemplateQuestion", "cancelTemplateQuestion", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelTemplateQuestion", "cancelTemplateQuestion", resR);
                return resR;
            }
        }









        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/ActtachmentCategories.php" target="_blank">For Page masterdata/Organizational/ActtachmentCategories.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchAttachCate")]
        public async Task<ResponseResult> searchAttachCate(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<MsActtachCategoryCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAttachCate", "searchAttachCate", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsAttachCategory> entitiySearchResult = await msActtachCategoryImp.Search(criteria);
                List<MsAttachCategory> lst = entitiySearchResult.data;
                SearchResultBase<MsAttachCategory> searchResult = new SearchResultBase<MsAttachCategory>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchAttachCate", "searchAttachCate", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchAttachCate", "searchAttachCate", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/ActtachmentCategories.php" target="_blank">For Page masterdata/Organizational/ActtachmentCategories.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addAttachCate")]
        public async Task<ResponseResult> addAttachCate(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, MsActtachCategoryModel msActtachCategoryModel)
        {
            try
            {
                onAfterReceiveRequest("addAttachCate", "addAttachCate", msActtachCategoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msActtachCategoryModel.UserProfile = userProfileForBack;
                MsAttachCategory model = await msActtachCategoryImp.Add(msActtachCategoryModel);
                SearchResultBase<MsAttachCategory> searchResult = new SearchResultBase<MsAttachCategory>();
                List<MsAttachCategory> list = new List<MsAttachCategory>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addAttachCate", "addAttachCate", res);
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
                onBeforeSendResponse("addAttachCate", "addAttachCate", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/ActtachmentCategories.php" target="_blank">For Page masterdata/Organizational/ActtachmentCategories.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpAppQuestionId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updAttachCate")]
        public async Task<ResponseResult> updAttachCate(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsActtachCategoryModel msActtachCategoryModel)
        {
            try
            {
                onAfterReceiveRequest("updAttachCate", "updAttachCate", msActtachCategoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msActtachCategoryModel.UserProfile = userProfileForBack;
                await msActtachCategoryImp.Update(msActtachCategoryModel);
                SearchResultBase<MsAttachCategory> searchResult = new SearchResultBase<MsAttachCategory>();
                List<MsAttachCategory> list = new List<MsAttachCategory>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updAttachCate", "updAttachCate", res);
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
                onBeforeSendResponse("updAttachCate", "updAttachCate", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/ActtachmentCategories.php" target="_blank">For Page masterdata/Organizational/ActtachmentCategories.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpAppQuestionId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelAttachCate")]
        public async Task<ResponseResult> cancelAttachCate(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsActtachCategoryModel msActtachCategoryModel)
        {
            try
            {
                onAfterReceiveRequest("cancelAttachCate", "cancelAttachCate", msActtachCategoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msActtachCategoryModel.UserProfile = userProfileForBack;
                await msActtachCategoryImp.DeleteUpdate(msActtachCategoryModel);
                SearchResultBase<MsAttachCategory> searchResult = new SearchResultBase<MsAttachCategory>();
                List<MsAttachCategory> list = new List<MsAttachCategory>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelAttachCate", "cancelAttachCate", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelAttachCate", "cancelAttachCate", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Territory.php" target="_blank">For Page masterdata/Organizational/Territory.php</a>
        /// <p>Required Paramerer</p>
        /// <para>buId</para>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updTerritoryByManagerEmpId")]
        public async Task<ResponseResult> updTerritoryByManagerEmpId(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, OrgTerritoryModel orgTerritoryModel)
        {
            try
            {
                onAfterReceiveRequest("updTerritoryByManagerEmpId", "updTerritoryByManagerEmpId", orgTerritoryModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                orgTerritoryModel.UserProfile = userProfileForBack;
                await orgTerritoryImp.updTerritoryByManagerEmpId(orgTerritoryModel);
                SearchResultBase<OrgTerritory> searchResult = new SearchResultBase<OrgTerritory>();
                List<OrgTerritory> list = new List<OrgTerritory>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updTerritoryByManagerEmpId", "updTerritoryByManagerEmpId", res);
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
                onBeforeSendResponse("updTerritoryByManagerEmpId", "updTerritoryByManagerEmpId", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateStockCard.php" target="_blank">For Page masterdata/Organizational/TemplateStockCard.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchTemplateStockCard")]
        public async Task<ResponseResult> searchTemplateStockCard(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<TemplateStockCardCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchTemplateStockCard", "searchTemplateStockCard", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<TemplateStockCard> entitiySearchResult = await templateStockCardImp.Search(criteria);
                List<TemplateStockCard> lst = entitiySearchResult.data;
                SearchResultBase<TemplateStockCard> searchResult = new SearchResultBase<TemplateStockCard>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchTemplateStockCard", "searchTemplateStockCard", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchTemplateStockCard", "searchTemplateStockCard", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateStockCard.php" target="_blank">For Page masterdata/Organizational/TemplateStockCard.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpCode</p>
        /// <p>TpNameTh</p>
        /// <p>TpNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addTemplateStockCard")]
        public async Task<ResponseResult> addTemplateStockCard(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, TemplateStockCardModel tremplateStockCardModel)
        {
            try
            {
                onAfterReceiveRequest("addTemplateStockCard", "addTemplateStockCard", tremplateStockCardModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                tremplateStockCardModel.UserProfile = userProfileForBack;
                TemplateStockCard model = await templateStockCardImp.Add(tremplateStockCardModel);
                SearchResultBase<TemplateStockCard> searchResult = new SearchResultBase<TemplateStockCard>();
                List<TemplateStockCard> list = new List<TemplateStockCard>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addTemplateStockCard", "addTemplateStockCard", res);
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
                onBeforeSendResponse("addTemplateStockCard", "addTemplateStockCard", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateStockCard.php" target="_blank">For Page masterdata/Organizational/TemplateStockCard.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpStockCardId</p>
        /// <p>TpCode</p>
        /// <p>TpNameTh</p>
        /// <p>TpNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updTemplateStockCard")]
        public async Task<ResponseResult> updTemplateStockCard(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateStockCardModel tremplateStockCardModel)
        {
            try
            {
                onAfterReceiveRequest("updTemplateStockCard", "updTemplateStockCard", tremplateStockCardModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                tremplateStockCardModel.UserProfile = userProfileForBack;
                await templateStockCardImp.Update(tremplateStockCardModel);
                SearchResultBase<TemplateStockCard> searchResult = new SearchResultBase<TemplateStockCard>();
                List<TemplateStockCard> list = new List<TemplateStockCard>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updTemplateStockCard", "updTemplateStockCard", res);
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
                onBeforeSendResponse("updTemplateStockCard", "updTemplateStockCard", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateStockCard.php" target="_blank">For Page masterdata/Organizational/TemplateStockCard.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpStockCardId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelTemplateStockCard")]
        public async Task<ResponseResult> cancelTemplateStockCard(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateStockCardModel tremplateStockCardModel)
        {
            try
            {
                onAfterReceiveRequest("cancelTemplateStockCard", "cancelTemplateStockCard", tremplateStockCardModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                tremplateStockCardModel.UserProfile = userProfileForBack;
                await templateStockCardImp.DeleteUpdate(tremplateStockCardModel);
                SearchResultBase<TemplateStockCard> searchResult = new SearchResultBase<TemplateStockCard>();
                List<TemplateStockCard> list = new List<TemplateStockCard>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelTemplateStockCard", "cancelTemplateStockCard", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelTemplateStockCard", "cancelTemplateStockCard", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateStockCard.php" target="_blank">For Page masterdata/Organizational/TemplateStockCard.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchTemplateByStockCardId")]
        public async Task<ResponseResult> searchTemplateByStockCardId(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<TemplateStockProductCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchTemplateByStockCardId", "searchTemplateByStockCardId", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchTemplateByStockCardCustom> entitiySearchResult = await templateStockProductImp.searchTemplateByStockCardId(criteria);
                List<SearchTemplateByStockCardCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchTemplateByStockCardCustom> searchResult = new SearchResultBase<SearchTemplateByStockCardCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchTemplateByStockCardId", "searchTemplateByStockCardId", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchTemplateByStockCardId", "searchTemplateByStockCardId", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Product.php" target="_blank">For Page masterdata/Organizational/Product.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchProduct")]
        public async Task<ResponseResult> searchProduct(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<MsProductCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchProduct", "searchProduct", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchProductCustomCustom> entitiySearchResult = await msProductImp.searchProduct(criteria);
                List<SearchProductCustomCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchProductCustomCustom> searchResult = new SearchResultBase<SearchProductCustomCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchProduct", "searchProduct", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchProduct", "searchProduct", resR);
                return resR;
            }
        }



        


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Product.php" target="_blank">For Page masterdata/Organizational/Product.php</a>
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
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Product.php" target="_blank">For Page masterdata/Organizational/Product.php</a>
        /// <p>Required Paramerer</p>
        /// <p>ReportProdConvId</p>
        /// <p>ProdCode</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updateReportProductConversion")]
        public async Task<ResponseResult> updateReportProductConversion(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, UpdateReportProductConversionModel updateReportProductConversionModel)
        {
            try
            {
                onAfterReceiveRequest("updateReportProductConversion", "updateReportProductConversion", updateReportProductConversionModel);
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                updateReportProductConversionModel.UserProfile = userProfileForBack;
                //
                await msProductImp.updateReportProductConversion(updateReportProductConversionModel);
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updateReportProductConversion", "updateReportProductConversion", res);
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
                onBeforeSendResponse("updateReportProductConversion", "updateReportProductConversion", resR);
                return resR;
            }
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateStockCard.php" target="_blank">For Page masterdata/Organizational/TemplateStockCard.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addTemplateStockProduct")]
        public async Task<ResponseResult> addTemplateStockProduct(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, TemplateStockProductModel templateStockProductModel)
        {
            try
            {
                onAfterReceiveRequest("addTemplateStockProduct", "addTemplateStockProduct", templateStockProductModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateStockProductModel.UserProfile = userProfileForBack;
                TemplateStockProduct model = null;
                SearchResultBase<TemplateStockProduct> searchResult = new SearchResultBase<TemplateStockProduct>();
                List<TemplateStockProduct> list = await templateStockProductImp.addTemplateStockProduct(templateStockProductModel);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addTemplateStockProduct", "addTemplateStockProduct", res);
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
                onBeforeSendResponse("addTemplateStockProduct", "addTemplateStockProduct", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateStockCard.php" target="_blank">For Page masterdata/Organizational/TemplateStockCard.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpStockCardId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelTemplateStockProduct")]
        public async Task<ResponseResult> cancelTemplateStockProduct(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateStockProductModel templateStockProductModel)
        {
            try
            {
                onAfterReceiveRequest("cancelTemplateStockProduct", "cancelTemplateStockProduct", templateStockProductModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateStockProductModel.UserProfile = userProfileForBack;
                await templateStockProductImp.DeleteUpdate(templateStockProductModel);
                SearchResultBase<TemplateStockProduct> searchResult = new SearchResultBase<TemplateStockProduct>();
                List<TemplateStockProduct> list = new List<TemplateStockProduct>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelTemplateStockProduct", "cancelTemplateStockProduct", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelTemplateStockProduct", "cancelTemplateStockProduct", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateStockCard.php" target="_blank">For Page masterdata/Organizational/TemplateStockCard.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpStockCardId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("delTemplateStockProduct")]
        public async Task<ResponseResult> delTemplateStockProduct(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateStockProductModel templateStockProductModel)
        {
            try
            {
                onAfterReceiveRequest("delTemplateStockProduct", "delTemplateStockProduct", templateStockProductModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateStockProductModel.UserProfile = userProfileForBack;
                await templateStockProductImp.delTemplateStockProduct(templateStockProductModel);
                SearchResultBase<TemplateStockProduct> searchResult = new SearchResultBase<TemplateStockProduct>();
                List<TemplateStockProduct> list = new List<TemplateStockProduct>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("delTemplateStockProduct", "delTemplateStockProduct", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("delTemplateStockProduct", "delTemplateStockProduct", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateForSA.php" target="_blank">For Page masterdata/Organizational/TemplateForSA.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchTemplateSaForm")]
        public async Task<ResponseResult> searchTemplateSaForm(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<TemplateSaFormCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchTemplateSaForm", "searchTemplateSaForm", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchTemplateSaFormCustom> entitiySearchResult = await templateSaFormImp.searchTemplateSaForm(criteria);
                List<SearchTemplateSaFormCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchTemplateSaFormCustom> searchResult = new SearchResultBase<SearchTemplateSaFormCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchTemplateSaForm", "searchTemplateSaForm", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchTemplateSaForm", "searchTemplateSaForm", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateForSA.php" target="_blank">For Page masterdata/Organizational/TemplateForSA.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpCode</p>
        /// <p>TpNameTh</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addTemplateSaForm")]
        public async Task<ResponseResult> addTemplateSaForm(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, TemplateSaFormModel templateSaFormModel)
        {
            try
            {
                onAfterReceiveRequest("addTemplateSaForm", "addTemplateSaForm", templateSaFormModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateSaFormModel.UserProfile = userProfileForBack;
                TemplateSaForm model = await templateSaFormImp.Add(templateSaFormModel);
                SearchResultBase<TemplateSaForm> searchResult = new SearchResultBase<TemplateSaForm>();
                List<TemplateSaForm> list = new List<TemplateSaForm>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addTemplateSaForm", "addTemplateSaForm", res);
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
                onBeforeSendResponse("addTemplateSaForm", "addTemplateSaForm", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateForSA.php" target="_blank">For Page masterdata/Organizational/TemplateForSA.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpSaFormId</p>
        /// <p>TpCode</p>
        /// <p>TpNameTh</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updTemplateSaForm")]
        public async Task<ResponseResult> updTemplateSaForm(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateSaFormModel templateSaFormModel)
        {
            try
            {
                onAfterReceiveRequest("updTemplateSaForm", "updTemplateSaForm", templateSaFormModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateSaFormModel.UserProfile = userProfileForBack;
                await templateSaFormImp.Update(templateSaFormModel);
                SearchResultBase<TemplateSaForm> searchResult = new SearchResultBase<TemplateSaForm>();
                List<TemplateSaForm> list = new List<TemplateSaForm>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updTemplateSaForm", "updTemplateSaForm", res);
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
                onBeforeSendResponse("updTemplateSaForm", "updTemplateSaForm", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateForSA.php" target="_blank">For Page masterdata/Organizational/TemplateForSA.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpSaFormId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelTemplateSaForm")]
        public async Task<ResponseResult> cancelTemplateSaForm(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateSaFormModel templateSaFormModel)
        {
            try
            {
                onAfterReceiveRequest("cancelTemplateSaForm", "cancelTemplateSaForm", templateSaFormModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateSaFormModel.UserProfile = userProfileForBack;
                await templateSaFormImp.DeleteUpdate(templateSaFormModel);
                SearchResultBase<TemplateSaForm> searchResult = new SearchResultBase<TemplateSaForm>();
                List<TemplateSaForm> list = new List<TemplateSaForm>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelTemplateSaForm", "cancelTemplateSaForm", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelTemplateSaForm", "cancelTemplateSaForm", resR);
                return resR;
            }
        }

        
        
        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateForSA.php" target="_blank">For Page masterdata/Organizational/TemplateForSA.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchTemplateSaFormById")]
        public async Task<ResponseResult> searchTemplateSaFormById(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<TemplateSaTitleCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchTemplateSaFormById", "searchTemplateSaFormById", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchTemplateSaFormByIdCustom> entitiySearchResult = await templateSaTitleImp.searchTemplateSaFormById(criteria);
                List<SearchTemplateSaFormByIdCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchTemplateSaFormByIdCustom> searchResult = new SearchResultBase<SearchTemplateSaFormByIdCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchTemplateSaFormById", "searchTemplateSaFormById", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchTemplateSaFormById", "searchTemplateSaFormById", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateForSA.php" target="_blank">For Page masterdata/Organizational/TemplateForSA.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TitleNameTh</p>
        /// <p>TitleNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addTemplateSaTitle")]
        public async Task<ResponseResult> addTemplateSaTitle(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, TemplateSaTitleModel templateSaTitleModel)
        {
            try
            {
                onAfterReceiveRequest("addTemplateSaTitle", "addTemplateSaTitle", templateSaTitleModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateSaTitleModel.UserProfile = userProfileForBack;
                TemplateSaTitle model = await templateSaTitleImp.addTemplateSaTitle(templateSaTitleModel);
                SearchResultBase<TemplateSaTitle> searchResult = new SearchResultBase<TemplateSaTitle>();
                List<TemplateSaTitle> list = new List<TemplateSaTitle>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addTemplateSaTitle", "addTemplateSaTitle", res);
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
                onBeforeSendResponse("addTemplateSaTitle", "addTemplateSaTitle", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/TemplateForSA.php" target="_blank">For Page masterdata/Organizational/TemplateForSA.php</a>
        /// <p>Required Paramerer</p>
        /// <p>TpSaTitleId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("delTemplateSaTitle")]
        public async Task<ResponseResult> delTemplateSaTitle(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, TemplateSaTitleModel templateSaTitleModel)
        {
            try
            {
                onAfterReceiveRequest("delTemplateSaTitle", "delTemplateSaTitle", templateSaTitleModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                templateSaTitleModel.UserProfile = userProfileForBack;
                int model = await templateSaTitleImp.delTemplateSaTitle(templateSaTitleModel);
                SearchResultBase<TemplateSaForm> searchResult = new SearchResultBase<TemplateSaForm>();
                List<TemplateSaForm> list = new List<TemplateSaForm>();
                //list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("delTemplateSaTitle", "delTemplateSaTitle", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("delTemplateSaTitle", "delTemplateSaTitle", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/products.php" target="_blank">For Page masterdata/Organizational/products.php</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/QRMaster-2.php" target="_blank">For Page masterdata/Organizational/QRMaster-2.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchGasoline")]
        public async Task<ResponseResult> searchGasoline(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<MsGasolineCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchGasoline", "searchGasoline", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsGasoline> entitiySearchResult = await msGasolineImp.Search(criteria);
                List<MsGasoline> lst = entitiySearchResult.data;
                SearchResultBase<MsGasoline> searchResult = new SearchResultBase<MsGasoline>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchGasoline", "searchGasoline", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchGasoline", "searchGasoline", resR);
                return resR;
            }
        }


        /*
        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/QRMaster-2.php" target="_blank">For Page masterdata/Organizational/QRMaster-2.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchGasoline")]
        public async Task<ResponseResult> searchGasoline(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, GasOnlineSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchGasoline", "searchGasoline", criteria);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsGasoline> entitiySearchResult = await gasOnlineImp.Search(criteria);
                List<MsGasoline> lst = entitiySearchResult.data;
                SearchResultBase<MsGasoline> searchResult = new SearchResultBase<MsGasoline>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchGasoline", "searchGasoline", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchGasoline", "searchGasoline", resR);
                return resR;
            }
        }
        */


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/QRMaster-2.php" target="_blank">For Page masterdata/Organizational/QRMaster-2.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchCustomer")]
        public async Task<ResponseResult> searchCustomer(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<CustomerCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchCustomer", "searchCustomer", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchCustomerCustom> entitiySearchResult = await customerImp.searchCustomer(criteria);
                List<SearchCustomerCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchCustomerCustom> searchResult = new SearchResultBase<SearchCustomerCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchCustomer", "searchCustomer", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchCustomer", "searchCustomer", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/QRMaster-2.php" target="_blank">For Page masterdata/Organizational/QRMaster-2.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchGasolineByCust")]
        public async Task<ResponseResult> searchGasolineByCust(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, QRSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchGasolineByCust", "searchGasolineByCust", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<QRCustom> entitiySearchResult = await customSpacialImp.searchGasolineByCust(criteria);
                List<QRCustom> lst = entitiySearchResult.data;
                SearchResultBase<QRCustom> searchResult = new SearchResultBase<QRCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchGasolineByCust", "searchGasolineByCust", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchGasolineByCust", "searchGasolineByCust", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/products.php" target="_blank">For Page masterdata/Organizational/products.php</a>
        /// <p>Required Paramerer</p>
        /// <p>GasNameTh</p>
        /// <p>GasNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addGasoline")]
        public async Task<ResponseResult> addGasoline(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, MsGasolineModel msGasolineModel)
        {
            try
            {
                onAfterReceiveRequest("addGasoline", "addGasoline", msGasolineModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msGasolineModel.UserProfile = userProfileForBack;
                MsGasoline model = await msGasolineImp.Add(msGasolineModel);
                SearchResultBase<MsGasoline> searchResult = new SearchResultBase<MsGasoline>();
                List<MsGasoline> list = new List<MsGasoline>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addGasoline", "addGasoline", res);
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
                onBeforeSendResponse("addGasoline", "addGasoline", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/products.php" target="_blank">For Page masterdata/Organizational/products.php</a>
        /// <p>Required Paramerer</p>
        /// <p>GasId</p>
        /// <p>GasNameTh</p>
        /// <p>GasNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updGasoline")]
        public async Task<ResponseResult> updGasoline(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsGasolineModel msGasolineModel)
        {
            try
            {
                onAfterReceiveRequest("updGasoline", "updGasoline", msGasolineModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msGasolineModel.UserProfile = userProfileForBack;
                await msGasolineImp.Update(msGasolineModel);
                SearchResultBase<MsGasoline> searchResult = new SearchResultBase<MsGasoline>();
                List<MsGasoline> list = new List<MsGasoline>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updGasoline", "updGasoline", res);
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
                onBeforeSendResponse("updGasoline", "updGasoline", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/products.php" target="_blank">For Page masterdata/Organizational/products.php</a>
        /// <p>Required Paramerer</p>
        /// <p>GasId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelGasoline")]
        public async Task<ResponseResult> cancelGasoline(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsGasolineModel msGasolineModel)
        {
            try
            {
                onAfterReceiveRequest("cancelGasoline", "cancelGasoline", msGasolineModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msGasolineModel.UserProfile = userProfileForBack;
                await msGasolineImp.DeleteUpdate(msGasolineModel);
                SearchResultBase<MsGasoline> searchResult = new SearchResultBase<MsGasoline>();
                List<MsGasoline> list = new List<MsGasoline>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelGasoline", "cancelGasoline", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelGasoline", "cancelGasoline", resR);
                return resR;
            }
        }

        
        

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/ReasonnotVisit.php" target="_blank">For Page masterdata/Organizational/ReasonnotVisit.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchPlanReasonNotVisit")]
        public async Task<ResponseResult> searchPlanReasonNotVisit(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<PlanReasonNotVisitCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchPlanReasonNotVisit", "searchPlanReasonNotVisit", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<PlanReasonNotVisit> entitiySearchResult = await planReasonNotVisitImp.Search(criteria);
                List<PlanReasonNotVisit> lst = entitiySearchResult.data;
                SearchResultBase<PlanReasonNotVisit> searchResult = new SearchResultBase<PlanReasonNotVisit>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchPlanReasonNotVisit", "searchPlanReasonNotVisit", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchPlanReasonNotVisit", "searchPlanReasonNotVisit", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/ReasonnotVisit.php" target="_blank">For Page masterdata/Organizational/ReasonnotVisit.php</a>
        /// <p>Required Paramerer</p>
        /// <p>ReasonCode</p>
        /// <p>ReasonNameTh</p>
        /// <p>ReasonNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addPlanReasonNotVisit")]
        public async Task<ResponseResult> addPlanReasonNotVisit(
     [FromHeader(Name = "Accept-Language")] string language,
     [FromHeader(Name = "User-Agent")] string agent, PlanReasonNotVisitModel planReasonNotVisitModel)
        {
            try
            {
                onAfterReceiveRequest("addPlanReasonNotVisit", "addPlanReasonNotVisit", planReasonNotVisitModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                planReasonNotVisitModel.UserProfile = userProfileForBack;
                PlanReasonNotVisit model = await planReasonNotVisitImp.Add(planReasonNotVisitModel);
                SearchResultBase<PlanReasonNotVisit> searchResult = new SearchResultBase<PlanReasonNotVisit>();
                List<PlanReasonNotVisit> list = new List<PlanReasonNotVisit>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addPlanReasonNotVisit", "addPlanReasonNotVisit", res);
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
                onBeforeSendResponse("addPlanReasonNotVisit", "addPlanReasonNotVisit", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/ReasonnotVisit.php" target="_blank">For Page masterdata/Organizational/ReasonnotVisit.php</a>
        /// <p>Required Paramerer</p>
        /// <p>ReasonNotVisitId</p>
        /// <p>ReasonCode</p>
        /// <p>ReasonNameTh</p>
        /// <p>ReasonNameEn</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updPlanReasonNotVisit")]
        public async Task<ResponseResult> updPlanReasonNotVisit(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, PlanReasonNotVisitModel planReasonNotVisitModel)
        {
            try
            {
                onAfterReceiveRequest("updPlanReasonNotVisit", "updPlanReasonNotVisit", planReasonNotVisitModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                planReasonNotVisitModel.UserProfile = userProfileForBack;
                await planReasonNotVisitImp.Update(planReasonNotVisitModel);
                SearchResultBase<PlanReasonNotVisit> searchResult = new SearchResultBase<PlanReasonNotVisit>();
                List<PlanReasonNotVisit> list = new List<PlanReasonNotVisit>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updPlanReasonNotVisit", "updPlanReasonNotVisit", res);
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
                onBeforeSendResponse("updPlanReasonNotVisit", "updPlanReasonNotVisit", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/ReasonnotVisit.php" target="_blank">For Page masterdata/Organizational/ReasonnotVisit.php</a>
        /// <p>Required Paramerer</p>
        /// <p>ReasonNotVisitId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelPlanReasonNotVisit")]
        public async Task<ResponseResult> cancelPlanReasonNotVisit(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, PlanReasonNotVisitModel planReasonNotVisitModel)
        {
            try
            {
                onAfterReceiveRequest("cancelPlanReasonNotVisit", "cancelPlanReasonNotVisit", planReasonNotVisitModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                planReasonNotVisitModel.UserProfile = userProfileForBack;
                await planReasonNotVisitImp.DeleteUpdate(planReasonNotVisitModel);
                SearchResultBase<PlanReasonNotVisit> searchResult = new SearchResultBase<PlanReasonNotVisit>();
                List<PlanReasonNotVisit> list = new List<PlanReasonNotVisit>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelPlanReasonNotVisit", "cancelPlanReasonNotVisit", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelPlanReasonNotVisit", "cancelPlanReasonNotVisit", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/District.php" target="_blank">For Page masterdata/Organizational/District.php</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/basic/" target="_blank">For Page prospect/basic/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchDistrict")]
        public async Task<ResponseResult> searchDistrict(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<MsDistrictCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchDistrict", "searchDistrict", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsDistrict> entitiySearchResult = await msDistrictImp.Search(criteria);
                List<MsDistrict> lst = entitiySearchResult.data;
                SearchResultBase<MsDistrict> searchResult = new SearchResultBase<MsDistrict>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchDistrict", "searchDistrict", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchDistrict", "searchDistrict", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/District.php" target="_blank">For Page masterdata/Organizational/District.php</a>
        /// <p>Required Paramerer</p>
        /// <p>DistrictCode</p>
        /// <p>ProvinceCode</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updDistrictByProvinceCode")]
        public async Task<ResponseResult> updDistrictByProvinceCode(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsDistrictModel msDistrictModel)
        {
            try
            {
                onAfterReceiveRequest("updDistrictByProvinceCode", "updDistrictByProvinceCode", msDistrictModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msDistrictModel.UserProfile = userProfileForBack;
                await msDistrictImp.updDistrictByProvinceCode(msDistrictModel);
                SearchResultBase<MsDistrict> searchResult = new SearchResultBase<MsDistrict>();
                List<MsDistrict> list = new List<MsDistrict>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updDistrictByProvinceCode", "updDistrictByProvinceCode", res);
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
                onBeforeSendResponse("updDistrictByProvinceCode", "updDistrictByProvinceCode", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Sub-district.php" target="_blank">For Page masterdata/Organizational/Sub-district.php</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/basic/" target="_blank">For Page prospect/basic/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchSubDistrict")]
        public async Task<ResponseResult> searchSubDistrict(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<MsSubDistrictCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchSubDistrict", "searchSubDistrict", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchSubDistrictCustom> entitiySearchResult = await msSubDistrictImp.Search(criteria);
                List<SearchSubDistrictCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchSubDistrictCustom> searchResult = new SearchResultBase<SearchSubDistrictCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchSubDistrict", "searchSubDistrict", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchSubDistrict", "searchSubDistrict", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Sub-district.php" target="_blank">For Page masterdata/Organizational/Sub-district.php</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/basic/" target="_blank">For Page prospect/basic/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("delSubDistrictSomById")]
        public async Task<ResponseResult> delSubDistrictSomById(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, DelSubDistrictSomByIdModel delSubDistrictSomByIdModel)
        {
            try
            {
                onAfterReceiveRequest("delSubDistrictSomById", "delSubDistrictSomById", delSubDistrictSomByIdModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                int model = await msSubDistrictImp.delSubDistrictSomById(delSubDistrictSomByIdModel);
                SearchResultBase<MsSubdistrict> searchResult = new SearchResultBase<MsSubdistrict>();
                List<MsSubdistrict> list = new List<MsSubdistrict>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("delSubDistrictSomById", "delSubDistrictSomById", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("delSubDistrictSomById", "delSubDistrictSomById", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Sub-district.php" target="_blank">For Page masterdata/Organizational/Sub-district.php</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/prospect/basic/" target="_blank">For Page prospect/basic/</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchMsSubDistrict")]
        public async Task<ResponseResult> searchMsSubDistrict(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchMsSubDistrictCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchMsSubDistrict", "searchMsSubDistrict", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchMsSubDistrictCustom> entitiySearchResult = await msSubDistrictImp.searchMsSubDistrict(criteria);
                List<SearchMsSubDistrictCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchMsSubDistrictCustom> searchResult = new SearchResultBase<SearchMsSubDistrictCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchMsSubDistrict", "searchMsSubDistrict", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchMsSubDistrict", "searchMsSubDistrict", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/Sub-district.php" target="_blank">For Page masterdata/Organizational/Sub-district.php</a>
        /// <p>Required Paramerer</p>
        /// <p>DistrictCode</p>
        /// <p>SubdistrictCode</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updSubDistrictByDistrictCode")]
        public async Task<ResponseResult> updSubDistrictByDistrictCode(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, MsSubDistrictModel msSubDistrictModel)
        {
            try
            {
                onAfterReceiveRequest("updSubDistrictByDistrictCode", "updSubDistrictByDistrictCode", msSubDistrictModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                msSubDistrictModel.UserProfile = userProfileForBack;
                await msSubDistrictImp.updSubDistrictByDistrictCode(msSubDistrictModel, language);
                SearchResultBase<MsSubdistrict> searchResult = new SearchResultBase<MsSubdistrict>();
                List<MsSubdistrict> list = new List<MsSubdistrict>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updSubDistrictByDistrictCode", "updSubDistrictByDistrictCode", res);
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
                onBeforeSendResponse("updSubDistrictByDistrictCode", "updSubDistrictByDistrictCode", resR);
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
        [HttpPost("getConfigLov")]
        public async Task<ResponseResult> getConfigLov(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<MsConfigLovCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getConfigLov", "getConfigLov", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsConfigLov> entitiySearchResult = await msConfigLovImp.Search(criteria);
                List<MsConfigLov> lst = entitiySearchResult.data;
                SearchResultBase<MsConfigLov> searchResult = new SearchResultBase<MsConfigLov>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getConfigLov", "getConfigLov", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getConfigLov", "getConfigLov", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// getConfigParam
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("getConfigParam")]
        public async Task<ResponseResult> getConfigParam(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<MsConfigParamCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("getConfigParam", "getConfigParam", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<MsConfigParam> entitiySearchResult = await msConfigParamImp.Search(criteria);
                List<MsConfigParam> lst = entitiySearchResult.data;
                SearchResultBase<MsConfigParam> searchResult = new SearchResultBase<MsConfigParam>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("getConfigParam", "getConfigParam", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getConfigParam", "getConfigParam", resR);
                return resR;
            }
        }



    }

}
