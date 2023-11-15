using Blazored.LocalStorage;
using FluentAssertions;
using Matkakertomus.Client.Pages;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using MudBlazor;
using RichardSzalay.MockHttp;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Matkakertomus.Client.Test
{

    public class LoginTest
    {

        [Fact]
        public void LoginForm_HasTwoInputFieldsAndLoginButton()
        {
            // Arrange
            using var ctx = new TestContext();
            ctx.Services.AddScoped<StateContainer>();
            var localStorageService = new Mock<ILocalStorageService>();
            var snackbarService = new Mock<ISnackbar>();
            ctx.Services.AddSingleton(localStorageService.Object);
            ctx.Services.AddSingleton(snackbarService.Object);

            var cut = ctx.RenderComponent<Login>();

            // Act
            var emailInput = cut.Find("input[type='text']");
            var passwordInput = cut.Find("input[type='password']");
            var submitButton = cut.Find("button[type='submit']");

            // Assert
            emailInput.Should().NotBeNull();
            passwordInput.Should().NotBeNull();
            submitButton.Should().NotBeNull();
        }

        [Fact]
        public async Task LoginForm_SubmitWithValidDataSendsARequest()
        {
            using var ctx = new TestContext();
            var mock = ctx.Services.AddMockHttpClient();
            var req = mock.When(HttpMethod.Post, "http://localhost/api/auth/login").Respond(HttpStatusCode.Accepted);

            ctx.Services.AddScoped<StateContainer>();
            var localStorageService = new Mock<ILocalStorageService>();
            var snackbarService = new Mock<ISnackbar>();
            ctx.Services.AddSingleton(localStorageService.Object);
            ctx.Services.AddSingleton(snackbarService.Object);



            var cut = ctx.RenderComponent<Login>();

            // Act
            var emailInput = cut.Find("input[type='text']");
            emailInput.Change("test@test.com");

            var passwordInput = cut.Find("input[type='password']");
            passwordInput.Change("test");
            var submitButton = cut.Find("button[type='submit']");

            await submitButton.ClickAsync(new MouseEventArgs());

            var matchCount = mock.GetMatchCount(req);
            matchCount.Should().Be(1);
        }


        [Fact]
        public async Task LoginForm_SubmitWithNoDataShowsAnError()
        {
            using var ctx = new TestContext();

            ctx.Services.AddScoped<StateContainer>();
            var localStorageService = new Mock<ILocalStorageService>();
            var snackbarService = new Mock<ISnackbar>();
            ctx.Services.AddSingleton(localStorageService.Object);
            ctx.Services.AddSingleton(snackbarService.Object);



            var cut = ctx.RenderComponent<Login>();

            // Act
            var submitButton = cut.Find("button[type='submit']");

            await submitButton.ClickAsync(new MouseEventArgs());


            cut.WaitForState(() => cut.FindAll("div").Any(p => p.TextContent == "Salasana on pakollinen tieto"), TimeSpan.FromSeconds(2));
            cut.WaitForState(() => cut.FindAll("div").Any(p => p.TextContent == "Email on pakollinen tieto"), TimeSpan.FromSeconds(2));

        }


        [Fact]
        public async Task LoginForm_SubmitWithInvalidCredentialsShowsAnError()
        {
            using var ctx = new TestContext();
            var mock = ctx.Services.AddMockHttpClient();
            var errorMessage = "Email tai salasana väärin";
            var content = new StringContent(errorMessage, Encoding.UTF8, "text/plain");
            var req = mock.When(HttpMethod.Post, "http://localhost/api/auth/login").Respond(HttpStatusCode.BadRequest, content);
            ctx.Services.AddScoped<StateContainer>();

            var localStorageService = new Mock<ILocalStorageService>();
            var snackbarService = new Mock<ISnackbar>();
            ctx.Services.AddSingleton(localStorageService.Object);
            ctx.Services.AddScoped<ISnackbar>(sp => snackbarService.Object);

            var cut = ctx.RenderComponent<Login>();

            // Act
            var emailInput = cut.Find("input[type='text']");
            emailInput.Change("test@test.com");

            var passwordInput = cut.Find("input[type='password']");
            passwordInput.Change("test");
            var submitButton = cut.Find("button[type='submit']");

            await submitButton.ClickAsync(new MouseEventArgs());

            snackbarService.Verify(x => x.Add(
                "Email tai salasana väärin",
                Severity.Error,
                null,
                ""
            ), Times.Once);
        }


    }


}

