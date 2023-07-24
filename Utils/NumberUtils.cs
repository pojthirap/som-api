using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Utils
{
    public class NumberUtils
    {

        public static bool isNumber(string str)
        {
            long long_ = 0;
            bool canConvert = long.TryParse(str, out long_); 
            if (canConvert)
                return true;
            return false;
        }

        public static bool isDecimal(string str)
        {
            decimal decimal_ = 0;
            bool canConvert = decimal.TryParse(str, out decimal_);
            if (canConvert)
                return true;
            return false;
        }
    }
}
