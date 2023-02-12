using System;
using LikeKero.Data.Constants;
using LikeKero.Data.Interfaces;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LikeKero.Data.Services
{
    public class TestCustomerRepository : Repository<TestCustomer>, ITestCustomerRepository
    {
        public TestCustomerRepository(IOptions<ReadConfig> connStr, IDapperResolver<TestCustomer> resolver) : base(connStr, resolver)
        {
        }


        public async Task<int> DeleteTestCustomer(string CustId)
        {
            TestCustomer obj = new TestCustomer() { CustID = CustId };
            string[] addParams = new string[] { TestCustomerInfra.CUSTID };
            return await ExecuteNonQueryAsync(obj, addParams, TestCustomerInfra.SPROC_TESTCUSTOMER_DEL);
        }

        public async Task<TestCustomer> GetTestCustomer(string CustId)
        {
            TestCustomer obj = new TestCustomer() {CustID = CustId };
            string[] addParams = new string[] { TestCustomerInfra.CUSTID };
            return await GetAsync(obj, addParams, TestCustomerInfra.SPROC_TESTCUSTOMER_SEL);
        }

        public async Task<int> SaveCustomer(TestCustomer obj)
        {
            string[] addParams = new string[] { TestCustomerInfra.CUSTID, TestCustomerInfra.FIRSTNAME, TestCustomerInfra.LASTNAME, TestCustomerInfra.MOBNO, TestCustomerInfra.ADDRESS, TestCustomerInfra.GENDER };

            return await ExecuteNonQueryAsync(obj, addParams, TestCustomerInfra.SPROC_TESTCUSTOMER_UPS);
        }

        public async Task<IEnumerable<TestCustomer>> GetAllTestCustomer(TestCustomer obj)
        {
            string[] addParams = new string[] { TestCustomerInfra.CUSTID, TestCustomerInfra.FIRSTNAME, TestCustomerInfra.LASTNAME, TestCustomerInfra.MOBNO, BaseInfra.PAGEINDEX, BaseInfra.PAGESIZE, BaseInfra.SORTEXP };
            return await GetAllAsync(obj, addParams, TestCustomerInfra.SPROC_TESTCUSTOMER_LSTALL);
        }

        public async Task<int> SaveFile(TestCustomer obj)
        {
            string[] addParams = new string[] { TestCustomerInfra.CUSTID, TestCustomerInfra.FILENAME, };
            return await ExecuteNonQueryAsync(obj, addParams, TestCustomerInfra.SPROC_TESTCUSTOMER_SAVEFILENAME);
        }

    }
}
