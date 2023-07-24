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
using static MyFirstAzureWebApp.Business.org.TemplateSaFormImp;
using MyFirstAzureWebApp.Models.record;

namespace MyFirstAzureWebApp.Business.org
{
    public interface ITemplateSaForm
    {
        Task<EntitySearchResultBase<SearchTemplateSaFormCustom>> searchTemplateSaForm(SearchCriteriaBase<TemplateSaFormCriteria> searchCriteria);
        Task<TemplateSaForm> Add(TemplateSaFormModel templateSaFormModel);
        Task<int> Update(TemplateSaFormModel templateSaFormModel);
        Task<int> DeleteUpdate(TemplateSaFormModel templateSaFormModel);
        Task<GetTaskTemplateSaFormForRecordResult> getTaskTemplateSaFormForRecord(SearchCriteriaBase<GetTaskTemplateSaFormForRecordCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<RecordSaForm> addRecordSaForm(AddRecordSaFormModel addRecordSaFormModel, UserProfileForBack userProfile);
        Task<EntitySearchResultBase<TemplateSaForm>> searchTemplateSa(SearchCriteriaBase<SearchTemplateSaCriteria> searchCriteria);
    }
}
