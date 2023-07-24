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

namespace MyFirstAzureWebApp.Business.org
{
    public interface IMsConfigLov
    {
        Task<EntitySearchResultBase<MsConfigLov>> Search(SearchCriteriaBase<MsConfigLovCriteria> searchCriteria);
        Task<EntitySearchResultBase<GetMasterDataForTemplateSaCustom>> getMasterDataForTemplateSa(SearchCriteriaBase<GetMasterDataForTemplateSaCriteria> searchCriteria);
    }
}
