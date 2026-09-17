using OrderManagement.Application.Ports;

namespace OrderManagement.Application.Orders.Handlers;

public sealed class GetOrderHandler(IOrderRepository repository)
{
    public async Task<GetOrderResult?> HandleAsync(
        GetOrderQuery query,
        CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(query.OrderId, cancellationToken);

        return order is null
            ? null
            : new GetOrderResult(order.Id, order.CustomerEmail, order.TotalAmount);
    }
}