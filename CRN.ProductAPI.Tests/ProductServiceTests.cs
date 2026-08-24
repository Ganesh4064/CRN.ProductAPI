using AutoMapper;
using CRN.ProductAPI.Application.DTOs;
using CRN.ProductAPI.Application.Interfaces;
using CRN.ProductAPI.Application.Services;
using CRN.ProductAPI.Domain.Entities;
using Moq;

namespace CRN.ProductAPI.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();

        _service = new ProductService(
            _repositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ReturnsProduct()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            ProductName = "Gaming Laptop",
            CreatedBy = "ganesh",
            CreatedOn = DateTime.UtcNow
        };

        var productDto = new ProductDto
        {
            Id = 1,
            ProductName = "Gaming Laptop",
            CreatedBy = "ganesh"
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(product);

        _mapperMock
            .Setup(x => x.Map<ProductDto>(product))
            .Returns(productDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Gaming Laptop", result.ProductName);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductDoesNotExist_ReturnsNull()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_CreatesProductSuccessfully()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            ProductName = "Gaming Laptop",
            CreatedBy = "ganesh"
        };

        var product = new Product
        {
            ProductName = "Gaming Laptop",
            CreatedBy = "ganesh"
        };

        var createdProduct = new Product
        {
            Id = 1,
            ProductName = "Gaming Laptop",
            CreatedBy = "ganesh",
            CreatedOn = DateTime.UtcNow
        };

        var productDto = new ProductDto
        {
            Id = 1,
            ProductName = "Gaming Laptop",
            CreatedBy = "ganesh"
        };

        _mapperMock
            .Setup(x => x.Map<Product>(dto))
            .Returns(product);

        _repositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync(createdProduct);

        _mapperMock
            .Setup(x => x.Map<ProductDto>(createdProduct))
            .Returns(productDto);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Gaming Laptop", result.ProductName);

        _repositoryMock.Verify(
            x => x.CreateAsync(It.IsAny<Product>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductExists_ReturnsUpdatedProduct()
    {
        // Arrange
        var existingProduct = new Product
        {
            Id = 1,
            ProductName = "Old Laptop",
            CreatedBy = "ganesh",
            CreatedOn = DateTime.UtcNow
        };

        var dto = new UpdateProductDto
        {
            ProductName = "Updated Laptop"
        };

        var updatedProduct = new Product
        {
            Id = 1,
            ProductName = "Updated Laptop",
            CreatedBy = "ganesh",
            CreatedOn = existingProduct.CreatedOn,
            ModifiedOn = DateTime.UtcNow
        };

        var updatedDto = new ProductDto
        {
            Id = 1,
            ProductName = "Updated Laptop",
            CreatedBy = "ganesh"
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingProduct);

        _mapperMock
            .Setup(x => x.Map(dto, existingProduct))
            .Callback(() =>
            {
                existingProduct.ProductName = dto.ProductName;
            });

        _repositoryMock
            .Setup(x => x.UpdateAsync(existingProduct))
            .ReturnsAsync(updatedProduct);

        _mapperMock
            .Setup(x => x.Map<ProductDto>(updatedProduct))
            .Returns(updatedDto);

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Laptop", result.ProductName);

        _repositoryMock.Verify(
            x => x.UpdateAsync(existingProduct),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductDoesNotExist_ReturnsNull()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            ProductName = "Updated Laptop"
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.UpdateAsync(99, dto);

        // Assert
        Assert.Null(result);

        _repositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<Product>()),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenRepositoryDeletesProduct_ReturnsTrue()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);

        _repositoryMock.Verify(
            x => x.DeleteAsync(1),
            Times.Once);
    }
}