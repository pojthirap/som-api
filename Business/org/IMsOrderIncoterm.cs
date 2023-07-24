using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.ModelCriteria;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IMsOrderIncoterm
    {
        Task<EntitySearchResultBase<MsOrderIncoterm>> searchOrderIncoterm(SearchCriteriaBase<SearchOrderIncotermCriteria> searchCriteria);
    }
}
