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
    public interface ITemplateQuestion
    {
        Task<EntitySearchResultBase<SearchTemplateQuestionCustom>> Search(SearchCriteriaBase<TemplateQuestionCriteria> searchCriteria);
        Task<TemplateQuestion> Add(TemplateQuestionModel templateQuestionModel);
        Task<int> Update(TemplateQuestionModel templateQuestionModel);
        Task<int> DeleteUpdate(TemplateQuestionModel templateQuestionModel);
        Task<EntitySearchResultBase<SearchTemplateQuestionCustom>> searchTemplateQuestion(SearchCriteriaBase<TemplateQuestionCriteria> searchCriteria, UserProfileForBack userProfileForBack);
    }
}
