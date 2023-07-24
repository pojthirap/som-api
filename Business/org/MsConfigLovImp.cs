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

namespace MyFirstAzureWebApp.Business.org
{

    public class MsConfigLovImp : IMsConfigLov
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<MsConfigLov>> Search(SearchCriteriaBase<MsConfigLovCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from MS_CONFIG_LOV where ACTIVE_FLAG = 'Y' ");
                MsConfigLovCriteria o = searchCriteria.model;
                if (o != null)
                {

                    if (!String.IsNullOrEmpty(o.LovKeyword))
                    {
                        queryBuilder.AppendFormat(" and LOV_KEYWORD = @LovKeyword ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("LovKeyword", o.LovKeyword));// Add New
                    }
                }
                // For Paging
                queryBuilder.AppendFormat(" ORDER BY LOV_ORDER   ");
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.MsConfigLov.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<MsConfigLov> lst = context.MsConfigLov.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<MsConfigLov> searchResult = new EntitySearchResultBase<MsConfigLov>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }

        public async Task<EntitySearchResultBase<GetMasterDataForTemplateSaCustom>> getMasterDataForTemplateSa(SearchCriteriaBase<GetMasterDataForTemplateSaCriteria> searchCriteria)
        {

            EntitySearchResultBase<GetMasterDataForTemplateSaCustom> searchResult = new EntitySearchResultBase<GetMasterDataForTemplateSaCustom>();
            List<GetMasterDataForTemplateSaCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                GetMasterDataForTemplateSaCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                switch (o.AnsLovType)
                {
                    case "1":
                        queryBuilder.AppendFormat(" select REGION_CODE CODE,REGION_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from MS_REGION WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "2":
                        queryBuilder.AppendFormat(" select PROVINCE_CODE CODE,PROVINCE_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from MS_PROVINCE WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "3":
                        queryBuilder.AppendFormat(" select DISTRICT_CODE CODE,DISTRICT_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from MS_DISTRICT WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "4":
                        queryBuilder.AppendFormat(" select SUBDISTRICT_CODE CODE,SUBDISTRICT_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from MS_SUBDISTRICT WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "5":
                        queryBuilder.AppendFormat(" select COMPANY_CODE CODE,COMPANY_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from ORG_COMPANY WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "6":
                        queryBuilder.AppendFormat(" select ORG_CODE CODE,ORG_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from ORG_SALE_ORGANIZATION WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "7":
                        queryBuilder.AppendFormat(" select CHANNEL_CODE CODE,CHANNEL_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from ORG_DIST_CHANNEL WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "8":
                        queryBuilder.AppendFormat(" select DIVISION_CODE CODE,DIVISION_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from ORG_DIVISION WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "9":
                        queryBuilder.AppendFormat(" select OFFICE_CODE CODE,DESCRIPTION_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from ORG_SALE_OFFICE WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "10":
                        queryBuilder.AppendFormat(" select GROUP_CODE CODE,DESCRIPTION_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from ORG_SALE_GROUP WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "11":
                        queryBuilder.AppendFormat(" select E.EMP_ID CODE,E.FIRST_NAME+' '+E.LAST_NAME DESCRIPTION ");
                        queryBuilder.AppendFormat(" from ADM_EMPLOYEE E ");
                        queryBuilder.AppendFormat(" inner join ADM_GROUP_USER GU on GU.EMP_ID = E.EMP_ID ");
                        queryBuilder.AppendFormat(" WHERE GU.ACTIVE_FLAG = 'Y' ");

                        //queryBuilder.AppendFormat(" select E.EMP_ID CODE,E.FIRST_NAME+' '+E.LAST_NAME DESCRIPTION ");
                        //queryBuilder.AppendFormat(" from ADM_EMPLOYEE E inner join ADM_GROUP_USER GU on GU.EMP_ID = E.EMP_ID  ");
                        break;
                    case "12":
                        queryBuilder.AppendFormat(" select BU_ID CODE,BU_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from ORG_BUSINESS_UNIT WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "13":
                        queryBuilder.AppendFormat(" XXXXX ");
                        queryBuilder.AppendFormat(" XXXXX ");
                        break;
                    case "14":
                        queryBuilder.AppendFormat(" XXXXX ");
                        queryBuilder.AppendFormat(" XXXXX ");
                        break;
                    case "15":
                        queryBuilder.AppendFormat(" select TERRITORY_ID CODE,TERRITORY_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from ORG_TERRITORY WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "16":
                        queryBuilder.AppendFormat(" select BRAND_ID CODE,BRAND_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from MS_BRAND WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "17":
                        queryBuilder.AppendFormat(" select LOC_ID CODE,LOC_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from MS_LOCATION WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "18":
                        queryBuilder.AppendFormat(" select LOC_TYPE_ID CODE,LOC_TYPE_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from MS_LOCATION_TYPE WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "19":
                        queryBuilder.AppendFormat(" select BRAND_CATE_ID CODE,BRAND_CATE_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from MS_BRAND_CATEGORY WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "20":
                        queryBuilder.AppendFormat(" select BANK_ID CODE,BANK_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from MS_BANK WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                    case "21":
                        queryBuilder.AppendFormat(" select PROD_CODE CODE,PROD_NAME_TH DESCRIPTION ");
                        queryBuilder.AppendFormat(" from MS_PRODUCT WHERE ACTIVE_FLAG = 'Y' ");
                        break;
                }

        
                // For Paging
                queryBuilder.AppendFormat(" ORDER BY DESCRIPTION  ");
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

                    List<GetMasterDataForTemplateSaCustom> dataRecordList = new List<GetMasterDataForTemplateSaCustom>();
                    GetMasterDataForTemplateSaCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new GetMasterDataForTemplateSaCustom();


                        dataRecord.Code = QueryUtils.getValueAsString(record, "CODE");
                        dataRecord.Description = QueryUtils.getValueAsString(record, "DESCRIPTION");

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


























