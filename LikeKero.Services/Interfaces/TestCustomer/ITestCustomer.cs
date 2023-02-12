using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using LikeKero.Domain;
namespace LikeKero.Services.Interfaces
{
    public interface ITestCustomer : IServiceBase<TestCustomer>
    {
        Task<string> SaveCustomer(TestCustomer obj);
        Task<TestCustomer> GetTestCustomer(string CustId);
        Task<string> DeleteTestCustomer(string CustId);
        Task<string> SaveFile(string CustId, IFormFile formFile);


    }
}
