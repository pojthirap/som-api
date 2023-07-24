using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models
{
    public class ModelBase
    {
        public UserProfileForBack UserProfile { get; set; }

        public string getUserName()
        {
            return this.UserProfile.UserProfileCustom.data[0].EmpId;
        }

        public string getEmpId()
        {
            return this.UserProfile.UserProfileCustom.data[0].EmpId;
        }

        public decimal? getBuId()
        {
            return this.UserProfile.UserProfileCustom.data[0].BuId;
        }

        public UserProfileForBackEndCustom getUserData()
        {
            return this.UserProfile.UserProfileCustom.data[0];
        }
    }
}
