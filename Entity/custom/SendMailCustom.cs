using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SendMailCustom
    {

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }

    }
}
