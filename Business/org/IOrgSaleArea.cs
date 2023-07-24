using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.ModelCriteria;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IOrgSaleArea
    {
        Task<EntitySearchResultBase<OrgSaleAreaCustom>> Search(SearchCriteriaBase<OrgSaleAreaCriteria> searchCriteria);
        Task<int> MapBU(OrgSaleAreaModel model);
    }
}
