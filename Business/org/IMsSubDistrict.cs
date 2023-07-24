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
using MyFirstAzureWebApp.Models.org;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IMsSubDistrict
    {
        Task<EntitySearchResultBase<SearchSubDistrictCustom>> Search(SearchCriteriaBase<MsSubDistrictCriteria> searchCriteria);
        Task<int> updSubDistrictByDistrictCode(MsSubDistrictModel msSubDistrictModel, string language);
        Task<EntitySearchResultBase<SearchMsSubDistrictCustom>> searchMsSubDistrict(SearchCriteriaBase<SearchMsSubDistrictCriteria> searchCriteria);
        Task<int> delSubDistrictSomById(DelSubDistrictSomByIdModel model);
    }
}
