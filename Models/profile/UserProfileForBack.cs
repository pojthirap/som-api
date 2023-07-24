using Entity;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.profile
{
    public class UserProfileForBack
    {

        public EntitySearchResultBase<UserProfileForBackEndCustom> UserProfileCustom { get; set; }
        public EntitySearchResultBase<SaleGroupSaleOfficeForBackEndCustom> SaleGroupSaleOfficeCustom { get; set; }
        public EntitySearchResultBase<OrgSaleArea> OrgSaleArea { get; set; }
        public EntitySearchResultBase<OrgTerritory> OrgTerritory { get; set; }
        public OrgTerritory OrgTerritoryObject { get; set; }
        public List<PermObjCodeCustom> listPermObjCode { get; set; }


        public string getUserName()
        {
            return this.UserProfileCustom.data[0].EmpId;
        }

        public string getEmpId()
        {
            return this.UserProfileCustom.data[0].EmpId;
        }

        public decimal? getBuId()
        {
            return this.UserProfileCustom.data[0].BuId;
        }

        public string getAdmEmployeeGroupCode()
        {
            return this.UserProfileCustom.data[0].GroupCode;
        }
        public UserProfileForBackEndCustom getUserData()
        {
            return this.UserProfileCustom.data[0];
        }


        public SaleGroupSaleOfficeForBackEndCustom getSaleGroupSaleOffice()
        {
            return this.SaleGroupSaleOfficeCustom.data[0];
        }
    }
}
