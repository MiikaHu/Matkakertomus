using FluentAssertions;
using Matkakertomus.Shared;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Matkakertomus.Server.Test.ControllerTests
{
    public class TravellerControllerTests : IDisposable
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public TravellerControllerTests()
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
        public async Task GetAllTravellersAsync_ReturnsAllTravellers()
        {
            var mockTravellers = new List<ProfileDto>
            {
                new()
                {
                    FirstName = "John", LastName = "Doe", Username = "johndoe", Email = "johndoe@example.com",
                    Area = "Test area 1", Description = "Test description 1"
                },
                new()
                {
                    FirstName = "Jane", LastName = "Doe", Username = "janedoe", Email = "janedoe@example.com",
                    Area = "Test area 2", Description = "Test description 2"
                }
            }.AsQueryable();

            _factory.TravellerServiceMock.Setup(r => r.GetAllTravellersAsync()).ReturnsAsync(mockTravellers);

            var response = await _client.GetAsync("/api/travellers");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseString = await response.Content.ReadAsStringAsync();
            var travellers = JsonConvert.DeserializeObject<IEnumerable<ProfileDto>>(responseString);

            travellers.Should().BeEquivalentTo(mockTravellers);
        }

        [Fact]
        public async Task GetTravellerByIdAsync_WithValidId_ReturnsTraveller()
        {
            var mockTraveller = new ProfileDto
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Email = "johndoe@example.com",
                Area = "Test area 1",
                Description = "Test description 1"
            };
            _factory.TravellerServiceMock.Setup(r => r.GetTravellerByIdAsync(1)).ReturnsAsync(mockTraveller);

            var response = await _client.GetAsync("/api/travellers/1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseString = await response.Content.ReadAsStringAsync();
            var traveller = JsonConvert.DeserializeObject<ProfileDto>(responseString);

            traveller.Should().BeEquivalentTo(mockTraveller);
        }

        [Fact]
        public async Task GetTravellerByIdAsync_WithInvalidId_ReturnsNotFound()
        {
            _factory.TravellerServiceMock.Setup(r => r.GetTravellerByIdAsync(1)).ReturnsAsync((ProfileDto?)null);

            var response = await _client.GetAsync("/api/travellers/1");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateTravellerAsync_ReturnsCreatedTraveller()
        {
            var mockTraveller = new ProfileDto
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Email = "johndoe@example.com",
                Description = "Test description",
                Area = "Test area",
                Image = new byte[] { 0x00, 0x01, 0x02 }
            };
            _factory.TravellerServiceMock.Setup(r => r.CreateTravellerAsync(mockTraveller)).ReturnsAsync(mockTraveller);
            var response = await _client.PostAsync("/api/travellers", JsonContent.Create(mockTraveller));
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseString = await response.Content.ReadAsStringAsync();
            var traveller = JsonConvert.DeserializeObject<ProfileDto>(responseString);
            traveller.Should().BeEquivalentTo(mockTraveller);
        }

        [Fact]
        public async Task UpdateTravellerAsync_WithValidToken_ReturnsNoContent()
        {
            var mockTraveller = new ProfileDto
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Email = "johndoe@example.com",
                Description = "Test description",
                Area = "Test area",
                Image = new byte[] { 0x00, 0x01, 0x02 }
            };

            var token =
                "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiSGFubmFSYXNpbGEiLCJJZCI6IjMiLCJleHAiOjE2ODkwOTUwMzJ9.pW-pdZpnuGSUPm56iF3a7niImyX4v2UU09An56V0UpZ8XQOXeGumM9fPr-kTyz3nLj9rLBgxEgdxatuQuOukAg";
            _factory.TravellerServiceMock.Setup(r => r.UpdateTravellerAsync(It.IsAny<uint>(), It.IsAny<ProfileDto>()))
                .ReturnsAsync(true);
            _factory.TravellerServiceMock.Setup(r => r.GetTravellerByIdAsync(It.IsAny<uint>())).ReturnsAsync(mockTraveller);

            var request = new HttpRequestMessage(HttpMethod.Put, "/api/Travellers/profile");
            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Content = JsonContent.Create(mockTraveller);


            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateTravellerAsync_WithoutToken_ReturnsNotAuthorized()
        {
            var mockTraveller = new ProfileDto
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Email = "johndoe@example.com",
                Description = "Test description",
                Area = "Test area",
                Image = new byte[] { 0x00, 0x01, 0x02 }
            };
            var response = await _client.PutAsync("/api/Travellers/profile", JsonContent.Create(mockTraveller));
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task DeleteTravellerAsync_WithValidId_ReturnsNoContent()
        {
            _factory.TravellerServiceMock.Setup(r => r.DeleteTravellerAsync(It.IsAny<uint>())).ReturnsAsync(true);
            var response = await _client.DeleteAsync("/api/travellers/1");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteTravellerAsync_WithInvalidId_ReturnsNotFound()
        {
            _factory.TravellerServiceMock.Setup(r => r.DeleteTravellerAsync(It.IsAny<uint>())).ReturnsAsync(false);
            var response = await _client.DeleteAsync("/api/travellers/6");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}