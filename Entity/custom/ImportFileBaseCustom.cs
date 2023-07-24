using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class ImportFileBaseCustom
    {

        public string TotalRecord { get; set; }
        public string TotalSuccess { get; set; }
        public string TotalFailed { get; set; }
        public string PathFileError { get; set; }
    }
}
