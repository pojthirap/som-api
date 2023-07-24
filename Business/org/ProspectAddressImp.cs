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
using MyFirstAzureWebApp.Entity.custom;
using System.Data.Common;
using System.Data;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.Models.pospect;

namespace MyFirstAzureWebApp.Business.org
{

    public class ProspectAddressImp : IProspectAddress
    {
        private Logger log = LogManager.GetCurrentClassLogger();


        public async Task<EntitySearchResultBase<ProspectAddress>> searchProspectAddress(SearchCriteriaBase<SearchProspectAddressCriteria> searchCriteria)
        {

            
            EntitySearchResultBase<ProspectAddress> searchResult = new EntitySearchResultBase<ProspectAddress>();
            List<ProspectAddress> lst;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                SearchProspectAddressCriteria o = searchCriteria.model;
                StringBuilder queryBuilder = new StringBuilder();
                if (searchCriteria.searchOption == 0)
                {
                    queryBuilder.AppendFormat(" select NULL MAIN_ADDRESS_FLAG,NULL SHIFT_TO_FLAG,NULL BILL_TO_FLAG, ");
                    queryBuilder.AppendFormat("   IIF(PD.PROSPECT_ID = @PROSPECT_ID and PA.CUST_CODE is null,'Y','N') EDIT_GENERAL_DATA_FLAG, ");
                    queryBuilder.AppendFormat("   TRIM(IIF(PD.ADDR_NO is null,'',PD.ADDR_NO+' ') + ");
                    queryBuilder.AppendFormat("   IIF(PD.MOO is null,'',PD.MOO+' ') + ");
                    queryBuilder.AppendFormat("   IIF(PD.SOI is null,'',PD.SOI+' ') + ");
                    queryBuilder.AppendFormat("   IIF(PD.STREET is null,'',PD.STREET +' ') + ");
                    queryBuilder.AppendFormat("   IIF(PD.SUBDISTRICT_CODE is null,'',S.SUBDISTRICT_NAME_TH+' ') + ");
                    queryBuilder.AppendFormat("   IIF(PD.DISTRICT_CODE is null,'',D.DISTRICT_NAME_TH+' ') + ");
                    queryBuilder.AppendFormat("   IIF(PD.PROVINCE_CODE is null,'',P.PROVINCE_NAME_TH+' /TH'))  ");
                    queryBuilder.AppendFormat("   ADDRESS_FULLNM,  ");
                    queryBuilder.AppendFormat("   PD.* ");
                    queryBuilder.AppendFormat("   from PROSPECT_ADDRESS PD  ");
                    queryBuilder.AppendFormat("   inner join PROSPECT_ACCOUNT PA on PA.PROSP_ACC_ID = PD.PROSP_ACC_ID  ");
                    queryBuilder.AppendFormat("   left join MS_REGION R on R.REGION_CODE = PD.REGION_CODE  ");
                    queryBuilder.AppendFormat("   left join MS_PROVINCE P on P.PROVINCE_CODE = PD.PROVINCE_CODE  ");
                    queryBuilder.AppendFormat("   left join MS_DISTRICT D on D.DISTRICT_CODE = PD.DISTRICT_CODE  ");
                    queryBuilder.AppendFormat("   left join MS_SUBDISTRICT S on S.SUBDISTRICT_CODE = PD.SUBDISTRICT_CODE  ");
                    queryBuilder.AppendFormat("   where PD.PROSPECT_ID = @PROSPECT_ID  ");
                    QueryUtils.addParam(command, "PROSPECT_ID", o.ProspectId);// Add new
                    if (!String.IsNullOrEmpty(o.ProsAccId))
                    {
                        queryBuilder.AppendFormat("   OR (PD.PROSP_ACC_ID = @PROSP_ACC_ID and  PD.MAIN_FLAG = 'Y')  ", o.ProsAccId);
                        QueryUtils.addParam(command, "PROSP_ACC_ID", o.ProsAccId);// Add new
                    }

                    // For Paging
                    queryBuilder.AppendFormat(" ORDER BY PROSP_ADDR_ID  ");
                }
                else if(searchCriteria.searchOption == 1)
                { 
                    queryBuilder.AppendFormat(" select TT.MAIN_ADDRESS_FLAG,TT.SHIFT_TO_FLAG,TT.BILL_TO_FLAG,  ");
                    queryBuilder.AppendFormat(" 'N' EDIT_GENERAL_DATA_FLAG,  ");
                    queryBuilder.AppendFormat(" TRIM(IIF(PD.ADDR_NO is null,'',PD.ADDR_NO+' ') + ");
                    queryBuilder.AppendFormat(" IIF(PD.MOO is null,'',PD.MOO+' ') + ");
                    queryBuilder.AppendFormat(" IIF(PD.SOI is null,'',PD.SOI+' ') + ");
                    queryBuilder.AppendFormat(" IIF(PD.STREET is null,'',PD.STREET +' ') + ");
                    queryBuilder.AppendFormat(" IIF(PD.SUBDISTRICT_CODE is null,'',S.SUBDISTRICT_NAME_TH+' ') + ");
                    queryBuilder.AppendFormat(" IIF(PD.DISTRICT_CODE is null,'',D.DISTRICT_NAME_TH+' ') + ");
                    queryBuilder.AppendFormat(" IIF(PD.PROVINCE_CODE is null,'',P.PROVINCE_NAME_TH+' /TH'))  ");
                    queryBuilder.AppendFormat(" ADDRESS_FULLNM,  ");
                    queryBuilder.AppendFormat(" PD.* ");
                    queryBuilder.AppendFormat(" from PROSPECT_ADDRESS PD  ");
                    queryBuilder.AppendFormat(" inner join PROSPECT_ACCOUNT AC on AC.PROSP_ACC_ID = PD.PROSP_ACC_ID  ");
                    queryBuilder.AppendFormat(" inner join(  ");
                    queryBuilder.AppendFormat("     select T.PROSP_ADDR_ID,  ");
                    queryBuilder.AppendFormat("     sum(IIF(T.FUNC_CODE = 'SP',1,0)) MAIN_ADDRESS_FLAG,  ");
                    queryBuilder.AppendFormat("     sum(IIF(T.FUNC_CODE = 'SH',1,0)) SHIFT_TO_FLAG,  ");
                    queryBuilder.AppendFormat("     sum(IIF(T.FUNC_CODE = 'BP',1,0)) BILL_TO_FLAG  ");
                    queryBuilder.AppendFormat("     from(");
                    queryBuilder.AppendFormat("         select distinct PA.PROSP_ADDR_ID,CP.FUNC_CODE  ");
                    queryBuilder.AppendFormat("         from dbo.CUSTOMER_PARTNER CP  ");
                    queryBuilder.AppendFormat("         inner join CUSTOMER C on C.CUST_CODE = CP.CUST_CODE_PARTNER and CP.FUNC_CODE in ('SP','SH','BP')  ");
                    queryBuilder.AppendFormat("         inner join PROSPECT P on P.PROSPECT_ID = C.PROSPECT_ID  ");
                    queryBuilder.AppendFormat("         inner join PROSPECT_ADDRESS PA on PA.PROSP_ACC_ID = P.PROSP_ACC_ID and PA.MAIN_FLAG = 'Y'  ");
                    queryBuilder.AppendFormat("         where CP.CUST_CODE = @CUST_CODE ");
                    queryBuilder.AppendFormat("     ) T  ");
                    queryBuilder.AppendFormat("     GROUP BY T.PROSP_ADDR_ID  ");
                    queryBuilder.AppendFormat(" ) TT on TT.PROSP_ADDR_ID = PD.PROSP_ADDR_ID  ");
                    queryBuilder.AppendFormat(" left join MS_REGION R on R.REGION_CODE = PD.REGION_CODE  ");
                    queryBuilder.AppendFormat(" left join MS_PROVINCE P on P.PROVINCE_CODE = PD.PROVINCE_CODE  ");
                    queryBuilder.AppendFormat(" left join MS_DISTRICT D on D.DISTRICT_CODE = PD.DISTRICT_CODE  ");
                    queryBuilder.AppendFormat(" left join MS_SUBDISTRICT S on S.SUBDISTRICT_CODE = PD.SUBDISTRICT_CODE  ");
                    QueryUtils.addParam(command, "CUST_CODE", o.CustCode);// Add new


                    // For Paging
                    queryBuilder.AppendFormat(" ORDER BY PROSP_ADDR_ID  ");

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

                    List<ProspectAddress> prospectAddressLst = new List<ProspectAddress>();
                    ProspectAddress prospectAddress = null;
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        prospectAddress = new ProspectAddress();


                        prospectAddress.EditGeneralDataFlag = QueryUtils.getValueAsString(record, "EDIT_GENERAL_DATA_FLAG");
                        prospectAddress.ProspAddrId = QueryUtils.getValueAsDecimalRequired(record, "PROSP_ADDR_ID");
                        prospectAddress.ProspectId = QueryUtils.getValueAsDecimal(record, "PROSPECT_ID");
                        prospectAddress.ProspAccId = QueryUtils.getValueAsDecimal(record, "PROSP_ACC_ID");
                        prospectAddress.AddressFullName = QueryUtils.getValueAsString(record, "ADDRESS_FULLNM"); 
                        prospectAddress.AddrNo = QueryUtils.getValueAsString(record, "ADDR_NO");
                        prospectAddress.Moo = QueryUtils.getValueAsString(record, "MOO");
                        prospectAddress.Soi = QueryUtils.getValueAsString(record, "SOI");
                        prospectAddress.Street = QueryUtils.getValueAsString(record, "STREET");
                        prospectAddress.TellNo = QueryUtils.getValueAsString(record, "TELL_NO");
                        prospectAddress.FaxNo = QueryUtils.getValueAsString(record, "FAX_NO");
                        prospectAddress.Latitude = QueryUtils.getValueAsString(record, "LATITUDE");
                        prospectAddress.Longitude = QueryUtils.getValueAsString(record, "LONGITUDE");
                        prospectAddress.RegionCode = QueryUtils.getValueAsString(record, "REGION_CODE");
                        prospectAddress.ProvinceCode = QueryUtils.getValueAsString(record, "PROVINCE_CODE");
                        prospectAddress.ProvinceDbd = QueryUtils.getValueAsString(record, "PROVINCE_DBD");
                        prospectAddress.DistrictCode = QueryUtils.getValueAsString(record, "DISTRICT_CODE");
                        prospectAddress.SubdistrictCode = QueryUtils.getValueAsString(record, "SUBDISTRICT_CODE");
                        prospectAddress.PostCode = QueryUtils.getValueAsString(record, "POST_CODE");
                        prospectAddress.Remark = QueryUtils.getValueAsString(record, "REMARK");
                        prospectAddress.MainFlag = QueryUtils.getValueAsString(record, "MAIN_FLAG");
                        prospectAddress.ActiveFlag = QueryUtils.getValueAsString(record, "ACTIVE_FLAG");
                        prospectAddress.CreateUser = QueryUtils.getValueAsString(record, "CREATE_USER");
                        prospectAddress.CreateDtm = QueryUtils.getValueAsDateTime(record, "CREATE_DTM");
                        prospectAddress.UpdateUser = QueryUtils.getValueAsString(record, "UPDATE_USER");
                        prospectAddress.UpdateDtm = QueryUtils.getValueAsDateTime(record, "UPDATE_DTM");

                        prospectAddress.ShiftToFlag = QueryUtils.getValueAsString(record, "SHIFT_TO_FLAG");
                        prospectAddress.MainAddressFlag = QueryUtils.getValueAsString(record, "MAIN_ADDRESS_FLAG");
                        prospectAddress.BillToFlag = QueryUtils.getValueAsString(record, "BILL_TO_FLAG");

                        prospectAddressLst.Add(prospectAddress);
                    }
                    // Call Close when done reading.
                    reader.Close();

                    //
                    lst = prospectAddressLst;
                    searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                    searchResult.data = lst;

                }
            }
            return searchResult;
        }






    }
}
