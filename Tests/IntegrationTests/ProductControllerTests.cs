using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManager.Application.DTOs;
using ProductManager.Domain.Interfaces;
using ProductManager.Presentation.Controllers;
using Xunit;

namespace ProductManager.Tests.IntegrationTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductController(_productServiceMock.Object, null);
        }

        [Fact]
        public async Task AddProduct_ShouldReturnOk()
        {
            var productDTO = new ProductDTO { Nome = "Produto 1", Preco = 10, QuantidadeEstoque = 5 };
            var result = await _controller.AddProductAsync(productDTO);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnOk()
        {
            var productDTO = new ProductDTO
            {
                Id = 2,
                Nome = "Produto Atualizado",
                Preco = 15,
                QuantidadeEstoque = 10
            };
            var result = await _controller.UpdateProductAsync(productDTO);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnOk()
        {
            int id = 1;
            var result = await _controller.DeleteProductAsync(id);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
