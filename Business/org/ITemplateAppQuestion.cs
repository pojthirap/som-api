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
    public interface ITemplateAppQuestion
    {
        Task<EntitySearchResultBase<TemplateAppQuestion>> Search(SearchCriteriaBase<TemplateAppQuestionCriteria> searchCriteria);
        Task<List<TemplateAppQuestion>> Add(List<TemplateAppQuestionModel> templateAppQuestionModel, UserProfileForBack userProfileForBack);
        Task<int> Update(TemplateAppQuestionModel templateAppQuestionModel);
        Task<int> DeleteUpdate(TemplateAppQuestionModel templateAppQuestionModel);
        Task<int> DeleteByTemplateAppFormId(TemplateAppQuestionModel model);
        Task<EntitySearchResultBase<SearchTemplateAppQuestionByIdCustom>> searchTemplateAppQuestionById(SearchCriteriaBase<TemplateAppQuestionCriteria> searchCriteria);
    }
}
