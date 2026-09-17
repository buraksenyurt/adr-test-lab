namespace OrderManagement.Application.Orders;

public sealed record GetOrderQuery(Guid OrderId);

public sealed record GetOrderResult(
    Guid OrderId,
    string CustomerEmail,
    decimal TotalAmount);