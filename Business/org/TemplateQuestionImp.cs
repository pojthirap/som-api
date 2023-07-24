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
using MyFirstAzureWebApp.Entity.custom;
using System.Data;
using System.Data.Common;
using MyFirstAzureWebApp.Models.profile;

namespace MyFirstAzureWebApp.Business.org
{

    public class TemplateQuestionImp : ITemplateQuestion
    {
        private Logger log = LogManager.GetCurrentClassLogger();



        public async Task<EntitySearchResultBase<SearchTemplateQuestionCustom>> Search(SearchCriteriaBase<TemplateQuestionCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                TemplateQuestionCriteria criteria = searchCriteria.model;
                var queryCommane = (from q in context.TemplateQuestion
                                    join cf in context.MsConfigLov 
                                    on  q.AnsType equals cf.LovKeyvalue
                                    where(cf.LovKeyword == "QUESTION_ANS_TYPE")
                                    where ((criteria.ActiveFlag == null ? 1 == 1 : q.ActiveFlag == criteria.ActiveFlag))
                                    where ((criteria.QuestionNameTh == null ? 1 == 1 : q.QuestionNameTh == criteria.QuestionNameTh))
                                    orderby (searchCriteria.searchOrder == 0 ? q.QuestionNameTh : q.QuestionNameTh)
                                    select new
                                    {
                                        QuestionId = q.QuestionId,
                                        QuestionCode = q.QuestionCode,
                                        QuestionNameTh = q.QuestionNameTh,
                                        QuestionNameEn = q.QuestionNameEn,
                                        AnsType = q.AnsType,
                                        AnsValues = q.AnsValues,
                                        PublicFlag = q.PublicFlag,
                                        ActiveFlag = q.ActiveFlag,
                                        CreateUser = q.CreateUser,
                                        CreateDtm = q.CreateDtm,
                                        UpdateUser = q.UpdateUser,
                                        UpdateDtm = q.UpdateDtm,

                                        LovKeyword = cf.LovKeyword,
                                        LovKeyvalue = cf.LovKeyvalue,
                                        LovNameTh = cf.LovNameTh,
                                        LovNameEn = cf.LovNameEn,
                                        LovCodeTh = cf.LovCodeTh,
                                        LovCodeEn = cf.LovCodeEn,
                                        LovOrder = cf.LovOrder,
                                        LovRemark = cf.LovRemark
                                    });

                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<SearchTemplateQuestionCustom> searchResult = new EntitySearchResultBase<SearchTemplateQuestionCustom>();
                searchResult.totalRecords = query.Count();



                List<SearchTemplateQuestionCustom> saleLst = new List<SearchTemplateQuestionCustom>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    SearchTemplateQuestionCustom s = new SearchTemplateQuestionCustom();
                    s.QuestionId = item.QuestionId;
                    s.QuestionCode = item.QuestionCode;
                    s.QuestionNameTh = item.QuestionNameTh;
                    s.QuestionNameEn = item.QuestionNameEn;
                    s.AnsType = item.AnsType;
                    s.AnsValues = item.AnsValues;
                    s.PublicFlag = item.PublicFlag;
                    s.ActiveFlag = item.ActiveFlag;
                    s.CreateUser = item.CreateUser;
                    s.CreateDtm = item.CreateDtm;
                    s.UpdateUser = item.UpdateUser;
                    s.UpdateDtm = item.UpdateDtm;
                    s.LovKeyword = item.LovKeyword;
                    s.LovKeyvalue = item.LovKeyvalue;
                    s.LovNameTh = item.LovNameTh;
                    s.LovNameEn = item.LovNameEn;
                    s.LovCodeTh = item.LovCodeTh;
                    s.LovCodeEn = item.LovCodeEn;
                    s.LovOrder = item.LovOrder;
                    s.LovRemark = item.LovRemark;
                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;
            }

        }
        

        public async Task<TemplateQuestion> Add(TemplateQuestionModel templateQuestionModel)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for TEMPLATE_QUESTION_SEQ", p);
                        var nextVal = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO  TEMPLATE_QUESTION (QUESTION_ID, QUESTION_CODE, QUESTION_NAME_TH, QUESTION_NAME_EN, ANS_TYPE, ANS_VALUES, PUBLIC_FLAG, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal, RIGHT('00000' + CAST(ISNULL((MAX(CAST(QUESTION_CODE AS INT)) + 1),1) AS VARCHAR), 5), @QuestionNameTh, @QuestionNameEn, @AnsType, @AnsValues, @PublicFlag,'Y', @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() from TEMPLATE_QUESTION ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("QuestionNameTh", templateQuestionModel.QuestionNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("QuestionNameEn", templateQuestionModel.QuestionNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AnsType", templateQuestionModel.AnsType));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AnsValues", templateQuestionModel.AnsValues));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PublicFlag", templateQuestionModel.PublicFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", templateQuestionModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        TemplateQuestion re = new TemplateQuestion();
                        re.QuestionId = nextVal;
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
        

        public async Task<int> Update(TemplateQuestionModel templateQuestionModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_QUESTION SET QUESTION_NAME_TH = @QuestionNameTh, QUESTION_NAME_EN=@QuestionNameEn, ANS_TYPE=@AnsType, ANS_VALUES=@AnsValues, PUBLIC_FLAG=@PublicFlag, ACTIVE_FLAG=@ActiveFlag, UPDATE_USER=@User, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE QUESTION_ID=@QuestionId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("QuestionNameTh", templateQuestionModel.QuestionNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("QuestionNameEn", templateQuestionModel.QuestionNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AnsType", templateQuestionModel.AnsType));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AnsValues", templateQuestionModel.AnsValues));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PublicFlag", templateQuestionModel.PublicFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", templateQuestionModel.ActiveFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", templateQuestionModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("QuestionId", templateQuestionModel.QuestionId));// Add New
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

        public async Task<int> DeleteUpdate(TemplateQuestionModel templateQuestionModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_QUESTION SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE QUESTION_ID=@QUESTION_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", templateQuestionModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("QUESTION_ID", templateQuestionModel.QuestionId));// Add New
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

        private List<SearchTemplateQuestionCustom> searchTemplateQuestionMapRow(DbDataReader reader)
        {
            List<SearchTemplateQuestionCustom> lst = new List<SearchTemplateQuestionCustom>();
            SearchTemplateQuestionCustom o = null;
            while (reader.Read())
            {
                IDataRecord record = (IDataRecord)reader;
                o = new SearchTemplateQuestionCustom();
                    o.EditFlag = QueryUtils.getValueAsString(record, "EDIT_FLAG");
                    o.QuestionId = QueryUtils.getValueAsDecimal(record, "QUESTION_ID");
                    o.QuestionCode = QueryUtils.getValueAsString(record, "QUESTION_CODE");
                    o.QuestionNameTh = QueryUtils.getValueAsString(record, "QUESTION_NAME_TH");
                    o.QuestionNameEn = QueryUtils.getValueAsString(record, "QUESTION_NAME_EN");
                    o.AnsType = QueryUtils.getValueAsDecimal(record, "ANS_TYPE");
                    o.AnsValues = QueryUtils.getValueAsString(record, "ANS_VALUES");
                    o.PublicFlag = QueryUtils.getValueAsString(record, "PUBLIC_FLAG");
                    o.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                    o.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                    o.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                    o.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                    o.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");
                    o.LovKeyword = QueryUtils.getValueAsString(record, "LOV_KEYWORD");
                    o.LovKeyvalue = QueryUtils.getValueAsDecimal(record, "LOV_KEYVALUE");
                    o.LovNameTh = QueryUtils.getValueAsString(record, "LOV_NAME_TH");
                    o.LovNameEn = QueryUtils.getValueAsString(record, "LOV_NAME_EN");
                    o.LovCodeTh = QueryUtils.getValueAsString(record, "LOV_CODE_TH");
                    o.LovCodeEn = QueryUtils.getValueAsString(record, "LOV_CODE_EN");
                    o.LovOrder = QueryUtils.getValueAsDecimal(record, "LOV_ORDER");
                    o.LovRemark = QueryUtils.getValueAsString(record, "LOV_REMARK");


                lst.Add(o);
            }

            // Call Close when done reading.
            reader.Close();

            return lst;
            
        }

        public async Task<EntitySearchResultBase<SearchTemplateQuestionCustom>> searchTemplateQuestion(SearchCriteriaBase<TemplateQuestionCriteria> searchCriteria, UserProfileForBack userProfileForBack)
        {

            EntitySearchResultBase<SearchTemplateQuestionCustom> searchResult = new EntitySearchResultBase<SearchTemplateQuestionCustom>();
            List<SearchTemplateQuestionCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("   select IIF(T.QUESTION_ID is null and Q.CREATE_USER = @User, 'Y', 'N') EDIT_FLAG, Q.*,CF.* from dbo.TEMPLATE_QUESTION Q inner join ms_config_lov CF on CF.LOV_KEYVALUE = Q.ANS_TYPE left join ( select distinct AQ.QUESTION_ID from TEMPLATE_APP_FORM AF inner join TEMPLATE_APP_QUESTION AQ ON AQ.TP_APP_FORM_ID = AF.TP_APP_FORM_ID where AF.USED_FLAG = 'Y') T on T.QUESTION_ID = Q.QUESTION_ID where CF.LOV_KEYWORD = 'QUESTION_ANS_TYPE' ");
                QueryUtils.addParam(command, "User", userProfileForBack.getUserName());// Add new
                TemplateQuestionCriteria o = searchCriteria.model;
                o.empId = userProfileForBack.UserProfileCustom.data[0].EmpId;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.QuestionNameTh))
                    {
                        queryBuilder.AppendFormat(" and Q.QUESTION_NAME_TH like @QuestionNameTh  ");
                        QueryUtils.addParamLike(command, "QuestionNameTh", o.QuestionNameTh);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.QuestionCode))
                    {
                        queryBuilder.AppendFormat(" and Q.QUESTION_CODE  = @QuestionCode  ");
                        QueryUtils.addParam(command, "QuestionCode", o.QuestionCode);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.ActiveFlag))
                    {
                        queryBuilder.AppendFormat(" and Q.ACTIVE_FLAG  = @ActiveFlag  ");
                        QueryUtils.addParam(command, "ActiveFlag", o.ActiveFlag);// Add new
                    }
                    if ("N".Equals(o.PublicFlag) && !String.IsNullOrEmpty(o.empId))
                    {
                        queryBuilder.AppendFormat("   and Q.CREATE_USER = @empId ");
                        QueryUtils.addParam(command, "empId", o.empId);// Add new
                    }
                    else
                    {
                        queryBuilder.AppendFormat("  and (Q.CREATE_USER = @empId OR Q.public_flag = 'Y') ");
                        QueryUtils.addParam(command, "empId", o.empId);// Add new
                    }

                    if (searchCriteria.searchOption == 1 && !String.IsNullOrEmpty(o.TpAppFormId))
                    {
                        queryBuilder.AppendFormat(" and NOT EXISTS ( select 1  from TEMPLATE_APP_QUESTION AP  where AP.QUESTION_ID=Q.QUESTION_ID  and AP.TP_APP_FORM_ID = @TpAppFormId )  ");
                        QueryUtils.addParam(command, "TpAppFormId", o.TpAppFormId);// Add new
                    }
                }


                // For Paging
                if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY Q.UPDATE_DTM DESC  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY QUESTION_ID  ");
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

        




    }
}
