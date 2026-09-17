namespace OrderManagement.Application.Orders;

public sealed record CreateOrderCommand(string CustomerEmail, decimal TotalAmount);

public sealed record CreateOrderResult(Guid OrderId);