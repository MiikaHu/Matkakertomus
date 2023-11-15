using FluentAssertions;
using Matkakertomus.Shared;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Matkakertomus.Server.Test.ControllerTests
{
	public class TripsControllerTests : IDisposable
	{
		private CustomWebApplicationFactory _factory;
		private HttpClient _client;
		private static string token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiSGFubmFSYXNpbGEiLCJJZCI6IjMiLCJleHAiOjE2ODkwOTUwMzJ9.pW-pdZpnuGSUPm56iF3a7niImyX4v2UU09An56V0UpZ8XQOXeGumM9fPr-kTyz3nLj9rLBgxEgdxatuQuOukAg";

		public TripsControllerTests()
		{
			_factory = new CustomWebApplicationFactory();
			_client = _factory.CreateClient();
		}

		public void Dispose()
		{
			_factory.Dispose();
			_client.Dispose();
		}

		[Fact]
		public async Task GetAllTripsAsync_ReturnsAllTrips()
		{
			var mockTrips = new TripDto[]
			{
				new()
				{
					TripId = 1, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(7), TravellerId = 1,
					Title = "TestTrip 1", Private = 0
				},
				new()
				{
					TripId = 2, StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.AddDays(14),
					TravellerId = 2, Title = "TestTrip 2", Private = 0
				}
			}.AsQueryable();

			_factory.TripServiceMock.Setup(r => r.GetAllTripsAsync()).ReturnsAsync(mockTrips);


			var request = new HttpRequestMessage(HttpMethod.Get, "/api/trips");
			request.Headers.Add("Authorization", $"Bearer {token}");
			request.Content = JsonContent.Create(mockTrips);

			var response = await _client.SendAsync(request);
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			var responseString = await response.Content.ReadAsStringAsync();
			var trips = JsonConvert.DeserializeObject<IEnumerable<TripDto>>(responseString);

			trips.Should().BeEquivalentTo(mockTrips);
		}

		[Fact]
		public async Task GetTripByIdAsync_WithValidId_ReturnsTrip()
		{
			var mockTrip = new TripDto
			{
				TripId = 1,
				StartDate = DateTime.UtcNow.Date,
				EndDate = DateTime.UtcNow.AddDays(7),
				TravellerId = 1,
				Title = "Test Trip 1",
				Private = 0
			};
			_factory.TripServiceMock.Setup(r => r.GetTripByIdAsync(1)).ReturnsAsync(mockTrip);
			var response = await _client.GetAsync("/api/trips/1");
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			var responseString = await response.Content.ReadAsStringAsync();
			var trip = JsonConvert.DeserializeObject<TripDto>(responseString);
			trip.Should().BeEquivalentTo(mockTrip);
		}

		[Fact]
		public async Task GetTripByIdAsync_WithInvalidId_ReturnsNotFound()
		{
			_factory.TripServiceMock.Setup(r => r.GetTripByIdAsync(1)).ReturnsAsync((TripDto?)null);
			var response = await _client.GetAsync("/api/trips/1");
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task CreateTripAsync_ReturnsCreatedTrip()
		{
			var mockTrip = new TripDto
			{
				TripId = 1,
				StartDate = DateTime.UtcNow.Date,
				EndDate = DateTime.UtcNow.AddDays(7),
				Title = "Test Title 1",
				TravellerId = 3,
				Private = 0
			};

			_factory.TripServiceMock.Setup(r => r.CreateTripAsync(It.IsAny<TripDto>())).ReturnsAsync(mockTrip);

			var request = new HttpRequestMessage(HttpMethod.Post, "/api/trips");
			request.Headers.Add("Authorization", $"Bearer {token}");
			request.Content = JsonContent.Create(mockTrip);

			var response = await _client.SendAsync(request);
			response.StatusCode.Should().Be(HttpStatusCode.Created);
			var responseString = await response.Content.ReadAsStringAsync();
			var trip = JsonConvert.DeserializeObject<TripDto>(responseString);
			trip.Should().BeEquivalentTo(mockTrip);
		}



	}
}