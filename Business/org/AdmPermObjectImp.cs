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

namespace MyFirstAzureWebApp.Business.org
{

    public class AdmPermObjectImp : IAdmPermObject
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<AdmPermObject>> searchAdmPermObject(SearchCriteriaBase<SearchAdmPermObjectCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select * ");
                queryBuilder.AppendFormat(" from ADM_PERM_OBJECT ");
                queryBuilder.AppendFormat(" where PERM_TYPE = 'APP'  ");//Fix
                SearchAdmPermObjectCriteria o = searchCriteria.model;
                
                queryBuilder.AppendFormat(" ORDER BY PERM_OBJ_NAME_TH  ");
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.AdmPermObject.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<AdmPermObject> lst = context.AdmPermObject.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<AdmPermObject> searchResult = new EntitySearchResultBase<AdmPermObject>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }





    }
}
