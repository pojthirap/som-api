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

namespace MyFirstAzureWebApp.Business.org
{

    public class OrgDivisionImp : IOrgDivision

    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<OrgDivision>> Search(OrgDivisionSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from ORG_DIVISION where 1=1 ");
                OrgDivisionCriteria o = searchCriteria.model;
                if (o != null)
                {

                    if (!String.IsNullOrEmpty(o.divisionName))
                    {
                        queryBuilder.AppendFormat(" and DIVISION_NAME_TH  like @DivisionName   ");// Edit New
                        sqlParameters.Add(new SqlParameter("@DivisionName", "%" + o.divisionName + "%"));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.activeFlag))
                    {
                        queryBuilder.AppendFormat(" and ACTIVE_FLAG  = @ActiveFlag  ");// Edit New
                        sqlParameters.Add(new SqlParameter("@ActiveFlag", o.activeFlag));// Add New
                    }
                }
                // For Paging
                if (1 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY DIVISION_CODE  ");
                }else if (2 == searchCriteria.searchOrder)
                {
                    queryBuilder.AppendFormat(" ORDER BY DIVISION_NAME_TH  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY UPDATE_DTM DESC  "); 
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.OrgDivision.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();// Add New
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<OrgDivision> lst = context.OrgDivision.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<OrgDivision> searchResult = new EntitySearchResultBase<OrgDivision>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }




    }
}
