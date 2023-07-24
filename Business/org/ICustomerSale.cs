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
using MyFirstAzureWebApp.Models.saleorder;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Controllers;

namespace MyFirstAzureWebApp.Business.org
{
    public interface ICustomerSale
    {
        Task<EntitySearchResultBase<SearchSaleDataTabCustom>> searchSaleDataTab(SearchCriteriaBase<SearchSaleDataTabCriteria> searchCriteria);
        Task<EntitySearchResultBase<SearchShipToByCustSaleIdCustom>> searchShipToByCustSaleId(SearchCriteriaBase<SearchShipToByCustSaleIdCriteria> searchCriteria);
        Task<EntitySearchResultBase<SearchCustomerSaleByCustCodeCustom>> searchCustomerSaleByCustCode(SearchCriteriaBase<SearchCustomerSaleByCustCodeCriteria> searchCriteria);
        Task<SaleOrder> createSaleOrder(CreateSaleOrderModel createSaleOrderModel, UserProfileForBack userProfile);
        Task<int> cancelSaleOrder(CancelSaleOrderModel cancelSaleOrderModel, UserProfileForBack userProfile, string language, InterfaceSapConfig ic);


    }
}
