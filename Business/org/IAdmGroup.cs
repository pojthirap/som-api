using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.adm;
using MyFirstAzureWebApp.ModelCriteria;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IAdmGroup
    {
        Task<EntitySearchResultBase<AdmGroup>> Search(SearchCriteriaBase<AdmGroupCriteria> searchCriteria);
        Task<AdmGroup> Add(AdmGroupModel model);
        Task<int> Update(AdmGroupModel model);
        Task<int> DeleteUate(AdmGroupModel model);

    }
}
