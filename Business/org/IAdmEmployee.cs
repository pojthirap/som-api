using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.adm;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models.profile;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IAdmEmployee
    {
        Task<EntitySearchResultBase<SearchAdmEmpCustom>> searchAdmEmp(AdmEmployeeSearchCriteria searchCriteria);
        Task<EntitySearchResultBase<AdmEmployee>> getEmpSupvisor(SearchCriteriaBase<AdmEmployeeCriteria> searchCriteria);
        Task<EntitySearchResultBase<AdmEmployee>> SearchSaleRep(AdmEmployeeSearchCriteria searchCriteria);
        Task<int> UpdateSaleGroupToSaleRep(AdmEmployeeModel admEmployeeModel);
        Task<int> DeleteSaleGroupWithOutSaleRep(AdmEmployeeModel admEmployeeModel);
        Task<int> UpdateApproverToSaleRep(AdmEmployeeModel admEmployeeModel);
        Task<EntitySearchResultBase<AdmEmployeeAdmGroupCustom>> searchAdmEmpForMapRole(AdmEmployeeSearchCriteria searchCriteria);
        Task<EntitySearchResultBase<AdmEmployee>> searchAdmEmpRoleManager(AdmEmployeeSearchCriteria searchCriteria, UserProfileForBack userProfileForBack);

        Task<EntitySearchResultBase<UserProfileForBackEndCustom>> getUserProfileForBackEnd(SearchCriteriaBase<AdmEmployeeCriteria> searchCriteria);
        Task<EntitySearchResultBase<SaleGroupSaleOfficeForBackEndCustom>> getSaleGroupSaleOfficeForBackEnd(SearchCriteriaBase<AdmEmployeeCriteria> searchCriteria);
        Task<EntitySearchResultBase<OrgSaleArea>> getSaleAreaForBackEnd(SearchCriteriaBase<OrgSaleOffice> searchCriteria);
        Task<EntitySearchResultBase<OrgTerritory>> getTerritoryForBackEnd(SearchCriteriaBase<AdmEmployeeCriteria> searchCriteria);
        Task<EntitySearchResultBase<UserProfileForBackEndCustom>> getUserProfileForBackEndByToken(SearchCriteriaBase<GetSessioinCriteria> searchCriter);
        Task<EntitySearchResultBase<AdmEmployee>> getEmployeeByEmpId(SearchCriteriaBase<GetAdmEmployeeByEmpIdCriteria> searchCriteria);
        Task<EntitySearchResultBase<SearchAdmEmpCustom>> searchAdmEmpForReport(SearchCriteriaBase<SearchAdmEmpForReportCriteria> searchCriteria);
        Task<List<PermObjCodeCustom>> searchPermObjCode(string userName);
        Task<List<GetValidRoleCustom>> searchCountPermObject(string api_service_name, UserProfileForBack userProfile);
    }
}
