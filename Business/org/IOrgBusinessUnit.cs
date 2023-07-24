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
    public interface IOrgBusinessUnit
    {
        Task<EntitySearchResultBase<OrgBusinessUnit>> Search(OrgBusinessUnitSearchCriteria searchCriteria, UserProfileForBack userProfile);
        Task<OrgBusinessUnit> Add(BusinessUnitModel model);
        Task<int> Update(BusinessUnitModel model);
        Task<int> DeleteUpdate(BusinessUnitModel model);

    }
}
