using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.adm;
using MyFirstAzureWebApp.Models.profile;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IAdmGroupApp
    {
        Task<EntitySearchResultBase<SearchAdmGroupAppCustom>> searchAdmGroupApp(SearchCriteriaBase<SearchAdmGroupAppCriteria> searchCriteria);
        Task<AdmGroupApp> addAdmGroupApp(AddAdmGroupAppModel addAdmGroupAppModel, UserProfileForBack userProfile);
        Task<int> updAdmGroupApp(UpdAdmGroupAppModel updAdmGroupAppModel, UserProfileForBack userProfile);
        Task<int> cancelAdmGroupApp(CancelAdmGroupAppModel cancelAdmGroupAppModel, UserProfileForBack userProfile);
        Task<EntitySearchResultBase<SearchAdmGroupPermCustom>> searchAdmGroupPerm(SearchCriteriaBase<SearchAdmGroupPermCriteria> searchCriteria);
        Task<int> updAdmGroupPerm(UpdAdmGroupPermModel updAdmGroupPermModel, UserProfileForBack userProfile);


    }
}
