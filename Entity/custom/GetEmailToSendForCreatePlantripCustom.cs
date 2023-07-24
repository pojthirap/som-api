using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class GetEmailToSendForCreatePlantripCustom
    {

        public string SenderEmail { get; set; }
        public string ReceiveEmail { get; set; }
        public string ReceiveEmailAssign { get; set; }
    }
}
