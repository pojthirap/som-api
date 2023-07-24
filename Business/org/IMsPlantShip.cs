using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Entity.custom;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IMsPlantShip
    {
        Task<EntitySearchResultBase<MsPlantShipCustom>> SearchForPlant(MsPlantShipSearchCriteria searchCriteria);
        Task<EntitySearchResultBase<MsPlantShipCustom>> SearchForShippingPoint(MsPlantShipSearchCriteria searchCriteria);


    }
}
