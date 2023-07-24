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

namespace MyFirstAzureWebApp.Business.org
{
    public interface IMsProduct
    {
        Task<EntitySearchResultBase<SearchProductCustomCustom>> searchProduct(SearchCriteriaBase<MsProductCriteria> searchCriteria);
        Task<int> updateReportProductConversion(UpdateReportProductConversionModel updateReportProductConversionModel);
        Task<EntitySearchResultBase<SearchProductByPlantCodeCustom>> searchProductByPlantCode(SearchCriteriaBase<SearchProductByPlantCodeCriteria> searchCriteria);
        Task<EntitySearchResultBase<SearchProductConversionByProductCodeCustom>> SearchProductConversionByProductCode(SearchCriteriaBase<SearchProductConversionByProductCodeCriteria> searchCriteria);
        Task<EntitySearchResultBase<MsProduct>> searchProductByCustSaleId(SearchCriteriaBase<SearchProductByCustSaleIdCriteria> searchCriteria);


    }
}
