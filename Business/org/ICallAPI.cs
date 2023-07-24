using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.SearchCriteria.type3;
using MyFirstAzureWebApp.Controllers;
using MyFirstAzureWebApp.SearchCriteria.type2;

namespace MyFirstAzureWebApp.Business.org
{
    public interface ICallAPI
    {
        Task<EntitySearchResultBase<ZResultCustom>> Search(SearchCriteriaBase<SearchPlantByCompanyCodeCriteria> searchCriteria);
        Task<EntitySearchResultBase<SearchPlantByCompanyCodeCustom>> searchPlantByCompanyCode(SearchCriteriaBase<SearchPlantByCompanyCodeCriteria> searchCriteria, InterfaceSapConfig ic);
        Task<EntitySearchResultBase<SearchShippingPointByPlantCodeCustom>> searchShippingPointByPlantCode(SearchCriteriaBase<SearchShippingPointByPlantCodeCriteria> searchCriteria, InterfaceSapConfig ic);
        Task<EntitySearchResultBase<SaleOrder>> createSaleOrderByQuotationNo(CreateSaleOrderByQuotationNoCriteria searchCriteria, UserProfileForBack userProfile, string language, InterfaceSapConfig ic);
        Task<StatusType2> updSaleOrder(UpdSaleOrderCriteria searchCriteria, UserProfileForBack userProfile, string language, InterfaceSapConfig ic, int timeZone);
        Task<EntitySearchResultBase<ItemType3>> searchSaleOrderDocFlow(SearchCriteriaBase<SearchSaleOrderDocFlowCriteria> searchCriteria, string language, InterfaceSapConfig ic);
        Task<LoginLdapResult> LoginLdap(LoginLdapCriteria searchCriteria);
        Task<StatusType2> delSaleOrder(UpdSaleOrderCriteria searchCriteria, UserProfileForBack userProfile, string language, InterfaceSapConfig ic, int timeZone);

    }
}
