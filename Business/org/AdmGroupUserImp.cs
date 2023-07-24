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
using MyFirstAzureWebApp.Models.adm;
using MyFirstAzureWebApp.Utils;

namespace MyFirstAzureWebApp.Business.org
{

    public class AdmGroupUserImp : IAdmGroupUser
    {
        private Logger log = LogManager.GetCurrentClassLogger();




        public async Task<AdmGroupUser> Add(AdmGroupUserModel admGroupUserModel, EmployeeGroup employeeGroup)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for ADM_GROUP_USER_SEQ", p);
                        var nextVal = (int)p.Value;
                        string code_ = QueryUtils.padLeft(nextVal.ToString(), '0', 4);

                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("INSERT INTO  ADM_GROUP_USER (GROUP_USER_ID, GROUP_ID, BU_ID, EMP_ID, GROUP_USER_TYPE, ACTIVE_FLAG, EFFECTIVE_DATE, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                        queryBuilder.AppendFormat("VALUES(@GROUP_USER_ID, @GROUP_ID, @BU_ID, @EMP_ID, @GROUP_USER_TYPE, 'Y', dbo.GET_SYSDATETIME(), @CREATE_USER, dbo.GET_SYSDATETIME(), @UPDATE_USER, dbo.GET_SYSDATETIME())");
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_USER_ID", nextVal));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_ID", admGroupUserModel.GroupId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BU_ID", admGroupUserModel.BuId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("EMP_ID", employeeGroup.EmpId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_USER_TYPE", admGroupUserModel.GroupUserType));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("CREATE_USER", admGroupUserModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", admGroupUserModel.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                        transaction.Commit();
                        AdmGroupUser re = new AdmGroupUser();
                        re.GroupUserId = nextVal;
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



        public async Task<int> Update(AdmGroupUserModel admGroupUserModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        String groupUserIdStr = "";
                        int length_ = admGroupUserModel.EmployeeGroups.Count;
                        String[] groupUserIdList = new String[length_];
                        for (int i = 0; i < length_; i++)
                        {
                            groupUserIdList[i] = admGroupUserModel.EmployeeGroups.ElementAt(i).GroupUserId;
                        }
                        groupUserIdStr = String.Join(",", groupUserIdList);
                        StringBuilder queryBuilder = new StringBuilder();
                        var sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder.AppendFormat("UPDATE ADM_GROUP_USER SET GROUP_ID = @GROUP_ID, BU_ID=@BU_ID, UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE GROUP_USER_ID in (" + QueryUtils.getParamIn("groupUserIdStr", groupUserIdStr) + ") ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_ID", admGroupUserModel.GroupId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BU_ID", admGroupUserModel.BuId));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", admGroupUserModel.getUserName()));// Add New
                        QueryUtils.addParamIn(sqlParameters, "groupUserIdStr", groupUserIdStr);
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
