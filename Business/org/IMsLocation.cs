using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.ms;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IMsLocation
    {
        Task<EntitySearchResultBase<SearchLocationCustom>> Search(MsLocationSearchCriteria searchCriteria);
        Task<MsLocation> Add(MsLocationModel msLocationModel);
        Task<int> Update(MsLocationModel msLocationModel);
        Task<int> DeleteUpdate(MsLocationModel msLocationModel);

    }
}
