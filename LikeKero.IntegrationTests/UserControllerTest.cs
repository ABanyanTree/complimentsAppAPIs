using LikeKero.Api.ApiPath;
using LikeKero.Contract.Requests;
using LikeKero.Contract.Responses;
using LikeKero.Contract.Responses.User;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LikeKero.IntegrationTests
{
    public class UserControllerTest : IntegrationTest
    {
        /// <summary>
        /// TC_2.1.1 - To verify Add/Edit functionality with valid data
        /// Add user test cases needs to be updated for Active/Inactive Group checks. 
        /// As of now the users are getting added in Inactive groups as well.
        /// This issue is resolved on UI side only but not in Unit test code.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddValidUser()
        {
            await AutheticateAsync();

            List<HttpStatusCode> lstStatusCode = new List<HttpStatusCode>();

            List<CreateUserRequest> lstCreateUser = new List<CreateUserRequest>();
            lstCreateUser.Add(new CreateUserRequest()
            {
                GroupId = "GRP3_EgfmXcfa",
                FirstName = "Alan",
                LastName = "Walker",
                EmailAddress = "Alanbrand@email.com",
                JobCodeId = "Pharmacist",
                HiringDate = Convert.ToDateTime("2015-01-23"),
                RoleChangeDate = Convert.ToDateTime("2019-01-23"),
                Status = true,
                RequesterUserId = "USR_oDORmRyK"
            });

            lstCreateUser.Add(new CreateUserRequest()
            {
                GroupId = "GRP3_EgfmXcfa",
                FirstName = "Alan",
                LastName = "Walker",
                EmailAddress = "alanwbrand@email.com",
                JobCodeId = "Pharmacist",
                HiringDate = Convert.ToDateTime("2015-01-23"),
                RoleChangeDate = (DateTime?)null,
                Status = true,
                RequesterUserId = "USR_oDORmRyK"
            });

            lstCreateUser.Add(new CreateUserRequest()
            {
                GroupId = "GRP3_EgfmXcfa",
                FirstName = "Alan",
                LastName = "Walker",
                EmailAddress = "Alanbrand",
                JobCodeId = "Pharmacist",
                HiringDate = Convert.ToDateTime("2015-01-23"),
                RoleChangeDate = Convert.ToDateTime("2019-01-23"),
                Status = true,
                RequesterUserId = "USR_oDORmRyK"
            });

            lstCreateUser.Add(new CreateUserRequest()
            {
                GroupId = "GRP3_EgfmXcfa",
                FirstName = "Alan",
                LastName = "Walker",
                EmailAddress = "Alanawbrand",
                JobCodeId = "Pharmacist",
                HiringDate = Convert.ToDateTime("2015-01-23"),
                RoleChangeDate = (DateTime?)null,
                Status = true,
                RequesterUserId = "USR_oDORmRyK"

            });

            foreach (CreateUserRequest model in lstCreateUser)
            {
                model.LoginId = model.EmailAddress;
              //  model.RequesterUserId = "10MINUTEPHARMACYL4";
                model.UserId = "";

                var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.AddEditUser, value: model);

                lstStatusCode.Add(result.StatusCode);
            }

            lstStatusCode.Should().NotContain(HttpStatusCode.BadRequest);

        }

        /// <summary>
        /// TC_2.1.2 - To verify Add/Edit functionality with Invalid data
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddInValidUser()
        {
            await AutheticateAsync();

            List<HttpStatusCode> lstStatusCode = new List<HttpStatusCode>();

            List<CreateUserRequest> lstCreateUser = new List<CreateUserRequest>();
            lstCreateUser.Add(new CreateUserRequest()
            {
                GroupId = "Cardinal",
                FirstName = "!@#$%",
                LastName = "!@#$%",
                EmailAddress = "johnw@",
                JobCodeId = "",
                HiringDate = Convert.ToDateTime("2020-01-23"),
                RoleChangeDate = Convert.ToDateTime("2020-01-23"),
                Status = true,
                RequesterUserId = "USR_fhpNZ6gg"
            });

            lstCreateUser.Add(new CreateUserRequest()
            {
                GroupId = "MedicineFranchisee",
                FirstName = "",
                LastName = "",
                EmailAddress = "johnw@email",
                JobCodeId = "",
                HiringDate = Convert.ToDateTime("2020-01-23"),
                RoleChangeDate = Convert.ToDateTime("2020-01-23"),
                Status = true,
                RequesterUserId = "USR_fhpNZ6gg"
            });

            lstCreateUser.Add(new CreateUserRequest()
            {
                GroupId = "Cardinal",
                FirstName = "12345",
                LastName = "12345",
                EmailAddress = "@email.com",
                JobCodeId = "",
                HiringDate = Convert.ToDateTime("2020-01-23"),
                RoleChangeDate = Convert.ToDateTime("2020-01-23"),
                Status = true,
                RequesterUserId = "USR_fhpNZ6gg"
            });

            lstCreateUser.Add(new CreateUserRequest()
            {
                GroupId = "MedicineFranchisee",
                FirstName = "abcd_1234",
                LastName = "abcd_1234",
                EmailAddress = "",
                JobCodeId = "",
                HiringDate = Convert.ToDateTime("2020-01-23"),
                RoleChangeDate = Convert.ToDateTime("2020-01-23"),
                Status = true,
                RequesterUserId = "USR_fhpNZ6gg"
            });

            lstCreateUser.Add(new CreateUserRequest()
            {
                GroupId = "MedicineFranchisee",
                FirstName = "!@#1234",
                LastName = "!@#1234",
                EmailAddress = "!@#$%",
                JobCodeId = "",
                HiringDate = Convert.ToDateTime("2020-01-23"),
                RoleChangeDate = Convert.ToDateTime("2020-01-23"),
                Status = true,
                RequesterUserId = "USR_fhpNZ6gg"
            });


            foreach (CreateUserRequest model in lstCreateUser)
            {
                model.LoginId = model.EmailAddress;
            //    model.RequesterUserId = "10MINUTEPHARMACYL4";
                model.UserId = "";

                var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.AddEditUser, value: model);
                lstStatusCode.Add(result.StatusCode);
            }

            lstStatusCode.Should().NotContain(HttpStatusCode.OK);

        }

        /// <summary>
        /// TC_2.1.3 - To verify whether duplicate users are not getting added 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task EmailIdIsInUse()
        {
            await AutheticateAsync();

            List<HttpStatusCode> lstStatusCode = new List<HttpStatusCode>();

            List<string> lstMessage = new List<string>();
            Dictionary<string, string> lstUsername = new Dictionary<string, string>();

            lstUsername.Add("1", "167STPHARMACYL4@email.com");

            lstUsername.Add("2", "milind");


            foreach (KeyValuePair<string, string> firstRow in lstUsername)
            {

                string EmailAddress = firstRow.Value;
                string UserId = "";
                var queryString = string.Format("?EmailAddress={0}&UserID={1}", EmailAddress, UserId);
                var result = await TestClient.GetAsync(requestUri: ApiRoutes.User.IsEmailInUse + queryString);
                var responseString = result.Content.ReadAsStringAsync().Result;

                if (responseString.Contains("in use"))
                {
                    lstStatusCode.Add(HttpStatusCode.OK);
                }
                else
                {
                    lstStatusCode.Add(HttpStatusCode.BadRequest);
                }
            }

            lstStatusCode.Should().NotContain(HttpStatusCode.BadRequest);

        }

        /// <summary>
        /// TC_2.1.3 - To verify whether duplicate users are not getting added 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task EmailIdIsNotInUse()
        {
            await AutheticateAsync();

            List<HttpStatusCode> lstStatusCode = new List<HttpStatusCode>();

            List<string> lstMessage = new List<string>();
            Dictionary<string, string> lstUsername = new Dictionary<string, string>();

           // lstUsername.Add("1", "167STPHARMACYL4@email.com");

            lstUsername.Add("1", "Marks");


            foreach (KeyValuePair<string, string> firstRow in lstUsername)
            {

                string EmailAddress = firstRow.Value;
                string UserId = "";
                var queryString = string.Format("?EmailAddress={0}&UserID={1}", EmailAddress, UserId);
                var result = await TestClient.GetAsync(requestUri: ApiRoutes.User.IsEmailInUse + queryString);
                var responseString = result.Content.ReadAsStringAsync().Result;

                if (responseString.Contains("in use"))
                {
                    lstStatusCode.Add(HttpStatusCode.BadRequest);
                }
                else
                {
                    lstStatusCode.Add(HttpStatusCode.OK);
                }
            }

            lstStatusCode.Should().NotContain(HttpStatusCode.BadRequest);

        }

        /// <summary>
        /// TC_2.2.1, TC_2.2.2 - To Verify Inactive Filter on manage screen
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUserListWithValidInactiveFilter()
        {
            await AutheticateAsync();

            List<HttpStatusCode> lstStatusCode = new List<HttpStatusCode>();
            List<bool> lstStaus = new List<bool>();
            List<List<UserListResponse>> lstUserList = new List<List<UserListResponse>>();
            List<string> lstMessage = new List<string>();

            List<UserListRequest> lstUserListRequest = new List<UserListRequest>();
            lstUserListRequest.Add(new UserListRequest()
            {
                Status = false
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                Status = false,
                GroupId = "CardinalHealth"
            });

            //lstUserListRequest.Add(new UserListRequest()
            //{
            //    FirstName = "Dhruv",
            //    Status = false,
            //    GroupId = "Cardinal"
            //});

            //lstUserListRequest.Add(new UserListRequest()
            //{
            //    FirstName = "Cardinal",
            //    Status = false,
            //    GroupId = "Cardinal"
            //});

            //lstUserListRequest.Add(new UserListRequest()
            //{
            //    FirstName = "mark",
            //    Status = false
            //});

            //lstUserListRequest.Add(new UserListRequest()
            //{
            //    FirstName = "april",
            //    Status = false
            //});

            foreach (UserListRequest model in lstUserListRequest)
            {
                model.RequesterUserId = "CardinalHealthL0";
                model.SortExp = "FirstName asc";
                model.NoPaging = true;
                var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.UserList, value: model);

                lstStatusCode.Add(result.StatusCode);
                var registrationResponse = await result.Content.ReadAsAsync<List<UserListResponse>>();
                if (registrationResponse.Count > 0)
                {
                    lstUserList.Add(registrationResponse);
                    lstStaus.Add(registrationResponse.Any(x => x.Status == true));
                }
            }
            lstStatusCode.Should().NotContain(HttpStatusCode.BadRequest);
            Assert.Equal(lstStatusCode.Count, lstUserList.Count);
            lstStaus.Should().NotContain("true");
        }

        /// <summary>
        /// TC_2.2, TC_2.2.2, TC_2.2.5 - To Verify Active Filter on manage screen
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUserListWithValidActiveFilter()
        {
            await AutheticateAsync();

            List<HttpStatusCode> lstStatusCode = new List<HttpStatusCode>();
            List<bool> lstStaus = new List<bool>();
            List<List<UserListResponse>> lstUserList = new List<List<UserListResponse>>();
            List<string> lstMessage = new List<string>();

            List<UserListRequest> lstUserListRequest = new List<UserListRequest>();
            lstUserListRequest.Add(new UserListRequest()
            {               
                Status = true,
                RequesterUserId = "CardinalHealthL0"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                FirstName = "Alan",
                Status = true,
                GroupId = "GRP3_EgfmXcfa",
                RequesterUserId = "CardinalHealthL0"// "5912818L5"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                FirstName = "Walker",
                Status = true,
                GroupId = "GRP3_EgfmXcfa",
                RequesterUserId = "CardinalHealthL0"// "USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                FirstName = "Alan",
                Status = true,
                RequesterUserId = "CardinalHealthL0"//"USR_oDORmRyK"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                GroupId = "GRP3_EgfmXcfa",
                Status = true,
                RequesterUserId = "CardinalHealthL0"//"USR_oDORmRyK"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                EmailAddress = "Alanbrand@email.com",
                Status = true,
                RequesterUserId = "CardinalHealthL0"//"USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                JobCodeId = "Pharmacist",
                Status = true,
                RequesterUserId = "CardinalHealthL0"//"USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                HiringDateFrom = Convert.ToDateTime("2015-01-23"),
                HiringDateTo = Convert.ToDateTime("2015-01-23"),
                Status = true,
                RequesterUserId = "CardinalHealthL0"//"USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                RoleChangeDateFrom = Convert.ToDateTime("2019-01-23"),
                RoleChangeDateTo = Convert.ToDateTime("2019-01-23"),
                Status = true,
                RequesterUserId = "CardinalHealthL0"//"USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                EmailAddress = "Alanbrand@email.com",
                JobCodeId = "Pharmacist",
                Status = true,
                RequesterUserId = "CardinalHealthL0"//"USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                EmailAddress = "alanwbrand@email.com",
                HiringDateFrom = Convert.ToDateTime("2015-01-23"),
                HiringDateTo = Convert.ToDateTime("2015-01-23"),
                Status = true,
                RequesterUserId = "CardinalHealthL0"//"USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                EmailAddress = "mark.april@email.com",
                RoleChangeDateTo = Convert.ToDateTime("2019-12-10"),
                RoleChangeDateFrom = Convert.ToDateTime("2019-12-10"),
                Status = true,
                RequesterUserId = "CardinalHealthL0"//"USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                EmailAddress = "Alanbrand",
                JobCodeId = "Pharmacist",
                HiringDateTo = Convert.ToDateTime("2015-01-23"),
                HiringDateFrom = Convert.ToDateTime("2015-01-23"),
                Status = true,
                RequesterUserId = "CardinalHealthL0"//"USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                EmailAddress = "Alanbrand@email.com",
                JobCodeId = "Pharmacist",
                HiringDateTo = Convert.ToDateTime("2015-01-23"),
                HiringDateFrom = Convert.ToDateTime("2015-01-23"),
                RoleChangeDateTo = Convert.ToDateTime("2019-01-23"),
                RoleChangeDateFrom = Convert.ToDateTime("2019-01-23"),
                Status = true,
                RequesterUserId = "CardinalHealthL0"//"USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                JobCodeId = "Pharmacist",
                HiringDateTo = Convert.ToDateTime("2015-01-23"),
                HiringDateFrom = Convert.ToDateTime("2015-01-23"),
                Status = true,
                RequesterUserId = "CardinalHealthL0"// "USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                JobCodeId = "Pharmacist",
                RoleChangeDateFrom = Convert.ToDateTime("2019-01-23"),
                RoleChangeDateTo = Convert.ToDateTime("2019-01-23"),
                Status = true,
                RequesterUserId = "CardinalHealthL0"// "USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                JobCodeId = "Pharmacist",
                HiringDateTo = Convert.ToDateTime("2015-01-23"),
                HiringDateFrom = Convert.ToDateTime("2015-01-23"),
                RoleChangeDateFrom = Convert.ToDateTime("2019-01-23"),
                RoleChangeDateTo = Convert.ToDateTime("2019-01-23"),
                Status = true,
                RequesterUserId = "CardinalHealthL0"//"USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                HiringDateTo = Convert.ToDateTime("2015-01-23"),
                HiringDateFrom = Convert.ToDateTime("2015-01-23"),
                RoleChangeDateFrom = Convert.ToDateTime("2019-01-23"),
                RoleChangeDateTo = Convert.ToDateTime("2019-01-23"),
                Status = true,
                RequesterUserId = "CardinalHealthL0"// "USR_fhpNZ6gg"
            });

            lstUserListRequest.Add(new UserListRequest()
            {
                EmailAddress = "Alanbrand@email.com",
                HiringDateTo = Convert.ToDateTime("2015-01-23"),
                HiringDateFrom = Convert.ToDateTime("2015-01-23"),
                RoleChangeDateFrom = Convert.ToDateTime("2019-01-23"),
                RoleChangeDateTo = Convert.ToDateTime("2019-01-23"),
                Status = true,
                RequesterUserId = "CardinalHealthL0"//"USR_fhpNZ6gg"
            });


            foreach (UserListRequest model in lstUserListRequest)
            {

                // model.RequesterUserId = "CardinalHealthL0";
                model.SortExp = "FirstName asc";
                model.NoPaging = true;

                var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.UserList, value: model);

                lstStatusCode.Add(result.StatusCode);
                var registrationResponse = await result.Content.ReadAsAsync<List<UserListResponse>>();
                if (registrationResponse.Count > 0)
                {
                    lstUserList.Add(registrationResponse);
                    lstStaus.Add(registrationResponse.Any(x => x.Status == false));
                }
            }
            lstStatusCode.Should().NotContain(HttpStatusCode.BadRequest);
            Assert.Equal(lstStatusCode.Count, lstUserList.Count);
            lstStaus.Should().NotContain("true");
        }

        /// <summary>
        /// TC_2.2.4 - To Verify Clear button Functionality
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUserListWitClearFilter()
        {
            await AutheticateAsync();

            List<HttpStatusCode> lstStatusCode = new List<HttpStatusCode>();
            List<bool> lstStaus = new List<bool>();
            List<List<UserListResponse>> lstUserList = new List<List<UserListResponse>>();
            List<string> lstMessage = new List<string>();

            List<UserListRequest> lstUserListRequest = new List<UserListRequest>();
            lstUserListRequest.Add(new UserListRequest()
            {
                Status = false
            });


            foreach (UserListRequest model in lstUserListRequest)
            {
                model.RequesterUserId = "CardinalHealthL0";
                model.SortExp = "FirstName asc";
                model.NoPaging = true;
                var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.UserList, value: model);

                lstStatusCode.Add(result.StatusCode);
                var registrationResponse = await result.Content.ReadAsAsync<List<UserListResponse>>();
                lstUserList.Add(registrationResponse);
                lstStaus.Add(registrationResponse.Any(x => x.Status == false));

            }
            lstStatusCode.Should().NotContain(HttpStatusCode.BadRequest);
            lstStaus.Should().NotContain("true");
            lstUserList.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// TC_2.2.3- To Verify Advanced Search Filters functionality with invalid data
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUserListWithInValidFilter()
        {
            await AutheticateAsync();

            List<HttpStatusCode> lstStatusCode = new List<HttpStatusCode>();

            List<List<UserListResponse>> lstUserList = new List<List<UserListResponse>>();
            List<string> lstMessage = new List<string>();
            List<UserListRequest> lstUserRequest = new List<UserListRequest>();
            //lstUserRequest.Add(new UserListRequest()
            //{
            //    FirstName = "Milind",
            //    Status = true,
            //    GroupId = "Cardin"
            //});

            //lstUserRequest.Add(new UserListRequest()
            //{
            //    FirstName = "Patil",
            //    Status = false,
            //    GroupId = "CardinalHealth"
            //});

            lstUserRequest.Add(new UserListRequest()
            {
                FirstName = "April",
                Status = true,
                GroupId = "CardinalHealth",
                RequesterUserId = "USR_oDORmRyK"
            });

            lstUserRequest.Add(new UserListRequest()
            {
                FirstName = "April",
                Status = false,
                GroupId = "CardinalHealth",
                RequesterUserId = "USR_oDORmRyK"
            });

            lstUserRequest.Add(new UserListRequest()
            {
                FirstName = "April",
                Status = false,
                RequesterUserId = "USR_oDORmRyK"
            });

            //lstUserRequest.Add(new UserListRequest()
            //{
            //    FirstName = "Patil",
            //    Status = false
            //});

            lstUserRequest.Add(new UserListRequest()
            {
                GroupId = "CardinalHealth",
                Status = false,
                RequesterUserId = "USR_oDORmRyK"

            });

            lstUserRequest.Add(new UserListRequest()
            {
                GroupId = "CardinalHealth",
                Status = true,
                RequesterUserId = "USR_oDORmRyK"
            });

            foreach (UserListRequest model in lstUserRequest)
            {                
                model.SortExp = "FirstName asc";
                model.NoPaging = true;
                var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.UserList, value: model);

                lstStatusCode.Add(result.StatusCode);
                var registrationResponse = await result.Content.ReadAsAsync<List<UserListResponse>>();
                if (registrationResponse.Count > 0)
                {
                    lstUserList.Add(registrationResponse);

                }
            }
            lstStatusCode.Should().NotContain(HttpStatusCode.BadRequest);
            Assert.Empty(lstUserList);
        }


        /// <summary>
        /// TC_2.2.3- To Verify Advanced Search Filters functionality with invalid data
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUserListWithInValidAdvancedFilter()
        {
            await AutheticateAsync();

            List<HttpStatusCode> lstStatusCode = new List<HttpStatusCode>();

            List<List<UserListResponse>> lstUserList = new List<List<UserListResponse>>();
            List<string> lstMessage = new List<string>();
            List<UserListRequest> lstUserRequest = new List<UserListRequest>();

            //Set 1

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "varun.b@email.com",
                Status = true

            }
                        );
            //Set 2

            lstUserRequest.Add(new UserListRequest()
            {
                HiringDateTo = Convert.ToDateTime("2020-12-02"),
                HiringDateFrom = Convert.ToDateTime("2020-12-02"),
                Status = true

            }
                        );

            //Set 3

            lstUserRequest.Add(new UserListRequest()
            {
                RoleChangeDateTo = Convert.ToDateTime("2020-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2020-12-03"),
                Status = true

            }
                        );

            //Set 4

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "varun.b@email.com",
                JobCodeId = "Designer",
                Status = true

            });

            //Set 5

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "varun.b@email.com",
                HiringDateTo = Convert.ToDateTime("2019-12-02"),
                HiringDateFrom = Convert.ToDateTime("2019-12-02"),
                Status = true

            });

            //Set 6

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "mark.april@email.com",
                HiringDateTo = Convert.ToDateTime("2020-12-02"),
                HiringDateFrom = Convert.ToDateTime("2020-12-02"),
                Status = true
            });

            //Set 7

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "varun.b@email.com",
                RoleChangeDateTo = Convert.ToDateTime("2019-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2019-12-03"),
                Status = true

            }
                        );

            //Set 8

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "mark.april@email.com",
                RoleChangeDateTo = Convert.ToDateTime("2020-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2020-12-03"),
                Status = true

            }
                        );

            //Set 9

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "varun.b@email.com",
                JobCodeId = "Cardinal Health",
                HiringDateTo = Convert.ToDateTime("2020-12-02"),
                HiringDateFrom = Convert.ToDateTime("2020-12-02"),
                Status = true
            }
                        );

            //Set 10

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "varun.b@email.com",
                JobCodeId = "Cardinal Health",
                HiringDateTo = Convert.ToDateTime("2019-12-02"),
                HiringDateFrom = Convert.ToDateTime("2019-12-02"),
                Status = true
            }
                        );

            //Set 11

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "mark.april@email.com",
                JobCodeId = "Cardinal Health",
                HiringDateTo = Convert.ToDateTime("2020-12-02"),
                HiringDateFrom = Convert.ToDateTime("2020-12-02"),
                Status = true
            }
                        );
            //New Record
            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "mark.april@email.com",
                HiringDateTo = Convert.ToDateTime("2019-12-01"),
                HiringDateFrom = Convert.ToDateTime("2019-12-01"),
                RoleChangeDateFrom = Convert.ToDateTime("2019-12-10"),
                RoleChangeDateTo = Convert.ToDateTime("2019-12-10"),
                Status = true,
                RequesterUserId = "USR_fhpNZ6gg"
            });

            ////Set 12

            //lstUserRequest.Add(new UserListRequest()
            //{
            //    EmailAddress = "varun.b@email.com",
            //    JobCodeId = "Designer",
            //    HiringDateTo = Convert.ToDateTime("201-12-02"),
            //    HiringDateFrom = Convert.ToDateTime("201-12-02"),
            //    RoleChangeDateTo = Convert.ToDateTime("201-12-03"),
            //    RoleChangeDateFrom = Convert.ToDateTime("201-12-03"),
            //    Status = true
            //}
            //            );

            //Set 13

            //lstUserRequest.Add(new UserListRequest()
            //{
            //    EmailAddress = "mark.april@email.com",
            //    JobCodeId = "Designer",
            //    HiringDateTo = Convert.ToDateTime("201-12-02"),
            //    HiringDateFrom = Convert.ToDateTime("201-12-02"),
            //    RoleChangeDateTo = Convert.ToDateTime("2019-12-03"),
            //    RoleChangeDateFrom = Convert.ToDateTime("2019-12-03"),
            //    Status = true
            //}
            //            );

            //Set 14

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "varun.b@email.com",
                JobCodeId = "Designer",
                HiringDateTo = Convert.ToDateTime("2019-12-02"),
                HiringDateFrom = Convert.ToDateTime("2019-12-02"),
                RoleChangeDateTo = Convert.ToDateTime("2020-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2020-12-03"),
                Status = true
            }
            );

            //Set 15

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "mark.april@email.com",
                JobCodeId = "Designer",
                HiringDateTo = Convert.ToDateTime("2019-12-02"),
                HiringDateFrom = Convert.ToDateTime("2019-12-02"),
                RoleChangeDateTo = Convert.ToDateTime("2020-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2020-12-03"),
                Status = true
            }
                        );

            //Set 16

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "varun.b@email.com",
                JobCodeId = "Cardinal Health",
                HiringDateTo = Convert.ToDateTime("2020-12-02"),
                HiringDateFrom = Convert.ToDateTime("2020-12-02"),
                RoleChangeDateTo = Convert.ToDateTime("2020-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2020-12-03"),
                Status = true
            }
            );

            //Set 17

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "mark.april@email.com",
                JobCodeId = "Designer",
                HiringDateTo = Convert.ToDateTime("2020-12-02"),
                HiringDateFrom = Convert.ToDateTime("2020-12-02"),
                RoleChangeDateTo = Convert.ToDateTime("2020-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2020-12-03"),
                Status = true
            }
            );


            //Set 18

            //lstUserRequest.Add(new UserListRequest()
            //{

            //    JobCodeId = "Cardinal Health",
            //    HiringDateTo = Convert.ToDateTime("201-12-02"),
            //    HiringDateFrom = Convert.ToDateTime("201-12-02"),
            //    Status = true
            //}
            //);

            //Set 19

            //lstUserRequest.Add(new UserListRequest()
            //{
            //    JobCodeId = "Designer",
            //    RoleChangeDateTo = Convert.ToDateTime("201-12-03"),
            //    RoleChangeDateFrom = Convert.ToDateTime("201-12-03"),
            //    Status = true
            //}
            //);

            //Set 20

            lstUserRequest.Add(new UserListRequest()
            {

                JobCodeId = "Designer",
                HiringDateTo = Convert.ToDateTime("2020-12-02"),
                HiringDateFrom = Convert.ToDateTime("2020-12-02"),
                RoleChangeDateTo = Convert.ToDateTime("2020-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2020-12-03"),
                Status = true

            }
            );

            //Set 21
            lstUserRequest.Add(new UserListRequest()
            {

                JobCodeId = "Designer",
                HiringDateTo = Convert.ToDateTime("2019-12-02"),
                HiringDateFrom = Convert.ToDateTime("2019-12-02"),
                RoleChangeDateTo = Convert.ToDateTime("2020-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2020-12-03"),
                Status = true

            }
            );

            //Set 22

            lstUserRequest.Add(new UserListRequest()
            {
                JobCodeId = "Designer",
                HiringDateTo = Convert.ToDateTime("2020-12-02"),
                HiringDateFrom = Convert.ToDateTime("2020-12-02"),
                RoleChangeDateTo = Convert.ToDateTime("2019-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2019-12-03"),
                Status = true

            }
            );

            //Set 23

            lstUserRequest.Add(new UserListRequest()
            {

                HiringDateTo = Convert.ToDateTime("2019-12-02"),
                HiringDateFrom = Convert.ToDateTime("2019-12-02"),
                RoleChangeDateTo = Convert.ToDateTime("2020-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2020-12-03"),
                Status = true

            }
            );


            //Set 24

            lstUserRequest.Add(new UserListRequest()
            {

                HiringDateTo = Convert.ToDateTime("2020-12-02"),
                HiringDateFrom = Convert.ToDateTime("2020-12-02"),
                RoleChangeDateTo = Convert.ToDateTime("2019-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2019-12-03"),
                Status = true

            }
            );

            //Set 25

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "varun.b@email.com",
                HiringDateTo = Convert.ToDateTime("2020-12-02"),
                HiringDateFrom = Convert.ToDateTime("2020-12-02"),
                RoleChangeDateTo = Convert.ToDateTime("2020-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2020-12-03"),
                Status = true

            }
            );


            //Set 26


            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "mark.aapril@email.com",
                HiringDateTo = Convert.ToDateTime("2020-12-02"),
                HiringDateFrom = Convert.ToDateTime("2020-12-02"),
                RoleChangeDateTo = Convert.ToDateTime("2020-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2020-12-03"),
                Status = true
            }
            );

            //set 27

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "mark.april@email.com",
                HiringDateTo = Convert.ToDateTime("2019-12-02"),
                HiringDateFrom = Convert.ToDateTime("2019-12-02"),
                RoleChangeDateTo = Convert.ToDateTime("2020-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2020-12-03"),
                Status = true
            }

            );

            //set 28

            lstUserRequest.Add(new UserListRequest()
            {
                EmailAddress = "mark.april@email.com",
                HiringDateTo = Convert.ToDateTime("2020-12-02"),
                HiringDateFrom = Convert.ToDateTime("2020-12-02"),
                RoleChangeDateTo = Convert.ToDateTime("2019-12-03"),
                RoleChangeDateFrom = Convert.ToDateTime("2019-12-03"),
                Status = true

            }
            );



            foreach (UserListRequest model in lstUserRequest)
            {

                model.RequesterUserId = "CardinalHealthL0";
                model.SortExp = "FirstName asc";
                model.NoPaging = true;
                var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.UserList, value: model);

                lstStatusCode.Add(result.StatusCode);
                var registrationResponse = await result.Content.ReadAsAsync<List<UserListResponse>>();
                if (registrationResponse.Count > 0)
                {
                    lstUserList.Add(registrationResponse);

                }
            }
            lstStatusCode.Should().NotContain(HttpStatusCode.BadRequest);
            Assert.Empty(lstUserList);
        }

        ///// <summary>
        ///// TC_2.1.4, TC_2.1.13,TC_2.1.11, TC_2.1.9, TC_2.1.6- -To verify the Bulk Upload functionality with valid file
        ///// </summary>
        ///// <returns></returns>
        //[Fact]
        //public async Task ValidateUploadedFile()
        //{
        //    await AutheticateAsync();
        //    List<bool> isValidExtension = new List<bool>();
        //    List<HttpStatusCode> lstStatusCode = new List<HttpStatusCode>();
        //    Dictionary<string, string> lstFileUpload = new Dictionary<string, string>();
        //    lstFileUpload.Add("1", @"C:\NBLS\Data in all fields.xlsx");
        //    lstFileUpload.Add("2", @"C:\NBLS\Data in all fields - Copy.xls");
        //    lstFileUpload.Add("3", @"C:\NBLS\Data in mandatory fields only.xlsx");
        //    lstFileUpload.Add("4", @"C:\NBLS\TC_2.1.9.xlsx");


        //    foreach (KeyValuePair<string, string> fileUpload in lstFileUpload)
        //    {
        //        using (var file2 = File.OpenRead(fileUpload.Value))
        //        using (var content2 = new StreamContent(file2))
        //        using (var formData = new MultipartFormDataContent())
        //        {
        //            string requestorUserId = "10MINUTEPHARMACYL4";
        //            string fileName = Path.GetFileName((fileUpload.Value));
        //            formData.Add(new StringContent(requestorUserId), "requestorUserId");
        //            formData.Add(content2, "uploadedFile", fileName);
        //            // response = await client.PostAsync(url, formData);                   

        //            var result = await TestClient.PostAsync(requestUri: ApiRoutes.User.UploadUserImport, formData);
        //            lstStatusCode.Add(result.StatusCode);

        //        }
        //    }
        //    lstStatusCode.Should().NotContain(HttpStatusCode.BadRequest);
        //}

        /// <summary>
        /// TC_2.1.5 -To verify the Bulk Upload functionality with invalid file
        /// </summary>
        /// <returns></returns>
        //[Fact]
        //public async Task ValidateUploadedInvalidFile()
        //{
        //    await AutheticateAsync();
        //    List<bool> isValidExtension = new List<bool>();
        //    List<HttpStatusCode> lstStatusCode = new List<HttpStatusCode>();
        //    Dictionary<string, string> lstFileUpload = new Dictionary<string, string>();
        //    lstFileUpload.Add("1", @"C:\NBLS\attachment_56232981-1.png");
        //    lstFileUpload.Add("2", @"C:\NBLS\demo.docx");
        //    lstFileUpload.Add("3", @"C:\NBLS\lesson2.pdf");
        //    lstFileUpload.Add("4", @"C:\NBLS\Test.pptx");
        //    lstFileUpload.Add("5", @"C:\NBLS\xps.xps");
        //    lstFileUpload.Add("6", @"C:\NBLS\xltx.xltx");
        //    lstFileUpload.Add("7", @"C:\NBLS\xml.xml");
        //    lstFileUpload.Add("8", @"C:\NBLS\File Format Examples copy.zip");
        //    lstFileUpload.Add("9", @"C:\NBLS\dotx.txt");



        //    foreach (KeyValuePair<string, string> fileUpload in lstFileUpload)
        //    {
        //        using (var file2 = File.OpenRead(fileUpload.Value))
        //        using (var content2 = new StreamContent(file2))
        //        using (var formData = new MultipartFormDataContent())
        //        {
        //            string requestorUserId = "10MINUTEPHARMACYL4";
        //            string fileName = Path.GetFileName((fileUpload.Value));
        //            formData.Add(new StringContent(requestorUserId), "requestorUserId");
        //            formData.Add(content2, "uploadedFile", fileName);
        //            // response = await client.PostAsync(url, formData);                   

        //            var result = await TestClient.PostAsync(requestUri: ApiRoutes.User.UploadUserImport, formData);
        //            lstStatusCode.Add(result.StatusCode);

        //        }
        //    }
        //    lstStatusCode.Should().NotContain(HttpStatusCode.OK);
        //}

        ///// <summary>
        ///// TC_2.1.7,TC_2.1.12, TC_2.1.10, TC_2.1.8  -To verify the Bulk Import functionality with invalid data 
        ///// </summary>
        ///// <returns></returns>
        //[Fact]
        //public async Task ValidateUploadedInvalidData()
        //{
        //    await AutheticateAsync();
        //    List<bool> isValidExtension = new List<bool>();
        //    List<HttpStatusCode> lstStatusCode = new List<HttpStatusCode>();
        //    Dictionary<string, string> lstFileUpload = new Dictionary<string, string>();
        //    lstFileUpload.Add("1", @"C:\NBLS\TC_2.1.7.xlsx");
        //    lstFileUpload.Add("2", @"C:\NBLS\TC_2.1.10.xlsx");
        //    lstFileUpload.Add("3", @"C:\NBLS\TC_2.1.12.xlsx");


        //    foreach (KeyValuePair<string, string> fileUpload in lstFileUpload)
        //    {
        //        using (var file2 = File.OpenRead(fileUpload.Value))
        //        using (var content2 = new StreamContent(file2))
        //        using (var formData = new MultipartFormDataContent())
        //        {
        //            string requestorUserId = "10MINUTEPHARMACYL4";
        //            string fileName = Path.GetFileName((fileUpload.Value));
        //            formData.Add(new StringContent(requestorUserId), "requestorUserId");
        //            formData.Add(content2, "uploadedFile", fileName);
        //            // response = await client.PostAsync(url, formData);                   

        //            var result = await TestClient.PostAsync(requestUri: ApiRoutes.User.UploadUserImport, formData);
        //            var response = result.Content.ReadAsAsync<BulkImportListResponse>();
        //            if (response.Result.ImportStatus != "Completed Succesfully" || response.Result.ImportStatus != "Completed With Errors")
        //            {
        //                lstStatusCode.Add(HttpStatusCode.BadRequest);
        //            }
        //        }
        //    }
        //    lstStatusCode.Should().NotContain(HttpStatusCode.OK);
        //}

        ///// <summary>
        ///// TC_2.1.14 - To verify export to excel functionality with all data
        ///// </summary>
        ///// <returns></returns>
        //[Fact]
        //public async Task GetUserListWithExportToExcel()
        //{
        //    await AutheticateAsync();

        //    List<HttpStatusCode> lstStatusCode = new List<HttpStatusCode>();
        //    List<bool> lstStaus = new List<bool>();
        //    List<List<UserListResponse>> lstUserList = new List<List<UserListResponse>>();
        //    List<string> lstMessage = new List<string>();

        //    List<UserListRequest> lstUserListRequest = new List<UserListRequest>();
        //    lstUserListRequest.Add(new UserListRequest()
        //    {
        //        Status = true
        //    });

        //    foreach (UserListRequest model in lstUserListRequest)
        //    {
        //        model.RequesterUserId = "CardinalHealthL0";
        //        model.SortExp = "FirstName asc";
        //        model.NoPaging = true;

        //        var result = await TestClient.PostAsJsonAsync(requestUri: ApiRoutes.User.UserList, value: model);

        //        lstStatusCode.Add(result.StatusCode);
        //        var registrationResponse = await result.Content.ReadAsAsync<List<UserListResponse>>();
        //        if (registrationResponse.Count > 50)
        //        {
        //            lstUserList.Add(registrationResponse);
        //            lstStaus.Add(registrationResponse.Any(x => x.Status == false));
        //        }
        //    }
        //    lstStatusCode.Should().NotContain(HttpStatusCode.BadRequest);
        //    Assert.Equal(lstStatusCode.Count, lstUserList.Count);
        //    lstStaus.Should().NotContain("true");
        //}
    }
}

