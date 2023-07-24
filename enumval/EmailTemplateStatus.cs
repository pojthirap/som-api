using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.enumval
{
    enum EmailTemplateStatus
    {
        WAITING_FOR_APPROVE = 1,
        ASSIGN = 2,
        APPROVE = 3,
        REJECT = 4,
        WAITING_FOR_APPROVE_ALERT = 5,
    }
}
