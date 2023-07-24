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
    public interface IMsBrand
    {
        Task<EntitySearchResultBase<MsBrand>> Search(MsBrandSearchCriteria searchCriteria);
        Task<MsBrand> Add(MsBrandModel msBrandModel);
        Task<int> Update(MsBrandModel msBrandModel);
        Task<int> DeleteUpdate(MsBrandModel msBrandModel);
    }
}
