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
using static MyFirstAzureWebApp.Business.org.PlanTripProspectImp;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IPlanTripProspect
    {
        Task<EntitySearchResultBase<GetLastCheckInCustom>> getLastCheckIn(SearchCriteriaBase<GetLastCheckInCriteria> searchCriteria);
        //Task<List<checkInPlanTripProspectData>> checkInPlanTripProspect(PlanTripProspectModel planTripProspectModel);
        Task<checkInPlanTripProspectData> checkInPlanTripProspect(PlanTripProspectModel planTripProspectModel);
        Task<int> checkOutPlanTripProspect(CheckOutPlanTripProspectModel checkOutPlanTripProspectModel);
        Task<int> updReasonNotVisitForProspect(PlanTripProspectModel planTripProspectModel);
        Task<int> updateRemindForProspect(PlanTripProspectModel planTripProspectModel);
        Task<EntitySearchResultBase<GetAddressForBestRouteCustom>> getAddressForBestRoute(SearchCriteriaBase<GetAddressForBestRouteCriteria> searchCriteria);
        Task<PlanTripProspect> addPlanTripProspectAdHoc(PlanTripProspectModel planTripProspectModel);
        Task<int> updateLocRemarkForProspect(UpdateLocRemarkForProspectModel updateLocRemarkForProspectModel, UserProfileForBack userProfile);


    }
}
