using Blazored.LocalStorage;
using FluentAssertions;
using Matkakertomus.Client.Shared;
using Matkakertomus.Shared;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Matkakertomus.Client.Test
{
    public class UserInfoAreaTest : TestContext
    {
        [Fact]
        public async Task UserInfoArea_ContainsLoginAndRegisterLinksWhenNotAuthenticated()
        {
            // Arrange
            using var ctx = new TestContext();
            ctx.Services.AddScoped<StateContainer>();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetNotAuthorized();
            ctx.Services.AddSingleton<IMudPopoverService, MudPopoverService>();
            ctx.Services.AddSingleton<IScrollManager, ScrollManager>();
            ctx.Services.AddSingleton<IJsApiService, JsApiService>();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var localStorageService = new Mock<ILocalStorageService>();
            ctx.Services.AddSingleton(localStorageService.Object);
            var cut = ctx.RenderComponent<UserInfoArea>();
            var popoverProvider = ctx.RenderComponent<MudPopoverProvider>();

            var root = cut.Find("div");
            await root.TriggerEventAsync("onmouseenter", new MouseEventArgs());
            cut.WaitForState(() => popoverProvider.FindAll("div").Any(d => d.TextContent == "Kirjaudu sisään"));
            cut.WaitForState(() => popoverProvider.FindAll("div").Any(d => d.TextContent == "Rekisteröidy"));
        }

        [Fact]
        public async Task UserInfoArea_ContainsLogoutLinkWhenAuthenticated()
        {
            // Arrange
            using var ctx = new TestContext();
            ctx.Services.AddScoped<StateContainer>();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("Test");
            ctx.Services.AddSingleton<IMudPopoverService, MudPopoverService>();
            ctx.Services.AddSingleton<IScrollManager, ScrollManager>();
            ctx.Services.AddSingleton<IJsApiService, JsApiService>();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var localStorageService = new Mock<ILocalStorageService>();
            ctx.Services.AddSingleton(localStorageService.Object);
            var cut = ctx.RenderComponent<UserInfoArea>();
            var popoverProvider = ctx.RenderComponent<MudPopoverProvider>();

            var root = cut.Find("div");
            await root.TriggerEventAsync("onmouseenter", new MouseEventArgs());
            cut.WaitForState(() => popoverProvider.FindAll("div").Any(d => d.TextContent == "Kirjaudu ulos"));
        }

        [Fact]
        public async Task UserInfoArea_LoggingOutRedirectsToHomePage()
        {
            // Arrange
            using var ctx = new TestContext();
            ctx.Services.AddScoped<StateContainer>();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("Test");
            ctx.Services.AddSingleton<IMudPopoverService, MudPopoverService>();
            ctx.Services.AddSingleton<IScrollManager, ScrollManager>();
            ctx.Services.AddSingleton<IJsApiService, JsApiService>();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var localStorageService = new Mock<ILocalStorageService>();
            ctx.Services.AddSingleton(localStorageService.Object);
            var cut = ctx.RenderComponent<UserInfoArea>();
            var popoverProvider = ctx.RenderComponent<MudPopoverProvider>();

            var root = cut.Find("div");
            await root.TriggerEventAsync("onmouseenter", new MouseEventArgs());
            var logout = popoverProvider.FindAll("div").First(d => d.TextContent == "Kirjaudu ulos");
            logout.Click();

            var navMan = ctx.Services.GetRequiredService<FakeNavigationManager>();
            navMan.Uri.Should().Be("http://localhost/");
        }

        [Fact]
        public async Task UserInfoArea_ContainsProfileImageAndUsernameWhenAuthenticated()
        {
            using var ctx = new TestContext();
            ctx.Services.AddScoped<StateContainer>((x) =>
            {
                return new StateContainer { User = new ProfileDto { Username = "TestUser", Image = new byte[] { 0x1, 0x2, 0x3 } } };
            });
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("Test");
            ctx.Services.AddSingleton<IMudPopoverService, MudPopoverService>();
            ctx.Services.AddSingleton<IScrollManager, ScrollManager>();
            ctx.Services.AddSingleton<IJsApiService, JsApiService>();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var localStorageService = new Mock<ILocalStorageService>();
            ctx.Services.AddSingleton(localStorageService.Object);
            var cut = ctx.RenderComponent<UserInfoArea>();
            var popoverProvider = ctx.RenderComponent<MudPopoverProvider>();

            var root = cut.Find("div");
            await root.TriggerEventAsync("onmouseenter", new MouseEventArgs());
            cut.WaitForState(() => popoverProvider.Find("img") is not null);
            cut.WaitForState(() => popoverProvider.FindAll("p").Any(d => d.TextContent == "TestUser"));
        }


    }
}
