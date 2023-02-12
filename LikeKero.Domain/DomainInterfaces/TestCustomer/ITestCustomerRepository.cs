using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LikeKero.Domain.DomainInterfaces
{
    public interface ITestCustomerRepository : IRepository<TestCustomer>
    {
        Task<int> SaveCustomer(TestCustomer obj);
        Task<TestCustomer> GetTestCustomer(string CustId);
        Task<int> DeleteTestCustomer(string CustId);
        Task<IEnumerable<TestCustomer>> GetAllTestCustomer(TestCustomer obj);
        Task<int> SaveFile(TestCustomer obj);
    }
}
