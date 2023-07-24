using Microsoft.IdentityModel.Tokens;
using MyFirstAzureWebApp.Business.org;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.ModelCriteria;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static MyFirstAzureWebApp.Authentication.JWTAuthenticationManager;

namespace MyFirstAzureWebApp.Authentication
{
    public interface IJWTAuthenticationManager
    {
        Task<AuthenResult> Authenticate(string username, string password, string tokenExprie, bool skipAuthen);
    }
    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {
        IDictionary<string, string> users = new Dictionary<string, string>
        {
            { "test", "test" },
            { "10000300", "10000300" },
            { "10000306", "10000306" },
            { "10000307", "10000307" },
            { "10000308", "10000308" },
            { "10000312", "10000312" }

            ,
            { "10000163", "10000163" },
            { "10000600", "10000600" },
            { "10000011", "10000011" },
            { "10000155", "10000155" }

            ,
            { "10000044", "10000044" },
            { "10000048", "10000048" },
            { "10000063", "10000063" },
            { "10000071", "10000071" },
            { "10000074", "10000074" },
            { "10000076", "10000076" },
            { "10000097", "10000097" },
            { "10000101", "10000101" },
            { "10000112", "10000112" },
            { "10000113", "10000113" },

            { "10000150", "10000150" },
            { "10000192", "10000192" },
            { "10000245", "10000245" },

            { "541219", "541219" },
            { "599817", "599817" },
            { "596635", "596635" },
            { "596425", "596425" },
            { "63016393", "63016393" },
            { "10008042", "10008042" },
            { "10010935", "10010935" },
            { "62020719", "62020719" },
            { "000282", "000282" },
            { "186123", "186123" },
            { "001776", "001776" },
            { "583107", "583107" },
            { "172213", "172213" },
            { "62015169", "62015169" },
            { "559437", "559437" },
            { "593512", "593512" },
            { "161737", "161737" },
            { "569072", "569072" },
            { "559091", "559091" },
            { "186026", "186026" },
            { "598633", "598633" },
            { "62013255", "62013255" },
            { "10001631", "10001631" },
            { "604937", "604937" },
            { "10000602", "10000602" },
            { "62016120", "62016120" },
            { "161253", "161253" },
            { "63001672", "63001672" },
            { "580838", "580838" },
            { "601198", "601198" },
            { "10002518", "10002518" },
            { "577140", "577140" },
            { "161795", "161795" },
            { "62008170", "62008170" },
            { "559373", "559373" },
            { "000820", "000820" },
            { "000821", "000821" },
            { "62013248", "62013248" },
            { "580441", "580441" },
            { "580442", "580442" },
            { "591322", "591322" },
            { "183075", "183075" },
            { "540147", "540147" },
            { "62020711", "62020711" },
            { "62020717", "62020717" },
            { "576176", "576176" },
            { "62016833", "62016833" },
            { "572389", "572389" },
            { "619317", "619317" },
            { "iwiz011", "Iwiz2021" }




        };

        private readonly string tokenKey;
        private ICallAPI callAPIImp = new CallAPIImp();

        public JWTAuthenticationManager(string tokenKey)
        {
            this.tokenKey = tokenKey;

        }

        public async Task<AuthenResult> Authenticate(string username, string password, string tokenExprie, bool skipAuthen)
        {
            try
            {
                AuthenResult athRes = new AuthenResult();

                if (!skipAuthen)
                {
                    // Your need to put more logic for Authenticate here
                    // This is for foo sample 
                    /*if (!users.Any(u => u.Key == username && u.Value == password))
                    {
                        athRes.ErrorMessage = "Login Fail";
                        athRes.Status = "N";
                        return athRes;//null
                    }*/
                    
                    // Ldap
                    
                    LoginLdapCriteria searchCriteria = new LoginLdapCriteria();
                    searchCriteria.Username = username;
                    searchCriteria.Password = password;
                    LoginLdapResult loginLdapCustom = await callAPIImp.LoginLdap(searchCriteria);
                    if (!loginLdapCustom.isSuccessful)
                    {
                        athRes.ErrorMessage = loginLdapCustom.responseMessage;
                        athRes.Status = "N";
                        return athRes;//null
                    }
                    
                    // Ldap

                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(tokenKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, username)
                    }),
                    //Expires = DateTime.UtcNow.AddMinutes(1),
                    Expires = DateTime.UtcNow.AddSeconds(Convert.ToDouble(tokenExprie)),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                athRes.Status = "Y";
                var token = tokenHandler.CreateToken(tokenDescriptor);
                athRes.Token = tokenHandler.WriteToken(token);
                return athRes;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public class AuthenResult
        {
            public string Token { get; set; }
            public string ErrorMessage{ get; set; }
            public string ErrorCode { get; set; }
            public string Status { get; set; }
            public string Message { get; set; }
        }
    }
}
