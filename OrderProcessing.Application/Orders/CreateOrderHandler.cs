using MediatR;
using OrderProcessing.Application;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Application.Orders;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateOrderHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);

        if (product == null) throw new Exception("Proizvod nije pronadjen.");

        // Domain Logic - Ovde se dešava "magic"
        product.DebitStock(request.Quantity);

        var order = new Order(product, request.Quantity);
        _context.Orders.Add(order);

        await _context.SaveChangesAsync(cancellationToken); // Ovde puca Concurrency ako ima konflikta

        return order.Id;
    }
}