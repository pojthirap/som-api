using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models.plan;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IPlanReasonNotVisit
    {
        Task<EntitySearchResultBase<PlanReasonNotVisit>> Search(SearchCriteriaBase<PlanReasonNotVisitCriteria> searchCriteria);
        Task<PlanReasonNotVisit> Add(PlanReasonNotVisitModel planReasonNotVisitModel);
        Task<int> Update(PlanReasonNotVisitModel planReasonNotVisitModel);
        Task<int> DeleteUpdate(PlanReasonNotVisitModel planReasonNotVisitModel);
    }
}
