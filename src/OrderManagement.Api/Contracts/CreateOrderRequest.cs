namespace OrderManagement.Api.Contracts;

public sealed record CreateOrderRequest(string CustomerEmail, decimal TotalAmount);