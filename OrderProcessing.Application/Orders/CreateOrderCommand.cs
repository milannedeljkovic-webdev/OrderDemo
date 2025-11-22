using MediatR;

namespace OrderProcessing.Application.Orders;

// Command pattern - samo podaci
public record CreateOrderCommand(int ProductId, int Quantity) : IRequest<int>;