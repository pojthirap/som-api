using MyFirstAzureWebApp.Entity.custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models
{
    public class LoginModel
    {
        public string token { get; set; }
        public UserProfileForBackEndCustom userProfileForBackEndCustom { get; set; }
    }
}
