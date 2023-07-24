using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.common
{
    public static class CommonConstant
    {

        public static string API_OutboundLoginLdap = "https://ptldap.pt.co.th/PTWebservice/PTWebservice.asmx";
        public static string API_OutboundPlantInformation =  "ZSOMI008_OutboundPlantInformation/";
        public static string API_OutboundPlantToShippingPointsInformation =  "ZSOMI010_OutboundPlantToShippingPointsInformation/";
        public static string API_OutboundSaveSaleOrder =  "ZSOMI013_OutboundQuotationInformation/";
        public static string API_OutboundSAP_ZSOMI014 =  "ZSOMI014_InboundSaveSaleOrder/";
        public static string API_OutboundSAP_ZSOMI015 =  "ZSOMI015_InboundSimulateSaleOrder/";
        public static string API_OutboundSAP_ZSOMI021 =  "ZSOMI021_InboundChangeSaleOrder/";
        public static string API_OutboundSearchSaleOrderDocFlow =  "ZSOMI020_OutboundSaleOrderDocumentFlow/";
        public static string API_OutboundCancelSaleOrder =  "ZSOMI022_InboundCancelSaleOrder/";
        public static string API_OutboundChangeSaleOrder = "ZSOMI021_InboundChangeSaleOrder/";

        public static string USER_AGEN = "PTG-Service SlaeOnMobile";
        public static string API_TIMEOUT = "30";
        public static string ReqKey = "req-key";

        public static string IMG_CONTAINER_SOM_RECORD_APP_FORM_FILE = "som-record-app-form-file";
        public static string IMG_CONTAINER_SOM_RECORD_SA_FORM_FILE = "som-record-sa-form-file";
        public static string IMG_CONTAINER_SOM_RECORD_METER_FILE = "som-record-meter-file";
        public static string IMG_CONTAINER_SOM_IMPORT_ERROR_FILE = "som-import-error-file"; 

        public static string GET_File_CONTROLLER = "/getFile/";
        public static string IMPORT_FILE_TMP = "/importFile_tmp/";

        //Report
        public static string API_OutboundSaleOrderInformation =  "ZSOMI027_OutboundSaleOrderInformation/";
        
        
        public static string COOKIE_TOKEN_KEY = "Hey-Cookie";
        public static string[] ORIGINS = { "http://localhost:3000", "http://192.168.68.128:3000", "http://192.168.1.200:3000", "https://ptg-master-git-p-peeraponnaja.vercel.app", "https://ptg-master-git-fortest-peeraponnaja.vercel.app", "https://ptg-master.vercel.app", "https://ptg-sale.vercel.app", "https://dev-saleonmobile-frontend-asv.azurewebsites.net", "https://dev-saleonmobile-backend-asv.azurewebsites.net", "http://192.168.1.38:3000"
        , "https://uat-saleonmobile-frontend-asv.azurewebsites.net", "https://uat-saleonmobile-backend-asv.azurewebsites.net", "https://prd-saleonmobile-frontend-asv.azurewebsites.net", "https://prd-saleonmobile-backend-asv.azurewebsites.net"};

        // Version
        public static string VERSION = "2.0.8";
    }
}
