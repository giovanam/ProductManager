using Moq;
using ProductManager.Application.DTOs;
using ProductManager.Application.Services;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using MassTransit;
using Xunit;
using ProductManager.Contracts.Events;

namespace ProductManager.Tests.ApplicationTests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly Mock<ILogger<ProductService>> _loggerMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _loggerMock = new Mock<ILogger<ProductService>>();

            _productService = new ProductService(
                _productRepositoryMock.Object,
                _publishEndpointMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task AddProductAsync_ShouldCallRepositoryAdd()
        {
            var productDTO = new ProductDTO { Nome = "Produto 1", Preco = 10.0m, QuantidadeEstoque = 5 };

            await _productService.AddProductAsync(productDTO);

            _productRepositoryMock.Verify(r => r.AddProductAsync(It.IsAny<Product>()), Times.Once, "O método AddProductAsync do repositório não foi chamado corretamente.");

            _loggerMock.Verify(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce, "Log de informação não foi chamado.");
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldCallRepositoryUpdate()
        {
            var productDTO = new ProductDTO { Nome = "Produto 1", Preco = 10.0m, QuantidadeEstoque = 5 };

            await _productService.UpdateProductAsync(productDTO);

            _productRepositoryMock.Verify(r => r.UpdateProductAsync(It.IsAny<Product>()), Times.Once, "O método UpdateProductAsync do repositório não foi chamado corretamente.");
            _loggerMock.Verify(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce, "Log de informação não foi chamado.");
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldCallRepositoryDelete()
        {
            int id = 1;

            await _productService.DeleteProductAsync(id);

            _productRepositoryMock.Verify(r => r.DeleteProductAsync(id), Times.Once, "O método DeleteProductAsync do repositório não foi chamado corretamente.");
            _loggerMock.Verify(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce, "Log de informação não foi chamado.");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldCallRepositoryGetById()
        {
            int id = 1;

            await _productService.GetByIdAsync(id);

            _productRepositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once, "O método GetByIdAsync do repositório não foi chamado corretamente.");
            _loggerMock.Verify(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce, "Log de informação não foi chamado.");
        }

        [Fact]
        public async Task AddProductAsync_ShouldPublishProductCreatedEvent()
        {
            var productDTO = new ProductDTO { Nome = "Produto 1", Preco = 10.0m, QuantidadeEstoque = 5 };

            await _productService.AddProductAsync(productDTO);

            _publishEndpointMock.Verify(p => p.Publish(It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once, "O evento ProductCreatedEvent não foi publicado.");
        }
    }
}
