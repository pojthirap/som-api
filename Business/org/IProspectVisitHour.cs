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
using static MyFirstAzureWebApp.Controllers.BaseController;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.pospect;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IProspectVisitHour
    {
        Task<EntitySearchResultBase<SearchVisitHourCustom>> searchVisitHour(SearchCriteriaBase<SearchProspectVisitHourCriteria> searchCriteria);
        Task<List<ProspectVisitHour>> addVisitHour(ProspectVisitHourModel prospectVisitHourModel);
        Task<int> delVisitHour(ProspectVisitHourModel prospectVisitHourModel);

    }
}
