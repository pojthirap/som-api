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
    public interface IPlanTrip
    {
        Task<EntitySearchResultBase<SearchPlanTripForSaleVisitCustom>> searchPlanTripForSaleVisit(SearchCriteriaBase<SearchPlanTripForSaleVisitCriteria> searchCriteria, UserProfileForBack userProfileForBack);
        Task<int> planTripStart(PlanTripModel planTripModel);
        Task<int> planTripFinish(PlanTripModel planTripModel);
        Task<KmTotalPlanTrip> getKmTotalPlanTripFinish(string PlanTripId);
        Task<EntitySearchResultBase<ViewPlanTripCustom>> viewPlanTrip(SearchCriteriaBase<ViewPlanTripCriteria> searchCriteria);
        Task<EntitySearchResultBase<PlanTrip>> searchPlanTrip(SearchCriteriaBase<SearchPlanTripCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<EntitySearchResultBase<AdmEmployee>> getEmpForAssignPlanTrip(SearchCriteriaBase<GetEmpForAssignPlanTripCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<PlanTrip> createPlanTrip(CreatePlanTripModel createPlanTripModel, UserProfileForBack userProfile);
        Task<int> updPlanTrip(UpdatePlanTripModel updatePlanTripModel, UserProfileForBack userProfile);
        Task<int> mergPlanTrip(MergPlanTripModel mergPlanTripModel, UserProfileForBack userProfile, string language);
        Task<int> cancelPlanTrip(CancelPlanTripModel cancelPlanTripModel, UserProfileForBack userProfile);
        Task<int> rejectPlanTrip(RejectPlanTripModel rejectPlanTripModel, UserProfileForBack userProfile);
        Task<int> approvePlanTrip(ApprovePlanTripModel approvePlanTripModel, UserProfileForBack userProfile, string language);
        Task<EntitySearchResultBase<GetLastRemindPlanTripProspectCustom>> getLastRemindPlanTripProspect(SearchCriteriaBase<GetLastRemindPlanTripProspectCriteria> searchCriteria);
        Task<List<GetEmailToSendForCreatePlantripCustom>> getEmailToSendForCreatePlantrip(string empId);
        Task<List<GetEmailToSendForUpdatePlantripCustom>> getEmailToSendForUpdatePlantrip(string empId);
        Task<List<GetEmailToSendForRejectPlantripCustom>> getEmailToSendForRejectPlanTrip(string planTripId);
        Task<List<GetEmailToSendForApprovePlanTripCustom>> getEmailToSendForapprovePlanTrip(string planTripId);

    }
}
