using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class GetEmailToSendForApprovePlanTripCustom
    {

        public string SenderEmail { get; set; }
        public string ReceiveEmail { get; set; }
        public string PlanTripName { get; set; }
        public DateTime? PlanTripDate { get; set; }
    }
}
