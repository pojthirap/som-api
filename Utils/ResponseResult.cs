using MyFirstAzureWebApp.exception;
using MyFirstAzureWebApp.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Utils
{
    public class ResponseResult
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        //map
        /*
         * 
         * https://www.tutorialspoint.com/how-to-iterate-any-map-in-chash
         */

        public static  String S_SUCCESS = "S_SUCCESS";
  
        public static  String E_EXCEPTION = "E_EXCEPTION";

        public static String MAPM_ESSAGE_EXCEPTION = "E_EXCEPTION_MAPM_ESSAGE_EXCEPTION";

        public static String appName = "PTG-Service";

        private static String paramSplitValue = "$P$";
        private static String paramSymbolSplitValue = paramSplitValue[0].ToString();// $
        private static String paramValue = paramSplitValue[1].ToString();// P

        private static String SERVER_EXCEPTION_MESSAGE = "System error. Please contact the administrator.";




        public String errorSource { get; set; }

        public String errorCode { get; set; }

        public String errorMessage { get; set; }

        public List<String> errorParams { get; set; }

        public Object data { get; set; }

        public String agent { get; set; }


        public static  ResponseResult warp<T>( T suppiler)
        {
            Exception throwable = null;
            try
            {
                T data = suppiler;
                return success(data);
            }
            catch (Exception throwable1)
            {
                throwable = new Exception("wrap:Exception while calling suppiler", throwable1);
                return error(throwable1);
            }
            return new ResponseResult();
        }

        public static ResponseResult warpError(Exception ex)
        {
            Exception throwable = null;
            try
            {
                return error(ex);
            }
            catch (Exception throwable1)
            {
                throwable = new Exception("wrap:Exception while calling suppiler", throwable1);
                return error(throwable1);
            }
            return new ResponseResult();
        }



        public static ResponseResult success(Object data)  {
            ResponseResult jsonResult = new ResponseResult();
            jsonResult.errorCode = "S_SUCCESS";
            jsonResult.data = data;
             return jsonResult;
        }


        public static ResponseResult error(String errorCode, String errorMessage)
        {
            ResponseResult jsonResult = new ResponseResult();
            jsonResult.errorCode = errorCode;
            jsonResult.errorMessage = errorMessage;
            return jsonResult;
        }

        public static ResponseResult error(String errorCode, String errorMessage, List<String> errorParams)
        {
            ResponseResult jsonResult = new ResponseResult();
            jsonResult.errorCode = errorCode;
            jsonResult.errorMessage = errorMessage;
            jsonResult.errorParams = errorParams;
            return jsonResult;
        }


        public static ResponseResult error(Exception ex)
        {
            log.Debug("Exception:" + ex.Message + ":" + ex.ToString());
            Console.WriteLine("Exception:" + ex.Message + ":" + ex.ToString());
            ResponseResult jsonResult = new ResponseResult();
            if (ex is CustomException) {
                CustomException customException = (CustomException)ex;
                jsonResult.errorSource = customException.errorSource;
                jsonResult.errorCode = customException.errorCode;
                jsonResult.errorParams = customException.errorParams;
                try
                {
                    jsonResult.errorMessage = getErrorMessage(customException);// customException.errorMessage;
                }catch(Exception e)
                {
                    jsonResult.errorCode = MAPM_ESSAGE_EXCEPTION;
                    jsonResult.errorMessage=e.Message+":"+e.ToString();
                    jsonResult.errorSource = e.Source;
                    jsonResult.errorParams = null;
                    return jsonResult;
                }
            } else
            {
                jsonResult.errorSource = appName;
                jsonResult.errorCode = "E_EXCEPTION";
                jsonResult.errorMessage = SERVER_EXCEPTION_MESSAGE + " "+ Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm:ss:ffff", "7");
                //jsonResult.errorMessage = ex.Source + ": " + ex.Message + ":" + ex.ToString();
            }
            return jsonResult;
        }


        private static String getErrorMessage(CustomException ex)
        {
            try{
                Properties.Resource.Culture = ex.culture;
                String message_ = Resources.gePropertiesResource(ex.errorCode);
                if(message_.IndexOf(paramSplitValue) == -1)
                {
                    return message_;
                }
                List<String> listName = ex.errorParams;
                List<String> listValue = new List<String>(listName.Count);
                for(int i = 0; i < listName.Count; i++)
                {
                    String val_ = "";
                    try
                    {
                        String strName = listName.ElementAt(i);
                        if (strName.IndexOf(",") == -1)
                        {
                            val_ = Resources.gePropertiesResource(strName);
                            if (String.IsNullOrEmpty(val_))
                            {
                                val_ = strName;
                            }
                        }
                        else
                        {
                            String[] arrStrName = strName.Split(",");
                            String[] arrStrValue = new string[arrStrName.Length];
                            for(int j = 0; j < arrStrName.Length; j++)
                            {
                                arrStrValue[j] = Resources.gePropertiesResource(arrStrName[j]);
                                if (String.IsNullOrEmpty(arrStrValue[j]))
                                {
                                    arrStrValue[j] = arrStrName[j];
                                }
                            }
                            val_ = String.Join(", ", arrStrValue);
                        }
                    }
                    catch(Exception e)
                    {
                        throw;
                        //throw new NullReferenceException("Message Mapping Exception !!!!!:get Propertie Resource");
                    }
                    listValue.Add(val_);
                }

                String[] msgArr = message_.Split(paramSymbolSplitValue);
                int index = 0; ;
                for(int i = 0; i < msgArr.Length; i++)
                {
                    if (paramValue.Equals(msgArr[i]))
                    {
                        try
                        {
                            msgArr[i] = listValue.ElementAt(index++);
                        }
                        catch(Exception e)
                        {
                            throw;
                            //throw new NullReferenceException("Message Mapping Exception !!!!!:Map Propertie Resource to Message");
                        }
                    }
                }

                message_ = String.Join("", msgArr);
                return message_;

            }
            catch(Exception e)
            {
                throw;
                //throw new NullReferenceException("Message Mapping Exception !!!!!");
            }
        }
        

    }



}
