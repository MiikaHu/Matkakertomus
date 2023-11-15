using FluentAssertions;
using Matkakertomus.Shared;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Matkakertomus.Server.Test.ControllerTests
{
    public class DestinationsControllerTests : IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public DestinationsControllerTests()
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
        public async Task GetAllDestinationsAsync_ReturnsAllDestinations()
        {
            var mockDestinations = new DestinationDto[]
            {
                new() {Area="Test area 1", Country = "Test country 1 ", Description = "Test description 1", DestinationId = 1, DestinationName = "Test name 1"},
                new() {Area="Test area 2", Country = "Test country 2", Description = "Test description 2", DestinationId = 2, DestinationName = "Test name 2"}
            }.AsQueryable();

            _factory.DestinationServiceMock.Setup(r => r.GetAllDestinationsAsync()).ReturnsAsync(mockDestinations);

            var response = await _client.GetAsync("/api/destinations");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseString = await response.Content.ReadAsStringAsync();
            var destinations = JsonConvert.DeserializeObject<IEnumerable<DestinationDto>>(responseString);

            destinations.Should().BeEquivalentTo(mockDestinations);
        }

        [Fact]
        public async Task GetDestinationByIdAsync_WithValidId_ReturnsDestination()
        {
            var mockDestination = new DestinationDto { Area = "Test area 1", Country = "Test country 1 ", Description = "Test description 1", DestinationId = 1, DestinationName = "Test name 1" };
            _factory.DestinationServiceMock.Setup(r => r.GetDestinationByIdAsync(1)).ReturnsAsync(mockDestination);
            var response = await _client.GetAsync("/api/destinations/1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();
            var destination = JsonConvert.DeserializeObject<DestinationDto>(responseString);
            destination.Should().BeEquivalentTo(mockDestination);
        }

        [Fact]
        public async Task GetDestinationByIdAsync_WithInvalidId_ReturnsNotFound()
        {
            _factory.DestinationServiceMock.Setup(r => r.GetDestinationByIdAsync(1)).ReturnsAsync((DestinationDto?)null);
            var response = await _client.GetAsync("/api/destinations/1");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateDestinationAsync_ReturnsCreatedDestination()
        {
            var mockDestination = new DestinationDto { Area = "Test area 1", Country = "Test country 1 ", Description = "Test description 1", DestinationId = 1, DestinationName = "Test name 1" };
            _factory.DestinationServiceMock.Setup(r => r.CreateDestinationAsync(mockDestination)).ReturnsAsync(mockDestination);
            var response = await _client.PostAsync("/api/destinations", JsonContent.Create(mockDestination));
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseString = await response.Content.ReadAsStringAsync();
            var destination = JsonConvert.DeserializeObject<DestinationDto>(responseString);
            destination.Should().BeEquivalentTo(mockDestination);
        }

        [Fact]
        public async Task UpdateDestinationAsync_ReturnsNoContent()
        {
            var mockDestination = new DestinationDto { Area = "Test area 1", Country = "Test country 1 ", Description = "Test description 1", DestinationId = 1, DestinationName = "Test name 1" };

            _factory.DestinationServiceMock.Setup(r => r.UpdateDestinationAsync(It.IsAny<uint>(), It.IsAny<DestinationDto>())).ReturnsAsync(true);

            var response = await _client.PutAsync("/api/destinations/1", JsonContent.Create(mockDestination));
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateDestinationAsync_WithInvalidId_ReturnsNotFound()
        {
            var mockDestination = new DestinationDto { Area = "Test area 1", Country = "Test country 1 ", Description = "Test description 1", DestinationId = 1, DestinationName = "Test name 1" };
            _factory.DestinationServiceMock.Setup(r => r.UpdateDestinationAsync(It.IsAny<uint>(), It.IsAny<DestinationDto>())).ReturnsAsync(false);

            var response = await _client.PutAsync("/api/destinations/6", JsonContent.Create(mockDestination));
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteDestinationAsync_WithValidId_ReturnsNoContent()
        {
            _factory.DestinationServiceMock.Setup(r => r.DeleteDestinationAsync(It.IsAny<uint>())).ReturnsAsync(true);
            var response = await _client.DeleteAsync("/api/destinations/1");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteDestinationAsync_WithInvalidId_ReturnsNotFound()
        {
            //TODO: Check if Destination contains Trips
            _factory.DestinationServiceMock.Setup(r => r.DeleteDestinationAsync(It.IsAny<uint>())).ReturnsAsync(false);
            var response = await _client.DeleteAsync("/api/destinations/6");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
