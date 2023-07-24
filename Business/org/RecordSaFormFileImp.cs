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

    public class RecordSaFormFileImp : IRecordSaFormFile
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<int> GetRecordSaFromFileSeq()
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
                        context.Database.ExecuteSqlRaw("set @result = next value for RECORD_SA_FORM_FILE_SEQ", p);
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



        public async Task<RecordSaFormFile> uploadFileSaForm(RecordSaFormFileModel recordSaFormFileModel, UserProfileForBack userProfileForBack)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO  RECORD_SA_FORM_FILE (REC_SA_FORM_FILE_ID, FILE_ID, PHOTO_FLAG, FILE_NAME, FILE_EXT, FILE_SIZE, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@REC_SA_FORM_FILE_ID, @FILE_ID , @PHOTO_FLAG, @FILE_NAME, @FILE_EXT, @FILE_SIZE, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("REC_SA_FORM_FILE_ID", recordSaFormFileModel.FileId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_ID", recordSaFormFileModel.FileId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PHOTO_FLAG", recordSaFormFileModel.PhotoFlag));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_NAME", recordSaFormFileModel.FileName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_EXT", recordSaFormFileModel.FileExt));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_SIZE", recordSaFormFileModel.FileSize));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfileForBack.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        RecordSaFormFile re = new RecordSaFormFile();
                        re.RecSaFormId = Convert.ToDecimal(recordSaFormFileModel.FileId);
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




    }
}
