using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models.pospect;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IProspectRecommend
    {
        Task<EntitySearchResultBase<OrgBusinessUnit>> searchProspectRecommend(SearchCriteriaBase<ProspectRecommendCriteria> searchCriteria);
        Task<ProspectRecommend> addProspectRecommend(ProspectRecommendModel prospectRecommendModel, int buIdIndex);
        Task<int> delProspectRecommend(ProspectRecommendModel prospectRecommendModel);
    }
}
