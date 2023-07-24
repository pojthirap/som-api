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
using MyFirstAzureWebApp.Entity.custom;
using System.Data;
using static MyFirstAzureWebApp.Entity.custom.ViewSurveyResultCustom;
using Newtonsoft.Json;
using static MyFirstAzureWebApp.Entity.custom.ViewTemplateSaResultCustom;

namespace MyFirstAzureWebApp.Business.org
{

    public class RecordSaFormImp : IRecordSaForm
    {
        private Logger log = LogManager.GetCurrentClassLogger();




        public async Task<EntitySearchResultBase<SearchTemplateSaResultTabCustom>> searchTemplateSaResultTab(SearchCriteriaBase<SearchTemplateSaResultTabCriteria> searchCriteria, UserProfileForBack userProfile)
        {


            EntitySearchResultBase<SearchTemplateSaResultTabCustom> searchResult = new EntitySearchResultBase<SearchTemplateSaResultTabCustom>();
            List<SearchTemplateSaResultTabCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                SearchTemplateSaResultTabCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("  select SF.TP_SA_FORM_ID,F.REC_SA_FORM_ID,SF.TP_NAME_TH,F.CREATE_DTM,E.FIRST_NAME+' '+E.LAST_NAME SALE_NAME ");
                queryBuilder.AppendFormat("  from RECORD_SA_FORM F ");
                queryBuilder.AppendFormat("  inner join TEMPLATE_SA_FORM SF on SF.TP_SA_FORM_ID = F.TP_SA_FORM_ID ");
                queryBuilder.AppendFormat("  inner join ADM_EMPLOYEE E on E.EMP_ID = F.CREATE_USER ");
                queryBuilder.AppendFormat("  where 1=1   ");
                
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.ProspectId))
                    {
                        queryBuilder.AppendFormat("  and F.PROSP_ID = @ProspectId   ");
                        QueryUtils.addParam(command, "ProspectId", o.ProspectId);// Add new
                    }

                    if (!String.IsNullOrEmpty(o.TpNameTh))
                    {
                        queryBuilder.AppendFormat(" and SF.TP_NAME_TH like @TpNameTh  ");
                        QueryUtils.addParamLike(command, "TpNameTh", o.TpNameTh);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.FromDate))
                    {
                        queryBuilder.AppendFormat(" and F.CREATE_DTM >= @FromDate ");//convert(datetime, 'Oct 23 2012 11:01AM') -  https://www.w3schools.com/sql/func_sqlserver_convert.asp
                        QueryUtils.addParam(command, "FromDate", o.FromDate);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.ToDate))
                    {
                        queryBuilder.AppendFormat(" and F.CREATE_DTM <= @CREATE_DTM ");
                        QueryUtils.addParam(command, "CREATE_DTM", o.ToDate.Replace("00:00:00", "23:59:59"));// Add new
                    }
                }

                // For Paging
                queryBuilder.AppendFormat(" ORDER BY TP_SA_FORM_ID  ");
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

                    List<SearchTemplateSaResultTabCustom> dataRecordList = new List<SearchTemplateSaResultTabCustom>();
                    SearchTemplateSaResultTabCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchTemplateSaResultTabCustom();


                        dataRecord.TpSaFormId = QueryUtils.getValueAsString(record, "TP_SA_FORM_ID");
                        dataRecord.RecSaFormId = QueryUtils.getValueAsString(record, "REC_SA_FORM_ID");
                        dataRecord.TpNameTh = QueryUtils.getValueAsString(record, "TP_NAME_TH");
                        dataRecord.SaleName = QueryUtils.getValueAsString(record, "SALE_NAME");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");


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







        public async Task<EntitySearchResultBase<ViewTemplateSaResultCustom>> viewTemplateSaResult(SearchCriteriaBase<ViewTemplateSaResultCriteria> searchCriteria, UserProfileForBack userProfile)
        {
            ViewTemplateSaResultCriteria criteria = searchCriteria.model;
            StringBuilder queryBuilder = new StringBuilder();
            List<ViewTemplateSaResultCustom> lst = new List<ViewTemplateSaResultCustom>();

            ViewTemplateSaResultCustom viewSurveyResultCustom = new ViewTemplateSaResultCustom();
            TemplateSaFormCustomForViewTemplateSaResult form = new TemplateSaFormCustomForViewTemplateSaResult();
            List<TemplateSaTitle> title = new List<TemplateSaTitle>();
            List<RecordSaFormFile> listFile = new List<RecordSaFormFile>();

            viewSurveyResultCustom.form = form;
            viewSurveyResultCustom.title = title;
            viewSurveyResultCustom.listFile = listFile;


            lst.Add(viewSurveyResultCustom);


            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Start Query Scope 
                // Set ObjectReponse.form

                queryBuilder.AppendFormat(" select SF.*,SF.CREATE_USER as SF_CREATE_USER,SF.CREATE_DTM  as SF_CREATE_DTM, R.CREATE_USER AS R_CREATE_USER, R.UPDATE_DTM AS R_UPDATE_DTM, R.UPDATE_USER AS R_UPDATE_USER, R.CREATE_DTM AS R_CREATE_DTM  ");
                queryBuilder.AppendFormat(" from RECORD_SA_FORM R  ");
                queryBuilder.AppendFormat(" inner join TEMPLATE_SA_FORM SF on SF.TP_SA_FORM_ID = R.TP_SA_FORM_ID  ");
                queryBuilder.AppendFormat(" where 1 = 1  ");


                if (criteria != null)
                {
                    if (!String.IsNullOrEmpty(criteria.RecSaFormId))
                    {
                        queryBuilder.AppendFormat(" AND  R.REC_SA_FORM_ID = @REC_SA_FORM_ID_1  ");
                        QueryUtils.addParam(command, "REC_SA_FORM_ID_1", criteria.RecSaFormId);// Add new
                    }
                }


                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        form.TpSaFormId = QueryUtils.getValueAsDecimalRequired(record, "TP_SA_FORM_ID");
                        form.TpCode = QueryUtils.getValueAsString(record, "TP_CODE");
                        form.TpNameTh = QueryUtils.getValueAsString(record, "TP_NAME_TH");
                        form.TpNameEn = QueryUtils.getValueAsString(record, "TP_NAME_EN");
                        form.UsedFlag = QueryUtils.getValueAsString(record, "USED_FLAG");
                        form.UsedDtm = QueryUtils.getValueAsDateTime(record, "USED_DTM");
                        form.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        form.CreateUser = QueryUtils.getValueAsString(record, "R_CREATE_USER");
                        form.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "R_CREATE_DTM");
                        form.UpdateUser = QueryUtils.getValueAsString(record, "R_UPDATE_USER");
                        form.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "R_UPDATE_DTM");

                        form.TemplateSaFormCreateUser = QueryUtils.getValueAsString(record, "SF_CREATE_USER");
                        form.TemplateSaFormCreateDtm = QueryUtils.getValueAsDateTime(record, "SF_CREATE_DTM");

                        //LovAttachCategory.Add(msAttachCategory);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 



                // Start Query Scope 
                // Set ObjectReponse.listFile
                queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select * ");
                queryBuilder.AppendFormat(" from RECORD_SA_FORM_FILE ");
                queryBuilder.AppendFormat(" where 1=1 ");
                if (criteria != null)
                {
                    if (!String.IsNullOrEmpty(criteria.RecSaFormId))
                    {
                        queryBuilder.AppendFormat(" AND REC_SA_FORM_ID = @REC_SA_FORM_ID_2  ");
                        QueryUtils.addParam(command, "REC_SA_FORM_ID_2", criteria.RecSaFormId);// Add new
                    }
                }


                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    RecordSaFormFile recordData = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        recordData = new RecordSaFormFile();
                        recordData.RecSaFormFileId = QueryUtils.getValueAsDecimalRequired(record, "REC_SA_FORM_FILE_ID"); 
                        recordData.FileId = QueryUtils.getValueAsDecimalRequired(record, "FILE_ID");
                        recordData.AttachCateId = QueryUtils.getValueAsDecimal(record, "ATTACH_CATE_ID");
                        recordData.RecSaFormId = QueryUtils.getValueAsDecimalRequired(record, "REC_SA_FORM_ID");
                        recordData.FileName = QueryUtils.getValueAsString(record, "FILE_NAME");
                        recordData.FileExt = QueryUtils.getValueAsString(record, "FILE_EXT");
                        recordData.FileSize = QueryUtils.getValueAsString(record, "FILE_SIZE");
                        recordData.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        recordData.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                        recordData.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        recordData.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");


                        listFile.Add(recordData);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 



                // Start Query Scope 
                // Set ObjectReponse.title
                queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("  select ST.*,  ");
                queryBuilder.AppendFormat("  CASE ST.TITLE_COLM_NO    ");
                queryBuilder.AppendFormat("          WHEN 1 THEN R.TITLE_COLM_NO1  ");
                queryBuilder.AppendFormat("          WHEN 2 THEN R.TITLE_COLM_NO2  ");
                queryBuilder.AppendFormat("          WHEN 3 THEN R.TITLE_COLM_NO3  ");
                queryBuilder.AppendFormat("          WHEN 4 THEN R.TITLE_COLM_NO4  ");
                queryBuilder.AppendFormat("          WHEN 5 THEN R.TITLE_COLM_NO5  ");
                queryBuilder.AppendFormat("          WHEN 6 THEN R.TITLE_COLM_NO6  ");
                queryBuilder.AppendFormat("          WHEN 7 THEN R.TITLE_COLM_NO7  ");
                queryBuilder.AppendFormat("          WHEN 8 THEN R.TITLE_COLM_NO8  ");
                queryBuilder.AppendFormat("          WHEN 9 THEN R.TITLE_COLM_NO9  ");
                queryBuilder.AppendFormat("          WHEN 10 THEN R.TITLE_COLM_NO10  ");
                queryBuilder.AppendFormat("          WHEN 11 THEN R.TITLE_COLM_NO11  ");
                queryBuilder.AppendFormat("          WHEN 12 THEN R.TITLE_COLM_NO12  ");
                queryBuilder.AppendFormat("          WHEN 13 THEN R.TITLE_COLM_NO13  ");
                queryBuilder.AppendFormat("          WHEN 14 THEN R.TITLE_COLM_NO14  ");
                queryBuilder.AppendFormat("          WHEN 15 THEN R.TITLE_COLM_NO15  ");
                queryBuilder.AppendFormat("          WHEN 16 THEN R.TITLE_COLM_NO16  ");
                queryBuilder.AppendFormat("          WHEN 17 THEN R.TITLE_COLM_NO17  ");
                queryBuilder.AppendFormat("          WHEN 18 THEN R.TITLE_COLM_NO18  ");
                queryBuilder.AppendFormat("          WHEN 19 THEN R.TITLE_COLM_NO19  ");
                queryBuilder.AppendFormat("          WHEN 20 THEN R.TITLE_COLM_NO20  ");
                queryBuilder.AppendFormat("          WHEN 21 THEN R.TITLE_COLM_NO21  ");
                queryBuilder.AppendFormat("          WHEN 22 THEN R.TITLE_COLM_NO22  ");
                queryBuilder.AppendFormat("          WHEN 23 THEN R.TITLE_COLM_NO23  ");
                queryBuilder.AppendFormat("          WHEN 24 THEN R.TITLE_COLM_NO24  ");
                queryBuilder.AppendFormat("          WHEN 25 THEN R.TITLE_COLM_NO25  ");
                queryBuilder.AppendFormat("          WHEN 26 THEN R.TITLE_COLM_NO26  ");
                queryBuilder.AppendFormat("          WHEN 27 THEN R.TITLE_COLM_NO27  ");
                queryBuilder.AppendFormat("          WHEN 28 THEN R.TITLE_COLM_NO28  ");
                queryBuilder.AppendFormat("          WHEN 29 THEN R.TITLE_COLM_NO29  ");
                queryBuilder.AppendFormat("          WHEN 30 THEN R.TITLE_COLM_NO30  ");
                queryBuilder.AppendFormat("          WHEN 31 THEN R.TITLE_COLM_NO31  ");
                queryBuilder.AppendFormat("          WHEN 32 THEN R.TITLE_COLM_NO32  ");
                queryBuilder.AppendFormat("          WHEN 33 THEN R.TITLE_COLM_NO33  ");
                queryBuilder.AppendFormat("          WHEN 34 THEN R.TITLE_COLM_NO34  ");
                queryBuilder.AppendFormat("          WHEN 35 THEN R.TITLE_COLM_NO35  ");
                queryBuilder.AppendFormat("          WHEN 36 THEN R.TITLE_COLM_NO36  ");
                queryBuilder.AppendFormat("          WHEN 37 THEN R.TITLE_COLM_NO37  ");
                queryBuilder.AppendFormat("          WHEN 38 THEN R.TITLE_COLM_NO38  ");
                queryBuilder.AppendFormat("          WHEN 39 THEN R.TITLE_COLM_NO39  ");
                queryBuilder.AppendFormat("          WHEN 40 THEN R.TITLE_COLM_NO40  ");
                queryBuilder.AppendFormat("          WHEN 41 THEN R.TITLE_COLM_NO41  ");
                queryBuilder.AppendFormat("          WHEN 42 THEN R.TITLE_COLM_NO42  ");
                queryBuilder.AppendFormat("          WHEN 43 THEN R.TITLE_COLM_NO43  ");
                queryBuilder.AppendFormat("          WHEN 44 THEN R.TITLE_COLM_NO44  ");
                queryBuilder.AppendFormat("          WHEN 45 THEN R.TITLE_COLM_NO45  ");
                queryBuilder.AppendFormat("          WHEN 46 THEN R.TITLE_COLM_NO46  ");
                queryBuilder.AppendFormat("          WHEN 47 THEN R.TITLE_COLM_NO47  ");
                queryBuilder.AppendFormat("          WHEN 48 THEN R.TITLE_COLM_NO48  ");
                queryBuilder.AppendFormat("          WHEN 49 THEN R.TITLE_COLM_NO49  ");
                queryBuilder.AppendFormat("          WHEN 50 THEN R.TITLE_COLM_NO50  ");
                queryBuilder.AppendFormat("          ELSE 'N/A'   ");
                queryBuilder.AppendFormat("  END TITLE_COLM_ANS  ");
                queryBuilder.AppendFormat("  from TEMPLATE_SA_TITLE ST  ");
                queryBuilder.AppendFormat("  inner join RECORD_SA_FORM R on R.TP_SA_FORM_ID = ST.TP_SA_FORM_ID  ");
                queryBuilder.AppendFormat("  where 1=1 ");

                if (criteria != null)
                {
                    if (!String.IsNullOrEmpty(criteria.RecSaFormId))
                    {
                        queryBuilder.AppendFormat(" AND  R.REC_SA_FORM_ID = @REC_SA_FORM_ID_3  ");
                        QueryUtils.addParam(command, "REC_SA_FORM_ID_3", criteria.RecSaFormId);// Add new
                    }
                }

                queryBuilder.AppendFormat(" order by ST.TITLE_COLM_NO ");

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                await using (var reader = command.ExecuteReader())
                {
                    TemplateSaTitle recordData = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        recordData = new TemplateSaTitle();
                        recordData.TpSaTitleId = QueryUtils.getValueAsDecimalRequired(record, "TP_SA_TITLE_ID");
                        recordData.TpSaFormId = QueryUtils.getValueAsDecimal(record, "TP_SA_FORM_ID");
                        recordData.TitleColmNo = QueryUtils.getValueAsDecimalRequired(record, "TITLE_COLM_NO");
                        recordData.TitleNameTh = QueryUtils.getValueAsString(record, "TITLE_NAME_TH");
                        recordData.TitleNameEn = QueryUtils.getValueAsString(record, "TITLE_NAME_EN");
                        recordData.AnsType = QueryUtils.getValueAsString(record, "ANS_TYPE");
                        recordData.AnsValType = QueryUtils.getValueAsDecimal(record, "ANS_VAL_TYPE");
                        recordData.AnsLovType = QueryUtils.getValueAsDecimal(record, "ANS_LOV_TYPE");
                        recordData.titleColmAns = QueryUtils.getValueAsString(record, "TITLE_COLM_ANS");

                        recordData.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        recordData.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                        recordData.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        recordData.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");
                        
                        title.Add(recordData);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 



            }

            EntitySearchResultBase<ViewTemplateSaResultCustom> searchResult = new EntitySearchResultBase<ViewTemplateSaResultCustom>();
            searchResult.data = lst;
            searchResult.totalRecords = lst==null ? 0 : lst.Count;
            return searchResult;
        }





    }
}
