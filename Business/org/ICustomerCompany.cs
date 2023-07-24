using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Entity.custom;

namespace MyFirstAzureWebApp.Business.org
{
    public interface ICustomerCompany
    {
        Task<EntitySearchResultBase<SearCompanyCustom>> searCompany(SearchCriteriaBase<SearCompanyCriteria> searchCriteria);
    }
}
