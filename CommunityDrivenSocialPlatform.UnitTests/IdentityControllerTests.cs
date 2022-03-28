
using System.Threading.Tasks;
using System.Net;
using Xunit;
using Xunit.Extensions;
using CDSP_API.Contracts.V1.Requests;
using CDSP_API.Contracts;
using System.Net.Http;
using System;
using Xunit.Extensions.Ordering;
using CDSP_API.Data;

namespace CommunityDrivenSocialPlatform.UnitTests
{
    public class IdentityControllerTests : IntegrationTests
    {
        private string postfix =null;

        protected override string CreateUrl(string route)
        {
            return $"{base.CreateUrl()}/{ApiRoutes.BaseAndVersionV1}{ApiRoutes.Controller.IdentityController}/{route}";
        }

        override public async Task RunTest()
        {
            //signup new user with valid email, username and password
            (var signupWithValidEmailUsernameAndPassword, var signupRequest) = await Signup_WithNonAlreadyExistingEmailAndUsername_ReturnsOkRequest();
            var signupWithValidEmailUsernameAndPasswordContent = signupWithValidEmailUsernameAndPassword.Content.ReadAsAsync<AuthResult>().Result;
            
            Assert.Equal(HttpStatusCode.OK, signupWithValidEmailUsernameAndPassword.StatusCode);
            Assert.NotNull(signupWithValidEmailUsernameAndPasswordContent.Token);
            Assert.NotNull(signupWithValidEmailUsernameAndPasswordContent.RefreshToken);

            //signup new user with already existing email and username
            var signupWithAlreadyExistingUsernameAndUsername = await Signup_WithAlreadyExistingEmailAndUsername_ReturnsBadRequest();
            
            Assert.Equal(HttpStatusCode.BadRequest, signupWithAlreadyExistingUsernameAndUsername.StatusCode);

            //signup new user with already existing email
            var signupWithAlreadyExistingEmail = await Signup_WithAlreadyExistingEmail_ReturnsBadRequest();
            
            Assert.Equal(HttpStatusCode.BadRequest, signupWithAlreadyExistingEmail.StatusCode);

            //signup new user with already existing username
            var signupWithAlreadyExistingUsername= await Signup_WithAlreadyExistingEmail_ReturnsBadRequest();

            Assert.Equal(HttpStatusCode.BadRequest, signupWithAlreadyExistingUsername.StatusCode);

            //signin user with valid username and password
            var signinWithValidUsernameAndPassword = await Signin_WithValidUsernameAndPassword_ReturnsOkRequest(signupRequest.Username);
            var signinWithValidUsernameAndPasswordContent = signinWithValidUsernameAndPassword.Content.ReadAsAsync<AuthResult>().Result;

            Assert.Equal(HttpStatusCode.OK, signinWithValidUsernameAndPassword.StatusCode);
            Assert.NotNull(signinWithValidUsernameAndPasswordContent.Token);
            Assert.NotNull(signinWithValidUsernameAndPasswordContent.RefreshToken);

            //signin user with valid username and invalid password
            var signinWithValidUsernameAndInvalidPassword = await Signin_WithInvalidPassword_ReturnsNotFoundRequest(signupRequest.Username, Guid.NewGuid().ToString());
            Assert.Equal(HttpStatusCode.NotFound, signinWithValidUsernameAndInvalidPassword.StatusCode);

            //signin user with invalid username 
            var signinWithInValidUsername = await Signin_WithInvalidUsername_ReturnsNotFoundRequest(signupRequest.Username+Guid.NewGuid().ToString());
            Assert.Equal(HttpStatusCode.NotFound, signinWithInValidUsername.StatusCode);

        }

        public async Task<HttpResponseMessage> Signin_WithValidUsernameAndPassword_ReturnsOkRequest(string username)
        {
           
            //arrange
            var signinRequest = new SigninRequest
            {
                Username = username,
                Password = "testing",
            };

            //act 
            var res = await TestClient.PostAsJsonAsync(CreateUrl(ApiRoutes.Controller.Action.Signin), signinRequest);
            return res;
        }

        public async Task<HttpResponseMessage> Signin_WithInvalidPassword_ReturnsNotFoundRequest(string username, string password)
        {
            //arrange
            var signinRequest = new SigninRequest
            {
                Username = username,
                Password = password,
            };

            //act 
            var res = await TestClient.PostAsJsonAsync(CreateUrl(ApiRoutes.Controller.Action.Signin), signinRequest);
            return res;
        }

        public async Task<HttpResponseMessage> Signin_WithInvalidUsername_ReturnsNotFoundRequest(string username)
        {
            //arrange
            var signinRequest = new SigninRequest
            {
                Username = username,
                Password = "testing",
            };

            //act 
            var res = await TestClient.PostAsJsonAsync(CreateUrl(ApiRoutes.Controller.Action.Signin), signinRequest);
            return res;
        }

        public async Task<(HttpResponseMessage, SignupRequest)> Signup_WithNonAlreadyExistingEmailAndUsername_ReturnsOkRequest()
        {
            postfix = Guid.NewGuid().ToString();

            //arrange
            var signupRequest = new SignupRequest
            {
                EmailAddress = $"test{postfix}@intergratisdon.com",
                Username = $"testing{postfix}",
                Password = "testing",
            };

            //act 
            var res = await TestClient.PostAsJsonAsync(CreateUrl(ApiRoutes.Controller.Action.Signup), signupRequest);
            return (res, signupRequest);
        }

        public async Task<HttpResponseMessage> Signup_WithAlreadyExistingEmailAndUsername_ReturnsBadRequest()
        {
            //arrange
            var signupRequest = new SignupRequest
            {
                EmailAddress = $"test{postfix}@intergratisdon.com",
                Username = $"testing{postfix}",
                Password = "testing",
            };

            //act 
            var res = await TestClient.PostAsJsonAsync(CreateUrl(ApiRoutes.Controller.Action.Signup), signupRequest);

            return res;
        }

        public async Task<HttpResponseMessage> Signup_WithAlreadyExistingEmail_ReturnsBadRequest()
        {
            //arrange
            var signupRequest = new SignupRequest
            {
                EmailAddress = $"test{postfix}@intergratisdon.com",
                Username = $"testing{Guid.NewGuid()}",
                Password = "testing",
            };

            //act 
            var res = await TestClient.PostAsJsonAsync(CreateUrl(ApiRoutes.Controller.Action.Signup), signupRequest);

            return res;
        }

        public async Task<HttpResponseMessage> Signup_WithAlreadyExistingUsername_ReturnsBadRequest()
        {
            //arrange
            var signupRequest = new SignupRequest
            {
                EmailAddress = $"test{Guid.NewGuid()}@intergratisdon.com",
                Username = $"testing{postfix}",
                Password = "testing",
            };

            //act 
            var res = await TestClient.PostAsJsonAsync(CreateUrl(ApiRoutes.Controller.Action.Signup), signupRequest);

            return res;
        }


    }
}
