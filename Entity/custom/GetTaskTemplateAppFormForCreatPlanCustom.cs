using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class GetTaskTemplateAppFormForCreatPlanCustom
    {

        public string Code { get; set; }
        public string Description { get; set; }
        public string TaskType { get; set; }

    }
}
