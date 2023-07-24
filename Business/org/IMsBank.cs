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
    public interface IMsBank
    {
        Task<EntitySearchResultBase<MsBank>> Search(SearchCriteriaBase<MsBankCriteria> searchCriteria);
        Task<MsBank> Add(MsBankModel msBankModel);
        Task<int> Update(MsBankModel msBankModel);
        Task<int> DeleteUpdate(MsBankModel msBankModel);
    }
}
