using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.org;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IOrgSaleOffice
    {
        Task<EntitySearchResultBase<OrgSaleOffice>> Search(OrgSaleOfficeSearchCriteria searchCriteria);
    }
}
