using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Models.ms;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IMsLocationType
    {
        Task<EntitySearchResultBase<MsLocationType>> Search(MsLocationTypeSearchCriteria searchCriteria);
        Task<MsLocationType> Add(MsLocationTypeModel msLocationTypeModel);
        Task<int> Update(MsLocationTypeModel msLocationTypeModel);
        Task<int> DeleteUpdate(MsLocationTypeModel msLocationTypeModel);

    }
}
