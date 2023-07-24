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
    public interface ITemplateCategory
    {
        Task<EntitySearchResultBase<TemplateCategory>> Search(SearchCriteriaBase<TemplateCategoryCriteria> searchCriteria);
        Task<TemplateCategory> Add(TemplateCategoryModel templateCategoryModel);
        Task<int> Update(TemplateCategoryModel templateCategoryModel);
        Task<int> DeleteUpdate(TemplateCategoryModel templateCategoryModel);
    }
}
