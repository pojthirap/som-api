using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.ModelCriteria;
using static MyFirstAzureWebApp.SearchCriteria.SearchStockCountTabCustom;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.record;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IRecordStockCard
    {
        Task<EntitySearchResultBase<ResponseData>> searchStockCountTab(SearchCriteriaBase<SearchStockCountTabCriteria> searchCriteria);
        Task<List<RecordStockCard>> addRecordStockCard(List<RecordStockCardModel> recordStockCardList, UserProfileForBack userProfileForBack);

    }
}
