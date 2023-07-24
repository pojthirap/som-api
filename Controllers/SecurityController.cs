using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyFirstAzureWebApp.Authentication;
using MyFirstAzureWebApp.Business.org;
using MyFirstAzureWebApp.common;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.exception;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.adm;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static MyFirstAzureWebApp.Authentication.JWTAuthenticationManager;

namespace MyFirstAzureWebApp.Controllers
{
    [Authorize]
    [Route("secure")]
    [ApiController]
    public class SecurityController : BaseController
    {
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        private Logger log = LogManager.GetCurrentClassLogger();

        private IAdmEmployee admEmployeeImp;
        private IAdmLogLogin admLogLoginImp;
        private IAdmSession admSessionImp;
        private IMsConfigParam msConfigParamImp;

        public SecurityController(IJWTAuthenticationManager jWTAuthenticationManager)
        {
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            admEmployeeImp = new AdmEmployeeImp();
            admLogLoginImp = new AdmLogloginImp();
            admSessionImp = new AdmSessionImp();
            msConfigParamImp = new MsConfigParamImp();
        }

        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <p>Required Paramerer</p>
        /// <p>Username</p>
        /// <p>Password</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ResponseResult> login(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent,[FromBody] UserCredential userCred)
        {
            try
            {
                string clientIPAddr = HttpContext.Connection.RemoteIpAddress?.ToString();
                SearchCriteriaBase<MsConfigParamCriteria> searchCriteriaMsParam = new SearchCriteriaBase<MsConfigParamCriteria>();
                MsConfigParamCriteria s = new MsConfigParamCriteria();
                s.ParamKeyword = "TOKEN_EXPIRE";
                searchCriteriaMsParam.model = s;
                EntitySearchResultBase<MsConfigParam> msParam = await msConfigParamImp.Search(searchCriteriaMsParam);
                string tokenExprie = msParam.data[0].ParamValue;
                //var token = await jWTAuthenticationManager.Authenticate(userCred.Username, userCred.Password);
                AuthenResult authenResult = await jWTAuthenticationManager.Authenticate(userCred.Username, userCred.Password, tokenExprie, false);
                var token = authenResult.Token;
                ResponseResult res;
                if (token != null)
                {
                    //
                    string ipAddress = clientIPAddr;
                    AdmLogLoginModel admLogLoginModel = new AdmLogLoginModel();
                    admLogLoginModel.UserName = userCred.Username;
                    admLogLoginModel.UserAgent = agent;
                    admLogLoginModel.Status = authenResult.Status;
                    admLogLoginModel.IpAddress = ipAddress;
                    AdmSessionModel admSessionModel = new AdmSessionModel();
                    AdmEmployeeCriteria admEmployeeCriteria = new AdmEmployeeCriteria();
                    admEmployeeCriteria.empId = userCred.Username;
                    //string sessionId = HttpContext.Session.Id;
                    //admSessionModel.SessionId = Convert.ToDecimal(sessionId);
                    admSessionModel.TokenNo = token;
                    admSessionModel.EmpId = userCred.Username;
                    admSessionModel.IpAddress = ipAddress;
                    admSessionModel.UserAgent = agent;
                    admSessionModel.TokenExpire = tokenExprie;
                    UserProfileForBackEndCustom userProfileForBackEndCustom = await createUserProfileAsync(admLogLoginModel, admSessionModel, admEmployeeCriteria);

                    if (userProfileForBackEndCustom != null)
                    {
                        //Set Object userProfile --> listOrgTerritory
                        /*SearchCriteriaBase<AdmEmployeeCriteria> searchCriteriaForUserProfile = new SearchCriteriaBase<AdmEmployeeCriteria>();
                        AdmEmployeeCriteria criteriaFouUserProfile = new AdmEmployeeCriteria();
                        criteriaFouUserProfile.empId = userCred.Username;
                        searchCriteriaForUserProfile.model = criteriaFouUserProfile;
                        EntitySearchResultBase<OrgTerritory> enOrgTerritory = await admEmployeeImp.getTerritoryForBackEnd(searchCriteriaForUserProfile);
                        userProfileForBackEndCustom.OrgTerritory = enOrgTerritory.data;*/
                        //
                        List<PermObjCodeCustom> listPermObjCode = await admEmployeeImp.searchPermObjCode(userCred.Username);
                        userProfileForBackEndCustom.listPermObjCode = listPermObjCode;
                    }
                    LoginModel model = new LoginModel();
                    model.token = token.ToString() ;
                    model.userProfileForBackEndCustom = userProfileForBackEndCustom;
                    res = ResponseResult.warp(model);
                }
                else
                {
                    // Insert ADM_LOG_LOGIN
                    AdmLogLoginModel admLogLoginModel = new AdmLogLoginModel();
                    admLogLoginModel.UserName = userCred.Username;
                    admLogLoginModel.UserAgent = agent;
                    admLogLoginModel.IpAddress = clientIPAddr;
                    admLogLoginModel.Status = authenResult.Status;
                    admLogLoginModel.ErrorDescription = authenResult.ErrorMessage;
                    AdmLogLogin admLogLogin = await admLogLoginImp.Add(admLogLoginModel);
                    ServiceException e = new ServiceException("E_INVALID_AUTHENTICATION", ObjectFacory.getCultureInfo(language));
                    res = ResponseResult.warpError(e);
                }
                /*if (token == null)
                    return Unauthorized();

                return Ok(token);*/

                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(7));
                option.HttpOnly = true;
                option.Secure = true;
                option.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;

                Response.Cookies.Append(CommonConstant.COOKIE_TOKEN_KEY, String.IsNullOrEmpty(token)? "" : token.ToString(), option);



                res.agent = agent;
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                resR.agent = agent;
                onBeforeSendResponse("login", "login", resR);
                return resR;
            }
        }


        [HttpPost("getUserProfile")]
        public async Task<ResponseResult> getUserProfile(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent)
        {
            try
            {
                onAfterReceiveRequest("getUserProfile", "getUserProfile", ""); 
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                List<PermObjCodeCustom> listPermObjCode = await admEmployeeImp.searchPermObjCode(userProfileForBack.getUserName());
                userProfileForBack.listPermObjCode = listPermObjCode;
                //
                ResponseResult res = ResponseResult.warp(userProfileForBack);
                res.agent = agent;
                onBeforeSendResponse("getUserProfile", "getUserProfile", new Object());
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getUserProfile", "getUserProfile", resR);
                return resR;
            }
        }


        private async Task<UserProfileForBackEndCustom> createUserProfileAsync(AdmLogLoginModel admLogLoginModel, AdmSessionModel admSessionModel, AdmEmployeeCriteria admEmployeeCriteria)
        {

                // Insert ADM_LOG_LOGIN
                //AdmLogLoginModel admLogLoginModel = new AdmLogLoginModel();
                AdmLogLogin admLogLogin = await admLogLoginImp.Add(admLogLoginModel);

                //Insert ADM_SESSION
                //AdmSessionModel admSessionModel = new AdmSessionModel();
                AdmSession admSession = await admSessionImp.Add(admSessionModel);

            //Set Object userProfile --> AdmEmployee,AdmGroup,OrgBusinessUnit		name:getUserProfileForBackEnd
            SearchCriteriaBase<AdmEmployeeCriteria> searchCriteria = new SearchCriteriaBase<AdmEmployeeCriteria>();
            searchCriteria.model = admEmployeeCriteria;
            EntitySearchResultBase<UserProfileForBackEndCustom> userProfileForBackEndCustom = await admEmployeeImp.getUserProfileForBackEnd(searchCriteria);
            UserProfileForBackEndCustom data = null;
            if (userProfileForBackEndCustom != null && userProfileForBackEndCustom.data != null && userProfileForBackEndCustom.data.Count != 0)
            {
                data = userProfileForBackEndCustom.data[0];
            }
            return data;


        }



        private async Task<AdmLogLoginModel> LoginFailAsync()
        {

            // Insert ADM_LOG_LOGIN
            AdmLogLoginModel admLogLoginModel = new AdmLogLoginModel();
            AdmLogLogin admLogLogin = await admLogLoginImp.Add(admLogLoginModel);

            return admLogLoginModel;

        }






        [HttpPost("logout")]
        public async Task<ResponseResult> logout(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent)
        {
            try
            {
                onAfterReceiveRequest("logout", "logout", new Object());
                //
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                //
                SearchCriteriaBase<AdmSessionCriteria> searchCriteria = new SearchCriteriaBase<AdmSessionCriteria>();
                AdmSessionCriteria admSessioncriteria = new AdmSessionCriteria();
                admSessioncriteria.TokenNo = _bearer_token;
                searchCriteria.model = admSessioncriteria;
                SessionValidate validSession = await validateSession(searchCriteria);
                if (validSession.Valid)
                {
                    AdmSessionModel admSessionModel = new AdmSessionModel();
                    admSessionModel.EmpId = userProfileForBack.getEmpId();
                    admSessionModel.SessionId = validSession.AdmSession.SessionId;
                    await admSessionImp.Logout(admSessionModel);

                    Response.Cookies.Delete(CommonConstant.COOKIE_TOKEN_KEY);
                }
                else
                {
                    throw new ServiceException("E_INVALID_DATA", ServiceException.setErrorParam(new String[] { "D_INVALID_SESSION" }), ObjectFacory.getCultureInfo(language));
                }
                SearchResultBase<Object> searchResult = new SearchResultBase<Object>();
                List<Object> list = new List<Object>();
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("logout", "logout", new Object());
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("logout", "logout", resR);
                return resR;
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <p>Required Paramerer</p>
        /// <p>Username</p>
        /// <p>Password</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [AllowAnonymous]
        [HttpPost("getSession")]
        public async Task<ResponseResult> getSession(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent)
        {
            try
            {
                
                String _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                onAfterReceiveRequest("getSession", "getSession", _bearer_token);
                //Set Object userProfile --> AdmEmployee,AdmGroup,OrgBusinessUnit		name:getUserProfileForBackEnd
                SearchCriteriaBase<GetSessioinCriteria> searchCriteria = new SearchCriteriaBase<GetSessioinCriteria>();
                GetSessioinCriteria getSessioinCriteria = new GetSessioinCriteria();
                getSessioinCriteria.Token = _bearer_token;
                searchCriteria.model = getSessioinCriteria;
                EntitySearchResultBase<UserProfileForBackEndCustom> userProfileForBackEndCustom = await admEmployeeImp.getUserProfileForBackEndByToken(searchCriteria);
                UserProfileForBackEndCustom data = null;
                if (userProfileForBackEndCustom != null && userProfileForBackEndCustom.data != null && userProfileForBackEndCustom.data.Count != 0)
                {
                    data = userProfileForBackEndCustom.data[0];
                }
                //return data;
                string clientIPAddr = HttpContext.Connection.RemoteIpAddress?.ToString();
                string VAL_TOKEN_NO = null;
                AuthenResult authenResult = null;
                string tokenExprie = null;
                if (data != null)
                {
                    
                    SearchCriteriaBase<MsConfigParamCriteria> searchCriteriaMsParam = new SearchCriteriaBase<MsConfigParamCriteria>();
                    MsConfigParamCriteria s = new MsConfigParamCriteria();
                    s.ParamKeyword = "TOKEN_EXPIRE";
                    searchCriteriaMsParam.model = s;
                    EntitySearchResultBase<MsConfigParam> msParam = await msConfigParamImp.Search(searchCriteriaMsParam);
                    tokenExprie = msParam.data[0].ParamValue;
                    authenResult = await jWTAuthenticationManager.Authenticate(data.EmpId, null, tokenExprie, true);
                    VAL_TOKEN_NO = authenResult.Token;
                }






                ResponseResult res;
                if (VAL_TOKEN_NO != null)
                {
                    //
                    string ipAddress = clientIPAddr;
                    AdmLogLoginModel admLogLoginModel = new AdmLogLoginModel();
                    admLogLoginModel.UserName = data.EmpId;
                    admLogLoginModel.UserAgent = agent;
                    admLogLoginModel.Status = authenResult.Status;
                    admLogLoginModel.IpAddress = ipAddress;
                    AdmSessionModel admSessionModel = new AdmSessionModel();
                    AdmEmployeeCriteria admEmployeeCriteria = new AdmEmployeeCriteria();
                    admEmployeeCriteria.empId = data.EmpId;
                    //string sessionId = HttpContext.Session.Id;
                    //admSessionModel.SessionId = Convert.ToDecimal(sessionId);
                    admSessionModel.TokenNo = VAL_TOKEN_NO;
                    admSessionModel.EmpId = data.EmpId;
                    admSessionModel.IpAddress = ipAddress;
                    admSessionModel.UserAgent = agent;
                    admSessionModel.TokenExpire = tokenExprie;


                    // Insert ADM_LOG_LOGIN
                    //AdmLogLoginModel admLogLoginModel = new AdmLogLoginModel();
                    AdmLogLogin admLogLogin = await admLogLoginImp.Add(admLogLoginModel);

                    //Insert ADM_SESSION
                    //AdmSessionModel admSessionModel = new AdmSessionModel();
                    AdmSession admSession = await admSessionImp.Add(admSessionModel);

                    //
                    LoginModel model = new LoginModel();
                    model.token = VAL_TOKEN_NO;
                    model.userProfileForBackEndCustom = data;
                    res = ResponseResult.warp(model);
                }
                else
                {
                    // Insert ADM_LOG_LOGIN
                    AdmLogLoginModel admLogLoginModel = new AdmLogLoginModel();
                    admLogLoginModel.UserName = null;
                    admLogLoginModel.UserAgent = agent;
                    admLogLoginModel.IpAddress = clientIPAddr;
                    admLogLoginModel.Status = "N";
                    admLogLoginModel.ErrorDescription = _bearer_token + " Not Found";
                    AdmLogLogin admLogLogin = await admLogLoginImp.Add(admLogLoginModel);
                    ServiceException e = new ServiceException("E_INVALID_AUTHENTICATION", ObjectFacory.getCultureInfo(language));
                    res = ResponseResult.warpError(e);


                }
                res.agent = agent;
                onBeforeSendResponse("getSession", "getSession", new Object());
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("getSession", "getSession", resR);
                return resR;
            }
        }







    }

}
