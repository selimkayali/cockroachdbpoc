using CockroachDb.Data;
using CockroachDb.Models;
using CockroachDbPoc.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CockroachDbPoc.Tests.Unit;

public class ProductServiceTests
{
    private readonly Mock<DbSet<Product>> _mockProductDbSet;
    private readonly Mock<DatabaseContext> _mockContext;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        // Create mock DbSet
        _mockProductDbSet = new Mock<DbSet<Product>>();

        // Create mock context with DbContextOptions parameter
        var options = new DbContextOptionsBuilder<DatabaseContext>().Options;
        _mockContext = new Mock<DatabaseContext>(options);

        // Setup Products property
        _mockContext.Setup(c => c.Products).Returns(_mockProductDbSet.Object);

        _service = new ProductService(_mockContext.Object);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var expectedProduct = new Product { Id = 1, Name = "Test Product" };
        _mockProductDbSet.Setup(db => db.FindAsync(It.IsAny<long>())).ReturnsAsync(expectedProduct);

        // Act
        var result = await _service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.Id, result.Id);
        Assert.Equal(expectedProduct.Name, result.Name);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        _mockContext.Setup(repo => repo.Products.FindAsync(1))
            .ReturnsAsync((Product)null);

        // Act
        var result = await _service.GetById(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnNewProduct()
    {
        // Arrange
        var newProduct = new Product { Name = "New Product", Price = 10.99m };
        var createdProduct = new Product { Id = 1, Name = "New Product", Price = 10.99m };
        var productEntry = _mockContext.Object.Products.Add(createdProduct);

        _mockContext.Setup(repo => repo.Products.AddAsync(newProduct, CancellationToken.None))
            .ReturnsAsync(productEntry);

        // Act
        await _service.Create(newProduct);

        // Assert
        Assert.NotNull(createdProduct);
        Assert.Equal(1, createdProduct.Id);
        Assert.Equal(newProduct.Name, createdProduct.Name);
    }
}