using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using LikeKero.Infra;
using LikeKero.Infra.BaseUri;
using LikeKero.Infra.FileSystem;
using LikeKero.Services.Interfaces;
namespace LikeKero.Services.Services
{
    public class TestCustomerService : ITestCustomer
    {
        private ITestCustomerRepository iTestCustomerRepository;
        private IFileUpload iFileUpload;
        private IUriService _uriService;
        private IOptions<FileSystemPath> _options = null;
        public TestCustomerService(ITestCustomerRepository ITestCustomerRepository, IFileUpload IFileUpload, IUriService IUriService, IOptions<FileSystemPath> options) : base()
        {
            iTestCustomerRepository = ITestCustomerRepository;
            iFileUpload = IFileUpload;
            _uriService = IUriService;
            _options = options;
        }
        public Task<int> AddEditAsync(TestCustomer obj)
        {
            throw new NotImplementedException();
        }
        public Task<int> DeleteAsync(TestCustomer obj)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<TestCustomer>> GetAllAsync(TestCustomer obj)        {
          
            return await iTestCustomerRepository.GetAllTestCustomer(obj);
        }
        public Task<TestCustomer> GetAsync(TestCustomer obj)
        {
            throw new NotImplementedException();
        }
        public async Task<string> DeleteTestCustomer(string CustId)
        {
             await iTestCustomerRepository.DeleteTestCustomer(CustId);
             return "";

        }
        public async Task<TestCustomer> GetTestCustomer(string CustId)
        {
            // return await iTestCustomerRepository.GetTestCustomer(CustId);

            var cust = await iTestCustomerRepository.GetTestCustomer(CustId);

            cust.FileName = _uriService.GetBaseUri().ToString() + Convert.ToString(_options.Value.AfterDomain) + "/" + _options.Value.TestCustFilePath + "/" + cust.CustID + "/" + cust.FileName;
            
            return cust;
        }
        public async Task<string> SaveCustomer(TestCustomer obj)
        {
            if (string.IsNullOrEmpty(obj.CustID))
                obj.CustID = Utility.GeneratorUniqueId("Cust_");
            await iTestCustomerRepository.SaveCustomer(obj);

            return obj.CustID;
        }
        public async Task<string> SaveFile(string CustID, IFormFile formFile)
        {
            TestCustomer obj = new TestCustomer()
            {
                CustID = CustID,
                FileName = formFile.FileName
            };

            //if (string.IsNullOrEmpty(CustID))               

            string strProfilePicPath = await iFileUpload.UploadTestCustomerFile(formFile, obj.CustID);

            await iTestCustomerRepository.SaveFile(obj);
            return obj.CustID;
        }


    }
}
