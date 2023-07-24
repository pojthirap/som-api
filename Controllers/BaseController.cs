using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFirstAzureWebApp.Authentication;
using MyFirstAzureWebApp.Business.org;
using MyFirstAzureWebApp.common;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.exception;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models.adm;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.sendGrid;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Utils;
using NLog;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Controllers
{
    [Authorize]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private Logger log = LogManager.GetCurrentClassLogger();
        private IAdmSession admSessionImp;
        private IAdmEmployee admEmployeeImp;
        protected IMsConfigParam msConfigParamImp;

        public BaseController()
        {
            admSessionImp = new AdmSessionImp();
            admEmployeeImp = new AdmEmployeeImp();
            msConfigParamImp = new MsConfigParamImp();
        }


        protected string getNameMaping()
        {
            return Guid.NewGuid().ToString();
        }

        protected void onAfterReceiveRequest<T>(String serviceName, String nameMaping, T t)
        {
            string jsonString = JsonSerializer.Serialize(t, typeof(T), new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
            string info = "\nSECEIVE REQUEST:\n" + "SERVICE NAME:" + serviceName + "\nNameMaping:" + nameMaping + "\nREQUEST DATA:\n" + jsonString + "\n";
            Console.WriteLine(info);
            log.Info(info);
        }

        protected void onBeforeSendResponse<T>(String serviceName, String nameMaping, T t)
        {
            string jsonString = JsonSerializer.Serialize(t, typeof(T), new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
            string info = "\nSEND RESPONSE:\n" + "SERVICE NAME:" + serviceName + "\nNameMaping:" + nameMaping + "\nRESPONSE DATA:\n" + jsonString + "\n";
            Console.WriteLine(info);
            log.Info(info);
        }

        protected bool isConstraintError(Exception ex)
        {
            if(ex.Message.Contains("UNIQUE KEY constraint"))
            {
                return true;
            }
            return false;
        }

        protected async Task<InterfaceSapConfig> getInterfaceSapConfigAsync()
        {
            InterfaceSapConfig interfaceSapConfig = new InterfaceSapConfig();
            SearchCriteriaBase<MsConfigParamCriteria> searchCriteriaMsParam = new SearchCriteriaBase<MsConfigParamCriteria>();
            MsConfigParamCriteria s = new MsConfigParamCriteria();
            s.ParamKeyword = "INTERFACE_SAP_URL";
            searchCriteriaMsParam.model = s;
            EntitySearchResultBase<MsConfigParam> msParam = await msConfigParamImp.Search(searchCriteriaMsParam);
            interfaceSapConfig.InterfaceSapUrl = msParam.data[0].ParamValue;

            s.ParamKeyword = "INTERFACE_SAP_USER";
            searchCriteriaMsParam.model = s;
            msParam = await msConfigParamImp.Search(searchCriteriaMsParam);
            interfaceSapConfig.InterfaceSapUser = msParam.data[0].ParamValue;

            interfaceSapConfig.InterfaceSapPwd = Environment.GetEnvironmentVariable("SAP_INTERFACE_PASSWORD");
            interfaceSapConfig.ReqKey = Environment.GetEnvironmentVariable("SAP_INTERFACE_REQ_KEY");


            return interfaceSapConfig;
        }
        protected async Task<String> getConfigAsync(string ParamKeyword)
        {
            SearchCriteriaBase<MsConfigParamCriteria> searchCriteriaMsParam = new SearchCriteriaBase<MsConfigParamCriteria>();
            MsConfigParamCriteria s = new MsConfigParamCriteria();
            s.ParamKeyword = ParamKeyword;
            searchCriteriaMsParam.model = s;
            EntitySearchResultBase<MsConfigParam> msParam = await msConfigParamImp.Search(searchCriteriaMsParam);
            string ParamKeyValue = msParam.data[0].ParamValue;

            return ParamKeyValue;
        }

        protected async Task<UserProfileForBack> getUserPrifileForBack(string token, string language)
        {

            token = String.IsNullOrEmpty(token) ? Request.Cookies[CommonConstant.COOKIE_TOKEN_KEY] : token;
            SearchCriteriaBase<AdmSessionCriteria> searchCriteria = new SearchCriteriaBase<AdmSessionCriteria>();
            AdmSessionCriteria admSessioncriteria = new AdmSessionCriteria();
            admSessioncriteria.TokenNo = token;
            searchCriteria.model = admSessioncriteria;
            SessionValidate validSession = await validateSession(searchCriteria);
            if (validSession.Valid)
            {
                // get exprie token

                SearchCriteriaBase<MsConfigParamCriteria> searchCriteriaMsParam = new SearchCriteriaBase<MsConfigParamCriteria>();
                MsConfigParamCriteria s = new MsConfigParamCriteria();
                s.ParamKeyword = "TOKEN_EXPIRE";
                searchCriteriaMsParam.model = s;
                EntitySearchResultBase<MsConfigParam> msParam = await msConfigParamImp.Search(searchCriteriaMsParam);
                string tokenExprie = msParam.data[0].ParamValue;

                // Update Adm Session
                AdmSessionModel admSessionModel = new AdmSessionModel();
                admSessionModel.EmpId = validSession.AdmSession.EmpId;
                admSessionModel.SessionId = validSession.AdmSession.SessionId;
                admSessionModel.TokenExpire = tokenExprie;
                await admSessionImp.Update(admSessionModel);


                // Get Employee Infomation
                UserProfileForBack UserProfileForBack = new UserProfileForBack();
                if ("Y".Equals(validSession.AdmSession.UserAccessFlag))
                {
                    SearchCriteriaBase<GetAdmEmployeeByEmpIdCriteria> searchCriteriaForUserProfile = new SearchCriteriaBase<GetAdmEmployeeByEmpIdCriteria>();
                    GetAdmEmployeeByEmpIdCriteria criteriaFouUserProfile = new GetAdmEmployeeByEmpIdCriteria();
                    criteriaFouUserProfile.EmpId = validSession.AdmSession.EmpId;
                    searchCriteriaForUserProfile.model = criteriaFouUserProfile;

                    //Set Object userProfile --> admEmployee
                    EntitySearchResultBase < AdmEmployee > dataResult = await admEmployeeImp.getEmployeeByEmpId(searchCriteriaForUserProfile);
                    UserProfileForBackEndCustom uCustom = new UserProfileForBackEndCustom();
                    AdmEmployee admEmployee = dataResult.data[0];
                    uCustom.EmpId = admEmployee.EmpId;
                    uCustom.CompanyCode = admEmployee.CompanyCode;
                    uCustom.JobTitle = admEmployee.JobTitle;
                    uCustom.TitleName = admEmployee.TitleName;
                    uCustom.FirstName = admEmployee.FirstName;
                    uCustom.LastName = admEmployee.LastName;
                    uCustom.Gender = admEmployee.Gender;
                    uCustom.Street = admEmployee.Street;
                    uCustom.TellNo = admEmployee.TellNo;
                    uCustom.CountryName = admEmployee.CountryName;
                    uCustom.ProvinceCode = admEmployee.ProvinceCode;
                    uCustom.GroupCode = admEmployee.GroupCode;
                    uCustom.DistrictName = admEmployee.DistrictName;
                    uCustom.SubdistrictName = admEmployee.SubdistrictName;
                    uCustom.PostCode = admEmployee.PostCode;
                    uCustom.Email = admEmployee.Email;
                    uCustom.Status = admEmployee.Status;
                    uCustom.ApproveEmpId = admEmployee.ApproveEmpId;
                    uCustom.ActiveFlag = admEmployee.ActiveFlag;
                    uCustom.CreateUser = admEmployee.CreateUser;
                    uCustom.CreateDtm = admEmployee.CreateDtm;
                    uCustom.UpdateUser = admEmployee.UpdateUser;
                    uCustom.UpdateDtm = admEmployee.UpdateDtm;
                    EntitySearchResultBase<UserProfileForBackEndCustom> resultUserProfile = new EntitySearchResultBase<UserProfileForBackEndCustom>();
                    List<UserProfileForBackEndCustom> lst = new List<UserProfileForBackEndCustom>();
                    lst.Add(uCustom);
                    resultUserProfile.data = lst;
                    UserProfileForBack.UserProfileCustom = resultUserProfile;

                }
                else
                {


                    SearchCriteriaBase<AdmEmployeeCriteria> searchCriteriaForUserProfile = new SearchCriteriaBase<AdmEmployeeCriteria>();
                    AdmEmployeeCriteria criteriaFouUserProfile = new AdmEmployeeCriteria();
                    criteriaFouUserProfile.empId = validSession.AdmSession.EmpId;
                    searchCriteriaForUserProfile.model = criteriaFouUserProfile;

                    //Set Object userProfile --> admEmployee,admGroup,orgBusinessUnit	
                    UserProfileForBack.UserProfileCustom = await admEmployeeImp.getUserProfileForBackEnd(searchCriteriaForUserProfile);

                    //Set Object userProfile --> orgSaleGroup,orgSaleOffice,orgTerritory	
                    criteriaFouUserProfile.groupCode = UserProfileForBack.UserProfileCustom.data == null || UserProfileForBack.UserProfileCustom.data.Count == 0 ? null : UserProfileForBack.UserProfileCustom.data[0].GroupCode;
                    if (!String.IsNullOrEmpty(criteriaFouUserProfile.groupCode))
                    {
                        UserProfileForBack.SaleGroupSaleOfficeCustom = await admEmployeeImp.getSaleGroupSaleOfficeForBackEnd(searchCriteriaForUserProfile);
                        if(UserProfileForBack.SaleGroupSaleOfficeCustom != null && UserProfileForBack.SaleGroupSaleOfficeCustom.data !=null && UserProfileForBack.SaleGroupSaleOfficeCustom.data.Count !=0)
                        {
                            SaleGroupSaleOfficeForBackEndCustom sgSoT = UserProfileForBack.SaleGroupSaleOfficeCustom.data[0];
                            OrgTerritory OrgTerritoryObject = new OrgTerritory();
                            OrgTerritoryObject.TerritoryId = sgSoT.TerritoryId;
                            OrgTerritoryObject.TerritoryCode = sgSoT.TerritoryCode;
                            OrgTerritoryObject.TerritoryNameTh = sgSoT.TerritoryNameTh;
                            OrgTerritoryObject.TerritoryNameEn = sgSoT.TerritoryNameEn;
                            OrgTerritoryObject.ManagerEmpId = sgSoT.TerritoryManagerEmpId;
                            OrgTerritoryObject.ActiveFlag = sgSoT.TerritoryActiveFlag;
                            OrgTerritoryObject.BuId = sgSoT.BuId;

                            UserProfileForBack.OrgTerritoryObject = OrgTerritoryObject;


                        }
                        
                    }

                    if (UserProfileForBack.SaleGroupSaleOfficeCustom != null && UserProfileForBack.SaleGroupSaleOfficeCustom.data[0].OfficeCode != null)
                    {
                        //Set Object userProfile --> listOrgSaleArea
                        SearchCriteriaBase<OrgSaleOffice> searchCriteriaSaleOffice = new SearchCriteriaBase<OrgSaleOffice>();
                        OrgSaleOffice saleOfficeCriteria = new OrgSaleOffice();
                        saleOfficeCriteria.OfficeCode = UserProfileForBack.SaleGroupSaleOfficeCustom.data[0].OfficeCode;
                        searchCriteriaSaleOffice.model = saleOfficeCriteria;
                        UserProfileForBack.OrgSaleArea = await admEmployeeImp.getSaleAreaForBackEnd(searchCriteriaSaleOffice);

                    }
                    //Set Object userProfile --> listOrgTerritory
                    //UserProfileForBack.OrgTerritory = await admEmployeeImp.getTerritoryForBackEnd(searchCriteriaForUserProfile);
                }

                // Check Role
                string _pathRequest = (Request.Method + Request.Path).Replace(@"/", @"\");
                log.Debug(" getUserPrifileForBack Check Role Path:" + _pathRequest);
                Console.WriteLine(" getUserPrifileForBack Check Role Path:" + _pathRequest);

                List<GetValidRoleCustom> validRole = await admEmployeeImp.searchCountPermObject(_pathRequest, UserProfileForBack);
                if(validRole==null || validRole.Count == 0 || validRole.ElementAt(0) == null || "0".Equals(validRole.ElementAt(0).CountPermObject))
                {
                    throw new ServiceException("E_NOT_PERMISSION", ObjectFacory.getCultureInfo(language));
                }
                //
                return UserProfileForBack;
            }
            else
            {
                throw new ServiceException("E_INVALID_DATA", ServiceException.setErrorParam(new String[] { "D_INVALID_SESSION" }), ObjectFacory.getCultureInfo(language));
            }

        }


        /*protected async Task<SessionValidate> validateSession(SearchCriteriaBase<AdmSessionCriteria> searchCriteria)
        {
            EntitySearchResultBase<AdmSession> admSession = await admSessionImp.Search(searchCriteria);
            SessionValidate s = new SessionValidate();
            s.Valid = false;
            if (admSession != null && admSession.data != null && admSession.data.Count != 0)
            {
                s.AdmSession = admSession.data[0];
                s.Valid = true;
                return s;
            }
            return s;
        }*/



        protected async Task<SessionValidate> validateSession(SearchCriteriaBase<AdmSessionCriteria> searchCriteria)
        {
            EntitySearchResultBase<SearchAdmSessionForGetSessionCustom> admSession = await admSessionImp.SearchAdmSessionForGetSession(searchCriteria);
            Console.WriteLine("validateSession token=" + admSession);
            log.Info("validateSession token=" + admSession);
            SessionValidate s = new SessionValidate();
            s.Valid = false;
            if (admSession != null && admSession.data != null && admSession.data.Count != 0)
            {
                Console.WriteLine("admSession != null");
                log.Info("admSession != null");
                s.AdmSession = admSession.data[0];
                s.Valid = true;
                return s;
            }
            return s;
        }



        private string getMailCalendarFormat(string dateTime)
        {
            if (String.IsNullOrEmpty(dateTime))
            {
                return dateTime;
            }


            Console.WriteLine("getMailCalendarFormat startDateTime=" + dateTime);
            Console.WriteLine("getMailCalendarFormat endDateTime=" + dateTime);
            log.Info("getMailCalendarFormat startDateTime=" + dateTime);
            log.Info("getMailCalendarFormat endDateTime=" + dateTime);

            if (dateTime.Contains("T"))
            {
                string tmp = dateTime.Split("T")[0];
                string[] tmpArr = tmp.Split("-");
                tmp = tmpArr[2] + "/" + tmpArr[1] + "/" + (Int32.Parse(tmpArr[0]) + 543);
                return tmp;
            }
            else
            {
                string tmp = dateTime.Split(" ")[0];
                string[] tmpArr = tmp.Split("/");
                tmp = tmpArr[1] + "/" + tmpArr[0] + "/" + (Int32.Parse(tmpArr[2]) + 543);
                return tmp;
            }

        }

        protected async Task<SendMailCustom> SendEmail(string language, SendMailModel sendMailModel, string mailSender, ISendGridClient sendGridClient, ILogger<SendGridController> logger)
        {

            var from = new EmailAddress(mailSender, sendMailModel.SenderName);
            List<EmailAddress> to = new List<EmailAddress>();
            foreach (string e in sendMailModel.MailTo)
            {
                to.Add(new EmailAddress(e));
            }
            var subject = sendMailModel.Subject;
            var plainTextContent = "";// "Sending calendar and easy to do anywhere, even with C#";
                                      //var htmlContent = "<strong>"+ sendMailModel.Content+ "</strong>";
            var htmlContent = sendMailModel.Content;
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, plainTextContent, htmlContent, sendMailModel.showAllRecipients);
            if (sendMailModel.Meeting != null)
            {
                string startDateTime = getMailCalendarFormat(sendMailModel.Meeting.StartDateTime);
                startDateTime += " 00:01";
                string endDateTime = getMailCalendarFormat(sendMailModel.Meeting.EndDateTime);
                endDateTime += " 23:59";
                logger.LogDebug("startDateTime="+ startDateTime);
                logger.LogDebug("endDateTime="+ endDateTime);
                Console.WriteLine("startDateTime=" + startDateTime);
                Console.WriteLine("endDateTime=" + endDateTime);
                log.Info("startDateTime=" + startDateTime);
                log.Info("endDateTime=" + endDateTime);
                //CultureInfo culture = new CultureInfo("en-US");
                CultureInfo culture = new CultureInfo("th-TH");
                DateTime DateTimeStart = Convert.ToDateTime(startDateTime, culture);
                DateTime DateTimeEnd = Convert.ToDateTime(endDateTime, culture);
                string CalendarContent = SendGridController.MeetingRequestString(sendMailModel.Meeting.Organizer, sendMailModel.MailTo, sendMailModel.Meeting.Subject, sendMailModel.Meeting.Description, sendMailModel.Meeting.Location, DateTimeStart, DateTimeEnd);
                /*string CalendarContent = MeetingRequestString("{ORGANIZER}", new List<string>()
            { "{ATTENDEE}" }, "{subject}", "{description}", "{location}", DateTime.Now, DateTime.Now.AddDays(2));*/
                byte[] calendarBytes = Encoding.UTF8.GetBytes(CalendarContent.ToString());
                SendGrid.Helpers.Mail.Attachment calendarAttachment = new SendGrid.Helpers.Mail.Attachment();
                calendarAttachment.Filename = "invite.ics";
                //the Base64 encoded content of the attachment.
                calendarAttachment.Content = Convert.ToBase64String(calendarBytes);
                calendarAttachment.Type = "text/calendar";
                msg.Attachments = new List<SendGrid.Helpers.Mail.Attachment>() { calendarAttachment };
            }
            if (sendMailModel.MailCC != null && sendMailModel.MailCC.Count != 0)
            {
                List<EmailAddress> cc = new List<EmailAddress>();
                foreach (string e in sendMailModel.MailCC)
                {
                    cc.Add(new EmailAddress(e));
                }
                msg.AddCcs(cc);
            }
            if (sendMailModel.MailBCC != null && sendMailModel.MailBCC.Count != 0)
            {
                List<EmailAddress> bcc = new List<EmailAddress>();
                foreach (string e in sendMailModel.MailBCC)
                {
                    bcc.Add(new EmailAddress(e));
                }
                msg.AddBccs(bcc);
            }
            // Test Send Mail Fail
            if ("1".Equals("11"))
            {
                throw new InvalidOperationException("Send Email Fail Exception");
            }

            var response = await sendGridClient.SendEmailAsync(msg);
            string logStr = ("response code:" + response.StatusCode
                + ",IsSuccessStatusCode:" + response.IsSuccessStatusCode
                + ",Headers:" + response.Headers
                + ",Body:" + response.Body
                );
            logger.LogDebug(logStr);
            logger.LogDebug("End Test SendGrid");
            Console.WriteLine(logStr);
            Console.WriteLine("End Test SendGrid");
            log.Info(logStr);
            log.Info("End Test SendGrid");

            SendMailCustom model = new SendMailCustom();
            model.StatusCode = response.StatusCode;
            model.IsSuccessStatusCode = response.IsSuccessStatusCode;
            //return Ok("StatusCode:"+response.StatusCode + ", IsSuccessStatusCode:" + response.IsSuccessStatusCode);
            /*SearchResultBase<SendMailCustom> searchResult = new SearchResultBase<SendMailCustom>();
            List<SendMailCustom> list = new List<SendMailCustom>();
            list.Add(model);
            searchResult.totalRecords = list.Count;
            searchResult.records = list;
            ResponseResult res = ResponseResult.warp(searchResult);
            res.agent = agent;*/
            onBeforeSendResponse("SendMail", "SendMail", model);
            return model;


        }



        public class SessionValidate
        {
            public Boolean Valid { set; get; }
            public SearchAdmSessionForGetSessionCustom AdmSession { set; get; }
        }



    }

    public class InterfaceSapConfig
    {
        public string InterfaceSapUrl { get; set; }
        public string InterfaceSapUser { get; set; }
        public string InterfaceSapPwd { get; set; }
        public string ReqKey { get; set; }
    }

}
