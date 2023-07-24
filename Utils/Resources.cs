using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Utils
{
    public class Resources
    {
        public static String gePropertiesResource(String name)
        {
            return Properties.Resource.ResourceManager.GetString(name, Properties.Resource.Culture);
        }
    }
}
