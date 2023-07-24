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

    public class OrgDistChannelImp : IOrgDistChannel

    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<OrgDistChannel>> Search(OrgDistChannelSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New


                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from ORG_DIST_CHANNEL where 1=1 ");
                OrgDistChannelCriteria o = searchCriteria.model;
                if (o != null)
                {

                    if (!String.IsNullOrEmpty(o.channelName))
                    {
                        queryBuilder.AppendFormat(" and CHANNEL_NAME_TH like @ChannelName  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("ChannelName", o.channelName));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.activeFlag))
                    {
                        queryBuilder.AppendFormat(" and ACTIVE_FLAG  = @ActiveFlag  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", o.activeFlag));// Add New
                    }
                }
                // For Paging
                if (searchCriteria.searchOrder == 2)
                {
                    queryBuilder.AppendFormat(" ORDER BY CHANNEL_NAME_TH  ");
                }
                else
                {
                    queryBuilder.AppendFormat(" ORDER BY CHANNEL_CODE  ");
                }
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.OrgDistChannel.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();// Add New
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                // Edit New
                List<OrgDistChannel> lst = context.OrgDistChannel.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList(); // Add New
                EntitySearchResultBase<OrgDistChannel> searchResult = new EntitySearchResultBase<OrgDistChannel>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }




    }
}
