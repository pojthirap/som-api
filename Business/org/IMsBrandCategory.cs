using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Models.ms;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IMsBrandCategory
    {
        Task<EntitySearchResultBase<MsBrandCategory>> Search(MsBrandCategorySearchCriteria searchCriteria);
        Task<MsBrandCategory> Add(MsBrandCategoryModel msBrandCategoryModel);
        Task<int> Update(MsBrandCategoryModel msBrandCategoryModel);
        Task<int> DeleteUpdate(MsBrandCategoryModel msBrandCategoryModel);

    }
}
