using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models.job;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Entity.custom;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IEmailJob
    {
        Task<EmailJob> Add(EmailJobModel emailJobModel, UserProfileForBack userProfile);
        Task<EntitySearchResultBase<SearchEmailJobForPlanTripCustom>> searchEmailJobForPlanTrip(SearchCriteriaBase<SearchEmailJobForPlanTripCriteria> searchCriteria, UserProfileForBack userProfile);
        //Task<int> Update(EmailJobModel emailJobModel, UserProfileForBack userProfile);
    }
}
