using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class GetAddressForBestRouteCustom
    {

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string AccName { get; set; }

    }
}
