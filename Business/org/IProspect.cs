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
    public interface IProspect
    {
        
        Task<int> updProspectSaTab(UpdateProspectModel updateProspectModel, UserProfileForBack userProfile);
        Task<int> updProspectDbdTab(ProspectModel prospectModel, UserProfileForBack userProfile);
        Task<int> updProspectBasicTab(UpdateProspectModel updateProspectModel, UserProfileForBack userProfile, string language);
        Task<Prospect> cloneProspect(CloneProspectModel cloneProspectModel, UserProfileForBack userProfile, string language);
        Task<EntitySearchResultBase<SearchProspectAllCustom>> searchProspectAll(SearchCriteriaBase<SearchProspectAllCriteria> searchCriteria);

    }
}
