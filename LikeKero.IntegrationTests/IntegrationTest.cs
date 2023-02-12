using LikeKero.Api;
using LikeKero.Api.ApiPath;
using LikeKero.Contract.Requests;
using LikeKero.Contract.Responses;
using LikeKero.Infra.Encryption;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LikeKero.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient = null;
        protected IntegrationTest()
        {
            var appfactory = new WebApplicationFactory<Startup>
            ().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    //services.RemoveAll(typeof(DataContext));
                    //services.AddDbContext<DataContext>(optionsAction: options => { options.UseInMemoryDatabase(databaseName: "TestBD"); });

                });
            });
            TestClient = appfactory.CreateClient();

            string uri = TestClient.BaseAddress.ToString();

            TestClient.BaseAddress = new Uri("https://localhost:5001");

            //https://localhost:5001

        }
        protected async Task AutheticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(scheme: "bearer",
                parameter: await GetJwtToken());
        }       
        public async Task<string> GetJwtToken()
        {

            var salt = await TestClient.GetAsync(requestUri: ApiRoutes.User.GetSalt);
            var _salt = salt.Content.ReadAsStringAsync().Result;

            string password = Cryptography.MD5Hash("Temp@123");
            string hashedpwd = Cryptography.MD5Hash(_salt + password);

            UserLoginRequest user = new UserLoginRequest();
            user.EmailAddress = "systemadmin@email.com";
            user.Password = hashedpwd;
            user.Salt = _salt;
           


            var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.Login, value: user);
            var registrationResponse = await result.Content.ReadAsAsync<AuthSuccessResponse>();
            return registrationResponse.Token;
        }
    }
}
