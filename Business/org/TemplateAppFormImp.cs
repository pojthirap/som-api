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
using MyFirstAzureWebApp.Models.profile;
using System.Data;
using MyFirstAzureWebApp.Entity.custom;
using static MyFirstAzureWebApp.Entity.custom.GetTaskTemplateAppFormForRecordCustom;
using MyFirstAzureWebApp.Models.record;
using System.Text.Json;

namespace MyFirstAzureWebApp.Business.org
{

    public class TemplateAppFormImp : ITemplateAppForm
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<TemplateAppForm>> Search(SearchCriteriaBase<TemplateAppFormCriteria> searchCriteria, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" SELECT IIF(USED_FLAG = 'Y' or CREATE_USER != @User,'N','Y') EDIT_FLAG,*  FROM TEMPLATE_APP_FORM  WHERE (PUBLIC_FLAG = 'Y' OR CREATE_USER = @User)  ");
                sqlParameters.Add(QueryUtils.addSqlParameter("User", userProfile.getUserName()));
                TemplateAppFormCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.TpAppFormId))
                    {
                        queryBuilder.AppendFormat(" and TP_APP_FORM_ID  = @TpAppFormId  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpAppFormId", o.TpAppFormId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.TpCateId))
                    {
                        queryBuilder.AppendFormat(" and TP_CATE_ID  = @TpCateId  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpCateId", o.TpCateId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.TpCode))
                    {
                        queryBuilder.AppendFormat(" and TP_CODE  = @TpCode  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpCode", o.TpCode));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.TpNameTh))
                    {
                        queryBuilder.AppendFormat(" and TP_NAME_TH like @TpNameTh   ", o.TpNameTh);
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("TpNameTh", o.TpNameTh));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.ActiveFlag))
                    {
                        queryBuilder.AppendFormat(" and ACTIVE_FLAG  = @ActiveFlag  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", o.ActiveFlag));// Add New
                    }
                }
                // For Paging
                if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY UPDATE_DTM DESC  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY TP_CODE  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.TemplateAppForm.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<TemplateAppForm> lst = context.TemplateAppForm.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<TemplateAppForm> searchResult = new EntitySearchResultBase<TemplateAppForm>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }

        public async Task<TemplateAppForm> Add(TemplateAppFormModel templateAppFormModel)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for TEMPLATE_APP_FORM_SEQ", p);
                        var nextVal = (int)p.Value;
                        //string code_ = QueryUtils.padLeft(nextVal.ToString(), '0', 4);

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  TEMPLATE_APP_FORM (TP_APP_FORM_ID, TP_CATE_ID, TP_CODE, TP_NAME_TH, TP_NAME_EN,PUBLIC_FLAG, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, @TpCateId, RIGHT('00000' + CAST(ISNULL((MAX(CAST(TP_CODE AS INT)) + 1),1) AS VARCHAR), 5), @TpNameTh, @TpNameEn, @PublicFlag, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from TEMPLATE_APP_FORM ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpCateId", templateAppFormModel.TpCateId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpNameTh", templateAppFormModel.TpNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpNameEn", templateAppFormModel.TpNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PublicFlag", templateAppFormModel.PublicFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", templateAppFormModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        TemplateAppForm re = new TemplateAppForm();
                        re.TpAppFormId = nextVal;
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

        public async Task<int> Update(TemplateAppFormModel templateAppFormModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_APP_FORM SET TP_CATE_ID= @TpCateId, TP_NAME_TH=@TpNameTh, TP_NAME_EN=@TpNameEn, PUBLIC_FLAG=@PublicFlag, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TP_APP_FORM_ID=@TpAppFormId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpCateId", templateAppFormModel.TpCateId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpNameTh", templateAppFormModel.TpNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpNameEn", templateAppFormModel.TpNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PublicFlag", templateAppFormModel.PublicFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", templateAppFormModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", templateAppFormModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpAppFormId", templateAppFormModel.TpAppFormId));// Add New
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


        public async Task<int> DeleteUpdate(TemplateAppFormModel templateAppFormModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_APP_FORM SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TP_APP_FORM_ID=@TP_APP_FORM_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", templateAppFormModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_APP_FORM_ID", templateAppFormModel.TpAppFormId));// Add New
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




        public async Task<EntitySearchResultBase<GetTaskTemplateAppFormForCreatPlanCustom>> getTaskTemplateAppFormForCreatPlan(SearchCriteriaBase<GetTaskTemplateAppFormForCreatPlanCriteria> searchCriteria, UserProfileForBack userProfile)
        {

            EntitySearchResultBase<GetTaskTemplateAppFormForCreatPlanCustom> searchResult = new EntitySearchResultBase<GetTaskTemplateAppFormForCreatPlanCustom>();
            List<GetTaskTemplateAppFormForCreatPlanCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                GetTaskTemplateAppFormForCreatPlanCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select TP_APP_FORM_ID CODE,TP_NAME_TH DESCRIPTION,'A' TASK_TYPE ");
                queryBuilder.AppendFormat(" from TEMPLATE_APP_FORM ");
                queryBuilder.AppendFormat(" where (PUBLIC_FLAG = 'Y' or CREATE_USER = @USER) ");
                queryBuilder.AppendFormat(" and ACTIVE_FLAG = 'Y' ");
                QueryUtils.addParam(command, "USER", userProfile.getEmpId());// Add new

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY TP_NAME_TH  ");
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

                    List<GetTaskTemplateAppFormForCreatPlanCustom> dataRecordList = new List<GetTaskTemplateAppFormForCreatPlanCustom>();
                    GetTaskTemplateAppFormForCreatPlanCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new GetTaskTemplateAppFormForCreatPlanCustom();


                        dataRecord.Code = QueryUtils.getValueAsString(record, "CODE");
                        dataRecord.Description = QueryUtils.getValueAsString(record, "DESCRIPTION");
                        dataRecord.TaskType = QueryUtils.getValueAsString(record, "TASK_TYPE");
                        
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





        public async Task<EntitySearchResultBase<GetTaskSpecialForCreatPlanCustom>> getTaskSpecialForCreatPlan(SearchCriteriaBase<GetTaskSpecialForCreatPlanCriteria> searchCriteria, UserProfileForBack userProfile)
        {

            EntitySearchResultBase<GetTaskSpecialForCreatPlanCustom> searchResult = new EntitySearchResultBase<GetTaskSpecialForCreatPlanCustom>();
            List<GetTaskSpecialForCreatPlanCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();


                /*string territoryIdStr = "";
                if (userProfile.OrgTerritory != null && userProfile.OrgTerritory.data != null && userProfile.OrgTerritory.data.Count != 0)
                {
                    List<string> territoryIdList = new List<string>();
                    foreach (OrgTerritory t in userProfile.OrgTerritory.data)
                    {
                        territoryIdList.Add(t.TerritoryId.ToString());
                    }
                    territoryIdStr = String.Join(",", territoryIdList);
                }*/


                // Move New
                GetTaskSpecialForCreatPlanCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                string PROSPECT_TYPE_CUSTOMER = "2";
                queryBuilder.AppendFormat(" with M as ( ");
                queryBuilder.AppendFormat(" select MAX(TP.VISIT_CHECKIN_DTM)UPDATE_DATE ");
                queryBuilder.AppendFormat(" from PLAN_TRIP T ");
                queryBuilder.AppendFormat(" inner join PLAN_TRIP_PROSPECT TP on TP.PLAN_TRIP_ID = T.PLAN_TRIP_ID ");
                queryBuilder.AppendFormat(" inner join PLAN_TRIP_TASK TT on TT.PLAN_TRIP_PROSP_ID = TP.PLAN_TRIP_PROSP_ID ");
                queryBuilder.AppendFormat(" where TT.TASK_TYPE = 'M' and T.STOP_CHECKIN_LOC_ID is not null  ");
                queryBuilder.AppendFormat(" and TP.PROSP_ID = @PROSP_ID ");
                queryBuilder.AppendFormat(" ), S as ( ");
                queryBuilder.AppendFormat(" select TT.TP_STOCK_CARD_ID, MAX(TP.VISIT_CHECKIN_DTM) UPDATE_DATE ");
                queryBuilder.AppendFormat(" from PLAN_TRIP T ");
                queryBuilder.AppendFormat(" inner join PLAN_TRIP_PROSPECT TP on TP.PLAN_TRIP_ID = T.PLAN_TRIP_ID ");
                queryBuilder.AppendFormat(" inner join PLAN_TRIP_TASK TT on TT.PLAN_TRIP_PROSP_ID = TP.PLAN_TRIP_PROSP_ID ");
                queryBuilder.AppendFormat(" where TT.TASK_TYPE = 'S' and T.STOP_CHECKIN_LOC_ID is not null  ");
                queryBuilder.AppendFormat(" and TP.PROSP_ID = @PROSP_ID ");
                queryBuilder.AppendFormat(" group by TT.TP_STOCK_CARD_ID ");
                queryBuilder.AppendFormat(" ), PP as ( ");
                queryBuilder.AppendFormat(" select PROSPECT_TYPE from PROSPECT where PROSPECT_ID = @PROSP_ID ");
                queryBuilder.AppendFormat(" ) ");
                queryBuilder.AppendFormat(" select null CODE,'จดมิเตอร์' DESCRIPTION,'M' TASK_TYPE, M.UPDATE_DATE from M ");
                queryBuilder.AppendFormat(" inner join PP on PP.PROSPECT_TYPE = @PROSPECT_TYPE_CUSTOMER ");//PROSPECT_TYPE_CUSTOMER--FIX = 2
                queryBuilder.AppendFormat(" union ");
                queryBuilder.AppendFormat(" select TS.TP_STOCK_CARD_ID CODE,TS.TP_NAME_TH DESCRIPTION,'S' TASK_TYPE, S.UPDATE_DATE ");
                queryBuilder.AppendFormat(" from TEMPLATE_STOCK_CARD TS ");
                queryBuilder.AppendFormat(" inner join PP on PP.PROSPECT_TYPE = @PROSPECT_TYPE_CUSTOMER  AND TS.ACTIVE_FLAG = 'Y'  ");//PROSPECT_TYPE_CUSTOMER--FIX = 2
                //queryBuilder.AppendFormat(" inner join ADM_GROUP_USER GU on GU.EMP_ID = TS.CREATE_USER and GU.BU_ID = @buId ");
                queryBuilder.AppendFormat(" left join S on S.TP_STOCK_CARD_ID = TS.TP_STOCK_CARD_ID ");
                queryBuilder.AppendFormat(" WHERE exists(select 1 from TEMPLATE_STOCK_PRODUCT SCP where SCP.TP_STOCK_CARD_ID = TS.TP_STOCK_CARD_ID) "); 
                queryBuilder.AppendFormat(" union  ");
                queryBuilder.AppendFormat(" select TP_SA_FORM_ID CODE,TP_NAME_TH DESCRIPTION,'T' TASK_TYPE, NULL UPDATE_DATE ");
                queryBuilder.AppendFormat(" from TEMPLATE_SA_FORM SF ");
                queryBuilder.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = SF.CREATE_USER ");
                //queryBuilder.AppendFormat(" where exists (select 1 from ORG_SALE_TERRITORY OST where OST.EMP_ID = SF.CREATE_USER and OST.TERRITORY_ID IN (" + QueryUtils.getParamIn("territoryIdStr", territoryIdStr) + ")) ");
                queryBuilder.AppendFormat(" where exists (select 1 from ORG_SALE_GROUP OSG where OSG.GROUP_CODE = E.GROUP_CODE and OSG.TERRITORY_ID = @orgSaleGroup_territoryId) ");
                queryBuilder.AppendFormat(" and SF.ACTIVE_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" order by TASK_TYPE,DESCRIPTION ");
                QueryUtils.addParam(command, "PROSP_ID", o.ProspId);// Add new
                QueryUtils.addParam(command, "PROSPECT_TYPE_CUSTOMER", PROSPECT_TYPE_CUSTOMER);// Add new
                //QueryUtils.addParam(command, "buId",  userProfile.getBuId());// Add new
                QueryUtils.addParam(command, "orgSaleGroup_territoryId",  userProfile.getSaleGroupSaleOffice().TerritoryId);// Add new
                //QueryUtils.addParamIn(command, "territoryIdStr", territoryIdStr);


                // For Paging
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

                    List<GetTaskSpecialForCreatPlanCustom> dataRecordList = new List<GetTaskSpecialForCreatPlanCustom>();
                    GetTaskSpecialForCreatPlanCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new GetTaskSpecialForCreatPlanCustom();


                        dataRecord.Code = QueryUtils.getValueAsString(record, "CODE");
                        dataRecord.Description = QueryUtils.getValueAsString(record, "DESCRIPTION");
                        dataRecord.TaskType = QueryUtils.getValueAsString(record, "TASK_TYPE");
                        dataRecord.UpdateDate = QueryUtils.getValueAsDateTime(record, "UPDATE_DATE"); 

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



        public class GetTaskTemplateAppFormForRecordResult
        {
            public string ValRecAppFormId { get; set; }
            public EntitySearchResultBase<GetTaskTemplateAppFormForRecordCustom> Result { get; set; }
        }


        public async Task<GetTaskTemplateAppFormForRecordResult> getTaskTemplateAppFormForRecord(SearchCriteriaBase<GetTaskTemplateAppFormForRecordCriteria> searchCriteria, UserProfileForBack userProfile)
        {
            GetTaskTemplateAppFormForRecordCriteria criteria = searchCriteria.model;
            StringBuilder queryBuilder = new StringBuilder();
            List<GetTaskTemplateAppFormForRecordCustom> lst = new List<GetTaskTemplateAppFormForRecordCustom>();
            EntitySearchResultBase<GetTaskTemplateAppFormForRecordCustom> searchResult = new EntitySearchResultBase<GetTaskTemplateAppFormForRecordCustom>();
            searchResult.data = lst;
            string ValRecAppFormId = null;
            string ValTpAppFormId = null;
            GetTaskTemplateAppFormForRecordResult Result = new GetTaskTemplateAppFormForRecordResult();
            Result.Result = searchResult;

            GetTaskTemplateAppFormForRecordCustom getTaskTemplateAppFormForRecordCustom = new GetTaskTemplateAppFormForRecordCustom();
            List<MsAttachCategory> LovAttachCategory = new List<MsAttachCategory>();
            List<RecordAppFormFile> ListFile = new List<RecordAppFormFile>();
            List<ObjectForm> ObjForm = new List<ObjectForm>();
            List<AppForm> QuestionList = new List<AppForm>();

            getTaskTemplateAppFormForRecordCustom.LovAttachCategory = LovAttachCategory;
            getTaskTemplateAppFormForRecordCustom.ListFile = ListFile;
            getTaskTemplateAppFormForRecordCustom.ObjForm = ObjForm;
            lst.Add(getTaskTemplateAppFormForRecordCustom);


            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Start Query Scope 
                // Set ObjectReponse.lovAttachCategory
                queryBuilder.AppendFormat(" select * from MS_ATTACH_CATEGORY ");
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    MsAttachCategory msAttachCategory = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        msAttachCategory = new MsAttachCategory();

                        msAttachCategory.AttachCateId = QueryUtils.getValueAsDecimalRequired(record, "ATTACH_CATE_ID");
                        msAttachCategory.AttachCateCode = QueryUtils.getValueAsString(record, "ATTACH_CATE_CODE");
                        msAttachCategory.AttachCateNameTh = QueryUtils.getValueAsString(record, "ATTACH_CATE_NAME_TH");
                        msAttachCategory.AttachCateNameEn = QueryUtils.getValueAsString(record, "ATTACH_CATE_NAME_EN");
                        msAttachCategory.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        msAttachCategory.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        msAttachCategory.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                        msAttachCategory.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        msAttachCategory.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");

                        LovAttachCategory.Add(msAttachCategory);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 

                


                // Set VAL_REC_APP_FORM_ID,VAL_TP_APP_FORM_ID 

                queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select AF.REC_APP_FORM_ID,TT.TP_APP_FORM_ID ");
                queryBuilder.AppendFormat(" from PLAN_TRIP_TASK TT ");
                queryBuilder.AppendFormat(" left join RECORD_APP_FORM AF on AF.PLAN_TRIP_TASK_ID = TT.PLAN_TRIP_TASK_ID ");
                queryBuilder.AppendFormat(" where TT.PLAN_TRIP_TASK_ID = @PLAN_TRIP_TASK_ID_1 ");
                QueryUtils.addParam(command, "PLAN_TRIP_TASK_ID_1", criteria.PlanTripTaskId);// Add new

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                await using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        ValRecAppFormId = QueryUtils.getValueAsString(record, "REC_APP_FORM_ID");
                        ValTpAppFormId = QueryUtils.getValueAsString(record, "TP_APP_FORM_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 

                Result.ValRecAppFormId = ValRecAppFormId;
                if (!String.IsNullOrEmpty(ValRecAppFormId)){
                    return Result;
                }






                // Set saleTerritory.salesRep 

                queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select AF.*,TC.* ");
                queryBuilder.AppendFormat(" from TEMPLATE_APP_FORM AF  ");
                queryBuilder.AppendFormat(" inner join TEMPLATE_CATEGORY TC on TC.TP_CATE_ID = AF.TP_CATE_ID ");
                queryBuilder.AppendFormat(" where AF.TP_APP_FORM_ID = @TP_APP_FORM_ID_1 ");
                QueryUtils.addParam(command, "TP_APP_FORM_ID_1", ValTpAppFormId);// Add new

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                await using (var reader = command.ExecuteReader())
                {
                    ObjectForm objForm = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        objForm = new ObjectForm();
                        objForm.AppForm = QuestionList;
                        objForm.TemplateId = QueryUtils.getValueAsString(record, "TP_APP_FORM_ID");
                        objForm.TemplateCateId = QueryUtils.getValueAsString(record, "TP_CATE_ID");
                        objForm.TemplateCateName = QueryUtils.getValueAsString(record, "TP_CATE_NAME_TH");
                        objForm.TemplateName = QueryUtils.getValueAsString(record, "TP_NAME_TH");
                        ObjForm.Add(objForm);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 








                // Set resultQuestion 

                queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select AQ.*,TQ.* ");
                queryBuilder.AppendFormat(" from TEMPLATE_APP_QUESTION AQ ");
                queryBuilder.AppendFormat(" inner join TEMPLATE_QUESTION TQ on TQ.QUESTION_ID = AQ.QUESTION_ID ");
                queryBuilder.AppendFormat(" where AQ.TP_APP_FORM_ID = @TP_APP_FORM_ID_2 ");
                queryBuilder.AppendFormat(" order by AQ.ORDER_NO ");
                QueryUtils.addParam(command, "TP_APP_FORM_ID_2", ValTpAppFormId);// Add new

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                await using (var reader = command.ExecuteReader())
                {
                    AppForm question = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        question = new AppForm();
                        question.AnsVal = new List<AnsVal>();
                        question.QuestionId = QueryUtils.getValueAsString(record, "QUESTION_ID");
                        question.RequireFlag = QueryUtils.getValueAsString(record, "REQUIRE_FLAG");
                        question.PrerequistOrderNo = QueryUtils.getValueAsString(record, "PREREQUIST_ORDER_NO");
                        question.QuestionNm = QueryUtils.getValueAsString(record, "QUESTION_NAME_TH");
                        question.AnsType = QueryUtils.getValueAsString(record, "ANS_TYPE");
                        log.Debug("ANS_VALUES:"+ QueryUtils.getValueAsString(record, "ANS_VALUES"));
                        string AnsValues = QueryUtils.getValueAsString(record, "ANS_VALUES");
                        log.Debug("AnsValues:" +AnsValues);
                        AnsVal ansVal = null;
                        switch (question.AnsType)
                        {

                            case "2":
                                    foreach (String s in AnsValues.Split("|"))
                                    {
                                        ansVal = new AnsVal();
                                        ansVal.Ans = s;
                                        question.AnsVal.Add(ansVal);
                                    }
                                    break;
                            case "3":
                                foreach (String s in AnsValues.Split("|"))
                                    {
                                        ansVal = new AnsVal();
                                        ansVal.Ans = s;
                                        question.AnsVal.Add(ansVal);
                                     }
                                break;
                            case "4":
                                foreach (String s in AnsValues.Split("|"))
                                    {
                                        ansVal = new AnsVal();
                                        ansVal.Ans = s;
                                        question.AnsVal.Add(ansVal);
                                    }
                                break;
                            case "5":
                                foreach (String s in AnsValues.Split("|"))
                                    {
                                        ansVal = new AnsVal();
                                        ansVal.Ans = s;
                                        question.AnsVal.Add(ansVal);
                                    }
                                break;
                            default:
                                question.AnsVal.Add(new AnsVal());
                                break;
                        }


                        QuestionList.Add(question);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 



            }


            return Result;
        }





        public async Task<RecordAppForm> addRecordAppForm(AddRecordAppFormModel addRecordAppFormModel, UserProfileForBack userProfile)
        {
            

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Delete
                        int numberOfRowDelete = await DeleteForAddRecordAppForm(addRecordAppFormModel.planTripTaskId);
                        
                        
                        // Insert and Update
                        string VAL_APPFORM = JsonSerializer.Serialize(addRecordAppFormModel.objForm, typeof(ObjectForm), new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

                        var sqlParameters = new List<SqlParameter>();// Add New
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for RECORD_APP_FORM_SEQ", p);
                        var RecAppFormId = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO  RECORD_APP_FORM (REC_APP_FORM_ID, PLAN_TRIP_TASK_ID, TP_APP_FORM_ID, PROSP_ID, APP_FORM, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@REC_APP_FORM_ID ,@PLAN_TRIP_TASK_ID, @TP_APP_FORM_ID, @PROSP_ID, @APP_FORM, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("REC_APP_FORM_ID", RecAppFormId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_TASK_ID", addRecordAppFormModel.planTripTaskId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_APP_FORM_ID", addRecordAppFormModel.TpAppFormId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_ID", addRecordAppFormModel.ProspId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("APP_FORM", VAL_APPFORM));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("numberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("numberOfRowEffective:" + numberOfRowInserted);

                        string queryStr = "";
                        foreach (AppForm q in addRecordAppFormModel.objForm.AppForm)
                        {
                            if("6".Equals(q.AnsType) || "7".Equals(q.AnsType))
                            {
                                String valPhotoFlag = q.AnsType.Equals("6") ? "Y" : "N";
                                foreach (AnsVal ans in q.AnsVal)
                                {
                                    sqlParameters = new List<SqlParameter>();// Add New
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat("UPDATE RECORD_APP_FORM_FILE SET ATTACH_CATE_ID=@ATTACH_CATE_ID, REC_APP_FORM_ID=@REC_APP_FORM_ID, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE REC_APP_FORM_FILE_ID=@REC_APP_FORM_FILE_ID ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("ATTACH_CATE_ID", ans.valExt1));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("REC_APP_FORM_ID", RecAppFormId));// Add New
                                    //sqlParameters.Add(QueryUtils.addSqlParameter("PHOTO_FLAG", valPhotoFlag));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("REC_APP_FORM_FILE_ID", ans.Val));// Add New
                                    queryStr = queryBuilder.ToString();
                                    queryStr = QueryUtils.cutStringNull(queryStr);
                                    log.Debug("Query:" + queryStr);
                                    Console.WriteLine("Query:" + queryStr);
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                                    log.Debug("numberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("numberOfRowEffective:" + numberOfRowInserted);

                                }
                            }
                        }


                        /*{
                            //Logic Delete RECORD_APP_FORM_FILE ที่ ไม่ได้ถูกใช้แล้ว 
                            *Pending ไว้ก่อน
                        }*/


                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_APP_FORM SET USED_FLAG='Y', UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE ISNULL(USED_FLAG,'N') != 'Y' and TP_APP_FORM_ID=@TemplateId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TemplateId", addRecordAppFormModel.objForm.TemplateId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("numberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("numberOfRowEffective:" + numberOfRowInserted);



                        transaction.Commit();
                        RecordAppForm re = new RecordAppForm();
                        re.RecAppFormId = RecAppFormId;
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




        public async Task<int> DeleteForAddRecordAppForm(string planTripTaskId)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM RECORD_APP_FORM WHERE PLAN_TRIP_TASK_ID=@planTripTaskId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("planTripTaskId", planTripTaskId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("numberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("numberOfRowEffective:" + numberOfRowInserted);
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


        public async Task<EntitySearchResultBase<TemplateAppForm>> searchTemplateAppForm(SearchCriteriaBase<SearchTemplateAppFormCriteria> searchCriteria)
        {

            EntitySearchResultBase<TemplateAppForm> searchResult = new EntitySearchResultBase<TemplateAppForm>();
            List<TemplateAppForm> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchTemplateAppFormCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select TP_APP_FORM_ID, TP_NAME_TH ");
                queryBuilder.AppendFormat(" from TEMPLATE_APP_FORM ");
                queryBuilder.AppendFormat(" where used_flag = 'Y' and ACTIVE_FLAG = 'Y' ");

                // For Paging
                queryBuilder.AppendFormat(" order by TP_NAME_TH ");
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

                    List<TemplateAppForm> dataRecordList = new List<TemplateAppForm>();
                    TemplateAppForm dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new TemplateAppForm();

                        dataRecord.TpAppFormId = QueryUtils.getValueAsDecimalRequired(record, "TP_APP_FORM_ID");
                        dataRecord.TpNameTh = QueryUtils.getValueAsString(record, "TP_NAME_TH");

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
