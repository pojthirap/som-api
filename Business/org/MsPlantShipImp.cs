using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NLog;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Utils;

namespace MyFirstAzureWebApp.Business.org
{

    public class MsPlantShipImp : IMsPlantShip
    {
        private Logger log = LogManager.GetCurrentClassLogger();
        public async Task<EntitySearchResultBase<MsPlantShipCustom>> SearchForPlant(MsPlantShipSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                MsPlantShipCriteria criteria = searchCriteria.model;
                var queryCommane = (from ps in context.MsPlantShip
                                    join p in context.MsPlant
                                 on ps.PlantCode equals p.PlantCode
                                 join s in context.MsShip
                                 on ps.ShipCode equals s.ShipCode
                             

                             where ((criteria.plantNameTh == null ? 1 == 1 : p.PlantNameTh.Contains(criteria.plantNameTh)))
                             where ((criteria.descriptionTh == null ? 1 == 1 : s.DescriptionTh.Contains(criteria.descriptionTh)))
                                    orderby (searchCriteria.searchOrder == 0 ? ps.PlantCode : p.PlantNameTh)
                                    select new
                             {
                                        PlantShipId = ps.PlantShipId,
                                        PlantCode = ps.PlantCode,
                                        ShipCode = ps.ShipCode,


                                        ActiveFlag = ps.ActiveFlag,
                                        CreateUser = ps.CreateUser,
                                        CreateDtm = ps.CreateDtm,
                                        UpdateUser = ps.UpdateUser,
                                        UpdateDtm = ps.UpdateDtm,

                                        PlantNameTh = p.PlantNameTh,
                                        PlantNameEn = p.PlantNameEn,
                                        PlantVendorNo = p.PlantVendorNo,
                                        PlantCustNo = p.PlantCustNo,
                                        PurchOrg = p.PurchOrg,
                                        FactCalendar = p.FactCalendar,
                                        BussPlace = p.BussPlace,
                                        City = p.City,

                                        DescriptionTh = s.DescriptionTh,
                                        DescriptionEn = s.DescriptionEn
                                    });
                //}).ToList();
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<MsPlantShipCustom> searchResult = new EntitySearchResultBase<MsPlantShipCustom>();
                searchResult.totalRecords = query.Count();

                List<MsPlantShipCustom> saleLst = new List<MsPlantShipCustom>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                    {
                    MsPlantShipCustom s = new MsPlantShipCustom();
                    s.PlantShipId = item.PlantShipId;
                    s.PlantCode = item.PlantCode;
                    s.ShipCode = item.ShipCode;
                    s.ActiveFlag = item.ActiveFlag;
                    s.createUser = item.CreateUser;
                    s.createDtm = item.CreateDtm;
                    s.updateUser = item.UpdateUser;
                    s.updateDtm = item.UpdateDtm;

                    s.PlantNameTh = item.PlantNameTh;
                    s.PlantNameEn = item.PlantNameEn;
                    s.PlantVendorNo = item.PlantVendorNo;
                    s.PlantCustNo = item.PlantCustNo;
                    s.PurchOrg = item.PurchOrg;
                    s.FactCalendar = item.FactCalendar;
                    s.BussPlace = item.BussPlace;
                    s.City = item.City;


                    s.DescriptionTh = item.DescriptionTh;
                    s.DescriptionEn = item.DescriptionEn;
                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;
               
            }

        }



        public async Task<EntitySearchResultBase<MsPlantShipCustom>> SearchForShippingPoint(MsPlantShipSearchCriteria searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                MsPlantShipCriteria criteria = searchCriteria.model;
                var queryCommane = (from ps in context.MsPlantShip
                                    join p in context.MsPlant
                                 on ps.PlantCode equals p.PlantCode
                                    join s in context.MsShip
                                    on ps.ShipCode equals s.ShipCode


                                    where ((criteria.plantNameTh == null ? 1 == 1 : p.PlantNameTh == criteria.plantNameTh))
                                    where ((criteria.descriptionTh == null ? 1 == 1 : s.DescriptionTh == criteria.descriptionTh))
                                    orderby (searchCriteria.searchOrder == 0 ? ps.PlantCode : p.PlantNameTh)
                                    select new
                                    {
                                        PlantShipId = ps.PlantShipId,
                                        PlantCode = ps.PlantCode,
                                        ShipCode = ps.ShipCode,


                                        ActiveFlag = ps.ActiveFlag,
                                        CreateUser = ps.CreateUser,
                                        CreateDtm = ps.CreateDtm,
                                        UpdateUser = ps.UpdateUser,
                                        UpdateDtm = ps.UpdateDtm,

                                        PlantNameTh = p.PlantNameTh,
                                        PlantNameEn = p.PlantNameEn,
                                        PlantVendorNo = p.PlantVendorNo,
                                        PlantCustNo = p.PlantCustNo,
                                        PurchOrg = p.PurchOrg,
                                        FactCalendar = p.FactCalendar,
                                        BussPlace = p.BussPlace,

                                        DescriptionTh = s.DescriptionTh,
                                        DescriptionEn = s.DescriptionEn
                                    });
                //}).ToList();
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<MsPlantShipCustom> searchResult = new EntitySearchResultBase<MsPlantShipCustom>();
                searchResult.totalRecords = query.Count();

                List<MsPlantShipCustom> saleLst = new List<MsPlantShipCustom>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    MsPlantShipCustom s = new MsPlantShipCustom();
                    s.PlantShipId = item.PlantShipId;
                    s.PlantCode = item.PlantCode;
                    s.ShipCode = item.ShipCode;
                    s.ActiveFlag = item.ActiveFlag;
                    s.createUser = item.CreateUser;
                    s.createDtm = item.CreateDtm;
                    s.updateUser = item.UpdateUser;
                    s.updateDtm = item.UpdateDtm;

                    s.PlantNameTh = item.PlantNameTh;
                    s.PlantNameEn = item.PlantNameEn;
                    s.PlantVendorNo = item.PlantVendorNo;
                    s.PlantCustNo = item.PlantCustNo;
                    s.PurchOrg = item.PurchOrg;
                    s.FactCalendar = item.FactCalendar;
                    s.BussPlace = item.BussPlace;


                    s.DescriptionTh = item.DescriptionTh;
                    s.DescriptionEn = item.DescriptionEn;
                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;

            }

        }



    }
}
