using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Models
{
    public class Settings
    {
        public string AppName { get; set; }
        public double Version { get; set; }
        public string Language { get; set; }
        public string Messages { get; set; }
        public string ConnectionString { get; set; }
    }
}
