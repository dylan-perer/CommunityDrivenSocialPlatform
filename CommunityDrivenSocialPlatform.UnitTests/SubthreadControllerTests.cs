using CDSP_API.Contracts;
using CDSP_API.Contracts.V1.Requests;
using CDSP_API.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

namespace CommunityDrivenSocialPlatform.UnitTests
{
    public class SubthreadControllerTests : IntegrationTests
    {
        protected override string CreateUrl(string route = null)
        {
            return $"{base.CreateUrl()}/{ApiRoutes.BaseAndVersionV1}{ApiRoutes.Controller.SubThreadController}/{route}";
        }

        override public async Task RunTest()
        {
            var authenticatedUser = await AuthenticateAsync();

            //get all
            var getAllRes = await GetAll_RetunsOkRequest();
            var getAllResContent = await getAllRes.Content.ReadAsAsync<List<SubThreadDetailsResponse>>();
            Assert.Equal(HttpStatusCode.OK, getAllRes.StatusCode);
            Assert.NotNull(getAllResContent);

            //create
            var createRes = await Create_ReturnsOkRequest();
            var createResContent = await createRes.Content.ReadAsAsync<SubThreadDetailsResponse>();
            Assert.Equal(HttpStatusCode.OK, createRes.StatusCode);
            Assert.NotNull(createResContent);

            //get by name
            var getByNameRes = await GetByName_RetunsOkRequest("testing_subthread");
            var getByNameResContent = await getByNameRes.Content.ReadAsAsync<SubThreadDetailsResponse>();
            Assert.Equal(HttpStatusCode.OK, getByNameRes.StatusCode);
            Assert.NotNull(getByNameResContent);

            //get users in subthread
            var getUsersRes = await GetSubThreadUsers_ReturnsOkRequest("testing_subthread");
            var getUsersResContent = await getUsersRes.Content.ReadAsAsync<List<UserDetailsResponse>>();
            Assert.Equal(HttpStatusCode.OK, getUsersRes.StatusCode);
            Assert.NotNull(getUsersResContent);

            //update
            var updateRes = await Update_ReturnsOkRequest("testing_subthread");
            Assert.Equal(HttpStatusCode.OK, updateRes.StatusCode);

            //leave subthread
            var leaveSubthreadRes = await leave_RetunsOkRequest("testing_subthread");
            Assert.Equal(HttpStatusCode.OK, leaveSubthreadRes.StatusCode);

            //join subthread
            var joinSubthreadRes = await Join_RetunsOkRequest("testing_subthread");
            Assert.Equal(HttpStatusCode.OK, joinSubthreadRes.StatusCode);

            //delete
            var deleteRes = await Delete_RetunsOkRequest("testing_subthread");
            Assert.Equal(HttpStatusCode.OK, deleteRes.StatusCode);

        }

        public async Task<HttpResponseMessage> GetAll_RetunsOkRequest()
        {
            //arrange
            var url = CreateUrl();

            //act 
            var res = await TestClient.GetAsync(url);
            return res;
        }

        public async Task<HttpResponseMessage> GetByName_RetunsOkRequest(string name)
        {
            //arrange
            var url = CreateUrl(ApiRoutes.Controller.RouteVariable.SubThreadName);
            url = url.Replace("{name}", name);

            //act 
            var res = await TestClient.GetAsync(url);
            return res;
        }

        public async Task<HttpResponseMessage> Create_ReturnsOkRequest()
        {
            //arrange
            var url = CreateUrl();
            CreateSubThreadRequest createSubThreadRequest = new CreateSubThreadRequest
            {
                Name = "testing_subthread",
                Description = "desc",
                WelcomeMessage = "Welcome msg"
            };

            //act 
            var res = await TestClient.PostAsJsonAsync(url, createSubThreadRequest);
            return res;
        }

        public async Task<HttpResponseMessage> Update_ReturnsOkRequest(string name)
        {
            //arrange
            var url = CreateUrl(ApiRoutes.Controller.RouteVariable.SubThreadName);
            url = url.Replace("{name}", name);
            UpdateSubThreadRequest updateSubThreadRequest = new UpdateSubThreadRequest
            {
                Description = "desc"+Guid.NewGuid().ToString(),
                WelcomeMessage = "Welcome msg"+Guid.NewGuid().ToString()
            };

            //act 
            var res = await TestClient.PutAsJsonAsync(url, updateSubThreadRequest);
            return res;
        }


        public async Task<HttpResponseMessage> GetSubThreadUsers_ReturnsOkRequest(string name)
        {
            //arrange
            var url = CreateUrl(ApiRoutes.Controller.RouteVariable.SubThreadUsers);
            url = url.Replace("{name}", name);

            //act 
            var res = await TestClient.GetAsync(url);
            return res;
        }

        public async Task<HttpResponseMessage> Join_RetunsOkRequest(string name)
        {
            //arrange
            var url = CreateUrl(ApiRoutes.Controller.RouteVariable.SubThreadJoin);
            url = url.Replace("{name}", name);

            //act 
            var res = await TestClient.GetAsync(url);
            return res;
        }

        public async Task<HttpResponseMessage> leave_RetunsOkRequest(string name)
        {
            //arrange
            var url = CreateUrl(ApiRoutes.Controller.RouteVariable.SubThreadLeave);
            url = url.Replace("{name}", name);

            //act 
            var res = await TestClient.GetAsync(url);
            return res;
        }

        public async Task<HttpResponseMessage> Delete_RetunsOkRequest(string name)
        {
            //arrange
            var url = CreateUrl(ApiRoutes.Controller.RouteVariable.SubThreadName);
            url = url.Replace("{name}", name);

            //act 
            var res = await TestClient.DeleteAsync(url);
            return res;
        }

    }
}
