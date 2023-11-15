using FluentAssertions;
using Matkakertomus.Client.Pages;
using Matkakertomus.Shared;
using RichardSzalay.MockHttp;
using System.Net.Http;

namespace Matkakertomus.Client.Test
{
    public class MembersTest
    {
        [Fact]
        public void Members_GridContainsCorrectMemberInformation()
        {
            // Arrange
            using var ctx = new TestContext();

            var mockMembers = new ProfileDto[]
            {
                new()
                {
                    Area = "Area 1", Description = "Description 1", Email = "test1@test.com", FirstName = "FirstName 1",
                    LastName = "LastName 1"
                },
                new()
                {
                    Area = "Area 2", Description = "Description 2", Email = "test2@test.com", FirstName = "FirstName 2",
                    LastName = "LastName 2"
                },
            };
            var mock = ctx.Services.AddMockHttpClient();
            var req = mock.When(HttpMethod.Get, "http://localhost/api/travellers").RespondJson(mockMembers);
            var cut = ctx.RenderComponent<Members>();
            cut.WaitForAssertion(() => cut.FindAll(".mud-grid").Count.Should().Be(1));
            cut.WaitForAssertion(() => cut.FindAll(".mud-card").Count.Should().Be(2));
            cut.WaitForAssertion(() => cut.FindAll("[style*= \"background-image\"]").Count.Should().Be(2));

            cut.WaitForAssertion(() => cut.Markup.Should().ContainAll("Area 1", "Area 2", "FirstName 1", "FirstName 2", "LastName 1", "LastName 2", "Area 1", "Area 2"));
        }

        [Fact]
        public void Members_DoesNotContainEmail()
        {
            // Arrange
            using var ctx = new TestContext();

            var mockMembers = new ProfileDto[]
            {
                new()
                {
                    Area = "Area 1", Description = "Description 1", Email = "test1@test.com", FirstName = "FirstName 1",
                    LastName = "LastName 1"
                },
                new()
                {
                    Area = "Area 2", Description = "Description 2", Email = "test2@test.com", FirstName = "FirstName 2",
                    LastName = "LastName 2"
                },
            };
            var mock = ctx.Services.AddMockHttpClient();
            var req = mock.When(HttpMethod.Get, "http://localhost/api/travellers").RespondJson(mockMembers);
            var cut = ctx.RenderComponent<Members>();

            cut.WaitForAssertion(() => cut.Markup.Should().NotContainAny("test@test.com", "test2@test.com"));
        }
    }


}
