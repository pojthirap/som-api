using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.SearchCriteria
{
    public class SearchResultOutboundBas<T>
    {
        public Header Header { get; set; }
        public List<T> Data { get; set; }
    }
    public class Header
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Interface_ID { get; set; }
        public string Start_Date { get; set; }
        public string Start_Time { get; set; }
        public string End_Date { get; set; }
        public string End_Time { get; set; }
    }
}
