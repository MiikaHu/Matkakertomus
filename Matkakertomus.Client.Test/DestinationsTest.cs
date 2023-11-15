using AngleSharp.Dom;
using FluentAssertions;
using Matkakertomus.Client.Pages;
using Matkakertomus.Shared;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using MudBlazor;
using RichardSzalay.MockHttp;
using System;
using System.Net.Http;

namespace Matkakertomus.Client.Test
{
    public class DestinationsTest
    {
        // Not sure if this is a valid way to share the TestContext between tests, but it works in this case.
        private readonly TestContext ctx = new TestContext();
        private readonly TestAuthorizationContext authContext;
        private readonly Mock<ISnackbar> snackbarServiceMock = new Mock<ISnackbar>();

        public DestinationsTest()
        {
            var dialogServiceMock = new Mock<IDialogService>();
            ctx.Services.AddSingleton(dialogServiceMock.Object);
            ctx.Services.AddSingleton(snackbarServiceMock.Object);
            authContext = ctx.AddTestAuthorization();
        }


        [Fact]
        public void Destinations_ContainsATableOfDestinations()
        {

            authContext.SetAuthorized("Test user");
            var mockDestinations = new DestinationDto[]
            {
                new()
                {
                    DestinationId = 1,
                    DestinationName = "Helsinki",
                    Country = "Finland",
                    Area = "Uusimaa",
                    Description = "The capital and largest city of Finland.",
                },
                new ()
                {
                    DestinationId = 2,
                    DestinationName = "Turku",
                    Country = "Finland",
                    Area = "Southwest Finland",
                    Description = "A city on the southwest coast of Finland.",
                },
                new()
                {
                    DestinationId = 3,
                    DestinationName = "Tampere",
                    Country = "Finland",
                    Area = "Pirkanmaa",
                    Description = "A city in southern Finland.",
                }
            };
            var mockStories = new StoryDto[]
                {new StoryDto {DestinationId = 1, Date = DateTime.Now, StoryId = 1, TripId = 1, Text = "Test Story"}};

            var mock = ctx.Services.AddMockHttpClient();
            var reqDestinations = mock.When(HttpMethod.Get, "http://localhost/api/Destinations").RespondJson(mockDestinations);
            var reqStories = mock.When(HttpMethod.Get, "http://localhost/api/Stories").RespondJson(mockDestinations);

            var cut = ctx.RenderComponent<Destinations>();
            cut.FindAll(".mud-table").Count.Should().Be(1);
            cut.FindAll("tbody").Count.Should().Be(1);
            cut.FindAll("th").Count.Should().Be(7);
            cut.WaitForAssertion(() => cut.Markup.Should().ContainAll("Turku", "Finland", "Pirkanmaa", "A city in southern Finland."));
        }

        [Fact]
        public void Destinations_ContainsAFormToAddNewDestinationWhenAuthorized()
        {

            authContext.SetAuthorized("Test user");
            var mockDestinations = new DestinationDto[]
            {
                new()
                {
                    DestinationId = 1,
                    DestinationName = "Helsinki",
                    Country = "Finland",
                    Area = "Uusimaa",
                    Description = "The capital and largest city of Finland.",
                },
                new ()
                {
                    DestinationId = 2,
                    DestinationName = "Turku",
                    Country = "Finland",
                    Area = "Southwest Finland",
                    Description = "A city on the southwest coast of Finland.",
                },
                new()
                {
                    DestinationId = 3,
                    DestinationName = "Tampere",
                    Country = "Finland",
                    Area = "Pirkanmaa",
                    Description = "A city in southern Finland.",
                }
            };

            var mock = ctx.Services.AddMockHttpClient();
            var req = mock.When(HttpMethod.Get, "http://localhost/api/Destinations").RespondJson(mockDestinations);
            var cut = ctx.RenderComponent<Destinations>();

            cut.FindAll("input").Count.Should().Be(5);
        }

        [Fact]
        public void Destinations_DoesNotContainAFormToAddNewDestinationWhenNotAuthorized()
        {

            authContext.SetNotAuthorized();
            var mockDestinations = new DestinationDto[]
            {
                new()
                {
                    DestinationId = 1,
                    DestinationName = "Helsinki",
                    Country = "Finland",
                    Area = "Uusimaa",
                    Description = "The capital and largest city of Finland.",
                },
                new ()
                {
                    DestinationId = 2,
                    DestinationName = "Turku",
                    Country = "Finland",
                    Area = "Southwest Finland",
                    Description = "A city on the southwest coast of Finland.",
                },
                new()
                {
                    DestinationId = 3,
                    DestinationName = "Tampere",
                    Country = "Finland",
                    Area = "Pirkanmaa",
                    Description = "A city in southern Finland.",
                }
            };

            var mock = ctx.Services.AddMockHttpClient();
            var req = mock.When(HttpMethod.Get, "http://localhost/api/Destinations").RespondJson(mockDestinations);
            var cut = ctx.RenderComponent<Destinations>();

            cut.FindAll("input").Should().BeEmpty();
        }

        [Fact]
        public void Destinations_DeleteAndEditButtonsAreDisabledWhenNotAuthorized()
        {

            authContext.SetNotAuthorized();
            var mockDestinations = new DestinationDto[]
            {
                new()
                {
                    DestinationId = 1,
                    DestinationName = "Helsinki",
                    Country = "Finland",
                    Area = "Uusimaa",
                    Description = "The capital and largest city of Finland.",
                },
                new ()
                {
                    DestinationId = 2,
                    DestinationName = "Turku",
                    Country = "Finland",
                    Area = "Southwest Finland",
                    Description = "A city on the southwest coast of Finland.",
                },
                new()
                {
                    DestinationId = 3,
                    DestinationName = "Tampere",
                    Country = "Finland",
                    Area = "Pirkanmaa",
                    Description = "A city in southern Finland.",
                }
            };

            var mock = ctx.Services.AddMockHttpClient();
            var req = mock.When(HttpMethod.Get, "http://localhost/api/Destinations").RespondJson(mockDestinations);
            var cut = ctx.RenderComponent<Destinations>();
            cut.FindAll("button[type='submit']").Should().OnlyContain(b => b.IsDisabled());
        }


        [Fact]
        public void Destinations_AddingNewDestinationWithValidDataSucceeds()
        {
            var mockDestinations = new DestinationDto[]
            {
                new()
                {
                    DestinationId = 1,
                    DestinationName = "Helsinki",
                    Country = "Finland",
                    Area = "Uusimaa",
                    Description = "The capital and largest city of Finland.",
                },
                new ()
                {
                    DestinationId = 2,
                    DestinationName = "Turku",
                    Country = "Finland",
                    Area = "Southwest Finland",
                    Description = "A city on the southwest coast of Finland.",
                },
                new()
                {
                    DestinationId = 3,
                    DestinationName = "Tampere",
                    Country = "Finland",
                    Area = "Pirkanmaa",
                    Description = "A city in southern Finland.",
                }
            };

            var newDestination = new DestinationDto()
            {
                DestinationName = "NewDestination",
                Country = "NewCountry",
                Area = "NewArea",
                Description = "NewDescription",
            };
            authContext.SetAuthorized("Test user");
            var mock = ctx.Services.AddMockHttpClient();

            var get = mock.When(HttpMethod.Get, "http://localhost/api/Destinations").RespondJson(mockDestinations);
            var post = mock.When(HttpMethod.Post, "http://localhost/api/Destinations").RespondJson(newDestination);

            var cut = ctx.RenderComponent<Destinations>();
            var inputs = new[] { "DestinationName", "Country", "Area", "Description" };
            var fields = cut.FindAll("input");

            int index = 0;
            foreach (string input in inputs)
                fields[index++].Change(input);


            cut.Find("button[type='submit']").Click(new MouseEventArgs());
            mock.GetMatchCount(post).Should().Be(1);


            snackbarServiceMock.Verify(x => x.Add(
                "Matkakohteen lisäys onnistui",
                Severity.Success,
                null,
                ""
            ), Times.Once);


        }

    }
}
