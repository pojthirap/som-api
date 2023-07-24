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
    public interface IMsOrderDocType
    {
        Task<EntitySearchResultBase<MsOrderDocType>> searchOrderDocType(SearchCriteriaBase<SearchOrderDocTypeCriteria> searchCriteria);
       
    }
}
