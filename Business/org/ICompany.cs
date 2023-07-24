using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.ModelCriteria;

namespace MyFirstAzureWebApp.Business.org
{
    public interface ICompany
    {
        Task<EntitySearchResultBase<OrgCompany>> Search(SearchCriteriaBase<OrgCompanyCriteria> searchCriteria);
    }
}
