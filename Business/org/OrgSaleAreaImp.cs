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
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Utils;
using System.Data;

namespace MyFirstAzureWebApp.Business.org
{

    public class OrgSaleAreaImp : IOrgSaleArea
    {
        private Logger log = LogManager.GetCurrentClassLogger();



        public async Task<EntitySearchResultBase<OrgSaleAreaCustom>> Search(SearchCriteriaBase<OrgSaleAreaCriteria> searchCriteria)
        {

            EntitySearchResultBase<OrgSaleAreaCustom> searchResult = new EntitySearchResultBase<OrgSaleAreaCustom>();
            List<OrgSaleAreaCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                OrgSaleAreaCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select * ");
                queryBuilder.AppendFormat(" from dbo.ORG_SALE_AREA SA  ");
                queryBuilder.AppendFormat(" inner join dbo.ORG_SALE_ORGANIZATION SO ON SO.ORG_CODE = SA.ORG_CODE ");
                queryBuilder.AppendFormat(" inner join dbo.ORG_DIST_CHANNEL DC ON DC.CHANNEL_CODE = SA.CHANNEL_CODE ");
                queryBuilder.AppendFormat(" inner join dbo.ORG_DIVISION DV ON DV.DIVISION_CODE = SA.DIVISION_CODE ");
                queryBuilder.AppendFormat(" inner join dbo.ORG_BUSINESS_AREA BA ON BA.BUSS_AREA_CODE = SA.BUSS_AREA_CODE ");
                queryBuilder.AppendFormat(" left  join dbo.ORG_BUSINESS_UNIT BU ON BU.BU_ID = SA.BU_ID ");
                queryBuilder.AppendFormat(" where 1=1 ");
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.orgNameTh))
                    {
                        queryBuilder.AppendFormat(" and SO.ORG_NAME_TH like @orgNameTh  ");
                        QueryUtils.addParamLike(command, "orgNameTh", o.orgNameTh);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.channelNameTh))
                    {
                        queryBuilder.AppendFormat(" and DC.CHANNEL_NAME_TH like @channelNameTh  ");
                        QueryUtils.addParamLike(command, "channelNameTh", o.channelNameTh);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.divisionNameTh))
                    {
                        queryBuilder.AppendFormat(" and DV.DIVISION_NAME_TH like @divisionNameTh  ");
                        QueryUtils.addParamLike(command, "divisionNameTh", o.divisionNameTh);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.buNameTh))
                    {
                        queryBuilder.AppendFormat("  and BU.BU_NAME_TH like @buNameTh  ");
                        QueryUtils.addParamLike(command, "buNameTh", o.buNameTh);// Add new
                    }

                }



                // For Paging
                if (searchCriteria.searchOrder == 1)
                {
                    queryBuilder.AppendFormat(" ORDER BY SO.ORG_CODE  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY SO.ORG_CODE  ");
                }
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

                    List<OrgSaleAreaCustom> dataRecordList = new List<OrgSaleAreaCustom>();
                    OrgSaleAreaCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new OrgSaleAreaCustom();


                        dataRecord.AreaId = QueryUtils.getValueAsDecimal(record, "AREA_ID");
                        dataRecord.OrgCode = QueryUtils.getValueAsString(record, "ORG_CODE");
                        //dataRecord.ChannelCode = QueryUtils.getValueAsString(record, "CHANNEL_CODE");
                        //dataRecord.DivisionCode = QueryUtils.getValueAsString(record, "DIVISION_CODE");
                        //dataRecord.BussAreaCode = QueryUtils.getValueAsString(record, "BUSS_AREA_CODE");
                        dataRecord.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");

                        dataRecord.CompanyCode = QueryUtils.getValueAsString(record, "COMPANY_CODE");
                        dataRecord.OrgNameTh = QueryUtils.getValueAsString(record, "ORG_NAME_TH");
                        dataRecord.OrgNameEn = QueryUtils.getValueAsString(record, "ORG_NAME_EN");
                        dataRecord.Currency = QueryUtils.getValueAsString(record, "CURRENCY");

                        dataRecord.ChannelCode = QueryUtils.getValueAsString(record, "CHANNEL_CODE");
                        dataRecord.ChannelNameTh = QueryUtils.getValueAsString(record, "CHANNEL_NAME_TH");
                        dataRecord.ChannelNameEn = QueryUtils.getValueAsString(record, "CHANNEL_NAME_EN");

                        dataRecord.DivisionCode = QueryUtils.getValueAsString(record, "DIVISION_CODE");
                        dataRecord.DivisionNameTh = QueryUtils.getValueAsString(record, "DIVISION_NAME_TH");
                        dataRecord.DivisionNameEn = QueryUtils.getValueAsString(record, "DIVISION_NAME_EN");

                        dataRecord.BussAreaCode = QueryUtils.getValueAsString(record, "BUSS_AREA_CODE");
                        dataRecord.BussAreaNameTh = QueryUtils.getValueAsString(record, "BUSS_AREA_NAME_TH");
                        dataRecord.BussAreaNameEn = QueryUtils.getValueAsString(record, "BUSS_AREA_NAME_EN");

                        dataRecord.BuNameTh = QueryUtils.getValueAsString(record, "BU_NAME_TH");


                        dataRecord.createUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        dataRecord.createDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        dataRecord.updateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        dataRecord.updateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");

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
        /*public async Task<EntitySearchResultBase<OrgSaleAreaCustom>> Search(OrgSaleAreaSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                OrgSaleAreaCriteria criteria = searchCriteria.model;
                var queryCommane = (from sa in context.OrgSaleArea
                                 join so in context.OrgSaleOrganization
                                 on sa.OrgCode equals so.OrgCode
                                 join dc in context.OrgDistChannel
                                 on sa.ChannelCode equals dc.ChannelCode
                                 join dv in context.OrgDivision
                                 on sa.DivisionCode equals dv.DivisionCode
                                 join ba in context.OrgBusinessArea
                                 on sa.BussAreaCode equals ba.BussAreaCode
                             

                             where ((criteria.orgNameTh == null ? 1 == 1 : so.OrgNameTh.Contains(criteria.orgNameTh)))
                             where ((criteria.channelNameTh == null ? 1 == 1 : dc.ChannelNameTh.Contains(criteria.channelNameTh)))
                             where ((criteria.divisionNameTh == null ? 1 == 1 : dv.DivisionNameTh.Contains(criteria.divisionNameTh)))
                                    orderby (searchCriteria.searchOrder == 0 ? so.OrgCode: so.OrgNameTh)
                                    select new
                             {
                                        AreaId = sa.AreaId,
                                        OrgCode = sa.OrgCode,
                                        ChannelCode = sa.ChannelCode,
                                        DivisionCode = sa.DivisionCode,
                                        BussAreaCode = sa.BussAreaCode,
                                        //BuId = sa.BuId,
                                        ActiveFlag = sa.ActiveFlag,
                                        CreateUser = sa.CreateUser,
                                        CreateDtm = sa.CreateDtm,
                                        UpdateUser = sa.UpdateUser,
                                        UpdateDtm = sa.UpdateDtm,

                                        CompanyCode = so.CompanyCode,
                                        OrgNameTh = so.OrgNameTh,
                                        OrgNameEn = so.OrgNameEn,
                                        Currency = so.Currency,

                                        ChannelNameTh = dc.ChannelNameTh,
                                        ChannelNameEn = dc.ChannelNameEn,

                                        DivisionNameTh = dv.DivisionNameTh,
                                        DivisionNameEn = dv.DivisionNameEn,

                                        BussAreaNameTh = ba.BussAreaNameTh,
                                        BussAreaNameEn = ba.BussAreaNameEn
                             });
                //}).ToList();
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<OrgSaleAreaCustom> searchResult = new EntitySearchResultBase<OrgSaleAreaCustom>();
                searchResult.totalRecords = query.Count();

                List<OrgSaleAreaCustom> saleLst = new List<OrgSaleAreaCustom>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                    {
                    OrgSaleAreaCustom s = new OrgSaleAreaCustom();
                    s.AreaId = item.AreaId;
                    s.OrgCode = item.OrgCode;
                    s.ChannelCode = item.ChannelCode;
                    s.DivisionCode = item.DivisionCode;
                    s.BussAreaCode = item.BussAreaCode;
                    //s.BuId = item.BuId;
                    s.ActiveFlag = item.ActiveFlag;
                    s.createUser = item.CreateUser;
                    s.createDtm = item.CreateDtm;
                    s.updateUser = item.UpdateUser;
                    s.updateDtm = item.UpdateDtm;
                    s.CompanyCode = item.CompanyCode;
                    s.OrgNameTh = item.OrgNameTh;
                    s.OrgNameEn = item.OrgNameEn;
                    s.Currency = item.Currency;
                    s.ChannelNameTh = item.ChannelNameTh;
                    s.ChannelNameEn = item.ChannelNameEn;
                    s.DivisionNameTh = item.DivisionNameTh;
                    s.DivisionNameEn = item.DivisionNameEn;
                    s.BussAreaNameTh = item.BussAreaNameTh;
                    s.BussAreaNameEn = item.BussAreaNameEn;
                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;
               
            }

        }*/

        public async Task<int> MapBU(OrgSaleAreaModel model)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ORG_SALE_AREA SET BU_ID = @BU_ID, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE area_id=@area_id ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("BU_ID", model.BuId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", model.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("area_id", model.AreaId));// Add New
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




    }
}
