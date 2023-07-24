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
using MyFirstAzureWebApp.Models.importfile;

namespace MyFirstAzureWebApp.Business.org
{

    public class ImportErrorFileLogImp : IImportErrorFileLog
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<int> GetImportErrorFileLogSeq()
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
                        context.Database.ExecuteSqlRaw("set @result = next value for IMPORT_ERROR_FILE_LOG_SEQ", p);
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



        public async Task<int> uploadErrorFileLog(ImportErrorLogFileModel importErrorLogFileModel, UserProfileForBack userProfileForBack)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO IMPORT_ERROR_FILE_LOG ([FILE_ID], [FILE_NAME], [FILE_EXT], [FILE_SIZE], [IMPORT_DATA_TYPE], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                        queryBuilder.AppendFormat(" VALUES(@FILE_ID, @FILE_NAME, @FILE_EXT, @FILE_SIZE, @IMPORT_DATA_TYPE, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME()) ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_ID", importErrorLogFileModel.FileId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_NAME", importErrorLogFileModel.FileName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_EXT", importErrorLogFileModel.FileExt));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_SIZE", importErrorLogFileModel.FileSize));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("IMPORT_DATA_TYPE", importErrorLogFileModel.ImportDataType));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfileForBack.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
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




    }
}
