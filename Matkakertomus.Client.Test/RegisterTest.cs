using FluentAssertions;
using Matkakertomus.Client.Pages;
using Moq;
using MudBlazor;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http;

namespace Matkakertomus.Client.Test
{
	public class RegisterTest
	{

		[Fact]
		public void Register_HasSixInputsFieldsAndOneButton()
		{
			using var ctx = new TestContext();
			var snackbarService = new Mock<ISnackbar>();
			ctx.Services.AddSingleton(snackbarService.Object);
			var cut = ctx.RenderComponent<Register>();

			var inputs = cut.FindAll("input");
			var submitButton = cut.Find("button[type='submit']");

			inputs.Should().HaveCount(6);
			submitButton.Should().NotBeNull();
		}

		[Fact]
		public void Register_AllFieldsAreRequired()
		{
			using var ctx = new TestContext();
			var snackbarService = new Mock<ISnackbar>();
			ctx.Services.AddSingleton(snackbarService.Object);
			var cut = ctx.RenderComponent<Register>();

			var submitButton = cut.Find("button[type='submit']");
			submitButton.Click();
			cut.Markup.Should().Contain("Etunimi on pakollinen tieto");
			cut.Markup.Should().Contain("Sukunimi on pakollinen tieto");
			cut.Markup.Should().Contain("Käyttäjätunnus on pakollinen tieto");
			cut.Markup.Should().Contain("Email on pakollinen tieto");
			cut.Markup.Should().Contain("Salasana on pakollinen tieto");
			cut.Markup.Should().Contain("Salasanan varmistus on pakollinen tieto");
		}

		[Fact]
		public void Register_ValidSubmissionShowsSnackbarAndRedirectsToLogin()
		{
			using var ctx = new TestContext();
			var snackbarService = new Mock<ISnackbar>();
			ctx.Services.AddSingleton(snackbarService.Object);
			var http = ctx.Services.AddMockHttpClient();
			http.When(HttpMethod.Post, "http://localhost/api/auth/register").Respond(HttpStatusCode.OK);
			var cut = ctx.RenderComponent<Register>();
			var inputs = cut.FindAll("input");
			var inputData = new[] { "First", "Last", "User", "user@email.com", "secret", "secret" };
			var submitButton = cut.Find("button[type='submit']");

			for (int i = 0; i < inputs.Count; i++)
				inputs[i].Change(inputData[i]);

			submitButton.Click();
			snackbarService.Verify(x => x.Add("Rekisteröinti onnistui", Severity.Success, null, ""));
			var navMan = ctx.Services.GetRequiredService<FakeNavigationManager>();
			navMan.Uri.Should().Be("http://localhost/login");
		}
	}
}
