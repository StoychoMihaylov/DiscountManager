namespace DiscountManager.UnitTests
{
    using DiscountManager.Data.Documents;
    using DiscountManager.Data.Repositories;
    using DiscountManager.Models.Options;
    using DiscountManager.Services;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class DiscountCodeGeneratorServiceTest
    {
        private readonly Mock<IDiscountCodeRepository> mockRepository;
        private readonly Mock<IOptions<ServiceSettings>> mockOptions;
        private readonly ServiceSettings serviceSettings;
        private readonly DiscountCodeGeneratorService service;

        public DiscountCodeGeneratorServiceTest()
        {
            this.mockRepository = new Mock<IDiscountCodeRepository>();
            this.mockOptions = new Mock<IOptions<ServiceSettings>>();
            this.serviceSettings = new ServiceSettings { MaxDiscountCodesPerRequest = 100 };
            this.mockOptions.Setup(o => o.Value).Returns(serviceSettings);
            this.service = new DiscountCodeGeneratorService(mockRepository.Object, mockOptions.Object);
        }

        [Fact]
        public async Task GenerateDiscountCodesAsync_ShouldGenerateUniqueCodes()
        {
            // Arrange
            var count = 5;
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await service.GenerateDiscountCodesAsync(count, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(count, result.Count);
            Assert.Equal(result.Count, result.Select(c => c.Code).Distinct().Count());
            mockRepository.Verify(r => r.InserNewDiscountCodesAsync(It.IsAny<List<DiscountCodeDocument>>(), cancellationToken), Times.Once);
        }

        [Fact]
        public async Task GenerateDiscountCodesAsync_ShouldThrowException_WhenCountIsInvalid()
        {
            // Arrange
            var invalidCount = 101; // greater than MaxDiscountCodesPerRequest
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.GenerateDiscountCodesAsync(invalidCount, cancellationToken));
            Assert.Equal($"Count must be between 1 and {serviceSettings.MaxDiscountCodesPerRequest}.", exception.Message);
        }

        [Fact]
        public async Task GetAllDiscountCodesAsync_ShouldReturnAllCodes()
        {
            // Arrange
            var codes = new List<DiscountCodeDocument>
            {
                new DiscountCodeDocument { Code = "CODE1234" },
                new DiscountCodeDocument { Code = "CODE5678" }
            };
            var cancellationToken = CancellationToken.None;
            mockRepository.Setup(r => r.GetAllDiscountCodesAsync(cancellationToken)).ReturnsAsync(codes);

            // Act
            var result = await service.GetAllDiscountCodesAsync(cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(codes.Count, result.Count);
            mockRepository.Verify(r => r.GetAllDiscountCodesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task InsertNewDiscountCodesAsync_ShouldThrowException_WhenCodesAreNull()
        {
            // Arrange
            List<DiscountCodeDocument> codes = null;
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.InsertNewDiscountCodesAsync(codes, cancellationToken));
            Assert.Equal("No discount codes provided to insert.", exception.Message);
        }

        [Fact]
        public async Task InsertNewDiscountCodesAsync_ShouldThrowException_WhenCodesAreEmpty()
        {
            // Arrange
            var codes = new List<DiscountCodeDocument>();
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.InsertNewDiscountCodesAsync(codes, cancellationToken));
            Assert.Equal("No discount codes provided to insert.", exception.Message);
        }

        [Fact]
        public async Task InsertNewDiscountCodesAsync_ShouldThrowException_WhenCodesExceedMaxLimit()
        {
            // Arrange
            var codes = new List<DiscountCodeDocument>(new DiscountCodeDocument[serviceSettings.MaxDiscountCodesPerRequest + 1]);
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.InsertNewDiscountCodesAsync(codes, cancellationToken));
            Assert.Equal($"Cannot insert more than {serviceSettings.MaxDiscountCodesPerRequest} discount codes in a single request.", exception.Message);
        }

        [Fact]
        public async Task InsertNewDiscountCodesAsync_ShouldCallRepository_WhenValidCodesProvided()
        {
            // Arrange
            var codes = new List<DiscountCodeDocument>
            {
                new DiscountCodeDocument { Code = "CODE1234" },
                new DiscountCodeDocument { Code = "CODE5678" }
            };
            var cancellationToken = CancellationToken.None;

            // Act
            await service.InsertNewDiscountCodesAsync(codes, cancellationToken);

            // Assert
            mockRepository.Verify(r => r.InserNewDiscountCodesAsync(codes, cancellationToken), Times.Once);
        }
    }
}
