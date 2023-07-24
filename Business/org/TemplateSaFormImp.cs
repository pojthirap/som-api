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
using System.Data.Common;
using System.Data;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.record;

namespace MyFirstAzureWebApp.Business.org
{
    //https://stackoverflow.com/questions/37672821/entity-framework-return-grouped-by-value-and-sum-of-some-columns-in-the-same-q/37673948
    //https://debuxing.com/entity-framework-group-by-example-c
    // group by groupby
    public class TemplateSaFormImp : ITemplateSaForm
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<SearchTemplateSaFormCustom>> searchTemplateSaForm(SearchCriteriaBase<TemplateSaFormCriteria> searchCriteria)
        {
            EntitySearchResultBase<SearchTemplateSaFormCustom> searchResult = new EntitySearchResultBase<SearchTemplateSaFormCustom>();
            List<SearchTemplateSaFormCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("  select F.ACTIVE_FLAG, F.TP_SA_FORM_ID, F.TP_CODE,F.TP_NAME_TH,FORMAT(F.UPDATE_DTM,'yyyyMMddHHmmss') F_UPDATE_DTM, sum(IIF(T.ANS_TYPE='L', 1, 0)) MASTER_TOTAL, sum(IIF(T.ANS_TYPE='V', 1, 0)) TITLE_TOTAL from TEMPLATE_SA_FORM F  left join TEMPLATE_SA_TITLE T on T.TP_SA_FORM_ID = F.TP_SA_FORM_ID  where 1=1 ");
                TemplateSaFormCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.TpNameTh))
                    {
                        queryBuilder.AppendFormat(" and TP_NAME_TH like @TpNameTh  ");
                        QueryUtils.addParamLike(command, "TpNameTh", o.TpNameTh);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.ActiveFlag))
                    {
                        queryBuilder.AppendFormat(" and F.ACTIVE_FLAG  = @ActiveFlag  ");
                        QueryUtils.addParam(command, "ActiveFlag", o.ActiveFlag);// Add new
                    }
                }
                queryBuilder.AppendFormat("  group by F.TP_SA_FORM_ID, F.TP_CODE,F.TP_NAME_TH, FORMAT(F.UPDATE_DTM,'yyyyMMddHHmmss'), F.ACTIVE_FLAG ");


                // For Paging
                if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY F_UPDATE_DTM DESC  ");
                }else if (2 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY F_UPDATE_DTM  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY F_UPDATE_DTM DESC  ");
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
                    lst = searchTemplateQuestionMapRow(reader);
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }

		
		
        private List<SearchTemplateSaFormCustom> searchTemplateQuestionMapRow(DbDataReader reader)
        {
            List<SearchTemplateSaFormCustom> lst = new List<SearchTemplateSaFormCustom>();
            SearchTemplateSaFormCustom o = null;
            while (reader.Read())
            {
                IDataRecord record = (IDataRecord)reader;
                o = new SearchTemplateSaFormCustom();
                    o.TpSaFormId = QueryUtils.getValueAsDecimalRequired(record, "TP_SA_FORM_ID");
                    o.TpCode = QueryUtils.getValueAsString(record, "TP_CODE");
                    o.TpNameTh = QueryUtils.getValueAsString(record, "TP_NAME_TH");
                    o.MasterTotal = QueryUtils.getValueAsDecimalRequired(record, "MASTER_TOTAL");
                    o.TitleTotal = QueryUtils.getValueAsDecimalRequired(record, "TITLE_TOTAL");
                    o.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                    //o.UpdateDtm = QueryUtils.getValueAsDateTime(record, "F_UPDATE_DTM"); 

                lst.Add(o);
            }

            // Call Close when done reading.
            reader.Close();

            return lst;

        }




        public async Task<TemplateSaForm> Add(TemplateSaFormModel templateSaFormModel)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for TEMPLATE_SA_FORM_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  TEMPLATE_SA_FORM (TP_SA_FORM_ID, TP_CODE, TP_NAME_TH, TP_NAME_EN, USED_FLAG, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, RIGHT('00000' + CAST(ISNULL((MAX(CAST(TP_CODE AS INT)) + 1),1) AS VARCHAR), 5), @TpNameTh, @TpNameEn, @UsedFlag, 'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from TEMPLATE_SA_FORM ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpNameTh", templateSaFormModel.TpNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpNameEn", templateSaFormModel.TpNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UsedFlag", templateSaFormModel.UsedFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", templateSaFormModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        TemplateSaForm re = new TemplateSaForm();
                        re.TpSaFormId = nextVal;
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

        public async Task<int> Update(TemplateSaFormModel templateSaFormModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_SA_FORM SET TP_NAME_TH=@TpNameTh, TP_NAME_EN=@TpNameEn, USED_FLAG=@UsedFlag, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TP_SA_FORM_ID=@TpSaFormId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpNameTh", templateSaFormModel.TpNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpNameEn", templateSaFormModel.TpNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UsedFlag", templateSaFormModel.UsedFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", templateSaFormModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", templateSaFormModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpSaFormId", templateSaFormModel.TpSaFormId));// Add New
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


        public async Task<int> DeleteUpdate(TemplateSaFormModel templateSaFormModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_SA_FORM SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TP_SA_FORM_ID=@TP_SA_FORM_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", templateSaFormModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_SA_FORM_ID", templateSaFormModel.TpSaFormId));// Add New
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





        //================================



        public class GetTaskTemplateSaFormForRecordResult
        {
            public string ValRecSaFormId { get; set; }
            public EntitySearchResultBase<GetTaskTemplateSaFormForRecordCustom> Result { get; set; }
        }


        public async Task<GetTaskTemplateSaFormForRecordResult> getTaskTemplateSaFormForRecord(SearchCriteriaBase<GetTaskTemplateSaFormForRecordCriteria> searchCriteria, UserProfileForBack userProfile)
        {
            GetTaskTemplateSaFormForRecordCriteria criteria = searchCriteria.model;
            StringBuilder queryBuilder = new StringBuilder();
            List<GetTaskTemplateSaFormForRecordCustom> lst = new List<GetTaskTemplateSaFormForRecordCustom>();
            EntitySearchResultBase<GetTaskTemplateSaFormForRecordCustom> searchResult = new EntitySearchResultBase<GetTaskTemplateSaFormForRecordCustom>();
            searchResult.data = lst;
            string ValRecSaFormId = null;
            string ValTpSaFormId = null;
            GetTaskTemplateSaFormForRecordResult Result = new GetTaskTemplateSaFormForRecordResult();
            Result.Result = searchResult;

            GetTaskTemplateSaFormForRecordCustom GetTaskTemplateSaFormForRecordCustom = new GetTaskTemplateSaFormForRecordCustom();
            TemplateSaForm Form = null;
            List<RecordSaFormFile> ListFile = new List<RecordSaFormFile>();
            List<TemplateSaTitle> Title = new List<TemplateSaTitle>();




            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();




                // Set VAL_REC_SA_FORM_ID, VAL_TP_SA_FORM_ID

                queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select SF.REC_SA_FORM_ID,TT.TP_SA_FORM_ID ");
                queryBuilder.AppendFormat(" from PLAN_TRIP_TASK TT ");
                queryBuilder.AppendFormat(" left join RECORD_SA_FORM SF on SF.PLAN_TRIP_TASK_ID = TT.PLAN_TRIP_TASK_ID ");
                queryBuilder.AppendFormat(" where TT.PLAN_TRIP_TASK_ID = @PlanTripTaskId ");
                QueryUtils.addParam(command, "PlanTripTaskId", criteria.PlanTripTaskId);// Add new

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                await using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        ValRecSaFormId = QueryUtils.getValueAsString(record, "REC_SA_FORM_ID");
                        ValTpSaFormId = QueryUtils.getValueAsString(record, "TP_SA_FORM_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 

                Result.ValRecSaFormId = ValRecSaFormId;
                if (!String.IsNullOrEmpty(ValRecSaFormId))
                {
                    return Result;
                }




                // Set ReponseData.form
                queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select SF.* ");
                queryBuilder.AppendFormat(" from TEMPLATE_SA_FORM SF ");
                queryBuilder.AppendFormat(" where SF.TP_SA_FORM_ID=@TP_SA_FORM_ID_1 ");
                QueryUtils.addParam(command, "TP_SA_FORM_ID_1", ValTpSaFormId);// Add new

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                await using (var reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        Form = new TemplateSaForm();
                        Form.TpSaFormId = QueryUtils.getValueAsDecimalRequired(record, "TP_SA_FORM_ID");
                        Form.TpCode = QueryUtils.getValueAsString(record, "TP_CODE");
                        Form.TpNameTh = QueryUtils.getValueAsString(record, "TP_NAME_TH");
                        Form.TpNameEn = QueryUtils.getValueAsString(record, "TP_NAME_EN");
                        Form.UsedFlag = QueryUtils.getValueAsString(record, "USED_FLAG");
                        Form.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        Form.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        Form.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        Form.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        Form.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");
                        Form.UsedDtm = QueryUtils.getValueAsDateTime(record, "USED_DTM");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 





                // Set saleTerritory.salesRep 

                queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select ST.* ");
                queryBuilder.AppendFormat(" from TEMPLATE_SA_TITLE ST  ");
                queryBuilder.AppendFormat(" where ST.TP_SA_FORM_ID = @TP_SA_FORM_ID_2 ");
                queryBuilder.AppendFormat(" order by ST.TITLE_COLM_NO  ");
                QueryUtils.addParam(command, "TP_SA_FORM_ID_2", ValTpSaFormId);// Add new

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                await using (var reader = command.ExecuteReader())
                {
                    TemplateSaTitle title = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        title = new TemplateSaTitle();

                        title.TpSaTitleId = QueryUtils.getValueAsDecimalRequired(record, "TP_SA_TITLE_ID");
                        title.TpSaFormId = QueryUtils.getValueAsDecimal(record, "TP_SA_FORM_ID");
                        title.TitleColmNo = QueryUtils.getValueAsDecimal(record, "TITLE_COLM_NO");
                        title.TitleNameTh = QueryUtils.getValueAsString(record, "TITLE_NAME_TH");
                        title.TitleNameEn = QueryUtils.getValueAsString(record, "TITLE_NAME_EN");
                        title.AnsType = QueryUtils.getValueAsString(record, "ANS_TYPE");
                        title.AnsValType = QueryUtils.getValueAsDecimal(record, "ANS_VAL_TYPE");
                        title.AnsLovType = QueryUtils.getValueAsDecimal(record, "ANS_LOV_TYPE");
                        title.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        title.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        title.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        title.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");
                        Title.Add(title);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 

            }
            GetTaskTemplateSaFormForRecordCustom.Form = Form;
            GetTaskTemplateSaFormForRecordCustom.ListFile = ListFile;
            GetTaskTemplateSaFormForRecordCustom.Title = Title;
            lst.Add(GetTaskTemplateSaFormForRecordCustom);

            return Result;
        }






        public async Task<RecordSaForm> addRecordSaForm(AddRecordSaFormModel addRecordSaFormModel, UserProfileForBack userProfile)
        {


            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Delete
                        //int numberOfRowDelete = await DeleteForAddRecordAppForm(addRecordAppFormModel.planTripTaskId);
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM RECORD_SA_FORM WHERE PLAN_TRIP_TASK_ID=@planTripTaskId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("planTripTaskId", addRecordSaFormModel.PlanTripTaskId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        List<string> listFile = new List<string>();
                        string[] titleColmNo = new string[60];

                        foreach(TemplateSaTitle title in addRecordSaFormModel.ListTitle)
                        {
                            titleColmNo[(int)title.TitleColmNo] = title.titleColmAns;
                            if (title.AnsValType == 4)
                            {
                                if (title.titleColmAns.Contains(","))
                                {
                                    string[] fileId_ = title.titleColmAns.Split(",");
                                    foreach(string fid in fileId_)
                                    {
                                        listFile.Add(fid);
                                    }
                                }
                                else
                                {
                                    listFile.Add(title.titleColmAns);
                                }
                            }
                        }


                        sqlParameters = new List<SqlParameter>();// Add New
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for RECORD_SA_FORM_SEQ", p);
                        var RecSaFormId = (int)p.Value;

                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO RECORD_SA_FORM ([REC_SA_FORM_ID], [PLAN_TRIP_TASK_ID], [TP_SA_FORM_ID], [PROSP_ID], [TITLE_COLM_NO1], [TITLE_COLM_NO2], [TITLE_COLM_NO3], [TITLE_COLM_NO4], [TITLE_COLM_NO5], [TITLE_COLM_NO6], [TITLE_COLM_NO7], [TITLE_COLM_NO8], [TITLE_COLM_NO9], [TITLE_COLM_NO10], [TITLE_COLM_NO11], [TITLE_COLM_NO12], [TITLE_COLM_NO13], [TITLE_COLM_NO14], [TITLE_COLM_NO15], [TITLE_COLM_NO16], [TITLE_COLM_NO17], [TITLE_COLM_NO18], [TITLE_COLM_NO19], [TITLE_COLM_NO20], [TITLE_COLM_NO21], [TITLE_COLM_NO22], [TITLE_COLM_NO23], [TITLE_COLM_NO24], [TITLE_COLM_NO25], [TITLE_COLM_NO26], [TITLE_COLM_NO27], [TITLE_COLM_NO28], [TITLE_COLM_NO29], [TITLE_COLM_NO30], [TITLE_COLM_NO31], [TITLE_COLM_NO32], [TITLE_COLM_NO33], [TITLE_COLM_NO34], [TITLE_COLM_NO35], [TITLE_COLM_NO36], [TITLE_COLM_NO37], [TITLE_COLM_NO38], [TITLE_COLM_NO39], [TITLE_COLM_NO40], [TITLE_COLM_NO41], [TITLE_COLM_NO42], [TITLE_COLM_NO43], [TITLE_COLM_NO44], [TITLE_COLM_NO45], [TITLE_COLM_NO46], [TITLE_COLM_NO47], [TITLE_COLM_NO48], [TITLE_COLM_NO49], [TITLE_COLM_NO50], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                        queryBuilder.AppendFormat(" VALUES(@VAL_RECORD_SA_FORM, @PLAN_TRIP_TASK_ID, @TP_SA_FORM_ID, @PROSP_ID, @TITLE_COLM_NO1, @TITLE_COLM_NO2, @TITLE_COLM_NO3, @TITLE_COLM_NO4, @TITLE_COLM_NO5, @TITLE_COLM_NO6, @TITLE_COLM_NO7, @TITLE_COLM_NO8, @TITLE_COLM_NO9, @TITLE_COLM_NO10, @TITLE_COLM_NO11, @TITLE_COLM_NO12, @TITLE_COLM_NO13, @TITLE_COLM_NO14, @TITLE_COLM_NO15, @TITLE_COLM_NO16, @TITLE_COLM_NO17, @TITLE_COLM_NO18, @TITLE_COLM_NO19, @TITLE_COLM_NO20, @TITLE_COLM_NO21, @TITLE_COLM_NO22, @TITLE_COLM_NO23, @TITLE_COLM_NO24, @TITLE_COLM_NO25, @TITLE_COLM_NO26, @TITLE_COLM_NO27, @TITLE_COLM_NO28, @TITLE_COLM_NO29, @TITLE_COLM_NO30, @TITLE_COLM_NO31, @TITLE_COLM_NO32, @TITLE_COLM_NO33, @TITLE_COLM_NO34, @TITLE_COLM_NO35, @TITLE_COLM_NO36, @TITLE_COLM_NO37, @TITLE_COLM_NO38, @TITLE_COLM_NO39, @TITLE_COLM_NO40, @TITLE_COLM_NO41, @TITLE_COLM_NO42, @TITLE_COLM_NO43, @TITLE_COLM_NO44, @TITLE_COLM_NO45, @TITLE_COLM_NO46, @TITLE_COLM_NO47, @TITLE_COLM_NO48, @TITLE_COLM_NO49, @TITLE_COLM_NO50, @user, dbo.GET_SYSDATETIME(), @user, dbo.GET_SYSDATETIME()) ");

                        sqlParameters.Add(QueryUtils.addSqlParameter("VAL_RECORD_SA_FORM", RecSaFormId));
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_TASK_ID", addRecordSaFormModel.PlanTripTaskId));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_SA_FORM_ID", addRecordSaFormModel.TpSaFormId));
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_ID", addRecordSaFormModel.ProspId));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO1", titleColmNo[1]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO2", titleColmNo[2]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO3", titleColmNo[3]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO4", titleColmNo[4]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO5", titleColmNo[5]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO6", titleColmNo[6]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO7", titleColmNo[7]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO8", titleColmNo[8]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO9", titleColmNo[9]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO10", titleColmNo[10]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO11", titleColmNo[11]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO12", titleColmNo[12]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO13", titleColmNo[13]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO14", titleColmNo[14]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO15", titleColmNo[15]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO16", titleColmNo[16]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO17", titleColmNo[17]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO18", titleColmNo[18]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO19", titleColmNo[19]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO20", titleColmNo[20]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO21", titleColmNo[21]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO22", titleColmNo[22]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO23", titleColmNo[23]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO24", titleColmNo[24]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO25", titleColmNo[25]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO26", titleColmNo[26]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO27", titleColmNo[27]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO28", titleColmNo[28]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO29", titleColmNo[29]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO30", titleColmNo[30]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO31", titleColmNo[31]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO32", titleColmNo[32]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO33", titleColmNo[33]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO34", titleColmNo[34]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO35", titleColmNo[35]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO36", titleColmNo[36]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO37", titleColmNo[37]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO38", titleColmNo[38]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO39", titleColmNo[39]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO40", titleColmNo[40]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO41", titleColmNo[41]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO42", titleColmNo[42]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO43", titleColmNo[43]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO44", titleColmNo[44]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO45", titleColmNo[45]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO46", titleColmNo[46]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO47", titleColmNo[47]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO48", titleColmNo[48]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO49", titleColmNo[49]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("TITLE_COLM_NO50", titleColmNo[50]));
                        sqlParameters.Add(QueryUtils.addSqlParameter("user", userProfile.getUserName()));
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        queryStr = "";
                        foreach (string fileId in listFile)
                        {
                                    sqlParameters = new List<SqlParameter>();
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat("UPDATE RECORD_SA_FORM_FILE SET REC_SA_FORM_ID=@REC_SA_FORM_ID, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE REC_SA_FORM_FILE_ID=@fileId ");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("REC_SA_FORM_ID", RecSaFormId)); 
                                    sqlParameters.Add(QueryUtils.addSqlParameter("fileId", fileId));
                                    sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));
                                    queryStr = queryBuilder.ToString();
                                    queryStr = QueryUtils.cutStringNull(queryStr);
                                    log.Debug("Query:" + queryStr);
                                    Console.WriteLine("Query:" + queryStr);
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        }

                        /*{
                            //Logic Delete RECORD_SA_FORM_FILEที่ ไม่ได้ถูกใช้แล้ว 
                            *Pending ไว้ก่อน
                        }*/


                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_SA_FORM SET USED_FLAG='Y', UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE ISNULL(USED_FLAG,'N') != 'Y' and TP_SA_FORM_ID=@TpSaFormId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpSaFormId", addRecordSaFormModel.Form.TpSaFormId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);



                        transaction.Commit();
                        RecordSaForm re = new RecordSaForm();
                        re.RecSaFormId = RecSaFormId;
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



        public async Task<EntitySearchResultBase<TemplateSaForm>> searchTemplateSa(SearchCriteriaBase<SearchTemplateSaCriteria> searchCriteria)
        {

            EntitySearchResultBase<TemplateSaForm> searchResult = new EntitySearchResultBase<TemplateSaForm>();
            List<TemplateSaForm> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchTemplateSaCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select * ");
                queryBuilder.AppendFormat(" from TEMPLATE_SA_FORM ");
                queryBuilder.AppendFormat(" where ACTIVE_FLAG = 'Y' ");

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

                    List<TemplateSaForm> dataRecordList = new List<TemplateSaForm>();
                    TemplateSaForm dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new TemplateSaForm();


                        dataRecord.TpSaFormId = QueryUtils.getValueAsDecimalRequired(record, "TP_SA_FORM_ID");
                        dataRecord.TpCode = QueryUtils.getValueAsString(record, "TP_CODE");
                        dataRecord.TpNameTh = QueryUtils.getValueAsString(record, "TP_NAME_TH");
                        dataRecord.TpNameEn = QueryUtils.getValueAsString(record, "TP_NAME_EN");
                        dataRecord.UsedFlag = QueryUtils.getValueAsString(record, "USED_FLAG");
                        dataRecord.UsedDtm = QueryUtils.getValueAsDateTime(record, "USED_DTM");

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







    }
}
