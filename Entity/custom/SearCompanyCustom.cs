﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class SearCompanyCustom
    {

        public string CompanyCode { get; set; }
        public string CompanyNameTh { get; set; }

    }
}
