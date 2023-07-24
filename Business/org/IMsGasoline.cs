using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.ModelCriteria;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IMsGasoline
    {
        Task<EntitySearchResultBase<MsGasoline>> Search(SearchCriteriaBase<MsGasolineCriteria> searchCriteria);
        Task<MsGasoline> Add(MsGasolineModel msGasolineModel);
        Task<int> Update(MsGasolineModel msGasolineModel);
        Task<int> DeleteUpdate(MsGasolineModel msGasolineModel);
    }
}
