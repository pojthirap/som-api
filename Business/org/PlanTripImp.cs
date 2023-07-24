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
using System.Data;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.plan;
using static MyFirstAzureWebApp.Models.plan.CreatePlanTripModel;
using static MyFirstAzureWebApp.Models.plan.UpdatePlanTripModel;
using MyFirstAzureWebApp.exception;
using MyFirstAzureWebApp.enumval;

namespace MyFirstAzureWebApp.Business.org
{

    public class PlanTripImp : IPlanTrip
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<SearchPlanTripForSaleVisitCustom>> searchPlanTripForSaleVisit(SearchCriteriaBase<SearchPlanTripForSaleVisitCriteria> searchCriteria, UserProfileForBack userProfileForBack)
        {

            EntitySearchResultBase<SearchPlanTripForSaleVisitCustom> searchResult = new EntitySearchResultBase<SearchPlanTripForSaleVisitCustom>();
            List<SearchPlanTripForSaleVisitCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchPlanTripForSaleVisitCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select P.*,E.FIRST_NAME,E.LAST_NAME,CFG.LOV_NAME_TH STATUS_DESC  ");
                queryBuilder.AppendFormat(" ,(SELECT SUM(TP.VISIT_CALC_KM) FROM PLAN_TRIP_PROSPECT TP WHERE TP.PLAN_TRIP_ID = P.PLAN_TRIP_ID) +P.STOP_CALC_KM TOTAL_KM_SYSTEM ");
                queryBuilder.AppendFormat(" from PLAN_TRIP P ");
                queryBuilder.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = P.CREATE_USER ");
                queryBuilder.AppendFormat(" inner join MS_CONFIG_LOV CFG on CFG.LOV_KEYWORD = 'PLAN_TRIP_STATUS' and CFG.LOV_KEYVALUE = P.STATUS ");
                queryBuilder.AppendFormat(" where P.STATUS in ( @APPROVE, @ASSIGN, @COMPLETED ) ");
                //queryBuilder.AppendFormat(" and CONVERT(varchar,P.PLAN_TRIP_DATE,23)= CONVERT(varchar,dbo.GET_SYSDATETIME(),23) ");
                //queryBuilder.AppendFormat(" and CONVERT(varchar, P.PLAN_TRIP_DATE,23) >= CONVERT(varchar, DATEADD(DD, -cast(dbo.GET_MS_CONFIG_PARAM('DAY_DELAY_SITE_VISIT') as int), dbo.GET_SYSDATETIME()), 23) ");
                queryBuilder.AppendFormat(" and CONVERT(varchar, P.PLAN_TRIP_DATE,23) between CONVERT(varchar, DATEADD(DD,-cast(dbo.GET_MS_CONFIG_PARAM('DAY_DELAY_SITE_VISIT') as int),dbo.GET_SYSDATETIME()),23) and dbo.GET_SYSDATETIME() ");
                queryBuilder.AppendFormat(" and ISNULL(P.ASSIGN_EMP_ID, P.CREATE_USER) = @EmpId ");
                QueryUtils.addParam(command, "EmpId", userProfileForBack.getEmpId());// Add new
                QueryUtils.addParam(command, "APPROVE", PlanTripStatus.APPROVE.ToString("d"));// Add new
                QueryUtils.addParam(command, "ASSIGN", PlanTripStatus.ASSIGN.ToString("d"));// Add new
                QueryUtils.addParam(command, "COMPLETED", PlanTripStatus.COMPLETED.ToString("d"));// Add new

                //QueryUtils.addParam(command, "PLAN_TRIP_STATUS_APPROVE", PlanTripStatus.APPROVE.ToString("d"));// Add new
                //QueryUtils.addParam(command, "PLAN_TRIP_STATUS_ASSIGN", PlanTripStatus.ASSIGN.ToString("d"));// Add new


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY P.UPDATE_DTM DESC  ");
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
                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    //

                    List<SearchPlanTripForSaleVisitCustom> dataRecordList = new List<SearchPlanTripForSaleVisitCustom>();
                    SearchPlanTripForSaleVisitCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SearchPlanTripForSaleVisitCustom();


                        dataRecord.PlanTripId = QueryUtils.getValueAsDecimalRequired(record, "PLAN_TRIP_ID");
                        dataRecord.PlanTripName = QueryUtils.getValueAsString(record, "PLAN_TRIP_NAME");
                        dataRecord.PlanTripDate = QueryUtils.getValueAsDateTimeRequired(record, "PLAN_TRIP_DATE");
                        dataRecord.AssignEmpId = QueryUtils.getValueAsString(record, "ASSIGN_EMP_ID");
                        dataRecord.Remark = QueryUtils.getValueAsString(record, "REMARK");
                        dataRecord.StartCheckinLocId = QueryUtils.getValueAsDecimal(record, "START_CHECKIN_LOC_ID");
                        dataRecord.StartCheckinMileNo = QueryUtils.getValueAsDecimal(record, "START_CHECKIN_MILE_NO");
                        dataRecord.StartCheckinDtm = QueryUtils.getValueAsDateTime(record, "START_CHECKIN_DTM");
                        dataRecord.StartCheckoutDtm = QueryUtils.getValueAsDateTime(record, "START_CHECKOUT_DTM");
                        dataRecord.StopCheckinLocId = QueryUtils.getValueAsDecimal(record, "STOP_CHECKIN_LOC_ID");
                        dataRecord.StopCheckinMileNo = QueryUtils.getValueAsDecimal(record, "STOP_CHECKIN_MILE_NO");
                        dataRecord.StopCheckinDtm = QueryUtils.getValueAsDateTime(record, "STOP_CHECKIN_DTM");
                        dataRecord.Status = QueryUtils.getValueAsString(record, "STATUS");
                        dataRecord.StopCalcKm = QueryUtils.getValueAsDecimal(record, "STOP_CALC_KM");
                        dataRecord.FirstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                        dataRecord.LastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                        dataRecord.StatusDesc = QueryUtils.getValueAsString(record, "STATUS_DESC");
                        dataRecord.TotalKmSystem = QueryUtils.getValueAsString(record, "TOTAL_KM_SYSTEM");

                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");


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





        public async Task<int> planTripStart(PlanTripModel planTripModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PLAN_TRIP SET START_CHECKIN_LOC_ID=@START_CHECKIN_LOC_ID, START_CHECKIN_MILE_NO=@START_CHECKIN_MILE_NO, START_CHECKIN_DTM=dbo.GET_SYSDATETIME(), START_CHECKOUT_DTM=dbo.GET_SYSDATETIME(), STOP_CHECKIN_LOC_ID=NULL, STOP_CHECKIN_MILE_NO=NULL, STOP_CHECKIN_DTM=NULL, STOP_CALC_KM=NULL, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PLAN_TRIP_ID=@PLAN_TRIP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("START_CHECKIN_LOC_ID", planTripModel.StartCheckinLocId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("START_CHECKIN_MILE_NO", planTripModel.StartCheckinMileNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_ID", planTripModel.PlanTripId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", planTripModel.getUserName()));// Add New
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


        public async Task<int> planTripFinish(PlanTripModel planTripModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PLAN_TRIP SET STOP_CHECKIN_LOC_ID=@STOP_CHECKIN_LOC_ID, STOP_CHECKIN_MILE_NO=@STOP_CHECKIN_MILE_NO, STOP_CHECKIN_DTM=dbo.GET_SYSDATETIME(), STOP_CALC_KM=@STOP_CALC_KM, STATUS=@STATUS, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PLAN_TRIP_ID=@PLAN_TRIP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("STOP_CHECKIN_LOC_ID", planTripModel.StopCheckinLocId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("STOP_CHECKIN_MILE_NO", planTripModel.StopCheckinMileNo));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("STOP_CALC_KM", planTripModel.StopCalcKm));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_ID", planTripModel.PlanTripId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", planTripModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("STATUS", PlanTripStatus.COMPLETED.ToString("d")));// Add New
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


        public async Task<KmTotalPlanTrip> getKmTotalPlanTripFinish(string PlanTripId)
        {


            string kmTotal=null;
            KmTotalPlanTrip kmTotalPlanTrip = new KmTotalPlanTrip();
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select sum(VISIT_CALC_KM) + (select stop_calc_km from PLAN_TRIP PT where PT.PLAN_TRIP_ID = @PLAN_TRIP_ID) KM_TOTAL from PLAN_TRIP_PROSPECT PP  ");
                QueryUtils.addParam(command, "PLAN_TRIP_ID", PlanTripId);// Add new
                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        kmTotal = QueryUtils.getValueAsString(record, "KM_TOTAL");
                        kmTotalPlanTrip.kmTotal = kmTotal;
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return kmTotalPlanTrip;
        }





    public class KmTotalPlanTrip
        {
            public string kmTotal { get; set; }
        }





        public async Task<EntitySearchResultBase<ViewPlanTripCustom>> viewPlanTrip(SearchCriteriaBase<ViewPlanTripCriteria> searchCriteria)
        {

            EntitySearchResultBase<ViewPlanTripCustom> searchResult = new EntitySearchResultBase<ViewPlanTripCustom>();
            
            ViewPlanTripCustom viewPlanTripCustom = new ViewPlanTripCustom();
            PlanTrip planTrip = null;
            List<PlanTripProspect> listPlanTripProspect = new List<PlanTripProspect>();

            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                ViewPlanTripCriteria o = searchCriteria.model;



                // Set objectResponse.planTrip
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select * ");
                queryBuilder.AppendFormat(" from PLAN_TRIP P  ");
                queryBuilder.AppendFormat(" where P.PLAN_TRIP_ID = @PlanTripId  ");
                QueryUtils.addParam(command, "PlanTripId", o.PlanTripId);// Add new


                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    //

                    PlanTrip dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new PlanTrip();

                        dataRecord.PlanTripId = QueryUtils.getValueAsDecimalRequired(record, "PLAN_TRIP_ID");
                        dataRecord.PlanTripName = QueryUtils.getValueAsString(record, "PLAN_TRIP_NAME");
                        dataRecord.PlanTripDate = QueryUtils.getValueAsDateTimeRequired(record, "PLAN_TRIP_DATE");
                        dataRecord.AssignEmpId = QueryUtils.getValueAsString(record, "ASSIGN_EMP_ID");
                        dataRecord.Remark = QueryUtils.getValueAsString(record, "REMARK");
                        dataRecord.StartCheckinLocId = QueryUtils.getValueAsDecimal(record, "START_CHECKIN_LOC_ID");
                        dataRecord.StartCheckinMileNo = QueryUtils.getValueAsDecimal(record, "START_CHECKIN_MILE_NO");
                        dataRecord.StartCheckinDtm = QueryUtils.getValueAsDateTime(record, "START_CHECKIN_DTM");
                        dataRecord.StartCheckoutDtm = QueryUtils.getValueAsDateTime(record, "START_CHECKOUT_DTM");
                        dataRecord.StopCheckinLocId = QueryUtils.getValueAsDecimal(record, "STOP_CHECKIN_LOC_ID");
                        dataRecord.StopCheckinMileNo = QueryUtils.getValueAsDecimal(record, "STOP_CHECKIN_MILE_NO");
                        dataRecord.StopCheckinDtm = QueryUtils.getValueAsDateTime(record, "STOP_CHECKIN_DTM");
                        dataRecord.Status = QueryUtils.getValueAsString(record, "STATUS");
                        dataRecord.StopCalcKm = QueryUtils.getValueAsDecimal(record, "STOP_CALC_KM");
                        dataRecord.MergPlanTripId = QueryUtils.getValueAsDecimal(record, "MERG_PLAN_TRIP_ID");

                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM");


                        planTrip = dataRecord;
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //


                }



                // PLAN_TRIP_PROSPECT
                queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select PP.*,ISNULL(PA.ACC_NAME,L.LOC_NAME_TH) ACC_NAME,PA.CUST_CODE,ISNULL(PD.LATITUDE,L.LATITUDE) LATITUDE,ISNULL(PD.LONGITUDE,L.LONGITUDE) LONGITUDE ");
                queryBuilder.AppendFormat("  ,IIF(PP.REASON_NOT_VISIT_ID is not null,'N',IIF(PP.VISIT_CHECKOUT_DTM is not null,'N','Y')) OPEN_FLAG ");
                //queryBuilder.AppendFormat("  ,PP.LOC_REMARK,PP.LOC_ID,PP.VISIT_LATITUDE,PP.VISIT_LONGITUDE ");
                queryBuilder.AppendFormat(" from PLAN_TRIP_PROSPECT PP ");
                queryBuilder.AppendFormat(" left  join PROSPECT P on P.PROSPECT_ID = PP.PROSP_ID ");
                queryBuilder.AppendFormat(" left  join PROSPECT_ACCOUNT PA on PA.PROSP_ACC_ID = P.PROSP_ACC_ID ");
                queryBuilder.AppendFormat(" left  join PROSPECT_ADDRESS PD on PD.PROSP_ACC_ID = PA.PROSP_ACC_ID and PD.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" left join MS_LOCATION L on L.LOC_ID = PP.LOC_ID ");
                queryBuilder.AppendFormat(" where PP.PLAN_TRIP_ID = @PlanTripId ");
                queryBuilder.AppendFormat(" order by PP.PLAN_TRIP_PROSP_ID ");
                //QueryUtils.addParam(command, "PlanTripId", o.PlanTripId);// Add new


                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    //

                    PlanTripProspect dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new PlanTripProspect();

                        dataRecord.PlanTripProspId = QueryUtils.getValueAsDecimalRequired(record, "PLAN_TRIP_PROSP_ID");
                        dataRecord.PlanTripId = QueryUtils.getValueAsDecimal(record, "PLAN_TRIP_ID");
                        dataRecord.ProspId = QueryUtils.getValueAsDecimal(record, "PROSP_ID");
                        dataRecord.PlanStartTime = QueryUtils.getValueAsDateTime(record, "PLAN_START_TIME");
                        dataRecord.PlanEndTime = QueryUtils.getValueAsDateTime(record, "PLAN_END_TIME");
                        dataRecord.OrderNo = QueryUtils.getValueAsDecimal(record, "ORDER_NO");
                        dataRecord.AdhocFlag = QueryUtils.getValueAsString(record, "ADHOC_FLAG");
                        dataRecord.MergFlag = QueryUtils.getValueAsString(record, "MERG_FLAG");
                        //dataRecord.VisitLatitude = QueryUtils.getValueAsString(record, "VISIT_LATITUDE");
                        //dataRecord.VisitLongitude = QueryUtils.getValueAsString(record, "VISIT_LONGITUDE");
                        dataRecord.VisitCheckinMileNo = QueryUtils.getValueAsDecimal(record, "VISIT_CHECKIN_MILE_NO");
                        dataRecord.VisitCheckinDtm = QueryUtils.getValueAsDateTime(record, "VISIT_CHECKIN_DTM");
                        dataRecord.VisitCheckoutDtm = QueryUtils.getValueAsDateTime(record, "VISIT_CHECKOUT_DTM");
                        dataRecord.Remind = QueryUtils.getValueAsString(record, "REMIND");
                        dataRecord.ReasonNotVisitId = QueryUtils.getValueAsDecimal(record, "REASON_NOT_VISIT_ID");
                        dataRecord.ReasonNotVisitRemark = QueryUtils.getValueAsString(record, "REASON_NOT_VISIT_REMARK");
                        dataRecord.VisitCalcKm = QueryUtils.getValueAsDecimal(record, "VISIT_CALC_KM");
                        dataRecord.MergPlanTripProspId = QueryUtils.getValueAsDecimal(record, "MERG_PLAN_TRIP_PROSP_ID");
                        dataRecord.AccName = QueryUtils.getValueAsString(record, "ACC_NAME");
                        dataRecord.CustCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        dataRecord.Latitude = QueryUtils.getValueAsString(record, "LATITUDE");
                        dataRecord.Longitude = QueryUtils.getValueAsString(record, "LONGITUDE");
                        dataRecord.OpenFlag = QueryUtils.getValueAsString(record, "OPEN_FLAG");
                        dataRecord.LocRemark = QueryUtils.getValueAsString(record, "LOC_REMARK");
                        dataRecord.LocId = QueryUtils.getValueAsDecimal(record, "LOC_ID");
                        dataRecord.VisitLatitude = QueryUtils.getValueAsString(record, "VISIT_LATITUDE");
                        dataRecord.VisitLongitude = QueryUtils.getValueAsString(record, "VISIT_LONGITUDE");


                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");


                        listPlanTripProspect.Add(dataRecord);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //


                }

                viewPlanTripCustom.PlanTrip = planTrip;
                viewPlanTripCustom.ListPlanTripProspect = listPlanTripProspect;
                if (planTrip != null)
                {
                    List<ViewPlanTripCustom> lst = new List<ViewPlanTripCustom>();
                    lst.Add(viewPlanTripCustom);
                    searchResult.data = lst;
                    searchResult.totalRecords = lst.Count;
                }
                


            }
            return searchResult;
        }





        public async Task<EntitySearchResultBase<PlanTrip>> searchPlanTrip(SearchCriteriaBase<SearchPlanTripCriteria> searchCriteria, UserProfileForBack userProfile)
        {

            EntitySearchResultBase<PlanTrip> searchResult = new EntitySearchResultBase<PlanTrip>();
            List<PlanTrip> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SearchPlanTripCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select P.*, E.TITLE_NAME,E.FIRST_NAME,E.LAST_NAME   ");
                queryBuilder.AppendFormat(" ,IIF(P.STOP_CALC_KM IS NOT NULL, 'Y', 'N') COMPLET_FLAG ");
                queryBuilder.AppendFormat(" from PLAN_TRIP P ");
                queryBuilder.AppendFormat(" inner join ADM_EMPLOYEE E on E.EMP_ID = P.CREATE_USER ");
                queryBuilder.AppendFormat(" where P.STATUS != @PLAN_TRIP_STATUS_CANCEL ");
                queryBuilder.AppendFormat(" and(");
                queryBuilder.AppendFormat("      ISNULL(P.ASSIGN_EMP_ID,P.CREATE_USER) = @EmpId ");
                queryBuilder.AppendFormat("      OR(");
                //queryBuilder.AppendFormat("         ( (E.APPROVE_EMP_ID = @EmpId and P.STATUS = @PLAN_TRIP_STATUS_WAITING_FOR_APPROVE) ");
                queryBuilder.AppendFormat("         ((E.APPROVE_EMP_ID = @EmpId and(P.STATUS in (@PLAN_TRIP_STATUS_WAITING_FOR_APPROVE, @PLAN_TRIP_STATUS_APPROVE, @PLAN_TRIP_STATUS_REJECT,@PLAN_TRIP_STATUS_COMPLETED,@PLAN_TRIP_STATUS_INCOMPLETED))) ");
                queryBuilder.AppendFormat("          OR EXISTS(");
                queryBuilder.AppendFormat("             select 1 ");
                queryBuilder.AppendFormat("             from ADM_EMPLOYEE E ");
                queryBuilder.AppendFormat("             inner join ADM_GROUP_USER GU ON GU.EMP_ID = E.EMP_ID ");
                queryBuilder.AppendFormat("             inner join ORG_SALE_GROUP SG on SG.GROUP_CODE = E.GROUP_CODE ");
                queryBuilder.AppendFormat("             inner join ORG_TERRITORY T ON T.TERRITORY_ID = SG.TERRITORY_ID ");
                queryBuilder.AppendFormat("             where E.EMP_ID = ISNULL(P.ASSIGN_EMP_ID,P.CREATE_USER)  ");
                queryBuilder.AppendFormat("             and T.MANAGER_EMP_ID = @EmpId ");
                queryBuilder.AppendFormat("             and  ((GU.GROUP_ID != @GROUP_ROLE_SUPEPVISOR and P.STATUS = @PLAN_TRIP_STATUS_WAITING_FOR_APPROVE) OR (GU.GROUP_ID = @GROUP_ROLE_SUPEPVISOR and P.STATUS = @PLAN_TRIP_STATUS_APPROVE)) ");
                /*queryBuilder.AppendFormat("                     select 1 ");
                queryBuilder.AppendFormat("                     from ORG_SALE_TERRITORY ST ");
                queryBuilder.AppendFormat("                     inner join ORG_TERRITORY T ON T.TERRITORY_ID = ST.TERRITORY_ID ");
                queryBuilder.AppendFormat("                     inner join ADM_GROUP_USER GU ON GU.EMP_ID = ST.EMP_ID ");
                queryBuilder.AppendFormat("                     where ST.EMP_ID = ISNULL(P.ASSIGN_EMP_ID,P.CREATE_USER)  ");
                queryBuilder.AppendFormat("                     and T.MANAGER_EMP_ID = @EmpId ");
                queryBuilder.AppendFormat("                     and((GU.GROUP_ID != @GROUP_ROLE_SUPEPVISOR and P.STATUS = @PLAN_TRIP_STATUS_WAITING_FOR_APPROVE) OR(GU.GROUP_ID = @GROUP_ROLE_SUPEPVISOR and P.STATUS = @PLAN_TRIP_STATUS_APPROVE)) ");*/

                queryBuilder.AppendFormat("         ) ) )  ");//-- Note PLAN_TRIP_STATUS.WAITING_FOR_APPROVE -- ตรงนี้ทำเป็น ENUM Value = '2' Fix
                queryBuilder.AppendFormat(" ) ");
                QueryUtils.addParam(command, "EmpId", userProfile.getEmpId());
                QueryUtils.addParam(command, "PLAN_TRIP_STATUS_CANCEL", PlanTripStatus.CANCEL.ToString("d"));
                QueryUtils.addParam(command, "PLAN_TRIP_STATUS_WAITING_FOR_APPROVE", PlanTripStatus.WAITING_FOR_APPROVE.ToString("d"));
                QueryUtils.addParam(command, "PLAN_TRIP_STATUS_APPROVE", PlanTripStatus.APPROVE.ToString("d"));
                QueryUtils.addParam(command, "GROUP_ROLE_SUPEPVISOR", GroupRole.SUPEPVISOR.ToString("d"));
                QueryUtils.addParam(command, "PLAN_TRIP_STATUS_REJECT", PlanTripStatus.REJECT.ToString("d"));
                QueryUtils.addParam(command, "PLAN_TRIP_STATUS_COMPLETED", PlanTripStatus.COMPLETED.ToString("d"));
                QueryUtils.addParam(command, "PLAN_TRIP_STATUS_INCOMPLETED", PlanTripStatus.INCOMPLETED.ToString("d"));
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.Calendar))
                    {
                        queryBuilder.AppendFormat(" and LEFT(CONVERT(varchar,plan_trip_date,112),6) = @CALENDAR  "); // -- Fotmat YYYYMM
                        QueryUtils.addParam(command, "CALENDAR", o.Calendar);
                    }
                    if (!String.IsNullOrEmpty(o.PlanTripDate))
                    {
                        queryBuilder.AppendFormat(" and CONVERT(varchar,P.PLAN_TRIP_DATE,112)= @PLAN_TRIP_DATE  "); // -- Fotmat YYYYMM
                        QueryUtils.addParam(command, "PLAN_TRIP_DATE", o.PlanTripDate);
                    }
                    if (o.Status !=null && o.Status.Length !=0)
                    {
                        string statusStr = "";
                        List<string> statusList = new List<string>();
                        foreach(string s in o.Status)
                        {
                            statusList.Add(s);
                        }
                        statusStr = String.Join(",", statusList);
                        queryBuilder.AppendFormat(" and P.STATUS IN (" + QueryUtils.getParamIn("statusStr", statusStr) + ")  ");
                        QueryUtils.addParamIn(command, "statusStr", statusStr);
                    }
                    if (!String.IsNullOrEmpty(o.AssignEmpId))
                    {
                        queryBuilder.AppendFormat(" and P.CREATE_USER = @ASSIGN_EMP_ID  ");
                        QueryUtils.addParam(command, "ASSIGN_EMP_ID", o.AssignEmpId);
                    }
                }



                // For Paging
                queryBuilder.AppendFormat(" order by P.UPDATE_DTM DESC  ");
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
                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    //

                    List<PlanTrip> dataRecordList = new List<PlanTrip>();
                    PlanTrip dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new PlanTrip();


                        dataRecord.PlanTripId = QueryUtils.getValueAsDecimalRequired(record, "PLAN_TRIP_ID");
                        dataRecord.PlanTripName = QueryUtils.getValueAsString(record, "PLAN_TRIP_NAME");
                        dataRecord.PlanTripDate = QueryUtils.getValueAsDateTime(record, "PLAN_TRIP_DATE");
                        dataRecord.AssignEmpId = QueryUtils.getValueAsString(record, "ASSIGN_EMP_ID");
                        dataRecord.Remark = QueryUtils.getValueAsString(record, "REMARK");
                        dataRecord.StartCheckinLocId = QueryUtils.getValueAsDecimal(record, "START_CHECKIN_LOC_ID");
                        dataRecord.StartCheckinMileNo = QueryUtils.getValueAsDecimal(record, "START_CHECKIN_MILE_NO");
                        dataRecord.StartCheckinDtm = QueryUtils.getValueAsDateTime(record, "START_CHECKIN_DTM");
                        dataRecord.StartCheckoutDtm = QueryUtils.getValueAsDateTime(record, "START_CHECKOUT_DTM");
                        dataRecord.StopCheckinLocId = QueryUtils.getValueAsDecimal(record, "STOP_CHECKIN_LOC_ID");
                        dataRecord.StopCheckinMileNo = QueryUtils.getValueAsDecimal(record, "STOP_CHECKIN_MILE_NO");
                        dataRecord.StopCheckinDtm = QueryUtils.getValueAsDateTime(record, "STOP_CHECKIN_DTM");
                        dataRecord.Status = QueryUtils.getValueAsString(record, "STATUS");
                        dataRecord.StopCalcKm = QueryUtils.getValueAsDecimal(record, "STOP_CALC_KM");
                        dataRecord.MergPlanTripId = QueryUtils.getValueAsDecimal(record, "MERG_PLAN_TRIP_ID");
                        dataRecord.TitleName = QueryUtils.getValueAsString(record, "TITLE_NAME");
                        dataRecord.FirstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                        dataRecord.LastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                        dataRecord.CompletFlag = QueryUtils.getValueAsString(record, "COMPLET_FLAG");
                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");

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




        public async Task<EntitySearchResultBase<AdmEmployee>> getEmpForAssignPlanTrip(SearchCriteriaBase<GetEmpForAssignPlanTripCriteria> searchCriteria, UserProfileForBack userProfile)
        {

            EntitySearchResultBase<AdmEmployee> searchResult = new EntitySearchResultBase<AdmEmployee>();
            List<AdmEmployee> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                GetEmpForAssignPlanTripCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select E.*");
                queryBuilder.AppendFormat(" from dbo.ADM_EMPLOYEE E   ");
                queryBuilder.AppendFormat(" where E.GROUP_CODE = @GroupCode   ");
                QueryUtils.addParam(command, "GroupCode", userProfile.SaleGroupSaleOfficeCustom.data[0].GroupCode);


                // For Paging
                queryBuilder.AppendFormat(" ORDER BY EMP_ID  ");
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
                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    //

                    List<AdmEmployee> dataRecordList = new List<AdmEmployee>();
                    AdmEmployee dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new AdmEmployee();


                        dataRecord.EmpId = QueryUtils.getValueAsString(record, "EMP_ID");
                        dataRecord.CompanyCode = QueryUtils.getValueAsString(record, "COMPANY_CODE");
                        dataRecord.JobTitle = QueryUtils.getValueAsString(record, "JOB_TITLE");
                        dataRecord.TitleName = QueryUtils.getValueAsString(record, "TITLE_NAME");
                        dataRecord.FirstName = QueryUtils.getValueAsString(record, "FIRST_NAME");
                        dataRecord.LastName = QueryUtils.getValueAsString(record, "LAST_NAME");
                        dataRecord.Gender = QueryUtils.getValueAsString(record, "GENDER");
                        dataRecord.Street = QueryUtils.getValueAsString(record, "STREET");
                        dataRecord.TellNo = QueryUtils.getValueAsString(record, "TELL_NO");
                        dataRecord.CountryName = QueryUtils.getValueAsString(record, "COUNTRY_NAME");
                        dataRecord.ProvinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                        dataRecord.GroupCode = QueryUtils.getValueAsString(record, "GROUP_CODE");
                        dataRecord.DistrictName = QueryUtils.getValueAsString(record, "DISTRICT_NAME");
                        dataRecord.SubdistrictName = QueryUtils.getValueAsString(record, "SUBDISTRICT_NAME");
                        dataRecord.PostCode = QueryUtils.getValueAsString(record, "POST_CODE");
                        dataRecord.Email = QueryUtils.getValueAsString(record, "EMAIL");
                        dataRecord.Status = QueryUtils.getValueAsString(record, "STATUS");
                        dataRecord.ApproveEmpId = QueryUtils.getValueAsString(record, "APPROVE_EMP_ID");
                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        dataRecord.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        dataRecord.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");

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






        public async Task<PlanTrip> createPlanTrip(CreatePlanTripModel createPlanTripModel, UserProfileForBack userProfile)
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
                        context.Database.ExecuteSqlRaw("set @result = next value for PLAN_TRIP_SEQ", p);
                        var VAL_PLAN_TRIP_SEQ = (int)p.Value;

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO PLAN_TRIP (PLAN_TRIP_ID, PLAN_TRIP_NAME, PLAN_TRIP_DATE, ASSIGN_EMP_ID, REMARK, STATUS, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@PLAN_TRIP_ID ,@PLAN_TRIP_NAME, @PLAN_TRIP_DATE, @ASSIGN_EMP_ID, @REMARK, @STATUS, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_ID", VAL_PLAN_TRIP_SEQ));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_NAME", createPlanTripModel.PlanTrip.PlanTripName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_DATE", createPlanTripModel.PlanTrip.PlanTripDate));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ASSIGN_EMP_ID", createPlanTripModel.PlanTrip.AssignEmpId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("REMARK", createPlanTripModel.PlanTrip.Remark));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("STATUS", createPlanTripModel.PlanTrip.Status));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        foreach (PlanTripProspectCreateModel prospect in createPlanTripModel.ListProspect)
                        {
                            sqlParameters = new List<SqlParameter>();// Add New
                            p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                            p.Direction = System.Data.ParameterDirection.Output;
                            context.Database.ExecuteSqlRaw("set @result = next value for PLAN_TRIP_PROSPECT_SEQ", p);
                            var VAL_PLAN_TRIP_PROSPECT_SEQ = (int)p.Value;

                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat("INSERT INTO PLAN_TRIP_PROSPECT (PLAN_TRIP_PROSP_ID, PLAN_TRIP_ID, PROSP_ID, LOC_ID, LOC_REMARK, PLAN_START_TIME, PLAN_END_TIME, ORDER_NO, ADHOC_FLAG, MERG_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                            queryBuilder.AppendFormat("VALUES(@PLAN_TRIP_PROSP_ID ,@PLAN_TRIP_ID, @PROSP_ID, @LOC_ID, @LOC_REMARK, @PLAN_START_TIME, @PLAN_END_TIME, @ORDER_NO, 'N', 'N', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                            sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_PROSP_ID", VAL_PLAN_TRIP_PROSPECT_SEQ));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_ID", VAL_PLAN_TRIP_SEQ));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_ID", prospect.ProspId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("LOC_ID", prospect.LocId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("LOC_REMARK", prospect.LocRemark));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_START_TIME", prospect.PlanStartTime));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_END_TIME", prospect.PlanEndTime));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_NO", prospect.OrderNo));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                            if (prospect.ListTask != null && prospect.ListTask.Count != 0)
                            {
                                foreach (PlanTripTaskForCreatePlanTrip task in prospect.ListTask)
                                {
                                    sqlParameters = new List<SqlParameter>();// Add New
                                    queryBuilder = new StringBuilder();
                                    queryBuilder.AppendFormat("INSERT INTO PLAN_TRIP_TASK (PLAN_TRIP_TASK_ID, PLAN_TRIP_PROSP_ID, TASK_TYPE, TP_STOCK_CARD_ID, TP_SA_FORM_ID, TP_APP_FORM_ID, REQUIRE_FLAG, ORDER_NO, ADHOC_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                                    queryBuilder.AppendFormat("VALUES(NEXT VALUE FOR PLAN_TRIP_TASK_SEQ ,@PLAN_TRIP_PROSP_ID, @TASK_TYPE, @TP_STOCK_CARD_ID, @TP_SA_FORM_ID, @TP_APP_FORM_ID, @REQUIRE_FLAG, @ORDER_NO, 'N', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                                    sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_PROSP_ID", VAL_PLAN_TRIP_PROSPECT_SEQ));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("TASK_TYPE", task.TaskType));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("TP_STOCK_CARD_ID", task.TpStockCardId));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("TP_SA_FORM_ID", task.TpSaFormId));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("TP_APP_FORM_ID", task.TpAppFormId));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("REQUIRE_FLAG", task.RequireFlag));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_NO", task.OrderNo));// Add New
                                    sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                                    log.Debug("Query:" + queryBuilder.ToString());
                                    Console.WriteLine("Query:" + queryBuilder.ToString());
                                    numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                    log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                    Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                                }
                            }
                        }

                        transaction.Commit();
                        PlanTrip re = new PlanTrip();
                        re.PlanTripId = VAL_PLAN_TRIP_SEQ;
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



        public async Task<int> updPlanTrip(UpdatePlanTripModel updatePlanTripModel, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var VAL_PLAN_TRIP_SEQ = updatePlanTripModel.PlanTrip.PlanTripId;
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PLAN_TRIP SET PLAN_TRIP_NAME=@PLAN_TRIP_NAME, PLAN_TRIP_DATE=@PLAN_TRIP_DATE, ASSIGN_EMP_ID=@ASSIGN_EMP_ID, REMARK=@REMARK, STATUS=@STATUS, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PLAN_TRIP_ID=@PLAN_TRIP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_NAME", updatePlanTripModel.PlanTrip.PlanTripName));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_DATE", updatePlanTripModel.PlanTrip.PlanTripDate));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ASSIGN_EMP_ID", updatePlanTripModel.PlanTrip.AssignEmpId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("REMARK", updatePlanTripModel.PlanTrip.Remark));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("STATUS", updatePlanTripModel.PlanTrip.Status));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_ID", VAL_PLAN_TRIP_SEQ));// Add New
                        string queryStr = queryBuilder.ToString();
                        queryStr = QueryUtils.cutStringNull(queryStr);
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE TT FROM PLAN_TRIP_TASK TT WHERE TT.PLAN_TRIP_PROSP_ID IN (SELECT TP.PLAN_TRIP_PROSP_ID FROM PLAN_TRIP_PROSPECT TP WHERE TP.PLAN_TRIP_ID = @VAL_PLAN_TRIP_SEQ) ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PLAN_TRIP_SEQ", VAL_PLAN_TRIP_SEQ));// Add New
                        queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM PLAN_TRIP_PROSPECT  WHERE PLAN_TRIP_ID= @VAL_PLAN_TRIP_SEQ ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PLAN_TRIP_SEQ", VAL_PLAN_TRIP_SEQ));// Add New
                        queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        if (updatePlanTripModel.ListProspect != null)
                        {
                            foreach (PlanTripProspectUpdateModel prospect in updatePlanTripModel.ListProspect)
                            {
                                sqlParameters = new List<SqlParameter>();// Add New
                                var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                                p.Direction = System.Data.ParameterDirection.Output;
                                context.Database.ExecuteSqlRaw("set @result = next value for PLAN_TRIP_PROSPECT_SEQ", p);
                                var VAL_PLAN_TRIP_PROSPECT_SEQ = (int)p.Value;

                                queryBuilder = new StringBuilder();
                                queryBuilder.AppendFormat("INSERT INTO PLAN_TRIP_PROSPECT (PLAN_TRIP_PROSP_ID, PLAN_TRIP_ID, PROSP_ID, LOC_ID, LOC_REMARK, PLAN_START_TIME, PLAN_END_TIME, ORDER_NO, ADHOC_FLAG, MERG_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                                queryBuilder.AppendFormat("VALUES(@PLAN_TRIP_PROSP_ID ,@PLAN_TRIP_ID, @PROSP_ID, @LOC_ID, @LOC_REMARK, @PLAN_START_TIME, @PLAN_END_TIME, @ORDER_NO, 'N', 'N', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                                sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_PROSP_ID", VAL_PLAN_TRIP_PROSPECT_SEQ));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_ID", VAL_PLAN_TRIP_SEQ));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_ID", prospect.ProspId));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("LOC_ID", prospect.LocId));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("LOC_REMARK", prospect.LocRemark));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_START_TIME", prospect.PlanStartTime));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_END_TIME", prospect.PlanEndTime));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_NO", prospect.OrderNo));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                                log.Debug("Query:" + queryBuilder.ToString());
                                Console.WriteLine("Query:" + queryBuilder.ToString());
                                numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                                if (prospect.ListTask != null)
                                {
                                    foreach (PlanTripTaskForUpdatePlanTrip task in prospect.ListTask)
                                    {
                                        sqlParameters = new List<SqlParameter>();// Add New
                                        queryBuilder = new StringBuilder();
                                        queryBuilder.AppendFormat("INSERT INTO PLAN_TRIP_TASK (PLAN_TRIP_TASK_ID, PLAN_TRIP_PROSP_ID, TASK_TYPE, TP_STOCK_CARD_ID, TP_SA_FORM_ID, TP_APP_FORM_ID, REQUIRE_FLAG, ORDER_NO, ADHOC_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                                        queryBuilder.AppendFormat("VALUES(NEXT VALUE FOR PLAN_TRIP_TASK_SEQ ,@PLAN_TRIP_PROSP_ID, @TASK_TYPE, @TP_STOCK_CARD_ID, @TP_SA_FORM_ID, @TP_APP_FORM_ID, @REQUIRE_FLAG, @ORDER_NO, 'N', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_PROSP_ID", VAL_PLAN_TRIP_PROSPECT_SEQ));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("TASK_TYPE", task.TaskType));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_STOCK_CARD_ID", task.TpStockCardId));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_SA_FORM_ID", task.TpSaFormId));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_APP_FORM_ID", task.TpAppFormId));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("REQUIRE_FLAG", task.RequireFlag));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("ORDER_NO", task.OrderNo));// Add New
                                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                                        log.Debug("Query:" + queryBuilder.ToString());
                                        Console.WriteLine("Query:" + queryBuilder.ToString());
                                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                                    }
                                }
                            }
                        }


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




        public async Task<int> mergPlanTrip(MergPlanTripModel mergPlanTripModel, UserProfileForBack userProfile, string language)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        string VAL_PLAN_TRIP_ID = await getPlantripIdForMergPlantrip(mergPlanTripModel.MergPlanTripId, userProfile.getEmpId());
                        if (String.IsNullOrEmpty(VAL_PLAN_TRIP_ID))
                        {
                            List<String> errorParam = new List<string>();
                            ServiceException se = new ServiceException("W_0005", errorParam, ObjectFacory.getCultureInfo(language));
                            throw se;
                        }

                        string VAL_INVALID_DATA = await getInvalidDataForMergPlantrip(mergPlanTripModel.MergPlanTripId, VAL_PLAN_TRIP_ID);
                        if ("Y".Equals(VAL_INVALID_DATA))
                        {
                            ServiceException se = new ServiceException("W_0013", ObjectFacory.getCultureInfo(language));
                            throw se;
                        }

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        List<string> PlanTripProspIdList = await getPlantripProspectIdForMergPlantrip(mergPlanTripModel.MergPlanTripId);
                        int numberOfRowInserted = 0;
                        if (PlanTripProspIdList != null && PlanTripProspIdList.Count != 0) {
                            foreach (string planTripProspId in PlanTripProspIdList)
                            {
                                sqlParameters = new List<SqlParameter>();// Add New
                                var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                                p.Direction = System.Data.ParameterDirection.Output;
                                context.Database.ExecuteSqlRaw("set @result = next value for PLAN_TRIP_PROSPECT_SEQ", p);
                                var nexPlanTripProspId = (int)p.Value;

                                sqlParameters = new List<SqlParameter>();// Add New
                                queryBuilder = new StringBuilder();
                                queryBuilder.AppendFormat(" INSERT INTO PLAN_TRIP_PROSPECT ([PLAN_TRIP_PROSP_ID], [PLAN_TRIP_ID], [PROSP_ID], [LOC_ID], [LOC_REMARK], [PLAN_START_TIME], [PLAN_END_TIME], [ORDER_NO], [ADHOC_FLAG], [MERG_FLAG], [MERG_PLAN_TRIP_PROSP_ID], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                                queryBuilder.AppendFormat(" SELECT @nexPlanTripProspId, @VAL_PLAN_TRIP_ID, [PROSP_ID], [LOC_ID], [LOC_REMARK], [PLAN_START_TIME], [PLAN_END_TIME], [ORDER_NO], [ADHOC_FLAG], 'Y', [PLAN_TRIP_PROSP_ID], @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME() FROM PLAN_TRIP_PROSPECT WHERE PLAN_TRIP_PROSP_ID = @planTripProspId ");
                                sqlParameters.Add(QueryUtils.addSqlParameter("nexPlanTripProspId", nexPlanTripProspId));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("VAL_PLAN_TRIP_ID", VAL_PLAN_TRIP_ID));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("planTripProspId", planTripProspId));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                                log.Debug("Query:" + queryBuilder.ToString());
                                Console.WriteLine("Query:" + queryBuilder.ToString());
                                numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                                sqlParameters = new List<SqlParameter>();// Add New
                                queryBuilder = new StringBuilder();
                                queryBuilder.AppendFormat(" INSERT INTO PLAN_TRIP_TASK ([PLAN_TRIP_TASK_ID], [PLAN_TRIP_PROSP_ID], [TASK_TYPE], [TP_STOCK_CARD_ID], [TP_SA_FORM_ID], [TP_APP_FORM_ID], [REQUIRE_FLAG], [ORDER_NO], [ADHOC_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                                queryBuilder.AppendFormat(" SELECT NEXT VALUE FOR PLAN_TRIP_TASK_SEQ, @nexPlanTripProspId, [TASK_TYPE], [TP_STOCK_CARD_ID], [TP_SA_FORM_ID], [TP_APP_FORM_ID], [REQUIRE_FLAG], [ORDER_NO], [ADHOC_FLAG], @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME() FROM PLAN_TRIP_TASK WHERE PLAN_TRIP_PROSP_ID = @planTripProspId ");
                                sqlParameters.Add(QueryUtils.addSqlParameter("nexPlanTripProspId", nexPlanTripProspId));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("planTripProspId", planTripProspId));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                                log.Debug("Query:" + queryBuilder.ToString());
                                Console.WriteLine("Query:" + queryBuilder.ToString());
                                numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                            }
                        }


                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PLAN_TRIP SET MERG_PLAN_TRIP_ID=@MERG_PLAN_TRIP_ID, STATUS = @PLAN_TRIP_STATUS_MERGE, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PLAN_TRIP_ID=@MERG_PLAN_TRIP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("MERG_PLAN_TRIP_ID", mergPlanTripModel.MergPlanTripId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_STATUS_MERGE", PlanTripStatus.MERGE.ToString("d")));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
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



        public async Task<String> getPlantripIdForMergPlantrip(string mergPlanTripId, string empId)
        {
            

            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select P.PLAN_TRIP_ID ");
                queryBuilder.AppendFormat("     from PLAN_TRIP P ");
                queryBuilder.AppendFormat(" where P.status = @PLAN_TRIP_STATUS_APPROVE ");// -- ตรงนี้ทำเป็น ENUM Value = '3' Fix
                queryBuilder.AppendFormat(" and exists( ");
                queryBuilder.AppendFormat(" select 1  ");
                queryBuilder.AppendFormat(" from PLAN_TRIP T  ");
                queryBuilder.AppendFormat(" where T.PLAN_TRIP_DATE=P.PLAN_TRIP_DATE  ");
                queryBuilder.AppendFormat(" and ISNULL(T.ASSIGN_EMP_ID,T.CREATE_USER) = ISNULL(P.ASSIGN_EMP_ID,P.CREATE_USER) ");
                queryBuilder.AppendFormat(" and T.PLAN_TRIP_ID = @MERG_PLAN_TRIP_ID ");
                queryBuilder.AppendFormat(" ) ");
                QueryUtils.addParam(command, "PLAN_TRIP_STATUS_APPROVE", PlanTripStatus.APPROVE.ToString("d"));// Add new
                QueryUtils.addParam(command, "MERG_PLAN_TRIP_ID", mergPlanTripId);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "PLAN_TRIP_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }


        public async Task<String> getInvalidDataForMergPlantrip(string mergPlanTripId, string VAL_PLAN_TRIP_ID)
        {
            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select TOP 1 'Y' INVALID_DATA ");
                queryBuilder.AppendFormat(" from PLAN_TRIP_PROSPECT PP ");
                queryBuilder.AppendFormat("     where PP.PLAN_TRIP_ID = @mergPlanTripId ");
                queryBuilder.AppendFormat("     and PP.PROSP_ID IN(select T.PROSP_ID from  PLAN_TRIP_PROSPECT T where T.PLAN_TRIP_ID = @VAL_PLAN_TRIP_ID) ");
                QueryUtils.addParam(command, "mergPlanTripId", mergPlanTripId);// Add new
                QueryUtils.addParam(command, "VAL_PLAN_TRIP_ID", VAL_PLAN_TRIP_ID);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "INVALID_DATA");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }




        public async Task<List<string>> getPlantripProspectIdForMergPlantrip(string mergPlanTripId)
        {
            List<string> PlanTripProspIdList = new List<string>(); ;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select PLAN_TRIP_PROSP_ID ");
                queryBuilder.AppendFormat(" from PLAN_TRIP_PROSPECT ");
                queryBuilder.AppendFormat(" where PLAN_TRIP_ID = @mergPlanTripId ");
                QueryUtils.addParam(command, "mergPlanTripId", mergPlanTripId);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        string o = QueryUtils.getValueAsString(record, "PLAN_TRIP_PROSP_ID");
                        PlanTripProspIdList.Add(o);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return PlanTripProspIdList;
        }





        public async Task<int> cancelPlanTrip(CancelPlanTripModel cancelPlanTripModel, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PLAN_TRIP SET STATUS = @PLAN_TRIP_STATUS_CANCEL, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PLAN_TRIP_ID=@PLAN_TRIP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_STATUS_CANCEL", PlanTripStatus.CANCEL.ToString("d")));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getEmpId()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_ID", cancelPlanTripModel.PlanTripId));// Add New
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



        public async Task<int> rejectPlanTrip(RejectPlanTripModel rejectPlanTripModel, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PLAN_TRIP SET STATUS = @PLAN_TRIP_STATUS_REJECT, REMARK=@REMARK,  UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PLAN_TRIP_ID=@PLAN_TRIP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_STATUS_REJECT", PlanTripStatus.REJECT.ToString("d")));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getEmpId()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("REMARK", rejectPlanTripModel.Remark));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_ID", rejectPlanTripModel.PlanTripId));// Add New
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




        public async Task<int> approvePlanTrip(ApprovePlanTripModel approvePlanTripModel, UserProfileForBack userProfile, string language)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        string VAL_PLAN_TRIP_ID = await getPlantripIdForapprovePlanTrip(approvePlanTripModel.PlanTripId, userProfile.getEmpId());
                        if (!String.IsNullOrEmpty(VAL_PLAN_TRIP_ID))
                        {
                            List<String> errorParam = new List<string>();
                            ServiceException se = new ServiceException("W_0004", errorParam, ObjectFacory.getCultureInfo(language));
                            throw se;
                        }

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE PLAN_TRIP SET STATUS = @PLAN_TRIP_STATUS_APPROVE, REMARK=@REMARK, UPDATE_USER=@USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE PLAN_TRIP_ID=@PLAN_TRIP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_STATUS_APPROVE", PlanTripStatus.APPROVE.ToString("d")));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("REMARK", approvePlanTripModel.Remark));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getEmpId()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PLAN_TRIP_ID", approvePlanTripModel.PlanTripId));// Add New
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


        public async Task<String> getPlantripIdForapprovePlanTrip(string planTripId, string empId)
        {

            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select P.PLAN_TRIP_ID ");
                queryBuilder.AppendFormat("     from PLAN_TRIP P ");
                queryBuilder.AppendFormat(" where P.status = @PLAN_TRIP_STATUS_APPROVE ");//-- ตรงนี้ทำเป็น ENUM Value = '3' Fix
                queryBuilder.AppendFormat(" and exists( ");
                queryBuilder.AppendFormat(" select 1  ");
                queryBuilder.AppendFormat(" from PLAN_TRIP T  ");
                queryBuilder.AppendFormat(" where T.PLAN_TRIP_DATE=P.PLAN_TRIP_DATE  ");
                queryBuilder.AppendFormat(" and ISNULL(T.ASSIGN_EMP_ID,T.CREATE_USER) = ISNULL(P.ASSIGN_EMP_ID,P.CREATE_USER) ");
                queryBuilder.AppendFormat(" and T.PLAN_TRIP_ID = @PLAN_TRIP_ID ");
                queryBuilder.AppendFormat(" ) ");
                QueryUtils.addParam(command, "PLAN_TRIP_STATUS_APPROVE", PlanTripStatus.APPROVE.ToString("d"));// Add new
                QueryUtils.addParam(command, "PLAN_TRIP_ID", planTripId);// Add new

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "PLAN_TRIP_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }



        public async Task<EntitySearchResultBase<GetLastRemindPlanTripProspectCustom>> getLastRemindPlanTripProspect(SearchCriteriaBase<GetLastRemindPlanTripProspectCriteria> searchCriteria)
        {

            EntitySearchResultBase<GetLastRemindPlanTripProspectCustom> searchResult = new EntitySearchResultBase<GetLastRemindPlanTripProspectCustom>();
            List<GetLastRemindPlanTripProspectCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                GetLastRemindPlanTripProspectCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select TOP 1 TP.REMIND ");
                queryBuilder.AppendFormat(" from PLAN_TRIP T ");
                queryBuilder.AppendFormat(" inner join PLAN_TRIP_PROSPECT TP on TP.PLAN_TRIP_ID = T.PLAN_TRIP_ID ");
                queryBuilder.AppendFormat(" where T.STOP_CHECKIN_LOC_ID is not null  ");
                queryBuilder.AppendFormat(" and TP.PROSP_ID = @PROSP_ID ");
                queryBuilder.AppendFormat(" order by TP.VISIT_CHECKIN_DTM DESC ");
                QueryUtils.addParam(command, "PROSP_ID", o.ProspId);// Add new

                // For Paging
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
                log.Debug("Query Data:" + queryBuilder.ToString());
                Console.WriteLine("Query Data:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();// Add new
                using (var reader = command.ExecuteReader())
                {
                    //

                    List<GetLastRemindPlanTripProspectCustom> dataRecordList = new List<GetLastRemindPlanTripProspectCustom>();
                    GetLastRemindPlanTripProspectCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new GetLastRemindPlanTripProspectCustom();
                        dataRecord.Remind = QueryUtils.getValueAsString(record, "REMIND");
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




        public async Task<List<GetEmailToSendForCreatePlantripCustom>> getEmailToSendForCreatePlantrip(string empId)
        {

            List<GetEmailToSendForCreatePlantripCustom> email = new List<GetEmailToSendForCreatePlantripCustom>();
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select  CFG.PARAM_VALUE SENDER_EMAIL,AE.EMAIL RECEIVE_EMAIL,E.EMAIL RECEIVE_EMAIL_ASSIGN ");
                queryBuilder.AppendFormat(" from ADM_EMPLOYEE E ");
                queryBuilder.AppendFormat(" inner join ADM_EMPLOYEE AE on AE.EMP_ID = E.APPROVE_EMP_ID ");
                queryBuilder.AppendFormat(" inner join MS_CONFIG_PARAM CFG on CFG.PARAM_KEYWORD = 'EMAIL_SENDER' ");
                queryBuilder.AppendFormat(" where E.EMP_ID = @EMP_ID ");
                QueryUtils.addParam(command, "EMP_ID", empId);// Add new


                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        GetEmailToSendForCreatePlantripCustom o = new GetEmailToSendForCreatePlantripCustom();
                        o.SenderEmail =  QueryUtils.getValueAsString(record, "SENDER_EMAIL");
                        o.ReceiveEmail = QueryUtils.getValueAsString(record, "RECEIVE_EMAIL");
                        o.ReceiveEmailAssign = QueryUtils.getValueAsString(record, "RECEIVE_EMAIL_ASSIGN");
                        email.Add(o);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return email;
        }



        public async Task<List<GetEmailToSendForUpdatePlantripCustom>> getEmailToSendForUpdatePlantrip(string empId)
        {

            List<GetEmailToSendForUpdatePlantripCustom> email = new List<GetEmailToSendForUpdatePlantripCustom>();
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select  CFG.PARAM_VALUE SENDER_EMAIL,AE.EMAIL RECEIVE_EMAIL ");
                queryBuilder.AppendFormat(" from ADM_EMPLOYEE E ");
                queryBuilder.AppendFormat(" inner join ADM_EMPLOYEE AE on AE.EMP_ID = E.APPROVE_EMP_ID ");
                queryBuilder.AppendFormat(" inner join MS_CONFIG_PARAM CFG on CFG.PARAM_KEYWORD = 'EMAIL_SENDER' ");
                queryBuilder.AppendFormat(" where E.EMP_ID = @EMP_ID ");
                QueryUtils.addParam(command, "EMP_ID", empId);// Add new


                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        GetEmailToSendForUpdatePlantripCustom o = new GetEmailToSendForUpdatePlantripCustom();
                        o.SenderEmail = QueryUtils.getValueAsString(record, "SENDER_EMAIL");
                        o.ReceiveEmail = QueryUtils.getValueAsString(record, "RECEIVE_EMAIL");
                        //o.ReceiveEmailAssign = QueryUtils.getValueAsString(record, "RECEIVE_EMAIL_ASSIGN");
                        email.Add(o);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return email;
        }



        public async Task<List<GetEmailToSendForRejectPlantripCustom>> getEmailToSendForRejectPlanTrip(string planTripId)
        {

            List<GetEmailToSendForRejectPlantripCustom> email = new List<GetEmailToSendForRejectPlantripCustom>();
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select CFG.PARAM_VALUE SENDER_EMAIL,E.EMAIL RECEIVE_EMAIL,T.PLAN_TRIP_NAME,T.PLAN_TRIP_DATE ");
                queryBuilder.AppendFormat(" from PLAN_TRIP T ");
                queryBuilder.AppendFormat(" inner join ADM_EMPLOYEE E on EMP_ID = T.CREATE_USER ");
                queryBuilder.AppendFormat(" inner join MS_CONFIG_PARAM CFG on CFG.PARAM_KEYWORD = 'EMAIL_SENDER' ");
                queryBuilder.AppendFormat(" where PLAN_TRIP_ID = @PLAN_TRIP_ID ");
                QueryUtils.addParam(command, "PLAN_TRIP_ID", planTripId);// Add new


                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        GetEmailToSendForRejectPlantripCustom o = new GetEmailToSendForRejectPlantripCustom();
                        o.SenderEmail = QueryUtils.getValueAsString(record, "SENDER_EMAIL");
                        o.ReceiveEmail = QueryUtils.getValueAsString(record, "RECEIVE_EMAIL");
                        o.PlanTripName = QueryUtils.getValueAsString(record, "PLAN_TRIP_NAME");
                        o.PlanTripDate = QueryUtils.getValueAsDateTime(record, "PLAN_TRIP_DATE");
                        email.Add(o);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return email;
        }



        public async Task<List<GetEmailToSendForApprovePlanTripCustom>> getEmailToSendForapprovePlanTrip(string planTripId)
        {

            List<GetEmailToSendForApprovePlanTripCustom> email = new List<GetEmailToSendForApprovePlanTripCustom>();
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();
                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select CFG.PARAM_VALUE SENDER_EMAIL,E.EMAIL RECEIVE_EMAIL,T.PLAN_TRIP_NAME,T.PLAN_TRIP_DATE ");
                queryBuilder.AppendFormat(" from PLAN_TRIP T ");
                queryBuilder.AppendFormat(" inner join ADM_EMPLOYEE E on EMP_ID = T.CREATE_USER ");
                queryBuilder.AppendFormat(" inner join MS_CONFIG_PARAM CFG on CFG.PARAM_KEYWORD = 'EMAIL_SENDER' ");
                queryBuilder.AppendFormat(" where PLAN_TRIP_ID = @PLAN_TRIP_ID ");
                QueryUtils.addParam(command, "PLAN_TRIP_ID", planTripId);// Add new

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        GetEmailToSendForApprovePlanTripCustom o = new GetEmailToSendForApprovePlanTripCustom();
                        o.SenderEmail = QueryUtils.getValueAsString(record, "SENDER_EMAIL");
                        o.ReceiveEmail = QueryUtils.getValueAsString(record, "RECEIVE_EMAIL");
                        o.PlanTripName = QueryUtils.getValueAsString(record, "PLAN_TRIP_NAME");
                        o.PlanTripDate = QueryUtils.getValueAsDateTime(record, "PLAN_TRIP_DATE");
                        email.Add(o);
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return email;
        }


    }
}
