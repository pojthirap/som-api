using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Entity.custom;
using static MyFirstAzureWebApp.Business.org.TemplateAppFormImp;
using MyFirstAzureWebApp.Models.record;

namespace MyFirstAzureWebApp.Business.org
{
    public interface ITemplateAppForm
    {
        Task<EntitySearchResultBase<TemplateAppForm>> Search(SearchCriteriaBase<TemplateAppFormCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<TemplateAppForm> Add(TemplateAppFormModel templateAppFormModel);
        Task<int> Update(TemplateAppFormModel templateAppFormModel);
        Task<int> DeleteUpdate(TemplateAppFormModel templateAppFormModel);
        Task<EntitySearchResultBase<GetTaskTemplateAppFormForCreatPlanCustom>> getTaskTemplateAppFormForCreatPlan(SearchCriteriaBase<GetTaskTemplateAppFormForCreatPlanCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<EntitySearchResultBase<GetTaskSpecialForCreatPlanCustom>> getTaskSpecialForCreatPlan(SearchCriteriaBase<GetTaskSpecialForCreatPlanCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<GetTaskTemplateAppFormForRecordResult> getTaskTemplateAppFormForRecord(SearchCriteriaBase<GetTaskTemplateAppFormForRecordCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<RecordAppForm> addRecordAppForm(AddRecordAppFormModel addRecordAppFormModel, UserProfileForBack userProfile);
        Task<EntitySearchResultBase<TemplateAppForm>> searchTemplateAppForm(SearchCriteriaBase<SearchTemplateAppFormCriteria> searchCriteria);


    }
}
