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
    public interface ITemplateSaTitle
    {
        
        Task<EntitySearchResultBase<SearchTemplateSaFormByIdCustom>> searchTemplateSaFormById(SearchCriteriaBase<TemplateSaTitleCriteria> searchCriteria);
        //Task<EntitySearchResultBase<TemplateSaTitle>> searchTemplateSaFormById(SearchCriteriaBase<TemplateSaTitleCriteria> searchCriteria);
        Task<TemplateSaTitle> addTemplateSaTitle(TemplateSaTitleModel templateSaTitleModel);
        //Task<TemplateSaTitle> Update(TemplateSaTitleModel templateSaTitleModel);
        Task<int> delTemplateSaTitle(TemplateSaTitleModel templateSaTitleModel);
    }
}
