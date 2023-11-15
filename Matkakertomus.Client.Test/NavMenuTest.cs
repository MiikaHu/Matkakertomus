using Blazored.LocalStorage;
using FluentAssertions;
using Matkakertomus.Client.Shared;
using Moq;
using MudBlazor.Interop;
namespace Matkakertomus.Client.Test;

public class NavMenuTest
{

    [Fact]
    public void NavMenu_ContainsLogo()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.JSInterop.Setup<BoundingClientRect>("mudElementRef.getBoundingClientRect", _ => true);
        ctx.Services.AddScoped<StateContainer>();
        var authContext = ctx.AddTestAuthorization();

        var localStorageService = new Mock<ILocalStorageService>();
        ctx.Services.AddSingleton(localStorageService.Object);
        var cut = ctx.RenderComponent<NavMenu>();

        // Act
        var logo = cut.Find("img");

        // Assert
        logo.Should().NotBeNull();
        logo.GetAttribute("src").Should().Be("images/kuopionkulkijatlogo.png");
        logo.GetAttribute("height").Should().Be("150");
        logo.GetAttribute("width").Should().Be("150");
    }

    [Fact]
    public void NavMenu_ContainsTwoLinksWhenNotAuthenticated()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.Services.AddScoped<StateContainer>();
        ctx.JSInterop.Setup<BoundingClientRect>("mudElementRef.getBoundingClientRect", _ => true);
        var authContext = ctx.AddTestAuthorization();
        authContext.SetNotAuthorized();

        var localStorageService = new Mock<ILocalStorageService>();
        ctx.Services.AddSingleton(localStorageService.Object);
        var cut = ctx.RenderComponent<NavMenu>();

        // Act
        var links = cut.FindAll("a");

        // Assert
        links.Should().HaveCount(2);
        links[0].GetAttribute("href").Should().Be("/");
        links[1].GetAttribute("href").Should().Be("/destinations");

    }

    [Fact]
    public void NavMenu_ContainsMoreThanTwoLinksWhenAuthenticated()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.Services.AddScoped<StateContainer>();
        ctx.JSInterop.Setup<BoundingClientRect>("mudElementRef.getBoundingClientRect", _ => true);
        var authContext = ctx.AddTestAuthorization();
        authContext.SetAuthorized("TEST USER");

        var localStorageService = new Mock<ILocalStorageService>();
        ctx.Services.AddSingleton(localStorageService.Object);
        var cut = ctx.RenderComponent<NavMenu>();

        // Act
        var links = cut.FindAll("a");

        // Assert
        links.Should().HaveCount(6);
        links[0].GetAttribute("href").Should().Be("/");
        links[1].GetAttribute("href").Should().Be("/destinations");
        links[2].GetAttribute("href").Should().Be("/grouptrips");
        links[3].GetAttribute("href").Should().Be("/mytrips");
        links[4].GetAttribute("href").Should().Be("/profile");
        links[5].GetAttribute("href").Should().Be("/members");




    }
}

