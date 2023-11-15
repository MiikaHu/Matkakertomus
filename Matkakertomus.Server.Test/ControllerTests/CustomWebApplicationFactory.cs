using Matkakertomus.Server.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Matkakertomus.Server.Test.ControllerTests
{
	public class CustomWebApplicationFactory : WebApplicationFactory<Program>
	{
		public Mock<IDestinationService> DestinationServiceMock { get; } = new Mock<IDestinationService>();
		public Mock<IAuthService> AuthServiceMock { get; } = new Mock<IAuthService>();
		public Mock<ITravellerService> TravellerServiceMock { get; } = new Mock<ITravellerService>();
		public Mock<ITripService> TripServiceMock { get; } = new Mock<ITripService>();

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			base.ConfigureWebHost(builder);

			builder.ConfigureServices(services =>
			{
				services.AddSingleton(DestinationServiceMock.Object);
				services.AddSingleton(AuthServiceMock.Object);
				services.AddSingleton(TravellerServiceMock.Object);
				services.AddSingleton(TripServiceMock.Object);
			});

		}
	}
}
