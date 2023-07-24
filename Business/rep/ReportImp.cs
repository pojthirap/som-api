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
using MyFirstAzureWebApp.ModelCriteriaReport;
using MyFirstAzureWebApp.Utils;
using System.Data;
using MyFirstAzureWebApp.ModelsReport;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using MyFirstAzureWebApp.common;
using System.Diagnostics;
using MyFirstAzureWebApp.Controllers;

namespace MyFirstAzureWebApp.Business.rep
{

    public class ReportImp : IReport
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<SearchResultBase<Rep01VisitPlanRawDataResult>> SearchRep01VisitPlanRawData(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep01VisitPlanRawDataResult> searchResult = new SearchResultBase<Rep01VisitPlanRawDataResult>();
            
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder sb = new StringBuilder();
                /*
                sb.AppendFormat(" with R as ( ");
                sb.AppendFormat(" select TP.PROSP_ID,PP.PROSPECT_TYPE PROSP_TYPE ");
                sb.AppendFormat(" ,E.FIRST_NAME+' '+E.LAST_NAME SALE_REP_NAME,EMP_ID SALE_REP_ID ");
                sb.AppendFormat(" ,T.PLAN_TRIP_ID VISIT_PLAN_ID,T.PLAN_TRIP_NAME VISIT_PLAN_NAME ");
                sb.AppendFormat(" ,IIF(TP.ADHOC_FLAG='Y','Adhoc','Plan') VISIT_TYPE ");
                sb.AppendFormat(" ,FORMAT(DATEADD(yy,543,T.PLAN_TRIP_DATE),'dd/MM/yyyy') VISIT_DATE,T.PLAN_TRIP_DATE ");
                sb.AppendFormat(" ,ISNULL(AC.ACC_NAME,L.LOC_NAME_TH) ACC_NAME ");
                sb.AppendFormat(" ,CASE PP.PROSPECT_TYPE ");
                sb.AppendFormat("   WHEN '0' THEN 'Prospect' ");
                sb.AppendFormat("   WHEN '1' THEN 'Pump' ");
                sb.AppendFormat("   WHEN '2' THEN 'Customer' ");
                sb.AppendFormat("   ELSE '' ");
                sb.AppendFormat("   END PROSPECT_TYPE ");
                sb.AppendFormat(" ,ISNULL(lag(TP.VISIT_CHECKIN_MILE_NO) over (PARTITION BY T.PLAN_TRIP_ID order by TP.VISIT_CHECKIN_DTM),T.START_CHECKIN_MILE_NO) as START_MILE_NO ");
                sb.AppendFormat(" ,TP.VISIT_CHECKIN_MILE_NO FINISH_MILE_NO ");
                sb.AppendFormat(" ,(TP.VISIT_CHECKIN_MILE_NO - ISNULL(lag(TP.VISIT_CHECKIN_MILE_NO) over (PARTITION BY T.PLAN_TRIP_ID order by TP.VISIT_CHECKIN_DTM),T.START_CHECKIN_MILE_NO)) as TOTAL_KM_INPUT ");
                sb.AppendFormat(" ,TP.VISIT_CALC_KM TOTAL_KM_SYSTEM ");
                sb.AppendFormat(" ,FORMAT(DATEADD(yy,543,TP.PLAN_START_TIME),'dd/MM/yyyy HH:mm') PLAN_START_TIME ");
                sb.AppendFormat(" ,FORMAT(DATEADD(yy,543,TP.PLAN_END_TIME),'dd/MM/yyyy HH:mm') PLAN_END_TIME ");
                sb.AppendFormat(" ,FORMAT(DATEADD(yy,543,TP.VISIT_CHECKIN_DTM),'dd/MM/yyyy HH:mm') VISIT_CHECKIN_DTM ");
                sb.AppendFormat(" ,FORMAT(DATEADD(yy,543,TP.VISIT_CHECKOUT_DTM),'dd/MM/yyyy HH:mm') VISIT_CHECKOUT_DTM ");
                sb.AppendFormat(" ,R.REASON_NAME_TH ");
                sb.AppendFormat(" from PLAN_TRIP T ");
                sb.AppendFormat(" inner join PLAN_TRIP_PROSPECT TP on TP.PLAN_TRIP_ID = T.PLAN_TRIP_ID ");
                sb.AppendFormat(" inner join PROSPECT PP on PP.PROSPECT_ID = TP.PROSP_ID ");
                sb.AppendFormat(" inner join PROSPECT_ACCOUNT AC on AC.PROSP_ACC_ID = PP.PROSP_ACC_ID ");
                sb.AppendFormat(" left join PLAN_REASON_NOT_VISIT R on R.REASON_NOT_VISIT_ID = TP.REASON_NOT_VISIT_ID ");
                sb.AppendFormat(" left join MS_LOCATION L on L.LOC_ID = TP.LOC_ID ");
                sb.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = IIF(ISNULL(ASSIGN_EMP_ID,'')='',T.CREATE_USER,T.ASSIGN_EMP_ID) ");
                sb.AppendFormat(" ) ");
                sb.AppendFormat(" SELECT R.*  ");
                sb.AppendFormat(" FROM R ");
                sb.AppendFormat(" where 1=1 ");
                */

                sb.AppendFormat(" with R as ( ");
                sb.AppendFormat("     select T.seq,T.ORDER_NO,T.CHECKIN_DTM,T.PROSP_ID,T.PROSPECT_TYPE PROSP_TYPE ");
                sb.AppendFormat("     ,E.FIRST_NAME+' '+E.LAST_NAME SALE_REP_NAME,EMP_ID SALE_REP_ID ");
                sb.AppendFormat("     ,T.PLAN_TRIP_ID VISIT_PLAN_ID,T.PLAN_TRIP_NAME VISIT_PLAN_NAME ");
                sb.AppendFormat("     ,IIF(T.ADHOC_FLAG is not null,IIF(T.ADHOC_FLAG='Y','Adhoc','Plan'),'') VISIT_TYPE ");
                sb.AppendFormat("     ,FORMAT(DATEADD(yy,543,T.PLAN_TRIP_DATE),'dd/MM/yyyy') VISIT_DATE,T.PLAN_TRIP_DATE ");
                sb.AppendFormat("     ,LOC_NAME ACC_NAME ");
                sb.AppendFormat("     ,CASE T.PROSPECT_TYPE ");
                sb.AppendFormat("       WHEN '0' THEN 'Prospect' ");
                sb.AppendFormat("       WHEN '1' THEN 'Pump' ");
                sb.AppendFormat("       WHEN '2' THEN 'Customer' ");
                sb.AppendFormat("       ELSE '' ");
                sb.AppendFormat("       END PROSPECT_TYPE ");
                sb.AppendFormat("     ,lag(T.CHECKIN_MILE_NO) over (PARTITION BY T.PLAN_TRIP_ID order by  T.PLAN_TRIP_ID,T.seq,T.ORDER_NO) as START_MILE_NO ");
                sb.AppendFormat("     ,T.CHECKIN_MILE_NO FINISH_MILE_NO ");
                sb.AppendFormat("     ,T.CHECKIN_MILE_NO - lag(T.CHECKIN_MILE_NO) over (PARTITION BY T.PLAN_TRIP_ID order by  T.PLAN_TRIP_ID,T.seq,T.ORDER_NO) as TOTAL_KM_INPUT ");
                sb.AppendFormat("     ,T.VISIT_CALC_KM TOTAL_KM_SYSTEM ");
                sb.AppendFormat("     ,FORMAT(DATEADD(yy,543,T.PLAN_START_TIME),'dd/MM/yyyy HH:mm') PLAN_START_TIME ");
                sb.AppendFormat("     ,FORMAT(DATEADD(yy,543,T.PLAN_END_TIME),'dd/MM/yyyy HH:mm') PLAN_END_TIME ");
                sb.AppendFormat("     ,FORMAT(DATEADD(yy,543,T.CHECKIN_DTM),'dd/MM/yyyy HH:mm') VISIT_CHECKIN_DTM ");
                sb.AppendFormat("     ,FORMAT(DATEADD(yy,543,T.CHECKOUT_DTM),'dd/MM/yyyy HH:mm') VISIT_CHECKOUT_DTM ");
                sb.AppendFormat("     ,T.REASON_NAME_TH ");
                sb.AppendFormat("     from ( ");
                sb.AppendFormat("         select 0 seq,0 ORDER_NO,PLAN_TRIP_ID,PLAN_TRIP_NAME,PLAN_TRIP_DATE,ASSIGN_EMP_ID,null PROSP_ID,null PROSPECT_TYPE ");
                sb.AppendFormat("         ,null CHECKIN_DTM,START_CHECKIN_DTM CHECKOUT_DTM,START_CHECKIN_MILE_NO CHECKIN_MILE_NO ");
                sb.AppendFormat("         ,L.LOC_NAME_TH LOC_NAME,T.CREATE_USER ");
                sb.AppendFormat("         ,null ADHOC_FLAG,null VISIT_CALC_KM,null PLAN_START_TIME,null PLAN_END_TIME,null REASON_NAME_TH ");
                sb.AppendFormat("         from PLAN_TRIP T ");
                sb.AppendFormat("         left join MS_LOCATION L on L.LOC_ID = T.START_CHECKIN_LOC_ID ");
                sb.AppendFormat("         union all ");
                //sb.AppendFormat("         select 1 seq,TP.ORDER_NO,TP.PLAN_TRIP_ID,PT.PLAN_TRIP_NAME,PT.PLAN_TRIP_DATE,PT.ASSIGN_EMP_ID,TP.PROSP_ID,PP.PROSPECT_TYPE ");
                sb.AppendFormat("         select 1 seq ");
                sb.AppendFormat("         ,row_number() over (PARTITION BY TP.PLAN_TRIP_ID order by isnull(TP.VISIT_CHECKOUT_DTM,TP.UPDATE_DTM)) ORDER_NO ");
                sb.AppendFormat("         ,TP.PLAN_TRIP_ID,PT.PLAN_TRIP_NAME,PT.PLAN_TRIP_DATE,PT.ASSIGN_EMP_ID,TP.PROSP_ID,PP.PROSPECT_TYPE ");
                sb.AppendFormat("         ,VISIT_CHECKIN_DTM CHECKIN_DTM,VISIT_CHECKOUT_DTM CHECKOUT_DTM ");
                //sb.AppendFormat("         ,CHECKIN_MILE_NO = LAST_VALUE(VISIT_CHECKIN_MILE_NO) IGNORE NULLS OVER (PARTITION BY TP.PLAN_TRIP_ID ORDER BY ORDER_NO) ");
                //sb.AppendFormat("         ,CHECKIN_MILE_NO = ISNULL(LAST_VALUE(VISIT_CHECKIN_MILE_NO) IGNORE NULLS OVER (PARTITION BY TP.PLAN_TRIP_ID ORDER BY ORDER_NO),PT.START_CHECKIN_MILE_NO) ");
                sb.AppendFormat("         ,CHECKIN_MILE_NO = ISNULL(LAST_VALUE(VISIT_CHECKIN_MILE_NO) IGNORE NULLS OVER (PARTITION BY TP.PLAN_TRIP_ID ORDER BY isnull(TP.VISIT_CHECKOUT_DTM,TP.UPDATE_DTM)),PT.START_CHECKIN_MILE_NO) ");
                sb.AppendFormat("         ,ISNULL(AC.ACC_NAME,L.LOC_NAME_TH) LOC_NAME,PT.CREATE_USER ");
                sb.AppendFormat("         ,TP.ADHOC_FLAG,TP.VISIT_CALC_KM,TP.PLAN_START_TIME,TP.PLAN_END_TIME,R.REASON_NAME_TH ");
                sb.AppendFormat("         from PLAN_TRIP_PROSPECT TP ");
                sb.AppendFormat("         inner join PLAN_TRIP PT on TP.PLAN_TRIP_ID = PT.PLAN_TRIP_ID ");
                sb.AppendFormat("         left join PROSPECT PP on PP.PROSPECT_ID = TP.PROSP_ID ");
                sb.AppendFormat("         left join PROSPECT_ACCOUNT AC on AC.PROSP_ACC_ID = PP.PROSP_ACC_ID ");
                sb.AppendFormat("         left join MS_LOCATION L on L.LOC_ID = TP.LOC_ID ");
                sb.AppendFormat("         left join PLAN_REASON_NOT_VISIT R on R.REASON_NOT_VISIT_ID = TP.REASON_NOT_VISIT_ID ");
                sb.AppendFormat("         union all ");
                sb.AppendFormat("         select 2 seq,0 ORDER_NO,PLAN_TRIP_ID,PLAN_TRIP_NAME,PLAN_TRIP_DATE,ASSIGN_EMP_ID,null PROSP_ID,null PROSPECT_TYPE ");
                sb.AppendFormat("         ,STOP_CHECKIN_DTM CHECKIN_DTM,null CHECKOUT_DTM,STOP_CHECKIN_MILE_NO CHECKIN_MILE_NO ");
                sb.AppendFormat("         ,L.LOC_NAME_TH LOC_NAME,T.CREATE_USER ");
                sb.AppendFormat("         ,null ADHOC_FLAG,STOP_CALC_KM VISIT_CALC_KM,null PLAN_START_TIME,null PLAN_END_TIME,null REASON_NAME_TH ");
                sb.AppendFormat("         from PLAN_TRIP T ");
                sb.AppendFormat("         left join MS_LOCATION L on L.LOC_ID = T.STOP_CHECKIN_LOC_ID ");
                sb.AppendFormat("     ) T inner join ADM_EMPLOYEE E on E.EMP_ID = IIF(ISNULL(T.ASSIGN_EMP_ID,'')='',T.CREATE_USER,T.ASSIGN_EMP_ID) ");
                sb.AppendFormat(" ) ");
                sb.AppendFormat(" SELECT R.* ");
                sb.AppendFormat(" FROM R ");
                sb.AppendFormat(" where R.seq != 0 ");

                ReportCriteria c = searchCriteria.model;
                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat(" and FORMAT(R.PLAN_TRIP_DATE,'yyyyMMdd') >= @startDate  ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat(" and FORMAT(R.PLAN_TRIP_DATE,'yyyyMMdd') <= @endDate  ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                    if (!String.IsNullOrEmpty(c.saleRepId))
                    {
                        sb.AppendFormat(" and R.SALE_REP_ID = @saleRepId  ");
                        QueryUtils.addParam(command, "saleRepId", c.saleRepId);
                    }
                    if (!String.IsNullOrEmpty(c.prospectId))
                    {
                        sb.AppendFormat(" and R.PROSP_ID  = @prospectId  ");
                        QueryUtils.addParam(command, "prospectId", c.prospectId);
                    }
                    if (!String.IsNullOrEmpty(c.prospectType))
                    {
                        sb.AppendFormat(" and R.PROSP_TYPE = @prospectType  ");
                        QueryUtils.addParam(command, "prospectType", c.prospectType);
                    }
                }
                // sb.AppendFormat(" ORDER BY R.PLAN_TRIP_DATE,R.SALE_REP_ID,VISIT_PLAN_NAME ");
                sb.AppendFormat(" order by R.PLAN_TRIP_DATE,R.VISIT_PLAN_ID,R.seq,R.ORDER_NO ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {                    
                    searchResult.records = new List<Rep01VisitPlanRawDataResult>();

                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        Rep01VisitPlanRawDataResult item = new Rep01VisitPlanRawDataResult();
                        item.prospId = QueryUtils.getValueAsString(record, "PROSP_ID");
                        item.saleRepName = QueryUtils.getValueAsString(record, "SALE_REP_NAME");
                        item.saleRepId = QueryUtils.getValueAsString(record, "SALE_REP_ID");
                        item.visitPlanId = QueryUtils.getValueAsString(record, "VISIT_PLAN_ID");
                        item.visitPlanName = QueryUtils.getValueAsString(record, "VISIT_PLAN_NAME");
                        item.visitType = QueryUtils.getValueAsString(record, "VISIT_TYPE");
                        item.visitDate = QueryUtils.getValueAsString(record, "VISIT_DATE");
                        item.accName = QueryUtils.getValueAsString(record, "ACC_NAME");
                        item.prospectType = QueryUtils.getValueAsString(record, "PROSPECT_TYPE");
                        item.startMileNo = QueryUtils.getValueAsString(record, "START_MILE_NO");
                        item.finishMileNo = QueryUtils.getValueAsString(record, "FINISH_MILE_NO");
                        item.totalKmInput = QueryUtils.getValueAsString(record, "TOTAL_KM_INPUT");
                        item.totalKmSystem = QueryUtils.getValueAsString(record, "TOTAL_KM_SYSTEM");
                        item.planStartTime = QueryUtils.getValueAsString(record, "PLAN_START_TIME");
                        item.planEndTime = QueryUtils.getValueAsString(record, "PLAN_END_TIME");
                        item.visitCheckinDtm = QueryUtils.getValueAsString(record, "VISIT_CHECKIN_DTM");
                        item.visitCheckoutDtm = QueryUtils.getValueAsString(record, "VISIT_CHECKOUT_DTM");
                        item.reasonNameTh = QueryUtils.getValueAsString(record, "REASON_NAME_TH");

                        if (!String.IsNullOrEmpty(item.totalKmSystem) && item.totalKmSystem.Contains("."))
                        {
                            item.totalKmSystem = item.totalKmSystem.Split(".")[0];
                        }


                        searchResult.records.Add(item);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    
                    searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;
                    
                }

            }

            return searchResult;
        }

        public async Task<SearchResultBase<Rep16AssignResult>> SearchRep16Assign(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep16AssignResult> searchResult = new SearchResultBase<Rep16AssignResult>();
            Rep16AssignResult result = new Rep16AssignResult();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                ReportCriteria c = searchCriteria.model;

                context.Database.OpenConnection();

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(" select  SF.TP_CODE,SF.TP_NAME_TH ,(select count(1) from TEMPLATE_SA_TITLE where ISNULL(ANS_VAL_TYPE,0) != 4 and TP_SA_FORM_ID = SF.TP_SA_FORM_ID) TOTAL_COLM ");
                sb.AppendFormat(" from TEMPLATE_SA_FORM SF ");
                sb.AppendFormat(" where SF.TP_SA_FORM_ID = @tpSaFormId ");
                QueryUtils.addParam(command, "tpSaFormId", c.tpSaFormId);

                command.CommandText = sb.ToString();
                log.Debug("Query Template:" + sb.ToString());
                Console.WriteLine("Query Template:" + sb.ToString());
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        result.tpCode = QueryUtils.getValueAsString(record, "TP_CODE");
                        result.tpNameTh = QueryUtils.getValueAsString(record, "TP_NAME_TH");
                        result.totalColm = QueryUtils.getValueAsString(record, "TOTAL_COLM");

                    }
                    reader.Close();
                }

                sb = new StringBuilder();
                sb.AppendFormat(" WITH H AS ( ");
                sb.AppendFormat(" SELECT  ");
                sb.AppendFormat(" [1] as 'TITLE_COLM_NO1',[2] as 'TITLE_COLM_NO2',[3] as 'TITLE_COLM_NO3',[4] as 'TITLE_COLM_NO4',[5] as 'TITLE_COLM_NO5',[6] as 'TITLE_COLM_NO6',[7] as 'TITLE_COLM_NO7',[8] as 'TITLE_COLM_NO8',[9] as 'TITLE_COLM_NO9',[10] as 'TITLE_COLM_NO10',[11] as 'TITLE_COLM_NO11',[12] as 'TITLE_COLM_NO12',[13] as 'TITLE_COLM_NO13',[14] as 'TITLE_COLM_NO14',[15] as 'TITLE_COLM_NO15',[16] as 'TITLE_COLM_NO16',[17] as 'TITLE_COLM_NO17',[18] as 'TITLE_COLM_NO18',[19] as 'TITLE_COLM_NO19',[20] as 'TITLE_COLM_NO20',[21] as 'TITLE_COLM_NO21',[22] as 'TITLE_COLM_NO22',[23] as 'TITLE_COLM_NO23',[24] as 'TITLE_COLM_NO24',[25] as 'TITLE_COLM_NO25',[26] as 'TITLE_COLM_NO26',[27] as 'TITLE_COLM_NO27',[28] as 'TITLE_COLM_NO28',[29] as 'TITLE_COLM_NO29',[30] as 'TITLE_COLM_NO30',[31] as 'TITLE_COLM_NO31',[32] as 'TITLE_COLM_NO32',[33] as 'TITLE_COLM_NO33',[34] as 'TITLE_COLM_NO34',[35] as 'TITLE_COLM_NO35',[36] as 'TITLE_COLM_NO36',[37] as 'TITLE_COLM_NO37',[38] as 'TITLE_COLM_NO38',[39] as 'TITLE_COLM_NO39',[40] as 'TITLE_COLM_NO40',[41] as 'TITLE_COLM_NO41',[42] as 'TITLE_COLM_NO42',[43] as 'TITLE_COLM_NO43',[44] as 'TITLE_COLM_NO44',[45] as 'TITLE_COLM_NO45',[46] as 'TITLE_COLM_NO46',[47] as 'TITLE_COLM_NO47',[48] as 'TITLE_COLM_NO48',[49] as 'TITLE_COLM_NO49',[50] as 'TITLE_COLM_NO50' ");
                sb.AppendFormat(" FROM   ");
                sb.AppendFormat(" ( ");
                sb.AppendFormat("   SELECT TITLE_NAME_TH,TITLE_COLM_NO ");
                sb.AppendFormat("   FROM TEMPLATE_SA_TITLE ");
                sb.AppendFormat("   where ISNULL(ANS_VAL_TYPE,0) != 4 and TP_SA_FORM_ID = @tpSaFormId ");
                sb.AppendFormat(" ) AS SourceTable   ");
                sb.AppendFormat(" PIVOT   ");
                sb.AppendFormat(" (   ");
                sb.AppendFormat("   max(TITLE_NAME_TH)   ");
                sb.AppendFormat("   FOR TITLE_COLM_NO IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31],[32],[33],[34],[35],[36],[37],[38],[39],[40],[41],[42],[43],[44],[45],[46],[47],[48],[49],[50])   ");
                sb.AppendFormat(" ) AS PivotTable ");
                sb.AppendFormat(" ), T AS ( ");
                sb.AppendFormat(" SELECT  ");
                sb.AppendFormat(" [1] as 'TYPE_COLM_NO1',[2] as 'TYPE_COLM_NO2',[3] as 'TYPE_COLM_NO3',[4] as 'TYPE_COLM_NO4',[5] as 'TYPE_COLM_NO5',[6] as 'TYPE_COLM_NO6',[7] as 'TYPE_COLM_NO7',[8] as 'TYPE_COLM_NO8',[9] as 'TYPE_COLM_NO9',[10] as 'TYPE_COLM_NO10',[11] as 'TYPE_COLM_NO11',[12] as 'TYPE_COLM_NO12',[13] as 'TYPE_COLM_NO13',[14] as 'TYPE_COLM_NO14',[15] as 'TYPE_COLM_NO15',[16] as 'TYPE_COLM_NO16',[17] as 'TYPE_COLM_NO17',[18] as 'TYPE_COLM_NO18',[19] as 'TYPE_COLM_NO19',[20] as 'TYPE_COLM_NO20',[21] as 'TYPE_COLM_NO21',[22] as 'TYPE_COLM_NO22',[23] as 'TYPE_COLM_NO23',[24] as 'TYPE_COLM_NO24',[25] as 'TYPE_COLM_NO25',[26] as 'TYPE_COLM_NO26',[27] as 'TYPE_COLM_NO27',[28] as 'TYPE_COLM_NO28',[29] as 'TYPE_COLM_NO29',[30] as 'TYPE_COLM_NO30',[31] as 'TYPE_COLM_NO31',[32] as 'TYPE_COLM_NO32',[33] as 'TYPE_COLM_NO33',[34] as 'TYPE_COLM_NO34',[35] as 'TYPE_COLM_NO35',[36] as 'TYPE_COLM_NO36',[37] as 'TYPE_COLM_NO37',[38] as 'TYPE_COLM_NO38',[39] as 'TYPE_COLM_NO39',[40] as 'TYPE_COLM_NO40',[41] as 'TYPE_COLM_NO41',[42] as 'TYPE_COLM_NO42',[43] as 'TYPE_COLM_NO43',[44] as 'TYPE_COLM_NO44',[45] as 'TYPE_COLM_NO45',[46] as 'TYPE_COLM_NO46',[47] as 'TYPE_COLM_NO47',[48] as 'TYPE_COLM_NO48',[49] as 'TYPE_COLM_NO49',[50] as 'TYPE_COLM_NO50' ");
                sb.AppendFormat(" FROM   ");
                sb.AppendFormat(" ( ");
                sb.AppendFormat("   SELECT ANS_TYPE,TITLE_COLM_NO ");
                sb.AppendFormat("   FROM TEMPLATE_SA_TITLE ");
                sb.AppendFormat("   where ISNULL(ANS_VAL_TYPE,0) != 4 and TP_SA_FORM_ID = @tpSaFormId ");
                sb.AppendFormat(" ) AS SourceTable   ");
                sb.AppendFormat(" PIVOT   ");
                sb.AppendFormat(" (   ");
                sb.AppendFormat("   max(ANS_TYPE)   ");
                sb.AppendFormat("   FOR TITLE_COLM_NO IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31],[32],[33],[34],[35],[36],[37],[38],[39],[40],[41],[42],[43],[44],[45],[46],[47],[48],[49],[50])   ");
                sb.AppendFormat(" ) AS PivotTable ");
                sb.AppendFormat(" ), V AS ( ");
                sb.AppendFormat(" SELECT  ");
                sb.AppendFormat(" [1] as 'VAL_COLM_NO1',[2] as 'VAL_COLM_NO2',[3] as 'VAL_COLM_NO3',[4] as 'VAL_COLM_NO4',[5] as 'VAL_COLM_NO5',[6] as 'VAL_COLM_NO6',[7] as 'VAL_COLM_NO7',[8] as 'VAL_COLM_NO8',[9] as 'VAL_COLM_NO9',[10] as 'VAL_COLM_NO10',[11] as 'VAL_COLM_NO11',[12] as 'VAL_COLM_NO12',[13] as 'VAL_COLM_NO13',[14] as 'VAL_COLM_NO14',[15] as 'VAL_COLM_NO15',[16] as 'VAL_COLM_NO16',[17] as 'VAL_COLM_NO17',[18] as 'VAL_COLM_NO18',[19] as 'VAL_COLM_NO19',[20] as 'VAL_COLM_NO20',[21] as 'VAL_COLM_NO21',[22] as 'VAL_COLM_NO22',[23] as 'VAL_COLM_NO23',[24] as 'VAL_COLM_NO24',[25] as 'VAL_COLM_NO25',[26] as 'VAL_COLM_NO26',[27] as 'VAL_COLM_NO27',[28] as 'VAL_COLM_NO28',[29] as 'VAL_COLM_NO29',[30] as 'VAL_COLM_NO30',[31] as 'VAL_COLM_NO31',[32] as 'VAL_COLM_NO32',[33] as 'VAL_COLM_NO33',[34] as 'VAL_COLM_NO34',[35] as 'VAL_COLM_NO35',[36] as 'VAL_COLM_NO36',[37] as 'VAL_COLM_NO37',[38] as 'VAL_COLM_NO38',[39] as 'VAL_COLM_NO39',[40] as 'VAL_COLM_NO40',[41] as 'VAL_COLM_NO41',[42] as 'VAL_COLM_NO42',[43] as 'VAL_COLM_NO43',[44] as 'VAL_COLM_NO44',[45] as 'VAL_COLM_NO45',[46] as 'VAL_COLM_NO46',[47] as 'VAL_COLM_NO47',[48] as 'VAL_COLM_NO48',[49] as 'VAL_COLM_NO49',[50] as 'VAL_COLM_NO50' ");
                sb.AppendFormat(" FROM   ");
                sb.AppendFormat(" ( ");
                sb.AppendFormat("   SELECT IIF(ANS_TYPE = 'V',ANS_VAL_TYPE,ANS_LOV_TYPE) VAL_TYPE,TITLE_COLM_NO ");
                sb.AppendFormat("   FROM TEMPLATE_SA_TITLE ");
                sb.AppendFormat("   where ISNULL(ANS_VAL_TYPE,0) != 4 and TP_SA_FORM_ID = @tpSaFormId ");
                sb.AppendFormat(" ) AS SourceTable   ");
                sb.AppendFormat(" PIVOT   ");
                sb.AppendFormat(" (   ");
                sb.AppendFormat("   max(VAL_TYPE)   ");
                sb.AppendFormat("   FOR TITLE_COLM_NO IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31],[32],[33],[34],[35],[36],[37],[38],[39],[40],[41],[42],[43],[44],[45],[46],[47],[48],[49],[50])   ");
                sb.AppendFormat(" ) AS PivotTable ");
                sb.AppendFormat(" ) ");
                sb.AppendFormat(" select NULL CREATE_DTM,'H' COLM_TYPE,H.* ");
                sb.AppendFormat(" from H ");
                sb.AppendFormat(" union all ");
                sb.AppendFormat(" select CREATE_DTM,'B' COLM_TYPE ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO1='V',TITLE_COLM_NO1,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO1,TITLE_COLM_NO1)) TITLE_COLM_NO1 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO2='V',TITLE_COLM_NO2,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO2,TITLE_COLM_NO2)) TITLE_COLM_NO2 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO3='V',TITLE_COLM_NO3,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO3,TITLE_COLM_NO3)) TITLE_COLM_NO3 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO4='V',TITLE_COLM_NO4,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO4,TITLE_COLM_NO4)) TITLE_COLM_NO4 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO5='V',TITLE_COLM_NO5,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO5,TITLE_COLM_NO5)) TITLE_COLM_NO5 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO6='V',TITLE_COLM_NO6,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO6,TITLE_COLM_NO6)) TITLE_COLM_NO6 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO7='V',TITLE_COLM_NO7,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO7,TITLE_COLM_NO7)) TITLE_COLM_NO7 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO8='V',TITLE_COLM_NO8,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO8,TITLE_COLM_NO8)) TITLE_COLM_NO8 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO9='V',TITLE_COLM_NO9,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO9,TITLE_COLM_NO9)) TITLE_COLM_NO9 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO10='V',TITLE_COLM_NO10,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO10,TITLE_COLM_NO10)) TITLE_COLM_NO10 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO11='V',TITLE_COLM_NO11,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO11,TITLE_COLM_NO11)) TITLE_COLM_NO11 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO12='V',TITLE_COLM_NO12,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO12,TITLE_COLM_NO12)) TITLE_COLM_NO12 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO13='V',TITLE_COLM_NO13,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO13,TITLE_COLM_NO13)) TITLE_COLM_NO13 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO14='V',TITLE_COLM_NO14,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO14,TITLE_COLM_NO14)) TITLE_COLM_NO14 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO15='V',TITLE_COLM_NO15,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO15,TITLE_COLM_NO15)) TITLE_COLM_NO15 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO16='V',TITLE_COLM_NO16,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO16,TITLE_COLM_NO16)) TITLE_COLM_NO16 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO17='V',TITLE_COLM_NO17,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO17,TITLE_COLM_NO17)) TITLE_COLM_NO17 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO18='V',TITLE_COLM_NO18,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO18,TITLE_COLM_NO18)) TITLE_COLM_NO18 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO19='V',TITLE_COLM_NO19,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO19,TITLE_COLM_NO19)) TITLE_COLM_NO19 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO20='V',TITLE_COLM_NO20,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO20,TITLE_COLM_NO20)) TITLE_COLM_NO20 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO21='V',TITLE_COLM_NO21,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO21,TITLE_COLM_NO21)) TITLE_COLM_NO21 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO22='V',TITLE_COLM_NO22,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO22,TITLE_COLM_NO22)) TITLE_COLM_NO22 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO23='V',TITLE_COLM_NO23,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO23,TITLE_COLM_NO23)) TITLE_COLM_NO23 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO24='V',TITLE_COLM_NO24,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO24,TITLE_COLM_NO24)) TITLE_COLM_NO24 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO25='V',TITLE_COLM_NO25,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO25,TITLE_COLM_NO25)) TITLE_COLM_NO25 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO26='V',TITLE_COLM_NO26,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO26,TITLE_COLM_NO26)) TITLE_COLM_NO26 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO27='V',TITLE_COLM_NO27,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO27,TITLE_COLM_NO27)) TITLE_COLM_NO27 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO28='V',TITLE_COLM_NO28,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO28,TITLE_COLM_NO28)) TITLE_COLM_NO28 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO29='V',TITLE_COLM_NO29,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO29,TITLE_COLM_NO29)) TITLE_COLM_NO29 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO30='V',TITLE_COLM_NO30,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO30,TITLE_COLM_NO30)) TITLE_COLM_NO30 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO31='V',TITLE_COLM_NO31,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO31,TITLE_COLM_NO31)) TITLE_COLM_NO31 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO32='V',TITLE_COLM_NO32,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO32,TITLE_COLM_NO32)) TITLE_COLM_NO32 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO33='V',TITLE_COLM_NO33,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO33,TITLE_COLM_NO33)) TITLE_COLM_NO33 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO34='V',TITLE_COLM_NO34,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO34,TITLE_COLM_NO34)) TITLE_COLM_NO34 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO35='V',TITLE_COLM_NO35,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO35,TITLE_COLM_NO35)) TITLE_COLM_NO35 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO36='V',TITLE_COLM_NO36,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO36,TITLE_COLM_NO36)) TITLE_COLM_NO36 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO37='V',TITLE_COLM_NO37,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO37,TITLE_COLM_NO37)) TITLE_COLM_NO37 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO38='V',TITLE_COLM_NO38,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO38,TITLE_COLM_NO38)) TITLE_COLM_NO38 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO39='V',TITLE_COLM_NO39,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO39,TITLE_COLM_NO39)) TITLE_COLM_NO39 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO40='V',TITLE_COLM_NO40,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO40,TITLE_COLM_NO40)) TITLE_COLM_NO40 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO41='V',TITLE_COLM_NO41,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO41,TITLE_COLM_NO41)) TITLE_COLM_NO41 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO42='V',TITLE_COLM_NO42,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO42,TITLE_COLM_NO42)) TITLE_COLM_NO42 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO43='V',TITLE_COLM_NO43,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO43,TITLE_COLM_NO43)) TITLE_COLM_NO43 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO44='V',TITLE_COLM_NO44,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO44,TITLE_COLM_NO44)) TITLE_COLM_NO44 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO45='V',TITLE_COLM_NO45,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO45,TITLE_COLM_NO45)) TITLE_COLM_NO45 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO46='V',TITLE_COLM_NO46,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO46,TITLE_COLM_NO46)) TITLE_COLM_NO46 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO47='V',TITLE_COLM_NO47,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO47,TITLE_COLM_NO47)) TITLE_COLM_NO47 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO48='V',TITLE_COLM_NO48,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO48,TITLE_COLM_NO48)) TITLE_COLM_NO48 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO49='V',TITLE_COLM_NO49,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO49,TITLE_COLM_NO49)) TITLE_COLM_NO49 ");
                sb.AppendFormat(" ,IIF(T.TYPE_COLM_NO50='V',TITLE_COLM_NO50,dbo.GET_VALUE_MASTER(V.VAL_COLM_NO50,TITLE_COLM_NO50)) TITLE_COLM_NO50 ");
                sb.AppendFormat(" from RECORD_SA_FORM,T,V ");
                sb.AppendFormat(" where TP_SA_FORM_ID = @tpSaFormId ");

                if (!String.IsNullOrEmpty(c.startDate))
                {
                    sb.AppendFormat(" and FORMAT(CREATE_DTM,'yyyyMMdd') >= @startDate  ");
                    QueryUtils.addParam(command, "startDate", c.startDate);
                }
                if (!String.IsNullOrEmpty(c.endDate))
                {
                    sb.AppendFormat(" and FORMAT(CREATE_DTM,'yyyyMMdd') <= @endDate  ");
                    QueryUtils.addParam(command, "endDate", c.endDate);
                }
                if (!String.IsNullOrEmpty(c.saleRepId))
                {
                    sb.AppendFormat(" and CREATE_USER = @saleRepId  ");
                    QueryUtils.addParam(command, "saleRepId", c.saleRepId);
                }
                sb.AppendFormat(" order by COLM_TYPE DESC,CREATE_DTM ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    sb.AppendFormat(" OFFSET (@Length+1)  * (@PageNo - 1) ROWS ");
                    sb.AppendFormat(" FETCH NEXT (@Length+1) ROWS ONLY ");
                    QueryUtils.addParam(command, "Length", searchCriteria.length);
                    QueryUtils.addParam(command, "PageNo", searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    result.bodyColumn = new List<Rep16AssignResult.ColumnData>();
                    
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        Rep16AssignResult.ColumnData item = new Rep16AssignResult.ColumnData();
                        item.titleColmNo1 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO1");
                        item.titleColmNo2 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO2");
                        item.titleColmNo3 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO3");
                        item.titleColmNo4 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO4");
                        item.titleColmNo5 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO5");
                        item.titleColmNo6 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO6");
                        item.titleColmNo7 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO7");
                        item.titleColmNo8 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO8");
                        item.titleColmNo9 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO9");
                        item.titleColmNo10 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO10");
                        item.titleColmNo11 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO11");
                        item.titleColmNo12 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO12");
                        item.titleColmNo13 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO13");
                        item.titleColmNo14 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO14");
                        item.titleColmNo15 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO15");
                        item.titleColmNo16 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO16");
                        item.titleColmNo17 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO17");
                        item.titleColmNo18 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO18");
                        item.titleColmNo19 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO19");
                        item.titleColmNo20 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO20");
                        item.titleColmNo21 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO21");
                        item.titleColmNo22 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO22");
                        item.titleColmNo23 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO23");
                        item.titleColmNo24 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO24");
                        item.titleColmNo25 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO25");
                        item.titleColmNo26 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO26");
                        item.titleColmNo27 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO27");
                        item.titleColmNo28 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO28");
                        item.titleColmNo29 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO29");
                        item.titleColmNo30 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO30");
                        item.titleColmNo31 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO31");
                        item.titleColmNo32 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO32");
                        item.titleColmNo33 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO33");
                        item.titleColmNo34 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO34");
                        item.titleColmNo35 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO35");
                        item.titleColmNo36 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO36");
                        item.titleColmNo37 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO37");
                        item.titleColmNo38 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO38");
                        item.titleColmNo39 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO39");
                        item.titleColmNo40 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO40");
                        item.titleColmNo41 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO41");
                        item.titleColmNo42 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO42");
                        item.titleColmNo43 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO43");
                        item.titleColmNo44 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO44");
                        item.titleColmNo45 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO45");
                        item.titleColmNo46 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO46");
                        item.titleColmNo47 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO47");
                        item.titleColmNo48 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO48");
                        item.titleColmNo49 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO49");
                        item.titleColmNo50 = QueryUtils.getValueAsString(record, "TITLE_COLM_NO50");

                        if (QueryUtils.getValueAsString(record, "COLM_TYPE").Equals("H")){
                            result.headColumn = item;
                        } else {
                            result.bodyColumn.Add(item);
                        }

                    }
                    
                    reader.Close();

                    searchResult.records = new List<Rep16AssignResult>();
                    searchResult.records.Add(result);

                    searchResult.totalRecords = searchCriteria.length == 0 ? result.bodyColumn == null ? 0 : result.bodyColumn.Count() : (count_-1);

                }

            }

            return searchResult;
        }

        public async Task<SearchResultBase<Rep03VisitPlanDailyResult>> Search03VisitPlanDaily(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep03VisitPlanDailyResult> searchResult = new SearchResultBase<Rep03VisitPlanDailyResult>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(" select FORMAT(DATEADD(yy,543,T.PLAN_TRIP_DATE),'dd/MM/yyyy') VISIT_DATE ");
                sb.AppendFormat(" ,CAST(T.START_CHECKIN_MILE_NO as varchar) START_CHECKIN_MILE_NO ");
                sb.AppendFormat(" ,ISNULL(CAST(T.STOP_CHECKIN_MILE_NO as varchar),'') STOP_CHECKIN_MILE_NO ");
                sb.AppendFormat(" ,ISNULL(CAST(T.STOP_CHECKIN_MILE_NO - T.START_CHECKIN_MILE_NO as varchar),'') TOTAL_KM_INPUT, ISNULL(T.STOP_CHECKIN_MILE_NO - T.START_CHECKIN_MILE_NO,0) TOTAL_KM_INPUT_FOR_CALC ");
                //sb.AppendFormat(" ,ISNULL(CAST(T.STOP_CALC_KM as varchar),'') TOTAL_KM_SYSTEM, ISNULL(T.STOP_CALC_KM,0) TOTAL_KM_SYSTEM_FOR_CALC ");
                sb.AppendFormat(" ,ISNULL(CAST(((SELECT SUM(TP.VISIT_CALC_KM) FROM PLAN_TRIP_PROSPECT TP WHERE TP.PLAN_TRIP_ID = T.PLAN_TRIP_ID) + T.STOP_CALC_KM) as varchar),'') TOTAL_KM_SYSTEM ");
                sb.AppendFormat(" ,ISNULL(((SELECT SUM(TP.VISIT_CALC_KM) FROM PLAN_TRIP_PROSPECT TP WHERE TP.PLAN_TRIP_ID = T.PLAN_TRIP_ID) + T.STOP_CALC_KM),0) TOTAL_KM_SYSTEM_FOR_CALC ");
                //sb.AppendFormat(" ,LS.LOC_NAME_TH + ' - ' + ISNULL(LE.LOC_NAME_TH,'') LOCATION_NAME ");
                sb.AppendFormat(" ,LS.LOC_NAME_TH + ', ' + TP.LOCATION_NAME + ', ' + ISNULL(LE.LOC_NAME_TH,'') LOCATION_NAME ");
                sb.AppendFormat(" ,IIF(T.STOP_CALC_KM IS NULL,'Incomplete','Complete') STATUS ");
                sb.AppendFormat(" from PLAN_TRIP T ");
                sb.AppendFormat(" inner join ( ");
                sb.AppendFormat("     SELECT TP.PLAN_TRIP_ID, STRING_AGG(ISNULL(AC.ACC_NAME,L.LOC_NAME_TH),', ') WITHIN GROUP ( ORDER BY TP.VISIT_CHECKIN_DTM ASC)  AS LOCATION_NAME ");
                sb.AppendFormat("     FROM PLAN_TRIP_PROSPECT TP ");
                sb.AppendFormat("     left join PROSPECT PP on PP.PROSPECT_ID = TP.PROSP_ID ");
                sb.AppendFormat("     left join PROSPECT_ACCOUNT AC on AC.PROSP_ACC_ID = PP.PROSP_ACC_ID ");
                sb.AppendFormat("     left join PLAN_REASON_NOT_VISIT R on R.REASON_NOT_VISIT_ID = TP.REASON_NOT_VISIT_ID ");
                sb.AppendFormat("     left join MS_LOCATION L on L.LOC_ID = TP.LOC_ID ");
                sb.AppendFormat("     GROUP BY TP.PLAN_TRIP_ID ");
                sb.AppendFormat(" ) TP on T.PLAN_TRIP_ID = TP.PLAN_TRIP_ID ");
                sb.AppendFormat(" inner join MS_LOCATION LS on LS.LOC_ID = T.START_CHECKIN_LOC_ID ");
                sb.AppendFormat(" left join MS_LOCATION LE on LE.LOC_ID = T.STOP_CHECKIN_LOC_ID ");
                sb.AppendFormat(" where 1=1 ");

                ReportCriteria c = searchCriteria.model;
                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat(" and FORMAT(T.PLAN_TRIP_DATE,'yyyyMMdd') >= @startDate  ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat(" and FORMAT(T.PLAN_TRIP_DATE,'yyyyMMdd') <= @endDate  ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                    if (!String.IsNullOrEmpty(c.saleRepId))
                    {
                        sb.AppendFormat(" and IIF(ISNULL(T.ASSIGN_EMP_ID,'')='',T.CREATE_USER,T.ASSIGN_EMP_ID) = @saleRepId  ");
                        QueryUtils.addParam(command, "saleRepId", c.saleRepId);
                    }
                }
                sb.AppendFormat(" ORDER BY T.PLAN_TRIP_DATE ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    searchResult.records = new List<Rep03VisitPlanDailyResult>();

                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        Rep03VisitPlanDailyResult item = new Rep03VisitPlanDailyResult();
                        item.visitDate = QueryUtils.getValueAsString(record, "VISIT_DATE");
                        item.startCheckinMileNo = QueryUtils.getValueAsString(record, "START_CHECKIN_MILE_NO");
                        item.stopCheckinMileNo = QueryUtils.getValueAsString(record, "STOP_CHECKIN_MILE_NO");
                        item.totalKmInput = QueryUtils.getValueAsString(record, "TOTAL_KM_INPUT");
                        item.totalKmSystem = QueryUtils.getValueAsString(record, "TOTAL_KM_SYSTEM");
                        item.locationName = QueryUtils.getValueAsString(record, "LOCATION_NAME");
                        item.status = QueryUtils.getValueAsString(record, "STATUS");
                        item.totalKmInputForCalc = QueryUtils.getValueAsString(record, "TOTAL_KM_INPUT_FOR_CALC");
                        item.totalKmSystemForCalc = QueryUtils.getValueAsString(record, "TOTAL_KM_SYSTEM_FOR_CALC");

                        if (!String.IsNullOrEmpty(item.totalKmSystem) && item.totalKmSystem.Contains("."))
                        {
                            item.totalKmSystem = item.totalKmSystem.Split(".")[0];
                        }


                        searchResult.records.Add(item);
                    }
                    // Call Close when done reading.
                    reader.Close();


                    searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;

                }

            }

            return searchResult;
        }

        public async Task<SearchResultBase<Rep04VisitPlanMonthlyResult>> Search04VisitPlanMonthly(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep04VisitPlanMonthlyResult> searchResult = new SearchResultBase<Rep04VisitPlanMonthlyResult>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder sb = new StringBuilder();
                /*
                sb.AppendFormat(" select FORMAT(DATEADD(yy,543,T.PLAN_TRIP_DATE),'dd/MM/yyyy') VISIT_DATE ");
                sb.AppendFormat(" ,FORMAT(T.START_CHECKIN_DTM ,'HH:mm') VISIT_TIME_START, T.START_CHECKIN_MILE_NO, LS.LOC_NAME_TH LOC_START_NAME ");
                sb.AppendFormat(" ,FORMAT(T.STOP_CHECKIN_DTM,'HH:mm') VISIT_TIME_STOP, T.STOP_CHECKIN_MILE_NO, LE.LOC_NAME_TH LOC_END_NAME ");
                sb.AppendFormat(" ,T.STOP_CHECKIN_MILE_NO - T.START_CHECKIN_MILE_NO TOTAL_KM_INPUT ");
                sb.AppendFormat(" ,'' CONTACT_NAME, '' ADDRESS, '' VISIT_DETAIL ");
                sb.AppendFormat(" from PLAN_TRIP T ");
                sb.AppendFormat(" inner join MS_LOCATION LS on LS.LOC_ID = T.START_CHECKIN_LOC_ID ");
                sb.AppendFormat(" inner join MS_LOCATION LE on LE.LOC_ID = T.STOP_CHECKIN_LOC_ID ");
                sb.AppendFormat(" where 1=1 ");
                */

                sb.AppendFormat("with T as ( ");
                sb.AppendFormat(" select T.seq,T.ORDER_NO,T.PLAN_TRIP_ID,T.PLAN_TRIP_DATE,T.ASSIGN_EMP_ID,T.CREATE_USER,T.CHECKIN_DTM ");
                sb.AppendFormat(" ,FORMAT(DATEADD(yy,543,T.CHECKIN_DTM),'dd/MM/yyyy') VISIT_DATE ");
                sb.AppendFormat(" ,FORMAT(T.CHECKIN_DTM ,'HH:mm') VISIT_TIME_START ");
                sb.AppendFormat(" ,T.CHECKIN_MILE_NO START_CHECKIN_MILE_NO ");
                sb.AppendFormat(" ,T.LOC_NAME LOC_START_NAME  ");
                sb.AppendFormat(" ,FORMAT(T.CHECKOUT_DTM ,'HH:mm') VISIT_TIME_STOP ");
                sb.AppendFormat(" ,lead(T.CHECKIN_MILE_NO) over (order by T.PLAN_TRIP_ID,T.seq,T.ORDER_NO) STOP_CHECKIN_MILE_NO ");
                sb.AppendFormat(" ,lead(T.LOC_NAME) over (order by T.PLAN_TRIP_ID,T.seq,T.ORDER_NO) LOC_END_NAME  ");
                sb.AppendFormat(" ,lead(T.CHECKIN_MILE_NO) over (order by T.PLAN_TRIP_ID,T.seq,T.ORDER_NO) - T.CHECKIN_MILE_NO TOTAL_KM_INPUT  ");
                sb.AppendFormat(" ,T.CONTACT_NAME,T.CONTACT_MOBILE_NO ADDRESS,T.CHECKOUT_REMARK VISIT_DETAIL  ");
                sb.AppendFormat(" from ( ");
                sb.AppendFormat("     select 0 seq,0 ORDER_NO,PLAN_TRIP_ID,PLAN_TRIP_DATE,ASSIGN_EMP_ID,null PROSP_ID,START_CHECKIN_DTM CHECKIN_DTM,null CHECKOUT_DTM,START_CHECKIN_MILE_NO CHECKIN_MILE_NO ");
                sb.AppendFormat("     ,L.LOC_NAME_TH LOC_NAME,T.CREATE_USER  ");
                sb.AppendFormat("     ,null contact_name,null contact_mobile_no,null checkout_remark ");
                sb.AppendFormat("     from PLAN_TRIP T ");
                sb.AppendFormat("     inner join MS_LOCATION L on L.LOC_ID = T.START_CHECKIN_LOC_ID  ");
                sb.AppendFormat("     union all ");
                //sb.AppendFormat("     select 1 seq,TP.ORDER_NO,TP.PLAN_TRIP_ID,PT.PLAN_TRIP_DATE,PT.ASSIGN_EMP_ID,TP.PROSP_ID,VISIT_CHECKIN_DTM CHECKIN_DTM,VISIT_CHECKOUT_DTM CHECKOUT_DTM ");
                sb.AppendFormat("     select 1 seq ");
                sb.AppendFormat("     ,row_number() over (PARTITION BY TP.PLAN_TRIP_ID order by isnull(TP.VISIT_CHECKOUT_DTM,TP.UPDATE_DTM)) ORDER_NO ");
                sb.AppendFormat("     ,TP.PLAN_TRIP_ID,PT.PLAN_TRIP_DATE,PT.ASSIGN_EMP_ID,TP.PROSP_ID,VISIT_CHECKIN_DTM CHECKIN_DTM,VISIT_CHECKOUT_DTM CHECKOUT_DTM ");
                //sb.AppendFormat("     ,CHECKIN_MILE_NO = LAST_VALUE(VISIT_CHECKIN_MILE_NO) IGNORE NULLS OVER (PARTITION BY TP.PLAN_TRIP_ID ORDER BY ORDER_NO) ");
                //sb.AppendFormat("     ,CHECKIN_MILE_NO = ISNULL(LAST_VALUE(VISIT_CHECKIN_MILE_NO) IGNORE NULLS OVER (PARTITION BY TP.PLAN_TRIP_ID ORDER BY ORDER_NO),PT.START_CHECKIN_MILE_NO) ");
                sb.AppendFormat("     ,CHECKIN_MILE_NO = ISNULL(LAST_VALUE(VISIT_CHECKIN_MILE_NO) IGNORE NULLS OVER (PARTITION BY TP.PLAN_TRIP_ID ORDER BY isnull(TP.VISIT_CHECKOUT_DTM,TP.UPDATE_DTM)),PT.START_CHECKIN_MILE_NO) ");
                sb.AppendFormat("     ,ISNULL(AC.ACC_NAME,L.LOC_NAME_TH) LOC_NAME,PT.CREATE_USER ");
                sb.AppendFormat("     ,TP.contact_name,TP.contact_mobile_no,TP.checkout_remark ");
                sb.AppendFormat("     from PLAN_TRIP_PROSPECT TP ");
                sb.AppendFormat("     inner join PLAN_TRIP PT on TP.PLAN_TRIP_ID = PT.PLAN_TRIP_ID and PT.START_CHECKIN_LOC_ID is not null ");
                sb.AppendFormat("     left join PROSPECT PP on PP.PROSPECT_ID = TP.PROSP_ID  ");
                sb.AppendFormat("     left join PROSPECT_ACCOUNT AC on AC.PROSP_ACC_ID = PP.PROSP_ACC_ID  ");
                sb.AppendFormat("     left join MS_LOCATION L on L.LOC_ID = TP.LOC_ID  ");
                sb.AppendFormat("     left join PLAN_REASON_NOT_VISIT R on R.REASON_NOT_VISIT_ID = TP.REASON_NOT_VISIT_ID  ");
                sb.AppendFormat("     union all ");
                sb.AppendFormat("     select 2 seq,0 ORDER_NO,PLAN_TRIP_ID,PLAN_TRIP_DATE,ASSIGN_EMP_ID,null PROSP_ID,null CHECKIN_DTM,STOP_CHECKIN_DTM CHECKOUT_DTM,STOP_CHECKIN_MILE_NO CHECKIN_MILE_NO ");
                sb.AppendFormat("     ,L.LOC_NAME_TH LOC_NAME,T.CREATE_USER  ");
                sb.AppendFormat("     ,null contact_name,null contact_mobile_no,null checkout_remark ");
                sb.AppendFormat("     from PLAN_TRIP T ");
                sb.AppendFormat("     left join MS_LOCATION L on L.LOC_ID = T.STOP_CHECKIN_LOC_ID ");
                sb.AppendFormat("     where T.START_CHECKIN_LOC_ID is not null ");
                sb.AppendFormat(" ) T  ");
                sb.AppendFormat(") ");
                sb.AppendFormat(" select * from T where T.seq != 2 ");

                ReportCriteria c = searchCriteria.model;
                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat(" and FORMAT(T.PLAN_TRIP_DATE,'yyyyMMdd') >= @startDate  ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat(" and FORMAT(T.PLAN_TRIP_DATE,'yyyyMMdd') <= @endDate  ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                    if (!String.IsNullOrEmpty(c.saleRepId))
                    {
                        sb.AppendFormat(" and IIF(ISNULL(T.ASSIGN_EMP_ID,'')='',T.CREATE_USER,T.ASSIGN_EMP_ID) = @saleRepId  ");
                        QueryUtils.addParam(command, "saleRepId", c.saleRepId);
                    }
                }
                //sb.AppendFormat(" ORDER BY T.PLAN_TRIP_DATE ");
                sb.AppendFormat(" order by T.PLAN_TRIP_DATE,T.PLAN_TRIP_ID,T.seq,T.ORDER_NO ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    searchResult.records = new List<Rep04VisitPlanMonthlyResult>();

                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        Rep04VisitPlanMonthlyResult item = new Rep04VisitPlanMonthlyResult();
                        item.visitDate = QueryUtils.getValueAsString(record, "VISIT_DATE");
                        item.visitTimeStart = QueryUtils.getValueAsString(record, "VISIT_TIME_START");
                        item.startDheckinMileNo = QueryUtils.getValueAsString(record, "START_CHECKIN_MILE_NO");
                        item.locStartName = QueryUtils.getValueAsString(record, "LOC_START_NAME");
                        item.visitTimeStop = QueryUtils.getValueAsString(record, "VISIT_TIME_STOP");
                        item.stopCheckinMileNo = QueryUtils.getValueAsString(record, "STOP_CHECKIN_MILE_NO");
                        item.locEndName = QueryUtils.getValueAsString(record, "LOC_END_NAME");
                        item.totalKmInput = QueryUtils.getValueAsString(record, "TOTAL_KM_INPUT");
                        item.contactName = QueryUtils.getValueAsString(record, "CONTACT_NAME");
                        item.address = QueryUtils.getValueAsString(record, "ADDRESS");
                        item.visitDetail = QueryUtils.getValueAsString(record, "VISIT_DETAIL");

                        searchResult.records.Add(item);
                    }
                    // Call Close when done reading.
                    reader.Close();


                    searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;

                }

            }

            return searchResult;
        }



        // BCW

        public async Task<SearchResultBase<Rep02VisitPlanTransactionResult>> SearchRep02VisitPlanTransaction(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep02VisitPlanTransactionResult> searchResult = new SearchResultBase<Rep02VisitPlanTransactionResult>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                ReportCriteria c = searchCriteria.model;
                StringBuilder sb = await sqlSearch02VisitPlanTransaction();
                sb.AppendFormat(" where 1=1 ");
                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat(" and FORMAT(R.PLAN_TRIP_DATE,'yyyyMMdd') >= @startDate  ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat(" and FORMAT(R.PLAN_TRIP_DATE,'yyyyMMdd') <= @endDate  ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                    if (!String.IsNullOrEmpty(c.saleRepId))
                    {
                        sb.AppendFormat(" and R.SALE_REP_ID = @saleRepId  ");
                        QueryUtils.addParam(command, "saleRepId", c.saleRepId);
                    }
                }
                sb.AppendFormat(" GROUP BY R.SALE_REP_ID,R.SALE_REP_NAME ");
                sb.AppendFormat(" ORDER BY R.SALE_REP_ID ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    searchResult.records = new List<Rep02VisitPlanTransactionResult>();
                    Rep02VisitPlanTransactionResult item = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        item = new Rep02VisitPlanTransactionResult();
                        item.SaleRepId = QueryUtils.getValueAsString(record, "SALE_REP_ID");
                        item.SaleRepName = QueryUtils.getValueAsString(record, "SALE_REP_NAME");
                        item.TotalKmInput = QueryUtils.getValueAsString(record, "TOTAL_KM_INPUT");
                        item.TotalKmSystem = QueryUtils.getValueAsString(record, "TOTAL_KM_SYSTEM");


                        if (!String.IsNullOrEmpty(item.TotalKmSystem) && item.TotalKmSystem.Contains("."))
                        {
                            item.TotalKmSystem = item.TotalKmSystem.Split(".")[0];
                        }

                        //item.StartDate = c.startDate;
                        //item.EndDate = c.endDate;

                        searchResult.records.Add(item);
                    }
                    // Call Close when done reading.
                    reader.Close();


                    searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;

                }

            }

            return searchResult;
        }

        private async Task<StringBuilder> sqlSearch02VisitPlanTransaction()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(" WITH R AS(");
            sb.AppendFormat(" select T.PLAN_TRIP_DATE ");
            sb.AppendFormat(" , IIF(ISNULL(ASSIGN_EMP_ID,'')='',T.CREATE_USER,T.ASSIGN_EMP_ID) SALE_REP_ID ");
            sb.AppendFormat(" , E.FIRST_NAME+' '+E.LAST_NAME SALE_REP_NAME ");
            sb.AppendFormat(" , T.STOP_CHECKIN_MILE_NO-T.START_CHECKIN_MILE_NO TOTAL_KM_INPUT ");
            sb.AppendFormat(" , (SELECT SUM(TP.VISIT_CALC_KM) FROM PLAN_TRIP_PROSPECT TP WHERE TP.PLAN_TRIP_ID = T.PLAN_TRIP_ID) + T.STOP_CALC_KM TOTAL_KM_SYSTEM ");
            sb.AppendFormat(" from PLAN_TRIP T ");
            sb.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = IIF(ISNULL(ASSIGN_EMP_ID,'')='',T.CREATE_USER,T.ASSIGN_EMP_ID) ");
            sb.AppendFormat(" where T.STOP_CHECKIN_DTM is not null ");
            sb.AppendFormat(" )  ");
            sb.AppendFormat(" select R.SALE_REP_ID,R.SALE_REP_NAME ");
            sb.AppendFormat(" ,SUM(R.TOTAL_KM_INPUT) TOTAL_KM_INPUT ");
            sb.AppendFormat(" ,SUM(R.TOTAL_KM_SYSTEM) TOTAL_KM_SYSTEM ");
            sb.AppendFormat(" FROM R ");
            sb.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = R.SALE_REP_ID ");

            return sb;
        }

        public async Task<SearchResultBase<Rep06MeterRawDataResult>> SearchRep06MeterRawData(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep06MeterRawDataResult> searchResult = new SearchResultBase<Rep06MeterRawDataResult>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                ReportCriteria c = searchCriteria.model;
                StringBuilder sb = await sqlSearch06MeterRawData();
                sb.AppendFormat(" where 1=1 ");
                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat(" and FORMAT(R.CREATE_DTM,'yyyyMMdd') >= @startDate  ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat(" and FORMAT(R.CREATE_DTM,'yyyyMMdd') <= @endDate  ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                    if (!String.IsNullOrEmpty(c.custCode))
                    {
                        sb.AppendFormat(" and C.CUST_CODE = @custCode  ");
                        QueryUtils.addParam(command, "custCode", c.custCode);
                    }
                }
                sb.AppendFormat(" ORDER BY C.CUST_CODE,R.CREATE_DTM,CNM.CNT_DISPENSER_NO,CNM.CNT_NOZZLE ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    searchResult.records = new List<Rep06MeterRawDataResult>();
                    Rep06MeterRawDataResult item = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        item = new Rep06MeterRawDataResult();
                        item.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        item.CustNameTh = QueryUtils.getValueAsString(record, "CUST_NAME_TH");
                        item.PlanTripId = QueryUtils.getValueAsString(record, "PLAN_TRIP_ID");
                        item.PlanTripName = QueryUtils.getValueAsString(record, "PLAN_TRIP_NAME");
                        item.CreateDtm = QueryUtils.getValueAsString(record, "CREATE_DTM");
                        item.RecordDtm = QueryUtils.getValueAsString(record, "RECORD_DTM");
                        item.PrevGasCode = QueryUtils.getValueAsString(record, "PREV_GAS_CODE");
                        item.PrevGasNameTh = QueryUtils.getValueAsString(record, "PREV_GAS_NAME_TH");
                        item.GasCode = QueryUtils.getValueAsString(record, "GAS_CODE");
                        item.GasNameTh = QueryUtils.getValueAsString(record, "GAS_NAME_TH");
                        item.DispenserNo = QueryUtils.getValueAsString(record, "DISPENSER_NO");
                        item.NozzleNo = QueryUtils.getValueAsString(record, "NOZZLE_NO");
                        item.RecRunNo = QueryUtils.getValueAsString(record, "REC_RUN_NO");
                        item.CntDispenserNo = QueryUtils.getValueAsString(record, "CNT_DISPENSER_NO");
                        item.CntNozzle = QueryUtils.getValueAsString(record, "CNT_NOZZLE");
                        item.VisitLatitude = QueryUtils.getValueAsString(record, "VISIT_LATITUDE");
                        item.VisitLongitude = QueryUtils.getValueAsString(record, "VISIT_LONGITUDE");
                        item.SaleRepId = QueryUtils.getValueAsString(record, "SALE_REP_ID");
                        item.SaleRepName = QueryUtils.getValueAsString(record, "SALE_REP_NAME");
                        searchResult.records.Add(item);
                    }
                    // Call Close when done reading.
                    reader.Close();


                    searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;

                }

            }

            return searchResult;
        }

        private async Task<StringBuilder> sqlSearch06MeterRawData()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" WITH CN AS(");
            sb.AppendFormat(" select distinct CUST_CODE, DISPENSER_NO ");
            sb.AppendFormat(" , COUNT(DISPENSER_NO) OVER (PARTITION BY CUST_CODE) CNT_NOZZLE ");
            sb.AppendFormat(" FROM MS_METER  ");
            sb.AppendFormat(" where ACTIVE_FLAG = 'Y' ");
            sb.AppendFormat(" ) ");
            sb.AppendFormat(" select C.CUST_CODE,C.CUST_NAME_TH ");
            sb.AppendFormat(" ,T.PLAN_TRIP_ID,T.PLAN_TRIP_NAME,FORMAT(DATEADD(yy,543,T.CREATE_DTM),'dd/MM/yyyy') CREATE_DTM, FORMAT(DATEADD(yy,543,R.CREATE_DTM),'dd/MM/yyyy') RECORD_DTM ");
            sb.AppendFormat(" ,OG.GAS_CODE PREV_GAS_CODE,OG.GAS_NAME_TH PREV_GAS_NAME_TH,G.GAS_CODE,G.GAS_NAME_TH ");
            sb.AppendFormat(" ,M.DISPENSER_NO,M.NOZZLE_NO,R.REC_RUN_NO ");
            sb.AppendFormat(" ,CNM.CNT_DISPENSER_NO,CNM.CNT_NOZZLE,TP.VISIT_LATITUDE,TP.VISIT_LONGITUDE ");
            sb.AppendFormat(" ,E.EMP_ID SALE_REP_ID,E.FIRST_NAME+' '+E.LAST_NAME SALE_REP_NAME ");
            sb.AppendFormat(" from RECORD_METER R ");
            sb.AppendFormat(" inner join MS_METER M on M.METER_ID = R.METER_ID ");
            sb.AppendFormat(" inner join( ");
            sb.AppendFormat("     select distinct CN.CUST_CODE,CN.CNT_NOZZLE ");
            sb.AppendFormat("     ,COUNT(DISPENSER_NO) OVER (PARTITION BY CUST_CODE) CNT_DISPENSER_NO ");
            sb.AppendFormat("     from CN ");
            sb.AppendFormat(" ) CNM on CNM.CUST_CODE = M.CUST_CODE ");
            sb.AppendFormat(" inner join MS_GASOLINE G on G.GAS_ID = M.GAS_ID ");
            sb.AppendFormat(" left join MS_GASOLINE OG on OG.GAS_ID = R.PREV_GAS_ID ");
            sb.AppendFormat(" inner join CUSTOMER C on C.CUST_CODE = M.CUST_CODE ");
            sb.AppendFormat(" inner join PLAN_TRIP_TASK TT on TT.PLAN_TRIP_TASK_ID = R.PLAN_TRIP_TASK_ID ");
            sb.AppendFormat(" inner join PLAN_TRIP_PROSPECT TP on TP.PLAN_TRIP_PROSP_ID = TT.PLAN_TRIP_PROSP_ID ");
            sb.AppendFormat(" inner join PLAN_TRIP T on T.PLAN_TRIP_ID = TP.PLAN_TRIP_ID ");
            sb.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = R.CREATE_USER ");
            return sb;
        }



        public async Task<SearchResultBase<Rep13SaleOrderRawDataResult>> SearchRep13SaleOrderRawData(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep13SaleOrderRawDataResult> searchResult = new SearchResultBase<Rep13SaleOrderRawDataResult>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                ReportCriteria c = searchCriteria.model;
                StringBuilder sb = await sqlSearch13SaleOrderRawData();
                sb.AppendFormat(" where 1=1 ");
                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat(" and FORMAT(O.CREATE_DTM,'yyyyMMdd') >= @startDate  ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat(" and FORMAT(O.CREATE_DTM,'yyyyMMdd') <= @endDate  ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                    if (!String.IsNullOrEmpty(c.saleOrg))
                    {
                        sb.AppendFormat(" and O.ORG_CODE = @saleOrg  ");
                        QueryUtils.addParam(command, "saleOrg", c.saleOrg);
                    }
                    if (!String.IsNullOrEmpty(c.saleChannel))
                    {
                        sb.AppendFormat(" and O.CHANNEL_CODE = @saleChannel  ");
                        QueryUtils.addParam(command, "saleChannel", c.saleChannel);
                    }
                    if (!String.IsNullOrEmpty(c.saleDivision))
                    {
                        sb.AppendFormat(" and O.DIVISION_CODE = @saleDivision  ");
                        QueryUtils.addParam(command, "saleDivision", c.saleDivision);
                    }
                }
                sb.AppendFormat(" order by O.SOM_ORDER_NO ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    searchResult.records = new List<Rep13SaleOrderRawDataResult>();
                    Rep13SaleOrderRawDataResult item = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        item = new Rep13SaleOrderRawDataResult();
                        item.SapOrderNo = QueryUtils.getValueAsString(record, "SAP_ORDER_NO");
                        item.SomOrderNo = QueryUtils.getValueAsString(record, "SOM_ORDER_NO");
                        item.OrderStatus = QueryUtils.getValueAsString(record, "ORDER_STATUS");
                        item.OrgNameTh = QueryUtils.getValueAsString(record, "ORG_NAME_TH");
                        item.ChannelNameTh = QueryUtils.getValueAsString(record, "CHANNEL_NAME_TH");
                        item.DivisionNameTh = QueryUtils.getValueAsString(record, "DIVISION_NAME_TH");
                        item.DescriptionTh = QueryUtils.getValueAsString(record, "DESCRIPTION_TH");
                        item.NetValue = QueryUtils.getValueAsString(record, "NET_VALUE");
                        item.CreateDate = QueryUtils.getValueAsString(record, "CREATE_DATE");
                        item.EmpName = QueryUtils.getValueAsString(record, "EMP_NAME");
                        searchResult.records.Add(item);
                    }
                    // Call Close when done reading.
                    reader.Close();


                    searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;

                }

            }

            return searchResult;
        }

        private async Task<StringBuilder> sqlSearch13SaleOrderRawData()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" select O.SAP_ORDER_NO,O.SOM_ORDER_NO,CG.LOV_NAME_TH ORDER_STATUS ");
            sb.AppendFormat(" ,ORG.ORG_NAME_TH,ODC.CHANNEL_NAME_TH,ODV.DIVISION_NAME_TH,SG.DESCRIPTION_TH ");
            sb.AppendFormat(" ,O.NET_VALUE,FORMAT(DATEADD(yy,543,O.CREATE_DTM),'dd/MM/yyyy')  CREATE_DATE,E.FIRST_NAME + ' ' +E.LAST_NAME EMP_NAME ");
            sb.AppendFormat(" from SALE_ORDER O ");
            sb.AppendFormat(" inner join MS_CONFIG_LOV CG on CG.LOV_KEYWORD = 'ORDER_STATUS' and CG.LOV_KEYVALUE = O.ORDER_STATUS ");
            sb.AppendFormat(" inner join ORG_SALE_ORGANIZATION ORG on ORG.ORG_CODE = O.ORG_CODE ");
            sb.AppendFormat(" inner join ORG_DIST_CHANNEL ODC on ODC.CHANNEL_CODE = O.CHANNEL_CODE ");
            sb.AppendFormat(" inner join ORG_DIVISION ODV on ODV.DIVISION_CODE = O.DIVISION_CODE ");
            sb.AppendFormat(" inner join ORG_SALE_GROUP SG on SG.GROUP_CODE = O.GROUP_CODE ");
            sb.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = O.SALE_REP ");
            return sb;
        }





        public async Task<SearchResultBase<Rep07MeterTransactionResult>> SearchRep07MeterTransaction(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep07MeterTransactionResult> searchResult = new SearchResultBase<Rep07MeterTransactionResult>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                ReportCriteria c = searchCriteria.model;
                StringBuilder sb = new StringBuilder();


                sb.AppendFormat(" WITH N AS(");
                sb.AppendFormat(" select distinct MAX(R.REC_METER_ID) OVER(PARTITION BY R.METER_ID) AS REC_METER_ID ");
                sb.AppendFormat(" from RECORD_METER R ");
                sb.AppendFormat(" inner join RECORD_METER PR on PR.REC_METER_ID = R.PREV_REC_METER_ID ");
                sb.AppendFormat(" inner join MS_METER M on M.METER_ID = R.METER_ID ");
                sb.AppendFormat(" where 1= 1 ");

                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat(" and FORMAT(R.CREATE_DTM,'yyyyMMdd') >= @startDate  ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat(" and FORMAT(R.CREATE_DTM,'yyyyMMdd') <= @endDate  ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                    if (!String.IsNullOrEmpty(c.custCode))
                    {
                        sb.AppendFormat(" and M.CUST_CODE = @custCode  ");
                        QueryUtils.addParam(command, "custCode", c.custCode);
                    }
                    if (!String.IsNullOrEmpty(c.meterNegativeFalg) && "Y".Equals(c.meterNegativeFalg))
                    {
                        sb.AppendFormat(" and cast(R.REC_RUN_NO as int) > cast(R.PREV_REC_RUN_NO as int)  ");
                    }
                }


                sb.AppendFormat(" ), T AS( ");
                sb.AppendFormat(" select R.CREATE_USER,M.CUST_CODE ");
                sb.AppendFormat(" ,SUM(ISNULL(cast(R.REC_RUN_NO as int) - cast(R.PREV_REC_RUN_NO as int),0)) METER_VISIT ");
                sb.AppendFormat(" ,CAST(PR.CREATE_DTM AS DATE) PREV_REC_DATE,CAST(R.CREATE_DTM AS DATE) REC_DATE ");
                sb.AppendFormat(" ,PT.PLAN_TRIP_ID PREV_PLAN_TRIP_ID,T.PLAN_TRIP_ID ");
                sb.AppendFormat(" from N ");
                sb.AppendFormat(" inner join RECORD_METER R on R.REC_METER_ID = N.REC_METER_ID ");
                sb.AppendFormat(" inner join RECORD_METER PR on PR.REC_METER_ID = R.PREV_REC_METER_ID ");
                sb.AppendFormat(" inner join MS_METER M on M.METER_ID = R.METER_ID ");
                sb.AppendFormat(" inner join PLAN_TRIP_TASK TT on TT.PLAN_TRIP_TASK_ID = R.PLAN_TRIP_TASK_ID ");
                sb.AppendFormat(" inner join PLAN_TRIP_PROSPECT TP on TP.PLAN_TRIP_PROSP_ID = TT.PLAN_TRIP_PROSP_ID ");
                sb.AppendFormat(" inner join PLAN_TRIP T on T.PLAN_TRIP_ID = TP.PLAN_TRIP_ID ");
                sb.AppendFormat(" inner join PLAN_TRIP_TASK PTT on PTT.PLAN_TRIP_TASK_ID = PR.PLAN_TRIP_TASK_ID ");
                sb.AppendFormat(" inner join PLAN_TRIP_PROSPECT PTP on PTP.PLAN_TRIP_PROSP_ID = PTT.PLAN_TRIP_PROSP_ID ");
                sb.AppendFormat(" inner join PLAN_TRIP PT on PT.PLAN_TRIP_ID = PTP.PLAN_TRIP_ID ");
                sb.AppendFormat(" GROUP BY R.CREATE_USER,M.CUST_CODE,CAST(PR.CREATE_DTM AS DATE),CAST(R.CREATE_DTM AS DATE),PT.PLAN_TRIP_ID,T.PLAN_TRIP_ID ");
                sb.AppendFormat(" ), CN AS( ");
                sb.AppendFormat(" select distinct CUST_CODE, DISPENSER_NO ");
                sb.AppendFormat(" ,COUNT(DISPENSER_NO) OVER (PARTITION BY CUST_CODE) CNT_NOZZLE ");
                sb.AppendFormat(" FROM MS_METER  ");
                sb.AppendFormat(" where ACTIVE_FLAG = 'Y' ");
                sb.AppendFormat(" ) ");
                sb.AppendFormat(" select E.EMP_ID,E.FIRST_NAME+' '+E.LAST_NAME SALE_NAME ");
                sb.AppendFormat(" ,G.GROUP_CODE,G.DESCRIPTION_TH,C.CUST_CODE,C.CUST_NAME_TH ");
                sb.AppendFormat(" ,CNM.CNT_DISPENSER_NO,CNM.CNT_NOZZLE ");
                sb.AppendFormat(" ,FORMAT(DATEADD(yy,543,T.PREV_REC_DATE),'dd/MM/yyyy') PREV_REC_DATE, FORMAT(DATEADD(yy,543,T.REC_DATE),'dd/MM/yyyy') REC_DATE,T.PREV_PLAN_TRIP_ID,T.PLAN_TRIP_ID,T.METER_VISIT ");
                sb.AppendFormat(" ,ROUND(IIF(DATEDIFF(DAY, T.PREV_REC_DATE, T.REC_DATE) = 0, 0, (CAST(T.METER_VISIT AS FLOAT) / CAST(DATEDIFF(DAY, T.PREV_REC_DATE, T.REC_DATE) AS FLOAT))) * 30, 0) METER_SUMMATION ");
                sb.AppendFormat(" from T ");
                sb.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = T.CREATE_USER ");
                sb.AppendFormat(" inner join ORG_SALE_GROUP G on G.GROUP_CODE = E.GROUP_CODE ");
                sb.AppendFormat(" inner join CUSTOMER C on C.CUST_CODE = T.CUST_CODE ");
                sb.AppendFormat(" inner join( ");
                sb.AppendFormat("     select distinct CN.CUST_CODE,CN.CNT_NOZZLE ");
                sb.AppendFormat("     ,COUNT(DISPENSER_NO) OVER (PARTITION BY CUST_CODE) CNT_DISPENSER_NO ");
                sb.AppendFormat("     from CN ");
                sb.AppendFormat(" ) CNM on CNM.CUST_CODE = C.CUST_CODE ");



                sb.AppendFormat(" ORDER BY C.CUST_CODE ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    searchResult.records = new List<Rep07MeterTransactionResult>();
                    Rep07MeterTransactionResult item = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        item = new Rep07MeterTransactionResult();
                        item.EmpId = QueryUtils.getValueAsString(record, "EMP_ID");
                        item.SaleName = QueryUtils.getValueAsString(record, "SALE_NAME");
                        item.GroupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        item.DescriptionTh = QueryUtils.getValueAsString(record, "DESCRIPTION_TH");
                        item.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        item.CustNameTh = QueryUtils.getValueAsString(record, "CUST_NAME_TH");
                        item.CntDispenserNno = QueryUtils.getValueAsString(record, "CNT_DISPENSER_NO");
                        item.CntNozzle = QueryUtils.getValueAsString(record, "CNT_NOZZLE");
                        item.PrevRecDate = QueryUtils.getValueAsString(record, "PREV_REC_DATE");
                        item.RecDate = QueryUtils.getValueAsString(record, "REC_DATE");
                        item.PrevPlanTripId = QueryUtils.getValueAsString(record, "PREV_PLAN_TRIP_ID");
                        item.PlanTripId = QueryUtils.getValueAsString(record, "PLAN_TRIP_ID");
                        item.MeterVisit = QueryUtils.getValueAsString(record, "METER_VISIT");
                        item.MeterSummation = QueryUtils.getValueAsString(record, "METER_SUMMATION");
                        searchResult.records.Add(item);
                    }
                    // Call Close when done reading.
                    reader.Close();


                    searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;

                }

            }

            return searchResult;
        }

        public async Task<SearchResultBase<Rep08StockCardRawDataResult>> SearchRep08StockCardRawData(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep08StockCardRawDataResult> searchResult = new SearchResultBase<Rep08StockCardRawDataResult>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                ReportCriteria c = searchCriteria.model;
                StringBuilder sb = await sqlSearch08StockCardRawData();
                sb.AppendFormat(" where 1=1 ");
                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat(" and FORMAT(T.PLAN_TRIP_DATE,'yyyyMMdd') >= @startDate  ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat(" and FORMAT(T.PLAN_TRIP_DATE,'yyyyMMdd') <= @endDate  ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                }
                sb.AppendFormat(" ORDER BY TP.VISIT_CHECKIN_DTM,T.PLAN_TRIP_ID,MP.PROD_NAME_TH ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    searchResult.records = new List<Rep08StockCardRawDataResult>();
                    Rep08StockCardRawDataResult item = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        item = new Rep08StockCardRawDataResult();
                        item.PlanTripName = QueryUtils.getValueAsString(record, "PLAN_TRIP_NAME");
                        item.PlanTripId = QueryUtils.getValueAsString(record, "PLAN_TRIP_ID");
                        item.TerritoryId = QueryUtils.getValueAsString(record, "TERRITORY_ID");
                        item.TerritoryNameTh = QueryUtils.getValueAsString(record, "TERRITORY_NAME_TH");
                        item.VisitCheckinDtm = QueryUtils.getValueAsString(record, "VISIT_CHECKIN_DTM");
                        item.VisitCheckoutDtm = QueryUtils.getValueAsString(record, "VISIT_CHECKOUT_DTM");
                        item.EmpName = QueryUtils.getValueAsString(record, "EMP_NAME");
                        item.EmpId = QueryUtils.getValueAsString(record, "EMP_ID");
                        item.CustNameTh = QueryUtils.getValueAsString(record, "CUST_NAME_TH");
                        item.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        item.ProdCateCode = QueryUtils.getValueAsString(record, "PROD_CATE_CODE");
                        item.ProdCateDesc = QueryUtils.getValueAsString(record, "PROD_CATE_DESC");
                        item.ProdNameTh = QueryUtils.getValueAsString(record, "PROD_NAME_TH");
                        item.ProdCode = QueryUtils.getValueAsString(record, "PROD_CODE");
                        item.RecQty = QueryUtils.getValueAsString(record, "REC_QTY");
                        item.AltUnit = QueryUtils.getValueAsString(record, "ALT_UNIT");
                        searchResult.records.Add(item);
                    }
                    // Call Close when done reading.
                    reader.Close();


                    searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;

                }

            }

            return searchResult;
        }

        private async Task<StringBuilder> sqlSearch08StockCardRawData()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" select T.PLAN_TRIP_NAME,T.PLAN_TRIP_ID,OT.TERRITORY_ID,OT.TERRITORY_NAME_TH ");
            sb.AppendFormat(" ,FORMAT(DATEADD(yy,543,TP.VISIT_CHECKIN_DTM),'dd/MM/yyyy') VISIT_CHECKIN_DTM, FORMAT(DATEADD(yy,543,TP.VISIT_CHECKOUT_DTM),'dd/MM/yyyy') VISIT_CHECKOUT_DTM,E.FIRST_NAME + ' ' + E.LAST_NAME EMP_NAME,E.EMP_ID ");
            sb.AppendFormat(" ,C.CUST_NAME_TH,C.CUST_CODE,MP.PROD_CATE_CODE,MP.PROD_CATE_DESC ");
            sb.AppendFormat(" ,MP.PROD_NAME_TH,MP.PROD_CODE,RS.REC_QTY,PC.ALT_UNIT ");
            sb.AppendFormat(" from RECORD_STOCK_CARD RS ");
            sb.AppendFormat(" inner join MS_PRODUCT_CONVERSION PC on PC.PROD_CONV_ID = RS.PROD_CONV_ID ");
            sb.AppendFormat(" inner join(         ");
            sb.AppendFormat("     SELECT T1.PROD_CODE,T1.PROD_NAME_TH ");
            sb.AppendFormat("     ,STRING_AGG (T2.PROD_CATE_CODE, ',') as PROD_CATE_CODE ");
            sb.AppendFormat("     ,STRING_AGG (T2.PROD_CATE_DESC, ',') as PROD_CATE_DESC ");
            sb.AppendFormat("     from MS_PRODUCT T1 ");
            sb.AppendFormat("     inner join (select distinct PROD_CODE,PROD_CATE_CODE,PROD_CATE_DESC from MS_PRODUCT_SALE) T2 on T2.PROD_CODE = T1.PROD_CODE ");
            sb.AppendFormat("     GROUP BY T1.PROD_CODE,T1.PROD_NAME_TH  ");
            sb.AppendFormat(" ) MP on MP.PROD_CODE = PC.PROD_CODE ");
            sb.AppendFormat(" inner join PLAN_TRIP_TASK TT on TT.PLAN_TRIP_TASK_ID = RS.PLAN_TRIP_TASK_ID ");
            sb.AppendFormat(" inner join PLAN_TRIP_PROSPECT TP on TP.PLAN_TRIP_PROSP_ID = TT.PLAN_TRIP_PROSP_ID ");
            sb.AppendFormat(" inner join PLAN_TRIP T on T.PLAN_TRIP_ID = TP.PLAN_TRIP_ID ");
            sb.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = RS.CREATE_USER ");
            sb.AppendFormat(" inner join PROSPECT P on P.PROSPECT_ID = TP.PROSP_ID ");
            sb.AppendFormat(" inner join PROSPECT_ACCOUNT PA on PA.PROSP_ACC_ID = P.PROSP_ACC_ID ");
            sb.AppendFormat(" inner join CUSTOMER C on C.CUST_CODE = PA.CUST_CODE ");
            sb.AppendFormat(" inner join ORG_SALE_GROUP SG ON SG.GROUP_CODE = E.GROUP_CODE ");
            sb.AppendFormat(" inner join ORG_TERRITORY OT on OT.TERRITORY_ID = SG.TERRITORY_ID ");
            /*sb.AppendFormat(" inner join( ");
            sb.AppendFormat("     SELECT ST.EMP_ID ");
            sb.AppendFormat("     ,STRING_AGG (OT.TERRITORY_ID, ',') as TERRITORY_ID ");
            sb.AppendFormat("     ,STRING_AGG (OT.TERRITORY_NAME_TH, ',') as TERRITORY_NAME_TH ");
            sb.AppendFormat("     from ORG_SALE_TERRITORY ST ");
            sb.AppendFormat("     inner join ORG_TERRITORY OT on OT.TERRITORY_ID = ST.TERRITORY_ID ");
            sb.AppendFormat("     GROUP BY ST.EMP_ID ");
            sb.AppendFormat(" ) TR on TR.EMP_ID = RS.CREATE_USER ");*/
            return sb;
        }



        public async Task<SearchResultBase<Rep10ProspectRawDataResult>> SearchRep10ProspectRawData(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep10ProspectRawDataResult> searchResult = new SearchResultBase<Rep10ProspectRawDataResult>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                ReportCriteria c = searchCriteria.model;
                StringBuilder sb = await sqlSearch10ProspectRawData();
                sb.AppendFormat(" where 1=1 ");
                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat(" and FORMAT(PP.CREATE_DTM,'yyyyMMdd') >= @startDate  ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat(" and FORMAT(PP.CREATE_DTM,'yyyyMMdd') <= @endDate  ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                    if (!String.IsNullOrEmpty(c.prospectId))
                    {
                        sb.AppendFormat(" and PP.PROSPECT_ID = @prospectId  ");
                        QueryUtils.addParam(command, "prospectId", c.prospectId);
                    }
                    if (!String.IsNullOrEmpty(c.prospectType))
                    {
                        sb.AppendFormat(" and PP.PROSPECT_TYPE = @prospectType  ");
                        QueryUtils.addParam(command, "prospectType", c.prospectType);
                    }
                    if (!String.IsNullOrEmpty(c.saleRepId))
                    {
                        sb.AppendFormat(" and PP.SALE_REP_ID = @saleRepId  ");
                        QueryUtils.addParam(command, "saleRepId", c.saleRepId);
                    }
                    if (!String.IsNullOrEmpty(c.groupCode))
                    {
                        sb.AppendFormat(" and E.GROUP_CODE = @groupCode  ");
                        QueryUtils.addParam(command, "groupCode", c.groupCode);
                    }
                }
                sb.AppendFormat(" order by PP.PROSPECT_ID ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    searchResult.records = new List<Rep10ProspectRawDataResult>();
                    Rep10ProspectRawDataResult item = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        item = new Rep10ProspectRawDataResult();
                        item.ProspectId = QueryUtils.getValueAsString(record, "PROSPECT_ID");
                        item.AccName = QueryUtils.getValueAsString(record, "ACC_NAME");
                        item.ProspectType = QueryUtils.getValueAsString(record, "PROSPECT_TYPE");
                        item.Latitude = QueryUtils.getValueAsString(record, "LATITUDE");
                        item.Longitude = QueryUtils.getValueAsString(record, "LONGITUDE");
                        item.SaleRepId = QueryUtils.getValueAsString(record, "SALE_REP_ID");
                        item.EmpName = QueryUtils.getValueAsString(record, "EMP_NAME");
                        item.GroupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        item.DescriptionTh = QueryUtils.getValueAsString(record, "DESCRIPTION_TH");
                        item.CreateDate = QueryUtils.getValueAsString(record, "CREATE_DATE");
                        item.Address = QueryUtils.getValueAsString(record, "ADDRESS");
                        item.ProvinceNameTh = QueryUtils.getValueAsString(record, "PROVINCE_NAME_TH");
                        item.DistrictNameTh = QueryUtils.getValueAsString(record, "DISTRICT_NAME_TH");
                        item.SubdistrictNameTh = QueryUtils.getValueAsString(record, "SUBDISTRICT_NAME_TH");
                        searchResult.records.Add(item);
                    }
                    // Call Close when done reading.
                    reader.Close();


                    searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;

                }

            }

            return searchResult;
        }

        private async Task<StringBuilder> sqlSearch10ProspectRawData()
        {
            StringBuilder sb = new StringBuilder();


            sb.AppendFormat(" select PP.PROSPECT_ID,PA.ACC_NAME,CG.LOV_NAME_TH PROSPECT_TYPE ");
            sb.AppendFormat(" ,PD.LATITUDE,PD.LONGITUDE,PP.SALE_REP_ID,E.FIRST_NAME + ' ' + E.LAST_NAME EMP_NAME ");
            sb.AppendFormat(" ,SG.GROUP_CODE,SG.DESCRIPTION_TH,FORMAT(DATEADD(yy,543,PP.CREATE_DTM),'dd/MM/yyyy') CREATE_DATE ");
            sb.AppendFormat(" ,TRIM(IIF(PD.ADDR_NO is null,'',PD.ADDR_NO+' ') + ");
            sb.AppendFormat(" IIF(PD.MOO is null,'',PD.MOO+' ') + ");
            sb.AppendFormat(" IIF(PD.SOI is null,'',PD.SOI+' ') + ");
            sb.AppendFormat(" IIF(PD.STREET is null,'',PD.STREET +' ')) ADDRESS ");
            sb.AppendFormat(" ,MP.PROVINCE_NAME_TH,MD.DISTRICT_NAME_TH,MSD.SUBDISTRICT_NAME_TH ");
            sb.AppendFormat(" from PROSPECT_ACCOUNT PA ");
            sb.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = PA.PROSP_ACC_ID ");
            sb.AppendFormat(" inner join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
            sb.AppendFormat(" inner join MS_CONFIG_LOV CG on CG.LOV_KEYWORD = 'PROSPECT_TYPE' and CG.LOV_KEYVALUE = PP.PROSPECT_TYPE ");
            sb.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = PP.SALE_REP_ID ");
            sb.AppendFormat(" inner join ORG_SALE_GROUP SG on SG.GROUP_CODE = E.GROUP_CODE ");
            sb.AppendFormat(" left join MS_PROVINCE MP on MP.PROVINCE_CODE = PD.PROVINCE_CODE ");
            sb.AppendFormat(" left join MS_DISTRICT MD on MD.DISTRICT_CODE = PD.DISTRICT_CODE ");
            sb.AppendFormat(" left join MS_SUBDISTRICT MSD on MSD.SUBDISTRICT_CODE = PD.SUBDISTRICT_CODE ");
            return sb;
        }

        public async Task<SearchResultBase<Rep05VisitPlanActualResult>> Search05VisitPlanActual(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep05VisitPlanActualResult> searchResult = new SearchResultBase<Rep05VisitPlanActualResult>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(" select IIF(lead(T1.ORDER_NO) over (PARTITION BY T1.PLAN_TRIP_ID ORDER BY T1.ORDER_NO) IS NULL,'Y','N') as LAST_RECORD_FLAG ");
                sb.AppendFormat(" ,T1.ORDER_NO,T1.PLAN_TRIP_ID,T1.VISIT_DATE,T1.EMP_ID,T1.EMP_NAME ");
                sb.AppendFormat(" ,IIF(ADHOC_FLAG = 'N',T1.CHECKPOINT_NAME,'') PLAN_ACC_NAME ");
                sb.AppendFormat(" ,IIF(T2.VISIT_CHECKOUT_DTM IS NOT NULL,T2.CHECKPOINT_NAME,'') ACTUAL_ACC_NAME ");
                sb.AppendFormat(" from ( ");
                sb.AppendFormat("     select ROW_NUMBER() over (PARTITION BY TP.PLAN_TRIP_ID ORDER BY TP.ORDER_NO) AS ORDER_NO ");
                sb.AppendFormat("     ,TP.PLAN_TRIP_ID,IIF(TP.PROSP_ID is null,L.LOC_NAME_TH,PA.ACC_NAME) CHECKPOINT_NAME ");
                sb.AppendFormat("     ,TP.ADHOC_FLAG,E.EMP_ID,E.FIRST_NAME+' '+E.LAST_NAME EMP_NAME ");
                sb.AppendFormat("     ,FORMAT(DATEADD(yy,543,T.PLAN_TRIP_DATE),'dd/MM/yyyy') VISIT_DATE ");
                sb.AppendFormat("     ,T.PLAN_TRIP_DATE ");
                sb.AppendFormat("     from PLAN_TRIP_PROSPECT TP ");
                sb.AppendFormat("     inner join PLAN_TRIP T on T.PLAN_TRIP_ID = TP.PLAN_TRIP_ID ");
                sb.AppendFormat("     inner join ADM_EMPLOYEE E on E.EMP_ID = IIF(ISNULL(ASSIGN_EMP_ID,'')='',T.CREATE_USER,T.ASSIGN_EMP_ID) ");
                sb.AppendFormat("     left join PROSPECT PP on PP.PROSPECT_ID = TP.PROSP_ID ");
                sb.AppendFormat("     left join PROSPECT_ACCOUNT PA on PA.PROSP_ACC_ID = PP.PROSP_ACC_ID ");
                sb.AppendFormat("     left join MS_LOCATION L on L.LOC_ID = TP.LOC_ID ");
                sb.AppendFormat(" )T1 inner join ( ");
                sb.AppendFormat("     select ROW_NUMBER() over (PARTITION BY TP.PLAN_TRIP_ID order by ISNULL(TP.VISIT_CHECKOUT_DTM,999999)) AS ORDER_NO ");
                sb.AppendFormat("     ,TP.PLAN_TRIP_ID,IIF(TP.PROSP_ID is null,L.LOC_NAME_TH,PA.ACC_NAME) CHECKPOINT_NAME ");
                sb.AppendFormat("     ,TP.VISIT_CHECKOUT_DTM ");
                sb.AppendFormat("     from PLAN_TRIP_PROSPECT TP ");
                sb.AppendFormat("     left join PROSPECT PP on PP.PROSPECT_ID = TP.PROSP_ID ");
                sb.AppendFormat("     left join PROSPECT_ACCOUNT PA on PA.PROSP_ACC_ID = PP.PROSP_ACC_ID ");
                sb.AppendFormat("     left join MS_LOCATION L on L.LOC_ID = TP.LOC_ID ");
                sb.AppendFormat(" )T2 on T2.PLAN_TRIP_ID = T1.PLAN_TRIP_ID and T2.ORDER_NO = T1.ORDER_NO ");
                sb.AppendFormat(" WHERE 1=1 ");

                ReportCriteria c = searchCriteria.model;
                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat(" and FORMAT(T1.PLAN_TRIP_DATE,'yyyyMMdd') >= @startDate  ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat(" and FORMAT(T1.PLAN_TRIP_DATE,'yyyyMMdd') <= @endDate  ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                    if (!String.IsNullOrEmpty(c.saleRepId))
                    {
                        sb.AppendFormat(" and T1.EMP_ID = @saleRepId  ");
                        QueryUtils.addParam(command, "saleRepId", c.saleRepId);
                    }
                }
                sb.AppendFormat(" ORDER BY CAST(T1.PLAN_TRIP_DATE as date),T1.PLAN_TRIP_ID,T1.ORDER_NO ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    searchResult.records = new List<Rep05VisitPlanActualResult>();

                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        Rep05VisitPlanActualResult item = new Rep05VisitPlanActualResult();
                        item.visitDate = QueryUtils.getValueAsString(record, "VISIT_DATE");
                        item.saleRepName = QueryUtils.getValueAsString(record, "EMP_NAME");
                        item.saleRepId = QueryUtils.getValueAsString(record, "EMP_ID");
                        item.planAccName = QueryUtils.getValueAsString(record, "PLAN_ACC_NAME");
                        item.actualAccName = QueryUtils.getValueAsString(record, "ACTUAL_ACC_NAME");
                        item.lastRecordFlag = QueryUtils.getValueAsString(record, "LAST_RECORD_FLAG");
                        item.orderNo = QueryUtils.getValueAsString(record, "ORDER_NO");
                        searchResult.records.Add(item);
                    }
                    // Call Close when done reading.
                    reader.Close();


                    searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;

                }

            }

            return searchResult;
        }

        public async Task<SearchResultBase<Rep11ProspectPerformancePerSaleRepResult>> Search11ProspectPerformancePerSaleRep(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep11ProspectPerformancePerSaleRepResult> searchResult = new SearchResultBase<Rep11ProspectPerformancePerSaleRepResult>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(" WITH P AS ( ");
                sb.AppendFormat("   select PP.SALE_REP_ID,E.FIRST_NAME,E.LAST_NAME,G.DESCRIPTION_TH ");
                sb.AppendFormat("   ,COUNT(1) TOTAL_PROSPECT,SUM(IIF(PP.PROSPECT_TYPE = 0,0,1)) TOTAL_PROSPECT_CHANGE ");
                sb.AppendFormat("   from PROSPECT PP ");
                sb.AppendFormat("   inner join ADM_EMPLOYEE E on E.EMP_ID = PP.SALE_REP_ID ");
                sb.AppendFormat("   inner join ADM_GROUP_USER GU on GU.EMP_ID = E.EMP_ID ");
                sb.AppendFormat("   inner join ORG_SALE_GROUP G on G.GROUP_CODE = E.GROUP_CODE ");
                sb.AppendFormat("   where 1=1 ");


                ReportCriteria c = searchCriteria.model;
                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat("   and FORMAT(PP.CREATE_DTM,'yyyyMMdd') >= @startDate  ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat("   and FORMAT(PP.CREATE_DTM,'yyyyMMdd') <= @endDate  ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                    if (!String.IsNullOrEmpty(c.saleRepId))
                    {
                        sb.AppendFormat("   and PP.SALE_REP_ID = @saleRepId  ");
                        QueryUtils.addParam(command, "saleRepId", c.saleRepId);
                    }
                    if (!String.IsNullOrEmpty(c.groupCode))
                    {
                        sb.AppendFormat("   and E.GROUP_CODE = @groupCode  ");
                        QueryUtils.addParam(command, "groupCode", c.groupCode);
                    }
                    if (!String.IsNullOrEmpty(c.buId))
                    {
                        sb.AppendFormat("   and GU.BU_ID = @buId  ");
                        QueryUtils.addParam(command, "buId", c.buId);
                    }
                }
                sb.AppendFormat("   GROUP BY PP.SALE_REP_ID,E.FIRST_NAME,E.LAST_NAME,G.DESCRIPTION_TH ");
                sb.AppendFormat(" ) ");
                sb.AppendFormat(" select P.SALE_REP_ID,P.FIRST_NAME + ' ' + P.LAST_NAME EMP_NAME ");
                sb.AppendFormat(" ,P.DESCRIPTION_TH GROUP_DESC,P.TOTAL_PROSPECT,P.TOTAL_PROSPECT_CHANGE ");
                sb.AppendFormat(" ,ROUND((cast(P.TOTAL_PROSPECT_CHANGE as float)/cast(P.TOTAL_PROSPECT as float))*100,2) PERFORM_PERCENT ");
                sb.AppendFormat(" from P ");
                sb.AppendFormat(" order by P.SALE_REP_ID ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    searchResult.records = new List<Rep11ProspectPerformancePerSaleRepResult>();

                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        Rep11ProspectPerformancePerSaleRepResult item = new Rep11ProspectPerformancePerSaleRepResult();
                        item.saleRepName = QueryUtils.getValueAsString(record, "EMP_NAME");
                        item.saleRepId = QueryUtils.getValueAsString(record, "SALE_REP_ID");
                        item.saleGroupDesc = QueryUtils.getValueAsString(record, "GROUP_DESC");
                        item.totalProspect = QueryUtils.getValueAsString(record, "TOTAL_PROSPECT");
                        item.totalProspectChange = QueryUtils.getValueAsString(record, "TOTAL_PROSPECT_CHANGE");
                        item.performPercent = QueryUtils.getValueAsString(record, "PERFORM_PERCENT");
                        searchResult.records.Add(item);
                    }
                    // Call Close when done reading.
                    reader.Close();


                    searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;

                }

            }

            return searchResult;
        }

        public async Task<SearchResultBase<Rep12ProspectPerformancePerSaleGroupResult>> Search12ProspectPerformancePerSaleGroup(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep12ProspectPerformancePerSaleGroupResult> searchResult = new SearchResultBase<Rep12ProspectPerformancePerSaleGroupResult>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(" WITH P AS ( ");
                sb.AppendFormat("   select G.GROUP_CODE,G.DESCRIPTION_TH ");
                sb.AppendFormat("   ,COUNT(1) TOTAL_PROSPECT,SUM(IIF(PP.PROSPECT_TYPE = 0,0,1)) TOTAL_PROSPECT_CHANGE ");
                sb.AppendFormat("   from PROSPECT PP ");
                sb.AppendFormat("   inner join ADM_EMPLOYEE E on E.EMP_ID = PP.SALE_REP_ID ");
                sb.AppendFormat("   inner join ORG_SALE_GROUP G on G.GROUP_CODE = E.GROUP_CODE ");
                sb.AppendFormat("   where 1=1 ");


                ReportCriteria c = searchCriteria.model;
                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat("   and FORMAT(PP.CREATE_DTM,'yyyyMMdd') >= @startDate  ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat("   and FORMAT(PP.CREATE_DTM,'yyyyMMdd') <= @endDate  ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                    if (!String.IsNullOrEmpty(c.groupCode))
                    {
                        sb.AppendFormat("   and E.GROUP_CODE = @groupCode  ");
                        QueryUtils.addParam(command, "groupCode", c.groupCode);
                    }
                }
                sb.AppendFormat("   GROUP BY G.GROUP_CODE,G.DESCRIPTION_TH ");
                sb.AppendFormat(" ) ");
                sb.AppendFormat(" select P.GROUP_CODE,P.DESCRIPTION_TH GROUP_DESC,P.TOTAL_PROSPECT,P.TOTAL_PROSPECT_CHANGE ");
                sb.AppendFormat(" ,ROUND((cast(P.TOTAL_PROSPECT_CHANGE as float)/cast(P.TOTAL_PROSPECT as float))*100,2) PERFORM_PERCENT ");
                sb.AppendFormat(" from P ");
                sb.AppendFormat(" order by P.GROUP_CODE ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    searchResult.records = new List<Rep12ProspectPerformancePerSaleGroupResult>();

                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        Rep12ProspectPerformancePerSaleGroupResult item = new Rep12ProspectPerformancePerSaleGroupResult();
                        item.saleGroupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        item.saleGroupDesc = QueryUtils.getValueAsString(record, "GROUP_DESC");
                        item.totalProspect = QueryUtils.getValueAsString(record, "TOTAL_PROSPECT");
                        item.totalProspectChange = QueryUtils.getValueAsString(record, "TOTAL_PROSPECT_CHANGE");
                        item.performPercent = QueryUtils.getValueAsString(record, "PERFORM_PERCENT");
                        searchResult.records.Add(item);
                    }
                    // Call Close when done reading.
                    reader.Close();


                    searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;

                }

            }

            return searchResult;
        }

        public async Task<SearchResultBase<Rep14SaleOrderByChannelResult>> search14SaleOrderByChannel(SearchCriteriaBase<ReportCriteria> searchCriteria, InterfaceSapConfig ic)
        {
            using (var client = new HttpClient())
            {
                OutboundSaleOrderInformationRequest reqData = new OutboundSaleOrderInformationRequest();
                reqData.SOM_Report_ID = "14";
                reqData.Document_from_date = searchCriteria.model.startDate;
                reqData.Document_to_date = searchCriteria.model.endDate;
                reqData.Sale_Org = new List<Sale_Orgs>();
                reqData.Distribution_Channel = new List<Distribution_Channels>();
                reqData.Division = new List<Divisions>();
                foreach (var item in searchCriteria.model.listSaleOrg)
                {
                    reqData.Sale_Org.Add(new Sale_Orgs(item));
                }
                reqData.Distribution_Channel.Add(new Distribution_Channels(searchCriteria.model.saleChannel));
                reqData.Division.Add(new Divisions(searchCriteria.model.saleDivision));

                client.BaseAddress = new Uri(ic.InterfaceSapUrl + CommonConstant.API_OutboundSaleOrderInformation);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{ic.InterfaceSapUser}:{ic.InterfaceSapPwd}")));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", CommonConstant.USER_AGEN);
                client.DefaultRequestHeaders.Add(CommonConstant.ReqKey, ic.ReqKey);
                client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(CommonConstant.API_TIMEOUT));
                string fullPath = ic.InterfaceSapUrl + CommonConstant.API_OutboundSaleOrderInformation;
                var jsonVal = JsonConvert.SerializeObject(reqData);
                var content = new StringContent(jsonVal, Encoding.UTF8, "application/json");
                log.Debug("Call Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundSaleOrderInformation);
                log.Debug("=========== jsonVal ================");
                log.Debug("REQUEST:" + jsonVal);
                log.Debug("REQUEST:" + JObject.Parse(jsonVal));

                // Cal use time
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();

                // Begin timing.
                stopwatch.Start();
                //
                HttpResponseMessage response = await client.PostAsync(fullPath, content);

                // Stop timing.
                stopwatch.Stop();

                // Write result.
                Console.WriteLine("Time elapsed: "+ stopwatch.Elapsed);
                log.Debug("Time Use: "+ stopwatch.Elapsed);

                OutboundSaleOrderInformationResponse resultOutbound = new OutboundSaleOrderInformationResponse();
                List<Rep14SaleOrderByChannelResult> listResult = new List<Rep14SaleOrderByChannelResult>();
                if (response.IsSuccessStatusCode)
                {
                    resultOutbound = await response.Content.ReadAsAsync<OutboundSaleOrderInformationResponse>();

                    if(resultOutbound.SO_Status.Equals("S")) {
                        List<Rep14SaleOrderByChannelResult> items = new List<Rep14SaleOrderByChannelResult>();
                        foreach (var company in resultOutbound.Data[0].Company)
                        {
                            foreach (var distribution in company.Distribution)
                            {
                                distribution.Sale_Order_Report.ForEach(s => items.Add(new Rep14SaleOrderByChannelResult(
                                distribution.Distribution_Channel_Name,
                                s.Sale_Group_Name,
                                s.Total_Sale_Order,
                                s.SO_SOM,
                                s.SO_SAP,
                                s.SO_div_SOM,
                                s.SO_div_SAP)));

                            }
                        }

                        foreach (var channel in items.GroupBy(group => group.channelDesc))
                        {
                            var channelDesc = channel.Key;
                            foreach (var saleGroup in channel.GroupBy(group => group.saleGroupDesc))
                            {
                                Rep14SaleOrderByChannelResult r = new Rep14SaleOrderByChannelResult(
                                    channelDesc,
                                    saleGroup.Key,
                                    saleGroup.Sum(s => int.Parse(s.totalOrder)).ToString(),
                                    saleGroup.Sum(s => int.Parse(s.totalSomOrder)).ToString(),
                                    saleGroup.Sum(s => int.Parse(s.totalSapOrder)).ToString(),
                                    "",
                                    "");
                                listResult.Add(r);
                            }
                        }


                    } else {
                        string result_error = response.Content.ReadAsStringAsync().Result;
                        Exception ex = new Exception(resultOutbound.SO_Message);
                        throw ex;
                    }
                }
                else
                {
                    string result_error = response.Content.ReadAsStringAsync().Result;
                    Exception ex = new Exception(result_error);
                    throw ex;
                }

                log.Debug("Response Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundPlantInformation);
                log.Debug("Response:" + JsonConvert.SerializeObject(resultOutbound));

                SearchResultBase<Rep14SaleOrderByChannelResult> searchResult = new SearchResultBase<Rep14SaleOrderByChannelResult>();
                searchResult.totalRecords = listResult.Count;
                searchResult.records = listResult;
                return searchResult;

            }
        }

        public async Task<SearchResultBase<Rep15SaleOrderByCompanyResult>> search15SaleOrderByCompany(SearchCriteriaBase<ReportCriteria> searchCriteria, InterfaceSapConfig ic)
        {
            using (var client = new HttpClient())
            {
                OutboundSaleOrderInformationRequest reqData = new OutboundSaleOrderInformationRequest();
                reqData.SOM_Report_ID = "15";
                reqData.Document_from_date = searchCriteria.model.startDate;
                reqData.Document_to_date = searchCriteria.model.endDate;
                reqData.Company = new List<Companys>();
                reqData.Sale_Org = new List<Sale_Orgs>();
                reqData.Distribution_Channel = new List<Distribution_Channels>();
                reqData.Division = new List<Divisions>();
                reqData.Sale_Org.Add(new Sale_Orgs(searchCriteria.model.saleOrg));
                foreach (var item in searchCriteria.model.listCompany)
                {
                    reqData.Company.Add(new Companys(item));
                }
                reqData.Distribution_Channel.Add(new Distribution_Channels(searchCriteria.model.saleChannel));
                reqData.Division.Add(new Divisions(searchCriteria.model.saleDivision));

                client.BaseAddress = new Uri(ic.InterfaceSapUrl + CommonConstant.API_OutboundSaleOrderInformation);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{ic.InterfaceSapUser}:{ic.InterfaceSapPwd}")));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", CommonConstant.USER_AGEN);
                client.DefaultRequestHeaders.Add(CommonConstant.ReqKey, ic.ReqKey);
                client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(CommonConstant.API_TIMEOUT));
                string fullPath = ic.InterfaceSapUrl + CommonConstant.API_OutboundSaleOrderInformation;
                var jsonVal = JsonConvert.SerializeObject(reqData);
                var content = new StringContent(jsonVal, Encoding.UTF8, "application/json");
                log.Debug("Call Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundSaleOrderInformation);
                log.Debug("=========== jsonVal ================");
                log.Debug("REQUEST:" + jsonVal);
                log.Debug("REQUEST:" + JObject.Parse(jsonVal));
                // Cal use time
                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();

                // Begin timing.
                stopwatch.Start();
                //
                HttpResponseMessage response = await client.PostAsync(fullPath, content);

                // Stop timing.
                stopwatch.Stop();

                // Write result.
                Console.WriteLine("Time elapsed: "+ stopwatch.Elapsed);
                log.Debug("Time Use: "+ stopwatch.Elapsed);

                OutboundSaleOrderInformationResponse resultOutbound = new OutboundSaleOrderInformationResponse();
                List<Rep15SaleOrderByCompanyResult> listResult = new List<Rep15SaleOrderByCompanyResult>();
                if (response.IsSuccessStatusCode)
                {
                    resultOutbound = await response.Content.ReadAsAsync<OutboundSaleOrderInformationResponse>();

                    if (resultOutbound.SO_Status.Equals("S"))
                    {
                        Rep15SaleOrderByCompanyResult result = new Rep15SaleOrderByCompanyResult();
                        result.Company = resultOutbound.Data[0].Company;
                        listResult.Add(result);
                    }
                    else
                    {
                        string result_error = response.Content.ReadAsStringAsync().Result;
                        Exception ex = new Exception(resultOutbound.SO_Message);                        
                        throw ex;
                    }
                }
                else
                {
                    string result_error = response.Content.ReadAsStringAsync().Result;
                    Exception ex = new Exception(result_error);
                    throw ex;
                }

                log.Debug("Response Outbound Service:" + ic.InterfaceSapUrl + CommonConstant.API_OutboundPlantInformation);
                log.Debug("Response:" + JsonConvert.SerializeObject(resultOutbound));

                SearchResultBase<Rep15SaleOrderByCompanyResult> searchResult = new SearchResultBase<Rep15SaleOrderByCompanyResult>();
                searchResult.totalRecords = listResult.Count == 0 ? 0 : listResult[0].Company.Count;
                searchResult.records = listResult;
                return searchResult;

            }
        }
        // BCW

        public async Task<SearchResultBase<Rep09Rult>> search09StockCardCustomerSummary(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep09Rult> searchResult = new SearchResultBase<Rep09Rult>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(" WITH R AS ( ");
                sb.AppendFormat("     select  ");
                sb.AppendFormat("      E.GROUP_CODE ");
                sb.AppendFormat("     ,RS.CREATE_USER ");
                sb.AppendFormat("     ,PA.CUST_CODE ");
                sb.AppendFormat("     ,FORMAT(RS.CREATE_DTM,'MMM') MMM ");
                sb.AppendFormat("     ,PC.PROD_CODE ");
                sb.AppendFormat("     ,RS.PROD_CONV_ID ");
                sb.AppendFormat("     ,RS.REC_QTY ");
                sb.AppendFormat("     from RECORD_STOCK_CARD RS ");
                sb.AppendFormat("     inner join MS_PRODUCT_CONVERSION PC on PC.PROD_CONV_ID = RS.PROD_CONV_ID ");
                sb.AppendFormat("     inner join PLAN_TRIP_TASK TT on TT.PLAN_TRIP_TASK_ID = RS.PLAN_TRIP_TASK_ID ");
                sb.AppendFormat("     inner join PLAN_TRIP_PROSPECT TP on TP.PLAN_TRIP_PROSP_ID = TT.PLAN_TRIP_PROSP_ID ");
                sb.AppendFormat("     inner join PLAN_TRIP T on T.PLAN_TRIP_ID = TP.PLAN_TRIP_ID ");
                sb.AppendFormat("     inner join PROSPECT P on P.PROSPECT_ID = TP.PROSP_ID ");
                sb.AppendFormat("     inner join PROSPECT_ACCOUNT PA on PA.PROSP_ACC_ID = P.PROSP_ACC_ID  ");
                sb.AppendFormat("     inner join ADM_EMPLOYEE E on E.EMP_ID = RS.CREATE_USER ");
                sb.AppendFormat("     inner join ORG_SALE_GROUP SG on SG.GROUP_CODE = E.GROUP_CODE ");
                sb.AppendFormat("     where 1=1 ");

                ReportCriteria c = searchCriteria.model;
                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat("   and FORMAT(RS.CREATE_DTM,'yyyyMMdd') >= @startDate  ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat("   and FORMAT(RS.CREATE_DTM,'yyyyMMdd') <= @endDate  ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                    if (!String.IsNullOrEmpty(c.saleRepId))
                    {
                        sb.AppendFormat("   and RS.CREATE_USER = @saleRepId  ");
                        QueryUtils.addParam(command, "saleRepId", c.saleRepId);
                    }
                    if (!String.IsNullOrEmpty(c.custCode))
                    {
                        sb.AppendFormat("   and PA.CUST_CODE = @custCode  ");
                        QueryUtils.addParam(command, "custCode", c.custCode);
                    }
                    if (!String.IsNullOrEmpty(c.groupCode))
                    {
                        sb.AppendFormat("   and E.GROUP_CODE = @groupCode  ");
                        QueryUtils.addParam(command, "groupCode", c.groupCode);
                    }
                }

                sb.AppendFormat(" ), G AS ( ");
                sb.AppendFormat(" SELECT R.GROUP_CODE,R.CREATE_USER,R.CUST_CODE,R.MMM,R.PROD_CODE,R.PROD_CONV_ID,SUM(R.REC_QTY) REC_QTY ");
                sb.AppendFormat(" FROM R ");
                sb.AppendFormat(" GROUP BY R.GROUP_CODE,R.CREATE_USER,R.CUST_CODE,R.MMM,R.PROD_CODE,R.PROD_CONV_ID ");
                sb.AppendFormat(" ), S AS ( ");
                sb.AppendFormat(" SELECT A.REPORT_UNIT ");
                sb.AppendFormat(" ,A.PROD_CODE,A.PROD_CONV_ID ");
                sb.AppendFormat(" ,G.GROUP_CODE,G.CREATE_USER,G.CUST_CODE,G.MMM,A.PROD_NAME_TH,A.ALT_UNIT ");
                sb.AppendFormat(" ,IIF(A.REPORT_UNIT=1,SUM(IIF(A.REPORT_UNIT=1,0,CEILING(((A.COUNTER/A.DENOMINATOR)*G.REC_QTY)/(A.BASE_COUNTER/A.BASE_DENOMINATOR)))) OVER (PARTITION BY G.GROUP_CODE+''+G.CREATE_USER+''+G.CUST_CODE+''+G.MMM+''+G.PROD_CODE),G.REC_QTY) REC_QTY ");
                sb.AppendFormat(" ,IIF(A.REPORT_UNIT=1,0,CEILING(((A.COUNTER/A.DENOMINATOR)*G.REC_QTY)/(A.BASE_COUNTER/A.BASE_DENOMINATOR))) BASE_REPORT_REC_QTY ");
                sb.AppendFormat(" FROM G ");
                sb.AppendFormat(" RIGHT JOIN ( ");
                sb.AppendFormat("     select P.PROD_CODE,P.PROD_NAME_TH,PC.PROD_CONV_ID,PC.ALT_UNIT,0 REPORT_UNIT ");
                sb.AppendFormat("     ,PC.COUNTER,PC.DENOMINATOR ");
                sb.AppendFormat("     ,PCR.COUNTER BASE_COUNTER,PCR.DENOMINATOR BASE_DENOMINATOR ");
                sb.AppendFormat("     from MS_PRODUCT P ");
                sb.AppendFormat("     inner join MS_PRODUCT_CONVERSION PC on PC.PROD_CODE = P.PROD_CODE ");
                sb.AppendFormat("     left join MS_PRODUCT_CONVERSION PCR on PCR.PROD_CONV_ID = P.REPORT_PROD_CONV_ID ");
                sb.AppendFormat("     union all ");
                sb.AppendFormat("     select P.PROD_CODE,P.PROD_NAME_TH,PC.PROD_CONV_ID,PC.ALT_UNIT,1 REPORT_UNIT ");
                sb.AppendFormat("     ,0 COUNTER,0 DENOMINATOR ");
                sb.AppendFormat("     ,0 COUNTER,0 DENOMINATOR ");
                sb.AppendFormat("     from MS_PRODUCT P ");
                sb.AppendFormat("     inner join MS_PRODUCT_CONVERSION PC on PC.PROD_CONV_ID = P.REPORT_PROD_CONV_ID ");
                sb.AppendFormat("     where exists (select 1 from MS_PRODUCT_CONVERSION T where T.PROD_CODE = P.PROD_CODE) ");
                sb.AppendFormat(" ) A on A.PROD_CONV_ID = G.PROD_CONV_ID ");
                sb.AppendFormat(" where G.PROD_CODE = A.PROD_CODE ");
                sb.AppendFormat(" ) ");
                sb.AppendFormat(" SELECT S.REPORT_UNIT,S.GROUP_CODE,G.DESCRIPTION_TH GROUP_DESC ");
                sb.AppendFormat(" ,E.EMP_ID,E.FIRST_NAME+' '+E.LAST_NAME EMP_NAME ");
                sb.AppendFormat(" ,C.CUST_CODE,C.CUST_NAME_TH ");
                sb.AppendFormat(" ,S.MMM ");
                sb.AppendFormat(" ,CAST(S.REPORT_UNIT as varchar)+CAST(S.PROD_CONV_ID as varchar) KEY_COLM_NAME ");
                sb.AppendFormat(" ,IIF(S.REPORT_UNIT=1,'Report Unit - ','')+S.PROD_NAME_TH+'('+S.ALT_UNIT+')' COLM_NAME ");
                sb.AppendFormat(" ,S.REC_QTY ");
                sb.AppendFormat(" FROM S ");
                sb.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = S.CREATE_USER ");
                sb.AppendFormat(" inner join CUSTOMER C on C.CUST_CODE = S.CUST_CODE ");
                sb.AppendFormat(" inner join ORG_SALE_GROUP G on G.GROUP_CODE = S.GROUP_CODE ");
                sb.AppendFormat(" ORDER BY S.PROD_CODE,S.REPORT_UNIT,S.PROD_CONV_ID ");

                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    searchResult.records = new List<Rep09Rult>();
                    var col = 2;
                    Rep09Rult rep09Rult = new Rep09Rult();
                    rep09Rult.mapColmn = new Dictionary<String, String[]>();
                    rep09Rult.listSaleGroup = new List<Rep09SaleGroup>();
                    Rep09SaleGroup repSaleGroup = new Rep09SaleGroup();
                    Rep09SaleRep repSaleRep = new Rep09SaleRep();
                    Rep09Cust repCust = new Rep09Cust();
                    Rep09Month repMonth = new Rep09Month();
                    var groupCode = "";
                    var empId = "";
                    var custCode = "";
                    var mon = "";

                    List<Rep09StockCardCustomerSummaryResult> resultSort = new List<Rep09StockCardCustomerSummaryResult>();
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        if ((String.IsNullOrEmpty(c.reportUnitFalg) || "N".Equals(c.reportUnitFalg))
                            && "1".Equals(QueryUtils.getValueAsString(record, "REPORT_UNIT")))
                            continue;

                        if (!rep09Rult.mapColmn.ContainsKey(QueryUtils.getValueAsString(record, "KEY_COLM_NAME")))
                        {
                            rep09Rult.mapColmn.Add(QueryUtils.getValueAsString(record, "KEY_COLM_NAME"), new string[] { QueryUtils.getValueAsString(record, "COLM_NAME"), col++.ToString() });
                        }

                        /*
                        if (!groupCode.Equals(QueryUtils.getValueAsString(record, "GROUP_CODE")))
                        {
                            groupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                            repSaleGroup = new Rep09SaleGroup();
                            repSaleGroup.groupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                            repSaleGroup.groupDesc = QueryUtils.getValueAsString(record, "GROUP_DESC");
                            repSaleGroup.listSaleRep = new List<Rep09SaleRep>();
                            rep09Rult.listSaleGroup.Add(repSaleGroup);
                            empId = "";
                            custCode = "";
                            mon = "";
                        }
                        if (!empId.Equals(QueryUtils.getValueAsString(record, "EMP_ID")))
                        {
                            empId = QueryUtils.getValueAsString(record, "EMP_ID");
                            repSaleRep = new Rep09SaleRep();
                            repSaleRep.empId = QueryUtils.getValueAsString(record, "EMP_ID");
                            repSaleRep.empName = QueryUtils.getValueAsString(record, "EMP_NAME");
                            repSaleRep.listCust = new List<Rep09Cust>();
                            repSaleGroup.listSaleRep.Add(repSaleRep);
                            custCode = "";
                            mon = "";
                        }
                        if (!custCode.Equals(QueryUtils.getValueAsString(record, "CUST_CODE")))
                        {
                            custCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                            repCust = new Rep09Cust();
                            repCust.custCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                            repCust.custNameTh = QueryUtils.getValueAsString(record, "CUST_NAME_TH");
                            repCust.listMonth = new List<Rep09Month>();
                            repSaleRep.listCust.Add(repCust);
                            mon = "";
                        }
                        if (!mon.Equals(QueryUtils.getValueAsString(record, "MMM")))
                        {
                            mon = QueryUtils.getValueAsString(record, "MMM");
                            repMonth = new Rep09Month();
                            repMonth.mon = QueryUtils.getValueAsString(record, "MMM");
                            repMonth.listQty = new List<Rep09Qty>();
                            repCust.listMonth.Add(repMonth);
                        }

                        Rep09Qty repQty = new Rep09Qty();
                        repQty.keyColmName = QueryUtils.getValueAsString(record, "KEY_COLM_NAME");
                        repQty.recQty = QueryUtils.getValueAsString(record, "REC_QTY");
                        var mKey = rep09Rult.mapColmn.Single(x => x.Key == repQty.keyColmName);
                        var mValue = mKey.Value;
                        repQty.colmNo = mValue[1];
                        repMonth.listQty.Add(repQty);
                        */



                        Rep09StockCardCustomerSummaryResult item = new Rep09StockCardCustomerSummaryResult();
                        item.groupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        item.groupDesc = QueryUtils.getValueAsString(record, "GROUP_DESC");
                        item.empId = QueryUtils.getValueAsString(record, "EMP_ID");
                        item.empName = QueryUtils.getValueAsString(record, "EMP_NAME");
                        item.custCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        item.custNameTh = QueryUtils.getValueAsString(record, "CUST_NAME_TH");
                        item.mon = QueryUtils.getValueAsString(record, "MMM");
                        item.keyColmName = QueryUtils.getValueAsString(record, "KEY_COLM_NAME");
                        item.colmName = QueryUtils.getValueAsString(record, "COLM_NAME");
                        item.recQry = QueryUtils.getValueAsString(record, "REC_QTY");

                        resultSort.Add(item);

                    }
                    // Call Close when done reading.
                    reader.Close();

                    foreach (var result in resultSort.OrderBy(s => s.groupCode).ThenBy(s => s.empId).ThenBy(s => s.custCode).ThenByDescending(s => s.mon))
                    {
                        if (!groupCode.Equals(result.groupCode))
                        {
                            groupCode = result.groupCode;
                            repSaleGroup = new Rep09SaleGroup();
                            repSaleGroup.groupCode = result.groupCode;
                            repSaleGroup.groupDesc = result.groupDesc;
                            repSaleGroup.listSaleRep = new List<Rep09SaleRep>();
                            rep09Rult.listSaleGroup.Add(repSaleGroup);
                            empId = "";
                            custCode = "";
                            mon = "";
                        }
                        if (!empId.Equals(result.empId))
                        {
                            empId = result.empId;
                            repSaleRep = new Rep09SaleRep();
                            repSaleRep.empId = result.empId;
                            repSaleRep.empName = result.empName;
                            repSaleRep.listCust = new List<Rep09Cust>();
                            repSaleGroup.listSaleRep.Add(repSaleRep);
                            custCode = "";
                            mon = "";
                        }
                        if (!custCode.Equals(result.custCode))
                        {
                            custCode = result.custCode;
                            repCust = new Rep09Cust();
                            repCust.custCode = result.custCode;
                            repCust.custNameTh = result.custNameTh;
                            repCust.listMonth = new List<Rep09Month>();
                            repSaleRep.listCust.Add(repCust);
                            mon = "";
                        }
                        if (!mon.Equals(result.mon))
                        {
                            mon = result.mon;
                            repMonth = new Rep09Month();
                            repMonth.mon = result.mon;
                            repMonth.listQty = new List<Rep09Qty>();
                            repCust.listMonth.Add(repMonth);
                        }

                        Rep09Qty repQty = new Rep09Qty();
                        repQty.keyColmName = result.keyColmName;
                        repQty.recQty = result.recQry;
                        var mKey = rep09Rult.mapColmn.Single(x => x.Key == repQty.keyColmName);
                        var mValue = mKey.Value;
                        repQty.colmNo = mValue[1];
                        repMonth.listQty.Add(repQty);
                    }


                    searchResult.records.Add(rep09Rult);
                    //searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;

                }

            }

            return searchResult;
        }

        public async Task<SearchResultBase<Rep17ImageInTemplateResult>> SearchRep17ImageInTemplate(SearchCriteriaBase<ReportCriteria> searchCriteria)
        {
            SearchResultBase<Rep17ImageInTemplateResult> searchResult = new SearchResultBase<Rep17ImageInTemplateResult>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                ReportCriteria c = searchCriteria.model;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select E.EMP_ID,E.FIRST_NAME+' '+E.LAST_NAME EMP_NAME ");
                sb.AppendFormat(",P.PLAN_TRIP_ID,P.PLAN_TRIP_NAME,FORMAT(DATEADD(yy,543,P.PLAN_TRIP_DATE),'dd/MM/yyyy') PLAN_TRIP_DATE ");
                sb.AppendFormat(",T.TP_NAME_TH,F.FILE_ID ");
                sb.AppendFormat("from RECORD_APP_FORM_FILE F ");
                sb.AppendFormat("inner join RECORD_APP_FORM R on F.REC_APP_FORM_ID = R.REC_APP_FORM_ID and F.PHOTO_FLAG = 'Y' ");
                sb.AppendFormat("inner join TEMPLATE_APP_FORM T on R.TP_APP_FORM_ID = T.TP_APP_FORM_ID and T.USED_FLAG = 'Y' ");
                sb.AppendFormat("inner join PLAN_TRIP_TASK TT on R.PLAN_TRIP_TASK_ID = TT.PLAN_TRIP_TASK_ID ");
                sb.AppendFormat("inner join PLAN_TRIP_PROSPECT TP on TT.PLAN_TRIP_PROSP_ID = TP.PLAN_TRIP_PROSP_ID ");
                sb.AppendFormat("inner join PLAN_TRIP P on TP.PLAN_TRIP_ID = P.PLAN_TRIP_ID ");
                sb.AppendFormat("inner join ADM_EMPLOYEE E on IIF(ISNULL(P.ASSIGN_EMP_ID,'')='',P.CREATE_USER,P.ASSIGN_EMP_ID) = E.EMP_ID ");
                sb.AppendFormat("where 1=1 ");
                if (c != null)
                {
                    if (!String.IsNullOrEmpty(c.startDate))
                    {
                        sb.AppendFormat(" and FORMAT(P.PLAN_TRIP_DATE,'yyyyMMdd') >= @startDate ");
                        QueryUtils.addParam(command, "startDate", c.startDate);
                    }
                    if (!String.IsNullOrEmpty(c.endDate))
                    {
                        sb.AppendFormat(" and FORMAT(P.PLAN_TRIP_DATE,'yyyyMMdd') <= @endDate ");
                        QueryUtils.addParam(command, "endDate", c.endDate);
                    }
                    if (!String.IsNullOrEmpty(c.saleRepId))
                    {
                        sb.AppendFormat(" and E.EMP_ID = @saleRepId ");
                        QueryUtils.addParam(command, "saleRepId", c.saleRepId);
                    }
                    if (!String.IsNullOrEmpty(c.prospectId))
                    {
                        sb.AppendFormat(" and R.PROSP_ID  = @prospectId ");
                        QueryUtils.addParam(command, "prospectId", c.prospectId);
                    }
                    if (!String.IsNullOrEmpty(c.tpAppFormId))
                    {
                        sb.AppendFormat(" and T.TP_APP_FORM_ID = @tpAppFormId ");
                        QueryUtils.addParam(command, "tpAppFormId", c.tpAppFormId);
                    }
                }
                sb.AppendFormat("order by P.PLAN_TRIP_DATE,P.PLAN_TRIP_ID ");


                // Total Record
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    command.CommandText = sb.ToString();
                    log.Debug("Query Count:" + sb.ToString());
                    Console.WriteLine("Query Count:" + sb.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count_++;
                        }
                    }

                    QueryUtils.setSqlPaging(sb, command, searchCriteria.length, searchCriteria.pageNo);
                }

                // For Paging
                log.Debug("Query Data:" + sb.ToString());
                Console.WriteLine("Query Data:" + sb.ToString());
                command.CommandText = sb.ToString();
                using (var reader = command.ExecuteReader())
                {
                    searchResult.records = new List<Rep17ImageInTemplateResult>();
                    Rep17ImageInTemplateResult item = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        item = new Rep17ImageInTemplateResult();
                        item.SaleRepId = QueryUtils.getValueAsString(record, "EMP_ID");
                        item.SaleRepName = QueryUtils.getValueAsString(record, "EMP_NAME");
                        item.VisitPlanId = QueryUtils.getValueAsString(record, "PLAN_TRIP_ID");
                        item.VisitPlanName = QueryUtils.getValueAsString(record, "PLAN_TRIP_NAME");
                        item.VisitDate = QueryUtils.getValueAsString(record, "PLAN_TRIP_DATE");
                        item.TemplateName = QueryUtils.getValueAsString(record, "TP_NAME_TH");
                        item.ImageUrl = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_RECORD_APP_FORM_FILE, QueryUtils.getValueAsString(record, "FILE_ID"));

                        searchResult.records.Add(item);
                    }
                    // Call Close when done reading.
                    reader.Close();


                    searchResult.totalRecords = searchCriteria.length == 0 ? searchResult.records == null ? 0 : searchResult.records.Count() : count_;

                }

            }

            return searchResult;
        }
    }
}