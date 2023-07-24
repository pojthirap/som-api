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
using MyFirstAzureWebApp.Models.profile;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IRecordAppForm
    {
        Task<EntitySearchResultBase<SearchSurveyResultTabCustom>> searchSurveyResultTab(SearchCriteriaBase<SearchSurveyResultTabCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<EntitySearchResultBase<ViewSurveyResultCustom>> viewSurveyResult(SearchCriteriaBase<ViewSurveyResultCriteria> searchCriteria, UserProfileForBack userProfile);
    }
}
