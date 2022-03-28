using System.Threading.Tasks;
using System.Net;
using Xunit;
using CDSP_API.Contracts.V1.Requests;
using CDSP_API.Contracts;
using System.Net.Http;
using System;
using Xunit.Extensions.Ordering;
using CDSP_API.Data;
using CDSP_API.Contracts.V1.Responses;
using System.Collections.Generic;

namespace CommunityDrivenSocialPlatform.UnitTests
{
    public class UserControllerTests : IntegrationTests
    {
        protected override string CreateUrl(string route=null)
        {
            return $"{base.CreateUrl()}/{ApiRoutes.BaseAndVersionV1}{ApiRoutes.Controller.UsersController}/{route}";
        }

        override public async Task RunTest()
        {
            var authenticatedUser = await AuthenticateAsync();

            //GetAll
            var getAllRes = await GetAll_RetunsOkRequest();
            var getAllResContent = await getAllRes.Content.ReadAsAsync<List<UserDetailsResponse>>();

            Assert.Equal(HttpStatusCode.OK, getAllRes.StatusCode);
            Assert.NotNull(getAllResContent);

            //GetByUsername
            var getNonExistantUser = await GetByUsername_WithNonExistentUsername_ReturnsNotFoundRequest(Guid.NewGuid().ToString());
            Assert.Equal(HttpStatusCode.NotFound, getNonExistantUser.StatusCode);

            var getValidUser = await GetByUsername_WithUsername_ReturnsOkRequest(authenticatedUser.Username);
            Assert.Equal(HttpStatusCode.OK, getValidUser.StatusCode);

            //Update
            var updateUserRes = await Update_WithUsername_ReturnsOkRequest(authenticatedUser.Username);
            var updateUserResContent = await updateUserRes.Content.ReadAsAsync<UserDetailsResponse>();

            Assert.Equal(HttpStatusCode.OK, updateUserRes.StatusCode);
            Assert.NotNull(updateUserResContent.Description);
            Assert.NotNull(updateUserResContent.ProfilePictureUrl);

            //Delete
            var deleteUserRes = await Delete_WithUsername_ReturnsOkRequest(authenticatedUser.Username);
            Assert.Equal(HttpStatusCode.OK, deleteUserRes.StatusCode);

        }

        public async Task<HttpResponseMessage> GetAll_RetunsOkRequest()
        {
            //arrange
            var url = CreateUrl();

            //act 
            var res = await TestClient.GetAsync(url);
            return res;
        }

        public async Task<HttpResponseMessage> GetByUsername_WithNonExistentUsername_ReturnsNotFoundRequest(string username)
        {
            //arrange
            var url = CreateUrl(ApiRoutes.Controller.RouteVariable.Username);
            url = url.Replace("{username}", username);

            //act 
            var res = await TestClient.GetAsync(url);
            return res;
        }

        public async Task<HttpResponseMessage> GetByUsername_WithUsername_ReturnsOkRequest(string username)
        {
            //arrange
            var url = CreateUrl(ApiRoutes.Controller.RouteVariable.Username);
            url = url.Replace("{username}", username);

            //act 
            var res = await TestClient.GetAsync(url);
            return res;
        }

        public async Task<HttpResponseMessage> Update_WithUsername_ReturnsOkRequest(string username)
        {
            //arrange
            var url = CreateUrl(ApiRoutes.Controller.RouteVariable.Username);
            url = url.Replace("{username}", username);

            UpdateUserDetailsRequest updateUserDetailsRequest = new UpdateUserDetailsRequest
            {
                Description = "desc" + Guid.NewGuid().ToString(),
                ProfilePictureUrl = "profilePic" + Guid.NewGuid().ToString(),
                Password = "psw" + Guid.NewGuid().ToString(),
            };

            //act 
            var res = await TestClient.PutAsJsonAsync(url, updateUserDetailsRequest);
            return res;
        }

        public async Task<HttpResponseMessage> Delete_WithUsername_ReturnsOkRequest(string username)
        {
            //arrange
            var url = CreateUrl(ApiRoutes.Controller.RouteVariable.Username);
            url = url.Replace("{username}", username);

            //act 
            var res = await TestClient.DeleteAsync(url);
            return res;
        }
    }
}
