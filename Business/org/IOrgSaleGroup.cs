using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Models.profile;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IOrgSaleGroup
    {
        Task<EntitySearchResultBase<OrgSaleGroup>> Search(OrgSaleGroupSearchCriteria searchCriteria);
        Task<int> UpdateManagerSaleGroup(OrgSaleGroupModel model);
        Task<int> updTerritorySaleGroup(UpdateTerritorySaleGroupModel model, UserProfileForBack userProfile);
        Task<int> updBusinessUnitSaleArea(UpdBusinessUnitSaleAreaModel model, UserProfileForBack userProfile);

    }
}
