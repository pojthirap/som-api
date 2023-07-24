using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.adm
{
    public class AddAdmGroupAppModel
    {
        public string GroupId { get; set; }
        public string AppId { get; set; }
        public string EffectiveDate { get; set; }
        public string ExpiryDate { get; set; }
    }
}
