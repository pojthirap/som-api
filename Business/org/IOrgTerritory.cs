using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.adm;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models.profile;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IOrgTerritory
    {
        Task<EntitySearchResultBase<OrgTerritory>> Search(OrgTerritorySearchCriteria searchCriteria);
        Task<OrgTerritory> Add(OrgTerritoryModel model);
        Task<int> Update(OrgTerritoryModel model);
        Task<int> DeleteUate(OrgTerritoryModel model);
        Task<EntitySearchResultBase<SearchInitialSaleRepCustom>> SearchInitialSaleRep(OrgSaleTerritorySearchCriteria searchCriteria);
        Task<EntitySearchResultBase<OrgSearchRepCustom>> SearchRep(SearchCriteriaBase<OrgSearchRepCriteria> searchCriteria);
        Task<List<OrgSaleTerritory>> MapSaleRep(OrgMapSaleRepModel model);
        Task<int> DellMapSaleRep(OrgSaleTerritoryModel model, UserProfileForBack userProfile);
        Task<int> updTerritoryByManagerEmpId(OrgTerritoryModel model);



        Task<EntitySearchResultBase<SearchSalesTerritoryTabCustom>> searchSalesTerritoryTab(SearchCriteriaBase<ProspectCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<EntitySearchResultBase<OrgTerritory>> getTerritoryForDedicated(SearchCriteriaBase<GetTerritoryForDedicatedCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<EntitySearchResultBase<SearchSaleGroupForMapSaleTerritoryCustom>> searchSaleGroupForMapSaleTerritory(SearchCriteriaBase<SearchSaleGroupForMapSaleTerritoryCriteria> searchCriteria);
    }
}
