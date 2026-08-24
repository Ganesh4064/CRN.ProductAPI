using AutoMapper;
using CRN.ProductAPI.Application.DTOs;
using CRN.ProductAPI.Application.Interfaces;
using CRN.ProductAPI.Application.Services;
using CRN.ProductAPI.Domain.Entities;
using Moq;

namespace CRN.ProductAPI.Tests;

public class ItemServiceTests
{
    private readonly Mock<IItemRepository> _itemRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ItemService _service;

    public ItemServiceTests()
    {
        _itemRepositoryMock = new Mock<IItemRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();

        _service = new ItemService(
            _itemRepositoryMock.Object,
            _productRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenItemExists_ReturnsItem()
    {
        // Arrange
        var item = new Item
        {
            Id = 1,
            ProductId = 2,
            Quantity = 5
        };

        var itemDto = new ItemDto
        {
            Id = 1,
            ProductId = 2,
            Quantity = 5
        };

        _itemRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(item);

        _mapperMock
            .Setup(x => x.Map<ItemDto>(item))
            .Returns(itemDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(2, result.ProductId);
        Assert.Equal(5, result.Quantity);
    }

    [Fact]
    public async Task GetByIdAsync_WhenItemDoesNotExist_ReturnsNull()
    {
        // Arrange
        _itemRepositoryMock
            .Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync((Item?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_WhenProductDoesNotExist_ReturnsNull()
    {
        // Arrange
        var dto = new CreateItemDto
        {
            ProductId = 99,
            Quantity = 5
        };

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.Null(result);

        _itemRepositoryMock.Verify(
            x => x.CreateAsync(It.IsAny<Item>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenProductExists_CreatesItem()
    {
        // Arrange
        var dto = new CreateItemDto
        {
            ProductId = 2,
            Quantity = 5
        };

        var product = new Product
        {
            Id = 2
        };

        var item = new Item
        {
            ProductId = 2,
            Quantity = 5
        };

        var createdItem = new Item
        {
            Id = 1,
            ProductId = 2,
            Quantity = 5
        };

        var itemDto = new ItemDto
        {
            Id = 1,
            ProductId = 2,
            Quantity = 5
        };

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(product);

        _mapperMock
            .Setup(x => x.Map<Item>(dto))
            .Returns(item);

        _itemRepositoryMock
            .Setup(x => x.CreateAsync(item))
            .ReturnsAsync(createdItem);

        _mapperMock
            .Setup(x => x.Map<ItemDto>(createdItem))
            .Returns(itemDto);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(2, result.ProductId);
        Assert.Equal(5, result.Quantity);

        _itemRepositoryMock.Verify(
            x => x.CreateAsync(item),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenItemDoesNotExist_ReturnsNull()
    {
        // Arrange
        var dto = new UpdateItemDto
        {
            Quantity = 10
        };

        _itemRepositoryMock
            .Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync((Item?)null);

        // Act
        var result = await _service.UpdateAsync(99, dto);

        // Assert
        Assert.Null(result);

        _itemRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<Item>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenItemExists_UpdatesQuantity()
    {
        // Arrange
        var existingItem = new Item
        {
            Id = 1,
            ProductId = 2,
            Quantity = 5
        };

        var dto = new UpdateItemDto
        {
            Quantity = 10
        };

        var updatedItem = new Item
        {
            Id = 1,
            ProductId = 2,
            Quantity = 10
        };

        var updatedDto = new ItemDto
        {
            Id = 1,
            ProductId = 2,
            Quantity = 10
        };

        _itemRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingItem);

        _itemRepositoryMock
            .Setup(x => x.UpdateAsync(existingItem))
            .ReturnsAsync(updatedItem);

        _mapperMock
            .Setup(x => x.Map<ItemDto>(updatedItem))
            .Returns(updatedDto);

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Quantity);

        Assert.Equal(10, existingItem.Quantity);

        _itemRepositoryMock.Verify(
            x => x.UpdateAsync(existingItem),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenRepositoryDeletesItem_ReturnsTrue()
    {
        // Arrange
        _itemRepositoryMock
            .Setup(x => x.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);

        _itemRepositoryMock.Verify(
            x => x.DeleteAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenRepositoryCannotDeleteItem_ReturnsFalse()
    {
        // Arrange
        _itemRepositoryMock
            .Setup(x => x.DeleteAsync(99))
            .ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(99);

        // Assert
        Assert.False(result);
    }
}