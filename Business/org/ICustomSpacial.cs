using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.ModelCriteria;

namespace MyFirstAzureWebApp.Business.org
{
    public interface ICustomSpacial
    {
        Task<EntitySearchResultBase<SaleOrgCustom>> SearchSaleOrg(SearchCriteriaBase<SaleOrgCriteria> searchCriteria);
        Task<EntitySearchResultBase<QRCustom>> searchGasolineByCust(QRSearchCriteria searchCriteria);

    }
}
