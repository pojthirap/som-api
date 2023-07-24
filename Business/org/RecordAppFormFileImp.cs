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
using MyFirstAzureWebApp.Models.record;

namespace MyFirstAzureWebApp.Business.org
{

    public class RecordAppFormFileImp : IRecordAppFormFile
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<int> GetRecordAppFromFileSeq()
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
                        context.Database.ExecuteSqlRaw("set @result = next value for RECORD_APP_FORM_FILE_SEQ", p);
                        var nextVal = (int)p.Value;
                        return nextVal;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

        }



        public async Task<RecordAppFormFile> uploadFileAppForm(RecordAppFormFileModel recordAppFormFileModel, UserProfileForBack userProfileForBack)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO  RECORD_APP_FORM_FILE (REC_APP_FORM_FILE_ID,FILE_ID, PHOTO_FLAG, FILE_NAME, FILE_EXT, FILE_SIZE, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@REC_APP_FORM_FILE_ID, @FILE_ID, @PHOTO_FLAG, @FILE_NAME, @FILE_EXT, @FILE_SIZE, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("REC_APP_FORM_FILE_ID", recordAppFormFileModel.FileId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_ID", recordAppFormFileModel.FileId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PHOTO_FLAG", recordAppFormFileModel.PhotoFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_NAME", recordAppFormFileModel.FileName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_EXT", recordAppFormFileModel.FileExt));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_SIZE", recordAppFormFileModel.FileSize));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfileForBack.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        RecordAppFormFile re = new RecordAppFormFile();
                        re.RecAppFormId = Convert.ToDecimal(recordAppFormFileModel.FileId);
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



        public async Task<EntitySearchResultBase<SearchAttachmentTabCustom>> searchAttachmentTab(SearchCriteriaBase<SearchAttachmentTabCriteria> searchCriteria)
        {

            EntitySearchResultBase<SearchAttachmentTabCustom> searchResult = new EntitySearchResultBase<SearchAttachmentTabCustom>();
            List<SearchAttachmentTabCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchAttachmentTabCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select FF.*,E.FIRST_NAME+' '+E.LAST_NAME EMP_NAME ");
                queryBuilder.AppendFormat(" from RECORD_APP_FORM AF ");
                queryBuilder.AppendFormat(" inner join RECORD_APP_FORM_FILE FF on FF.REC_APP_FORM_ID = AF.REC_APP_FORM_ID ");
                queryBuilder.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = FF.UPDATE_USER ");
                queryBuilder.AppendFormat(" where AF.PROSP_ID = @ProspId ");
                QueryUtils.addParam(command, "ProspId", o.ProspId);

                if (o != null)
                {
                    if (o.AttachCateId != null && o.AttachCateId.Length != 0)
                    {
                        StringBuilder AttachCateIdStr = new StringBuilder();
                        for (int i = 0; i < o.AttachCateId.Length; i++)
                        {
                            AttachCateIdStr.Append(o.AttachCateId[i]);
                            if (i == o.AttachCateId.Length - 1)
                            {
                                break;
                            }
                            AttachCateIdStr.Append(",");
                        }
                        queryBuilder.AppendFormat(" and FF.ATTACH_CATE_ID in (" + QueryUtils.getParamIn("AttachCateIdStr", AttachCateIdStr.ToString()) + ") ");
                        QueryUtils.addParamIn(command, "AttachCateIdStr", AttachCateIdStr.ToString());
                        QueryUtils.addParamLike(command, "AttachCateId", AttachCateIdStr.ToString());
                        //queryBuilder.AppendFormat(" and FF.ATTACH_CATE_ID in (@AttachCateId) ");
                        //QueryUtils.addParamLike(command, "AttachCateId", AttachCateIdStr.ToString());
                    }
                        if (!String.IsNullOrEmpty(o.PhotoFlag))
                        {
                            queryBuilder.AppendFormat(" and FF.PHOTO_FLAG = @PhotoFlag  ");
                            QueryUtils.addParam(command, "PhotoFlag", o.PhotoFlag);
                        }
                        if (!String.IsNullOrEmpty(o.FromDate))
                        {
                            queryBuilder.AppendFormat(" and FF.UPDATE_DTM >= @FromDate ");//convert(datetime, 'Oct 23 2012 11:01AM') -  https://www.w3schools.com/sql/func_sqlserver_convert.asp
                            QueryUtils.addParam(command, "FromDate", o.FromDate);
                        }
                        if (!String.IsNullOrEmpty(o.ToDate))
                        {
                            queryBuilder.AppendFormat(" and FF.UPDATE_DTM <= @ToDate ");
                            QueryUtils.addParam(command, "ToDate", o.ToDate.Replace("00:00:00", "23:59:59"));
                        }
                    
                }



                // For Paging
                queryBuilder.AppendFormat(" ORDER BY FILE_ID  ");
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
                command.CommandText = queryBuilder.ToString();// Add new
                log.Debug("Query Count:" + queryBuilder.ToString());
                Console.WriteLine("Query Count:" + queryBuilder.ToString());
                using (var reader = command.ExecuteReader())
                {
                    //

                    List<RecordAppFormFileForSearchAttachmentTab> dataRecordList = new List<RecordAppFormFileForSearchAttachmentTab>();
                    RecordAppFormFileForSearchAttachmentTab dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new RecordAppFormFileForSearchAttachmentTab();


                        dataRecord.FileId = QueryUtils.getValueAsDecimalRequired(record, "FILE_ID");
                        dataRecord.AttachCateId = QueryUtils.getValueAsDecimal(record, "ATTACH_CATE_ID");
                        dataRecord.RecAppFormId = QueryUtils.getValueAsDecimalRequired(record, "REC_APP_FORM_ID");
                        dataRecord.FileName = QueryUtils.getValueAsString(record, "FILE_NAME");
                        dataRecord.FileExt = QueryUtils.getValueAsString(record, "FILE_EXT");
                        dataRecord.FileSize = QueryUtils.getValueAsString(record, "FILE_SIZE");
                        dataRecord.PhotoFlag = QueryUtils.getValueAsString(record, "PHOTO_FLAG");
                        dataRecord.EmpName = QueryUtils.getValueAsString(record, "EMP_NAME");
                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");
                        dataRecord.UpdateDateTime = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");
                        dataRecord.UpdateDtmStr = QueryUtils.getValueAsString(record, "UPDATE_DTM").Split(" ")[0];

                        dataRecordList.Add(dataRecord);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //

                    List<SearchAttachmentTabCustom> lstResponse = new List<SearchAttachmentTabCustom>();
                    SearchAttachmentTabCustom resData = null;
                    if (dataRecordList!=null && dataRecordList.Count != 0)
                    {
                        var groupedByUpdateDtm = dataRecordList.GroupBy(z => z.UpdateDtmStr);
                        foreach (var group in groupedByUpdateDtm)
                        {
                            Console.WriteLine("Users starting with " + group.Key + ":");
                            int i = 0;
                            List<RecordAppFormFile> listRec = new List<RecordAppFormFile>();
                            resData = new SearchAttachmentTabCustom();
                            foreach (var obj in group)
                            {
                                listRec.Add(obj);
                                log.Info(obj.UpdateDtmStr);
                                log.Info(obj.UpdateDtm);
                                log.Info(obj.FileName);
                                if (i == 0) {
                                    resData.UpdateDtmStr = obj.UpdateDtmStr;
                                    resData.UpdateDateTime = obj.UpdateDtm;
                                    i++;
                                }
                            }
                            resData.RecordAppFormFileLst = listRec;
                            lstResponse.Add(resData);
                        }
                    }



                    lst = lstResponse;
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }







    }
}
