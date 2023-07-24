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
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.adm;
using System.Data;
using MyFirstAzureWebApp.Models.profile;

namespace MyFirstAzureWebApp.Business.org
{

    public class AdmEmployeeImp : IAdmEmployee

    {
        private Logger log = LogManager.GetCurrentClassLogger();





        public async Task<EntitySearchResultBase<SearchAdmEmpCustom>> searchAdmEmp(AdmEmployeeSearchCriteria searchCriteria)
        {

            EntitySearchResultBase<SearchAdmEmpCustom> searchResult = new EntitySearchResultBase<SearchAdmEmpCustom>();
            List<SearchAdmEmpCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                AdmEmployeeCriteria o = searchCriteria.model;

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select E.*,EA.EMP_ID APPROVER_ID,EA.TITLE_NAME+ EA.FIRST_NAME+ ' ' + EA.LAST_NAME APPROVER_NAME,GU.BU_ID ");
                queryBuilder.AppendFormat(" from ADM_EMPLOYEE E ");
                queryBuilder.AppendFormat(" inner join ADM_GROUP_USER GU on GU.EMP_ID = E.EMP_ID ");
                queryBuilder.AppendFormat(" left join ADM_EMPLOYEE EA on EA.EMP_ID = E.APPROVE_EMP_ID where 1 = 1 ");
                if (o != null)
                {

                    if (!String.IsNullOrEmpty(o.groupCode))
                    {
                        queryBuilder.AppendFormat(" and E.GROUP_CODE  = @groupCode  ");// Edit New
                        QueryUtils.addParam(command, "groupCode", o.groupCode);// Add New
                    }
                    if (!String.IsNullOrEmpty(o.activeFlag))
                    {
                        queryBuilder.AppendFormat(" and E.ACTIVE_FLAG  = @ActiveFlag  ");// Edit New
                        QueryUtils.addParam(command, "ActiveFlag", o.activeFlag);// Add New
                    }
                    if (!String.IsNullOrEmpty(o.empId))
                    {
                        queryBuilder.AppendFormat(" and E.EMP_ID  = @empId  ");// Edit New
                        QueryUtils.addParam(command, "empId", o.empId);// Add New
                    }
                    if (!String.IsNullOrEmpty(o.name))
                    {
                        o.name = o.name.Replace(" ", "");
                        queryBuilder.AppendFormat(" and (E.FIRST_NAME+E.LAST_NAME like  @FullName ) ");
                        QueryUtils.addParamLike(command, "FullName", o.name);// Add New
                    }
                }
                if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY E.UPDATE_DTM DESC  ");
                }
                else
                {
                    // For Paging
                    queryBuilder.AppendFormat(" ORDER BY EMP_ID  ");
                }


                // For Paging
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

                    List<SearchAdmEmpCustom> dataRecordList = new List<SearchAdmEmpCustom>();
                    SearchAdmEmpCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchAdmEmpCustom();


                        dataRecord.EmpId = QueryUtils.getValueAsString(record, "EMP_ID");
                        dataRecord.CompanyCode = QueryUtils.getValueAsString(record, "COMPANY_CODE");
                        dataRecord.JobTitle = QueryUtils.getValueAsString(record, "JOB_TITLE");
                        dataRecord.TitleName = QueryUtils.getValueAsString(record, "TITLE_NAME");
                        dataRecord.FirstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                        dataRecord.LastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                        dataRecord.Gender = QueryUtils.getValueAsString(record, "GENDER");
                        dataRecord.Street = QueryUtils.getValueAsString(record, "STREET");
                        dataRecord.TellNo = QueryUtils.getValueAsString(record, "TELL_NO");
                        dataRecord.CountryName = QueryUtils.getValueAsString(record, "COUNTRY_NAME");
                        dataRecord.ProvinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                        dataRecord.GroupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        dataRecord.DistrictName = QueryUtils.getValueAsString(record, "DISTRICT_NAME");
                        dataRecord.SubdistrictName = QueryUtils.getValueAsString(record, "SUBDISTRICT_NAME");
                        dataRecord.PostCode = QueryUtils.getValueAsString(record, "POST_CODE");
                        dataRecord.Email = QueryUtils.getValueAsString(record, "EMAIL");
                        dataRecord.Status = QueryUtils.getValueAsString(record, "STATUS");
                        dataRecord.ApproveEmpId = QueryUtils.getValueAsString(record, "APPROVE_EMP_ID");
                        dataRecord.ApproverId = QueryUtils.getValueAsString(record, "APPROVER_ID");
                        dataRecord.ApproveName = QueryUtils.getValueAsString(record, "APPROVER_NAME");
                        dataRecord.BuId = QueryUtils.getValueAsString(record, "BU_ID"); 

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



        public async Task<EntitySearchResultBase<SearchAdmEmpCustom>> searchAdmEmpForReport(SearchCriteriaBase<SearchAdmEmpForReportCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchAdmEmpCustom> searchResult = new EntitySearchResultBase<SearchAdmEmpCustom>();
            List<SearchAdmEmpCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                SearchAdmEmpForReportCriteria o = searchCriteria.model;

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select * ");
                queryBuilder.AppendFormat(" from ADM_EMPLOYEE E ");
                //queryBuilder.AppendFormat(" where exists (select 1 from ORG_SALE_TERRITORY T where T.EMP_ID = E.EMP_ID) ");
                queryBuilder.AppendFormat(" where exists (select 1 from ORG_SALE_GROUP G where G.GROUP_CODE = E.GROUP_CODE and G.TERRITORY_ID IS NOT NULL) ");


                if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY E.UPDATE_DTM DESC  ");
                }
                else
                {
                    // For Paging
                    queryBuilder.AppendFormat(" ORDER BY FIRST_NAME, LAST_NAME  ");
                }


                // For Paging
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

                    List<SearchAdmEmpCustom> dataRecordList = new List<SearchAdmEmpCustom>();
                    SearchAdmEmpCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchAdmEmpCustom();


                        dataRecord.EmpId = QueryUtils.getValueAsString(record, "EMP_ID");
                        dataRecord.CompanyCode = QueryUtils.getValueAsString(record, "COMPANY_CODE");
                        dataRecord.JobTitle = QueryUtils.getValueAsString(record, "JOB_TITLE");
                        dataRecord.TitleName = QueryUtils.getValueAsString(record, "TITLE_NAME");
                        dataRecord.FirstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                        dataRecord.LastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                        dataRecord.Gender = QueryUtils.getValueAsString(record, "GENDER");
                        dataRecord.Street = QueryUtils.getValueAsString(record, "STREET");
                        dataRecord.TellNo = QueryUtils.getValueAsString(record, "TELL_NO");
                        dataRecord.CountryName = QueryUtils.getValueAsString(record, "COUNTRY_NAME");
                        dataRecord.ProvinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                        dataRecord.GroupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        dataRecord.DistrictName = QueryUtils.getValueAsString(record, "DISTRICT_NAME");
                        dataRecord.SubdistrictName = QueryUtils.getValueAsString(record, "SUBDISTRICT_NAME");
                        dataRecord.PostCode = QueryUtils.getValueAsString(record, "POST_CODE");
                        dataRecord.Email = QueryUtils.getValueAsString(record, "EMAIL");
                        dataRecord.Status = QueryUtils.getValueAsString(record, "STATUS");
                        dataRecord.ApproveEmpId = QueryUtils.getValueAsString(record, "APPROVE_EMP_ID");
                        //dataRecord.ApproverId = QueryUtils.getValueAsString(record, "APPROVER_ID");
                        //dataRecord.ApproveName = QueryUtils.getValueAsString(record, "APPROVER_NAME");

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




        public async Task<EntitySearchResultBase<AdmEmployee>> getEmpSupvisor(SearchCriteriaBase<AdmEmployeeCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                
                AdmEmployeeCriteria criteria = searchCriteria.model;
                decimal buIdDecimal = String.IsNullOrEmpty(criteria.buId) ? 0 : Convert.ToDecimal(criteria.buId);
                var queryCommane = (from gu in context.AdmGroupUser
                                    join g in context.AdmGroup
                                    on gu.GroupId equals g.GroupId
                                    join e in context.AdmEmployee
                                    on gu.EmpId equals e.EmpId 

                                    where ( g.GroupCode == "SUPEPVISOR")
                                    where ((criteria.groupCode == null ? 1 == 1 : e.GroupCode == criteria.groupCode))
                                    where ((criteria.buId == null ? 1 == 1 : gu.BuId == buIdDecimal))
                                    //orderby (searchCriteria.searchOrder == 1 ? e.FirstName : e.GroupCode)
                                    select e
                                    );

                if (searchCriteria.searchOrder == 1)
                {
                    queryCommane = queryCommane.OrderBy(o => o.FirstName);
                }
                else
                {
                    queryCommane = queryCommane.OrderByDescending(o => o.UpdateDtm); 
                }
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<AdmEmployee> searchResult = new EntitySearchResultBase<AdmEmployee>();
                searchResult.totalRecords = query.Count();




                List<AdmEmployee> saleLst = new List<AdmEmployee>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    AdmEmployee s = new AdmEmployee();
                    s.EmpId = item.EmpId;
                    s.CompanyCode = item.CompanyCode;
                    s.JobTitle = item.JobTitle;
                    s.TitleName = item.TitleName;
                    s.FirstName = item.FirstName;
                    s.LastName = item.LastName;
                    s.Gender = item.Gender;
                    s.Street = item.Street;
                    s.TellNo = item.TellNo;
                    s.CountryName = item.CountryName;
                    s.ProvinceCode = item.ProvinceCode;
                    s.GroupCode = item.GroupCode;
                    s.DistrictName = item.DistrictName;
                    s.SubdistrictName = item.SubdistrictName;
                    s.PostCode = item.PostCode;
                    s.Email = item.Email;
                    s.Status = item.Status;
                    s.ApproveEmpId = item.ApproveEmpId;
                    s.ActiveFlag = item.ActiveFlag;
                    s.CreateUser = item.CreateUser;
                    s.CreateDtm = item.CreateDtm;
                    s.UpdateUser = item.UpdateUser;
                    s.UpdateDtm = item.UpdateDtm;
                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;

            }

        }


        public async Task<EntitySearchResultBase<AdmEmployee>> SearchSaleRep(AdmEmployeeSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from ADM_EMPLOYEE where 1=1 and GROUP_CODE is null ");
                AdmEmployeeCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.empId))
                    {
                        queryBuilder.AppendFormat("  and EMP_ID = @empId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("empId", o.empId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.name))
                    {
                        queryBuilder.AppendFormat(" and (FIRST_NAME like @name OR LAST_NAME like @name )   ");
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("name", o.name));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.activeFlag))
                    {
                        queryBuilder.AppendFormat(" and ACTIVE_FLAG  = @ActiveFlag  ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", o.activeFlag));// Add New
                    }
                }

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY GROUP_CODE  ");
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.AdmEmployee.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<AdmEmployee> lst = context.AdmEmployee.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<AdmEmployee> searchResult = new EntitySearchResultBase<AdmEmployee>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }



        public async Task<int> UpdateSaleGroupToSaleRep(AdmEmployeeModel admEmployeeModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        String empIdStr = "";
                        int length_ = admEmployeeModel.empIdList.Length;
                        String[] empIdList = new String[length_];
                        for (int i = 0; i < length_; i++)
                        {
                            empIdList[i] = admEmployeeModel.empIdList[i];
                        }
                        empIdStr = String.Join(",", empIdList);

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ADM_EMPLOYEE SET GROUP_CODE = @GROUP_CODE, APPROVE_EMP_ID=@APPROVE_EMP_ID, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE EMP_ID IN (" + QueryUtils.getParamIn("empIdStr", empIdStr) + ")  ");
                        QueryUtils.addParamIn(sqlParameters, "empIdStr", empIdStr);
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_CODE", admEmployeeModel.groupCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("APPROVE_EMP_ID", admEmployeeModel.approveEmpId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", admEmployeeModel.getUserName()));// Add New
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


        public async Task<int> DeleteSaleGroupWithOutSaleRep(AdmEmployeeModel admEmployeeModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ADM_EMPLOYEE SET GROUP_CODE = null, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE EMP_ID=@EMP_ID  ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", admEmployeeModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("EMP_ID", admEmployeeModel.empId));// Add New
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





        public async Task<EntitySearchResultBase<AdmEmployeeAdmGroupCustom>> searchAdmEmpForMapRole(AdmEmployeeSearchCriteria searchCriteria)
        {


            EntitySearchResultBase <AdmEmployeeAdmGroupCustom> searchResult = new EntitySearchResultBase<AdmEmployeeAdmGroupCustom>();
            List<AdmEmployeeAdmGroupCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();


                AdmEmployeeCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("  select E.UPDATE_DTM as E_UPDATE_DTM, E.ACTIVE_FLAG as E_ACTIVE_FLAG, E.*,G.*,GU.* ,BU.BU_NAME_TH, BU.BU_NAME_EN, SG.DESCRIPTION_TH, OT.TERRITORY_NAME_TH ");
                queryBuilder.AppendFormat("  from ADM_EMPLOYEE E   ");
                queryBuilder.AppendFormat("  left join ADM_GROUP_USER GU on GU.EMP_ID = E.EMP_ID  ");
                queryBuilder.AppendFormat("  left join ADM_GROUP G on G.GROUP_ID = GU.GROUP_ID ");
                queryBuilder.AppendFormat("  left join ORG_BUSINESS_UNIT BU on BU.BU_ID = GU.BU_ID ");
                queryBuilder.AppendFormat("  left join ORG_SALE_GROUP SG on SG.GROUP_CODE = E.GROUP_CODE ");
                //queryBuilder.AppendFormat("  left join ORG_SALE_TERRITORY ST on ST.EMP_ID = E.EMP_ID ");
                queryBuilder.AppendFormat("  left join ORG_TERRITORY OT on OT.TERRITORY_ID = SG.TERRITORY_ID ");


                queryBuilder.AppendFormat(" where 1=1 ");

                if (o != null)
                {

                    if (!String.IsNullOrEmpty(o.empId))
                    {
                        queryBuilder.AppendFormat(" and E.EMP_ID  = @empId  ");
                        QueryUtils.addParam(command, "empId", o.empId);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.name))
                    {   o.name = o.name.Replace(" ", "");
                        queryBuilder.AppendFormat("  and (E.FIRST_NAME+E.LAST_NAME like @FullName )  ");
                        QueryUtils.addParamLike(command, "FullName", o.name);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.jobTitle))
                    {
                        queryBuilder.AppendFormat(" and E.JOB_TITLE  = @jobTitle  ");
                        QueryUtils.addParam(command, "jobTitle", o.jobTitle);// Add new
                    }
                }

                // For Paging
                if (searchCriteria.searchOrder == 1)
                {
                    queryBuilder.AppendFormat(" ORDER BY E.FIRST_NAME, E.LAST_NAME "); 
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY E.UPDATE_DTM  ");
                }
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

                    List<AdmEmployeeAdmGroupCustom> dataRecordList = new List<AdmEmployeeAdmGroupCustom>();
                    AdmEmployeeAdmGroupCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new AdmEmployeeAdmGroupCustom();


                        dataRecord.empId = QueryUtils.getValueAsString(record, "EMP_ID");
                        dataRecord.companyCode = QueryUtils.getValueAsString(record, "COMPANY_CODE");
                        dataRecord.JobTitle = QueryUtils.getValueAsString(record, "JOB_TITLE");
                        dataRecord.titleName = QueryUtils.getValueAsString(record, "TITLE_NAME");
                        dataRecord.firstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                        dataRecord.lastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                        dataRecord.gender = QueryUtils.getValueAsString(record, "GENDER");
                        dataRecord.street = QueryUtils.getValueAsString(record, "STREET");
                        dataRecord.tellNo = QueryUtils.getValueAsString(record, "TELL_NO");
                        dataRecord.countryName = QueryUtils.getValueAsString(record, "COUNTRY_NAME");
                        dataRecord.provinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                        dataRecord.groupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        dataRecord.districtName = QueryUtils.getValueAsString(record, "DISTRICT_NAME");
                        dataRecord.subdistrictName = QueryUtils.getValueAsString(record, "SUBDISTRICT_NAME");
                        dataRecord.postCode = QueryUtils.getValueAsString(record, "POST_CODE");
                        dataRecord.email = QueryUtils.getValueAsString(record, "EMAIL");
                        dataRecord.status = QueryUtils.getValueAsString(record, "STATUS");
                        dataRecord.approveEmpId = QueryUtils.getValueAsString(record, "APPROVE_EMP_ID");
                        dataRecord.activeFlag = QueryUtils.getValueAsString(record, "E_ACTIVE_FLAG");
                        dataRecord.createUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.createDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        dataRecord.updateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.updateDtm = QueryUtils.getValueAsDateTime(record, "E_UPDATE_DTM");



                        dataRecord.groupId = QueryUtils.getValueAsDecimal(record, "GROUP_ID");
                        dataRecord.groupNameTh = QueryUtils.getValueAsString(record, "GROUP_NAME_TH");
                        dataRecord.groupNameEn = QueryUtils.getValueAsString(record, "GROUP_NAME_EN");
                        dataRecord.effectiveDate = QueryUtils.getValueAsDateTime(record, "EFFECTIVE_DATE");
                        dataRecord.expiryDate = QueryUtils.getValueAsDateTime(record, "EXPIRY_DATE");


                        dataRecord.groupUserId = QueryUtils.getValueAsDecimal(record, "GROUP_USER_ID");
                        dataRecord.groupUserType = QueryUtils.getValueAsString(record, "GROUP_USER_TYPE");
                        dataRecord.buId = QueryUtils.getValueAsDecimal(record, "BU_ID");
                        dataRecord.buNameTh = QueryUtils.getValueAsString(record, "BU_NAME_TH");
                        dataRecord.buNameEn = QueryUtils.getValueAsString(record, "BU_NAME_EN");
                        dataRecord.descriptionTh = QueryUtils.getValueAsString(record, "DESCRIPTION_TH");
                        dataRecord.territoryNameTh = QueryUtils.getValueAsString(record, "TERRITORY_NAME_TH");

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




        



        public async Task<int> UpdateApproverToSaleRep(AdmEmployeeModel admEmployeeModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        String empIdStr = "";
                        int length_ = admEmployeeModel.empIdList.Length;
                        String[] empIdList = new String[length_];
                        for (int i = 0; i < length_; i++)
                        {
                            empIdList[i] = admEmployeeModel.empIdList[i];
                        }
                        empIdStr = String.Join(",", empIdList);

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ADM_EMPLOYEE SET APPROVE_EMP_ID = @APPROVE_EMP_ID, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE EMP_ID IN (" + QueryUtils.getParamIn("empIdStr", empIdStr) + ")  ");
                        QueryUtils.addParamIn(sqlParameters, "empIdStr", empIdStr);
                        sqlParameters.Add(QueryUtils.addSqlParameter("APPROVE_EMP_ID", admEmployeeModel.empId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", admEmployeeModel.getUserName()));// Add New
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




        public async Task<EntitySearchResultBase<AdmEmployee>> searchAdmEmpRoleManager(AdmEmployeeSearchCriteria searchCriteria, UserProfileForBack userProfileForBack)
        {

            EntitySearchResultBase<AdmEmployee> searchResult = new EntitySearchResultBase<AdmEmployee>();
            List<AdmEmployee> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                AdmEmployeeCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select E.*");
                queryBuilder.AppendFormat(" from dbo.ADM_GROUP_USER GU ");
                queryBuilder.AppendFormat(" inner join dbo.ADM_GROUP G on G.GROUP_ID = GU.GROUP_ID ");
                queryBuilder.AppendFormat(" inner join dbo.ADM_EMPLOYEE E on E.EMP_ID = GU.EMP_ID ");
                //queryBuilder.AppendFormat(" inner join dbo.ORG_SALE_TERRITORY ST on ST.EMP_ID = E.EMP_ID and ST.ACTIVE_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" inner join dbo.ORG_SALE_GROUP SG on SG.GROUP_CODE = E.GROUP_CODE and SG.TERRITORY_ID is not null ");
                queryBuilder.AppendFormat(" where G.GROUP_CODE = 'MANAGER' ");

                queryBuilder.AppendFormat(" and GU.BU_ID = @BU_ID ");
                QueryUtils.addParam(command, "BU_ID", userProfileForBack.getBuId());// Add new


                // For Paging

                if (searchCriteria.searchOrder == 1)
                {
                    queryBuilder.AppendFormat(" ORDER BY FIRST_NAME  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY UPDATE_DTM  ");
                }
                
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

                    List<AdmEmployee> dataRecordList = new List<AdmEmployee>();
                    AdmEmployee dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new AdmEmployee();


                        dataRecord.EmpId = QueryUtils.getValueAsString(record, "EMP_ID");
                        dataRecord.CompanyCode = QueryUtils.getValueAsString(record, "COMPANY_CODE");
                        dataRecord.JobTitle = QueryUtils.getValueAsString(record, "JOB_TITLE");
                        dataRecord.TitleName = QueryUtils.getValueAsString(record, "TITLE_NAME");
                        dataRecord.FirstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                        dataRecord.LastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                        dataRecord.Gender = QueryUtils.getValueAsString(record, "GENDER");
                        dataRecord.Street = QueryUtils.getValueAsString(record, "STREET");
                        dataRecord.TellNo = QueryUtils.getValueAsString(record, "TELL_NO");
                        dataRecord.CountryName = QueryUtils.getValueAsString(record, "COUNTRY_NAME");
                        dataRecord.ProvinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                        dataRecord.GroupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        dataRecord.DistrictName = QueryUtils.getValueAsString(record, "DISTRICT_NAME");
                        dataRecord.SubdistrictName = QueryUtils.getValueAsString(record, "SUBDISTRICT_NAME");
                        dataRecord.PostCode = QueryUtils.getValueAsString(record, "POST_CODE");
                        dataRecord.Email = QueryUtils.getValueAsString(record, "EMAIL");
                        dataRecord.Status = QueryUtils.getValueAsString(record, "STATUS");
                        dataRecord.ApproveEmpId = QueryUtils.getValueAsString(record, "APPROVE_EMP_ID");
                        dataRecord.ProvinceName = QueryUtils.getValueAsString(record, "PROVINCE_NAME");
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




        /*
                public async Task<EntitySearchResultBase<AdmEmployee>> searchAdmEmpRoleManager(AdmEmployeeSearchCriteria searchCriteria, UserProfileForBack userProfileForBack)
                {
                    String territoryIdStr = null;
                    if (userProfileForBack.OrgTerritory.data != null && userProfileForBack.OrgTerritory.data.Count != 0)
                    {
                        int length_ = userProfileForBack.OrgTerritory.data.Count;
                        String[] territoryIdList = new String[length_];
                        for (int i = 0; i < length_; i++)
                        {
                            territoryIdList[i] = userProfileForBack.OrgTerritory.data.ElementAt(i).TerritoryId.ToString();
                        }
                        territoryIdStr = String.Join(",", territoryIdList);
                    }

                    using (var context = new MyAppContext())
                    {
                        AdmEmployeeCriteria criteria = searchCriteria.model;
                        var queryCommane = (from gu in context.AdmGroupUser
                                            join g in context.AdmGroup
                                            on gu.GroupId equals g.GroupId
                                            join e in context.AdmEmployee
                                            on gu.EmpId equals e.EmpId
                                            join st in context.OrgSaleTerritory
                                            on e.EmpId equals st.EmpId 

                                            where (g.GroupCode == "MANAGER")
                                            //where ((criteria.groupCode == null ? 1 == 1 : e.GroupCode == criteria.groupCode))
                                            //where ((territoryIdStr == null ? 1 == 1 : territoryIdStr.Contains(st.TerritoryId.ToString())))
                                            where ((userProfileForBack.getBuId() == null ? 1 == 1 : gu.BuId == userProfileForBack.getBuId()))
                                            //orderby (searchCriteria.searchOrder == 0 ? e.EmpId : e.FirstName)
                                            select e
                                           );

                        if (searchCriteria.searchOrder == 1)
                        {
                            queryCommane = queryCommane.OrderBy(o => o.FirstName);
                        }
                        else
                        {
                            queryCommane = queryCommane.OrderByDescending(o => o.UpdateDtm);
                        }
                        var queryString = queryCommane.ToQueryString();
                        log.Debug("Query:" + queryString);
                        Console.WriteLine("Query:" + queryString);
                        var query = queryCommane.ToList();
                        EntitySearchResultBase<AdmEmployee> searchResult = new EntitySearchResultBase<AdmEmployee>();
                        searchResult.totalRecords = query.Count();




                        List<AdmEmployee> saleLst = new List<AdmEmployee>();
                        foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                        {

                            AdmEmployee s = new AdmEmployee();
                            s.EmpId = item.EmpId;
                            s.CompanyCode = item.CompanyCode;
                            s.JobTitle = item.JobTitle;
                            s.TitleName = item.TitleName;
                            s.FirstName = item.FirstName;
                            s.LastName = item.LastName;
                            s.Gender = item.Gender;
                            s.Street = item.Street;
                            s.TellNo = item.TellNo;
                            s.CountryName = item.CountryName;
                            s.ProvinceCode = item.ProvinceCode;
                            s.GroupCode = item.GroupCode;
                            s.DistrictName = item.DistrictName;
                            s.SubdistrictName = item.SubdistrictName;
                            s.PostCode = item.PostCode;
                            s.Email = item.Email;
                            s.Status = item.Status;
                            s.ApproveEmpId = item.ApproveEmpId;
                            s.ActiveFlag = item.ActiveFlag;
                            s.CreateUser = item.CreateUser;
                            s.CreateDtm = item.CreateDtm;
                            s.UpdateUser = item.UpdateUser;
                            s.UpdateDtm = item.UpdateDtm;
                            saleLst.Add(s);

                        }

                        searchResult.data = saleLst;
                        return searchResult;

                    }

                }
                */








        // For Backend

        public async Task<EntitySearchResultBase<UserProfileForBackEndCustom>> getUserProfileForBackEnd(SearchCriteriaBase<AdmEmployeeCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                AdmEmployeeCriteria criteria = searchCriteria.model;
                var queryCommane = (from e in context.AdmEmployee
                                    join gu in context.AdmGroupUser
                                    on e.EmpId equals gu.EmpId
                                    join g in context.AdmGroup
                                    on gu.GroupId equals g.GroupId
                                    join bu in context.OrgBusinessUnit
                                    on gu.BuId equals bu.BuId
                                    where g.ActiveFlag == "Y"

                                    where ((criteria.empId == null ? 1 == 1 : e.EmpId == criteria.empId))
                                    orderby (searchCriteria.searchOrder == 0 ? e.EmpId : e.FirstName)
                                    select new
                                    {

                                        EmpId = e.EmpId,
                                        CompanyCode = e.CompanyCode,
                                        JobTitle = e.JobTitle,
                                        TitleName = e.TitleName,
                                        FirstName = e.FirstName,
                                        LastName = e.LastName,
                                        Gender = e.Gender,
                                        Street = e.Street,
                                        TellNo = e.TellNo,
                                        CountryName = e.CountryName,
                                        ProvinceCode = e.ProvinceCode,
                                        GroupCode = e.GroupCode,
                                        DistrictName = e.DistrictName,
                                        SubdistrictName = e.SubdistrictName,
                                        PostCode = e.PostCode,
                                        Email = e.Email,
                                        Status = e.Status,
                                        ApproveEmpId = e.ApproveEmpId,
                                        ActiveFlag = e.ActiveFlag,
                                        GroupUserId = gu.GroupUserId,
                                        AdmGroup_GroupCode = g.GroupCode,
                                        GroupId = g.GroupId,
                                        GroupNameTh = g.GroupNameTh,
                                        GroupNameEn = g.GroupNameEn,
                                        EffectiveDate = g.EffectiveDate,
                                        ExpiryDate = g.ExpiryDate,
                                        CreateUser = e.CreateUser,
                                        CreateDtm = e.CreateDtm,
                                        UpdateUser = e.UpdateUser,
                                        UpdateDtm = e.UpdateDtm,
                                        BuId = gu.BuId
                                    }
                                    );
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<UserProfileForBackEndCustom> searchResult = new EntitySearchResultBase<UserProfileForBackEndCustom>();
                searchResult.totalRecords = query.Count();

                List<UserProfileForBackEndCustom> saleLst = new List<UserProfileForBackEndCustom>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    UserProfileForBackEndCustom s = new UserProfileForBackEndCustom();
                    s.EmpId = item.EmpId;
                    s.CompanyCode = item.CompanyCode;
                    s.JobTitle = item.JobTitle;
                    s.TitleName = item.TitleName;
                    s.FirstName = item.FirstName;
                    s.LastName = item.LastName;
                    s.Gender = item.Gender;
                    s.Street = item.Street;
                    s.TellNo = item.TellNo;
                    s.CountryName = item.CountryName;
                    s.ProvinceCode = item.ProvinceCode;
                    s.GroupCode = item.GroupCode;
                    s.DistrictName = item.DistrictName;
                    s.SubdistrictName = item.SubdistrictName;
                    s.PostCode = item.PostCode;
                    s.Email = item.Email;
                    s.Status = item.Status;
                    s.ApproveEmpId = item.ApproveEmpId;
                    s.ActiveFlag = item.ActiveFlag;
                    s.CreateUser = item.CreateUser;
                    s.CreateDtm = item.CreateDtm;
                    s.UpdateUser = item.UpdateUser;
                    s.UpdateDtm = item.UpdateDtm;

                    s.GroupUserId = item.GroupUserId;
                    s.GroupId = item.GroupId;
                    s.AdmGroup_GroupCode = item.AdmGroup_GroupCode;
                    s.GroupNameTh = item.GroupNameTh;
                    s.GroupNameEn = item.GroupNameEn;
                    s.EffectiveDate = item.EffectiveDate;
                    s.ExpiryDate = item.ExpiryDate;

                    s.BuId = item.BuId;

                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;

            }

        }



        public async Task<EntitySearchResultBase<SaleGroupSaleOfficeForBackEndCustom>> getSaleGroupSaleOfficeForBackEnd(SearchCriteriaBase<AdmEmployeeCriteria> searchCriteria)
        {

            EntitySearchResultBase<SaleGroupSaleOfficeForBackEndCustom> searchResult = new EntitySearchResultBase<SaleGroupSaleOfficeForBackEndCustom>();
            List<SaleGroupSaleOfficeForBackEndCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                AdmEmployeeCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select SG.*,SO.*,T.* ");
                queryBuilder.AppendFormat(" ,SG.GROUP_CODE as SG_GROUP_CODE ");
                queryBuilder.AppendFormat(" ,SG.OFFICE_CODE as SG_OFFICE_CODE ");
                queryBuilder.AppendFormat(" ,SG.DESCRIPTION_TH as SG_DESCRIPTION_TH ");
                queryBuilder.AppendFormat(" ,SG.DESCRIPTION_EN as SG_DESCRIPTION_EN ");
                queryBuilder.AppendFormat(" ,SG.MANAGER_EMP_ID as SG_MANAGER_EMP_ID ");
                queryBuilder.AppendFormat(" ,SG.ACTIVE_FLAG as SG_ACTIVE_FLAG ,SG.CREATE_USER as SG_CREATE_USER ,SG.CREATE_DTM as SG_CREATE_DTM ,SG.UPDATE_USER as SG_UPDATE_USER ,SG.UPDATE_DTM as SG_UPDATE_DTM ");
                queryBuilder.AppendFormat(" ,SO.DESCRIPTION_TH as SO_DESCRIPTION_TH ");
                queryBuilder.AppendFormat(" ,SO.DESCRIPTION_EN as SO_DESCRIPTION_EN ");
                queryBuilder.AppendFormat(" ,T.TERRITORY_ID as T_TERRITORY_ID ");
                queryBuilder.AppendFormat(" ,T.MANAGER_EMP_ID as T_MANAGER_EMP_ID ");
                queryBuilder.AppendFormat(" ,T.ACTIVE_FLAG as T_ACTIVE_FLAG ");
                queryBuilder.AppendFormat(" from ORG_SALE_GROUP SG ");
                queryBuilder.AppendFormat(" inner join ORG_SALE_OFFICE SO on SO.OFFICE_CODE = SG.OFFICE_CODE ");
                queryBuilder.AppendFormat(" left join ORG_TERRITORY T on T.TERRITORY_ID = SG.TERRITORY_ID ");
                queryBuilder.AppendFormat(" where SG.GROUP_CODE = @groupCode ");
                QueryUtils.addParam(command, "groupCode", o.groupCode);// Add new


                // For Paging
                if (searchCriteria.searchOrder == 0) {
                    queryBuilder.AppendFormat(" ORDER BY SG.GROUP_CODE  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY SG.DESCRIPTION_TH  ");
                }
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

                    List<SaleGroupSaleOfficeForBackEndCustom> dataRecordList = new List<SaleGroupSaleOfficeForBackEndCustom>();
                    SaleGroupSaleOfficeForBackEndCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SaleGroupSaleOfficeForBackEndCustom();


                        dataRecord.GroupCode = QueryUtils.getValueAsString(record, "SG_GROUP_CODE");
                        dataRecord.OfficeCode = QueryUtils.getValueAsString(record, "SG_OFFICE_CODE");
                        dataRecord.DescriptionTh = QueryUtils.getValueAsString(record, "SG_DESCRIPTION_TH");
                        dataRecord.DescriptionEn = QueryUtils.getValueAsString(record, "SG_DESCRIPTION_EN");
                        dataRecord.ManagerEmpId = QueryUtils.getValueAsString(record, "SG_MANAGER_EMP_ID");
                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "SG_ACTIVE_FLAG");
                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "SG_CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "SG_CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "SG_UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "SG_UPDATE_DTM");

                        dataRecord.SaleOfficeDescriptionTh = QueryUtils.getValueAsString(record, "SO_DESCRIPTION_TH");
                        dataRecord.SaleOfficeDescriptionEn = QueryUtils.getValueAsString(record, "SO_DESCRIPTION_EN");


                        dataRecord.TerritoryId = QueryUtils.getValueAsDecimalRequired(record, "T_TERRITORY_ID");
                        dataRecord.TerritoryCode = QueryUtils.getValueAsString(record, "TERRITORY_CODE");
                        dataRecord.TerritoryNameTh = QueryUtils.getValueAsString(record, "TERRITORY_NAME_TH");
                        dataRecord.TerritoryNameEn = QueryUtils.getValueAsString(record, "TERRITORY_NAME_EN");
                        dataRecord.TerritoryManagerEmpId = QueryUtils.getValueAsString(record, "T_MANAGER_EMP_ID");
                        dataRecord.TerritoryActiveFlag = QueryUtils.getValueAsString(record, "T_ACTIVE_FLAG");
                        dataRecord.BuId = QueryUtils.getValueAsDecimal(record, "BU_ID");

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



        /*public async Task<EntitySearchResultBase<SaleGroupSaleOfficeForBackEndCustom>> getSaleGroupSaleOfficeForBackEnd(SearchCriteriaBase<AdmEmployeeCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                AdmEmployeeCriteria criteria = searchCriteria.model;
                var queryCommane = (from sg in context.OrgSaleGroup
                                    join so in context.OrgSaleOffice
                                    on sg.OfficeCode equals so.OfficeCode

                                    where ((criteria.groupCode == null ? 1 == 1 : sg.GroupCode == criteria.groupCode))
                                    orderby (searchCriteria.searchOrder == 0 ? sg.GroupCode : sg.DescriptionTh)
                                    select new
                                    {

                                        GroupCode = sg.GroupCode,
                                        OfficeCode = so.OfficeCode,
                                        DescriptionTh = sg.DescriptionTh,
                                        DescriptionEn = sg.DescriptionEn,
                                        ManagerEmpId = sg.ManagerEmpId,
                                        ActiveFlag = sg.ActiveFlag,
                                        CreateUser = sg.CreateUser,
                                        CreateDtm = sg.CreateDtm,
                                        UpdateUser = sg.UpdateUser,
                                        UpdateDtm = sg.UpdateDtm,

                                        SaleOfficeDescriptionTh = so.DescriptionTh,
                                        SaleOfficeDescriptionEn = so.DescriptionEn
                                    }
                                    );
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<SaleGroupSaleOfficeForBackEndCustom> searchResult = new EntitySearchResultBase<SaleGroupSaleOfficeForBackEndCustom>();
                searchResult.totalRecords = query.Count();

                List<SaleGroupSaleOfficeForBackEndCustom> saleLst = new List<SaleGroupSaleOfficeForBackEndCustom>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    SaleGroupSaleOfficeForBackEndCustom s = new SaleGroupSaleOfficeForBackEndCustom();
                    s.GroupCode = item.GroupCode;
                    s.OfficeCode = item.OfficeCode;
                    s.DescriptionTh = item.DescriptionTh;
                    s.DescriptionEn = item.DescriptionEn;
                    s.ManagerEmpId = item.ManagerEmpId;
                    s.ActiveFlag = item.ActiveFlag;
                    s.CreateUser = item.CreateUser;
                    s.CreateDtm = item.CreateDtm;
                    s.UpdateUser = item.UpdateUser;
                    s.UpdateDtm = item.UpdateDtm;

                    s.SaleOfficeDescriptionTh = item.SaleOfficeDescriptionTh;
                    s.SaleOfficeDescriptionEn = item.SaleOfficeDescriptionEn;

                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;

            }

        }
        */


        public async Task<EntitySearchResultBase<OrgSaleArea>> getSaleAreaForBackEnd(SearchCriteriaBase<OrgSaleOffice> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                OrgSaleOffice criteria = searchCriteria.model;
                var queryCommane = (from oa in context.OrgSaleOfficeArea
                                    join sa in context.OrgSaleArea
                                    on oa.AreaId equals sa.AreaId

                                    where ((criteria.OfficeCode == null ? 1 == 1 : oa.OfficeCode == criteria.OfficeCode))
                                    orderby (searchCriteria.searchOrder == 0 ? oa.OfficeCode : oa.OfficeCode)
                                    select sa
                                    );
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<OrgSaleArea> searchResult = new EntitySearchResultBase<OrgSaleArea>();
                searchResult.totalRecords = query.Count();

                List<OrgSaleArea> saleLst = new List<OrgSaleArea>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    OrgSaleArea s = new OrgSaleArea();
                    s.AreaId = item.AreaId;
                    s.OrgCode = item.OrgCode;
                    s.ChannelCode = item.ChannelCode;
                    s.DivisionCode = item.DivisionCode;
                    s.BussAreaCode = item.BussAreaCode;
                    s.ActiveFlag = item.ActiveFlag;
                    s.CreateUser = item.CreateUser;
                    s.CreateDtm = item.CreateDtm;
                    s.UpdateUser = item.UpdateUser;
                    s.UpdateDtm = item.UpdateDtm;

                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;

            }

        }


        public async Task<EntitySearchResultBase<OrgTerritory>> getTerritoryForBackEnd(SearchCriteriaBase<AdmEmployeeCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                AdmEmployeeCriteria criteria = searchCriteria.model;
                var queryCommane = (from t in context.OrgTerritory
                                    join st in context.OrgSaleTerritory
                                    on t.TerritoryId equals st.TerritoryId

                                    where ((criteria.empId == null ? 1 == 1 : st.EmpId == criteria.empId))
                                    orderby (searchCriteria.searchOrder == 0 ? st.TerritoryId : st.TerritoryId)
                                    select t
                                    );
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<OrgTerritory> searchResult = new EntitySearchResultBase<OrgTerritory>();
                searchResult.totalRecords = query.Count();

                List<OrgTerritory> saleLst = new List<OrgTerritory>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    OrgTerritory s = new OrgTerritory();
                    s.TerritoryId = item.TerritoryId;
                    s.TerritoryCode = item.TerritoryCode;
                    s.TerritoryNameTh = item.TerritoryNameTh;
                    s.TerritoryNameEn = item.TerritoryNameEn;
                    s.ManagerEmpId = item.ManagerEmpId;
                    s.BuId = item.BuId;
                    s.ActiveFlag = item.ActiveFlag;
                    s.CreateUser = item.CreateUser;
                    s.CreateDtm = item.CreateDtm;
                    s.UpdateUser = item.UpdateUser;
                    s.UpdateDtm = item.UpdateDtm;

                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;

            }

        }


        public async Task<EntitySearchResultBase<UserProfileForBackEndCustom>> getUserProfileForBackEndByToken(SearchCriteriaBase<GetSessioinCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                GetSessioinCriteria criteria = searchCriteria.model;
                var queryCommane = (from e in context.AdmEmployee
                                    join t in context.AdmUserAccessToken
                                    on e.EmpId equals t.EmpId 
                                    where t.AccessToken == criteria.Token
                                    select e
                                    );
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<UserProfileForBackEndCustom> searchResult = new EntitySearchResultBase<UserProfileForBackEndCustom>();
                searchResult.totalRecords = query.Count();

                List<UserProfileForBackEndCustom> saleLst = new List<UserProfileForBackEndCustom>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    UserProfileForBackEndCustom s = new UserProfileForBackEndCustom();
                    s.EmpId = item.EmpId;
                    s.CompanyCode = item.CompanyCode;
                    s.JobTitle = item.JobTitle;
                    s.TitleName = item.TitleName;
                    s.FirstName = item.FirstName;
                    s.LastName = item.LastName;
                    s.Gender = item.Gender;
                    s.Street = item.Street;
                    s.TellNo = item.TellNo;
                    s.CountryName = item.CountryName;
                    s.ProvinceCode = item.ProvinceCode;
                    s.GroupCode = item.GroupCode;
                    s.DistrictName = item.DistrictName;
                    s.SubdistrictName = item.SubdistrictName;
                    s.PostCode = item.PostCode;
                    s.Email = item.Email;
                    s.Status = item.Status;
                    s.ApproveEmpId = item.ApproveEmpId;
                    s.ActiveFlag = item.ActiveFlag;
                    s.CreateUser = item.CreateUser;
                    s.CreateDtm = item.CreateDtm;
                    s.UpdateUser = item.UpdateUser;
                    s.UpdateDtm = item.UpdateDtm;

                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;

            }

        }


        public async Task<EntitySearchResultBase<AdmEmployee>> getEmployeeByEmpId(SearchCriteriaBase<GetAdmEmployeeByEmpIdCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                GetAdmEmployeeByEmpIdCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select E.* ");
                queryBuilder.AppendFormat(" from ADM_EMPLOYEE E ");
                queryBuilder.AppendFormat(" WHERE E.EMP_ID = @EmpId ");
                sqlParameters.Add(QueryUtils.addSqlParameter("EmpId", o.EmpId));
                queryBuilder.AppendFormat(" ORDER BY E.EMP_ID  ");
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.AdmEmployee.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();// Add New
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<AdmEmployee> lst = context.AdmEmployee.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList(); // Add New
                EntitySearchResultBase<AdmEmployee> searchResult = new EntitySearchResultBase<AdmEmployee>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }


        public async Task<List<PermObjCodeCustom>> searchPermObjCode(string userName)
        {
            List<PermObjCodeCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" WITH n(PERM_OBJ_ID, PERM_OBJ_CODE, level, PARENT_ID, concatenador) AS ");
                queryBuilder.AppendFormat(" (");
                queryBuilder.AppendFormat("     SELECT PERM_OBJ_ID, PERM_OBJ_CODE, 1 as level, PARENT_ID ");
                queryBuilder.AppendFormat("     , '('+CONVERT(VARCHAR (MAX), PERM_OBJ_ID)+' - '+CONVERT(VARCHAR (MAX), 1)+')' as concatenador ");
                queryBuilder.AppendFormat("     FROM ADM_PERM_OBJECT ");
                queryBuilder.AppendFormat("         UNION ALL ");
                queryBuilder.AppendFormat("     SELECT m.PERM_OBJ_ID, m.PERM_OBJ_CODE, n.level + 1, m.PARENT_ID ");
                queryBuilder.AppendFormat("     , n.concatenador+' * ('+RIGHT('00'+CONVERT (VARCHAR (MAX), case when ISNULL(m.PARENT_ID, 0) = 0 then 0 else m.PERM_OBJ_ID END),3)+' - '+CONVERT(VARCHAR (MAX), n.level + 1)+')' as concatenador ");
                queryBuilder.AppendFormat("     FROM ADM_PERM_OBJECT as m, n ");
                queryBuilder.AppendFormat("     WHERE n.PERM_OBJ_ID = m.PARENT_ID ");
                queryBuilder.AppendFormat(" ) ");
                queryBuilder.AppendFormat(" SELECT n.PERM_OBJ_ID,n.PERM_OBJ_CODE,n.level,n.PARENT_ID,IIF(GP.PERM_OBJ_ID is null,'N','Y')  SELECTED_FLAG ");
                queryBuilder.AppendFormat(" FROM n  ");
                queryBuilder.AppendFormat(" INNER JOIN ADM_GROUP_PERM GP ON (GP.PERM_OBJ_ID = n.PERM_OBJ_ID)  ");
                queryBuilder.AppendFormat(" INNER JOIN ADM_GROUP_APP GA ON (GA.GROUP_APP_ID = GP.GROUP_APP_ID)  ");
                queryBuilder.AppendFormat(" INNER JOIN ADM_GROUP_USER GU ON (GU.GROUP_ID = GA.GROUP_ID)  ");
                queryBuilder.AppendFormat(" WHERE GU.EMP_ID = @EMP_ID ");
                queryBuilder.AppendFormat(" ORDER BY concatenador asc ");
                QueryUtils.addParam(command, "EMP_ID", userName);// Add new
                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    //

                    List<PermObjCodeCustom> dataRecordList = new List<PermObjCodeCustom>();
                    PermObjCodeCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new PermObjCodeCustom();

                        dataRecord.PermObjId = QueryUtils.getValueAsString(record, "PERM_OBJ_ID");
                        dataRecord.PermObjCode = QueryUtils.getValueAsString(record, "PERM_OBJ_CODE");
                        dataRecord.Level = QueryUtils.getValueAsString(record, "level");
                        dataRecord.ParentId = QueryUtils.getValueAsString(record, "PARENT_ID");
                        dataRecord.SelectedFlag = QueryUtils.getValueAsString(record, "SELECTED_FLAG");
                        //dataRecord.PermObjNameTh = QueryUtils.getValueAsString(record, "PERM_OBJ_NAME_TH");

                        dataRecordList.Add(dataRecord);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //
                    lst = dataRecordList;

                }
            }
            return lst;
        }


        public async Task<List<GetValidRoleCustom>> searchCountPermObject(string api_service_name, UserProfileForBack userProfile)
        {
            List<GetValidRoleCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" SELECT count(1) COUNT_PERM_OBJECT ");
                queryBuilder.AppendFormat(" FROM ADM_PERM_OBJECT_API OA ");
                queryBuilder.AppendFormat(" CROSS APPLY STRING_SPLIT(OA.[PERM_OBJ_ID], ',') T ");
                queryBuilder.AppendFormat(" where oa.api_service_name = @api_service_name  ");
                queryBuilder.AppendFormat(" and (T.[value] = '0' or  ");
                queryBuilder.AppendFormat(" exists(");
                queryBuilder.AppendFormat("         select 1  ");
                queryBuilder.AppendFormat("         From ADM_GROUP_APP GA ");
                queryBuilder.AppendFormat("         inner join ADM_GROUP_PERM GP on GP.GROUP_APP_ID = GA.GROUP_APP_ID ");
                queryBuilder.AppendFormat("         where GP.PERM_OBJ_ID = T.[value] and GA.GROUP_ID = @groupId ");
                queryBuilder.AppendFormat("         ) ");
                queryBuilder.AppendFormat(" )  ");


                QueryUtils.addParam(command, "api_service_name", api_service_name);// Add new
                QueryUtils.addParam(command, "groupId", userProfile.getUserData().GroupId);// Add new
                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    //

                    List<GetValidRoleCustom> dataRecordList = new List<GetValidRoleCustom>();
                    GetValidRoleCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new GetValidRoleCustom();

                        dataRecord.CountPermObject = QueryUtils.getValueAsString(record, "COUNT_PERM_OBJECT");

                        dataRecordList.Add(dataRecord);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //
                    lst = dataRecordList;

                }
            }
            return lst;
        }



    }
}
