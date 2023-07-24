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
    public interface IMsActtachCategory
    {
        Task<EntitySearchResultBase<MsAttachCategory>> Search(SearchCriteriaBase<MsActtachCategoryCriteria> searchCriteria);
        Task<MsAttachCategory> Add(MsActtachCategoryModel msActtachCategoryModel);
        Task<int> Update(MsActtachCategoryModel msActtachCategoryModel);
        Task<int> DeleteUpdate(MsActtachCategoryModel msActtachCategoryModel);
    }
}
