using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Business.People
{
    // see https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio
    // https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/crud?view=aspnetcore-5.0
    public class Employees : IEmployees
    {
        public async Task<int> New(Employee employee)
        {
            using (var db = new MyAppContext())
            {
                //Add New
                db.Add(employee);
                //Add Address

                //Add Contact person etc

                //Save
                return await db.SaveChangesAsync();
            }
        }
        public async Task<int> Resigns(Employee employee)
        {
            using (var db = new MyAppContext())
            {
                //Delete an employee etc
                db.Remove(employee);
                //Delete Address

                //Delete Contact person etc

                //Save
                return await db.SaveChangesAsync();
            }
        }

        public Task<int> UpdateAddress(Employee employee)
        {
            throw new NotImplementedException();
        }        
    }
}
