using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.org;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IRegion
    {
        Task<EntitySearchResultBase<MsRegion>> Search(RegionSearchCriteria searchCriteria);
        Task<MsRegion> Add(RegionModel regionModel);
        Task<int> Update(RegionModel regionModel);
        Task<int> DeleteUpdate(RegionModel model);
    }
}
