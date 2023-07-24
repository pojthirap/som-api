using MyFirstAzureWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.exception
{
    public class CustomException : Exception
    {

        public static  String S_SUCCESS = "S_SUCCESS";
  
        public static  String E_ERROR = "E_ERROR";
  
        public static  String E_EXCEPTION = "E_EXCEPTION";

        public static String appName = "PTG-Service";

        public String errorSource { get; set; }

        public String errorCode { get; set; }

        public String errorMessage { get; set; }

        public CultureInfo culture { get; set; }

        public List<String> errorParams { get; set; }
        
        protected CustomException(String errorSource, String errorCode, List<String> errorParams, CultureInfo culture)
        {
            this.errorSource = (errorSource == null ? appName : errorSource);
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
            this.errorParams = errorParams;
            this.culture = culture;
        }

        protected CustomException(String errorSource, String errorCode, CultureInfo culture)
        {
            this.errorSource = (errorSource == null ? appName : errorSource);
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
            this.errorParams = errorParams;
            this.culture = culture;
        }

        protected CustomException(String errorCode, List<String> errorParams, CultureInfo culture)
        {
            this.errorSource = (errorSource == null ? appName : errorSource);
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
            this.errorParams = errorParams;
            this.culture = culture;
        }

        protected CustomException(String errorCode, CultureInfo culture)
        {
            string strMsg = Resources.gePropertiesResource(errorCode);
            this.errorSource = (errorSource == null ? appName : errorSource);
            this.errorCode = errorCode;
            this.errorMessage = strMsg;
            this.errorParams = errorParams;
            this.culture = culture;
        }


    }
}
