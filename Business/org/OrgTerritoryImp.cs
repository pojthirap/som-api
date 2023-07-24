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
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.adm;
using MyFirstAzureWebApp.Utils;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Models.profile;
using System.Data;
using static MyFirstAzureWebApp.Entity.custom.SearchSalesTerritoryTabCustom;

namespace MyFirstAzureWebApp.Business.org
{

    public class OrgTerritoryImp : IOrgTerritory
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        // Action Search
        public async Task<EntitySearchResultBase<OrgTerritory>> Search(OrgTerritorySearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from ORG_TERRITORY where 1=1 ");
                OrgTerritoryCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.territoryId))
                    {
                        queryBuilder.AppendFormat(" and TERRITORY_ID  = @territoryId  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("territoryId", o.territoryId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.name))
                    {
                        queryBuilder.AppendFormat(" and TERRITORY_NAME_TH like @territoryName  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("territoryName", o.name));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.activeFlag))
                    {
                        queryBuilder.AppendFormat(" and ACTIVE_FLAG  = @ActiveFlag  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", o.activeFlag));// Add New
                    }
                }
                // For Paging
                if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY UPDATE_DTM DESC  ");
                }
                else
                {

                    queryBuilder.AppendFormat(" ORDER BY UPDATE_DTM DESC  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.OrgTerritory.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<OrgTerritory> lst = context.OrgTerritory.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<OrgTerritory> searchResult = new EntitySearchResultBase<OrgTerritory>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }

        // Action Add
        public async Task<OrgTerritory> Add(OrgTerritoryModel model)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for ORG_TERRITORY_SEQ ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  ORG_TERRITORY (TERRITORY_ID, TERRITORY_CODE, TERRITORY_NAME_TH, TERRITORY_NAME_EN, BU_ID, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, RIGHT('00000' + CAST(ISNULL((MAX(CAST(TERRITORY_CODE AS INT)) + 1),1) AS VARCHAR), 5), @TerritoryNameTh, @TerritoryNameEn, @BuId, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from ORG_TERRITORY ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TerritoryNameTh", model.TerritoryNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TerritoryNameEn", model.TerritoryNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BuId", model.getBuId() == 0 ? null : model.getBuId()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", model.getUserName()));// Add New
                        string queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        OrgTerritory re = new OrgTerritory();
                        re.TerritoryId = nextVal;
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

        // Action Edit
        public async Task<int> Update(OrgTerritoryModel model)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ORG_TERRITORY SET TERRITORY_NAME_TH = @TerritoryNameTh, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TERRITORY_ID=@TerritoryId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TerritoryNameTh", model.TerritoryNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", model.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", model.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TerritoryId", model.TerritoryId));// Add New
                        string queryStr = queryBuilder.ToString();
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

        public async Task<int> DeleteUate(OrgTerritoryModel model)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ORG_TERRITORY SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TERRITORY_ID=@TERRITORY_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", model.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TERRITORY_ID", model.TerritoryId));// Add New
                        string queryStr = queryBuilder.ToString();
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



        // Manage sale representative
        // Initial Sale Rep
        public async Task<EntitySearchResultBase<SearchInitialSaleRepCustom>> SearchInitialSaleRep(OrgSaleTerritorySearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                
                OrgSaleTerritoryCriteria criteria = searchCriteria.model;
                decimal territoryIdDec = String.IsNullOrEmpty(criteria.territoryId) ? 0 : Convert.ToDecimal(criteria.territoryId);
                var queryCommane = (from st in context.OrgSaleTerritory
                                    join e in context.AdmEmployee
                                    on st.EmpId equals e.EmpId
                                    join t in context.OrgTerritory
                                    on st.TerritoryId equals t.TerritoryId
                                    where (st.ActiveFlag == "Y")
                                    where ((criteria.territoryId == null ? 1 == 1 : st.TerritoryId == territoryIdDec))
                                    orderby ((searchCriteria.searchOrder == 1 ? st.UpdateDtm : st.UpdateDtm)) descending

                                    select new
                                    {
                                        SaleTerritoryId = st.SaleTerritoryId,
                                        TerritoryId = st.TerritoryId,
                                        EmpId = st.EmpId,
                                        
                                        CreateUser = st.CreateUser,
                                        CreateDtm = st.CreateDtm,
                                        UpdateUser = st.UpdateUser,
                                        UpdateDtm = st.UpdateDtm,

                                        TerritoryCode = t.TerritoryCode,
                                        TerritoryNameTh = t.TerritoryNameTh,
                                        TerritoryNameEn = t.TerritoryNameEn,
                                        ManagerEmpId = t.ManagerEmpId,
                                        ActiveFlag = t.ActiveFlag,

                                        CompanyCode = e.CompanyCode,
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
                                        JobTitle = e.JobTitle

                                    }
                                    );
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<SearchInitialSaleRepCustom> searchResult = new EntitySearchResultBase<SearchInitialSaleRepCustom>();
                searchResult.totalRecords = query.Count();

                List<SearchInitialSaleRepCustom> saleLst = new List<SearchInitialSaleRepCustom>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    SearchInitialSaleRepCustom s = new SearchInitialSaleRepCustom();
                    s.SaleTerritoryId = item.SaleTerritoryId;
                    s.TerritoryId = item.TerritoryId;
                    s.EmpId = item.EmpId;
                    s.ActiveFlag = item.ActiveFlag;
                    s.CreateUser = item.CreateUser;
                    s.CreateDtm = item.CreateDtm;
                    s.UpdateUser = item.UpdateUser;
                    s.UpdateDtm = item.UpdateDtm;
                    s.TerritoryCode = item.TerritoryCode;
                    s.TerritoryNameTh = item.TerritoryNameTh;
                    s.TerritoryNameEn = item.TerritoryNameEn;
                    s.ManagerEmpId = item.ManagerEmpId;
                    s.CompanyCode = item.CompanyCode;
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
                    s.JobTitle = item.JobTitle;
                    saleLst.Add(s);

                }
                searchResult.data = saleLst;
                return searchResult;
            }

        }



        public async Task<EntitySearchResultBase<OrgSearchRepCustom>> SearchRep(SearchCriteriaBase<OrgSearchRepCriteria> searchCriteria)
        {

            EntitySearchResultBase<OrgSearchRepCustom> searchResult = new EntitySearchResultBase<OrgSearchRepCustom>();
            List<OrgSearchRepCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                OrgSearchRepCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select E.ACTIVE_FLAG as E_ACTIVE_FLAG,  E.*, G.* ");
                queryBuilder.AppendFormat(" from ADM_EMPLOYEE E ");
                queryBuilder.AppendFormat(" inner join ADM_GROUP_USER GU on GU.EMP_ID = E.EMP_ID ");
                queryBuilder.AppendFormat(" inner join ORG_TERRITORY T on T.BU_ID = GU.BU_ID and T.TERRITORY_ID = @TerritoryId ");
                queryBuilder.AppendFormat(" left join ORG_SALE_GROUP G on G.GROUP_CODE = E.GROUP_CODE ");
                QueryUtils.addParam(command, "TerritoryId", o.TerritoryId);// Add new

                if (o != null)
                {


                    if (!String.IsNullOrEmpty(o.TerritoryId))
                    {
                        queryBuilder.AppendFormat(" where NOT EXISTS ");
                        queryBuilder.AppendFormat(" (SELECT 1 FROM ORG_SALE_TERRITORY ST WHERE ST.EMP_ID = E.EMP_ID and ST.TERRITORY_ID = @TerritoryId ) ");
                        //QueryUtils.addParam(command, "TerritoryId", o.TerritoryId);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.empId))
                    {
                        queryBuilder.AppendFormat(" and E.EMP_ID = @empId ");
                        QueryUtils.addParam(command, "empId", o.empId);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.name))
                    {
                        o.name = o.name.Replace(" ", "");
                        queryBuilder.AppendFormat("  and (E.FIRST_NAME+E.LAST_NAME like @FullName )  ");
                        QueryUtils.addParamLike(command, "FullName", o.name);// Add new

                        //queryBuilder.AppendFormat(" and (E.FIRST_NAME like @FullName OR E.LAST_NAME like @FullName )  ");
                        //QueryUtils.addParamLike(command, "FullName", o.name);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.groupCode))
                    {
                        queryBuilder.AppendFormat(" and G.GROUP_CODE = @groupCode  ");
                        QueryUtils.addParam(command, "groupCode", o.groupCode);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.description))
                    {
                        queryBuilder.AppendFormat("  and G.DESCRIPTION_TH = @description ");
                        QueryUtils.addParam(command, "description", o.description);// Add new
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
                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    //

                    List<OrgSearchRepCustom> dataRecordList = new List<OrgSearchRepCustom>();
                    OrgSearchRepCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new OrgSearchRepCustom();


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
                        dataRecord.OfficeCode = QueryUtils.getValueAsString(record, "OFFICE_CODE");
                        dataRecord.DescriptionTh = QueryUtils.getValueAsString(record, "DESCRIPTION_TH");
                        dataRecord.DescriptionEn = QueryUtils.getValueAsString(record, "DESCRIPTION_EN");
                        dataRecord.ManagerEmpId = QueryUtils.getValueAsString(record, "MANAGER_EMP_ID");
                        
                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "E_ACTIVE_FLAG");
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



        // Search Rep
 /*       public async Task<EntitySearchResultBase<OrgSearchRepCustom>> SearchRep(OrgSearchRepSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {

                OrgSearchRepCriteria criteria = searchCriteria.model;
                decimal TerritoryId_dec = Convert.ToDecimal(criteria.TerritoryId);
                var queryCommane = (from e in context.AdmEmployee
                                    join g in context.OrgSaleGroup
                                    on e.GroupCode equals g.GroupCode into empJgroup
                                    from x in empJgroup.DefaultIfEmpty()


                                    where(criteria.TerritoryId == null ? 1==1 :  !context.OrgSaleTerritory.Any(st =>
                                    st.EmpId == e.EmpId && st.TerritoryId == TerritoryId_dec
                                    ))


                                    where ((criteria.empId == null ? 1 == 1 : e.EmpId == criteria.empId))
                                    where ((criteria.name == null ? 1 == 1 : (e.FirstName.Contains(criteria.name) || e.LastName.Contains(criteria.name))))
                                    where ((criteria.groupCode == null ? 1 == 1 : x.GroupCode == criteria.groupCode))
                                    where ((criteria.description == null ? 1 == 1 : x.DescriptionTh == criteria.description))
                                    orderby (searchCriteria.searchOrder == 0 ? e.EmpId : x.DescriptionTh)

                                    select new
                                    {
                                        EmpId = e.EmpId,
                                        CompanyCode = e.CompanyCode,
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
                                        JobTitle = e.JobTitle,
                                        ActiveFlag = e.ActiveFlag,
                                        CreateUser = e.CreateUser,
                                        CreateDtm = e.CreateDtm,
                                        UpdateUser = e.UpdateUser,
                                        UpdateDtm = e.UpdateDtm,

                                        OfficeCode = x.OfficeCode,
                                        DescriptionTh = x.DescriptionTh,
                                        DescriptionEn = x.DescriptionEn,
                                        ManagerEmpId = x.ManagerEmpId

                                    }
                                    );
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<OrgSearchRepCustom> searchResult = new EntitySearchResultBase<OrgSearchRepCustom>();
                searchResult.totalRecords = query.Count();

                List<OrgSearchRepCustom> saleLst = new List<OrgSearchRepCustom>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    OrgSearchRepCustom s = new OrgSearchRepCustom();
                    s.EmpId = item.EmpId;
                    s.CompanyCode = item.CompanyCode;
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
                    s.JobTitle = item.JobTitle;
                    s.ActiveFlag = item.ActiveFlag;
                    s.CreateUser = item.CreateUser;
                    s.CreateDtm = item.CreateDtm;
                    s.UpdateUser = item.UpdateUser;
                    s.UpdateDtm = item.UpdateDtm;

                    s.OfficeCode = item.OfficeCode;
                    s.DescriptionTh = item.DescriptionTh;
                    s.DescriptionEn = item.DescriptionEn;
                    s.ManagerEmpId = item.ManagerEmpId;

                    saleLst.Add(s);

                }
                searchResult.data = saleLst;
                return searchResult;
            }

        }
        */

        // Map Sale Rep
        public async Task<List<OrgSaleTerritory>> MapSaleRep(OrgMapSaleRepModel model)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        /*var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" UPDATE ORG_TERRITORY SET [UPDATE_USER]=@UPDATE_USER, [UPDATE_DTM] = dbo.GET_SYSDATETIME() WHERE TERRITORY_ID = @TERRITORY_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", model.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TERRITORY_ID", model.TerritoryId));// Add New
                        string queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);*/

                        List<OrgSaleTerritory> list = new List<OrgSaleTerritory>();
                        for (int i = 0; i < model.EmpId.Count; i++)
                        {
                            string empId = model.EmpId[i];
                            //
                            var sqlParameters = new List<SqlParameter>();// Add New
                            StringBuilder queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat(" UPDATE [dbo].[ORG_SALE_TERRITORY]  ");
                            queryBuilder.AppendFormat(" SET [ACTIVE_FLAG]='Y', [UPDATE_USER]=@UPDATE_USER, [UPDATE_DTM]= dbo.GET_SYSDATETIME() ");
                            queryBuilder.AppendFormat(" WHERE TERRITORY_ID = @TERRITORY_ID and EMP_ID = @EMP_ID ");
                            sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", model.getUserName()));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("TERRITORY_ID", model.TerritoryId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("EMP_ID", empId));// Add New
                            string queryStr = queryBuilder.ToString();
                            queryStr = QueryUtils.cutStringNull(queryStr);
                            log.Debug("Query:" + queryStr);
                            Console.WriteLine("Query:" + queryStr);
                            int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                            //
                            if (numberOfRowInserted == 0)
                            {
                                var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                                p.Direction = System.Data.ParameterDirection.Output;
                                context.Database.ExecuteSqlRaw("set @result = next value for ORG_SALE_TERRITORY_SEQ ", p);
                                var nextVal = (int)p.Value;

                                sqlParameters = new List<SqlParameter>();// Add New
                                queryBuilder = new StringBuilder();
                                queryBuilder.AppendFormat("INSERT INTO  ORG_SALE_TERRITORY (SALE_TERRITORY_ID, TERRITORY_ID, EMP_ID, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                                queryBuilder.AppendFormat("VALUES(@SALE_TERRITORY_ID, @TERRITORY_ID, @EMP_ID, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                                sqlParameters.Add(QueryUtils.addSqlParameter("SALE_TERRITORY_ID", nextVal));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("TERRITORY_ID", model.TerritoryId));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("EMP_ID", empId));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("USER", model.getUserName()));// Add New
                                queryStr = queryBuilder.ToString();
                                queryStr = QueryUtils.cutStringNull(queryStr);
                                log.Debug("Query:" + queryStr);
                                Console.WriteLine("Query:" + queryStr);
                                numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                                OrgSaleTerritory re = new OrgSaleTerritory();
                                re.SaleTerritoryId = nextVal;
                                list.Add(re);
                            }
                        }
                        transaction.Commit();
                        return list;





                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }


        // Dell Map Sale Rep
        public async Task<int> DellMapSaleRep(OrgSaleTerritoryModel model, UserProfileForBack userProfile)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        //queryBuilder.AppendFormat("DELETE FROM ORG_SALE_TERRITORY  WHERE SALE_TERRITORY_ID=@SALE_TERRITORY_ID ");

                        queryBuilder.AppendFormat("UPDATE [dbo].[ORG_SALE_TERRITORY]  ");
                        queryBuilder.AppendFormat("SET [ACTIVE_FLAG]='N', [UPDATE_USER]=@USER, [UPDATE_DTM]=dbo.GET_SYSDATETIME() ");
                        queryBuilder.AppendFormat("WHERE SALE_TERRITORY_ID = @SALE_TERRITORY_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("SALE_TERRITORY_ID", model.SaleTerritoryId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        string queryStr = queryBuilder.ToString();
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





        // updTerritoryByManagerEmpId
        public async Task<int> updTerritoryByManagerEmpId(OrgTerritoryModel model)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ORG_TERRITORY SET MANAGER_EMP_ID = @MANAGER_EMP_ID, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TERRITORY_ID=@TERRITORY_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("MANAGER_EMP_ID", model.ManagerEmpId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", model.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TERRITORY_ID", model.TerritoryId));// Add New
                        string queryStr = queryBuilder.ToString();
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









        public async Task<EntitySearchResultBase<SearchSalesTerritoryTabCustom>> searchSalesTerritoryTab(SearchCriteriaBase<ProspectCriteria> searchCriteria, UserProfileForBack userProfile)
        {
            ProspectCriteria criteria = searchCriteria.model;
            StringBuilder queryBuilder = new StringBuilder();
            List<SearchSalesTerritoryTabCustom> lst = new List<SearchSalesTerritoryTabCustom>();

            SearchSalesTerritoryTabCustom saleTerritory = new SearchSalesTerritoryTabCustom();
            List<OrgTerritory> myTerritory = new List<OrgTerritory>();
            List<OrgTerritory> dedicatedTerritory = new List<OrgTerritory>();
            List<ObjectSalesRep> salesRep = new List<ObjectSalesRep>();

            saleTerritory.MyTerritory = myTerritory;
            saleTerritory.DedicatedTerritory = dedicatedTerritory;
            saleTerritory.SalesRep = salesRep;
            lst.Add(saleTerritory);
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Start Query Scope 
                // Set saleTerritory.myTerritory
                //queryBuilder.AppendFormat(" select * from ORG_TERRITORY T where exists (select 1 from ORG_SALE_TERRITORY ST where ST.TERRITORY_ID = T.TERRITORY_ID and ST.EMP_ID = (select sale_rep_id from PROSPECT where PROSPECT_ID=@ProspectId) )   ");
                queryBuilder.AppendFormat(" select T.*");
                queryBuilder.AppendFormat(" from ORG_TERRITORY T  ");
                queryBuilder.AppendFormat(" where exists (select 1 from ORG_SALE_GROUP SG where T.TERRITORY_ID = SG.TERRITORY_ID and SG.GROUP_CODE = (select group_code from PROSPECT where PROSPECT_ID=@ProspectId))  ");
                QueryUtils.addParam(command, "ProspectId", criteria.ProspectId);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    OrgTerritory territory = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        territory = new OrgTerritory();

                            territory.TerritoryId = QueryUtils.getValueAsDecimalRequired(record, "TERRITORY_ID");
                            territory.TerritoryCode = QueryUtils.getValueAsString(record, "TERRITORY_CODE");
                            territory.TerritoryNameTh = QueryUtils.getValueAsString(record, "TERRITORY_NAME_TH");
                            territory.TerritoryNameEn = QueryUtils.getValueAsString(record, "TERRITORY_NAME_EN");
                            territory.ManagerEmpId = QueryUtils.getValueAsString(record, "MANAGER_EMP_ID");
                            territory.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                            territory.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                            territory.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                            territory.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                            territory.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");
                            territory.BuId = QueryUtils.getValueAsDecimal(record, "BU_ID");

                        myTerritory.Add(territory);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 

                // Start Query Scope 
                // Set saleTerritory.dedicatedTerritory
                queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select T.*,DT.PROSP_DEDICATE_ID from ORG_TERRITORY T inner join PROSPECT_DEDICATE_TERT DT on DT.TERRITORY_ID = T.TERRITORY_ID and DT.PROSPECT_ID = @ProspectId   ");
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                await using (var reader = command.ExecuteReader())
                {
                    OrgTerritory territory = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        territory = new OrgTerritory();
                            territory.TerritoryId = QueryUtils.getValueAsDecimalRequired(record, "TERRITORY_ID");
                            territory.TerritoryCode = QueryUtils.getValueAsString(record, "TERRITORY_CODE");
                            territory.TerritoryNameTh = QueryUtils.getValueAsString(record, "TERRITORY_NAME_TH");
                            territory.TerritoryNameEn = QueryUtils.getValueAsString(record, "TERRITORY_NAME_EN");
                            territory.ManagerEmpId = QueryUtils.getValueAsString(record, "MANAGER_EMP_ID");
                            territory.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                            territory.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                            territory.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                            territory.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                            territory.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");
                            territory.BuId = QueryUtils.getValueAsDecimal(record, "BU_ID");
                            territory.ProspDedicateId = QueryUtils.getValueAsDecimal(record, "PROSP_DEDICATE_ID");

                        dedicatedTerritory.Add(territory);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 


                // Set saleTerritory.salesRep 

                queryBuilder = new StringBuilder();

                //queryBuilder.AppendFormat("  select E.*,G.*,SG.*,T.* ");
                queryBuilder.AppendFormat("  select ");
                queryBuilder.AppendFormat("  E.GROUP_CODE AS E_GROUP_CODE, E.ACTIVE_FLAG AS E_ACTIVE_FLAG, E.CREATE_USER AS E_CREATE_USER, E.CREATE_DTM AS E_CREATE_DTM, E.UPDATE_USER AS E_UPDATE_USER, E.UPDATE_DTM AS E_UPDATE_DTM, E.*,  ");
                queryBuilder.AppendFormat("  G.GROUP_CODE AS G_GROUP_CODE, G.ACTIVE_FLAG AS G_ACTIVE_FLAG, G.CREATE_USER AS G_CREATE_USER, G.CREATE_DTM AS G_CREATE_DTM, G.UPDATE_USER AS G_UPDATE_USER, G.UPDATE_DTM AS G_UPDATE_DTM, G.*,  ");
                queryBuilder.AppendFormat("  SG.GROUP_CODE AS SG_GROUP_CODE, SG.ACTIVE_FLAG AS SG_ACTIVE_FLAG, SG.CREATE_USER AS SG_CREATE_USER, SG.CREATE_DTM AS SG_CREATE_DTM, SG.UPDATE_USER AS SG_UPDATE_USER, SG.UPDATE_DTM AS SG_UPDATE_DTM, SG.*, ");
                queryBuilder.AppendFormat("  T.MANAGER_EMP_ID AS T_MANAGER_EMP_ID, T.ACTIVE_FLAG AS T_ACTIVE_FLAG, T.CREATE_USER AS T_CREATE_USER, T.CREATE_DTM AS T_CREATE_DTM, T.UPDATE_USER AS T_UPDATE_USER, T.UPDATE_DTM AS T_UPDATE_DTM, T.* ");
                queryBuilder.AppendFormat("  from ADM_EMPLOYEE E  ");
                queryBuilder.AppendFormat("  inner join ADM_GROUP_USER GU on GU.EMP_ID = E.EMP_ID  ");
                queryBuilder.AppendFormat("  inner join ADM_GROUP G on G.GROUP_ID = GU.GROUP_ID  ");
                queryBuilder.AppendFormat("  inner join ORG_SALE_GROUP SG on SG.GROUP_CODE = E.GROUP_CODE  ");
                queryBuilder.AppendFormat("  inner join ORG_TERRITORY T on T.TERRITORY_ID = SG.TERRITORY_ID ");
                queryBuilder.AppendFormat("  where G.ACTIVE_FLAG = 'Y'  ");
                /*queryBuilder.AppendFormat("  and exists (   ");
                queryBuilder.AppendFormat("  	select 1 from ORG_SALE_TERRITORY ST   ");
                queryBuilder.AppendFormat("  	where ST.EMP_ID = E.EMP_ID   ");
                queryBuilder.AppendFormat("  	and ST.TERRITORY_ID IN (  ");
                queryBuilder.AppendFormat("  		select T.TERRITORY_ID  ");
                queryBuilder.AppendFormat("  		from ORG_TERRITORY T   ");
                queryBuilder.AppendFormat("          where ");
                queryBuilder.AppendFormat("          (exists (select 1 from ORG_SALE_TERRITORY EST where EST.TERRITORY_ID = T.TERRITORY_ID and EST.EMP_ID =  (select sale_rep_id from PROSPECT where PROSPECT_ID=@ProspectId) ) or   ");
                queryBuilder.AppendFormat("  		exists (select 1 from PROSPECT_DEDICATE_TERT EDT where EDT.TERRITORY_ID = T.TERRITORY_ID and EDT.PROSPECT_ID = @ProspectId ))  ");
                queryBuilder.AppendFormat("  	)  ");
                queryBuilder.AppendFormat("  )  ");*/
                queryBuilder.AppendFormat("  and(");
                queryBuilder.AppendFormat("        exists (select 1 from ORG_SALE_GROUP EST where EST.TERRITORY_ID = T.TERRITORY_ID and EST.GROUP_CODE =  (select group_code from PROSPECT where PROSPECT_ID=@ProspectId)) or  ");
                queryBuilder.AppendFormat("        exists (select 1 from PROSPECT_DEDICATE_TERT EDT where EDT.TERRITORY_ID = T.TERRITORY_ID and EDT.PROSPECT_ID = @ProspectId) ");
                queryBuilder.AppendFormat("  ) ");

                if (!String.IsNullOrEmpty(criteria.FullName))
                {
                    criteria.FullName = criteria.FullName.Replace(" ", "");
                    queryBuilder.AppendFormat("  and (E.FIRST_NAME+E.LAST_NAME like @FullName )  ");
                    QueryUtils.addParamLike(command, "FullName", criteria.FullName);// Add new

                }

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                await using (var reader = command.ExecuteReader())
                {
                    //salesRep

                    //
                    AdmEmployee admEmployee;
                    OrgSaleGroup orgSaleGroup;
                    AdmGroup admGroup;
                    OrgTerritory orgTerritory;
                    List<OrgTerritory> listOrgTerritory;

                    //

                    ObjectSalesRep objectSalesRep;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        objectSalesRep = new ObjectSalesRep();

                            admEmployee = new AdmEmployee();
                            orgSaleGroup = new OrgSaleGroup();
                            admGroup = new AdmGroup();
                            orgTerritory = new OrgTerritory();

                            admEmployee.GroupCode = QueryUtils.getValueAsString(record, "E_GROUP_CODE");
                            admEmployee.ActiveFlag = QueryUtils.getValueAsString(record, "E_ACTIVE_FLAG");
                            admEmployee.CreateUser = QueryUtils.getValueAsString(record, "E_CREATE_USER");
                            admEmployee.CreateDtm = QueryUtils.getValueAsDateTime(record, "E_CREATE_DTM");
                            admEmployee.UpdateUser = QueryUtils.getValueAsString(record, "E_UPDATE_USER");
                            admEmployee.UpdateDtm = QueryUtils.getValueAsDateTime(record, "E_UPDATE_DTM");
                            admEmployee.EmpId = QueryUtils.getValueAsString(record, "EMP_ID");
                            admEmployee.CompanyCode = QueryUtils.getValueAsString(record, "COMPANY_CODE");
                            admEmployee.JobTitle = QueryUtils.getValueAsString(record, "JOB_TITLE");
                            admEmployee.TitleName = QueryUtils.getValueAsString(record, "TITLE_NAME");
                            admEmployee.FirstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                            admEmployee.LastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                            admEmployee.Gender = QueryUtils.getValueAsString(record, "GENDER");
                            admEmployee.Street = QueryUtils.getValueAsString(record, "STREET");
                            admEmployee.TellNo = QueryUtils.getValueAsString(record, "TELL_NO");
                            admEmployee.CountryName = QueryUtils.getValueAsString(record, "COUNTRY_NAME");
                            admEmployee.ProvinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                            admEmployee.DistrictName = QueryUtils.getValueAsString(record, "DISTRICT_NAME");
                            admEmployee.SubdistrictName = QueryUtils.getValueAsString(record, "SUBDISTRICT_NAME");
                            admEmployee.PostCode = QueryUtils.getValueAsString(record, "POST_CODE");
                            admEmployee.Email = QueryUtils.getValueAsString(record, "EMAIL");
                            admEmployee.Status = QueryUtils.getValueAsString(record, "STATUS");
                            admEmployee.ApproveEmpId = QueryUtils.getValueAsString(record, "APPROVE_EMP_ID");

                            admGroup.GroupCode = QueryUtils.getValueAsString(record, "G_GROUP_CODE");
                            admGroup.ActiveFlag = QueryUtils.getValueAsString(record, "G_ACTIVE_FLAG");
                            admGroup.CreateUser = QueryUtils.getValueAsString(record, "G_CREATE_USER");
                            admGroup.CreateDtm = QueryUtils.getValueAsDateTime(record, "G_CREATE_DTM");
                            admGroup.UpdateUser = QueryUtils.getValueAsString(record, "G_UPDATE_USER");
                            admGroup.UpdateDtm = QueryUtils.getValueAsDateTime(record, "G_UPDATE_DTM");
                            admGroup.GroupId = QueryUtils.getValueAsDecimal(record, "GROUP_ID");
                            admGroup.GroupNameTh = QueryUtils.getValueAsString(record, "GROUP_NAME_TH");
                            admGroup.GroupNameEn = QueryUtils.getValueAsString(record, "GROUP_NAME_EN");
                            admGroup.EffectiveDate = QueryUtils.getValueAsDateTime(record, "EFFECTIVE_DATE");
                            admGroup.ExpiryDate = QueryUtils.getValueAsDateTime(record, "EXPIRY_DATE");

                            orgSaleGroup.GroupCode = QueryUtils.getValueAsString(record, "SG_GROUP_CODE");
                            orgSaleGroup.ActiveFlag = QueryUtils.getValueAsString(record, "SG_ACTIVE_FLAG");
                            orgSaleGroup.CreateUser = QueryUtils.getValueAsString(record, "SG_CREATE_USER");
                            orgSaleGroup.CreateDtm = QueryUtils.getValueAsDateTime(record, "SG_CREATE_DTM");
                            orgSaleGroup.UpdateUser = QueryUtils.getValueAsString(record, "SG_UPDATE_USER");
                            orgSaleGroup.UpdateDtm = QueryUtils.getValueAsDateTime(record, "SG_UPDATE_DTM");
                            orgSaleGroup.OfficeCode = QueryUtils.getValueAsString(record, "OFFICE_CODE");
                            orgSaleGroup.DescriptionTh = QueryUtils.getValueAsString(record, "DESCRIPTION_TH");
                            orgSaleGroup.DescriptionEn = QueryUtils.getValueAsString(record, "DESCRIPTION_EN");
                            orgSaleGroup.ManagerEmpId = QueryUtils.getValueAsString(record, "MANAGER_EMP_ID");

                            //orgTerritory
                            orgTerritory.TerritoryId = QueryUtils.getValueAsDecimalRequired(record, "TERRITORY_ID");
                            orgTerritory.TerritoryCode = QueryUtils.getValueAsString(record, "TERRITORY_CODE");
                            orgTerritory.TerritoryNameTh = QueryUtils.getValueAsString(record, "TERRITORY_NAME_TH");
                            orgTerritory.TerritoryNameEn = QueryUtils.getValueAsString(record, "TERRITORY_NAME_EN");
                            orgTerritory.ManagerEmpId = QueryUtils.getValueAsString(record, "T_MANAGER_EMP_ID");
                            orgTerritory.ActiveFlag = QueryUtils.getValueAsString(record, "T_ACTIVE_FLAG");
                            orgTerritory.CreateUser = QueryUtils.getValueAsString(record, "T_CREATE_USER");
                            orgTerritory.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "T_CREATE_DTM");
                            orgTerritory.UpdateUser = QueryUtils.getValueAsString(record, "T_UPDATE_USER");
                            orgTerritory.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "T_UPDATE_DTM");
                            orgTerritory.BuId = QueryUtils.getValueAsDecimal(record, "BU_ID");
                            listOrgTerritory = new List<OrgTerritory>();
                            listOrgTerritory.Add(orgTerritory);

                            objectSalesRep.AdmEmployee = admEmployee;
                            objectSalesRep.OrgSaleGroup = orgSaleGroup;
                            objectSalesRep.AdmGroup = admGroup;
                            objectSalesRep.listOrgTerritory = listOrgTerritory;
                  
                        salesRep.Add(objectSalesRep);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 


                /*foreach (ObjectSalesRep saleRep in saleTerritory.SalesRep)
                //for (int idx = 0; idx < salesRep.Count; idx++)
                {
                    string empId_ = saleRep.AdmEmployee.EmpId;
                    List<OrgTerritory> orgTerritoryLst = new List<OrgTerritory>();
                    // Start Query Scope 
                    // SFor ObjectSalesRep saleRep : saleTerritory.salesRep
                    command.Parameters.Clear();
                    queryBuilder = new StringBuilder();
                    queryBuilder.AppendFormat("  select T.* from ORG_TERRITORY T where exists (select 1 from ORG_SALE_TERRITORY ST where ST.TERRITORY_ID = T.TERRITORY_ID and ST.EMP_ID = @empId_ )   ");
                    QueryUtils.addParam(command, "empId_", empId_);// Add new
                    log.Debug("Query:" + queryBuilder.ToString());
                    Console.WriteLine("Query:" + queryBuilder.ToString());
                    command.CommandText = queryBuilder.ToString();
                    await using (var reader = command.ExecuteReader())
                    {
                        
                        OrgTerritory orgTerritory = null;
                        while (reader.Read())
                        {
                            IDataRecord record = (IDataRecord)reader;
                            orgTerritory = new OrgTerritory();
                                orgTerritory.TerritoryId = QueryUtils.getValueAsDecimalRequired(record, "TERRITORY_ID");
                                orgTerritory.TerritoryCode = QueryUtils.getValueAsString(record, "TERRITORY_CODE");
                                orgTerritory.TerritoryNameTh = QueryUtils.getValueAsString(record, "TERRITORY_NAME_TH");
                                orgTerritory.TerritoryNameEn = QueryUtils.getValueAsString(record, "TERRITORY_NAME_EN");
                                orgTerritory.ManagerEmpId = QueryUtils.getValueAsString(record, "MANAGER_EMP_ID");
                                orgTerritory.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                                orgTerritory.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                                orgTerritory.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                                orgTerritory.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                                orgTerritory.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");
                                orgTerritory.BuId = QueryUtils.getValueAsDecimal(record, "BU_ID");
                        
                            orgTerritoryLst.Add(orgTerritory);
                        }
                        // Call Close when done reading.
                        reader.Close();
                    }
                    // End Query Scope 

                    saleRep.listOrgTerritory = orgTerritoryLst;
                }*/



            }

            EntitySearchResultBase<SearchSalesTerritoryTabCustom> searchResult = new EntitySearchResultBase<SearchSalesTerritoryTabCustom>();
            searchResult.data = lst; 
            return searchResult;
        }


        public void WriteColumn(IDataRecord record , int i)
        {
            //columns.Add(record.GetName(i));
            var columnName = record.GetName(i);//"ColumnID"
            var valR1 = record.GetValue(record.GetOrdinal(columnName)).ToString();
            //Console.WriteLine(columnName);
            //Console.WriteLine(valR1);
            log.Debug(columnName);
            log.Debug(valR1);
        }







        public async Task<EntitySearchResultBase<OrgTerritory>> getTerritoryForDedicated(SearchCriteriaBase<GetTerritoryForDedicatedCriteria> searchCriteria, UserProfileForBack userProfile)
        {
            /*string inTerritorySrt = "";
            List<string> inTerritoryLst = new List<string>();
            if (userProfile.OrgTerritory != null && userProfile.OrgTerritory.data != null && userProfile.OrgTerritory.data.Count != 0)
            {
                foreach (OrgTerritory t in userProfile.OrgTerritory.data)
                {
                    inTerritoryLst.Add(t.TerritoryId.ToString());
                }
                inTerritorySrt = String.Join(",", inTerritoryLst);
            }*/

            GetTerritoryForDedicatedCriteria o = searchCriteria.model;
            

            EntitySearchResultBase<OrgTerritory> searchResult = new EntitySearchResultBase<OrgTerritory>();
            List<OrgTerritory> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select* ");
                queryBuilder.AppendFormat(" from ORG_TERRITORY T ");
                queryBuilder.AppendFormat(" where T.BU_ID = @BU_ID  ");
                //queryBuilder.AppendFormat(" and T.TERRITORY_ID not in (" + QueryUtils.getParamIn("inTerritorySrt", inTerritorySrt) + ") ");
                queryBuilder.AppendFormat(" and T.TERRITORY_ID != @orgSaleGroup_territoryId ");
                queryBuilder.AppendFormat(" and not exists(select 1 from PROSPECT_DEDICATE_TERT DT where DT.TERRITORY_ID = T.TERRITORY_ID and DT.PROSPECT_ID = @PROSPECT_ID ) ");
                /*queryBuilder.AppendFormat(" and not exists(");
                queryBuilder.AppendFormat(" select 1 ");
                queryBuilder.AppendFormat(" from PROSPECT P ");
                queryBuilder.AppendFormat(" inner join ORG_SALE_TERRITORY ST on ST.EMP_ID = P.SALE_REP_ID ");
                queryBuilder.AppendFormat(" where ST.TERRITORY_ID = T.TERRITORY_ID ");
                queryBuilder.AppendFormat(" and P.PROSPECT_ID = @PROSPECT_ID ");
                queryBuilder.AppendFormat(" ) ");*/
                queryBuilder.AppendFormat(" and T.ACTIVE_FLAG = 'Y' ");

                QueryUtils.addParam(command, "BU_ID", userProfile.getBuId());// Add new
                QueryUtils.addParam(command, "orgSaleGroup_territoryId", userProfile.getSaleGroupSaleOffice().TerritoryId);// Add new
                //QueryUtils.addParamIn(command, "inTerritorySrt", inTerritorySrt);
                QueryUtils.addParam(command, "PROSPECT_ID", o.PropectId);// Add new

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY TERRITORY_ID  ");
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

                    List<OrgTerritory> orgTerritoryLst = new List<OrgTerritory>();
                    OrgTerritory territory = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        territory = new OrgTerritory();


                        territory.TerritoryId = QueryUtils.getValueAsDecimalRequired(record, "TERRITORY_ID");
                        territory.TerritoryCode = QueryUtils.getValueAsString(record, "TERRITORY_CODE");
                        territory.TerritoryNameTh = QueryUtils.getValueAsString(record, "TERRITORY_NAME_TH");
                        territory.TerritoryNameEn = QueryUtils.getValueAsString(record, "TERRITORY_NAME_EN");
                        territory.ManagerEmpId = QueryUtils.getValueAsString(record, "MANAGER_EMP_ID");
                        territory.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        territory.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        territory.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                        territory.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        territory.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");
                        territory.BuId = QueryUtils.getValueAsDecimal(record, "BU_ID");

                        orgTerritoryLst.Add(territory);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //
                    lst = orgTerritoryLst;
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }



        public async Task<EntitySearchResultBase<SearchSaleGroupForMapSaleTerritoryCustom>> searchSaleGroupForMapSaleTerritory(SearchCriteriaBase<SearchSaleGroupForMapSaleTerritoryCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchSaleGroupForMapSaleTerritoryCustom> searchResult = new EntitySearchResultBase<SearchSaleGroupForMapSaleTerritoryCustom>();
            List<SearchSaleGroupForMapSaleTerritoryCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchSaleGroupForMapSaleTerritoryCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select G.GROUP_CODE,DESCRIPTION_TH ");
                queryBuilder.AppendFormat(" from ORG_SALE_GROUP G ");
                //queryBuilder.AppendFormat(" where G.TERRITORY_ID != @TerritoryId ");//#Require
                queryBuilder.AppendFormat(" where IIF(G.TERRITORY_ID is null, -1, G.TERRITORY_ID) != @TerritoryId ");//#Require

                QueryUtils.addParam(command, "TerritoryId", o.TerritoryId);// Add new

                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.GroupCode))
                    {
                        queryBuilder.AppendFormat(" and G.GROUP_CODE = @groupCode  ");
                        QueryUtils.addParam(command, "groupCode", o.GroupCode);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.DescriptionTh))
                    {
                        queryBuilder.AppendFormat("  and G.DESCRIPTION_TH like @description ");
                        QueryUtils.addParamLike(command, "description", o.DescriptionTh);// Add new
                    }
                }

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY G.GROUP_CODE  ");
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

                    List<SearchSaleGroupForMapSaleTerritoryCustom> dataRecordList = new List<SearchSaleGroupForMapSaleTerritoryCustom>();
                    SearchSaleGroupForMapSaleTerritoryCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchSaleGroupForMapSaleTerritoryCustom();

                        dataRecord.GroupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        dataRecord.DescriptionTh = QueryUtils.getValueAsString(record, "DESCRIPTION_TH");

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



    }
}
