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
using System.Data.Common;
using System.Data;

namespace MyFirstAzureWebApp.Business.org
{

    public class TemplateSaTitleImp : ITemplateSaTitle
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        



        public async Task<EntitySearchResultBase<SearchTemplateSaFormByIdCustom>> searchTemplateSaFormById(SearchCriteriaBase<TemplateSaTitleCriteria> searchCriteria)
        {


            EntitySearchResultBase<SearchTemplateSaFormByIdCustom> searchResult = new EntitySearchResultBase<SearchTemplateSaFormByIdCustom>();
            List<SearchTemplateSaFormByIdCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select ST.TITLE_COLM_NO, ST.UPDATE_DTM, ST.TP_SA_TITLE_ID, ST.title_name_th, IIF(ST.ANS_TYPE='L', 'Master', 'Title') title_type, ST.ANS_LOV_TYPE, ST.ANS_VAL_TYPE,CF.LOV_NAME_TH,CF.LOV_NAME_EN from TEMPLATE_SA_TITLE ST inner join MS_CONFIG_LOV CF on CF.LOV_KEYWORD = IIF(ST.ANS_TYPE='L', 'ANS_LOV_TYPE','ANS_VAL_TYPE') and CF.LOV_KEYVALUE = IIF(ST.ANS_TYPE='L', ST.ANS_LOV_TYPE, ANS_VAL_TYPE)  ");
                TemplateSaTitleCriteria o = searchCriteria.model;
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.TpSaFormId))
                    {
                        queryBuilder.AppendFormat(" and ST.TP_SA_FORM_ID = @TpSaFormId ");
                        QueryUtils.addParam(command, "TpSaFormId", o.TpSaFormId);// Add new
                    }
                }


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY ST.TITLE_COLM_NO  ");
                /*if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY ST.TITLE_COLM_NO DESC  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY ST.TITLE_COLM_NO  ");
                }*/
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
                    lst = SearchTemplateSaFormByIdMapRow(reader);
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }

		
		
        private List<SearchTemplateSaFormByIdCustom> SearchTemplateSaFormByIdMapRow(DbDataReader reader)
        {
            List<SearchTemplateSaFormByIdCustom> lst = new List<SearchTemplateSaFormByIdCustom>();
            SearchTemplateSaFormByIdCustom o = null;
            while (reader.Read())
            {
                IDataRecord record = (IDataRecord)reader;
                o = new SearchTemplateSaFormByIdCustom();
                    o.TpSaTitleId = QueryUtils.getValueAsDecimal(record, "TP_SA_TITLE_ID");
                    o.TitleNameTh = QueryUtils.getValueAsString(record, "title_name_th");
                    o.TitleType = QueryUtils.getValueAsString(record, "title_type");
                    o.AnsLovType = QueryUtils.getValueAsDecimal(record, "ANS_LOV_TYPE");
                    o.AnsValType = QueryUtils.getValueAsDecimal(record, "ANS_VAL_TYPE");
                    o.LovNameTh = QueryUtils.getValueAsString(record, "LOV_NAME_TH");
                    o.LovNameEn = QueryUtils.getValueAsString(record, "LOV_NAME_EN");
                
                lst.Add(o);
            }

            // Call Close when done reading.
            reader.Close();

            return lst;

        }


        public async Task<TemplateSaTitle> addTemplateSaTitle(TemplateSaTitleModel templateSaTitleModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" UPDATE TEMPLATE_SA_FORM SET[UPDATE_USER] = @User, [UPDATE_DTM] = dbo.GET_SYSDATETIME() WHERE TP_SA_FORM_ID = @TpSaFormId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpSaFormId", templateSaTitleModel.TpSaFormId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", templateSaTitleModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        sqlParameters = new List<SqlParameter>();// Add New
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for TEMPLATE_STOCK_CARD_SEQ", p);
                        var nextVal = (int)p.Value;

                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO  TEMPLATE_SA_TITLE (TP_SA_TITLE_ID, TP_SA_FORM_ID, TITLE_COLM_NO, TITLE_NAME_TH, TITLE_NAME_EN, ANS_TYPE, ANS_VAL_TYPE, ANS_LOV_TYPE, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat(" SELECT @nextVal , @TpSaFormId, IIF(max(title_colm_no) IS null,1,max(title_colm_no)+1), @TitleNameTh, @TitleNameEn, @AnsType, @AnsValType, @AnsLovType, @User, dbo.GET_SYSDATETIME(), @User, dbo.GET_SYSDATETIME() FROM TEMPLATE_SA_TITLE WHERE TP_SA_FORM_ID = @TpSaFormId ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("nextVal", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpSaFormId", templateSaTitleModel.TpSaFormId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TitleNameTh", templateSaTitleModel.TitleNameTh));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TitleNameEn", templateSaTitleModel.TitleNameEn));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AnsType", templateSaTitleModel.AnsType));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AnsValType", templateSaTitleModel.AnsValType));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("AnsLovType", templateSaTitleModel.AnsLovType));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("User", templateSaTitleModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        TemplateSaTitle re = new TemplateSaTitle();
                        re.TpSaTitleId = nextVal;
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

        

        public async Task<int> delTemplateSaTitle(TemplateSaTitleModel templateSaTitleModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        string TpSaFormId = await getEditGeneralDataFlag(templateSaTitleModel);

                        // Delete
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM  TEMPLATE_SA_TITLE   WHERE TP_SA_TITLE_ID=@TP_SA_TITLE_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_SA_TITLE_ID", templateSaTitleModel.TpSaTitleId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query Delete:" + queryStr);
                        Console.WriteLine("Query Delete:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        // Update
                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" UPDATE TITLES  ");
                        queryBuilder.AppendFormat(" SET ");
                        queryBuilder.AppendFormat("     TITLES.TITLE_COLM_NO = TITLES_ORDER.Row# ");
                        queryBuilder.AppendFormat(" FROM TEMPLATE_SA_TITLE TITLES ");
                        queryBuilder.AppendFormat(" INNER JOIN ");
                        queryBuilder.AppendFormat(" (SELECT ROW_NUMBER() OVER(ORDER BY TITLE_COLM_NO ASC) AS Row#,TP_SA_TITLE_ID ");
                        queryBuilder.AppendFormat(" FROM TEMPLATE_SA_TITLE ");
                        queryBuilder.AppendFormat(" WHERE TP_SA_FORM_ID = @TpSaFormId ) TITLES_ORDER ");
                        queryBuilder.AppendFormat(" ON TITLES.TP_SA_TITLE_ID = TITLES_ORDER.TP_SA_TITLE_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TpSaFormId", TpSaFormId));// Add New
                        queryStr = queryBuilder.ToString();
                        log.Debug("Query Update:" + queryStr);
                        Console.WriteLine("Query Update:" + queryStr);
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
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




        public async Task<String> getEditGeneralDataFlag(TemplateSaTitleModel templateSaTitleModel)
        {
            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" SELECT TP_SA_FORM_ID FROM TEMPLATE_SA_TITLE WHERE TP_SA_TITLE_ID = @TP_SA_TITLE_ID ");
                QueryUtils.addParam(command, "TP_SA_TITLE_ID", templateSaTitleModel.TpSaTitleId);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                            o = QueryUtils.getValueAsString(record, "TP_SA_FORM_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }




    }
}
