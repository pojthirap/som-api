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
using MyFirstAzureWebApp.Models.profile;

namespace MyFirstAzureWebApp.Business.org
{

    public class TemplateAppQuestionImp : ITemplateAppQuestion
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<TemplateAppQuestion>> Search(SearchCriteriaBase<TemplateAppQuestionCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from TEMPLATE_APP_QUESTION where 1=1 ");
                TemplateAppQuestionCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.TpAppQuestionId))
                    {
                        queryBuilder.AppendFormat(" and TP_APP_QUESTION_ID = @TpAppQuestionId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpAppQuestionId", o.TpAppQuestionId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.TpAppFormId))
                    {
                        queryBuilder.AppendFormat(" and TP_APP_FORM_ID = @TpAppFormId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpAppFormId", o.TpAppFormId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.QuestionId))
                    {
                        queryBuilder.AppendFormat(" and QUESTION_ID = @QuestionId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("QuestionId", o.QuestionId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.PrerequistOrderNo))
                    {
                        queryBuilder.AppendFormat(" and PREREQUIST_ORDER_NO = @PrerequistOrderNo ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PrerequistOrderNo", o.PrerequistOrderNo));// Add New
                    }
                }
                // For Paging
                queryBuilder.AppendFormat(" ORDER BY ORDER_NO  ");
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.TemplateAppQuestion.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<TemplateAppQuestion> lst = context.TemplateAppQuestion.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<TemplateAppQuestion> searchResult = new EntitySearchResultBase<TemplateAppQuestion>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }
        
        public async Task<List<TemplateAppQuestion>> Add(List<TemplateAppQuestionModel> templateAppQuestionModel, UserProfileForBack userProfileForBack)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        List<TemplateAppQuestion> list = new List<TemplateAppQuestion>();
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" UPDATE TEMPLATE_APP_FORM SET [UPDATE_USER] = @UPDATE_USER, [UPDATE_DTM] = dbo.GET_SYSDATETIME() WHERE TP_APP_FORM_ID = @TP_APP_FORM_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", userProfileForBack.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_APP_FORM_ID", templateAppQuestionModel[0].TpAppFormId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM TEMPLATE_APP_QUESTION  WHERE TP_APP_FORM_ID=@TP_APP_FORM_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_APP_FORM_ID", templateAppQuestionModel[0].TpAppFormId));// Add New
                        queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        for (int i = 0; i < templateAppQuestionModel.Count; i++)
                        {
                            TemplateAppQuestionModel model = templateAppQuestionModel[i];

                            var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                            p.Direction = System.Data.ParameterDirection.Output;
                            context.Database.ExecuteSqlRaw("set @result = next value for TEMPLATE_APP_QUESTION_SEQ", p);
                            var nextVal = (int)p.Value;

                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat("INSERT INTO  TEMPLATE_APP_QUESTION (TP_APP_QUESTION_ID, TP_APP_FORM_ID, QUESTION_ID, ORDER_NO, PREREQUIST_ORDER_NO, REQUIRE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                            queryBuilder.AppendFormat("VALUES(@TP_APP_QUESTION_ID ,@TP_APP_FORM_ID, @QUESTION_ID, @ORDER_NO, @PREREQUIST_ORDER_NO, @REQUIRE_FLAG, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                            sqlParameters.Add(QueryUtils.addSqlParameter("TP_APP_QUESTION_ID", nextVal));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("TP_APP_FORM_ID", model.TpAppFormId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("QUESTION_ID", model.QuestionId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_NO", model.OrderNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PREREQUIST_ORDER_NO", model.PrerequistOrderNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("REQUIRE_FLAG", model.RequireFlag));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfileForBack.getUserName()));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                            TemplateAppQuestion re = new TemplateAppQuestion();
                            re.TpAppQuestionId = nextVal;
                            list.Add(re);
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

        public async Task<int> Update(TemplateAppQuestionModel templateAppQuestionModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_APP_QUESTION SET ORDER_NO= @ORDER_NO, PREREQUIST_ORDER_NO = @PREREQUIST_ORDER_NO, REQUIRE_FLAG=@REQUIRE_FLAG, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TP_APP_QUESTION_ID=@TP_APP_QUESTION_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_NO", templateAppQuestionModel.OrderNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PREREQUIST_ORDER_NO", templateAppQuestionModel.PrerequistOrderNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("REQUIRE_FLAG", templateAppQuestionModel.RequireFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", templateAppQuestionModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_APP_QUESTION_ID", templateAppQuestionModel.TpAppQuestionId));// Add New
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


        public async Task<int> DeleteUpdate(TemplateAppQuestionModel templateAppQuestionModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_APP_QUESTION SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TP_APP_QUESTION_ID=@TP_APP_QUESTION_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", templateAppQuestionModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_APP_QUESTION_ID", templateAppQuestionModel.TpAppQuestionId));// Add New
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




        public async Task<int> DeleteByTemplateAppFormId(TemplateAppQuestionModel model)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM TEMPLATE_APP_QUESTION  WHERE TP_APP_FORM_ID=@TP_APP_FORM_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_APP_FORM_ID", model.TpAppFormId));// Add New
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





        public async Task<EntitySearchResultBase<SearchTemplateAppQuestionByIdCustom>> searchTemplateAppQuestionById(SearchCriteriaBase<TemplateAppQuestionCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                TemplateAppQuestionCriteria criteria = searchCriteria.model;
                decimal TpAppFormId_cri = Convert.ToDecimal(criteria.TpAppFormId);
                var queryCommane = (from aq in context.TemplateAppQuestion
                                    join q in context.TemplateQuestion
                                    on aq.QuestionId equals q.QuestionId

                                    where ((criteria.TpAppFormId == null ? 1 == 1 : aq.TpAppFormId == TpAppFormId_cri))
                                    orderby (searchCriteria.searchOrder == 0 ? aq.TpAppFormId : aq.TpAppFormId)
                                    select new
                                    {
                                        TpAppQuestionId = aq.TpAppQuestionId,
                                        TpAppFormId = aq.TpAppFormId,
                                        QuestionId = aq.QuestionId,
                                        OrderNo = aq.OrderNo,
                                        PrerequistOrderNo = aq.PrerequistOrderNo,
                                        RequireFlag = aq.RequireFlag,
                                        CreateUser = aq.CreateUser,
                                        CreateDtm = aq.CreateDtm,
                                        UpdateUser = aq.UpdateUser,
                                        UpdateDtm = aq.UpdateDtm,


                                        QuestionCode = q.QuestionCode,
                                        QuestionNameTh = q.QuestionNameTh,
                                        QuestionNameEn = q.QuestionNameEn,
                                        AnsType = q.AnsType,
                                        AnsValues = q.AnsValues,
                                        PublicFlag = q.PublicFlag,
                                        ActiveFlag = q.ActiveFlag

                                    });
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<SearchTemplateAppQuestionByIdCustom> searchResult = new EntitySearchResultBase<SearchTemplateAppQuestionByIdCustom>();
                searchResult.totalRecords = query.Count();



                List<SearchTemplateAppQuestionByIdCustom> saleLst = new List<SearchTemplateAppQuestionByIdCustom>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    SearchTemplateAppQuestionByIdCustom s = new SearchTemplateAppQuestionByIdCustom();
                    s.TpAppQuestionId = item.TpAppQuestionId;
                    s.TpAppFormId = item.TpAppFormId;
                    s.QuestionId = item.QuestionId;
                    s.OrderNo = item.OrderNo;
                    s.PrerequistOrderNo = item.PrerequistOrderNo;
                    s.RequireFlag = item.RequireFlag;
                    s.CreateUser = item.CreateUser;
                    s.CreateDtm = item.CreateDtm;
                    s.UpdateUser = item.UpdateUser;
                    s.UpdateDtm = item.UpdateDtm;
                    s.QuestionCode = item.QuestionCode;
                    s.QuestionNameTh = item.QuestionNameTh;
                    s.QuestionNameEn = item.QuestionNameEn;
                    s.AnsType = item.AnsType;
                    s.AnsValues = item.AnsValues;
                    s.PublicFlag = item.PublicFlag;
                    s.ActiveFlag = item.ActiveFlag;
                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;
            }

        }
        

    }
}
