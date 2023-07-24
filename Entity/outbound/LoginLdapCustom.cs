using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Entity.custom
{
    [Keyless]
    public class LoginLdapCustom
    {
        public bool success { get; set; }
        public String message { get; set; }
        public LoginLdapUsers users { get; set; }

    }
    public class LoginLdapUsers
    {
        public String username { get; set; }
        public String firstName { get; set; }
        public String lastName { get; set; }
        public String token { get; set; }

    }

    public class LoginLdapResult
    {
        public bool hasEmail { get; set; }
        public bool isSuccessful { get; set; }
        public bool resetPassword { get; set; }
        public String responseMessage { get; set; }
    }
}
