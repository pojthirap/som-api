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
using MyFirstAzureWebApp.Utils;
using System.Data;

namespace MyFirstAzureWebApp.Business.org
{

    public class CustomSpacialImp : ICustomSpacial
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<SaleOrgCustom>> SearchSaleOrg(SearchCriteriaBase<SaleOrgCriteria> searchCriteria)
        {

            EntitySearchResultBase<SaleOrgCustom> searchResult = new EntitySearchResultBase<SaleOrgCustom>();
            List<SaleOrgCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                SaleOrgCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select C.COMPANY_NAME_TH,O.* ");
                queryBuilder.AppendFormat(" from dbo.ORG_COMPANY C  ");
                queryBuilder.AppendFormat(" INNER JOIN dbo.ORG_SALE_ORGANIZATION O ON C.COMPANY_CODE = O.COMPANY_CODE ");
                queryBuilder.AppendFormat(" where 1=1 ");
                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.companyName))
                    {
                        queryBuilder.AppendFormat(" and C.COMPANY_NAME_TH like @CompanyName  ");
                        QueryUtils.addParamLike(command, "CompanyName", o.companyName);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.orgName))
                    {
                        queryBuilder.AppendFormat(" and O.ORG_NAME_TH like @OrgName  ");
                        QueryUtils.addParamLike(command, "OrgName", o.orgName);// Add new
                    }
                }

                // For Paging
                if(searchCriteria.searchOrder == 1) {
                    queryBuilder.AppendFormat(" ORDER BY COMPANY_NAME_TH  ");
                }else if (searchCriteria.searchOrder == 2)
                {
                    queryBuilder.AppendFormat(" ORDER BY ORG_NAME_TH  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY COMPANY_CODE  ");
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

                    List<SaleOrgCustom> dataRecordList = new List<SaleOrgCustom>();
                    SaleOrgCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new SaleOrgCustom();


                        dataRecord.companyCode = QueryUtils.getValueAsString(record, "COMPANY_CODE");
                        dataRecord.companyNameTh = QueryUtils.getValueAsString(record, "COMPANY_NAME_TH");
                        dataRecord.orgCode = QueryUtils.getValueAsString(record, "ORG_CODE");
                        dataRecord.orgNameTh = QueryUtils.getValueAsString(record, "ORG_NAME_TH");
                        dataRecord.orgNameEn = QueryUtils.getValueAsString(record, "ORG_NAME_EN");
                        dataRecord.currency = QueryUtils.getValueAsString(record, "CURRENCY");
                        dataRecord.activeFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
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


        /*public async Task<EntitySearchResultBase<SaleOrgCustom>> SearchSaleOrg(SaleOrgSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                SaleOrgCriteria criteria = searchCriteria.model;
                var queryCommane = (from c in context.OrgCompany
                                 join o in context.OrgSaleOrganization
                                 on c.CompanyCode equals o.CompanyCode
                             
                             //where ( (criteria.companyName == null ? 1==1 : c.CompanyNameTh == criteria.companyName)  || (criteria.companyName == null ? 1 == 1 : c.CompanyNameEn == criteria.companyName))
                             //where ((criteria.orgName == null ? 1 == 1 : o.OrgNameTh == criteria.orgName) || (criteria.orgName == null ? 1 == 1 : o.OrgNameEn == criteria.orgName))

                             where ((criteria.companyName == null ? 1 == 1 : c.CompanyNameTh.Contains(criteria.companyName)))
                             where ((criteria.orgName == null ? 1 == 1 : o.OrgNameTh.Contains(criteria.orgName)))
                                    orderby (searchCriteria.searchOrder == 0 ? c.CompanyCode : c.CompanyNameTh)
                                    select new
                             {
                                 CompanyNameTh = c.CompanyNameTh,
                                 CompanyNameEn = c.CompanyNameEn,
                                 CompanyCode = o.CompanyCode,
                                 OrgCode = o.OrgCode,
                                 OrgNameTh = o.OrgNameTh,
                                 OrgNameEn = o.OrgNameEn,
                                 Currency = o.Currency,
                                 ActiveFlag = o.ActiveFlag,
                                 CreateUser = o.CreateUser,
                                 CreateDtm = o.CreateDtm,
                                 UpdateUser = o.UpdateUser,
                                 UpdateDtm = o.UpdateDtm
                             });
                //}).ToList();
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<SaleOrgCustom> searchResult = new EntitySearchResultBase<SaleOrgCustom>();
                searchResult.totalRecords = query.Count();

                List<SaleOrgCustom> saleLst = new List<SaleOrgCustom>();
                //foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().OrderBy(x => x.CompanyCode).Skip(searchCriteria.startRecord).Take(searchCriteria.length) : query.ToList().OrderBy(x => x.CompanyCode)))
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                    {
                    SaleOrgCustom s = new SaleOrgCustom();
                    s.companyNameTh = item.CompanyNameTh;
                    s.companyNameEN = item.CompanyNameEn;
                    s.companyCode = item.CompanyCode;
                    s.orgCode = item.OrgCode;
                    s.orgNameTh = item.OrgNameTh;
                    s.orgNameEn = item.OrgNameEn;
                    s.currency = item.Currency;
                    s.activeFlag = item.ActiveFlag;
                    s.createUser = item.CreateUser;
                    s.createDtm = item.CreateDtm;
                    s.updateUser = item.UpdateUser;
                    s.updateDtm = item.UpdateDtm;
                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;
               
            }

        }
        */



        public async Task<EntitySearchResultBase<QRCustom>> searchGasolineByCust(QRSearchCriteria searchCriteria)
        {

            EntitySearchResultBase<QRCustom> searchResult = new EntitySearchResultBase<QRCustom>();
            List<QRCustom> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                // Move New
                QRCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select C.CUST_CODE,C.CUST_NAME_TH,C.CUST_NAME_EN,M.CREATE_DTM AS CREATE_DTM_METER,M.UPDATE_DTM AS UPDATE_DTM_METER,M.CREATE_USER AS CREATE_USER_METER,M.UPDATE_USER AS UPDATE_USER_METER,M.ACTIVE_FLAG as ACTIVE_FLAG_METER,M.* ");
                queryBuilder.AppendFormat(" ,G.ACTIVE_FLAG as ACTIVE_FLAG_GAS,G.CREATE_USER AS CREATE_USER_GAS ,G.CREATE_DTM AS CREATE_DTM_GAS,G.UPDATE_USER AS UPDATE_USER_GAS, G.UPDATE_DTM AS UPDATE_DTM_GAS, G.* ");
                queryBuilder.AppendFormat(" from dbo.CUSTOMER C  ");
                queryBuilder.AppendFormat(" inner join dbo.MS_METER M on C.CUST_CODE = M.CUST_CODE ");
                queryBuilder.AppendFormat(" inner join dbo.MS_GASOLINE G on G.GAS_ID = M.GAS_ID  ");
                queryBuilder.AppendFormat(" where 1=1  ");

                if (o != null)
                {
                    if (!String.IsNullOrEmpty(o.custCode))
                    {
                        queryBuilder.AppendFormat(" and C.CUST_CODE = @CUST_CODE  ");
                        QueryUtils.addParam(command, "CUST_CODE", o.custCode);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.custName))
                    {
                        queryBuilder.AppendFormat(" and C.CUST_NAME_TH = @CUST_NAME_TH  ");
                        QueryUtils.addParam(command, "CUST_NAME_TH", o.custName);// Add new
                    }
                    if (!String.IsNullOrEmpty(o.ActiveFlag))
                    {
                        queryBuilder.AppendFormat(" and M.ACTIVE_FLAG  = @ActiveFlag  ");
                        QueryUtils.addParam(command, "ActiveFlag", o.ActiveFlag);// Add new
                    }
                }



                // For Paging
                queryBuilder.AppendFormat(" order by DISPENSER_NO, NOZZLE_NO ");
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

                    List<QRCustom> dataRecordList = new List<QRCustom>();
                    QRCustom dataRecord = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        dataRecord = new QRCustom();


                        dataRecord.custCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        dataRecord.custNameTh = QueryUtils.getValueAsString(record, "CUST_NAME_TH");
                        dataRecord.custNameEn = QueryUtils.getValueAsString(record, "CUST_NAME_EN");
                        dataRecord.meterId = QueryUtils.getValueAsDecimalRequired(record, "METER_ID");
                        dataRecord.gasId = QueryUtils.getValueAsDecimalRequired(record, "GAS_ID");
                        //dataRecord.custCode = QueryUtils.getValueAsString(record, "CUST_CODE");
                        dataRecord.dispenserNo = QueryUtils.getValueAsDecimal(record, "DISPENSER_NO");
                        dataRecord.nozzleNo = QueryUtils.getValueAsDecimal(record, "NOZZLE_NO");
                        dataRecord.qrcode = QueryUtils.getValueAsString(record, "QRCODE");
                        dataRecord.activeFlagGas = QueryUtils.getValueAsString(record, "ACTIVE_FLAG_GAS");
                        //dataRecord.gasId = QueryUtils.getValueAsDecimalRequired(record, "GAS_ID");
                        dataRecord.GasNameTh = QueryUtils.getValueAsString(record, "GAS_NAME_TH");
                        dataRecord.GasNameEn = QueryUtils.getValueAsString(record, "GAS_NAME_EN");
                        dataRecord.activeFlagMeter = QueryUtils.getValueAsString(record, "ACTIVE_FLAG_METER");
                        dataRecord.GasCode = QueryUtils.getValueAsString(record, "GAS_CODE");

                        /*dataRecord.createUser = QueryUtils.getValueAsString(record, "CREATE_USER_GAS");
                        dataRecord.createDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM_GAS");
                        dataRecord.updateUser = QueryUtils.getValueAsString(record, "UPDATE_USER_GAS");
                        dataRecord.updateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM_GAS");
                        */
                        dataRecord.createUser = QueryUtils.getValueAsString(record, "CREATE_USER_METER");
                        dataRecord.createDtm = QueryUtils.getValueAsDateTimeRequired(record, "CREATE_DTM_METER");
                        dataRecord.updateUser = QueryUtils.getValueAsString(record, "UPDATE_USER_METER");
                        dataRecord.updateDtm = QueryUtils.getValueAsDateTimeRequired(record, "UPDATE_DTM_METER");

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



        /*public async Task<EntitySearchResultBase<QRCustom>> searchGasolineByCust(QRSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                QRCriteria criteria = searchCriteria.model;
                var queryCommane = (from c in context.Customer 
                             join m in context.MsMeter
                             on c.CustCode equals m.CustCode
                             join g in context.MsGasoline
                             on m.GasId equals g.GasId

                             where ((criteria.custCode == null ? 1 == 1 : c.CustCode == criteria.custCode))
                             where ((criteria.ActiveFlag == null ? 1 == 1 : m.ActiveFlag == criteria.ActiveFlag))
                                    //where ((criteria.custName == null ? 1 == 1 : c.CustNameTh == criteria.custName) || (criteria.custName == null ? 1 == 1 : c.CustNameEn == criteria.custName))
                                    where ((criteria.custName == null ? 1 == 1 : c.CustNameTh == criteria.custName))
                                    //orderby (searchCriteria.searchOrder == 1 ? m.UpdateDtm : m.UpdateDtm) descending
                                    orderby (m.DispenserNo)
                                    orderby (m.NozzleNo)
                                    select new
                             {
                                 CustCode = c.CustCode,
                                 CustNameTh = c.CustNameTh,
                                 CustNameEn = c.CustNameEn,
                                 MeterId = m.MeterId,
                                 GasId = m.GasId,
                                 DispenserNo = m.DispenserNo,
                                 NozzleNo = m.NozzleNo,
                                 Qrcode = m.Qrcode,
                                 ActiveFlag = m.ActiveFlag,
                                 CreateUser = m.CreateUser,
                                 CreateDtm = m.CreateDtm,
                                 UpdateUser = m.UpdateUser,
                                 UpdateDtm = m.UpdateDtm,

                                        GasNameTh = g.GasNameTh,
                                        GasNameEn = g.GasNameEn,
                                        GasCode = g.GasCode

                                    });
                //}).ToList();
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<QRCustom> searchResult = new EntitySearchResultBase<QRCustom>();
                searchResult.totalRecords = query.Count();



                List<QRCustom> saleLst = new List<QRCustom>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    QRCustom s = new QRCustom();
                    s.custCode = item.CustCode;
                    s.custNameTh = item.CustNameTh;
                    s.custNameEn = item.CustNameEn;
                    s.meterId = item.MeterId;
                    s.gasId = item.GasId;
                    s.dispenserNo = item.DispenserNo;
                    s.nozzleNo = item.NozzleNo;
                    s.qrcode = item.Qrcode;
                    s.activeFlag = item.ActiveFlag;
                    s.createUser = item.CreateUser;
                    s.createDtm = item.CreateDtm;
                    s.updateUser = item.UpdateUser;
                    s.updateDtm = item.UpdateDtm;
                    s.GasNameTh = item.GasNameTh;
                    s.GasNameEn = item.GasNameEn;
                    s.GasCode = item.GasCode;
                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;
            }

        }
        */



    }
}
