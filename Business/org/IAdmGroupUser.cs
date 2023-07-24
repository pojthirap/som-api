using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.adm;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IAdmGroupUser
    {
        Task<AdmGroupUser> Add(AdmGroupUserModel admGroupUserModel, EmployeeGroup employeeGroup);
        Task<int> Update(AdmGroupUserModel admGroupUserModel);


    }
}
