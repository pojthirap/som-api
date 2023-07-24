using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.ms;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IMsServiceType
    {
        Task<EntitySearchResultBase<MsServiceType>> Search(MsServiceTypeSearchCriteria searchCriteria);
        Task<MsServiceType> Add(MsServiceTypeModel msServiceTypeModel);
        Task<int> Update(MsServiceTypeModel msServiceTypeModel);
        Task<int> DeleteUpdate(MsServiceTypeModel msServiceTypeModel);
    }
}
