using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.exception
{
    public class ServiceException : CustomException
    {

        public static List<String> setErrorParam(String[] param)
        {
            List<String> errorParam = new List<string>();
            for(int i = 0; i < param.Length; i++)
            {
                errorParam.Add(param[i]);
            }
            return errorParam;
        }

        public ServiceException(String errorSource, String errorCode, List<String> errorParams, CultureInfo culture)
        : base(errorSource, errorCode, errorParams, culture)
        {
        }


        public ServiceException(String errorSource, String errorCode, CultureInfo culture)
        : base(errorSource, errorCode, culture)
        {
        }

        public ServiceException(String errorCode, List<String> errorParams, CultureInfo culture)
        : base(errorCode, errorParams, culture)
        {
        }

        public ServiceException(String errorCode, CultureInfo culture)
        : base(errorCode, culture)
        {
        }


    }

}
