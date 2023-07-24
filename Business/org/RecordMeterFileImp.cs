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

    public class RecordMeterFileImp : IRecordMeterFile
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<int> GetRecordMeterFileSeq()
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
                        context.Database.ExecuteSqlRaw("set @result = next value for RECORD_METER_FILE_SEQ", p);
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



        public async Task<RecordMeterFile> uploadFileMeter(RecordMeterFileModel recordMeterFileModel, UserProfileForBack userProfileForBack)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO  RECORD_METER_FILE (FILE_ID, FILE_NAME, FILE_EXT, FILE_SIZE, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@FILE_ID, @FILE_NAME, @FILE_EXT, @FILE_SIZE, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_ID", recordMeterFileModel.FileId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_NAME", recordMeterFileModel.FileName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_EXT", recordMeterFileModel.FileExt));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FILE_SIZE", recordMeterFileModel.FileSize));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfileForBack.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        RecordMeterFile re = new RecordMeterFile();
                        re.RecMeterId = Convert.ToDecimal(recordMeterFileModel.FileId);
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
