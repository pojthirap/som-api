using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Utils
{
    public class ObjectFacory
    {
        public static CultureInfo langTH = new CultureInfo("th-TH");
        public static CultureInfo langEN = new CultureInfo("en-US");

        public static CultureInfo getCultureInfo(String lang)
        {
            if (lang == null)
                return langTH;

            return "en".Equals(lang.ToLower()) ? langEN : langTH;
       }
    }
}
