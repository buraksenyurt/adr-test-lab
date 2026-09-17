using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Ports;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);

    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);
}