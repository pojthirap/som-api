using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Business.People
{
    public interface IEmployees
    {
        Task<int> New(Employee employee);
        Task<int> Resigns(Employee employee);
        Task<int> UpdateAddress(Employee employee);
    }
}
