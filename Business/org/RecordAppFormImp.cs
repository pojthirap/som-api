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

namespace MyFirstAzureWebApp.Business.org
{

    public class RecordAppFormImp : IRecordAppForm
    {
        private Logger log = LogManager.GetCurrentClassLogger();




        public async Task<EntitySearchResultBase<SearchSurveyResultTabCustom>> searchSurveyResultTab(SearchCriteriaBase<SearchSurveyResultTabCriteria> searchCriteria, UserProfileForBack userProfile)
        {

            SearchSurveyResultTabCriteria o = searchCriteria.model;

            EntitySearchResultBase<SearchSurveyResultTabCustom> searchResult = new EntitySearchResultBase<SearchSurveyResultTabCustom>();
            List<SearchSurveyResultTabCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("  select F.REC_APP_FORM_ID,AF.TP_NAME_TH,F.CREATE_DTM,E.FIRST_NAME+' '+E.LAST_NAME SALE_NAME ");
                queryBuilder.AppendFormat("  from RECORD_APP_FORM F ");
                queryBuilder.AppendFormat("  inner join TEMPLATE_APP_FORM AF on AF.TP_APP_FORM_ID = F.TP_APP_FORM_ID ");
                queryBuilder.AppendFormat("  inner join ADM_EMPLOYEE E on E.EMP_ID = F.CREATE_USER ");
                queryBuilder.AppendFormat("  where 1=1 ");


                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.ProspId))
                    {
                        queryBuilder.AppendFormat(" and F.PROSP_ID = @ProspId ");
                        QueryUtils.addParam(command, "ProspId", o.ProspId);// Add new
                    }

                    if (!String.IsNullOrEmpty(o.TpNameTh))
                    {
                        queryBuilder.AppendFormat(" and AF.TP_NAME_TH like @TpNameTh   ");
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
                queryBuilder.AppendFormat(" ORDER BY TP_NAME_TH  ");
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

                    List<SearchSurveyResultTabCustom> dataRecordList = new List<SearchSurveyResultTabCustom>();
                    SearchSurveyResultTabCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchSurveyResultTabCustom();


                        dataRecord.TpNameTh = QueryUtils.getValueAsString(record, "TP_NAME_TH");
                        dataRecord.SaleName = QueryUtils.getValueAsString(record, "SALE_NAME");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        dataRecord.RecAppFormId = QueryUtils.getValueAsString(record, "REC_APP_FORM_ID"); 


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







        public async Task<EntitySearchResultBase<ViewSurveyResultCustom>> viewSurveyResult(SearchCriteriaBase<ViewSurveyResultCriteria> searchCriteria, UserProfileForBack userProfile)
        {
            ViewSurveyResultCriteria criteria = searchCriteria.model;
            StringBuilder queryBuilder = new StringBuilder();
            List<ViewSurveyResultCustom> lst = new List<ViewSurveyResultCustom>();

            ViewSurveyResultCustom viewSurveyResultCustom = new ViewSurveyResultCustom();
            List<MsAttachCategory> LovAttachCategory = new List<MsAttachCategory>();
            List<RecordAppFormFile> ListFile = new List<RecordAppFormFile>();
            List<ObjectForm> ObjForm = new List<ObjectForm>();

            viewSurveyResultCustom.LovAttachCategory = LovAttachCategory;
            viewSurveyResultCustom.ListFile = ListFile;
            viewSurveyResultCustom.ObjForm = ObjForm;
            lst.Add(viewSurveyResultCustom);


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

                // Start Query Scope 
                // Set ObjectReponse.listFile
                queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select * from RECORD_APP_FORM_FILE where REC_APP_FORM_ID = @REC_APP_FORM_ID_1 ");
                QueryUtils.addParam(command, "REC_APP_FORM_ID_1", criteria.RceAppFormId);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                await using (var reader = command.ExecuteReader())
                {
                    RecordAppFormFile recordAppFromFile = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        recordAppFromFile = new RecordAppFormFile();
                        
                        recordAppFromFile.RecAppFormFileId = QueryUtils.getValueAsDecimalRequired(record, "REC_APP_FORM_FILE_ID");
                        recordAppFromFile.FileId = QueryUtils.getValueAsDecimalRequired(record, "FILE_ID");
                        recordAppFromFile.AttachCateId = QueryUtils.getValueAsDecimal(record, "ATTACH_CATE_ID");
                        recordAppFromFile.RecAppFormId = QueryUtils.getValueAsDecimalRequired(record, "REC_APP_FORM_ID");
                        recordAppFromFile.FileName = QueryUtils.getValueAsString(record, "FILE_NAME");
                        recordAppFromFile.FileExt = QueryUtils.getValueAsString(record, "FILE_EXT");
                        recordAppFromFile.FileSize = QueryUtils.getValueAsString(record, "FILE_SIZE");

                        recordAppFromFile.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        recordAppFromFile.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                        recordAppFromFile.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        recordAppFromFile.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");
                        
                        ListFile.Add(recordAppFromFile);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 


                // Set saleTerritory.salesRep 

                queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select * from RECORD_APP_FORM where REC_APP_FORM_ID = @REC_APP_FORM_ID_2 ");
                QueryUtils.addParam(command, "REC_APP_FORM_ID_2", criteria.RceAppFormId);// Add new


                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                await using (var reader = command.ExecuteReader())
                {
                    ObjectForm objForm = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        string json_ = QueryUtils.getValueAsString(record, "APP_FORM");
                        objForm = JsonConvert.DeserializeObject<ObjectForm>(json_);
                        ObjForm.Add(objForm);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
                // End Query Scope 



            }

            EntitySearchResultBase<ViewSurveyResultCustom> searchResult = new EntitySearchResultBase<ViewSurveyResultCustom>();
            searchResult.data = lst;
            return searchResult;
        }





    }
}
