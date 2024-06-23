namespace DiscountManager.UnitTests
{
    using DiscountManager.Data.Repositories;
    using DiscountManager.Data.Documents;
    using DiscountManager.Services;
    using DiscountManagerController.Grpc;
    using Grpc.Core;
    using Moq;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class DiscountManagerControllerServiceTest
    {
        private readonly Mock<IDiscountCodeGeneratorService> mockGeneratorService;
        private readonly Mock<IDiscountCodeRepository> mockRepository;
        private readonly DiscountManagerControllerService service;

        public DiscountManagerControllerServiceTest()
        {
            this.mockGeneratorService = new Mock<IDiscountCodeGeneratorService>();
            this.mockRepository = new Mock<IDiscountCodeRepository>();
            this.service = new DiscountManagerControllerService(mockGeneratorService.Object);
        }

        [Fact]
        public async Task GenerateDiscountCodes_ShouldReturnGeneratedCodes()
        {
            // Arrange
            var codes = new List<DiscountCodeDocument>()
            {
                new DiscountCodeDocument() { Code = "CODE1234" },
                new DiscountCodeDocument() { Code = "CODE5678" }
            };

            this.mockGeneratorService
                .Setup(service => service.GenerateDiscountCodesAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(codes);

            var request = new GenerateDiscountCodesRequest { Count = 2 };
            var context = new Mock<ServerCallContext>();

            // Act
            var response = await this.service.GenerateDiscountCodes(request, context.Object);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(2, response.Codes.Count);
            Assert.Equal("CODE1234", response.Codes[0]);
            Assert.Equal("CODE5678", response.Codes[1]);
        }

        [Fact]
        public async Task GetAllDiscountCodes_ShouldReturnAllCodes()
        {
            // Arrange
            var codes = new List<DiscountCodeDocument>()
            {
                new DiscountCodeDocument() { Code = "CODE1234" },
                new DiscountCodeDocument() { Code = "CODE5678" }
            };

            this.mockGeneratorService
                .Setup(service => service.GetAllDiscountCodesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(codes);

            var request = new GetAllDiscountCodesRequest();
            var context = new Mock<ServerCallContext>();

            // Act
            var response = await this.service.GetAllDiscountCodes(request, context.Object);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(2, response.Codes.Count);
            Assert.Equal("CODE1234", response.Codes[0]);
            Assert.Equal("CODE5678", response.Codes[1]);
        }
    }
}
