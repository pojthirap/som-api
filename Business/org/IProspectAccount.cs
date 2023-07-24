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
    public interface IProspectAccount
    {
        Task<EntitySearchResultBase<SearchMyProspectCustom>> searchMyAccount(SearchCriteriaBase<ProspectAccountCriteria> searchCriteria, UserProfileForBack userProfileForBack);
        Task<EntitySearchResultBase<SearchProspectRecommendCustom>> searchProspectRecommend(SearchCriteriaBase<ProspectAccountCriteria> searchCriteria, UserProfileForBack userProfileForBack);
        Task<EntitySearchResultBase<SearchAccountInTerritoryCustom>> searchAccountInTerritory(SearchCriteriaBase<ProspectAccountCriteria> searchCriteria, UserProfileForBack userProfileForBack);
        Task<EntitySearchResultBase<SearchOtherProspectCustom>> searchOtherProspect(SearchCriteriaBase<ProspectAccountCriteria> searchCriteria, UserProfileForBack userProfileForBack);
        Task<CreateProspectModel> createProspect(CreateProspectModel createProspectModel, UserProfileForBack userProfile, string language);
        Task<EntitySearchResultBase<SearchProspectSaTabCustom>> searchProspectSaTab(SearchCriteriaBase<ProspectCriteria> searchCriteria);
        Task<EntitySearchResultBase<GetProspectForCreatePlanTripAdHocCustom>> getProspectForCreatePlanTripAdHoc(SearchCriteriaBase<GetProspectForCreatePlanTripAdHocCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<EntitySearchResultBase<GetProspectForCreatePlanTripCustom>> getProspectForCreatePlanTrip(SearchCriteriaBase<GetProspectForCreatePlanTripCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<int> delProspect(DeleteProspectModel model, UserProfileForBack userProfile);
        Task<int> delPlanTripProspectAdHoc(DeletePlanTripProspectAdHocModel model);
        Task<int> delPlanTripTaskAdHoc(DeletePlanTripTaskAdHocModel model);

    }
}
