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
using MyFirstAzureWebApp.ModelCriteria.inbound.changeSaleOrder;
using MyFirstAzureWebApp.ModelCriteria.inbound.cancelSaleOrder;

namespace MyFirstAzureWebApp.Business.org
{
    public interface ISaleOrder
    {
        Task<EntitySearchResultBase<SearchSaleOrderTabCustom>> searchSaleOrderTab(SearchCriteriaBase<SearchSaleOrderTabCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<EntitySearchResultBase<SearchSaleOrderCustom>> searchSaleOrder(SearchCriteriaBase<SearchSaleOrderCriteria> searchCriteria, UserProfileForBack userProfile);
        Task<EntitySearchResultBase<GetSaleOrderByOrderIdCustom>> getSaleOrderByOrderId(SearchCriteriaBase<GetSaleOrderByOrderIdCriteria> searchCriteria);
        Task<InboundChangeSaleOrderModelResponse> changeSaleOrder(InboundChangeSaleOrderModel model);
        Task<InboundCancelSaleOrderModelResponse> cancelSaleOrder(InboundCancelSaleOrderModel model);


    }
}
