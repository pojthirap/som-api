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
using MyFirstAzureWebApp.Models.adm;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Controllers
{
    [Route("adm")]
    [ApiController]
    public class ADMController : BaseController
    {

        private Logger log = LogManager.GetCurrentClassLogger();
        private IAdmEmployee admEmployeeImp;
        private IAdmGroup admGroupImp;
        private IAdmGroupUser admGroupUserImp;

        public ADMController()
        {
            admEmployeeImp = new AdmEmployeeImp();
            admGroupImp = new AdmGroupImp();
            admGroupUserImp = new AdmGroupUserImp();
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleGroup.php" target="_blank">For Page masterdata/Organizational/SaleGroup.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchAdmEmpRoleSupvisor")]
        public async Task<ResponseResult> searchAdmEmpRoleSupvisor(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, AdmEmployeeSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAdmEmpRoleSupvisor", "searchAdmEmpRoleSupvisor", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<AdmEmployee> entitiySearchResult = await admEmployeeImp.getEmpSupvisor(criteria);
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
                onBeforeSendResponse("searchAdmEmpRoleSupvisor", "searchAdmEmpRoleSupvisor", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchAdmEmpRoleSupvisor", "searchAdmEmpRoleSupvisor", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleGroup.php" target="_blank">For Page masterdata/Organizational/SaleGroup.php</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/MappingApprover.php" target="_blank">For Page masterdata/Organizational/MappingApprover.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchAdmEmp")]
        public async Task<ResponseResult> searchAdmEmp(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, AdmEmployeeSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAdmEmp", "searchAdmEmp", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchAdmEmpCustom> entitiySearchResult = await admEmployeeImp.searchAdmEmp(criteria);
                List<SearchAdmEmpCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchAdmEmpCustom> searchResult = new SearchResultBase<SearchAdmEmpCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchAdmEmp", "searchAdmEmp", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchAdmEmp", "searchAdmEmp", resR);
                return resR;
            }
        }


        [HttpPost("searchAdmEmpForReport")]
        public async Task<ResponseResult> searchAdmEmpForReport(
           [FromHeader(Name = "Accept-Language")] string language,
           [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<SearchAdmEmpForReportCriteria> criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAdmEmpForReport", "searchAdmEmpForReport", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<SearchAdmEmpCustom> entitiySearchResult = await admEmployeeImp.searchAdmEmpForReport(criteria);
                List<SearchAdmEmpCustom> lst = entitiySearchResult.data;
                SearchResultBase<SearchAdmEmpCustom> searchResult = new SearchResultBase<SearchAdmEmpCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchAdmEmpForReport", "searchAdmEmpForReport", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchAdmEmpForReport", "searchAdmEmpForReport", resR);
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
        [HttpPost("searchAdmEmpForUpdSaleGroup")]
        public async Task<ResponseResult> searchAdmEmpForUpdSaleGroup(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, AdmEmployeeSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAdmEmpForUpdSaleGroup", "searchAdmEmpForUpdSaleGroup", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<AdmEmployee> entitiySearchResult = await admEmployeeImp.SearchSaleRep(criteria);
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
                onBeforeSendResponse("searchAdmEmpForUpdSaleGroup", "searchAdmEmpForUpdSaleGroup", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchAdmEmpForUpdSaleGroup", "searchAdmEmpForUpdSaleGroup", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleGroup.php" target="_blank">For Page masterdata/Organizational/SaleGroup.php</a>
        /// <p>Required Paramerer</p>
        /// <para>groupCode</para>
        /// <p>empIdList[]</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updAdmEmpBySaleGroup")]
        public async Task<ResponseResult> updAdmEmpBySaleGroup(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, AdmEmployeeModel admEmployeeModel)
        {
            try
            {
                onAfterReceiveRequest("updAdmEmpBySaleGroup", "updAdmEmpBySaleGroup", admEmployeeModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                admEmployeeModel.UserProfile = userProfileForBack;
                await admEmployeeImp.UpdateSaleGroupToSaleRep(admEmployeeModel);
                SearchResultBase<AdmEmployee> searchResult = new SearchResultBase<AdmEmployee>();
                List<AdmEmployee> list = new List<AdmEmployee>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updAdmEmpBySaleGroup", "updAdmEmpBySaleGroup", res);
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
                onBeforeSendResponse("updAdmEmpBySaleGroup", "updAdmEmpBySaleGroup", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleGroup.php" target="_blank">For Page masterdata/Organizational/SaleGroup.php</a>
        /// <p>Required Paramerer</p>
        /// <p>empId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelSaleGroupByEmpId")]
        public async Task<ResponseResult> cancelSaleGroupByEmpId(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, AdmEmployeeModel admEmployeeModel)
        {
            try
            {
                onAfterReceiveRequest("cancelSaleGroupByEmpId", "cancelSaleGroupByEmpId", admEmployeeModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                admEmployeeModel.UserProfile = userProfileForBack;
                await admEmployeeImp.DeleteSaleGroupWithOutSaleRep(admEmployeeModel);
                SearchResultBase<AdmEmployee> searchResult = new SearchResultBase<AdmEmployee>();
                List<AdmEmployee> list = new List<AdmEmployee>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelSaleGroupByEmpId", "cancelSaleGroupByEmpId", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelSaleGroupByEmpId", "cancelSaleGroupByEmpId", resR);
                return resR;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleRep.php" target="_blank">For Page masterdata/Organizational/SaleRep.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchAdmEmpForMapRole")]
        public async Task<ResponseResult> searchAdmEmpForMapRole(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, AdmEmployeeSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAdmEmpForMapRole", "searchAdmEmpForMapRole", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<AdmEmployeeAdmGroupCustom> entitiySearchResult = await admEmployeeImp.searchAdmEmpForMapRole(criteria);
                List<AdmEmployeeAdmGroupCustom> lst = entitiySearchResult.data;
                SearchResultBase<AdmEmployeeAdmGroupCustom> searchResult = new SearchResultBase<AdmEmployeeAdmGroupCustom>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchAdmEmpForMapRole", "searchAdmEmpForMapRole", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchAdmEmpForMapRole", "searchAdmEmpForMapRole", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleRep.php" target="_blank">For Page masterdata/Organizational/SaleRep.php</a>
        /// <br></br>And<br></br>
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/RoleOnSOM.php" target="_blank">For Page masterdata/Organizational/RoleOnSOM.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchAdmGroup")]
        public async Task<ResponseResult> searchAdmGroup(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, AdmGroupSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAdmGroup", "searchAdmGroup", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<AdmGroup> entitiySearchResult = await admGroupImp.Search(criteria);
                List<AdmGroup> lst = entitiySearchResult.data;
                SearchResultBase<AdmGroup> searchResult = new SearchResultBase<AdmGroup>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchAdmGroup", "searchAdmGroup", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchAdmGroup", "searchAdmGroup", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/SaleRep.php" target="_blank">For Page masterdata/Organizational/SaleRep.php</a>
        /// <p>Required Paramerer</p>
        /// <p></p>
        /// <p>===============================================</p>
        /// <p>Action Add</p>
        /// <p>===============================================</p>
        /// <p>empId</p>
        /// <p>groupUserType</p>
        /// <p></p>
        /// <p>===============================================</p>
        /// <p>Action Update</p>
        /// <p>===============================================</p>
        /// <p>groupUserId</p>
        /// <p>groupUserType</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("mapRoleWithEmp")]
        public async Task<ResponseResult> mapRoleWithEmp(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, AdmGroupUserModel admGroupUserModel)
        {
            try
            {
                onAfterReceiveRequest("mapRoleWithEmp", "mapRoleWithEmp", admGroupUserModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                admGroupUserModel.UserProfile = userProfileForBack;
                AdmGroupUser model = null;
                List<EmployeeGroup> employeeGroups = admGroupUserModel.EmployeeGroups;
                List<AdmGroupUser> list = new List<AdmGroupUser>();

                AdmGroupUserModel admGroupUserModelInsert = new AdmGroupUserModel();
                List<EmployeeGroup> EmployeeGroupsInsert = new List<EmployeeGroup>();
                admGroupUserModelInsert.EmployeeGroups = EmployeeGroupsInsert;

                AdmGroupUserModel admGroupUserModelUpdate = new AdmGroupUserModel();
                admGroupUserModelUpdate.UserProfile = userProfileForBack;
                List<EmployeeGroup> EmployeeGroupsUpdate = new List<EmployeeGroup>();
                admGroupUserModelUpdate.EmployeeGroups = EmployeeGroupsUpdate;

                for (int i=0;i< employeeGroups.Count; i++)
                {
                    if (String.IsNullOrEmpty(employeeGroups[i].GroupUserId))
                    {
                        EmployeeGroup gm = new EmployeeGroup();
                        gm.EmpId = employeeGroups[i].EmpId;
                        gm.GroupUserId = employeeGroups[i].GroupUserId;
                        EmployeeGroupsInsert.Add(gm);
                    }
                    else
                    {
                        EmployeeGroup gm = new EmployeeGroup();
                        gm.EmpId = employeeGroups[i].EmpId;
                        gm.GroupUserId = employeeGroups[i].GroupUserId;
                        EmployeeGroupsUpdate.Add(gm);
                    }
                }// End for


                if (admGroupUserModelInsert.EmployeeGroups.Count != 0)
                {
                    for (int i = 0; i < admGroupUserModelInsert.EmployeeGroups.Count; i++)
                    {
                        model = await admGroupUserImp.Add(admGroupUserModel, admGroupUserModelInsert.EmployeeGroups[i]);
                        list.Add(model);
                    }

                }

                if (admGroupUserModelUpdate.EmployeeGroups.Count != 0)
                {
                    admGroupUserModelUpdate.GroupId = admGroupUserModel.GroupId;
                    admGroupUserModelUpdate.GroupUserType = admGroupUserModel.GroupUserType;
                    admGroupUserModelUpdate.ActiveFlag = admGroupUserModel.ActiveFlag;
                    admGroupUserModelUpdate.EffectiveDate = admGroupUserModel.EffectiveDate;
                    admGroupUserModelUpdate.ExpiryDate = admGroupUserModel.ExpiryDate;
                    admGroupUserModelUpdate.BuId = admGroupUserModel.BuId;

                    {//Update
                        await admGroupUserImp.Update(admGroupUserModelUpdate);
                    }
                }

                SearchResultBase<AdmGroupUser> searchResult = new SearchResultBase<AdmGroupUser>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("mapRoleWithEmp", "mapRoleWithEmp", res);
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
                onBeforeSendResponse("mapRoleWithEmp", "mapRoleWithEmp", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/MappingApprover.php" target="_blank">For Page masterdata/Organizational/MappingApprover.php</a>
        /// <p>Required Paramerer</p>
        /// <p>approveEmpId</p>
        /// <p>empIdList[]</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updAdmEmpByApprover")]
        public async Task<ResponseResult> updAdmEmpByApprover(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, AdmEmployeeModel admEmployeeModel)
        {
            try
            {
                onAfterReceiveRequest("updAdmEmpByApprover", "updAdmEmpByApprover", admEmployeeModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                admEmployeeModel.UserProfile = userProfileForBack;
                await admEmployeeImp.UpdateApproverToSaleRep(admEmployeeModel);
                SearchResultBase<AdmEmployee> searchResult = new SearchResultBase<AdmEmployee>();
                List<AdmEmployee> list = new List<AdmEmployee>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updAdmEmpByApprover", "updAdmEmpByApprover", res);
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
                onBeforeSendResponse("updAdmEmpByApprover", "updAdmEmpByApprover", resR);
                return resR;
            }
        }


        /*/// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/RoleOnSOM.php" target="_blank">For Page masterdata/Organizational/RoleOnSOM.php</a>
        /// <p>Required Paramerer</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("searchAdmGroup")]
        public async Task<ResponseResult> searchAdmGroup(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, AdmGroupSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAdmGroup", "searchAdmGroup", criteria);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<AdmGroup> entitiySearchResult = await admEmployeeImp.SearchAdmGroup(criteria);
                List<AdmGroup> lst = entitiySearchResult.data;
                SearchResultBase<AdmGroup> searchResult = new SearchResultBase<AdmGroup>();
                searchResult.totalRecords = entitiySearchResult.totalRecords;
                searchResult.pageNo = criteria.pageNo;
                searchResult.recordPerPage = criteria.length;
                searchResult.records = lst;
                searchResult.recordStart = criteria.startRecord;
                searchResult.totalPages = criteria.length == 0 ? 0 : searchResult.totalRecords == 0 ? 0 : (searchResult.totalRecords % searchResult.recordPerPage == 0 ? searchResult.totalRecords / searchResult.recordPerPage : ((searchResult.totalRecords / searchResult.recordPerPage) + 1));

                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("searchAdmGroup", "searchAdmGroup", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                return resR;
            }
        }*/



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/RoleOnSOM.php" target="_blank">For Page masterdata/Organizational/RoleOnSOM.php</a>
        /// <p>Required Paramerer</p>
        /// <p>GroupCode</p>
        /// <p>GroupNameTh</p>
        /// <p>GroupNameEn</p>
        /// <p>activeFlag</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("addAdmGroup")]
        public async Task<ResponseResult> addAdmGroup(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, AdmGroupModel meterModel)
        {
            try
            {
                onAfterReceiveRequest("addAdmGroup", "addAdmGroup", meterModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                meterModel.UserProfile = userProfileForBack;
                AdmGroup model = await admGroupImp.Add(meterModel);
                SearchResultBase<AdmGroup> searchResult = new SearchResultBase<AdmGroup>();
                List<AdmGroup> list = new List<AdmGroup>();
                list.Add(model);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("addAdmGroup", "addAdmGroup", res);
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
                onBeforeSendResponse("addAdmGroup", "addAdmGroup", resR);
                return resR;
            }
        }






        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/RoleOnSOM.php" target="_blank">For Page masterdata/Organizational/RoleOnSOM.php</a>
        /// <p>Required Paramerer</p>
        /// <p>GroupId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("updAdmGroup")]
        public async Task<ResponseResult> updAdmGroup(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, AdmGroupModel admGroupModel)
        {
            try
            {
                onAfterReceiveRequest("updAdmGroup", "updAdmGroup", admGroupModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                admGroupModel.UserProfile = userProfileForBack;
                await admGroupImp.Update(admGroupModel);
                SearchResultBase<AdmGroup> searchResult = new SearchResultBase<AdmGroup>();
                List<AdmGroup> list = new List<AdmGroup>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("updAdmGroup", "updAdmGroup", res);
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
                onBeforeSendResponse("updAdmGroup", "updAdmGroup", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/masterdata/Organizational/RoleOnSOM.php" target="_blank">For Page masterdata/Organizational/RoleOnSOM.php</a>
        /// <p>Required Paramerer</p>
        /// <p>GroupId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("cancelAdmGroup")]
        public async Task<ResponseResult> cancelAdmGroup(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, AdmGroupModel admGroupModel)
        {
            try
            {
                onAfterReceiveRequest("cancelAdmGroup", "cancelAdmGroup", admGroupModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                admGroupModel.UserProfile = userProfileForBack;
                await admGroupImp.DeleteUate(admGroupModel);
                SearchResultBase<AdmGroup> searchResult = new SearchResultBase<AdmGroup>();
                List<AdmGroup> list = new List<AdmGroup>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("cancelAdmGroup", "cancelAdmGroup", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("cancelAdmGroup", "cancelAdmGroup", resR);
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
        [HttpPost("searchAdmEmpRoleManager")]
        public async Task<ResponseResult> searchAdmEmpRoleManager(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, AdmEmployeeSearchCriteria criteria)
        {
            try
            {
                onAfterReceiveRequest("searchAdmEmpRoleManager", "searchAdmEmpRoleManager", criteria);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                criteria.startRecord = (criteria.length * (criteria.pageNo - 1) + 1);
                EntitySearchResultBase<AdmEmployee> entitiySearchResult = await admEmployeeImp.searchAdmEmpRoleManager(criteria, userProfileForBack);
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
                onBeforeSendResponse("searchAdmEmpRoleManager", "searchAdmEmpRoleManager", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("searchAdmEmpRoleManager", "searchAdmEmpRoleManager", resR);
                return resR;
            }
        }






    }

}
