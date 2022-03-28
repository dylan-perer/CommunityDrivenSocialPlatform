
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CDSP_API;
using CDSP_API.Data;
using CDSP_API.Contracts.V1.Requests;
using CDSP_API.Contracts;

namespace CommunityDrivenSocialPlatform.UnitTests
{
    public class IntegrationTests
    {
        protected readonly HttpClient TestClient;
        public DataContext dataContext;
        protected IntegrationTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));
                        services.Remove(descriptor);
                        services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("TestDb"));

                    });
                });

            TestClient = appFactory.CreateClient();
        }

        public virtual async Task RunTest()
        {
        }

        protected async Task<UserCredentials> AuthenticateAsync()
        {
            var credentials = await SignupTestAccount();
            if (credentials is null)
            {
                credentials = await SigninTestAccount();
            }
            
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", credentials.Token);
            return credentials;
        }

        private async Task<UserCredentials> SignupTestAccount()
        {
            AuthResult signupRespons = null;

            var request = new SignupRequest
            {
                EmailAddress = "testing@intergation.com",
                Username = "testing",
                Password = "testing"
            };
            var response = await TestClient.PostAsJsonAsync("https://localhost:44348/api/v1/identity/signup",
                request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                signupRespons = await response.Content.ReadAsAsync<AuthResult>();
                return new UserCredentials
                {
                    Username = request.Username,
                    Password = request.Password,
                    Token = signupRespons.Token,
                    RefreshToken = signupRespons.RefreshToken
                };
            }
            return null;
        }

        private async Task<UserCredentials> SigninTestAccount()
        {
            AuthResult singinResponse = null;

            var request = new SigninRequest
            {
                Username = "testing",
                Password = "testing"
            };
            var response = await TestClient.PostAsJsonAsync("https://localhost:44348/api/v1/identity/signin",
                request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                singinResponse = await response.Content.ReadAsAsync<AuthResult>();
                return new UserCredentials
                {
                    Username = request.Username,
                    Password = request.Password,
                    Token = singinResponse.Token,
                    RefreshToken = singinResponse.RefreshToken
                };
            }
            return null;
        }

        protected virtual string CreateUrl(string route = null)
        {
            return "https://localhost:44348";
        }

        protected class UserCredentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Token { get; set; }
            public string RefreshToken { get; set; }

        }

    }
}
