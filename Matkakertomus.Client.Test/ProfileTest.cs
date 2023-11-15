using AngleSharp.Html.Dom;
using FluentAssertions;
using Matkakertomus.Client.Pages;
using Matkakertomus.Shared;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using MudBlazor;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http;

namespace Matkakertomus.Client.Test
{
	public class ProfileTest
	{

		// Not sure if this is a valid way to share the TestContext between tests, but it works in this case.
		private readonly TestContext ctx = new TestContext();
		private readonly TestAuthorizationContext authContext;
		private readonly Mock<ISnackbar> snackbarServiceMock = new Mock<ISnackbar>();

		public ProfileTest()
		{
			ctx.Services.AddSingleton(snackbarServiceMock.Object);
			authContext = ctx.AddTestAuthorization();
			ctx.Services.AddScoped<StateContainer>();
		}

		[Fact]
		public void Profile_ContainsFieldsPopulatedWithExistingDataOnEntry()
		{
			var mockProfile = new ProfileDto
			{
				FirstName = "John",
				LastName = "Doe",
				Username = "johndoe",
				Email = "johndoe@example.com",
				Area = "Test area 1",
				Description = "Test description 1",
				Image = null
			};
			var mock = ctx.Services.AddMockHttpClient();
			var req = mock.When(HttpMethod.Get, "http://localhost/api/Travellers/profile").RespondJson(mockProfile);
			var cut = ctx.RenderComponent<Profile>();

			var inputs = cut.FindAll("input");
			var data = new[] { "John", "Doe", "johndoe", "johndoe@example.com", "Test description 1", "Test area 1", "" };

			int index = 0;
			foreach (IHtmlInputElement input in inputs)
				input.Value.Should().Be(data[index++]);
		}

		[Fact]
		public void Profile_UpdatingProfileWithValidDataSucceeds()
		{
			var mockProfile = new ProfileDto
			{
				FirstName = "John",
				LastName = "Doe",
				Username = "johndoe",
				Email = "johndoe@example.com",
				Area = "Test area 1",
				Description = "Test description 1",
				Image = null
			};

			var updatedProfile = new ProfileDto
			{
				FirstName = "Jane",
				LastName = "Doe",
				Username = "janedoe",
				Email = "janedoe@example.com",
				Area = "Test area 2",
				Description = "Test description 2",
				Image = null
			};
			var http = ctx.Services.AddMockHttpClient();
			var get = http.When(HttpMethod.Get, "http://localhost/api/Travellers/profile").RespondJson(mockProfile);
			var post = http.When(HttpMethod.Put, "http://localhost/api/Travellers/profile").Respond(HttpStatusCode.OK);
			var cut = ctx.RenderComponent<Profile>();

			var inputData = new[] { "Jane", "Doe", "janedoe", "janedoe@example.com", "Test description 2", "Test area 2", "" };
			var inputs = cut.FindAll("input[type='text']");

			int index = 0;
			foreach (var input in inputs)
				input.Change(inputData[index++]);

			cut.Find("button[type='submit']").Click(new MouseEventArgs());

			snackbarServiceMock.Verify(x => x.Add(
				"Profiili päivitetty",
				Severity.Success,
				null,
				""
			), Times.Once);

			http.GetMatchCount(post).Should().Be(1);
		}
	}
}
