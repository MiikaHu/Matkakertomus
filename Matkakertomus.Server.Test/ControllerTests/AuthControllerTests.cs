using FluentAssertions;
using Matkakertomus.Shared;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace Matkakertomus.Server.Test.ControllerTests
{
    public class AuthControllerTests : IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public AuthControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }


        [Fact]
        public async Task Register_WithValidUser_ReturnsOk()
        {
            var mockRegistration = new UserRegisterDto { Email = "test@test.com", Username = "Test", ConfirmPassword = "secret", Password = "secret", FirstName = "Test", LastName = "Test" };
            _factory.AuthServiceMock.Setup(r => r.Register(It.IsAny<UserRegisterDto>())).ReturnsAsync(true);
            var response = await _client.PostAsync("/api/auth/register", JsonContent.Create(mockRegistration));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Register_WithTheSameEmail_ReturnsBadRequestWithErrorMessage()
        {
            var mockRegistration = new UserRegisterDto { Email = "test@test.com", Username = "Test", ConfirmPassword = "secret", Password = "secret", FirstName = "Test", LastName = "Test" };
            _factory.AuthServiceMock.Setup(r => r.Register(It.IsAny<UserRegisterDto>())).ReturnsAsync(false);
            var response = await _client.PostAsync("/api/auth/register", JsonContent.Create(mockRegistration));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Be("Email on jo käytössä");
        }

        [Fact]
        public async Task Register_WithInvalidUserData_ReturnsBadRequestWithErrorMessage()
        {
            var mockRegistration = new UserRegisterDto { };
            _factory.AuthServiceMock.Setup(r => r.Register(It.IsAny<UserRegisterDto>())).ReturnsAsync(false);
            var response = await _client.PostAsync("/api/auth/register", JsonContent.Create(mockRegistration));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().ContainAll("Email on pakollinen tieto", "Käyttäjätunnus on pakollinen tieto", "Salasana on pakollinen tieto");
        }

        [Fact]
        public async Task Login_With_ValidData_ReturnsJWTToken()
        {
            var mockLogin = new UserLoginDto { Email = "test@test.com", Password = "secret" };
            _factory.AuthServiceMock.Setup(r => r.Login(It.IsAny<UserLoginDto>())).ReturnsAsync("mocktoken");
            var response = await _client.PostAsync("/api/auth/login", JsonContent.Create(mockLogin));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Be("mocktoken");
        }

        [Fact]
        public async Task Login_WithInvalidData_ReturnBadRequestWithErrorMessage()
        {
            var mockLogin = new UserLoginDto { Email = "test@test.com", Password = "secret" };
            _factory.AuthServiceMock.Setup(r => r.Login(It.IsAny<UserLoginDto>())).ReturnsAsync((string?)null);
            var response = await _client.PostAsync("/api/auth/login", JsonContent.Create(mockLogin));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Be("Email tai salasana on väärin");
        }



        public void Dispose()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}
