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
using MyFirstAzureWebApp.Models.plan;
using static MyFirstAzureWebApp.Business.org.PlanTripImp;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IPlanTripTask
    {
        Task<EntitySearchResultBase<PlanTripTask>> viewPlanTripTask(SearchCriteriaBase<ViewPlanTripTaskCriteria> searchCriteria);
        Task<PlanTripTask> addPlanTripTaskAdHoc(PlanTripTaskModel planTripTaskModel);

    }
}
