using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Orders;
using OrderProcessing.Infrastructure.Persistence;
using OrderProcessing.Domain.Entities;
using System.Threading;

namespace OrderProcessing.Tests;

public class CreateOrderHandlerTests
{
    [Xunit.Fact]
    public async System.Threading.Tasks.Task Handle_CreatesOrderAndDebitsStock()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        // Arrange - add a product
        var product = new Product(100, "Test Product", 50, 12.5m);
        context.Products.Add(product);
        // InMemory provider doesn't auto-generate RowVersion; set a dummy value so EF doesn't complain.
        context.Entry(product).Property("RowVersion").CurrentValue = new byte[] { 1 };
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new CreateOrderHandler(context);
        var command = new CreateOrderCommand(product.Id, 5);

        // Act
        var orderId = await handler.Handle(command, CancellationToken.None);

        // Assert
        Xunit.Assert.True(orderId > 0);
        var dbProduct = await context.Products.FindAsync(new object[] { product.Id }, CancellationToken.None);
        Xunit.Assert.Equal(45, dbProduct!.StockQuantity);
    }
}
