using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.adm
{
    public class UpdAdmGroupAppModel
    {

        public string GroupAppId { get; set; }
        public string GroupId { get; set; }
        public string AppId { get; set; }
        public string EffectiveDate { get; set; }
        public string ExpiryDate { get; set; }
        public string ActiveFlag { get; set; }
    }
}
