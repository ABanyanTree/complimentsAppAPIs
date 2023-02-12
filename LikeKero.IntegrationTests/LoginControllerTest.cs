using LikeKero.Api.ApiPath;
using LikeKero.Contract.Requests;
using LikeKero.Contract.Requests.User;
using LikeKero.Contract.Responses;
using LikeKero.Infra.Encryption;
using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LikeKero.IntegrationTests
{
    public class LoginControllerTest : IntegrationTest
    {
        /// <summary>
        /// TC_1.1 - To verify user login with valid Email/username and valid password
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task LoginWithCorrectCredentials()
        {
            List<HttpStatusCode> lstStatus = new List<HttpStatusCode>();
            List<string> successMethod = new List<string>();
            
            List<UserLoginRequest> lstEmailLoginRequest = new List<UserLoginRequest>();

            lstEmailLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "167STPHARMACYL4@email.com",
                Password = "Temp@123"
            });

            //lstEmailLoginRequest.Add(new UserLoginRequest
            //{
            //    EmailAddress = "139370L5@email.com",
            //    Password = "Temp@123"
            //});

            lstEmailLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "milind",
                Password = "Temp@123"
            });
            
            foreach (UserLoginRequest model in lstEmailLoginRequest)
            {
                var salt = await TestClient.GetAsync(requestUri: ApiRoutes.User.GetSalt);
                var _salt = salt.Content.ReadAsStringAsync().Result;

                string password = Cryptography.MD5Hash(model.Password);
                string hashedpwd = Cryptography.MD5Hash(_salt + password);
                              
                model.Password = hashedpwd;
                model.Salt = _salt;

                var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.Login, value: model);

                lstStatus.Add(result.StatusCode);
                var registrationResponse = await result.Content.ReadAsAsync<AuthSuccessResponse>();
                successMethod.Add(registrationResponse.Token);
            }

            lstStatus.Should().NotContain(HttpStatusCode.BadRequest);
            successMethod.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// TC_1.2/ TC_1.3 / TC_1.5  - To verify user login with invalid Email/username and password
        /// TC_1.3 - To verify user login with valid Email/username and invalid password
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task LoginWithIncorrectCredentials()
        {
            List<HttpStatusCode> lstStatus = new List<HttpStatusCode>();

            List<UserLoginRequest> lstUserLoginRequest = new List<UserLoginRequest>();

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "varun.b@email.com",
                Password = "Temp@123"
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "XYZ@email.com",
                Password = "Temp@123"
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "1234567",
                Password = "Temp@123"
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "var@1",
                Password = "Temp@123"
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "",
                Password = "Temp@123"
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "167STPHARMACYL4@email.com",
                Password = "Pemt@123"
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "139370L5@email.com",
                Password = "Temp@456"
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "milind",
                Password = "Temp@456"
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "139370L5@email.com",
                Password = "Temp_123"
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "Tracy1234",
                Password = "TEMP@123"
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "13thAvePharmacyIncL4@email.com",
                Password = ""
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "12345",
                Password = "Temp@123"
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "milind",
                Password = ""
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "167STPHARMACYL4@email.com",
                Password = "TEMP@123"
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "milind",
                Password = "TEMP@123"
            });

            foreach (UserLoginRequest model in lstUserLoginRequest)
            {
                var salt = await TestClient.GetAsync(requestUri: ApiRoutes.User.GetSalt);
                var _salt = salt.Content.ReadAsStringAsync().Result;

                string password = Cryptography.MD5Hash(model.Password);
                string hashedpwd = Cryptography.MD5Hash(_salt + password);

                model.Password = hashedpwd;
                model.Salt = _salt;

                var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.Login, value: model);

                lstStatus.Add(result.StatusCode);
            }

            lstStatus.Should().NotContain(HttpStatusCode.OK);
        }

        /// <summary>
        /// TC_1.6 / TC_1.7 - To verify user logout from application
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AccessApplicationWithoutLogin()
        {
            UserListRequest user = new UserListRequest();
            user.RequesterUserId = "CardinalHealthL0";
            user.SortExp = "FirstName asc";
            user.NoPaging = true;
            var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.UserList, value: user);

            result.StatusCode.Should().Be(expected: HttpStatusCode.Unauthorized);

        }

        /// <summary>
        /// TC_1.4 - To verify whether Email-ID/Username is not considered for case sensitivity 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task LoginWithCaseSensitiveCredentials()
        {
            List<HttpStatusCode> lstStatus = new List<HttpStatusCode>();
            List<string> successMethod = new List<string>();

            List<UserLoginRequest> lstUserLoginRequest = new List<UserLoginRequest>();
            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "10minutepharmacyl4@email.com",
                Password = "Temp@123"
            });

            lstUserLoginRequest.Add(new UserLoginRequest
            {
                EmailAddress = "MILIND",
                Password = "Temp@123"
            });

            foreach (UserLoginRequest model in lstUserLoginRequest)
            {
                var salt = await TestClient.GetAsync(requestUri: ApiRoutes.User.GetSalt);
                var _salt = salt.Content.ReadAsStringAsync().Result;

                string password = Cryptography.MD5Hash(model.Password);
                string hashedpwd = Cryptography.MD5Hash(_salt + password);
                               
                model.Password = hashedpwd;
                model.Salt = _salt;

                var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.Login, value: model);

                lstStatus.Add(result.StatusCode);
                var registrationResponse = await result.Content.ReadAsAsync<AuthSuccessResponse>();

                successMethod.Add(registrationResponse.Token);
            }

            lstStatus.Should().NotContain(HttpStatusCode.BadRequest);
            successMethod.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// TC_1.10 - To Verify Forgot Password Functionality
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ForgotPasswordWithValidUserName()
        {
            List<HttpStatusCode> lstStatus = new List<HttpStatusCode>();
            List<string> successMethod = new List<string>();

            List<UserForgotPasswordRequest> lstForgotPassword = new List<UserForgotPasswordRequest>();
            lstForgotPassword.Add(new UserForgotPasswordRequest
            {
                EmailAddress = "145THSTPHARMACYL4@email.com "
            });

            lstForgotPassword.Add(new UserForgotPasswordRequest
            {
                EmailAddress = "1445178L5@email.com"
            });

            foreach (UserForgotPasswordRequest model in lstForgotPassword)
            {                
                var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.ForgotPassword, value: model);

                lstStatus.Add(result.StatusCode);
            }
            lstStatus.Should().NotContain(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// TC_1.11 - To Verify Forgot Password Functionality With Invalid UserName
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ForgotPasswordWithInvalidUserName()
        {
            List<HttpStatusCode> lstStatus = new List<HttpStatusCode>();
            List<string> successMethod = new List<string>();

            List<UserForgotPasswordRequest> lstForgotPassword = new List<UserForgotPasswordRequest>();
            //lstForgotPassword.Add(new UserForgotPasswordRequest()
            //{
            //    EmailAddress = "varun.b@email.com"
            //});

            //lstForgotPassword.Add(new UserForgotPasswordRequest()
            //{
            //    EmailAddress = "XYZ@email.com"
            //});
            lstForgotPassword.Add(new UserForgotPasswordRequest()
            {
                EmailAddress = ""
            });

            foreach (UserForgotPasswordRequest model in lstForgotPassword)
            {
                var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.ForgotPassword, value: model);

                lstStatus.Add(result.StatusCode);
            }

            lstStatus.Should().NotContain(HttpStatusCode.OK);
        }
    }
}
