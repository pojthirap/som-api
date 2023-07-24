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
using MyFirstAzureWebApp.Models.profile;

namespace MyFirstAzureWebApp.Business.org
{

    public class OrgSaleGroupImp : IOrgSaleGroup

    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public async Task<EntitySearchResultBase<OrgSaleGroup>> Search(OrgSaleGroupSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                var sqlParameters = new List<SqlParameter>();// Add New
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("select * from ORG_SALE_GROUP where 1=1 ");
                OrgSaleGroupCriteria o = searchCriteria.model;
                if (o != null)
                {

                    if (!String.IsNullOrEmpty(o.description))
                    {
                        queryBuilder.AppendFormat(" and DESCRIPTION_TH  like @Description  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameterLike("Description", o.description));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.territoryId))
                    {
                        queryBuilder.AppendFormat(" and TERRITORY_ID   = @TERRITORY_ID   ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TERRITORY_ID", o.territoryId));// Add New
                    }
                    if (!String.IsNullOrEmpty(o.activeFlag))
                    {
                        queryBuilder.AppendFormat(" and ACTIVE_FLAG  = @ActiveFlag  ");// Edit New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ActiveFlag", o.activeFlag));// Add New
                    }
                }
                // For Paging
                queryBuilder.AppendFormat(" ORDER BY DESCRIPTION_TH  ");
                int count_ = 0;
                if (searchCriteria.length != 0)
                {
                    log.Debug("Query Count:" + queryBuilder.ToString());
                    Console.WriteLine("Query Count:" + queryBuilder.ToString());
                    var elements = context.OrgSaleGroup.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                    count_ = elements.Count();

                    QueryUtils.setSqlPaging(queryBuilder, sqlParameters, searchCriteria.length, searchCriteria.pageNo);
                }
                // For Paging

                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                List<OrgSaleGroup> lst = context.OrgSaleGroup.FromSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray()).ToList();
                EntitySearchResultBase<OrgSaleGroup> searchResult = new EntitySearchResultBase<OrgSaleGroup>();
                searchResult.totalRecords = searchCriteria.length == 0 ? lst == null ? 0 : lst.Count() : count_;
                searchResult.data = lst;
                return searchResult;

            }

        }



        // Update Manager Sale Group

        public async Task<int> UpdateManagerSaleGroup(OrgSaleGroupModel model)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE ORG_SALE_GROUP SET MANAGER_EMP_ID = @MANAGER_EMP_ID, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE GROUP_CODE=@GROUP_CODE ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("MANAGER_EMP_ID", model.ManagerEmpId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", model.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_CODE", model.GroupCode));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" UPDATE E ");
                        queryBuilder.AppendFormat(" SET  ");
                        queryBuilder.AppendFormat("     E.APPROVE_EMP_ID=@APPROVE_EMP_ID ");
                        queryBuilder.AppendFormat(" from ADM_EMPLOYEE E ");
                        queryBuilder.AppendFormat(" inner join ADM_GROUP_USER GU on GU.EMP_ID = E.EMP_ID ");
                        queryBuilder.AppendFormat(" inner join ADM_GROUP G on G.GROUP_ID = GU.GROUP_ID and G.GROUP_CODE in ('SA','SALE_REP','00005') ");
                        queryBuilder.AppendFormat(" where E.GROUP_CODE = @GROUP_CODE ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("APPROVE_EMP_ID", model.ApproveEmpId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_CODE", model.GroupCode));// Add New
                        queryStr = queryBuilder.ToString();
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


        public async Task<int> updTerritorySaleGroup(UpdateTerritorySaleGroupModel model, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        int numberOfRowInserted = 0;
                        if (model.ListGroupCode != null && model.ListGroupCode.Count != 0)
                        {
                            foreach (string groupCode in model.ListGroupCode)
                            {
                                var sqlParameters = new List<SqlParameter>();// Add New
                                StringBuilder queryBuilder = new StringBuilder();
                                queryBuilder.AppendFormat(" UPDATE [dbo].[ORG_SALE_GROUP]  ");
                                queryBuilder.AppendFormat(" SET [TERRITORY_ID]=@territoryId, [UPDATE_USER]=@UPDATE_USER, [UPDATE_DTM]=dbo.GET_SYSDATETIME() ");
                                queryBuilder.AppendFormat(" WHERE GROUP_CODE = @groupCode ");
                                sqlParameters.Add(QueryUtils.addSqlParameter("territoryId", model.TerritoryId));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", userProfile.getUserName()));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("groupCode", groupCode));// Add New
                                string queryStr = queryBuilder.ToString();
                                log.Debug("Query:" + queryStr);
                                Console.WriteLine("Query:" + queryStr);
                                numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
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



        public async Task<int> updBusinessUnitSaleArea(UpdBusinessUnitSaleAreaModel model, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        int numberOfRowInserted = 0;
                        if (model.ListAreaId != null && model.ListAreaId.Count != 0)
                        {
                            foreach (string areaId in model.ListAreaId)
                            {
                                var sqlParameters = new List<SqlParameter>();// Add New
                                StringBuilder queryBuilder = new StringBuilder();


                                queryBuilder.AppendFormat(" UPDATE [dbo].[ORG_SALE_AREA]   ");
                                queryBuilder.AppendFormat(" SET [BU_ID]= @buId, [UPDATE_USER]=@UPDATE_USER, [UPDATE_DTM]=dbo.GET_SYSDATETIME()  ");
                                queryBuilder.AppendFormat(" WHERE AREA_ID = @areaId  ");
                                sqlParameters.Add(QueryUtils.addSqlParameter("buId", model.BuId));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", userProfile.getUserName()));// Add New
                                sqlParameters.Add(QueryUtils.addSqlParameter("areaId", areaId));// Add New
                                string queryStr = queryBuilder.ToString();
                                log.Debug("Query:" + queryStr);
                                Console.WriteLine("Query:" + queryStr);
                                numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
                                log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                                Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
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



    }
}
