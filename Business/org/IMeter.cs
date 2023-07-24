using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.record;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models.profile;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IMeter
    {
        Task<MsMeter> Add(MeterModel meterModel);
        Task<int> Update(MeterModel meterModel);
        Task<int> DeleteUpdate(MeterModel meterModel);
        Task<EntitySearchResultBase<GetTaskMeterForRecordCustom>> getTaskMeterForRecord(SearchCriteriaBase<GetTaskMeterForRecordCriteria> searchCriteria);
        Task<EntitySearchResultBase<SearchMeterCustom>> searchMeter(SearchCriteriaBase<SearchMeterCriteria> searchCriteria);
        Task<List<RecordMeter>> addRecordMeter(List<RecordMeterModel> recordMeterModelList, UserProfileForBack userProfileForBack);
        Task<EntitySearchResultBase<CheckPercentRangRecordMeterCustom>> checkPercentRangRecordMeter(SearchCriteriaBase<CheckPercentRangRecordMeterCriteria> searchCriteria, string language);


    }
}
