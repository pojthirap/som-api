using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models.adm;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IAdmSession
    {
        Task<EntitySearchResultBase<AdmSession>> Search(SearchCriteriaBase<AdmSessionCriteria> searchCriteria);
        Task<EntitySearchResultBase<SearchAdmSessionForGetSessionCustom>> SearchAdmSessionForGetSession(SearchCriteriaBase<AdmSessionCriteria> searchCriteria);
        Task<AdmSession> Add(AdmSessionModel admSessionModel);
        Task<int> Update(AdmSessionModel admSessionModel);
        Task<int> Logout(AdmSessionModel admSessionModel);
    }
}
