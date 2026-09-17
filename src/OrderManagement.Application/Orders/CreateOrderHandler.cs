using OrderManagement.Application.Ports;
using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Orders.Handlers;

public sealed class CreateOrderHandler(IOrderRepository repository)
{
    public async Task<CreateOrderResult> HandleAsync(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var order = Order.Create(command.CustomerEmail, command.TotalAmount);
        await repository.AddAsync(order, cancellationToken);

        return new CreateOrderResult(order.Id);
    }
}