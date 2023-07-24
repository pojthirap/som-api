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
using System.Data;

namespace MyFirstAzureWebApp.Business.org
{

    public class RecordMeterImp : IRecordMeter
    {
        private Logger log = LogManager.GetCurrentClassLogger();



        public async Task<EntitySearchResultBase<SearchRecordMeterTabCustom>> searchRecordMeterTab(SearchCriteriaBase<SearchRecordMeterTabCriteria> searchCriteria)
        {

            SearchRecordMeterTabCriteria o = searchCriteria.model;
            EntitySearchResultBase<SearchRecordMeterTabCustom> searchResult = new EntitySearchResultBase<SearchRecordMeterTabCustom>();
            List<SearchRecordMeterTabCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                //queryBuilder.AppendFormat("  select RM.CREATE_USER as RM_CREATE_USER, RM.CREATE_DTM as RM_CREATE_DTM, RM.UPDATE_USER as RM_UPDATE_USER, RM.UPDATE_DTM as RM_UPDATE_DTM ,RM.*,MF.*,M.* ");
                queryBuilder.AppendFormat("  select M.DISPENSER_NO,M.NOZZLE_NO,G.GAS_NAME_TH,RM.REC_RUN_NO,RM.UPDATE_DTM,E.FIRST_NAME+' '+E.LAST_NAME  CREATE_USER,RM.FILE_ID ");
                queryBuilder.AppendFormat("          from RECORD_METER RM  ");
                queryBuilder.AppendFormat("          inner join MS_METER M on M.METER_ID = RM.METER_ID  ");
                queryBuilder.AppendFormat("          inner join MS_GASOLINE G on G.GAS_ID = M.GAS_ID ");
                queryBuilder.AppendFormat("          inner join ADM_EMPLOYEE E on E.EMP_ID = RM.CREATE_USER ");
                queryBuilder.AppendFormat("          left join RECORD_METER_FILE MF on MF.FILE_ID = RM.FILE_ID  ");
                queryBuilder.AppendFormat("          where RM.PLAN_TRIP_TASK_ID = ( ");
                queryBuilder.AppendFormat("                  select top 1 TT.PLAN_TRIP_TASK_ID  ");
                queryBuilder.AppendFormat("                  from PLAN_TRIP_PROSPECT TP  ");
                queryBuilder.AppendFormat("                  inner join PLAN_TRIP_TASK TT on TT.TASK_TYPE = 'M' and TT.PLAN_TRIP_PROSP_ID = TP.PLAN_TRIP_PROSP_ID  ");
                queryBuilder.AppendFormat("                  and TP.VISIT_CHECKOUT_DTM is not null  ");
                queryBuilder.AppendFormat("                  and TP.PROSP_ID = @PROSP_ID   ");
                queryBuilder.AppendFormat("                  ORDER BY TP.VISIT_CHECKOUT_DTM DESC ");
                queryBuilder.AppendFormat("  )  ");
                QueryUtils.addParam(command, "PROSP_ID", o.prospId);// Add new


                // For Paging
                if (searchCriteria.searchOrder == 1)
                {
                    queryBuilder.AppendFormat(" ORDER BY M.DISPENSER_NO , M.NOZZLE_NO   ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY G.GAS_NAME_TH  ");
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
                    //

                    List<SearchRecordMeterTabCustom> dataRecordList = new List<SearchRecordMeterTabCustom>();
                    SearchRecordMeterTabCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchRecordMeterTabCustom();

                        dataRecord.DispenserNo = QueryUtils.getValueAsDecimal(record, "DISPENSER_NO");
                        dataRecord.NozzleNo = QueryUtils.getValueAsDecimal(record, "NOZZLE_NO");
                        dataRecord.GasNameTh = QueryUtils.getValueAsString(record, "GAS_NAME_TH");
                        dataRecord.RecRunNo = QueryUtils.getValueAsString(record, "REC_RUN_NO");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");
                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.FileId =  QueryUtils.getValueAsString(record, "FILE_ID");

                        /*dataRecord.RecordMeterCreateUser = QueryUtils.getValueAsString(record, "RM_CREATE_USER");
                        dataRecord.RecordMeterCreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "RM_CREATE_DTM");
                        dataRecord.RecordMeterUpdateUser = QueryUtils.getValueAsString(record, "RM_UPDATE_USER");
                        dataRecord.RecordMeterUpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "RM_UPDATE_DTM");
                        dataRecord.RecMeterId = QueryUtils.getValueAsDecimalRequired(record, "REC_METER_ID");
                        dataRecord.PlanTripTaskId = QueryUtils.getValueAsDecimalRequired(record, "PLAN_TRIP_TASK_ID");
                        dataRecord.MeterId = QueryUtils.getValueAsDecimalRequired(record, "METER_ID");
                        dataRecord.RecRunNo = QueryUtils.getValueAsString(record, "REC_RUN_NO");
                        dataRecord.FileId = QueryUtils.getValueAsDecimalRequired(record, "FILE_ID");
                        dataRecord.FileName = QueryUtils.getValueAsString(record, "FILE_NAME");
                        dataRecord.FileExt = QueryUtils.getValueAsString(record, "FILE_EXT");
                        dataRecord.FileSize = QueryUtils.getValueAsString(record, "FILE_SIZE");
                        dataRecord.GasId = QueryUtils.getValueAsDecimalRequired(record, "GAS_ID");
                        dataRecord.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        dataRecord.DispenserNo = QueryUtils.getValueAsDecimal(record, "DISPENSER_NO");
                        dataRecord.NozzleNo = QueryUtils.getValueAsDecimal(record, "NOZZLE_NO");
                        dataRecord.Qrcode = QueryUtils.getValueAsString(record, "QRCODE");
                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");*/


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
