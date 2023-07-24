using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.enumval
{
    enum PlanTripStatus
    {
        WAITING_FOR_APPROVE=2,
        APPROVE = 3,
        REJECT = 4,
        CANCEL = 5,
        MERGE =6,
        ASSIGN = 7,
        COMPLETED = 8,
        INCOMPLETED = 9,
    }
}
