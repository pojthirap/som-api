﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models.ms
{
    public class TemplateAppFormModel : ModelBase
    {

        public string TpAppFormId { get; set; }
        public string TpCateId { get; set; }
        public string TpCode { get; set; }
        public string TpNameTh { get; set; }
        public string TpNameEn { get; set; }
        public string PublicFlag { get; set; }
        public string ActiveFlag { get; set; }

    }
}
