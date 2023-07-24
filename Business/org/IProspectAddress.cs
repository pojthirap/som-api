using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Entity.custom;
using static MyFirstAzureWebApp.Controllers.BaseController;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.pospect;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IProspectAddress
    {
        Task<EntitySearchResultBase<ProspectAddress>> searchProspectAddress(SearchCriteriaBase<SearchProspectAddressCriteria> searchCriteria);

    }
}
