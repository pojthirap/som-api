using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.ModelCriteria;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IAdmPermObject
    {
        Task<EntitySearchResultBase<AdmPermObject>> searchAdmPermObject(SearchCriteriaBase<SearchAdmPermObjectCriteria> searchCriteria);
    }
}
