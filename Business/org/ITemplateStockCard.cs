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
    public interface ITemplateStockCard
    {
        Task<EntitySearchResultBase<TemplateStockCard>> Search(SearchCriteriaBase<TemplateStockCardCriteria> searchCriteria);
        Task<TemplateStockCard> Add(TemplateStockCardModel templateStockCardModel);
        Task<int> Update(TemplateStockCardModel templateStockCardModel);
        Task<int> DeleteUpdate(TemplateStockCardModel templateStockCardModel);
        Task<EntitySearchResultBase<GetTaskStockCardForRecordCustom.Product>> getTaskStockCardForRecord(SearchCriteriaBase<GetTaskStockCardForRecordCriteria> searchCriteria);

    }
}
