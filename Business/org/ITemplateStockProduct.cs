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
    public interface ITemplateStockProduct
    {
        Task<EntitySearchResultBase<SearchTemplateByStockCardCustom>> searchTemplateByStockCardId(SearchCriteriaBase<TemplateStockProductCriteria> searchCriteria);
        //Task<EntitySearchResultBase<TemplateStockProduct>> Search(SearchCriteriaBase<TemplateStockProductCriteria> searchCriteria);
        Task<List<TemplateStockProduct>> addTemplateStockProduct(TemplateStockProductModel templateStockProductModel);
        //Task<TemplateStockProduct> Update(TemplateStockProductModel templateStockProductModel);
        Task<int> DeleteUpdate(TemplateStockProductModel templateStockProductModel);
        Task<int> delTemplateStockProduct(TemplateStockProductModel templateStockProductModel);
    }
}
