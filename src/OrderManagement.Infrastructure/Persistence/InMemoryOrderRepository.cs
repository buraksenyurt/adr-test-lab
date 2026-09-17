using System.Collections.Concurrent;
using OrderManagement.Application.Ports;
using OrderManagement.Domain.Orders;

namespace OrderManagement.Infrastructure.Persistence;

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!_orders.TryAdd(order.Id, order))
        {
            throw new InvalidOperationException($"Order '{order.Id}' already exists.");
        }

        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _orders.TryGetValue(orderId, out var order);

        return Task.FromResult(order);
    }
}