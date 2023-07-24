using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NLog;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Utils;
using MyFirstAzureWebApp.Models.ms;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using MyFirstAzureWebApp.common;

namespace MyFirstAzureWebApp.Business.org
{

    public class CallOutbround
    {
        private Logger log = LogManager.GetCurrentClassLogger();

        public static async Task<T> callAPI<T>(T t,string jsonVal, string baseUrl, string fullUrl)
        {
            Object object_Response = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", CommonConstant.USER_AGEN);
                client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(CommonConstant.API_TIMEOUT));
                string fullPath = fullUrl;
                //var jsonVal = JsonConvert.SerializeObject(new Object());
                var content = new StringContent(jsonVal, Encoding.UTF8, "application/json");
                Console.WriteLine("=========== jsonVal ================");
                Console.WriteLine(jsonVal);
                Console.WriteLine(JObject.Parse(jsonVal));


                HttpResponseMessage response = await client.PostAsync(fullPath, content);
                object_Response = new Object();
                if (response.IsSuccessStatusCode)
                {
                    object_Response = await response.Content.ReadAsAsync<Object>();
                }
                else
                {
                    string result_error = response.Content.ReadAsStringAsync().Result;
                    Exception ex = new Exception(result_error);
                    throw ex;
                }

                return (T)object_Response;
            }
        }




    }
}
