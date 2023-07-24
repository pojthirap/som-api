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
    public interface IMsDistrict
    {
        Task<EntitySearchResultBase<MsDistrict>> Search(SearchCriteriaBase<MsDistrictCriteria> searchCriteria);
        Task<int> updDistrictByProvinceCode(MsDistrictModel msDistrictModel);
    }
}
